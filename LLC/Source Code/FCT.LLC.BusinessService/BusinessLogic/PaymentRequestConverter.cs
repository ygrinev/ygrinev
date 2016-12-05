using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PayeeService.Contracts;
using FCT.EPS.PayeeService.DataContracts;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;
using FCT.Services.LIM.DataContracts;
using FCT.Services.LIM.MessageContracts;
using FCT.Services.LIM.ServiceContracts;
using Account = FCT.EPS.PaymentService.DataContracts.Account;
using Address = FCT.EPS.PaymentService.DataContracts.Address;
using PayeeInfo = FCT.EPS.PaymentService.DataContracts.PayeeInfo;
using PayeeType = FCT.LLC.Common.DataContracts.PayeeType;
using UserContext = FCT.LLC.Common.DataContracts.UserContext;


namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class PaymentRequestConverter:IPaymentRequestConverter
    {
        private readonly ILIMServiceContract _limService;
        private readonly IEPSPayeeService _payeeService;
        private readonly IDealScopeRepository _dealScopeRepository;
        private readonly IValidationHelper _validationHelper;
        private readonly IPaymentReportDetailsCollector _paymentReportDetailsCollector;

        private const string EPSSubscriptionID = "EPSSubscriptionID";
        private const string FCTPayeeName = "002";
        private const string CanadianClearingCodePrefix = "CC0";

        public PaymentRequestConverter(ILIMServiceContract limService,
            IDealScopeRepository dealScopeRepository, IEPSPayeeService payeeService, IValidationHelper validationHelper, IPaymentReportDetailsCollector paymentReportDetailsCollector)
        {
            _limService = limService;
            _payeeService = payeeService;
            _dealScopeRepository = dealScopeRepository;
            _validationHelper = validationHelper;
            _paymentReportDetailsCollector = paymentReportDetailsCollector;
        }

        public async Task<IDictionary<int, PaymentRequest>> ConvertToPaymentRequests(DisbursementCollection disbursements, FundingDeal fundingDeal,
            Payment payment, Common.DataContracts.UserContext userContext)
        {           
            var paymentDictionary = new Dictionary<int, PaymentRequest>();
         
            foreach (var disbursement in disbursements)
            {
                if (disbursement.PayeeType!=EasyFundFee.FeeName && disbursement.PayeeType!=FeeDistribution.VendorLawyer)
                {
                    paymentDictionary.Add(disbursement.DisbursementID.GetValueOrDefault(),MapToPaymentRequest(disbursement, fundingDeal, payment.PaymentDate, userContext));
                }
                if (disbursement.PayeeType == FeeDistribution.VendorLawyer && disbursement.Amount>0 && disbursement.DisbursedAmount>0)
                {
                    paymentDictionary.Add(disbursement.DisbursementID.GetValueOrDefault(),GetVendorPaymentRequest(disbursement, userContext, payment, fundingDeal.FCTURN));
                }
            }
            return paymentDictionary;
        }

        public IDictionary<int, PaymentRequest> ConvertToPaymentRequests(IEnumerable<DealFundsAllocation> dealFundsAllocations, FundingDeal fundingDeal, DateTime paymentDate, Common.DataContracts.UserContext userContext)
        {
            var dictionary = new Dictionary<int, PaymentRequest>();
            UserProfile lawyerProfile=null;
            foreach (var dealFundsAllocation in dealFundsAllocations)
            {
                if (dealFundsAllocation.RecordType != RecordType.FCTFee)
                {
                    var paymentRequest = MapBaseRequest(fundingDeal.FCTURN, paymentDate, userContext);
                    paymentRequest.PaymentMethodName = PaymentMethod.Wire;
                    paymentRequest.Amount = dealFundsAllocation.Amount;
                    lawyerProfile =
                        _validationHelper.GetActiveLawyer(dealFundsAllocation.TrustAccountID.GetValueOrDefault());
                    var payment = new Payment
                    {
                        LawyerProfile = lawyerProfile,
                        LawyerTrustAccountId = dealFundsAllocation.TrustAccountID.GetValueOrDefault(),
                        PaymentDate = paymentDate
                    };
                    if (paymentRequest.Payee == null)
                    {
                        paymentRequest.Payee = new PayeeInfo();
                    }
                    paymentRequest.Payee.PayeeName = lawyerProfile.FullName;
                    var lawyerAddress = lawyerProfile.Addresses.FirstOrDefault();
                    paymentRequest.Payee.PayeeAddress = PaymentServiceMapper.MapAddress(lawyerAddress);
                    AssignLawyerInfo(payment, paymentRequest);
                    if (paymentRequest.Payee.PayeeAccount != null)
                    {
                        paymentRequest.Payee.PayeeAccount.CanadianClearingCode =
                            GetCanadianClearingCode(paymentRequest.Payee.PayeeAccount);
                    }

                    dictionary.Add(dealFundsAllocation.DealFundsAllocationID.GetValueOrDefault(), paymentRequest); 
                   
                }
                else
                {
                    if (dealFundsAllocation.Fee != null && lawyerProfile!=null)
                    {
                        var fee = dealFundsAllocation.Fee;
                        var feePaymentRequest = MapBaseRequest(fundingDeal.FCTURN, paymentDate, userContext);
                        if (feePaymentRequest.Payee == null)
                        {
                            feePaymentRequest.Payee = new PayeeInfo();
                        }
                        feePaymentRequest.Payee.PayeeName = FCTPayeeName;
                        feePaymentRequest = GetFeePaymentRequest(fee, lawyerProfile.UserID, feePaymentRequest,
                            fundingDeal.Property.Province, FinanceServiceCode.ReturnOfFunds);
                        dictionary.Add(dealFundsAllocation.DealFundsAllocationID.GetValueOrDefault(), feePaymentRequest);
                    }
                }
            }                                  
            return dictionary;
        }

        internal PaymentRequest MapToPaymentRequest(Disbursement disbursement, FundingDeal fundingDeal,
            DateTime paymentRequestdate, Common.DataContracts.UserContext userContext)
        {
            
            var payment = MapDisbursementBaseRequest(disbursement, fundingDeal.FCTURN, paymentRequestdate, userContext);
         
            switch (payment.PaymentMethodName)
            {
                case PaymentMethod.Cheque:                
                    payment.Payee = PaymentServiceMapper.MapPayeeInfo(disbursement);
                    break;
                
                case PaymentMethod.Wire:
                case PaymentMethod.EFT:               
                    MapToElectronicTypePayments(disbursement, payment);

                    if (payment.PaymentRequestTypeName == PaymentRequestType.Batch)
                    {
                        payment.BatchPaymentReport =
                            _paymentReportDetailsCollector.GetBatchPaymentReportDetails(disbursement);
                    }

                    if (string.IsNullOrWhiteSpace(payment.Payee.PayeeAccount.CanadianClearingCode) || 
                        payment.Payee.PayeeAccount.CanadianClearingCode.Equals("NULL", StringComparison.OrdinalIgnoreCase))
                    {
                        payment.Payee.PayeeAccount.CanadianClearingCode = GetCanadianClearingCode(payment.Payee.PayeeAccount);
                    }
                    break;

                case PaymentMethod.Electronic_Delivery:
                    MapToElectronicTypePayments(disbursement, payment);
                    break;
            }
            payment.Payee.DebtorName = PaymentServiceMapper.MapDebtorName(fundingDeal, disbursement.PayeeType);
            return payment;
        }

        private void MapToElectronicTypePayments(Disbursement disbursement, PaymentRequest payment)
        {
            if (disbursement.PayeeType != FeeDistribution.VendorLawyer)
            {
                var payeeRequest = new GetPayeeRequest {PayeeInfoID = disbursement.PayeeID.GetValueOrDefault()};

                var payeeInfo = _payeeService.GetPayee(payeeRequest);
                if (payeeInfo != null && payeeInfo.PayeeInfo != null)
                {
                    payment.Payee = PaymentServiceMapper.MapPayeeInfo(payeeInfo.PayeeInfo);
                    payment.Payee.PayeeReferenceNumber = disbursement.ReferenceNumber;
                    payment.PaymentRequestTypeName =
                        ConvertToEPSPaymentRequest(payeeInfo.PayeeInfo.PayeeRequestTypeName);
                    if (disbursement.PayeeType == PayeeType.CreditCard)
                    {
                        if (payment.Payee.PayeeAccount == null)
                        {
                           payment.Payee.PayeeAccount=new Account(); 
                        }
                        payment.Payee.PayeeAccount.TokenNumber = disbursement.Token;
                    }

                    if (disbursement.PayeeType.Equals(PayeeType.CreditCard, StringComparison.InvariantCultureIgnoreCase)
                        || disbursement.PayeeType.Equals(PayeeType.LineOfCredit, StringComparison.InvariantCultureIgnoreCase)
                        || disbursement.PayeeType.Equals(PayeeType.MunicipalorUtility, StringComparison.InvariantCultureIgnoreCase)
                       )
                    {
                        payment.Payee.PayeeBankAccountHolderName = disbursement.AccountHolderName;
                    }
                }
            }
        }

        private PaymentRequest MapDisbursementBaseRequest(Disbursement disbursement, string fctURN, DateTime paymentRequestdate,
            Common.DataContracts.UserContext userContext)
        {
            var payment = MapBaseRequest(fctURN, paymentRequestdate, userContext);
            payment.Amount = disbursement.Amount;
            payment.PaymentMethodName = ConvertToEPSPaymentMethod(disbursement.PaymentMethod);
            if (payment.PaymentMethodName == PaymentMethod.Electronic_Delivery)
            {
                payment.BPSwithWirePayment = true;
            }
            return payment;
        }

        private PaymentRequest MapBaseRequest(string fctURN, DateTime paymentRequestdate, Common.DataContracts.UserContext userContext)
        {
            string longFCTURN = _dealScopeRepository.GetFCTRefNumber(fctURN);
            var payment = new PaymentRequest()
            {
                FCTRefNumShort = fctURN,
                FCTRefNum = longFCTURN,
                PaymentRequestTypeName = PaymentRequestType.Single,
                PaymentRequestDate = paymentRequestdate,
                EPSSubscriptionID = int.Parse(ConfigurationManager.AppSettings[EPSSubscriptionID]),
                PayementRequestUserContext = new EPS.PaymentService.DataContracts.UserContext()
                {
                    RequestUserName = userContext.UserID,
                    RequestClientIP = FundsAllocationHelper.GetIPAddress()
                },
            };
            return payment;
        }

        internal PaymentRequest GetVendorPaymentRequest(Disbursement disbursement,
            Common.DataContracts.UserContext userContext, Payment payment, string FCTURN)
        {
            var paymentRequest = MapDisbursementBaseRequest(disbursement, FCTURN, payment.PaymentDate, userContext);
            paymentRequest.Amount = disbursement.DisbursedAmount.GetValueOrDefault();
            if (paymentRequest.Payee == null)
            {
                paymentRequest.Payee = new PayeeInfo();
            }
            AssignLawyerInfo(payment, paymentRequest);
            paymentRequest.Payee.PayeeName = disbursement.PayeeName;
            paymentRequest.Payee.PayeeAddress = PaymentServiceMapper.MapAddress(disbursement);
            if (paymentRequest.Payee.PayeeAccount != null)
            {
                paymentRequest.Payee.PayeeAccount.CanadianClearingCode =
                    GetCanadianClearingCode(paymentRequest.Payee.PayeeAccount);
            }
            return paymentRequest;
        }

        private void AssignLawyerInfo(Payment payment, PaymentRequest paymentRequest)
        {
            var trustAccountRequest = new GetTrustAccountsRequest() {UserID = payment.LawyerProfile.UserID};
            var trustAccountResponse = _limService.GetTrustAccounts(trustAccountRequest);
            var trustAccount = trustAccountResponse.TrustAccounts.SingleOrDefault(
                t => t.TrustAccountID == payment.LawyerTrustAccountId);

            if (trustAccount != null)
            {
                paymentRequest.Payee.PayeeAccount = new Account
                {
                    BankName = trustAccount.BankName,
                    BankNumber = trustAccount.BankNum,
                    TransitNumber = trustAccount.BranchNum,
                    AccountNumber = trustAccount.AccountNum,
                    //SWIFTBIC = trustAccount.SwiftCode
                };
                paymentRequest.Payee.PayeeBankAccountHolderName = trustAccount.HolderName;
                paymentRequest.Payee.PayeeContact = payment.LawyerProfile.FullName;
                paymentRequest.Payee.PayeeContactPhoneNumber = payment.LawyerProfile.Phone;
                paymentRequest.Payee.PayeeEmail = payment.LawyerProfile.Email;
                paymentRequest.Payee.PayeeAccount.BankAddress = new Address()
                {
                    PostalCode = trustAccount.BankPostalCode,
                    City = trustAccount.BankCity,
                    StreetAddress1 = trustAccount.BankAddress,
                    ProvinceCode = trustAccount.BankProvince
                };
            }
        }

        public IList<PaymentRequest> GetFeePaymentRequests(DisbursementCollection disbursements,
            FundingDeal fundingDeal, DateTime paymentDate, Common.DataContracts.UserContext userContext)
        {
            IList<PaymentRequest> list = null;
            switch (fundingDeal.ActingFor)
            {
                case LawyerActingFor.Purchaser:
                    list = AddFeePayments(disbursements, fundingDeal.OtherLawyer, fundingDeal.Lawyer, paymentDate, userContext, fundingDeal.FCTURN, fundingDeal.Property.Province);
                    break;
                case LawyerActingFor.Vendor:
                    list = AddFeePayments(disbursements, fundingDeal.Lawyer, fundingDeal.OtherLawyer, paymentDate, userContext, fundingDeal.FCTURN, fundingDeal.Property.Province);
                    break;
                case LawyerActingFor.Both:
                case LawyerActingFor.Mortgagor:
                    list = AddFeePayments(disbursements, fundingDeal.Lawyer, fundingDeal.Lawyer, paymentDate, userContext, fundingDeal.FCTURN, fundingDeal.Property.Province);
                    break;
            }

            return list;
        }

        private IList<PaymentRequest> AddFeePayments(IEnumerable<Disbursement> disbursements, Lawyer vendorLawyer,
            Lawyer purchaserLawyer, DateTime paymentDate, Common.DataContracts.UserContext userContext, string fctURN,
            string province)
        {
            var list = new List<PaymentRequest>();
            foreach (var disbursement in disbursements)
            {
                if (disbursement.PayeeType == EasyFundFee.FeeName)
                {
                    switch (disbursement.FCTFeeSplit)
                    {
                        case FeeDistribution.VendorLawyer:

                            if (disbursement.VendorFee != null)
                            {
                                var paymentRequest = GetBaseFeePaymentRequest(paymentDate, userContext, fctURN,
                                    disbursement);
                                paymentRequest = GetFeePaymentRequest(disbursement.VendorFee, vendorLawyer.LawyerID,
                                    paymentRequest, province, FinanceServiceCode.FCTServiceFee);
                                list.Add(paymentRequest);
                            }
                            break;
                        case FeeDistribution.PurchaserLawyer:
                            if (disbursement.PurchaserFee != null)
                            {
                                var paymentRequest = GetBaseFeePaymentRequest(paymentDate, userContext, fctURN,
                                    disbursement);
                                paymentRequest = GetFeePaymentRequest(disbursement.PurchaserFee,
                                    purchaserLawyer.LawyerID, paymentRequest, province, FinanceServiceCode.FCTServiceFee);
                                list.Add(paymentRequest);
                            }
                            break;
                        case FeeDistribution.SplitEqually:
                            if (disbursement.VendorFee != null)
                            {
                                var paymentRequest = GetBaseFeePaymentRequest(paymentDate, userContext, fctURN,
                                    disbursement);
                                var vendorPaymentRequest = GetFeePaymentRequest(disbursement.VendorFee,
                                    vendorLawyer.LawyerID, paymentRequest, province, FinanceServiceCode.FCTServiceFee);
                                list.Add(vendorPaymentRequest);
                            }
                            if (disbursement.PurchaserFee != null)
                            {
                                var paymentRequest = GetBaseFeePaymentRequest(paymentDate, userContext, fctURN,
                                    disbursement);
                                var purchaserPaymentRequest = GetFeePaymentRequest(disbursement.PurchaserFee,
                                    purchaserLawyer.LawyerID, paymentRequest, province, FinanceServiceCode.FCTServiceFee);
                                list.Add(purchaserPaymentRequest);
                            }
                            break;
                    }
                }
            }
            return list;
        }

        private PaymentRequest GetBaseFeePaymentRequest(DateTime paymentDate, UserContext userContext, string fctURN,
            Disbursement disbursement)
        {
            PaymentRequest paymentRequest = MapDisbursementBaseRequest(disbursement, fctURN, paymentDate, userContext);
            paymentRequest.PaymentRequestTypeName=PaymentRequestType.Batch;

            if (paymentRequest.Payee == null)
            {
                paymentRequest.Payee = new PayeeInfo();
            }
            paymentRequest.Payee.PayeeName = FCTPayeeName;
            return paymentRequest;
        }


        private PaymentRequest GetFeePaymentRequest(Fee fee, int lawyerID, PaymentRequest paymentRequest, string province, FinanceServiceCode code)
        {
            PaymentRequest feePaymentRequest = paymentRequest;
            var userprofileRequest = new GetUserProfileByUserIDRequest
            {
                UserID = lawyerID
            };
            var userprofileResponse = _limService.GetUserProfileByUserID(userprofileRequest);

            if (userprofileResponse.UserProfile != null)
            {
                var fctFee = new FCTFeePaymentRequest
                {
                    BaseAmount = fee.Amount,
                    GST = fee.GST,
                    HST = fee.HST,
                    QST = fee.QST,
                    PST = 0,
                    RST = 0,
                    Service = code,
                    LawyerCRMReference = userprofileResponse.UserProfile.CrmID,
                    LaywerReference = userprofileResponse.UserProfile.LawyerCode.GetValueOrDefault(),
                    ProvinceCode = province,
                };
                feePaymentRequest.PaymentMethodName = PaymentMethod.FCTFee;
                feePaymentRequest.FCTFee = fctFee;
                feePaymentRequest.Amount = fee.Amount+fee.GST+fee.HST+fee.QST;

                return feePaymentRequest;
            }
            return null;
        }

        private static PaymentMethod ConvertToEPSPaymentMethod(string llcPaymentMethod)
        {
            switch (llcPaymentMethod)
            {
                case LLCPaymentMethod.Wire:
                case LLCPaymentMethod.WireTransfer:
                    return PaymentMethod.Wire;
                case LLCPaymentMethod.EFT:
                    return PaymentMethod.EFT;
                case LLCPaymentMethod.Cheque:
                    return PaymentMethod.Cheque;
                case LLCPaymentMethod.EDelivery:
                    return PaymentMethod.Electronic_Delivery;
            }
            return PaymentMethod.Wire;
        }

        private static PaymentRequestType ConvertToEPSPaymentRequest(string paymentRequestTypeName)
        {
            switch (paymentRequestTypeName.ToUpper())
            {
                case "SINGLE":
                    return PaymentRequestType.Single;
                case  "BATCH":
                    return PaymentRequestType.Batch;
                case "MULTIPAYEES":
                    return PaymentRequestType.MultiPayees;
            }
            return PaymentRequestType.Single;
        }

        private static string GetCanadianClearingCode(Account account)
        {
            return CanadianClearingCodePrefix + account.BankNumber + account.TransitNumber;
        }
    }

    internal struct LLCPaymentMethod
    {
        public const string Wire = "WIRE";
        public const string EFT = "EFT";
        public const string Cheque = "CHEQUE";
        public const string WireTransfer = "WIRE TRANSFER";
        public const string EDelivery = "ELECTRONIC DELIVERY";
    }
}
