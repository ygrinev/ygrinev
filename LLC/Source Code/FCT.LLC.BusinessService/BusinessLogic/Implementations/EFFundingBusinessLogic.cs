using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Transactions;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.DocumentService.Client;
using FCT.LLC.DocumentService.Common;


namespace FCT.LLC.BusinessService.BusinessLogic
{
    public partial class DealManagementBusinessLogic : IDealManagementBusinessLogic
    {
        private readonly IFundingDealRepository _dealFundingRepository;
        private readonly IDealScopeRepository _dealScopeRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IVendorRepository _vendorRepository;
        private readonly IMortgagorRepository _mortgagorRepository;
        private readonly IDealContactRepository _dealContactRepository;
        private readonly IMortgageRepository _mortgageRepository;
        private readonly IPINRepository _pinRepository;
        private readonly ILawyerRepository _lawyerRepository;
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly IFundedRepository _fundedRepository;
        private readonly DocumentListManager _documentListManager;
        private readonly IDealAmendmentsChecker _amendmentsChecker;
        private readonly IFeeCalculator _feeCalculator;
        private readonly IEmailHelper _emailHelper;
        private readonly IVendorLawyerHelper _vendorLawyerHelper;
        private readonly IMilestoneUpdater _milestoneUpdater;
        private readonly IBuilderLegalDescriptionRepository _builderDescriptionRepository;
        private readonly IBuilderUnitLevelRepository _builderUnitLevelRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IDealEventsPublisher _dealEventsPublisher;

        private Common.DataContracts.UserContext _userContext;
        private string globalFCTURN;
        const string ConnectionKeyName = "LLCDBconnection";

        public DealManagementBusinessLogic(IFundingDealRepository dealFundingRepository,
            IDealScopeRepository dealScopeRepository,
            IPropertyRepository propertyRepository, IVendorRepository vendorRepository,
            IMortgagorRepository mortgagorRepository, IDealContactRepository dealContactRepository,
            IMortgageRepository mortgageRepository, IPINRepository pinRepository, ILawyerRepository lawyerRepository,
            IDealHistoryRepository dealHistoryRepository, IFundedRepository fundedRepository,
            IDealAmendmentsChecker amendmentsChecker, IFeeCalculator feeCalculator, IEmailHelper emailHelper,
            IVendorLawyerHelper vendorLawyerHelper, IMilestoneUpdater milestoneUpdater,
            IBuilderLegalDescriptionRepository builderDescriptionRepository, IBuilderUnitLevelRepository builderUnitLevelRepository,
            IDealRepository dealRepository, IDealEventsPublisher dealEventsPublisher)
        {
            _dealFundingRepository = dealFundingRepository;
            _dealScopeRepository = dealScopeRepository;
            _propertyRepository = propertyRepository;
            _mortgagorRepository = mortgagorRepository;
            _vendorRepository = vendorRepository;
            _dealContactRepository = dealContactRepository;
            _mortgageRepository = mortgageRepository;
            _pinRepository = pinRepository;
            _lawyerRepository = lawyerRepository;
            _dealHistoryRepository = dealHistoryRepository;
            _fundedRepository = fundedRepository;
            _amendmentsChecker = amendmentsChecker;
            _feeCalculator = feeCalculator;
            _emailHelper = emailHelper;
            _vendorLawyerHelper = vendorLawyerHelper;
            _milestoneUpdater = milestoneUpdater;
            _builderDescriptionRepository = builderDescriptionRepository;
            _builderUnitLevelRepository = builderUnitLevelRepository;
            _dealRepository = dealRepository;
            _dealEventsPublisher = dealEventsPublisher;
            _documentListManager = new DocumentListManager();
        }

        public GetFundingDealResponse GetFundingDeal(GetFundingDealRequest request)
        {
            _userContext = request.UserContext;
            string status = _dealFundingRepository.GetStatus(request.DealID);
            FundingDeal data;
            switch (status)
            {
                case DealStatus.CancelRequest:
                case DealStatus.Cancelled:
                    data=_dealFundingRepository.GetCancelledDeal(request.DealID);
                    break;
                default:
                    data=_dealFundingRepository.GetFundingDeal(request.DealID);
                    break;
            }
            var response = new GetFundingDealResponse {Deal = data};
            return response;
        }

