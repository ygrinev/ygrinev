namespace FCT.LLC.Common.DataContracts
{
    public struct LawyerActingFor
    {
        public const string Purchaser = "PURCHASER";
        public const string Vendor = "VENDOR";
        public const string Both = "BOTH";
        public const string Mortgagor = "MORTGAGOR";
    }

    public struct  DealStatus
    {
        public const string Active = "ACTIVE";
        public const string Cancelled = "CANCELLED";
        public const string CancelRequest = "CANCELLATION REQUESTED";
        public const string Complete = "COMPLETED";
        public const string Declined = "DECLINED";
        public const string UserDraft = "USER_DRAFT";
        public const string SystemDraft = "SYSTEM_DRAFT";
        public const string New = "PENDING ACCEPTANCE";


    }
    public struct BusinessModel
    {
        public const string EASYFUND = "EASYFUND";
        public const string LLC = "LLC";
        public const string COMBO = "LLC/EASYFUND";
        public const string LLCCOMBO = "LLC/EASYFUND";
        public const string MMS = "MMS";
        public const string MMSCOMBO = "MMS/EASYFUND";
    }

    public struct LawyerApplication
    {
        public const string Conveyancer = "DoProcess";
        public const string Portal = "Portal";
    }

    public struct DealType
    {
        public const string PurchaseSale = "PURCHASE/SALE";
        public const string Refinance = "REFINANCE";
    }

    public struct AllocationStatus
    {
        public const string Allocated = "ALLOCATED";
        public const string Assigned = "ASSIGNED";
        public const string UnAssigned = "UNASSIGNED";
        public const string UnAllocated = "UNALLOCATED";
        public const string PendingAckowledgement = "PENDING ACKNOWLEDGEMENT";
        public const string Acknowledged = "ACKNOWLEDGED";
        public const string Disbursed = "DISBURSED";
        public const string Retracted = "RETRACTED";

    }

    public enum SolutionType
    {
        LLC = 1,
        OOA = 2,
        MMS = 3,
        EASYFUND = 4
    }

    public struct UserType
    {
         public const string Lawyer = "Lawyer";
         public const string FCTAdmin = "FCT";
         public const string Clerk = "Clerk";
         public const string LENDER = "LENDER";
         public const string NOTIFICATION = "NOTIFICATION";
         public const string SYSTEM = "SYSTEM";
         public const string Assistant = "Assistant";        
    }

    public struct LocaleType
    {
        public const string EN = "en";
        public const string FR = "fr";
    }

    public struct EasyFundFee
    {
        public const string FeeName = "EasyFund Fee";
        public const string CategoryName = "EASYFUND";
        public const string FCTFeeAmount = "FCTFeeAmount";
        public const string FCTFeeIncrementAmount = "FCTFeeIncrementAmount";
        public const string FCTFeeIncrementStep = "FCTFeeIncrementStep";
        public const string FCTReturnFundsFee = "FCTReturnFundsFee";
    }

    public struct FeeDistribution
    {
        public const string VendorLawyer = "Vendor Lawyer";
        public const string PurchaserLawyer = "Purchaser Lawyer";
        public const string SplitEqually = "Split Equally";
    }

    public enum UserStatus
    {
        New=1,
        Active=2,
        Inactive=3,
        Declined=4,
        Deleted=5
    }

    public enum AccountStatus
    {
        Pending=1,
        Active=2,
        InActive=3
    }

    public struct RecordType
    {
        public const string Deposit = "DEPOSIT";
        public const string Return = "RETURN";
        public const string FCTFee = "FCTFee";
    }

    public struct PayeeType
    {
        public const string VendorLawyer = "Vendor Lawyer";
        public const string Mortgagee = "Mortgagee";
        public const string Builder = "Builder";
        public const string CondominiumStrataFees = "Condominium/Strata Fees";
        public const string LineOfCredit = "Loan/Unsecured Line of Credit";
        public const string MortgageBroker = "Mortgage Broker";
        public const string MunicipalorUtility = "Municipal or Utility";
        public const string Other = "Other";
        public const string RealEstateBroker = "Real Estate Broker";
        public const string CreditCard = "Credit Card";
        public const string ChainDeal = "Chain Deal";


    }

    public struct PartyType
    {
        public const string Business = "BUSINESS";
        public const string Person = "PERSON";
    }

    public struct DisbursementActiontype
    {
        public const string Save = "SAVE";
        public const string Sign = "SIGN";
        public const string Disburse = "DISBURSE";
    }
}
