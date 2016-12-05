using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;
using System.Globalization;

namespace FCT.LLC.BusinessService.DataAccess.Implementations
{
    public  class DealHistoryRepository:Repository<tblDealHistory>, IDealHistoryRepository
    {
        private readonly EFBusinessContext _context;
        private readonly DealHistoryHelper _dealHistoryHelper;
        private readonly ILawyerRepository _lawyerRepository;

        public DealHistoryRepository(EFBusinessContext context, DealHistoryHelper dealHistoryHelper,
            ILawyerRepository lawyerRepository) : base(context)
        {
            _context = context;
            _dealHistoryHelper = dealHistoryHelper;
            _lawyerRepository = lawyerRepository;

        }

        public void CreateDealHistory(int dealId, UserContext user, string activity, string otherLawyerName = null)
        {
            string lawyerName;
            if (string.IsNullOrEmpty(otherLawyerName))
            {
                if (IsLawyerOrClerkOrAssistant(user))
                {
                    var profile = _lawyerRepository.GetUserDetails(user.UserID);
                    lawyerName = string.Format("{0} {1}", profile.FirstName, profile.LastName);
                }
                else
                {
                    lawyerName = "FCT Administrator";
                }
            }

            else
            {
                lawyerName = otherLawyerName;
            }
            var entry = _dealHistoryHelper.GetDealHistoryEntry(activity, lawyerName);
            CreateDealHistoryByDealHistoryEntry(dealId, entry, user, false);
        }

        public void CreateDealHistory(int dealID, string activity,UserContext user ,decimal? amount )
        {
            var entry = _dealHistoryHelper.GetDealHistoryEntry(activity, amount.GetValueOrDefault());
            CreateDealHistoryByDealHistoryEntry(dealID, entry, user, false);
        }

        public void CreateDealHistory(int dealID, string activity, UserContext user, DisbursementHistory disbursementHistory)
        {
            var entry = _dealHistoryHelper.GetDealHistoryEntry(activity, disbursementHistory);
            CreateDealHistoryByDealHistoryEntry(dealID, entry, user, false);
        }


        public void CreateDealHistoryByStatus(int dealID, string fromStatus, string toStatus, string historyFor,
            UserContext user)
        {
            bool IsVisibleToLender = false;
            string resourceKey;
            switch (toStatus)
            {
                case "CANCELLED":
                    resourceKey = "dealCancelled";
                    break;
                case "PENDING ACCEPTANCE":
                    resourceKey = "dealPendingAcceptance";
                    break;
                case "ACTIVE":
                    resourceKey = GetResourceKeyWhenToStatusIsActive(fromStatus);
                    break;
                case "COMPLETED":
                    resourceKey = historyFor == LawyerActingFor.Vendor
                        ? DealActivity.VendorDealCompleted
                        : DealActivity.DealCompleted;
                    break;
                default:
                    resourceKey = toStatus;
                    break;
            }

            var entry = _dealHistoryHelper.GetDealHistoryEntry(resourceKey, "");

            if (!IsLawyerOrClerkOrAssistant(user))
            {
                user.UserID =  "FCT Administrator";
                user.UserType = UserType.SYSTEM ;
                if (resourceKey.Equals("dealUndoCancel", StringComparison.CurrentCultureIgnoreCase) 
                    || resourceKey.Equals("dealUndoCancelReq", StringComparison.CurrentCultureIgnoreCase)
                    || resourceKey.Equals("dealCancelled", StringComparison.CurrentCultureIgnoreCase) 
                    || resourceKey.Equals("dealCompleted", StringComparison.CurrentCultureIgnoreCase) 
                    || resourceKey.Equals("dealActive", StringComparison.CurrentCultureIgnoreCase))
                {
                    IsVisibleToLender = true;
                }
            }

            CreateDealHistoryByDealHistoryEntry(dealID, entry, user, IsVisibleToLender);
        }

        private string GetResourceKeyWhenToStatusIsActive(string fromStatus)
        {
            string resourceKey = "";
            switch (fromStatus) {
                case "CANCELLED":
                    resourceKey = "dealUndoCancel";
                    break;
                case "CANCELLATION REQUESTED":
                    resourceKey = "dealUndoCancelReq";
                    break;
                default:
                    resourceKey = "dealActive";
                    break;
            }
            return resourceKey;
        }

        public void CreateDealHistory(int dealID, string activity)
        {
            string resourceKey;
            resourceKey = activity;

            var entry = _dealHistoryHelper.GetDealHistoryEntry(resourceKey, "");
            UserContext user = new UserContext()
            {
                UserID = UserType.SYSTEM,
                UserType = UserType.SYSTEM
            };

            CreateDealHistoryByDealHistoryEntry(dealID, entry, user, false);

        }
        public void CreateDealChangeHistories(IEnumerable<UserHistory> userHistories, UserContext user)
        {
                var dealhistories = _dealHistoryHelper.GetDealHistoryMessages(userHistories);
                var list = dealhistories.Select(dealhistory => new tblDealHistory()
                {
                    DealID = dealhistory.DealId,
                    Activity = dealhistory.EnglishVersion,
                    ActivityFrench = dealhistory.FrenchVersion,
                    LogDate = DateTime.Now,
                    IsShowOnLender = false,
                    UserID = user.UserID,
                    UserType = user.UserType
                }).ToList();
                InsertRange(list);
                _context.SaveChanges();
           
        }
        public void CreateDealHistory(int dealID, string historyMessage, UserContext user, string addedOrRemovedValue)
        {
            var entry = _dealHistoryHelper.GetDealHistoryEntry(historyMessage, addedOrRemovedValue, ResourceSet.LawyerPortalMessage);
            CreateDealHistoryByDealHistoryEntry(dealID, entry, user, false);
        }