        public SaveFundingDealResponse SaveFundingDeal(SaveFundingDealRequest request)
        {
            _userContext = request.UserContext;
            string invalidFields;
            bool validRequest = FundingDealRequestValidator.ValidateRequest(request, out invalidFields);
            if (validRequest)
            {
                FundingDeal requestDeal = request.Deal;
                var response = new SaveFundingDealResponse();


                string UserName = (request.UserContext != null) ? request.UserContext.UserID : string.Empty;


                if (requestDeal.DealID.HasValue)
                {
                    int otherDealId = 0;
                    //this is to handel LLC/EasyFund combo deal, calling from Transaction Details tab
                    if (requestDeal.BusinessModel == BusinessModel.LLC || requestDeal.BusinessModel == BusinessModel.MMS)
                    {
                        //if this is a LLC standalone deal, easy fund is to be added.
                        response.Deal = CreateFundingDealForLLC(requestDeal);
                        return response;
                    }
                    if (requestDeal.BusinessModel == BusinessModel.LLCCOMBO || requestDeal.BusinessModel == BusinessModel.MMSCOMBO)
                    {
                        //if this is already a LLC/EasyFund combo deal
                        response.Deal = UpdateFundingDealForLLC(requestDeal, request.UserContext);
                        return response;
                    }

                    int dealScopeId = _dealScopeRepository.GetDealScope(requestDeal.FCTURN);
                    globalFCTURN = requestDeal.FCTURN;
                    string lastStatus = _dealFundingRepository.GetStatus((int) requestDeal.DealID);

                    //Retrieve old data from database before update for compare and deal history
                    FundingDeal oldFundingDeal = _dealFundingRepository.GetFundingDeal(requestDeal.DealID.Value);

                    if (!FundingBusinessLogicHelper.IsValidDealForUpdate(oldFundingDeal.DealStatus))
                    {
                        throw new InValidDealException()
                        {
                            ViolationCode = ErrorCode.DealCancelledOrDeclined,
                            ExceptionMessage = ErrorCode.DealCancelledOrDeclined.ToString()
                        };
                    }
                    using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                    {
                        _vendorLawyerHelper.SyncAnyLawyerActingForChanges(requestDeal, oldFundingDeal.ActingFor);


                        //Do not update the Lawyer Application if it already exists
                        //This check is made to make sure that the application source is not oiverwritten
                        //e.g., Deal Ccreated from Conveyancer request and updated in lawyer Portal
                        if (!string.IsNullOrEmpty(oldFundingDeal.LawyerApplication))
                            requestDeal.LawyerApplication = oldFundingDeal.LawyerApplication;

                        response.Deal = _dealFundingRepository.UpdateFundingDeal(requestDeal, dealScopeId);
                        int responseDealId = response.Deal.DealID ?? default(int);


                        // Clear values sent by Conveyancer at the time of Deal creation,
                        // if the other lawyer has been selected in lawyer portal
                        if(requestDeal.OtherLawyer != null && requestDeal.OtherLawyer.LawyerID > 0)
                        {
                            requestDeal.OtherLawyer.LawFirm = null;
                            requestDeal.OtherLawyer.FirstName = null;
                            requestDeal.OtherLawyer.LastName = null;

                            _dealFundingRepository.UpdateOtherLawyerInfoFromDoProcess(requestDeal, dealScopeId);
                        }


                        switch (requestDeal.ActingFor.ToUpper())
                        {
                            case LawyerActingFor.Both:
                            case LawyerActingFor.Mortgagor:

                                _mortgagorRepository.UpdateMorgagorRange(requestDeal.Mortgagors,
                                    (int) requestDeal.DealID);

                                _dealContactRepository.UpdateDealContactRange(requestDeal.Lawyer.DealContacts,
                                    responseDealId);

                                _mortgageRepository.UpdateMortgage(responseDealId,
                                    requestDeal.ClosingDate);

                                _propertyRepository.UpdateProperty(requestDeal.Property,
                                    responseDealId);

                                _pinRepository.UpdatePINRange(requestDeal.Property.Pins,
                                    requestDeal.Property.PropertyID ?? default(int));

                                //_builderDescriptionRepository.UpdateBuilderLegalDescription(requestDeal.Property.BuilderLegalDescription, 
                                //    requestDeal.Property.PropertyID ?? default(int));

                                if (requestDeal.DealType == DealType.PurchaseSale)
                                {
                                    _vendorRepository.UpdateVendorRange(requestDeal.Vendors, dealScopeId);

                                    _vendorLawyerHelper.UpdateVendorLawyerDisbursement(requestDeal.Lawyer, responseDealId, request.UserContext,
                                        requestDeal.ActingFor, oldFundingDeal.ActingFor); 
                                }

                                if (lastStatus != requestDeal.DealStatus &&
                                    requestDeal.DealStatus == DealStatus.Active)
                                {
                                    UpdateMilestones(dealScopeId, true, false);  
                                }
                                
                                break;
                            case LawyerActingFor.Purchaser:
                                _mortgagorRepository.UpdateMorgagorRange(requestDeal.Mortgagors,
                                    responseDealId);

                                _dealContactRepository.UpdateDealContactRange(requestDeal.Lawyer.DealContacts,
                                    responseDealId);

                                _mortgageRepository.UpdateMortgage(responseDealId,
                                    requestDeal.ClosingDate);

                                _propertyRepository.UpdateProperty(requestDeal.Property,
                                    responseDealId);

                                _pinRepository.UpdatePINRange(requestDeal.Property.Pins,
                                    requestDeal.Property.PropertyID ?? default(int));

                                //_builderDescriptionRepository.UpdateBuilderLegalDescription(requestDeal.Property.BuilderLegalDescription,
                                //    requestDeal.Property.PropertyID ?? default(int));

                                if (lastStatus == DealStatus.UserDraft)
                                {
                                    _vendorRepository.UpdateVendorRange(requestDeal.Vendors, dealScopeId);
                                }
                                if ((requestDeal.OtherLawyer != null) && (requestDeal.OtherLawyer.LawyerID > 0))
                                {                                   
                                    otherDealId = UpdateOtherDeal(responseDealId, requestDeal, dealScopeId,
                                        LawyerActingFor.Vendor, lastStatus);

                                    _vendorLawyerHelper.UpdateVendorLawyerDisbursement(requestDeal.OtherLawyer,
                                        responseDealId, request.UserContext, requestDeal.ActingFor, oldFundingDeal.ActingFor);

                                    if (lastStatus != requestDeal.DealStatus &&
                                        requestDeal.DealStatus == DealStatus.Active)
                                    {
                                        UpdateMilestones(dealScopeId, true, false);
                                        UpdateHistory(requestDeal, _userContext, requestDeal.DealStatus, otherDealId);
                                    }
                                }                               
                                break;

                            case LawyerActingFor.Vendor:
                                _vendorRepository.UpdateVendorRange(requestDeal.Vendors,
                                    dealScopeId);

                                _dealContactRepository.UpdateDealContactRange(requestDeal.Lawyer.DealContacts,
                                    responseDealId);

                                if (lastStatus.Equals(DealStatus.UserDraft))
                                {
                                    _mortgageRepository.UpdateMortgage(responseDealId,
                                        requestDeal.ClosingDate);

                                    _propertyRepository.UpdateProperty(requestDeal.Property,
                                        responseDealId);

                                    _pinRepository.UpdatePINRange(requestDeal.Property.Pins,
                                        requestDeal.Property.PropertyID ?? default(int));
                                    
                                    _mortgagorRepository.UpdateMorgagorRange(requestDeal.Mortgagors,responseDealId);

                                    otherDealId = _dealFundingRepository.GetOtherDealInScope(requestDeal.DealID.Value);

                                    //FIX FOR DEFECT# 41139
                                    //Do not update other deal when the other deal id is 0. System throws error when Other Deal ID is 0 
                                    /* Naga efcode if (otherDealId > 0)
                                    {
                                        _mortgagorRepository.UpdateMortgagorRangeForOtherDeal(requestDeal.Mortgagors, otherDealId, responseDealId);
                                    }
                                    */
                                    _mortgagorRepository.UpdateMortgagorRangeForOtherDeal(otherDealId, responseDealId);

                                }

                                if (requestDeal.Property.BuilderLegalDescription != null)
                                {
                                    if (requestDeal.Property.BuilderLegalDescription.BuilderLegalDescriptionID != null && requestDeal.Property.BuilderLegalDescription.BuilderLegalDescriptionID > 0)
                                    {
                                        _builderDescriptionRepository.UpdateBuilderLegalDescription(requestDeal.Property.BuilderLegalDescription,
                                        requestDeal.Property.PropertyID ?? default(int));

                                        _builderUnitLevelRepository.UpdateBuilderUnitLevelRange(requestDeal.Property.BuilderLegalDescription.BuilderUnitsLevels,
                                           requestDeal.Property.BuilderLegalDescription.BuilderLegalDescriptionID ?? default(int));
                                    }
                                    else
                                    {
                                        var builderLegalDescription = _builderDescriptionRepository.InsertBuilderLegalDescription(requestDeal.Property.BuilderLegalDescription,
                                        requestDeal.Property.PropertyID ?? default(int));

                                        _builderUnitLevelRepository.InsertBuilderUnitLevelRange(requestDeal.Property.BuilderLegalDescription.BuilderUnitsLevels,
                                            builderLegalDescription.BuilderLegalDescriptionID ?? default(int));
                                    }
                                }


                                if ((requestDeal.OtherLawyer != null) && (requestDeal.OtherLawyer.LawyerID > 0))
                                {
                                    otherDealId = UpdateOtherDeal(responseDealId, requestDeal, dealScopeId,
                                        LawyerActingFor.Purchaser, lastStatus);
                                }

                                _vendorLawyerHelper.UpdateVendorLawyerDisbursement(requestDeal.Lawyer, responseDealId, request.UserContext,
                                    requestDeal.ActingFor, oldFundingDeal.ActingFor);

                                if (lastStatus != requestDeal.DealStatus && requestDeal.DealStatus == DealStatus.Active)
                                {
                                    UpdateMilestones(dealScopeId, true, false);
                                    UpdateHistory(requestDeal, _userContext, requestDeal.DealStatus, otherDealId);
                                }

                                break;
                        }
                        //Retrieve updated data from database after update for compare and deal history
                        FundingDeal newFundingDeal = _dealFundingRepository.GetFundingDeal(requestDeal.DealID.Value);

                        if (newFundingDeal.Property.Province != oldFundingDeal.Property.Province)
                        {
                            _feeCalculator.ReCalculateFee(newFundingDeal.Property.Province, requestDeal.DealID.Value);
                        }
                        if (FundingBusinessLogicHelper.ReassignedToSingleLawyer(oldFundingDeal.ActingFor,
                            newFundingDeal.ActingFor))
                        {
                            otherDealId = _dealFundingRepository.GetOtherDealInScope(requestDeal.DealID.Value,
                                currentActingFor: oldFundingDeal.ActingFor);
                            if (otherDealId > 0)
                            {
                                _dealFundingRepository.DeleteFundingDeal(otherDealId, true);
                            }
                            _feeCalculator.ReAssignFee(requestDeal.DealID.Value);
                        }                       
                        bool hasAmendments = _amendmentsChecker.CheckAmendments(oldFundingDeal, newFundingDeal,
                            request.UserContext);
                        //If there is amendments, remove signature for both parties
                        if (hasAmendments) RemoveSignature(oldFundingDeal.DealID.GetValueOrDefault());


                        //Regenerate documents if the acting for has changed
                        if (oldFundingDeal.ActingFor != requestDeal.ActingFor)
                        {
                            //Create document metadata for the deal
                            GenerateDocumentMetadata(requestDeal.DealID.GetValueOrDefault());
                        }

                        response.Deal = newFundingDeal;
                        scope.Complete();
                    }
                    if (lastStatus != requestDeal.DealStatus && requestDeal.DealStatus == DealStatus.Active &&
                        requestDeal.ActingFor != LawyerActingFor.Both &&
                        requestDeal.ActingFor != LawyerActingFor.Mortgagor)
                    {
                        NotifyLawyerAndDelegates(requestDeal.ActingFor, requestDeal, otherDealId);
                    }

                    
                    return response;
                }
                switch (requestDeal.ActingFor.ToUpper())
                {
                    case LawyerActingFor.Both:
                    case LawyerActingFor.Mortgagor:
                        response.Deal = CreateSingleDeal(requestDeal);
                        break;
                    case LawyerActingFor.Purchaser:
                        response.Deal = CreatePurchaserDeal(requestDeal);
                        break;
                    case LawyerActingFor.Vendor:
                        response.Deal = CreateVendorDeal(requestDeal);
                        break;
                }


                if (response.Deal != null)
                {
                    UpdateOtherLawyerInfo(requestDeal, response.Deal.FCTURN);


                    //Send business model to MMS
                    if (!string.IsNullOrEmpty(response.Deal.BusinessModel) 
                        && response.Deal.BusinessModel.ToUpper().Contains(BusinessModel.MMS) 
                        && request.Deal.BusinessModel.ToUpper() != response.Deal.BusinessModel.ToUpper())
                    {
                        _dealEventsPublisher.SendDealBusinessModel((int)response.Deal.DealID, response.Deal.Lawyer.LawyerID);
                }
                }


                response.Deal.DealFCTURN = request.Deal.FCTURN;
                
                return response;
            }
            else
            {
                string errorMsg = string.Format("{0} is/are missing or invalid", invalidFields);
                throw new ArgumentException(errorMsg);
            }
        }

