using System.Runtime.Serialization;

namespace FCT.LLC.Common.DataContracts
{
    [DataContract(Name = "ErrorCode")]
    public enum ErrorCode
    {
        [EnumMember] None = 0,
        [EnumMember] DealDoesNotExist = 1,
        [EnumMember] DealsDoNotMatch = 2,
        [EnumMember] DisbursementsNotFound = 3,
        [EnumMember] DisbursementsDoNotMatch = 4,
        [EnumMember] NoTrustAccountID = 5,
        [EnumMember] TrustAccountNotActive = 6,
        [EnumMember] SummaryNotFound = 7,
        [EnumMember] FundsDoNotMatch = 8,
        [EnumMember] PurchaserHasNotSigned = 9,
        [EnumMember] VendorHasNotSigned = 10,
        [EnumMember] CPASystemClosure = 11,
        [EnumMember] VendorLawyerDisbursementNotFound = 12,
        [EnumMember] VendorLawyerAmountInadequate = 13,
        [EnumMember] Weekend = 14,
        [EnumMember] NotWithinWorkingHours=15,
        [EnumMember] SubmitPaymentFaulted=16,
        [EnumMember] InSufficientFunds=17,
        [EnumMember] VendorAcknowledged=18,
        [EnumMember] InactiveUser=19,
        [EnumMember] OutstandingDeposit=20,
        [EnumMember] VendorMissing=21,
        [EnumMember] PurchaserMissing=22,
        [EnumMember] DealDisbursed=23, 
        [EnumMember] ConcurrencyCheckFailed=24,
        [EnumMember] DealCancelledOrDeclined=25,
        [EnumMember] ClosingDateMissing=26,
        [EnumMember] OtherLawyerMissing = 27,
        [EnumMember] ChainDealCircularReference = 28,
        [EnumMember] ChainDealNotEasyFund = 29,
        [EnumMember] ChainDealNotValidPurchaser = 30,
        [EnumMember] ChainDealRefinanceTransType = 31,
        [EnumMember] ChainDealFctRefNumDoesNotExist = 32,
        [EnumMember] ChainDealVendorCannotBeSigned = 33,
        [EnumMember] ChainDealPurchaserNotValidDisbursement = 34,
        [EnumMember] ChainDealPurchaserCannotSign = 35,
        [EnumMember] ChainDealVendorCannotBeDisbursed = 36,
        [EnumMember] ChainDealVendorAlreadyDisbursed = 37,
        [EnumMember] ChainDealPurchaserCannotDisburse = 38
    }

}
