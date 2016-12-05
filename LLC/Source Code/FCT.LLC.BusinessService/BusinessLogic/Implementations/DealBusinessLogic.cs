using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.Services.AuditLog.DataContracts;
using FCT.Services.AuditLog.MessageContracts;
using FCT.Services.AuditLog.ServiceContracts;
//using FCT.LLC.Portal.DTOs.Requests;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class DealBusinessLogic : IDealBusinessLogic
    {
        private readonly IDealRepository _dealRepository;
        private readonly IFundedRepository _fundedRepository;
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly IEntityMapper<tblDeal, Deal> _dealMapper;
        private readonly IAuditLogService _auditLogService;
        private readonly IGlobalizationRepository _globalizationRepository;
        private readonly IFundingDealRepository _fundingDealRepository;

        public DealBusinessLogic(IDealRepository dealRepository, IEntityMapper<tblDeal, Deal> dealMapper, IDealHistoryRepository dealHistoryRepository, IFundedRepository fundedRepository, IAuditLogService auditLogService, IGlobalizationRepository globalizationRepository, IFundingDealRepository fundingDealRepository)
        {
            _dealRepository = dealRepository;
            _dealMapper = dealMapper;
            _dealHistoryRepository = dealHistoryRepository;
            _fundedRepository = fundedRepository;
            _auditLogService = auditLogService;
            _globalizationRepository = globalizationRepository;
            _fundingDealRepository = fundingDealRepository;
        }


        public GetDealResponse GetDeal(GetDealRequest request)
        {
            

            GetDealResponse response = new GetDealResponse();
            tblDeal _deal = _dealRepository.GetDealDetails(request.DealID, request.UserContext);
            tblBranchContact branchContact = null;
            if (_deal.MortgageCentreID.HasValue)
                if (_deal.MtgCentreContactID.HasValue)
                    branchContact = _dealRepository.GetBranchContact(_deal.MtgCentreContactID.Value);            
            
            response.Deal = _dealMapper.MapToData(_deal);
            if (branchContact != null)
            {
                Lender lender = new Lender();
                lender.Fax = branchContact.Fax ?? "";
                lender.Phone = branchContact.Phone ?? "";
                
                response.Deal.Lender.Fax = lender.Fax;
                response.Deal.Lender.Phone = lender.Phone;
                response.Deal.Lender.ContactName = branchContact.FirstName == null ? "" : branchContact.FirstName + " ";
                response.Deal.Lender.ContactName += branchContact.LastName ?? "";
                
            }

            
            // Set Lender Name based on Lender Financial Institution
            // For MMS we need to show actual lendor name instead of FCT 
            tblFinancialInstitutionNumber fiDetails = _dealRepository.GetFinancialInstitutionDetails(_deal.LenderFINumber);
            if(fiDetails != null)
            {
                response.Deal.Lender.LenderName = fiDetails.FINameEnglish;
            }

            return response;
        }

        public tblDeal GetTbDeal(GetDealRequest request)
        {
            return _dealRepository.GetDealDetails(request.DealID, request.UserContext);
        }

        public tblDeal GetTbDealByFCTURN(string FCTURN)
        {
            return _dealRepository.GetDealDetailsByFCTURN(FCTURN);
        }

        public void UpdateDealStatus(UpdateDealStatusRequest request)
        {
            int otherDealId = 0;
            tblDeal deal = _dealRepository.GetDealDetails(request.DealID, request.UserContext);
            string userID = (request.UserContext == null || request.UserContext.UserID == null) ? "" : request.UserContext.UserID;

            if (deal.Status!=request.DealStatus && 
                    (deal.BusinessModel == BusinessModel.LLCCOMBO || deal.BusinessModel == BusinessModel.MMSCOMBO))
            {
                otherDealId=_fundingDealRepository.GetOtherDealInScope(request.DealID);
            }
            using (var scope=TransactionScopeBuilder.CreateReadCommitted())
            {
                bool suppressDealHistory = deal.BusinessModel.Contains("MMS") && request.DealStatus == DealStatus.Cancelled;
                _dealRepository.UpdateDealStatus(request.DealID, request.DealStatus, request.UserContext);
                string historyFor;
                if (otherDealId > 0)
                {
                    historyFor=_fundingDealRepository.GetLawyerActingFor(otherDealId);
                    _dealRepository.UpdateDealStatus(otherDealId, request.DealStatus, request.UserContext);

                    if (!suppressDealHistory)
                    {
                        _dealHistoryRepository.CreateDealHistoryByStatus(otherDealId, deal.Status, request.DealStatus, historyFor, request.UserContext);
                    }
                }
                historyFor = _fundingDealRepository.GetLawyerActingFor(request.DealID);
                if (!suppressDealHistory)
                {
                    _dealHistoryRepository.CreateDealHistoryByStatus(request.DealID, deal.Status, request.DealStatus, historyFor, request.UserContext);
                }

                scope.Complete();
            }

            UpdateAuditLog(deal, request.DealStatus, userID, request);
           
        }

        private void UpdateAuditLog(tblDeal deal, string toDealStatus, string UserID, UpdateDealStatusRequest request)
        {

            string resourceKey = string.Empty;
            string strStatusUpdateEng = string.Empty;

            string userContextType = (request.UserContext == null || request.UserContext.UserType == null)
                ? ""
                : request.UserContext.UserType;

            switch (toDealStatus)
            {
                case "PENDING ACCEPTANCE":
                    resourceKey = "dealPendingAcceptance";
                    break;
                case "ACTIVE":
                    resourceKey = GetStatus(deal.Status,toDealStatus);
                    break;
                case "COMPLETED":
                    resourceKey = "dealCompleted";
                    break;
                case "CANCELLED":
                    resourceKey = "dealCancelled";
                    break;
            }

            DealHistoryEntry _dealHistoryEntry = _globalizationRepository.GetEntry(ResourceSet.FCTLLCBusinessService, resourceKey);
            if (_dealHistoryEntry != null)
            {
                strStatusUpdateEng = _dealHistoryEntry.EnglishVersion;
            }

            string message = string.Format("{0} FCT Reference Number: {1}; Lender reference Number: {2}",
                strStatusUpdateEng, deal.FCTRefNum, deal.LenderRefNum);
            string activity = "Deal Status Update";
           
            var auditLogRequest = new WriteLogRequest
            {
                LogEntry = new LogEntry()
                {
                    Activity = activity,
                    ActivityDate = DateTime.Now,
                    Message = message,
                    UserName = UserID,
                    IPAddress = FundsAllocationHelper.GetIPAddress()
                }
            };

            if (!IsLawyerOrClerkOrAssistant(userContextType))
            {
                auditLogRequest.LogEntry.RequestSource = "FCT";
                auditLogRequest.LogEntry.SourceSystem = "Lender Admin Tool";
            }

            _auditLogService.WriteLog(auditLogRequest);
        }

        private bool IsLawyerOrClerkOrAssistant(string userType)
        {
            bool IsLawyer = userType.Equals(UserType.Lawyer, StringComparison.CurrentCultureIgnoreCase)
                || userType.Equals(UserType.Clerk, StringComparison.CurrentCultureIgnoreCase)
                || userType.Equals(UserType.Assistant, StringComparison.CurrentCultureIgnoreCase);
            return IsLawyer;
        }

        private string GetStatus(string prevStatus, string currentStatus)
        {
            string status = string.Empty;
            switch (prevStatus)
            {
                case "CANCELLED":
                    status = "dealUndoCancel";
                    break;
                case "CANCELLATION REQUESTED":
                    status = "dealUndoCancelReq";
                    break;
                default:
                    status = "dealActive";
                    break;
            }

            return status;
        }

        public GetNotesResponse GetNotes(GetNotesRequest request)
        {
            

            GetNotesResponse response = new GetNotesResponse();
            tblDeal _deal = _dealRepository.GetDealNotes(request.DealID, request.UserContext);

            response.Notes = MapDealRelatedEntities.MapToNotesCollection(_deal);
            return response;
        }

        public GetDealHistoryResponse GetDealHistory(GetDealHistoryRequest request)
        {
            

            GetDealHistoryResponse response = new GetDealHistoryResponse();
            tblDeal _deal = _dealRepository.GetDealHistory(request.DealID, request.UserContext);

            response.Activities = MapDealRelatedEntities.MapToActivityCollection(_deal);
            return response;
        }

        public GetMilestonesResponse GetDealMilestones(GetMilestonesRequest request)
        {
            tblDeal _deal = _dealRepository.GetMilestones(request.DealID, request.UserContext);
            
            if (_deal == null) return null;
            
            GetMilestonesResponse response = new GetMilestonesResponse();
            var dealTypes = _deal.BusinessModel.Split('/');
            if (dealTypes.Contains("LLC") || dealTypes.Contains("MMS"))
                response.Milestones = MapDealRelatedEntities.MapToMilestonesCollection(_deal);
            if (dealTypes.Contains("EASYFUND"))
            {
                var _fundingDeal = _fundedRepository.GetMilestonesByDeal(request.DealID);
                if (_fundingDeal != null)
                    response.FundingMilestones = MapDealRelatedEntities.MapToFundedDealMilestones(_fundingDeal);
            }

            return response;
        }

        public int GetOtherDealid(GetDealRequest request)
        {
            return _fundingDealRepository.GetOtherDealInScope(request.DealID); 
        }

        //Added By MEHDI
        public IQueryable<tblDealHistory> GetDealHistories(int dealId)
        {
            return _dealRepository.GetDealHistory(dealId);
        }

        public tblDeal GetTbDealByDealId(int dealId)
        {
            return _dealRepository.GetDeal(dealId, true);
        }
    }
}