        private void UpdateOtherLawyerInfo(FundingDeal deal, string shortFCTNum)
        {
            // Clear values sent by Conveyancer at the time of Deal creation,
            // if the other lawyer has been selected in lawyer portal

            int dealScopeId = _dealScopeRepository.GetDealScope(shortFCTNum);

            if (deal.OtherLawyer != null && deal.OtherLawyer.LawyerID > 0)
            {
                deal.OtherLawyer.LawFirm = null;
                deal.OtherLawyer.FirstName = null;
                deal.OtherLawyer.LastName = null;
            }

            _dealFundingRepository.UpdateOtherLawyerInfoFromDoProcess(deal, dealScopeId);
        }

        private void NotifyLawyerAndDelegates(string actingFor, FundingDeal requestDeal, int otherDealId)
        {

            if (globalFCTURN == null)
                globalFCTURN = requestDeal.FCTURN;

            var mailerDetails = _emailHelper.GetEmailRecipientList(requestDeal, otherDealId);
            if (mailerDetails.HasValue)
            {
                FundingBusinessLogicHelper.SendNotificationEmail(actingFor, globalFCTURN,
                    mailerDetails.GetValueOrDefault().Key,
                    mailerDetails.GetValueOrDefault().Value, requestDeal);
            }

        }

        private void UpdateHistory(FundingDeal requestDeal, Common.DataContracts.UserContext userContext, string status,
            int otherDealId = 0, List<UserHistory> histories = null)
        {
            if (otherDealId <= 0)
            {
                List<UserHistory> dealhistories = null;
                if (!string.IsNullOrEmpty(requestDeal.LawyerApplication) && requestDeal.LawyerApplication.ToUpper() == "DOPROCESS")
                {
                    dealhistories = new List<UserHistory>
                    {
                        new UserHistory()
                        {
                            DealId = requestDeal.DealID.GetValueOrDefault(),
                            Activity = DealActivity.DoProcessEFDealDraftSaved
                        }
                    };
                }
                else
                {
                    dealhistories = new List<UserHistory>
                    {
                        new UserHistory()
                        {
                            DealId = requestDeal.DealID.GetValueOrDefault(),
                            Activity =(status == DealStatus.UserDraft || status == DealStatus.SystemDraft)? DealActivity.EFDealDraftSaved: DealActivity.EFDealCreated
                        }
                    };
                }

                if (histories == null)
                {
                    histories = dealhistories;
                }
                else
                {
                    histories.AddRange(dealhistories);
                }
            }
            else
            {
                List<UserHistory> dealhistories;

                if (status == DealStatus.UserDraft || status == DealStatus.SystemDraft)
                {
                    dealhistories = new List<UserHistory>()
                    {
                        new UserHistory()
                        {
                            DealId = requestDeal.DealID.GetValueOrDefault(),
                            Activity = DealActivity.EFDealDraftSaved
                        }
                    };
                }
                else
                {
                    string otherLawyerName = string.Format("{0} {1}", requestDeal.OtherLawyer.FirstName,
                        requestDeal.OtherLawyer.LastName);
                    string lawyerName = string.Format("{0} {1}", requestDeal.Lawyer.FirstName,
                        requestDeal.Lawyer.LastName);
                    dealhistories = new List<UserHistory>
                    {
                        new UserHistory()
                        {
                            DealId = requestDeal.DealID.GetValueOrDefault(),
                            Activity = DealActivity.EFCreatedInviteSent,
                            LawyerName = otherLawyerName
                        },
                        new UserHistory()
                        {
                            DealId = otherDealId,
                            Activity = DealActivity.EFInviteReceived,
                            LawyerName = lawyerName
                        }
                    };
                }

                if (histories == null)
                {
                    histories = dealhistories;
                }
                else
                {
                    histories.AddRange(dealhistories);
                }
            }

            _dealHistoryRepository.CreateDealHistories(histories, userContext);
        }

