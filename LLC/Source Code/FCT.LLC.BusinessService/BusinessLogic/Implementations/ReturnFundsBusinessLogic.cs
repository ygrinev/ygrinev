using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;
using UserContext = FCT.LLC.Common.DataContracts.UserContext;

namespace FCT.LLC.BusinessService.BusinessLogic.Implementations
{
    public class ReturnFundsBusinessLogic : IReturnFundsBusinessLogic
    {
        private readonly IValidationHelper _validationHelper;
        private readonly IFundedRepository _fundedRepository;
        private readonly IFundingDealRepository _fundingDealRepository;
        private readonly IDealFundsAllocRepository _dealFundsAllocRepository;
        private readonly IFeeRepository _feeRepository;
        private readonly IPaymentRequestProcessor _paymentRequestProcessor;
        private readonly IPaymentTransferService _paymentTransferService;
        private readonly ILawyerRepository _lawyerRepository;
        private readonly IReturnFundsHelper _returnFundsHelper;
        private readonly IDisbursementSummaryRepository _disbursementSummaryRepository;
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly IMilestoneUpdater _milestoneUpdater;
        private readonly IApplicationLocker _applicationLocker;
        private readonly IEmailHelper _emailHelper;

        private bool _confirmDepositReceived=true;


        public ReturnFundsBusinessLogic(IValidationHelper validationHelper, IFundedRepository fundedRepository,
            IFundingDealRepository fundingDealRepository, IDealFundsAllocRepository dealFundsAllocRepository,
            IFeeRepository feeRepository, IPaymentRequestProcessor paymentRequestProcessor,
            IPaymentTransferService paymentTransferService, ILawyerRepository lawyerRepository,
            IReturnFundsHelper returnFundsHelper, IDisbursementSummaryRepository disbursementSummaryRepository,
            IDealHistoryRepository dealHistoryRepository, IMilestoneUpdater milestoneUpdater, IApplicationLocker applicationLocker, IEmailHelper emailHelper)
        {
            _validationHelper = validationHelper;
            _dealFundsAllocRepository = dealFundsAllocRepository;
            _fundedRepository = fundedRepository;
            _fundingDealRepository = fundingDealRepository;
            _feeRepository = feeRepository;
            _paymentRequestProcessor = paymentRequestProcessor;
            _paymentTransferService = paymentTransferService;
            _lawyerRepository = lawyerRepository;
            _returnFundsHelper = returnFundsHelper;
            _disbursementSummaryRepository = disbursementSummaryRepository;
            _dealHistoryRepository = dealHistoryRepository;
            _milestoneUpdater = milestoneUpdater;
            _applicationLocker = applicationLocker;
            _emailHelper = emailHelper;
        }

        public GetPendingFundsReturnResponse GetPendingFundsReturn(GetPendingFundsReturnRequest request)
        {
            var fundedDeal = _fundedRepository.GetMilestonesByDeal(request.DealID);

            var disbursementSummary = _disbursementSummaryRepository.GetDisbursementSummary(fundedDeal.FundingDealId);

            var returnedFunds = _dealFundsAllocRepository.SearchReturns(AllocationStatus.PendingAckowledgement,
                RecordType.Return, fundedDeal.FundingDealId);

            DealFundsAllocation feeFund = null;
            DealFundsAllocation fundsToReturn = null;

            foreach (var dealFundsAllocation in returnedFunds)
            {
                if (dealFundsAllocation.RecordType == RecordType.FCTFee)
                {
                    feeFund = dealFundsAllocation;
                }
                else
                {
                    fundsToReturn = dealFundsAllocation;
                }
            }
            if (fundsToReturn != null)
            {
                var response = new GetPendingFundsReturnResponse()
                {
                    DealID = request.DealID,
                    DisbursementSummary = disbursementSummary
                };

                if (feeFund != null)
                {
                    fundsToReturn.Amount = fundsToReturn.Amount + feeFund.Amount;
                    fundsToReturn.Fee = feeFund.Fee;
                    response.ReturnFCTFee = feeFund.Fee.Amount;
                }
                response.PendingFundsReturn = fundsToReturn;

                return response;
            }

            var noFundsResponse = new GetPendingFundsReturnResponse()
            {
                DealID = request.DealID,
                DisbursementSummary = disbursementSummary,
                ReturnFCTFee = _feeRepository.GetBaseFeeFromConfiguration(EasyFundFee.FCTReturnFundsFee)
            };
            return noFundsResponse;
        }

