using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DealHistoryEntry
    {
        public string EnglishVersion { get; set; }
        public string FrenchVersion { get; set; }
        public string ResourceKey { get; set; }
        public int DealId { get; set; }
    }

    public struct ResourceSet
    {
        //When create deal history entry, please use this value as ResourceSet if only used in FCT.LLC.BusinessService
        public const string FCTLLCBusinessService = "FCT.LLC.BusinessService";
        public const string BusinessServiceDisbursementSet = "FCT.LLC.BusinessService.Disbursement";
        public const string LawyerPortalMessage = "LAWYER_PORTAL_MESSAGE";
    }

    public struct DealActivity
    {
        public const string EFCreatedInviteSent = "EFCreatedInviteSent";
        public const string EFInviteReceived = "EFInviteReceived";
        public const string EFDealCreated = "EFDealCreated";
        public const string EFDealAccepted = "EFDealAccepted";
        public const string EFDealDraftSaved = "EFDealDraftSaved";
        public const string LLCDealAccepted = "LLCDealAccepted";
        public const string DepositReceived = "DepositReceived";
        public const string FundsAllocated = "FundsAllocated";
        public const string DisbursementCreated = "DisbursementCreated";
        public const string DisbursementEdited = "DisbursementEdited";
        public const string EFPayeeEdited = "EFPayeeEdited";
        public const string DisbursementRemoved = "DisbursementRemoved";
        public const string DisbursementIsActive = "DisbursementIsActive";
        public const string DisbursementIsDraft = "DisbursementIsDraft";
        public const string EFAddedToDeal = "EFAddedToDeal";
        public const string EFDealSigned = "EFDealSigned";
        public const string EFDealSignatureRemoved = "EFDealSignatureRemoved";
        public const string EFDealDeclined = "EFDealDeclined";
        public const string EFDealCancelled = "EFDealCancelled";
        public const string DealCancelledWithReason = "DealCancelledWithReason";
        public const string DealCancelledNoReason = "DealCancelledNoReason";
        public const string LLCDealCancelled = "LLCDealCancelled";
        public const string DealCancelled = "dealCancelled";
        public const string EFDealDisbursed = "EFDealDisbursed";
        public const string ReturnFundsProcessed = "ReturnFundsProcessed";
        public const string ReturnFundsRequested = "ReturnFundsRequested";
        public const string ReturnFundsRequestRetracted = "ReturnFundsRequestRetracted";
        public const string ReturnFundsVendorAcknowledged = "ReturnFundsVendorAcknowledged";
        public const string LLCDealCancelledAdmin = "LLCAdminCancelled";
        public const string FundsReconciled = "FundsReconciled";
        public const string FundsUnreconciled = "FundsUnreconciled";
        public const string TrustAccountEdited = "TrustAccountEdited";
        public const string CreditCardEdited = "CreditCardEdited";
        public const string ActivityEFDealCancelled = "ActivityEFDealCancelled";
        public const string VendorDealCompleted = "DealCompleted.Vendor";
        public const string DealCompleted = "dealCompleted";
        public const string ConfirmedClosing = "ConfirmedClosing";
        // Deal History Messages for DoProcess
        public const string DoProcessEFDealDraftSaved = "DoProcessEFDealDraftSaved";
        public const string DoProcessDisbursementsReceived  = "DoProcessDisbursementsReceived";
        public const string DoProcessDealAccespted = "DoProcessDealAccespted";

    }

    internal struct PlaceHolder
    {
        public const string PayeeName = "{PayeeName}";
        public const string Amount = "{Amount}";
        public const string OldPayeeName = "{OldPayeeName}";
        public const string OldAmount = "{OldAmount}";
        public const string OldValue = "{OldValue}";
        public const string NewValue = "{NewValue}";
        public const string TrustAccount = "{TrustAccountValue}";
        public const string DisbursementStatus = "{DisbursementStatus}";
    }

    public struct HistoryMessage
    {
        //Closing Date
        public const string ClosingDateChange = "HistoryMessage.ClosingDateChange";
        //Vendor Information – Person: First Name, Middle Name, Last Name
        public const string VendorFirstName = "HistoryMessage.VendorFirstName";
        public const string VendorMiddleName = "HistoryMessage.VendorMiddleName";
        public const string VendorLastName = "HistoryMessage.VendorLastName";
        //Vendor Information – Business: Company Name, Contact First Name, Contact Last Name
        public const string VendorCompanyName = "HistoryMessage.VendorCompanyName";
        public const string VendorContactFirstName = "HistoryMessage.VendorContactFirstName";
        public const string VendorContactLastName = "HistoryMessage.VendorContactLastName";

        public const string VendorTypeChanged = "HistoryMessage.VendorTypeChanged";
        public const string VendorAdded = "HistoryMessage.VendorAdded";
        public const string VendorRemoved = "HistoryMessage.VendorRemoved";

        public const string DocumentCreated = "HistoryMessage.DocumentCreated";
        public const string DocumentPublished = "HistoryMessage.DocumentPublished";

        //Purchaser Information – Person: First Name, Middle Name, Last Name
        public const string PurchaserFirstName = "HistoryMessage.PurchaserFirstName";
        public const string PurchaserMiddleName = "HistoryMessage.PurchaserMiddleName";
        public const string PurchaserLastName = "HistoryMessage.PurchaserLastName";
        //Purchaser Information – Business: Company Name, Contact First Name, Contact Last Name
        public const string PurchaserCompanyName = "HistoryMessage.PurchaserCompanyName";
        public const string PurchaserContactFirstName = "HistoryMessage.PurchaserContactFirstName";
        public const string PurchaserContactLastName = "HistoryMessage.PurchaserContactLastName";
        
        public const string PurchaserTypeChanged = "HistoryMessage.PurchaserTypeChanged";
        public const string PurchaserAdded = "HistoryMessage.PurchaserAdded";
        public const string PurchaserRemoved = "HistoryMessage.PurchaserRemoved";

        //PIN(s)
        public const string PinAdd = "HistoryMessage.PinAdd";
        public const string PinRemove = "HistoryMessage.PinRemove";
        public const string PinUpdate = "HistoryMessage.PinUpdate";

        //Property: Unit Number, Street Number, Address 1, Address 2, City, Province, Postal Code
        public const string UnitNumberUpdated = "HistoryMessage.UnitNumberUpdated";
        public const string StreetNumberUpdated = "HistoryMessage.StreetNumberUpdated";
        public const string StreetNameUpdated = "HistoryMessage.StreetNameUpdated";
        public const string CityUpdated = "HistoryMessage.CityUpdated";
        public const string PropertyProvince = "HistoryMessage.PropertyProvince";
        public const string PostalCodeUpdated = "HistoryMessage.PostalCodeUpdated";


        //Mortgagor Information – Person: First Name, Middle Name, Last Name
        public const string MortgagorFirstName = "HistoryMessage.MortgagorFirstName";
        public const string MortgagorMiddleName = "HistoryMessage.MortgagorMiddleName";
        public const string MortgagorLastName = "HistoryMessage.MortgagorLastName";
        //Mortgagor Information – Business: Company Name, Contact First Name, Contact Last Name
        public const string MortgagorCompanyName = "HistoryMessage.MortgagorCompanyName";
        public const string MortgagorContactFirstName = "HistoryMessage.MortgagorContactFirstName";
        public const string MortgagorContactLastName = "HistoryMessage.MortgagorContactLastName";

        public const string MortgagorTypeChanged = "HistoryMessage.MortgagorTypeChanged";
        public const string MortgagorAdded = "HistoryMessage.MortgagorAdded";
        public const string MortgagorRemoved = "HistoryMessage.MortgagorRemoved";
    }
}