        private void UpdateMilestones(int dealScopeId, bool update, bool draft)
        {
            
            //Check if Funding Milestone exists irrespective of update flag
            //We have issues with MMS deal creating multiple Milestone records for the same dealscope
            int fundingDealId = _fundedRepository.GetFundingDealIdByScope(dealScopeId);
            update = (fundingDealId > 0); //Overwrite update flag based on existence of funding deal record
            
            if (update)
            {
                /* Naga efcode  int fundingDealId = _fundedRepository.GetFundingDealIdByScope(dealScopeId);
                 if (fundingDealId > 0)
                 {*/
                var milestone = new FundingMilestone() {InvitationSent = DateTime.Now};
                    _fundedRepository.UpdateMilestones(fundingDealId, milestone);
           // }
            }

            else
            {
                FundedDeal fundedDeal;
                if (draft)
                {
                    fundedDeal = new FundedDeal()
                    {
                        DealScopeId = dealScopeId,
                    };
                }
                else
                {
                    fundedDeal = new FundedDeal()
                    {
                        DealScopeId = dealScopeId,
                        Milestone =
                        {
                            InvitationSent = DateTime.Now
                        }
                    };
                }

                _fundedRepository.InsertFundedDeal(fundedDeal);
            }

        }

        public void DeleteDraftDeal(DeleteDraftDealRequest request)
        {
            _dealFundingRepository.DeleteFundingDeal(request.DealID, false);
        }