        public void SavePendingFundsReturn(SavePendingFundsReturnRequest request)
        {
            var errorcodes = new List<ErrorCode>();

            var fundedDeal = _fundedRepository.GetMilestonesByDeal(request.DealID);

                request.PendingFundsReturn.FundingDealID = fundedDeal.FundingDealId;

                decimal availableFunds = _dealFundsAllocRepository.GetTotalAllocatedFunds(fundedDeal.FundingDealId);

                var deal = _fundingDealRepository.GetFundingDeal(request.DealID);
                if (deal != null)
                {
                    Fee fee;
                    int otherDealId;

                    switch (deal.ActingFor)
                    {
                        case LawyerActingFor.Purchaser:

                            if (IsRetractionRequest(request))
                            {

                                RetractRequest(request, errorcodes, fundedDeal, deal);
                                _confirmDepositReceived = false;

                                if (errorcodes.Count > 0)
                                {
                                    throw new ValidationException(errorcodes);
                                }
                            }
                            else
                            {
                                fee = GetFee(deal, availableFunds, errorcodes);

                                ValidateRequest(request, fee, availableFunds, errorcodes);                               

                                if (fundedDeal.Milestone.SignedByVendor == null ||
                                    fundedDeal.Milestone.SignedByVendor <= DateTime.MinValue)
                                {
                                    ReturnFundsVendorAcknowledgementNotNeeded(request, errorcodes, fee, deal, fundedDeal, availableFunds);
                                }
                                else
                                {
                                    decimal depositRequired = _disbursementSummaryRepository.GetDepositRequired(fundedDeal.FundingDealId);

                                    if (availableFunds != depositRequired)
                                    {
                                        ReturnFundsVendorAcknowledgementNotNeeded(request, errorcodes, fee, deal,
                                            fundedDeal, availableFunds);
                                    }
                                    else
                                    {
                                        otherDealId =
                                            _fundingDealRepository.GetOtherDealInScope(deal.DealID.GetValueOrDefault());

                                        using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                                        {
                                            _applicationLocker.GetApplicationLock(fundedDeal.FundingDealId, errorcodes);
                                            var pendingFunds =
                                                _dealFundsAllocRepository.SearchReturns(
                                                    AllocationStatus.PendingAckowledgement,
                                                    RecordType.Return, fundedDeal.FundingDealId);
                                            if (pendingFunds.Any())
                                            {
                                                errorcodes.Add(ErrorCode.ConcurrencyCheckFailed);
                                            }
                                            ValidateConcurrency(request, errorcodes);
                                            CreateVendorAcknowledgementRequest(request, fee, deal, otherDealId);
                                            scope.Complete();

                                        }
                                        _confirmDepositReceived = false;

                                        EmailRequestToVendorLawyer(request, deal);
                                    }
                                }
                            }
                            break;

                        case LawyerActingFor.Both:
                        case LawyerActingFor.Mortgagor:

                            fee = GetFee(deal, availableFunds, errorcodes);

                            ValidateRequest(request, fee, availableFunds, errorcodes);                               

                            ReturnFundsVendorAcknowledgementNotNeeded(request, errorcodes, fee, deal, fundedDeal, availableFunds);
                            break;

                        case LawyerActingFor.Vendor:
                        
                            var vendorUserContext = new UserContext() 
                            {
                                UserID = request.UserContext.UserID, 
                                UserType = request.UserContext.UserType
                            };

                            otherDealId = _fundingDealRepository.GetOtherDealInScope(deal.DealID.GetValueOrDefault());

                            if (request.UserContext.UserType != Common.DataContracts.UserType.FCTAdmin)
                            {
                                var purchaserLawyerDetails =
                                    _lawyerRepository.GetNotificationDetails(deal.OtherLawyer.LawyerID);

                                request.UserContext.UserID = purchaserLawyerDetails.UserName;
                            }
                            
                            DateTime disbursementDate = ValidatePaymentDate(request.UserContext, errorcodes);

                            bool removeSignatures = RemoveSignatures(fundedDeal, availableFunds);
                            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                            {
                                _applicationLocker.GetApplicationLock(fundedDeal.FundingDealId, errorcodes);

                                IEnumerable<DealFundsAllocation> returnedFunds =
                                    _dealFundsAllocRepository.SearchReturns(AllocationStatus.PendingAckowledgement,
                                        RecordType.Return, fundedDeal.FundingDealId);

                                ValidateRequestOnVendorAcknowledgement(returnedFunds, availableFunds, errorcodes);

                                ValidateConcurrency(request, errorcodes);

                                ReturnFundsOnVendorAcknowledgement(request, returnedFunds, deal, vendorUserContext,
                                    otherDealId, disbursementDate, removeSignatures, fundedDeal);

                                scope.Complete();
                            }                        

                            EmailConfirmationToPurchaserLawyer(request, deal);

                            break;
                    }
                    if(_confirmDepositReceived)
                    EmailDepositReceivedToVendorLawyer(deal);
                }          
        }

