using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;
using FCT.LLC.Logging;
using UserContext = FCT.LLC.Common.DataContracts.UserContext;
using UserType = FCT.LLC.Common.DataContracts.UserType;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class DisbursementBusinessLogic : IDisbursementBusinessLogic
    {
        private const string EasyFundFeeDisbursementType = "EasyFund Fee";
        private const string VendorLawyerDisbursementType = "Vendor Lawyer";
        private const string ConnectionKeyName = "LLCDBconnection";
        private readonly IDisbursementRepository _disbursementRepository;
        private readonly IDisbursementSummaryRepository _disbursementSummaryRepository;
        private readonly IFundedRepository _fundedRepository;
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly IFundingDealRepository _fundingDealRepository;
        private readonly IValidationHelper _validationHelper;
        private readonly IPaymentTransferService _paymentTransferService;
        private readonly IPaymentRequestProcessor _paymentRequestProcessor;
        private readonly ILawyerRepository _lawyerRepository;
        private readonly IFeeCalculator _feeCalculator;
        private readonly IEmailHelper _emailHelper;
        private readonly IDealFundsAllocRepository _dealFundAllocRepository;
        private readonly IVendorLawyerHelper _vendorLawyerHelper;
        private readonly IMilestoneUpdater _milestoneUpdater;
        private readonly IApplicationLocker _applicationLocker;
        private readonly ILogger _logger;
        private readonly IDealEventsPublisher _dealEventsPublisherepository;
        private readonly IDealRepository _dealRepository;
        private IEnumerable<Disbursement> _preSavingDisbursements;

        public DisbursementBusinessLogic(IDisbursementRepository disbursementRepository,
            IDisbursementSummaryRepository disbursementSummaryRepository, IFundedRepository fundedRepository,
            IDealHistoryRepository dealHistoryRepository,
            IFundingDealRepository fundingDealRepository, IValidationHelper validationHelper,
            IPaymentTransferService paymentTransferService, IPaymentRequestProcessor paymentRequestProcessor,
            ILawyerRepository lawyerRepository, IFeeCalculator feeCalculator, IEmailHelper emailHelper, 
            IDealFundsAllocRepository dealFundsAllocRepository, IVendorLawyerHelper vendorLawyerHelper, IMilestoneUpdater milestoneUpdater, IApplicationLocker applicationLocker, ILogger logger,
            IDealEventsPublisher dealEventsPublisherepository, IDealRepository dealRepository)
        {
            _disbursementRepository = disbursementRepository;
            _disbursementSummaryRepository = disbursementSummaryRepository;
            _fundedRepository = fundedRepository;
            _dealHistoryRepository = dealHistoryRepository;
            _fundingDealRepository = fundingDealRepository;
            _validationHelper = validationHelper;
            _paymentTransferService = paymentTransferService;
            _paymentRequestProcessor = paymentRequestProcessor;
            _lawyerRepository = lawyerRepository;
            _feeCalculator = feeCalculator;
            _emailHelper = emailHelper;
            _dealFundAllocRepository = dealFundsAllocRepository;
            _vendorLawyerHelper = vendorLawyerHelper;
            _milestoneUpdater = milestoneUpdater;
            _applicationLocker = applicationLocker;
            _logger = logger;
            _dealEventsPublisherepository = dealEventsPublisherepository;
            _dealRepository = dealRepository;
        }

        public GetDisbursementsResponse GetDisbursements(GetDisbursementsRequest request)
        {
            var disbursements = _disbursementRepository.GetDisbursements(request.DealID);
            var response = new GetDisbursementsResponse()
            {
                DealID = request.DealID,
            };
            if (disbursements==null || disbursements.Count<=0)
            {
                response.Disbursements = new DisbursementCollection();
                int fundingDealId = _fundedRepository.GetFundingDealIdByDeal(request.DealID);
                var summary = _disbursementSummaryRepository.GetDisbursementSummary(fundingDealId);
                response.DisbursementSummary = summary;
            }
            else
            {
                response.Disbursements = disbursements;
                var disbursement = disbursements.FirstOrDefault();
                if (disbursement != null)
                {
                    var summary = _disbursementSummaryRepository.GetDisbursementSummary(disbursement.FundingDealID);
                    response.DisbursementSummary = summary;
                }
            }                        
            response.Deposits = _dealFundAllocRepository.GetDeposits(request.DealID);                      
            return response;
        }
        public void SavePayoutComments(SavePayoutCommentsRequest request)
        {
            int fundingDealId = _fundedRepository.GetFundingDealIdByDeal(request.DealID);
            var summary = _disbursementSummaryRepository.GetDisbursementSummary(fundingDealId);
            summary.Comments = request.Comments;
            _disbursementSummaryRepository.UpdateDisbursementSummary(summary);
        }
        public SaveDisbursementsResponse SaveDisbursements(SaveDisbursementsRequest request)
        {
            int fundingDealId = _fundedRepository.GetFundingDealIdByDeal(request.DealID);
            List<ErrorCode> errorCodes = new List<ErrorCode>();

            var version = _disbursementSummaryRepository.GetDisbursementSummaryVersion(fundingDealId);
            if (request.DisbursementSummary.Version.Length > 0)
            {
                if (!version.SequenceEqual(request.DisbursementSummary.Version))
                {
                    throw new DBConcurrencyException("Data has been updated since last GET");
                }
            }
            Fee fee = null;
            decimal sum = 0;
            decimal feeWithTaxes = 0;
            string feedistribution = string.Empty;
            DisbursementSummary disbursementSummary = null;
            DisbursementCollection disbursements;
            MilestoneUpdated milestoneUpdated = null;
            DisbursementFee disbursementFee = null;

            int disburseRecordsCount =
                request.Disbursements.Count(d => d.Action != CRUDAction.Delete && d.PayeeType != EasyFundFee.FeeName);

            _preSavingDisbursements = _disbursementRepository.GetDisbursements(request.DealID);

            CheckConstraint(request.Disbursements, VendorLawyerDisbursementType);

            var fundingDeal = _fundingDealRepository.GetFundingDeal(request.DealID);

            if (!FundingBusinessLogicHelper.IsValidDealForUpdate(fundingDeal.DealStatus))
            {
                throw new InValidDealException()
                {
                    ViolationCode = ErrorCode.DealCancelledOrDeclined,
                    ExceptionMessage = ErrorCode.DealCancelledOrDeclined.ToString()
                };
            }

            //Validate Disbursements
            foreach (var disbursement in request.Disbursements)
            {
                if (disbursement.Action != CRUDAction.Delete)
                {
                    errorCodes = DisbursementHelper.ValidateDisbursement(disbursement, request.DealID, request.UserContext,
                        _fundingDealRepository, _dealRepository).ToList();
                    if (errorCodes.Count > 0)
                    {
                        throw new ValidationException(errorCodes);
                    }
                }
            }

            int otherDealId = _fundingDealRepository.GetOtherDealInScope(request.DealID);

            bool isDisbursementChainDealOnly = false;
            bool hasChainDealDisbursement = false;

            Disbursement chainDealDisbursement = new Disbursement();
            chainDealDisbursement = request.Disbursements.Find(d => d.PayeeType.ToUpper() == "CHAIN DEAL");
            if (chainDealDisbursement != null && chainDealDisbursement.Action != CRUDAction.Delete)
            {
                hasChainDealDisbursement = true;

                if (disburseRecordsCount == 1)
                    isDisbursementChainDealOnly = true;
            }

            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                foreach (var disbursement in request.Disbursements)
                {
                    if (disbursement.PayeeType.ToUpper() == "CHAIN DEAL")
                        continue;

                    if (disbursement.PayeeType == EasyFundFee.FeeName && disburseRecordsCount > 0)
                    {
                        feedistribution = disbursement.FCTFeeSplit;

                        if (isDisbursementChainDealOnly)
                            disburseRecordsCount = 0;
                        else if (hasChainDealDisbursement)
                            disburseRecordsCount = disburseRecordsCount - 1;

                        disbursementFee = _feeCalculator.RecalculateAndSaveFees(disbursement, disburseRecordsCount, fundingDealId);
                        fee = disbursementFee.PurchaserFee ?? disbursementFee.VendorFee;

                        disbursement.Action = CRUDAction.Update;
                        disbursement.PaymentMethod = RecordType.FCTFee;
                        disbursement.PayeeName = UserType.FCTAdmin;
                        feeWithTaxes = FeeCalculator.AssignFeeWithTaxes(fee, disbursement);
                    }
                    else
                    {
                        if (disbursement.Action != CRUDAction.Delete)
                        {
                            sum = sum + disbursement.Amount;
                        }
                    }
                }

                if (request.Disbursements.All(d => d.PayeeType != EasyFundFee.FeeName))
                {
                    if (isDisbursementChainDealOnly)
                        disburseRecordsCount = 0;
                    else if (hasChainDealDisbursement)
                        disburseRecordsCount = disburseRecordsCount - 1;

                    disbursementFee = _feeCalculator.CalculateDefaultFees(disburseRecordsCount, fundingDeal.Property.Province,
                        fundingDeal.ActingFor);
                    fee = disbursementFee.PurchaserFee ?? disbursementFee.VendorFee;

                    if (fundingDeal.ActingFor == LawyerActingFor.Both ||
                        fundingDeal.ActingFor == LawyerActingFor.Mortgagor)
                    {
                        feedistribution = FeeDistribution.PurchaserLawyer;
                    }
                    else
                    {
                        feedistribution = FeeDistribution.SplitEqually;
                    }
                    var disbursement = new Disbursement()
                    {
                        FCTFeeSplit = feedistribution,
                        PayeeType = EasyFundFee.FeeName,
                        PayeeName = UserType.FCTAdmin,
                        Action = CRUDAction.Create,
                        PaymentMethod = RecordType.FCTFee,
                        Province = fundingDeal.Property.Province
                    };
                    feeWithTaxes = FeeCalculator.AssignFeeWithTaxes(fee, disbursement);
                    request.Disbursements.Add(disbursement);
                    CheckConstraint(request.Disbursements, EasyFundFeeDisbursementType);

                }

                request.Disbursements = _vendorLawyerHelper.AdjustVendorLawyerDisbursement(request.Disbursements, fee,
               feedistribution);

                if (fee != null && feedistribution != FeeDistribution.VendorLawyer)
                {
                    sum = sum + feeWithTaxes;
                }

                disbursements = _disbursementRepository.SaveDisbursements(request, fundingDealId, disbursementFee);

                if (disburseRecordsCount > 0)
                {
                    if (version.Length <= 0)
                    {
                        disbursementSummary = _disbursementSummaryRepository.InsertDisbursementSummary(fundingDealId, sum);
                    }
                    else
                    {
                        disbursementSummary =
                            _disbursementSummaryRepository.UpdateDisbursementSummary(request.DisbursementSummary, fundingDealId,
                                sum);
                    }
                }

                if (disbursements.Count > 0)
                {
                    milestoneUpdated = _milestoneUpdater.UpdateMilestones(request.DealID, fundingDeal.ActingFor);
                    if (milestoneUpdated.PassiveUserSignRemoved)
                    {
                        UpdateSignatureHistories(fundingDeal, otherDealId, request.UserContext);
                    }
                    else if (milestoneUpdated.ActiveUserSignRemoved && !milestoneUpdated.PassiveUserSignRemoved)
                    {

                        _dealHistoryRepository.CreateDealHistory(request.DealID, request.UserContext,
                            DealActivity.EFDealSignatureRemoved);
                    }
                    UpdateDealHistories(request.Disbursements, request.DealID, request.UserContext);

                    if (otherDealId > 0)
                    {
                        UpdateDealHistories(request.Disbursements, otherDealId, request.UserContext);
                    }
                }

                scope.Complete();
            }

            if (milestoneUpdated != null && milestoneUpdated.PassiveUserSignRemoved)
            {
                if (fundingDeal.ActingFor != LawyerActingFor.Both &&
                    fundingDeal.ActingFor != LawyerActingFor.Mortgagor &&
                    fundingDeal.OtherLawyer != null)
                {
                    SendNotification(fundingDeal);
                }
            }

            var response = new SaveDisbursementsResponse()
            {
                Disbursements = disbursements,
                DisbursementSummary = disbursementSummary,
                DealID = request.DealID
            };
            return response;
        }

        private void UpdateSignatureHistories(FundingDeal fundingDeal, int otherDealId, UserContext userContext)
        {
            _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(), userContext,
                DealActivity.EFDealSignatureRemoved,
                string.Format("{0} {1}", fundingDeal.Lawyer.FirstName, fundingDeal.Lawyer.LastName));
            if (fundingDeal.OtherLawyer != null)
            {
                _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(), userContext,
                    DealActivity.EFDealSignatureRemoved,
                    string.Format("{0} {1}", fundingDeal.OtherLawyer.FirstName, fundingDeal.OtherLawyer.LastName));
            }

            if (fundingDeal.ActingFor != LawyerActingFor.Both 
                && fundingDeal.ActingFor != LawyerActingFor.Mortgagor 
                && fundingDeal.OtherLawyer != null)
            {
                _dealHistoryRepository.CreateDealHistory(otherDealId, userContext,
                    DealActivity.EFDealSignatureRemoved,
                    string.Format("{0} {1}", fundingDeal.Lawyer.FirstName, fundingDeal.Lawyer.LastName));
                _dealHistoryRepository.CreateDealHistory(otherDealId, userContext, DealActivity.EFDealSignatureRemoved,
                    string.Format("{0} {1}", fundingDeal.OtherLawyer.FirstName, fundingDeal.OtherLawyer.LastName));
            }
        }

        private void SendNotification(FundingDeal fundingDeal)
        {
            var recipientProfile = _lawyerRepository.GetNotificationDetails(fundingDeal.OtherLawyer.LawyerID);
            string recipientActingFor = fundingDeal.ActingFor == LawyerActingFor.Purchaser
                ? LawyerActingFor.Vendor
                : LawyerActingFor.Purchaser;

            //Get Email List including Delegates
            string mailingList = GetNotificationRecipients(fundingDeal);
            if (!string.IsNullOrEmpty(mailingList))
            {
                recipientProfile.Email = mailingList;
            }
            _emailHelper.SendStandardNotification(fundingDeal, recipientProfile, recipientActingFor,
                StandardNotificationKey.EFUpdatedDealUnSigned);
        }

        public CalculateFCTFeeResponse CalculateFctFee(CalculateFCTFeeRequest request)
        {             
            var response = new CalculateFCTFeeResponse();
            int disburseRecordsCount =
                request.Disbursements.Count(d => d.Action != CRUDAction.Delete && d.PayeeType != EasyFundFee.FeeName && d.PayeeType.ToUpper() != "CHAIN DEAL");
            foreach (var disbursement in request.Disbursements)
            {
                if (disbursement.PayeeType.ToUpper() == "CHAIN DEAL")
                    continue;

                if (disbursement.PayeeType == EasyFundFee.FeeName)
                {
                    var fee = _feeCalculator.RecalculateFee(disbursement, disburseRecordsCount, disbursement.FundingDealID);
                    FeeCalculator.AssignFeeWithTaxes(fee, disbursement);

                    disbursement.VendorFee = null;
                    disbursement.PurchaserFee = null;

                    switch (disbursement.FCTFeeSplit)
                    {
                        case FeeDistribution.VendorLawyer:
                            disbursement.VendorFee = fee;
                            break;
                        case FeeDistribution.PurchaserLawyer:
                            disbursement.PurchaserFee = fee;
                            break;
                        case FeeDistribution.SplitEqually:
                            disbursement.PurchaserFee = fee;
                            disbursement.VendorFee = fee;
                            break;
                    }
                    response.Disbursement = disbursement;
                }
            }

            return response;
        }

        public async Task<DisburseFundsResponse> DisburseFunds(DisburseFundsRequest request)
        {
            DisburseFundsResponse response = null;
            FundingMilestone updatedMilestone = null;
            var errorcodes = new List<ErrorCode>();
            DisbursementCollection disbursements;

            FundingDeal fundingDeal = _validationHelper.ValidateDealInDB(request.DealID, errorcodes,
                out disbursements);

            var paymentDetailsTaskAsync = _validationHelper.ValidateVendorLawyer(disbursements, errorcodes, fundingDeal);

            var summary = _disbursementSummaryRepository.GetDisbursementSummary(disbursements.First().FundingDealID);
            if (summary == null)
            {
                errorcodes.Add(ErrorCode.SummaryNotFound);
                throw new ValidationException(errorcodes);
            }           

            DisbursementHelper.ValidateDealMilestones(summary, fundingDeal.ActingFor, errorcodes);

            var disbursementDate = _validationHelper.ValidatePaymentDate(request.UserContext, errorcodes);        

            var totalDepositsReceived=_dealFundAllocRepository.GetTotalAllocatedFunds(summary.FundingDealID);           

            if (errorcodes.Count > 0)
            {
                throw new ValidationException(errorcodes);
            }

            Payment vendorpaymentDetails = await paymentDetailsTaskAsync;
            if (vendorpaymentDetails != null)
            {
                vendorpaymentDetails.PaymentDate = disbursementDate;
            }
            else
            {
                vendorpaymentDetails = new Payment {PaymentDate = disbursementDate};
            }

            TransactionScope scope = TransactionScopeBuilder.CreateReadCommitted();
            try
            {
                _applicationLocker.GetApplicationLock(summary.FundingDealID, errorcodes);

                summary=_disbursementSummaryRepository.GetDisbursementSummary(disbursements.First().FundingDealID);

                if (summary.FundingMilestone.Disbursed > DateTime.MinValue)
                {
                    errorcodes.Add(ErrorCode.DealDisbursed);
                }
                if (totalDepositsReceived != request.DepositAmountReceived && 
                    !errorcodes.Contains(ErrorCode.ConcurrencyCheckFailed))
                {
                    errorcodes.Add(ErrorCode.ConcurrencyCheckFailed);
                }
                if (errorcodes.Count > 0)
                {
                    throw new ValidationException(errorcodes);
                }
                var paymentRequestsAsync = _paymentRequestProcessor.ProcessPaymentRequests(disbursements, fundingDeal,
                    vendorpaymentDetails, request.UserContext);

                updatedMilestone = _fundedRepository.UpdateDisbursedMilestone(summary.FundingDealID);

                var paymentRequests = await paymentRequestsAsync;

                var transferRequest = new PaymentTransferRequest()
                {
                    PaymentRequests = paymentRequests
                };

                PaymentTransferResponse paymentTransferResponse =
                    _paymentTransferService.SubmitPaymentRequest(transferRequest);

                if (paymentTransferResponse != null && paymentTransferResponse.RequestStatus == EPSStatus.Received)
                {                  
                    //retract pending returns if any
                    var returnedFunds =
                        _dealFundAllocRepository.SearchReturns(AllocationStatus.PendingAckowledgement,
                            RecordType.Return, summary.FundingDealID);

                    if (returnedFunds.Any())
                    {
                        _dealFundAllocRepository.UpdateStatus(returnedFunds, AllocationStatus.Retracted);
                    }

                    //Update DealStatus 
                    if (fundingDeal.BusinessModel.Equals(BusinessModel.EASYFUND))
                    {
                        _fundingDealRepository.UpdateDealStatus(DealStatus.Complete,
                            fundingDeal.DealID.GetValueOrDefault());
                    }

                    FundingDeal otherfundingDeal = null;
                    if (fundingDeal.ActingFor != LawyerActingFor.Both && fundingDeal.ActingFor!=LawyerActingFor.Mortgagor)
                    {
                        int otherDealId =
                            _fundingDealRepository.GetOtherDealInScope(fundingDeal.DealID.GetValueOrDefault());

                        //Update DealStatus of other deal, if it is not LLC
                        otherfundingDeal = _fundingDealRepository.GetFundingDeal(otherDealId);
                        if (otherfundingDeal.BusinessModel.Equals(BusinessModel.EASYFUND))
                        {
                            _fundingDealRepository.UpdateDealStatus(DealStatus.Complete,
                                otherfundingDeal.DealID.GetValueOrDefault());
                        }
                        //update dealhistories
                        _dealHistoryRepository.CreateDealHistory(otherfundingDeal.DealID.GetValueOrDefault(),
                            request.UserContext,
                            DealActivity.EFDealDisbursed);
                    }
                    _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(),
                        request.UserContext, DealActivity.EFDealDisbursed);

                    // Update COnfirm Closing flag and log it in Deal History for Purchaser Lawyer    
                    if(fundingDeal.BusinessModel.Contains(BusinessModel.MMS))
                    {
                        _fundingDealRepository.UpdateConfirmClosing(fundingDeal.DealID.GetValueOrDefault(), true);
                        _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(), request.UserContext, DealActivity.ConfirmedClosing);

                        //Add history entry for Vendor Lawyer
                        //if (otherfundingDeal != null && otherfundingDeal.BusinessModel.Equals(BusinessModel.EASYFUND))
                        //{
                        //    _dealHistoryRepository.CreateDealHistory(otherfundingDeal.DealID.GetValueOrDefault(), request.UserContext, DealActivity.ConfirmedClosing);
                        //}
                    }

                }
                scope.Complete();

                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                scope.Dispose();
            }

            if (fundingDeal.ActingFor != LawyerActingFor.Both && fundingDeal.ActingFor != LawyerActingFor.Mortgagor)
            {
                SendDisbursementNotification(fundingDeal)
                    .ContinueWith(t => _logger.LogError(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
            }
            
            if (updatedMilestone != null)
            {
                response = new DisburseFundsResponse
                {
                    Disbursed = updatedMilestone.Disbursed.GetValueOrDefault()
                };
            }

            // Trigger Confirm Closing Event back to MMS Portal
            if (fundingDeal.BusinessModel.Contains(BusinessModel.MMS))
            {
                _dealEventsPublisherepository.PublishConfirmClosing((int)fundingDeal.DealID, fundingDeal.Lawyer.LawyerID);
            }

            return response;
        }

        private async Task SendDisbursementNotification(FundingDeal fundingDeal)
        {
            LawyerProfile receiverprofile =
                _lawyerRepository.GetNotificationDetails(fundingDeal.OtherLawyer.LawyerID);
            string recipientActingFor = fundingDeal.ActingFor == LawyerActingFor.Purchaser
                ? LawyerActingFor.Vendor
                : LawyerActingFor.Purchaser;

            //Get Email List including Delegates
            string mailingList = GetNotificationRecipients(fundingDeal);
            if (!string.IsNullOrEmpty(mailingList))
            {
                receiverprofile.Email = mailingList;
            }
            _emailHelper.SendStandardNotification(fundingDeal, receiverprofile, recipientActingFor,
                StandardNotificationKey.EFDealDisbursed);
        }

        private void UpdateDealHistories(IEnumerable<Disbursement> afterSavingDisbusements, int dealId, UserContext user)
        {
            var beforeSavingDisbursementsNoFee =
                _preSavingDisbursements.Where(p => p.PayeeType != EasyFundFee.FeeName).OrderBy(d => d.DisbursementID);
            var afterSavingDisbursementsNoFee =
                afterSavingDisbusements.Where(p => p.PayeeType != EasyFundFee.FeeName).OrderBy(d => d.DisbursementID);
            var varianceIgnoreproperties = new List<string>
            {
                "Amount",
                "DisbursementID",
                "ChainDealID",
                "VendorFee",
                "PurchaserFee",
                "Action",
                "Version",
                "TrustAccountID",
                "ExtensionData",
                "PayeeID"
            };

            var variances = VarianceChecker.GetVariances(beforeSavingDisbursementsNoFee, afterSavingDisbursementsNoFee,
                "Disbursement", varianceIgnoreproperties);

            foreach (var afterSavingDisbursement in afterSavingDisbursementsNoFee)
            {
                var history = new DisbursementHistory
                {
                    Amount = afterSavingDisbursement.Amount
                };
                if (string.IsNullOrEmpty(afterSavingDisbursement.PayeeName) &&
                    afterSavingDisbursement.PayeeType == FeeDistribution.VendorLawyer)
                {
                    history.Payee = FeeDistribution.VendorLawyer;
                }
                else
                {
                    history.Payee = afterSavingDisbursement.PayeeName;
                }

                switch (afterSavingDisbursement.Action)
                {
                    case CRUDAction.Create:
                        _dealHistoryRepository.CreateDealHistory(dealId, DealActivity.DisbursementCreated, user, history);
                        break;
                    case CRUDAction.Delete:
                        _dealHistoryRepository.CreateDealHistory(dealId, DealActivity.DisbursementRemoved, user, history);
                        break;
                    case CRUDAction.Update:

                        var preSavingRecord =
                            _preSavingDisbursements.SingleOrDefault(
                                d => d.DisbursementID == afterSavingDisbursement.DisbursementID);
                        if (preSavingRecord != null)
                        {
                            if (!string.IsNullOrWhiteSpace(preSavingRecord.PayeeName) &&
                                preSavingRecord.PayeeName != afterSavingDisbursement.PayeeName)
                            {
                                history.OldPayee = preSavingRecord.PayeeName;
                                _dealHistoryRepository.CreateDealHistory(dealId, DealActivity.EFPayeeEdited, user,
                                    history);
                            }
                            if (preSavingRecord.Amount != afterSavingDisbursement.Amount)
                            {
                                history.OldAmount = preSavingRecord.Amount;
                                _dealHistoryRepository.CreateDealHistory(dealId, DealActivity.DisbursementEdited, user,
                                    history);
                            }
                            //Disbursement is not active anymore : Saman
                            if (preSavingRecord.DisbursementStatus != afterSavingDisbursement.DisbursementStatus && afterSavingDisbursement.DisbursementStatus == "Draft")
                            {
                                history.OldDisbursementStatus = preSavingRecord.DisbursementStatus;
                                _dealHistoryRepository.CreateDealHistory(dealId, DealActivity.DisbursementIsDraft, user,
                                    history);
                            }
                            //Disbursement is active now : Saman
                            if (preSavingRecord.DisbursementStatus != afterSavingDisbursement.DisbursementStatus && afterSavingDisbursement.DisbursementStatus != "Draft")
                            {
                                history.OldDisbursementStatus = preSavingRecord.DisbursementStatus;
                                _dealHistoryRepository.CreateDealHistory(dealId, DealActivity.DisbursementIsActive, user,
                                    history);
                            }
                            if (DisbursementHelper.HasCreditCardNumberChanged(preSavingRecord, afterSavingDisbursement))
                            {
                                history.OldValue = preSavingRecord.ReferenceNumber;
                                history.NewValue = afterSavingDisbursement.ReferenceNumber;
                                _dealHistoryRepository.CreateDealHistory(dealId, DealActivity.CreditCardEdited, user,history);
                            }
                        }

                        break;
                }
            }
            if (variances != null && variances.Count > 0)
            {
                _dealHistoryRepository.CreateDealHistories(dealId, ResourceSet.BusinessServiceDisbursementSet,
                    variances.Values, user);
            }
        }

        private string GetNotificationRecipients(FundingDeal _fundingDeal)
        {
            int OtherDealID = _fundingDealRepository.GetOtherDealInScope(_fundingDeal.DealID.Value);

            var mailerDetails = _emailHelper.GetEmailRecipientList(_fundingDeal, OtherDealID);

            string sEmailRecipients = mailerDetails.GetValueOrDefault().Value;

            return sEmailRecipients;

        }

        /// <summary>
        /// Ensure that the specified Disbursement Type has not already been added to the deal
        /// This check is necessary because certain Disbursement Types (Ie. 'Vendor Lawyer', 'EasyFund Fee') can only be added to the Deal once
        /// </summary>
        private void CheckConstraint(DisbursementCollection disbursementsToSave, string disbursementType)
        {
            var vendorLawyerDisbursementExists = ((_preSavingDisbursements != null) && (_preSavingDisbursements.Any(x => (x.PayeeType == disbursementType))));

            if (vendorLawyerDisbursementExists)
            {
                if (disbursementsToSave.Any(x => (x.PayeeType == disbursementType) && (x.Action == CRUDAction.Create)))
                {
                    throw new DBConcurrencyException(string.Format("Deal already has a {0} disbursement.", disbursementType));
                }
            }
        }
    }
}