        private int UpdateOtherDeal(int currentDealID, FundingDeal requestDeal, int dealscopeId, string actingFor,
            string lastStatus, FundedDeal milestonesForDeal=null)
        {
            int otherdealId = _dealFundingRepository.GetOtherDealInScope(currentDealID, dealscopeId);

            if (otherdealId > 0 && requestDeal.OtherLawyerDealStatus != DealStatus.Declined)
            {
                _dealFundingRepository.UpdateOtherLawyer(requestDeal.OtherLawyer, otherdealId);
                _dealContactRepository.UpdateDealContactRange(requestDeal.OtherLawyer.DealContacts, otherdealId);

                if (actingFor == LawyerActingFor.Vendor ||
                    (actingFor == LawyerActingFor.Purchaser && lastStatus.Equals(DealStatus.UserDraft)))
                {
                    _mortgageRepository.UpdateMortgage(otherdealId, requestDeal.ClosingDate);

                    var propertyId = _propertyRepository.UpdatePropertyForOtherDeal(requestDeal.Property, otherdealId);

                    _pinRepository.UpdatePINRangeForOtherDeal(requestDeal.Property.Pins, propertyId);

                    if (actingFor == LawyerActingFor.Vendor)
                    {
                        //Naga efcode _mortgagorRepository.UpdateMortgagorRangeForOtherDeal(requestDeal.Mortgagors, otherdealId, currentDealID);
                        _mortgagorRepository.UpdateMortgagorRangeForOtherDeal(otherdealId, currentDealID);

                    }
                }
                //Following code only applys to EasyFund only deal, Combo deal will be depending on the OtherLawyerDealStatus
                string otherDealStatus = _dealFundingRepository.GetStatus(otherdealId);
                if (requestDeal.BusinessModel == BusinessModel.EASYFUND &&
                    requestDeal.DealStatus.ToUpper() == DealStatus.Active && otherDealStatus == DealStatus.SystemDraft)
                {
                    _dealFundingRepository.UpdateDealStatus(DealStatus.New, otherdealId);
                }
                else if (milestonesForDeal!=null && 
                    FundingBusinessLogicHelper.ComboDealStatusChangeRequired(requestDeal, milestonesForDeal, otherDealStatus))
                {
                    _dealFundingRepository.UpdateDealStatus(requestDeal.OtherLawyerDealStatus, otherdealId);
                }
            }
            else
            {
                string otherDealStatus = requestDeal.DealStatus.ToUpper().Equals(DealStatus.Active)
                    ? DealStatus.New
                    : DealStatus.SystemDraft;
                otherdealId = CreateOtherDeal(requestDeal, dealscopeId, actingFor, otherDealStatus);
            }
            return otherdealId;
        }