        private void EmailDepositReceivedToVendorLawyer(FundingDeal deal)
        {
            if (deal.ActingFor != LawyerActingFor.Both &&
                deal.ActingFor != LawyerActingFor.Mortgagor)
            {
                LawyerProfile vendorLawyerProfile = _lawyerRepository.GetNotificationDetails(deal.OtherLawyer.LawyerID);
                var updatedFundedDeal =
                    _fundedRepository.GetMilestonesByDeal(deal.DealID.GetValueOrDefault());
                int otherDealId = _fundingDealRepository.GetOtherDealInScope(deal.DealID.GetValueOrDefault());
                var emailList = _emailHelper.GetEmailRecipientList(deal, otherDealId);

                if (updatedFundedDeal.Milestone.SignedByPurchaser > DateTime.MinValue &&
                    updatedFundedDeal.Milestone.SignedByVendor > DateTime.MinValue &&
                    updatedFundedDeal.Milestone.Funded > DateTime.MinValue)
                {
                    FundsAllocationHelper.SendNotificationEmail(deal, vendorLawyerProfile,
                        StandardNotificationKey.EFDepositVendor, emailList.GetValueOrDefault().Value);
                }
            }
        }

        private bool RemoveSignatures(FundedDeal fundedDeal, decimal availableFunds)
        {
            decimal depositRequired = _disbursementSummaryRepository.GetDepositRequired(fundedDeal.FundingDealId);

            if (availableFunds == depositRequired)
            {
                return true; 
            }
            return false;
        }

        private void EmailConfirmationToPurchaserLawyer(SavePendingFundsReturnRequest request, FundingDeal deal)
        {
            LawyerProfile recipientProfile = _lawyerRepository.GetNotificationDetails(deal.OtherLawyer.LawyerID);

            _returnFundsHelper.SendEmailNotification(deal, recipientProfile, LawyerActingFor.Purchaser,
                StandardNotificationKey.ReturnFundsAcknowledged, request.PendingFundsReturn.Amount);
        }

        private void ReturnFundsOnVendorAcknowledgement(SavePendingFundsReturnRequest request,
            IEnumerable<DealFundsAllocation> returnedFunds,FundingDeal deal, UserContext vendorUserContext, 
            int otherDealId, DateTime disbursementDate,bool removeSignatures,FundedDeal fundedDeal)
        {
            _dealFundsAllocRepository.UpdateStatus(returnedFunds, AllocationStatus.Acknowledged);

            _dealHistoryRepository.CreateDealHistory(deal.DealID.GetValueOrDefault(),
                DealActivity.ReturnFundsVendorAcknowledged, vendorUserContext,
                request.PendingFundsReturn.Amount);

            _dealHistoryRepository.CreateDealHistory(otherDealId,
                DealActivity.ReturnFundsVendorAcknowledged,
                vendorUserContext, request.PendingFundsReturn.Amount);

            ProcessReturns(request, returnedFunds, deal, disbursementDate);

            //Remove all signatures only if deal is exactly funded and funds are requested to returned
            if (removeSignatures)
            {
                UpdateAllMilestones(fundedDeal);
            }
        }

        private static void ValidateRequestOnVendorAcknowledgement(IEnumerable<DealFundsAllocation> returnedFunds,  
            decimal availableFunds, List<ErrorCode> errorcodes)
        {
            decimal requestedAmount = 0;
            decimal feeWithTaxes = 0;
            foreach (var returnedFund in returnedFunds)
            {
                if (returnedFund.RecordType == RecordType.FCTFee)
                {
                    feeWithTaxes = returnedFund.Amount;
                }
                if (returnedFund.RecordType == RecordType.Return)
                {
                    requestedAmount = requestedAmount + returnedFund.Amount;
                }
            }

            if (requestedAmount + feeWithTaxes > availableFunds)
            {
                errorcodes.Add(ErrorCode.InSufficientFunds);
            }
        }

        private void EmailRequestToVendorLawyer(SavePendingFundsReturnRequest request, FundingDeal deal)
        {
            LawyerProfile recipientProfile = _lawyerRepository.GetNotificationDetails(deal.OtherLawyer.LawyerID);

            _returnFundsHelper.SendEmailNotification(deal, recipientProfile,
                LawyerActingFor.Vendor,
                StandardNotificationKey.ReturnFundsRequested,
                request.PendingFundsReturn.Amount);
        }

