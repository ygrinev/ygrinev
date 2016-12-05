using FCT.EPS.WSP.ExternalResources.FinanceService;
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;
using FCT.EPS.WSP.Resources;

namespace FCT.EPS.WSP.GCSA.BusinessLogic
{
    public static class Translate
    {
        public static FCT.EPS.DataEntities.tblPaymentNotification tbEPSChequeRecord2ChequeStatusResponse(ChequeStatusResponse passedChequeStatusResponse)
        {
            FCT.EPS.DataEntities.tblPaymentNotification mytblPaymentNotification = new FCT.EPS.DataEntities.tblPaymentNotification();
            mytblPaymentNotification.StatusID                   = Constants.DataBase.Tables.tblEPSStatus.RECEIVED;
            mytblPaymentNotification.PaymentDateTime            = passedChequeStatusResponse.DatePaid;
            mytblPaymentNotification.LastModifyDate             = passedChequeStatusResponse.LastModifiedDate;
            mytblPaymentNotification.AdditionalInfo             = passedChequeStatusResponse.Comments;
            mytblPaymentNotification.PaymentBatchID             = passedChequeStatusResponse.PaymentBatchID;
            mytblPaymentNotification.PaymentBatchDescription    = passedChequeStatusResponse.PaymentBatchDescription;
            mytblPaymentNotification.PaymentReferenceNumber     = passedChequeStatusResponse.ChequeNumber;
            mytblPaymentNotification.NotificationType           = PaymentNotification.NotificationTypeType.ChequeConfirmation.ToString();
            mytblPaymentNotification.PaymentTransactionID       = passedChequeStatusResponse.DisbursemenTransactionID;
            mytblPaymentNotification.PaymentStatusCode          = passedChequeStatusResponse.StatusID;
            return mytblPaymentNotification;
        }
    }
}