        private int CreateDealScope(string longFCTURN, string wireDepositcode)
        {
            var dealscope = new DealScope()
            {
                FCTRefNumber = longFCTURN,
                ShortFCTRefNumber = globalFCTURN,
                WireDepositVerificationCode = wireDepositcode,
                FormattedFCTRefNumber = FCTURNHelper.FormatShortFCTURN(globalFCTURN)
            };
            int dealScopeId = _dealScopeRepository.InsertDealScope(dealscope);
            return dealScopeId;
        }

        private FundingDeal CreateFundingDeal(FundingDeal requestDeal, int dealScopeId)
        {
            //store current deal from request in database
            requestDeal.FCTURN = FundingBusinessLogicHelper.GetFCTURN();
            FundingDeal deal = _dealFundingRepository.InsertFundingDeal(requestDeal, dealScopeId);
            deal.FCTURN = globalFCTURN;
            int dealId = deal.DealID ?? default(int);

            //store common attributes for single deal or vendor deal or purchaser deal
            //Save property
            var property = _propertyRepository.InsertProperty(requestDeal.Property, dealId);
            deal.Property = property;
            //Save PIN
            var pins = _pinRepository.InsertPINRange(requestDeal.Property.Pins, property.PropertyID ?? default(int));
            deal.Property.Pins = pins;
            //Save contacts
            var contacts = _dealContactRepository.InsertDealContactRange(requestDeal.Lawyer.DealContacts, dealId);
            deal.Lawyer = requestDeal.Lawyer;
            deal.Lawyer.DealContacts = contacts;
            deal.OtherLawyer = requestDeal.OtherLawyer;

            //Save mortgage
            _mortgageRepository.InsertMortgage(dealId, requestDeal.ClosingDate);
            deal.ClosingDate = requestDeal.ClosingDate;

            //Save Builder Project Details
            if (requestDeal.Property.BuilderLegalDescription != null)
            {
                var builderLegalDescription = _builderDescriptionRepository.InsertBuilderLegalDescription(requestDeal.Property.BuilderLegalDescription,
                                            property.PropertyID ?? default(int));

                var builderUnitLevels = _builderUnitLevelRepository.InsertBuilderUnitLevelRange(requestDeal.Property.BuilderLegalDescription.BuilderUnitsLevels,
                                            builderLegalDescription.BuilderLegalDescriptionID ?? default(int));
                
                builderLegalDescription.BuilderUnitsLevels = builderUnitLevels;

                deal.Property.BuilderLegalDescription = builderLegalDescription;
            }
            
            //Create document metadata for the deal
            GenerateDocumentMetadata(deal.DealID.GetValueOrDefault());

            return deal;
        }