        private void CreateVendorAcknowledgementRequest(SavePendingFundsReturnRequest request, Fee fee, FundingDeal deal,
            int otherDealId)
        {
            CreateReturnRecords(request.PendingFundsReturn, fee,
                AllocationStatus.PendingAckowledgement);

            _dealHistoryRepository.CreateDealHistory(deal.DealID.GetValueOrDefault(),
                DealActivity.ReturnFundsRequested, request.UserContext,
                request.PendingFundsReturn.Amount);

            _dealHistoryRepository.CreateDealHistory(otherDealId,
                DealActivity.ReturnFundsRequested,
                request.UserContext, request.PendingFundsReturn.Amount);
        }

        private static void ValidateRequest(SavePendingFundsReturnRequest request, Fee fee, decimal depositReceived,
            List<ErrorCode> errorcodes)
        {
            decimal feeWithTaxes = fee.Amount + fee.HST + fee.GST + fee.QST;

            if (request.PendingFundsReturn.Amount + feeWithTaxes > depositReceived)
            {
                errorcodes.Add(ErrorCode.InSufficientFunds);
            }

            if (depositReceived != request.DepositAmountReceived)
            {
                errorcodes.Add(ErrorCode.ConcurrencyCheckFailed);
            }
            if (errorcodes.Count > 0)
            {
                throw new ValidationException(errorcodes);
            }
            
        }

        private static bool IsRetractionRequest(SavePendingFundsReturnRequest request)
        {
            return !string.IsNullOrWhiteSpace(request.PendingFundsReturn.AllocationStatus) &&
                   request.PendingFundsReturn.AllocationStatus == AllocationStatus.Retracted &&
                   request.PendingFundsReturn.DealFundsAllocationID.GetValueOrDefault() > 0;
        }

        private void ReturnFundsVendorAcknowledgementNotNeeded(SavePendingFundsReturnRequest request,
            List<ErrorCode> errorcodes, Fee fee,FundingDeal deal, FundedDeal fundedDeal, decimal availableFunds)
        {
            DateTime disbursementDate = ValidatePaymentDate(request.UserContext, errorcodes);            
            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                _applicationLocker.GetApplicationLock(fundedDeal.FundingDealId, errorcodes);

                ValidateConcurrency(request, errorcodes);

                IEnumerable<DealFundsAllocation> returnedFunds = CreateReturnRecords(request.PendingFundsReturn, fee,
                    AllocationStatus.Acknowledged);

                ProcessReturns(request, returnedFunds, deal, disbursementDate);

                _milestoneUpdater.UpdateMilestoneDepositReceived(fundedDeal);

                scope.Complete();
            }
        }

        private DateTime ValidatePaymentDate(UserContext userContext, List<ErrorCode> errorcodes)
        {
            var disbursementDate=_validationHelper.ValidatePaymentDate(userContext, errorcodes);
            if (errorcodes.Count > 0)
            {
                throw new ValidationException(errorcodes);
            }
            return disbursementDate;
        }

        private void RetractRequest(SavePendingFundsReturnRequest request, List<ErrorCode> errorcodes,
            FundedDeal fundedDeal, FundingDeal deal)
        {
            var retractedFund =
                _dealFundsAllocRepository.GetDealFundsAllocation(
                    request.PendingFundsReturn.DealFundsAllocationID.GetValueOrDefault());

            if (retractedFund != null)
            {
                var returnedFunds = _dealFundsAllocRepository.SearchReturns(AllocationStatus.PendingAckowledgement,
                    RecordType.Return, fundedDeal.FundingDealId);

                int otherDealId = _fundingDealRepository.GetOtherDealInScope(deal.DealID.GetValueOrDefault());

                using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                {
                    _applicationLocker.GetApplicationLock(fundedDeal.FundingDealId, errorcodes);

                    if (retractedFund.AllocationStatus == AllocationStatus.Disbursed)
                    {
                        errorcodes.Add(ErrorCode.VendorAcknowledged);
                    }

                    ValidateConcurrency(request, errorcodes);

                    _dealFundsAllocRepository.UpdateStatus(returnedFunds, AllocationStatus.Retracted);

                    _milestoneUpdater.UpdateMilestoneDepositReceived(fundedDeal);

                    _dealHistoryRepository.CreateDealHistory(deal.DealID.GetValueOrDefault(),
                        DealActivity.ReturnFundsRequestRetracted, request.UserContext,
                        request.PendingFundsReturn.Amount);

                    _dealHistoryRepository.CreateDealHistory(otherDealId, DealActivity.ReturnFundsRequestRetracted,
                        request.UserContext, request.PendingFundsReturn.Amount);

                    scope.Complete();
                }

                var recipientProfile = _lawyerRepository.GetNotificationDetails(deal.OtherLawyer.LawyerID);

                _returnFundsHelper.SendEmailNotification(deal, recipientProfile, LawyerActingFor.Vendor,
                    StandardNotificationKey.ReturnFundsRequestRetracted, request.PendingFundsReturn.Amount);
            }
        }

