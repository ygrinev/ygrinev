using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;
using FCT.LLC.DocumentService.Client;
using FCT.LLC.DocumentService.Common;
using FCT.Services.AuditLog.DataContracts;
using FCT.Services.AuditLog.MessageContracts;
using FCT.Services.AuditLog.ServiceContracts;
using UserContext = FCT.LLC.Common.DataContracts.UserContext;
using UserType = FCT.LLC.Common.DataContracts.UserType;
using System.ServiceModel;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class CancelDealBusinessLogic : ICancelDealBusinessLogic
    {
        private readonly IFundingDealRepository _fundingDealRepository;
        private readonly IFundedRepository _fundedRepository;
        private readonly IDealFundsAllocRepository _dealFundsAllocRepository;
        private readonly IDealScopeRepository _dealScopeRepository;
        private readonly ILawyerRepository _lawyerRepository;
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly IMortgagorRepository _mortgagorRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMortgageRepository _mortgageRepository;
        private readonly IDealContactRepository _dealContactRepository;
        private readonly IPINRepository _pinRepository;
        private readonly ICancelDealHelper _cancelDealHelper;
        private UserContext _userContext;
        private readonly IDealRepository _dealRepository;
        private readonly IGlobalizationRepository _globalizationRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly IDealEventsPublisher _dealEventsPublisher;
        private readonly IVendorRepository _vendorRepository;

        private string _documentsFilter;
        const string ConnectionKeyName = "LLCDBconnection";
        private Lookup _otherDealDetails; 

        private bool _suppressDealHistory = false;

        public CancelDealBusinessLogic(IFundingDealRepository fundingDealRepository, IFundedRepository fundedRepository,
            IDealFundsAllocRepository dealFundsAllocRepository, IDealScopeRepository dealScopeRepository,
            ILawyerRepository lawyerRepository, IDealHistoryRepository dealHistoryRepository, ICancelDealHelper cancelDealHelper,
            IMortgagorRepository mortgagorRepository, IMortgageRepository mortgageRepository,
            IDealContactRepository dealContactRepository, IPropertyRepository propertyRepository,
            IPINRepository pinRepository, IDealRepository dealRepository,
            IGlobalizationRepository globalizationRepository,
            IAuditLogService auditLogService, IDealEventsPublisher dealEventsPublisher, IVendorRepository vendorRepository)
        {
            _fundedRepository = fundedRepository;
            _fundingDealRepository = fundingDealRepository;
            _dealFundsAllocRepository = dealFundsAllocRepository;
            _dealScopeRepository = dealScopeRepository;
            _lawyerRepository = lawyerRepository;
            _dealHistoryRepository = dealHistoryRepository;
            _cancelDealHelper = cancelDealHelper;
            _mortgageRepository = mortgageRepository;
            _mortgagorRepository = mortgagorRepository;
            _dealContactRepository = dealContactRepository;
            _propertyRepository = propertyRepository;
            _pinRepository = pinRepository;
            _dealRepository = dealRepository;
            _globalizationRepository = globalizationRepository;
            _auditLogService = auditLogService;
            _dealEventsPublisher = dealEventsPublisher;
            _vendorRepository = vendorRepository;
        }

        public void CancelDeal(CancelDealRequest request)
        {
            bool suppressDealHistory = false;

            FundingDeal fundingDeal = _fundingDealRepository.GetFundingDeal(request.DealID);
            FundedDeal milestoneDetails = _fundedRepository.GetMilestonesByDeal(request.DealID);
            _otherDealDetails = _fundingDealRepository.GetOtherDealStatus(request.DealID);
            FundingDeal deal = null;

            if (request.CancellationReason != null && request.CancellationReason == "SOLICITOR_CHANGE")
            {
                _suppressDealHistory = true;
            }

            string beforeCancellationBusinessModel = fundingDeal.BusinessModel;
            int beforeCancellationDealId = (int)fundingDeal.DealID;
            int lawyerID = fundingDeal.Lawyer.LawyerID;

            if (milestoneDetails == null && fundingDeal.BusinessModel != BusinessModel.LLC && fundingDeal.BusinessModel != BusinessModel.MMS)
            {
                //do nothing - even though data is bad we don't throw an exception
                return;
            }

            if (IsValidStatusForCancel(fundingDeal.DealStatus))
            {
                int otherDealId=0;
                int splitDealId = 0;

                List<ErrorCode> errorcodes = new List<ErrorCode>();

                if ( milestoneDetails != null && milestoneDetails.Milestone.Disbursed > DateTime.MinValue)
                {
                    errorcodes.Add(ErrorCode.DealDisbursed);
                }
                _userContext = request.UserContext;

                switch (request.CancelledProduct)
                {
                    case CancelledProduct.EF:
                        CancelEasyFund(fundingDeal, request, errorcodes, milestoneDetails, ref otherDealId, ref splitDealId);
                        GetDealAndNotifyLawyer(deal, request, splitDealId, fundingDeal);//, otherDealId);
                        break;
                    case CancelledProduct.LLC:
                        CancelLLC(fundingDeal, request, errorcodes, ref splitDealId);
                        break;
                    case CancelledProduct.LLCEF:
                        CancelLLCandEasyFund(fundingDeal, request, errorcodes, milestoneDetails, ref otherDealId, ref splitDealId, ref deal);
                        GetDealAndNotifyLawyer(deal, request, splitDealId, fundingDeal);
                        break;
                }

                // Send Business Model updates to MMS
                if (!string.IsNullOrEmpty(beforeCancellationBusinessModel) && beforeCancellationBusinessModel.ToUpper().Contains(BusinessModel.MMS))
                {
                    _dealEventsPublisher.SendDealBusinessModel(beforeCancellationDealId, lawyerID);
                }

                    UpdateAuditLog(request, fundingDeal);
        }
        }

        private void GetDealAndNotifyLawyer(FundingDeal deal, CancelDealRequest request, int splitDealId, FundingDeal dealBeforeCancellation)
        {
            int otherDealId = 0;
            if (deal == null)
            {
                if (splitDealId > 0)
                {
                    string status = _fundingDealRepository.GetStatus(splitDealId);
                    if (status == DealStatus.Cancelled || status == DealStatus.CancelRequest)
                    {
                        deal = _fundingDealRepository.GetCancelledDeal(splitDealId);
                        if (deal.BusinessModel.Contains(BusinessModel.EASYFUND))
                        {
                            otherDealId = _fundingDealRepository.GetOtherCancelledDealInScope(splitDealId);
                        }
                    }
                    else
                    {
                        deal = _fundingDealRepository.GetFundingDeal(splitDealId);
                        if (deal.BusinessModel.Contains(BusinessModel.EASYFUND))
                        {
                            otherDealId = _fundingDealRepository.GetOtherDealInScope(splitDealId);
                        }
                    }
                    
                }
                else
                {
                    deal = _fundingDealRepository.GetCancelledDeal(request.DealID);
                    otherDealId = _fundingDealRepository.GetOtherCancelledDealInScope(request.DealID);
                }
            }
            if (CancelDealHelper.NotificationRequired(dealBeforeCancellation, request.UserContext.UserType))
            {
                string recipientParty = CancelDealHelper.GetRecipientActingFor(dealBeforeCancellation, _otherDealDetails);
                _cancelDealHelper.NotifyLawyer(deal, recipientParty);  
            }

            if (CancelDealHelper.OtherLawyerNotificationRequired(_otherDealDetails, request.UserContext.UserType))
            {
                //To send email to another lawyer as well
                deal = _fundingDealRepository.GetCancelledDeal(otherDealId);
                if (deal != null)
                {
                    _cancelDealHelper.NotifyLawyer(deal, dealBeforeCancellation.ActingFor);  
                }
            }
        }

        private void CancelEasyFund(FundingDeal fundingDeal, CancelDealRequest request, List<ErrorCode> errorcodes, FundedDeal milestoneDetails, ref int otherDealId, ref int splitDealId)
        {
            _documentsFilter = BusinessModel.LLC;
            decimal depositAmount = _dealFundsAllocRepository.GetTotalAllocatedFunds(milestoneDetails.FundingDealId);
            if (depositAmount > 0)
            {
                errorcodes.Add(ErrorCode.OutstandingDeposit);
            }
            if (errorcodes.Count > 0)
            {
                throw new ValidationException(errorcodes);
            }

            if (fundingDeal.BusinessModel == BusinessModel.EASYFUND)
            {
                CancelEasyFundInEasyFundOnlyDeal(ref fundingDeal, ref request, ref otherDealId, ref splitDealId);
            }

            if (fundingDeal.BusinessModel == BusinessModel.LLCCOMBO || fundingDeal.BusinessModel == BusinessModel.MMSCOMBO)
            {
                CancelEasyFundInComboDeal(ref fundingDeal, ref request, ref otherDealId, ref splitDealId, milestoneDetails);
            }
            return;
        }
        private void CancelLLC(FundingDeal fundingDeal, CancelDealRequest request, List<ErrorCode> errorcodes, ref int splitDealId)
        {
            _documentsFilter = BusinessModel.LLC;
            if (errorcodes.Count > 0)
            {
                throw new ValidationException(errorcodes);
            }
            int currentDealID = fundingDeal.DealID.GetValueOrDefault();

            if (fundingDeal.BusinessModel == BusinessModel.LLCCOMBO || fundingDeal.BusinessModel == BusinessModel.MMSCOMBO)
            {
                CancelLLCInComboDeal(ref fundingDeal, ref request, ref splitDealId, ref currentDealID);
            }
            else
            {
                CancelLLCInLLCOnlyDeal(ref fundingDeal, ref request, ref currentDealID); 
            }
            return;

        }


        private void CancelLLCInComboDeal(ref FundingDeal fundingDeal, ref CancelDealRequest request, ref int splitDealId, ref int currentDealID)
        {
            var llcDeal = new LLCDeal()
            {
                StatusReasonID = request.StatusReasonID,
                StatusReason = request.CancellationReason
            };

            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                // Lawyer can only REQUEST a cancellation - admin user can CANCEL directly
                if (CancelDealHelper.IsLawyerOrClerkOrAssistant(_userContext.UserType))
                {
                    splitDealId = CancelComboDeal(fundingDeal, fundingDeal.DealStatus, DealStatus.CancelRequest, llcDeal);
                }
                else
                {
                    splitDealId = CancelComboDeal(fundingDeal, fundingDeal.DealStatus, DealStatus.Cancelled, llcDeal);
                }

                if (_suppressDealHistory == false)
                {
                    CreateLLCCancelDealHistory(currentDealID, request, splitDealId);
                }

                //Get the Funding Deal DealID because the check for BusinessModel uses fudningDeal object
                _dealEventsPublisher.AddDealEventForCancellation(request.DealID, _userContext.UserID, _userContext.UserType);

                scope.Complete();
            }
        }
        private void CancelLLCandEasyFund(FundingDeal fundingDeal, CancelDealRequest request, List<ErrorCode> errorcodes, FundedDeal milestoneDetails, ref int otherDealId, ref int splitDealId, ref FundingDeal deal)
        {
            decimal depositAmount = _dealFundsAllocRepository.GetTotalAllocatedFunds(milestoneDetails.FundingDealId);
            if (depositAmount > 0)
            {
                errorcodes.Add(ErrorCode.OutstandingDeposit);
            }
            if (errorcodes.Count > 0)
            {
                throw new ValidationException(errorcodes);
            }
            if (fundingDeal.BusinessModel == BusinessModel.LLCCOMBO || fundingDeal.BusinessModel == BusinessModel.MMSCOMBO)
            {
                var llcDeal = new LLCDeal()
                {
                    StatusReasonID = request.StatusReasonID,
                    StatusReason = request.CancellationReason
                };

                deal = _fundingDealRepository.GetFundingDeal(request.DealID);

                using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                {
                    //splitDealId = CancelComboDeal(fundingDeal, DealStatus.Cancelled, DealStatus.CancelRequest, llcDeal);

                    //When Lender is cancelling Combo Deal then cancel both MMS & EasyFund
                    //When Lawyer is cancelling Combo Deal then Cancel EasyFund deal & set MMS deal to Cancellation Rquested
                    if (CancelDealHelper.IsLawyerOrClerkOrAssistant(_userContext.UserType))
                    {
                        splitDealId = CancelComboDeal(fundingDeal, DealStatus.Cancelled, DealStatus.CancelRequest, llcDeal);
                    }
                    else
                    {
                        splitDealId = CancelComboDeal(fundingDeal, DealStatus.Cancelled, DealStatus.Cancelled, llcDeal);
                    }

                    if (fundingDeal.ActingFor != LawyerActingFor.Both && fundingDeal.ActingFor!=LawyerActingFor.Mortgagor)
                    {
                        otherDealId = _fundingDealRepository.GetOtherDealInScope(splitDealId);

                        _fundingDealRepository.UpdateDealStatus(DealStatus.Cancelled, otherDealId);
                        if (_suppressDealHistory == false)
                        {
                            UpdateComboDealHistory(deal, request, otherDealId);
                        }
                    }
                    else
                    {
                        if (_suppressDealHistory == false)
                        {
                            UpdateComboDealHistory(deal, request);
                        }
                    }

                    //Add Deal Events Publisher
                    //Get the Funding Deal DealID because the check for BusinessModel uses fudningDeal object
                    _dealEventsPublisher.AddDealEventForCancellation(request.DealID, _userContext.UserID, _userContext.UserType);
                    scope.Complete();
                }
            }
            return;
        }

        private void CancelEasyFundInComboDeal(ref FundingDeal fundingDeal, ref CancelDealRequest request, ref int otherDealId, ref int splitDealId, FundedDeal milestoneDetails)
        {
            if (fundingDeal.ActingFor != LawyerActingFor.Both && fundingDeal.ActingFor!=LawyerActingFor.Mortgagor)
            {
                otherDealId = _fundingDealRepository.GetOtherDealInScope(request.DealID);
            }

            int fundingDealId = fundingDeal.DealID.HasValue ? fundingDeal.DealID.Value : -1;

            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                splitDealId = CancelComboDeal(fundingDeal, DealStatus.Cancelled, fundingDeal.DealStatus);

                //Clean up the Vendors & Milestones of an EasyFund deal as a Deal can be cancelled and reinstructed at a later date
                _vendorRepository.DeleteRange(fundingDeal.Vendors);
                _fundedRepository.ResetMilestones(milestoneDetails.FundingDealId);

                if (fundingDeal.ActingFor != LawyerActingFor.Both && fundingDeal.ActingFor != LawyerActingFor.Mortgagor)
                {
                    _fundingDealRepository.UpdateDealStatus(DealStatus.Cancelled, otherDealId);
                    if (fundingDeal.DealID == null)
                    {
                        fundingDeal.DealID = fundingDealId;
                    }
                    if (_suppressDealHistory == false)
                    {
                        UpdateComboDealHistory(fundingDeal, request, otherDealId, splitDealId);
                    }
                }
                else
                {
                    if (_suppressDealHistory == false)
                    {
                        UpdateEFOnlyDealHistory(request.DealID, request.UserContext);
                    }
                }
                scope.Complete();
            }
            return;
        }

        private void CancelEasyFundInEasyFundOnlyDeal(ref FundingDeal fundingDeal, ref CancelDealRequest request, ref int otherDealId, ref int splitDealId)
        {
            switch (fundingDeal.ActingFor)
            {
                case LawyerActingFor.Vendor:
                    CancelEasyFundDealActingForVendor(ref request, ref otherDealId, ref splitDealId);
                    break;
                case LawyerActingFor.Purchaser:
                    CancelEasyFundInEasyFundOnlyDealActingForPurchaser(ref request, ref otherDealId);
                    break;
                case LawyerActingFor.Both:
                case LawyerActingFor.Mortgagor:
                    CancelEasyFundInEasyFundOnlyDealActingForBoth(ref request);
                    break;
            }
            return;
        }

        private void CancelEasyFundInEasyFundOnlyDealActingForBoth(ref CancelDealRequest request)
        {
            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                _fundingDealRepository.UpdateDealStatus(DealStatus.Cancelled, request.DealID);
                if (_suppressDealHistory == false)
                {
                    UpdateEFOnlyDealHistory(request.DealID, request.UserContext);
                }
                scope.Complete();
            }
        }

        private void CancelEasyFundInEasyFundOnlyDealActingForPurchaser(ref CancelDealRequest request, ref int otherDealId)
        {
            var ids = new List<int> { request.DealID };
            otherDealId = _fundingDealRepository.GetOtherDealInScope(request.DealID);
            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                if (otherDealId > 0)
                {
                    ids.Add(otherDealId);
                }
                _fundingDealRepository.UpdateDealStatus(ids, DealStatus.Cancelled);

                if (_suppressDealHistory == false) { 
                    UpdateEFOnlyDealHistory(request.DealID, request.UserContext, otherDealId);
                }
                scope.Complete();
            }

        }

        private void CancelEasyFundDealActingForVendor(ref CancelDealRequest request, ref int otherDealId,
            ref int splitDealId)
        {
            var ids = new List<int> {request.DealID};
            var otherDeal = _fundingDealRepository.GetOtherDeal(request.DealID);

            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                if (otherDeal != null)
                {
                    otherDealId = otherDeal.DealID.GetValueOrDefault();
                    if (otherDeal.BusinessModel == BusinessModel.LLCCOMBO || otherDeal.BusinessModel == BusinessModel.MMSCOMBO)
                    {
                        splitDealId = CancelComboDeal(otherDeal, DealStatus.Cancelled,
                            otherDeal.DealStatus);
                    }
                    else
                    {
                        ids.Add(otherDealId);
                    }
                }
                _fundingDealRepository.UpdateDealStatus(ids, DealStatus.Cancelled);

                if (_suppressDealHistory == false)
                {
                    UpdateEFOnlyDealHistory(request.DealID, request.UserContext, otherDealId);
                }

                scope.Complete();
            }
        }
        private void CancelLLCInLLCOnlyDeal(ref FundingDeal fundingDeal, ref CancelDealRequest request, ref int currentDealID)
        {

            if (fundingDeal.BusinessModel == BusinessModel.LLC || fundingDeal.BusinessModel == BusinessModel.MMS)
            {
                using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                {
                    // Lawyer can only REQUEST a cancellation - admin user can CANCEL directly
                    if (!CancelDealHelper.IsLawyerOrClerkOrAssistant(_userContext.UserType))
                    {
                        _dealRepository.UpdateDealStatus(request.DealID, DealStatus.Cancelled,
                            request.UserContext);
                    }
                    else
                    {
                        _dealRepository.UpdateDealStatus(request.DealID, DealStatus.CancelRequest,
                            request.UserContext);
                    }


                    if (_suppressDealHistory == false)
                    {
                        CreateLLCCancelDealHistory(currentDealID, request);
                    }
                    
                    //Get the Funding Deal DealID because the check for BusinessModel uses fudningDeal object
                    _dealEventsPublisher.AddDealEventForCancellation(request.DealID, _userContext.UserID, _userContext.UserType);

                    scope.Complete();
                }
            }
        }
        private bool IsValidStatusForCancel(string dealStatus)
        {
            bool validDeal =  ( dealStatus == DealStatus.Active || 
                                dealStatus == DealStatus.New ||
                                dealStatus == DealStatus.CancelRequest ||
                                dealStatus == DealStatus.UserDraft || 
                                dealStatus == DealStatus.SystemDraft );
            return validDeal;
        }

        private void UpdateEFOnlyDealHistory(int currentDealId, UserContext userContext, int otherDealId = 0)
        {
            _dealHistoryRepository.CreateDealHistory(currentDealId, userContext, DealActivity.EFDealCancelled);
            if (otherDealId > 0)
            {
                _dealHistoryRepository.CreateDealHistory(otherDealId, userContext, DealActivity.EFDealCancelled);
            }
        }

        private void UpdateComboDealHistory(FundingDeal fundingDeal, CancelDealRequest request, int otherDealId = 0, int splitDealId = 0)
        {
            switch (fundingDeal.ActingFor)
            {
                case LawyerActingFor.Purchaser:
                    //LLC Deal History Updated
                    _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(), request.UserContext, DealActivity.EFDealCancelled);

                    //FIX FOR DEFECT# 41094
                    //Sytem throws error when otherDealId is 0. This will happen in below scenario 
                    //EasyFund deal is added to LLC and other lawyer is not selected.
                    //User tries to Request a cancellation for EF deal
                    if (otherDealId > 0)
                    {
                        //EF Vendor Deal History Updated
                        _dealHistoryRepository.CreateDealHistory(otherDealId, request.UserContext, DealActivity.EFDealCancelled);
                    }

                    if (splitDealId > 0)
                    {
                        //EF Purchaser Deal History Updated
                        _dealHistoryRepository.CreateDealHistory(splitDealId, request.UserContext, DealActivity.EFDealCancelled);
                    }

                    break;
                case LawyerActingFor.Vendor:
                    _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(), request.UserContext, DealActivity.EFDealCancelled);

                    break;
                case LawyerActingFor.Both:
                case LawyerActingFor.Mortgagor:
                    _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(), request.UserContext, DealActivity.EFDealCancelled);
                    break;
            }
        }


        private void CreateLLCCancelDealHistory(int dealId, CancelDealRequest request, int OtherDealId = 0)
        {
            if (CancelDealHelper.IsLawyerOrClerkOrAssistant(_userContext.UserType))
            {
                CancelLLCDealHistory(dealId, ref request);
            }
            else
            {
                CancelLLCDealHistoryAdmin(dealId, ref request);
                if(OtherDealId > 0)
                {
                    CancelLLCDealHistoryAdmin(OtherDealId, ref request);
                }
            }
        }

        private void CancelLLCDealHistoryAdmin(int dealId, ref CancelDealRequest request)
        {
            tblDeal deal = _dealRepository.GetDealDetails(dealId, request.UserContext, true);
            string strStatusUpdateEng = string.Empty;

            DealHistoryEntry dealHistoryEntry = _globalizationRepository.GetEntry(ResourceSet.FCTLLCBusinessService, DealActivity.DealCancelled);
            //_dealHistoryRepository.CreateCancelDealHistory(dealId, dealHistoryEntry.ResourceKey, _userContext);
            _dealHistoryRepository.CreateDealHistoryByDealHistoryEntry(dealId, dealHistoryEntry, _userContext, true);
        }

        private void CancelLLCDealHistory(int dealId, ref CancelDealRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.CancellationReason))
            {
                _dealHistoryRepository.CreateLLCDealHistory(dealId, DealActivity.DealCancelledWithReason,
                    request.UserContext, request.CancellationReason, request.StatusReasonID.GetValueOrDefault());
            }
            else
            {
                _dealHistoryRepository.CreateLLCDealHistory(dealId, DealActivity.DealCancelledNoReason,
                    request.UserContext, string.Empty);
            }
        }

        private int CancelComboDeal(FundingDeal fundingDeal, string efStatus, string llcStatus, LLCDeal llcDeal = null)
        {
            if (llcDeal == null)
            {
                llcDeal = new LLCDeal();
            }

            llcDeal.DealID = fundingDeal.DealID.GetValueOrDefault();
            //llcDeal.BusinessModel = BusinessModel.LLC; 
            llcDeal.BusinessModel = (fundingDeal.BusinessModel.Contains(BusinessModel.MMS)) ? BusinessModel.MMS : BusinessModel.LLC;
            llcDeal.Status = llcStatus;
            llcDeal.ActingFor = LawyerActingFor.Purchaser;

            int splitDealID=SplitEFDeal(fundingDeal, efStatus);
            _fundingDealRepository.UpdateDealToLLC(llcDeal);
            return splitDealID;
        }

        private int SplitEFDeal(FundingDeal fundingDeal, string status)
        {
            int dealscopeId = _dealScopeRepository.GetDealScope(fundingDeal.FCTURN);

            var efDeal = fundingDeal;
            efDeal.BusinessModel = BusinessModel.EASYFUND;
            efDeal.DealStatus = status;
            int existingDealId = _fundingDealRepository.GetExistingDeal(efDeal, dealscopeId);
            if (existingDealId <= 0)
            {
                var existingFundingDeal = _fundingDealRepository.GetFundingDeal(fundingDeal.DealID.GetValueOrDefault());
                efDeal.FCTURN = FundingBusinessLogicHelper.GetFCTURN();
                efDeal.DealID = null;


                var newEFDeal = _fundingDealRepository.InsertFundingDeal(efDeal, dealscopeId);

                _mortgagorRepository.InsertMortgagorRange(existingFundingDeal.Mortgagors,
                    newEFDeal.DealID.GetValueOrDefault());

                _dealContactRepository.InsertDealContactRange(existingFundingDeal.Lawyer.DealContacts,
                    newEFDeal.DealID.GetValueOrDefault());

                _mortgageRepository.InsertMortgage(newEFDeal.DealID.GetValueOrDefault(),
                    existingFundingDeal.ClosingDate);

                var property = _propertyRepository.InsertProperty(existingFundingDeal.Property,
                    newEFDeal.DealID.GetValueOrDefault());

                _pinRepository.InsertPINRange(existingFundingDeal.Property.Pins, property.PropertyID.GetValueOrDefault());

                _dealHistoryRepository.SyncDealHistories(existingFundingDeal.DealID.GetValueOrDefault(),
                    newEFDeal.DealID.GetValueOrDefault());

                MigrateDocuments(existingFundingDeal, newEFDeal);

                return newEFDeal.DealID.GetValueOrDefault();
            }
            return 0;
        }

        private void MigrateDocuments(FundingDeal existingFundingDeal, FundingDeal newEFDeal)
        {
            var documentManagerClient = new DocumentManagerClient();
            var documentUserContext = new DocumentService.Common.UserContext
            {
                UserName = _userContext.UserID,
                IPAddress = FundsAllocationHelper.GetIPAddress()
            };
            //using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                documentManagerClient.MigrateDocuments(existingFundingDeal.DealID.GetValueOrDefault(),
                    newEFDeal.DealID.GetValueOrDefault(), Originator.NONE, documentUserContext, _documentsFilter);
                if (existingFundingDeal.DealID.HasValue) {
                    documentManagerClient.DeleteDocumentsByDeal(existingFundingDeal.DealID.Value, BusinessModel.EASYFUND, documentUserContext);
                }
            }
        }

        private void UpdateAuditLog(CancelDealRequest request, FundingDeal dealBeforeCancellation)
        {
            string strStatusUpdateEng = string.Empty;
                        
            string userId = (request.UserContext == null || request.UserContext.UserID == null)
                ? ""
                : request.UserContext.UserID;

            //Check if userType is not Lawyer then log the Audit Log message
            if (!CancelDealHelper.IsLawyerOrClerkOrAssistant(request.UserContext.UserType))
            {
                
                DealHistoryEntry dealHistoryEntry;
                const string activity = "Deal Cancelled";
                string message = string.Empty;

                //if request to cancel Easyfund Deal only - then the audit log message differs accordingly.
                if (request.CancelledProduct == CancelledProduct.EF)
                {
                    dealHistoryEntry = _globalizationRepository.GetEntry(ResourceSet.FCTLLCBusinessService,
                    DealActivity.ActivityEFDealCancelled);
                    if (dealHistoryEntry != null)
                    {
                        strStatusUpdateEng = dealHistoryEntry.EnglishVersion;
                    }
                    message = string.Format(strStatusUpdateEng, dealBeforeCancellation.FCTURN);
                }
                else
                {
                    tblDeal deal = _dealRepository.GetDealDetails(request.DealID, request.UserContext);

                    dealHistoryEntry = _globalizationRepository.GetEntry(ResourceSet.FCTLLCBusinessService, 
                    DealActivity.DealCancelled);
                    if (dealHistoryEntry != null)
                    {
                        strStatusUpdateEng = dealHistoryEntry.EnglishVersion;
                    }
                   
                    message = string.Format("{0} FCT Reference Number: {1}; Lender reference Number: {2}",
                        strStatusUpdateEng, deal.FCTRefNum, deal.LenderRefNum);
                }
                
                //Since this audit log message is updated through Admin tool(Operations Portal)
                //Update the RequestSource and SourceSystem values 
                //because the default values in stored proc "WriteLog" has default value ID: 1 for both
                var auditLogRequest = new WriteLogRequest
                {
                    LogEntry = new LogEntry()
                    {
                        Activity = activity,
                        ActivityDate = DateTime.Now,
                        Message = message,
                        UserName = userId,
                        IPAddress = FundsAllocationHelper.GetIPAddress(),
                        RequestSource = "FCT",
                        SourceSystem = "Lender Admin Tool"

                    }
                };
                _auditLogService.WriteLog(auditLogRequest);
            }
        }
       
        private void AddDealEventForCancellation(int dealId, string userName)
        {
            if (!CancelDealHelper.IsLawyerOrClerkOrAssistant(_userContext.UserType))
            {
                return;
        }

            bool bIsToWayLender = _dealRepository.IsTwoWayLender(dealId);
            
            if(bIsToWayLender)
            {
                int userId = 0;
                LawyerProfile lawyerProfile = _lawyerRepository.GetUserDetails(userName);
                if(null != lawyerProfile)
                {
                    userId = lawyerProfile.LawyerID;
                }
                var publisher = new DealEventPublishing.Client.Publisher(ConnectionKeyName);

                // using (new TransactionScope(TransactionScopeOption.Suppress))
                {
                    publisher.PublishRequestCancellation(dealId, userId);
                }
                
    }
}
    }
}