        private FundingDeal CreateSingleDeal(FundingDeal requestDeal)
        {
            string longFCTURN = FundingBusinessLogicHelper.GetFCTURN();
            globalFCTURN = FundingBusinessLogicHelper.GetShortFCTURN(longFCTURN);
            string wireDepositCode = FCTURNHelper.GenerateWireDepositCode();
            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                int dealScopeId = CreateDealScope(longFCTURN, wireDepositCode);

                FundingDeal deal = CreateFundingDeal(requestDeal, dealScopeId);
                int dealId = deal.DealID ?? default(int);
                requestDeal.DealID = dealId;
                //Save both mortgagors and vendors
                var mortgagors = _mortgagorRepository.InsertMortgagorRange(requestDeal.Mortgagors, dealId);
                deal.Mortgagors = mortgagors;

                if (requestDeal.DealType == DealType.PurchaseSale)
                {
                    var vendors = _vendorRepository.InsertVendorRange(requestDeal.Vendors, dealScopeId);
                    deal.Vendors = vendors;
                }

                UpdateHistory(requestDeal, _userContext, requestDeal.DealStatus);

                UpdateMilestones(dealScopeId, false, false);

                scope.Complete();

                return deal;
            }
        }


        private FundingDeal CreatePurchaserDeal(FundingDeal requestDeal)
        {
            string longFCTURN = FundingBusinessLogicHelper.GetFCTURN();
            globalFCTURN = FundingBusinessLogicHelper.GetShortFCTURN(longFCTURN);
            string wireDepositCode = FCTURNHelper.GenerateWireDepositCode();
            int otherDealId = 0;
            FundingDeal deal = null;
            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                int dealScopeId = CreateDealScope(longFCTURN, wireDepositCode);

                deal = CreateFundingDeal(requestDeal, dealScopeId);
                int dealId = deal.DealID ?? default(int);
                requestDeal.DealID = dealId;
                //Save mortgagors
                var mortgagors = _mortgagorRepository.InsertMortgagorRange(requestDeal.Mortgagors, dealId);
                deal.Mortgagors = mortgagors;
                //Save vendors 
                var vendors = _vendorRepository.InsertVendorRange(requestDeal.Vendors, dealScopeId);
                deal.Vendors = vendors;

                if (requestDeal.DealStatus.ToUpper() == DealStatus.Active)
                {
                    otherDealId = CreateOtherDeal(requestDeal, dealScopeId, LawyerActingFor.Vendor, DealStatus.New);

                    UpdateMilestones(dealScopeId, false, false);
                    UpdateHistory(requestDeal, _userContext, requestDeal.DealStatus, otherDealId);
                }
                if (requestDeal.DealStatus.ToUpper() == DealStatus.UserDraft)
                {
                    if ((requestDeal.OtherLawyer) != null && (requestDeal.OtherLawyer.LawyerID > 0))
                    {
                        CreateOtherDeal(requestDeal, dealScopeId, LawyerActingFor.Vendor, DealStatus.SystemDraft);
                    }
                    UpdateMilestones(dealScopeId, false, true);
                    UpdateHistory(deal, _userContext, requestDeal.DealStatus);
                }
                scope.Complete();
            }