        private void ValidateConcurrency(SavePendingFundsReturnRequest request, List<ErrorCode> errorcodes)
        {
            var updatedFundedDeal = _fundedRepository.GetMilestonesByDeal(request.DealID);
            decimal availableFunds = _dealFundsAllocRepository.GetTotalAllocatedFunds(updatedFundedDeal.FundingDealId);

            if (updatedFundedDeal.Milestone.Disbursed > DateTime.MinValue)
            {
                errorcodes.Add(ErrorCode.DealDisbursed);
            }
            if (availableFunds != request.DepositAmountReceived &&
                !errorcodes.Contains(ErrorCode.ConcurrencyCheckFailed))
            {
                errorcodes.Add(ErrorCode.ConcurrencyCheckFailed);
            }
            if (errorcodes.Count > 0)
            {
                throw new ValidationException(errorcodes);
            }
        }

        private void ProcessReturns(SavePendingFundsReturnRequest request, IEnumerable<DealFundsAllocation> dealFunds,
            FundingDeal deal,DateTime disbursementDate)
        {
            _dealFundsAllocRepository.UpdateStatus(dealFunds, AllocationStatus.Disbursed);

            var paymentRequests = _paymentRequestProcessor.ProcessPaymentRequests(dealFunds, deal,
                disbursementDate,
                request.UserContext);
            var transferRequest = new PaymentTransferRequest()
            {
                PaymentRequests = paymentRequests
            };
            var transferResponse = _paymentTransferService.SubmitPaymentRequest(transferRequest);
            if (transferResponse != null && transferResponse.RequestStatus == EPSStatus.Received)
            {

                _dealHistoryRepository.CreateDealHistory(deal.DealID.GetValueOrDefault(),
                    DealActivity.ReturnFundsProcessed, request.UserContext, request.PendingFundsReturn.Amount);

                if (deal.ActingFor != LawyerActingFor.Both && deal.ActingFor!=LawyerActingFor.Mortgagor)
                {
                    int otherDealId = _fundingDealRepository.GetOtherDealInScope(deal.DealID.GetValueOrDefault());

                    _dealHistoryRepository.CreateDealHistory(otherDealId, DealActivity.ReturnFundsProcessed,
                        request.UserContext, request.PendingFundsReturn.Amount);
                }
            }
        }

        private void UpdateAllMilestones(FundedDeal milestones)
        {           
            milestones.Milestone.SignedByPurchaser = null;
            milestones.Milestone.SignedByVendor = null;
            _milestoneUpdater.UpdateMilestoneDepositReceived(milestones);
        }

        private Fee GetFee(FundingDeal deal, decimal availableFunds, List<ErrorCode> errorcodes)
        {
            string province = deal.Property.Province;
            Fee fee = _feeRepository.CalculateFee(province);

            decimal totalFee = fee.Amount + fee.GST + fee.HST + fee.QST;
            if (totalFee > availableFunds)
            {
                errorcodes.Add(ErrorCode.InSufficientFunds);
            }
            return fee;
        }

        private IEnumerable<DealFundsAllocation> CreateReturnRecords(DealFundsAllocation funds, Fee fee, string status)
        {
            var dealFunds = new List<DealFundsAllocation>();

            funds.AllocationStatus = status;
            funds.RecordType = RecordType.Return;
            dealFunds.Add(funds);

            if (fee.Amount > 0)
            {
                int feeId = _feeRepository.SaveFee(fee);
                var feeFunds = new DealFundsAllocation
                {
                    Fee = new Fee() {FeeID = feeId},
                    Amount = fee.Amount + fee.GST + fee.HST + fee.QST,
                    RecordType = RecordType.FCTFee,
                    FundingDealID = funds.FundingDealID,
                    AllocationStatus = status,
                    ReferenceNumber = funds.ReferenceNumber
                };
                dealFunds.Add(feeFunds);
            }
            var insertedFunds = _dealFundsAllocRepository.InsertDealFundsAllocationRange(dealFunds);
            return insertedFunds;
        }
    }
}