        public void CreateLLCDealHistory(int dealID, string activity, UserContext user, string placeHolderValue, int statusReasonId=0)
        {
            var entry = _dealHistoryHelper.GetDealHistoryEntry(activity, placeHolderValue, ResourceSet.FCTLLCBusinessService, statusReasonId);
            CreateDealHistoryByDealHistoryEntry(dealID, entry, user, true);
        }
        public void CreateDealHistories(int dealID, string resourceSet, IEnumerable<IDictionary<string, Variance>> dataChanges,
            UserContext user)
        {
            var entries = _dealHistoryHelper.GetDealHistoryEntries(resourceSet, dataChanges);
            if (dealID > 0)
            {
                var histories = entries.Select(entry => new tblDealHistory()
                {
                    DealID = dealID,
                    Activity = entry.EnglishVersion,
                    ActivityFrench = entry.FrenchVersion,
                    LogDate = DateTime.Now,
                    IsShowOnLender = false,
                    UserID = user.UserID,
                    UserType = user.UserType
                }).ToList();
                InsertRange(histories);
                _context.SaveChanges();
            }

        }

        public void SyncDealHistories(int sourceDealId, int targetDealId)
        {
            var dealHistories=GetAll().Where(d => d.DealID == sourceDealId && d.UserType.ToUpper()!=UserType.LENDER).ToList();
            foreach (var tblDealHistory in dealHistories)
            {
                tblDealHistory.DealID = targetDealId;
            }
            InsertRange(dealHistories);
        }

        public void CreateDealHistories(IEnumerable<UserHistory> userHistories, UserContext user)
        {
            var dealhistories = _dealHistoryHelper.GetDealHistoryEntries(userHistories);
            var list = dealhistories.Select(dealhistory => new tblDealHistory()
            {
                DealID = dealhistory.DealId,
                Activity = dealhistory.EnglishVersion,
                ActivityFrench = dealhistory.FrenchVersion,
                LogDate = DateTime.Now,
                IsShowOnLender = false,
                UserID = user.UserID,
                UserType = user.UserType
            }).ToList();
            InsertRange(list);
            _context.SaveChanges();
        }

        public void CreateDealHistory(int dealID, string entryKey, UserContext user)
        {
            DealHistoryEntry entry = null;
            entry = _dealHistoryHelper.GetDealHistoryMessage(entryKey);

            CreateDealHistoryByDealHistoryEntry(dealID, entry, user, false);
        }

        public void CreateCancelDealHistory(int dealID, string entryKey, UserContext user)
        {
            DealHistoryEntry entry = null;
            entry = _dealHistoryHelper.GetDealHistoryMessageDealCancelledAdmin(entryKey);
            CreateDealHistoryByDealHistoryEntry(dealID, entry, user, false);

        }

        private bool IsLawyerOrClerkOrAssistant(UserContext user)
        {
            bool IsLawyerOrClerkOrAssistant = user.UserType.Equals("LAWYER", StringComparison.CurrentCultureIgnoreCase) 
                || user.UserType.Equals( "CLERK", StringComparison.CurrentCultureIgnoreCase) 
                || user.UserType.Equals( "ASSISTANT", StringComparison.CurrentCultureIgnoreCase);

            return IsLawyerOrClerkOrAssistant;
        }

        public DealHistoryEntry GetDealHistoryEntry(string resourceSet, string resourceKey)
        {
            DealHistoryEntry entry = _dealHistoryHelper.GetDealHistoryEntryDirectly(resourceSet, resourceKey);
            return entry;
        }

        //MAIN
        public void CreateDealHistoryByDealHistoryEntry(int dealID, DealHistoryEntry entry, UserContext user, bool showOnLender)
        {
            try
            {
                var entity = new tblDealHistory()
                {
                    DealID = dealID,
                    Activity = entry.EnglishVersion,
                    ActivityFrench = entry.FrenchVersion,
                    LogDate = DateTime.Now,
                    IsShowOnLender = showOnLender,
                    UserID = string.IsNullOrEmpty(user.UserID) == false && IsLawyerOrClerkOrAssistant(user) ? user.UserID : "SYSTEM",
                    UserType = user.UserType == "" ? UserType.FCTAdmin : user.UserType
                };
                Insert(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                TraceExceptionInDebugMode(dbEx);
                throw new DataAccessException(){BaseException = dbEx};
            }
           
        }

        [ConditionalAttribute("DEBUG")]
        private static void TraceExceptionInDebugMode(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                        validationError.ErrorMessage);
                }
            }
        }
       
    }
}