            if (requestDeal.DealStatus.ToUpper() == DealStatus.Active)
            {
                NotifyLawyerAndDelegates(LawyerActingFor.Purchaser, deal, otherDealId);
            }
            return deal;
        }

        private FundingDeal CreateVendorDeal(FundingDeal requestDeal)
        {
            string longFCTURN = FundingBusinessLogicHelper.GetFCTURN();
            globalFCTURN = FundingBusinessLogicHelper.GetShortFCTURN(longFCTURN);
            string wireDepositCode = FCTURNHelper.GenerateWireDepositCode();
            int otherDealId = 0;
            FundingDeal deal = null;
            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                int dealScopeId = CreateDealScope(longFCTURN, wireDepositCode);

                deal = CreateFundingDeal(requestDeal, dealScopeId);
                int dealId = deal.DealID ?? default(int);
                requestDeal.DealID = dealId;
                //Save vendors 
                var vendors = _vendorRepository.InsertVendorRange(requestDeal.Vendors, dealScopeId);
                deal.Vendors = vendors;
                //Save purchasers
                var mortgagors = _mortgagorRepository.InsertMortgagorRange(requestDeal.Mortgagors, dealId);
                deal.Mortgagors = mortgagors;
                if (requestDeal.DealStatus.ToUpper() == DealStatus.Active)
                {
                    otherDealId = CreateOtherDeal(requestDeal, dealScopeId, LawyerActingFor.Purchaser, DealStatus.New);
                    UpdateMilestones(dealScopeId, false, false);
                    UpdateHistory(requestDeal, _userContext, requestDeal.DealStatus, otherDealId);
                }
                if (requestDeal.DealStatus.ToUpper() == DealStatus.UserDraft)
                {
                    if ((requestDeal.OtherLawyer != null) && (requestDeal.OtherLawyer.LawyerID > 0))
                    {
                        CreateOtherDeal(requestDeal, dealScopeId, LawyerActingFor.Purchaser, DealStatus.SystemDraft);
                    }
                    UpdateMilestones(dealScopeId, false, true);
                    UpdateHistory(deal, _userContext, requestDeal.DealStatus);
                }
                scope.Complete();
            }
            if (requestDeal.DealStatus.ToUpper() == DealStatus.Active)
            {
                NotifyLawyerAndDelegates(LawyerActingFor.Vendor, deal, otherDealId);
            }
            return deal;
        }

        private int CreateOtherDeal(FundingDeal requestDeal, int dealScopeId, string actingFor, string dealstatus)
        {
            var otherDeal = new FundingDeal
            {
                BusinessModel = BusinessModel.EASYFUND,
                ActingFor = actingFor,
                DealType = requestDeal.DealType,
                DealStatus = dealstatus,
                Lawyer = requestDeal.OtherLawyer,
                OtherLawyer = requestDeal.Lawyer,
                LawyerFileNumber = requestDeal.OtherLawyerFileNumber,
                Property = requestDeal.Property,
                ClosingDate = requestDeal.ClosingDate
            };

            //Other Lawyer Info - EasyFund Phase 3
            if(requestDeal.OtherLawyer.LawyerID <= 0)
            {
                otherDeal.OtherLawyer.FirstName = requestDeal.OtherLawyer.FirstName;
                otherDeal.OtherLawyer.LastName = requestDeal.OtherLawyer.LastName;
                otherDeal.OtherLawyer.LawFirm = requestDeal.OtherLawyer.LawFirm;
            }
            else
            {
                otherDeal.OtherLawyer.LawFirm = null;
                otherDeal.OtherLawyer.FirstName = null;
                otherDeal.OtherLawyer.LastName = null;
            }

            FundingDeal deal = CreateFundingDeal(otherDeal, dealScopeId);

            int otherDealId = deal.DealID ?? default(int);

            // Add mortgagor for the other deal when other deal is being created
            if (otherDealId > 0)
            {
                int firstDealId = _dealFundingRepository.GetOtherDealInScope(otherDealId, dealScopeId);
                _mortgagorRepository.InsertMortgagorRangeForOtherDeal(requestDeal.Mortgagors, otherDealId, firstDealId);
            }

            return otherDealId;
        }

        private void GenerateDocumentMetadata(int dealId)
        {
            var info = new DocumentListInfo {DealId = dealId, UsesPackageId = false, IsOtherAllowed = true};
            //using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                var results = _documentListManager.CreateDocumentListSpecificToDeal(info); 
            }
            
        }

        private void UpdateWireDepositDetails(string longFCTURN, string wireDepositcode)
        {
            var dealscope = new DealScope()
            {
                FCTRefNumber = longFCTURN,
                ShortFCTRefNumber = globalFCTURN,
                WireDepositVerificationCode = wireDepositcode,
                FormattedFCTRefNumber = FCTURNHelper.FormatShortFCTURN(globalFCTURN)
            };

            _dealScopeRepository.UpdateWireDepositDetails(dealscope);
        }
    }
}
