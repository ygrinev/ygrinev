using FCT.EPS.WSP.ExternalResources.FinanceService;
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;
using FCT.EPS.WSP.GFSA.Resources;
using FCT.EPS.WSP.Resources;

namespace FCT.EPS.WSP.GFSA.BusinessLogic
{
    public static class Translate
    {
        public static FCT.EPS.DataEntities.tblPaymentNotification tbFCTFeeStatusInfo2tblPaymentNotification(FCTFeeStatusInfo passedFCTFeeStatusInfo)
        {
            FCT.EPS.DataEntities.tblPaymentNotification mytblPaymentNotification = new FCT.EPS.DataEntities.tblPaymentNotification()
            {
            PaymentReferenceNumber = passedFCTFeeStatusInfo.JournalControlNumber,
            PaymentStatusCode = passedFCTFeeStatusInfo.ErrorNumber != 0 ? -1 : passedFCTFeeStatusInfo.ProcessFlag,
            PaymentDateTime = passedFCTFeeStatusInfo.DatePaid,
            LastModifyDate = passedFCTFeeStatusInfo.LastModifiedDate,
            PaymentTransactionID = passedFCTFeeStatusInfo.DisbursemenTransactionID,
            AdditionalInfo = passedFCTFeeStatusInfo.Comments,
            NotificationType = PaymentNotification.NotificationTypeType.FCTFeeConfirmation.ToString(),
            StatusID = Constants.DataBase.Tables.tblEPSStatus.RECEIVED,
        };

            return mytblPaymentNotification;
        }
    }
}
