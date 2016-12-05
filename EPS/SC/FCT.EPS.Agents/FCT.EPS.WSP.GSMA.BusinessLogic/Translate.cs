using FCT.EPS.WSP.GSMA.Resources;
using FCT.EPS.WSP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.GSMA.BusinessLogic
{
    public static class Translate
    {

        public static FCT.EPS.DataEntities.tblPaymentNotification tbFCTFeeStatusInfo2tblPaymentNotification(FCT.EPS.Swift.MT900DataValues passedMT900DataValues)
        {
            FCT.EPS.DataEntities.tblPaymentNotification mytblPaymentNotification = new FCT.EPS.DataEntities.tblPaymentNotification()
            {
                PaymentReferenceNumber = passedMT900DataValues.FIReferenceNumber,
                PaymentTransactionID = Convert.ToInt32(passedMT900DataValues.FCTPaymentTransactionID),
                FCTTrustAccountNumber = passedMT900DataValues.SenderBankAccountID,
                OriginalorAccountNumber = passedMT900DataValues.SenderBankAccountID,
                PaymentDateTime = passedMT900DataValues.Date,
                Amount = passedMT900DataValues.Amount,
                OriginatorName = passedMT900DataValues.PayorName,
                AdditionalInfo = passedMT900DataValues.PayeeReferenceNumber,
                NotificationType = AgentConstants.Misc.DEBIT_NOTIFICATION_TYPE,
                StatusID = Constants.DataBase.Tables.tblEPSStatus.RECEIVED,
                PaymentStatusCode = Constants.DataBase.Tables.tblPaymentStatus.DEBITCONFIRMATION_PAID,
                LastModifyDate = DateTime.Now
            };

            return mytblPaymentNotification;
        }



        internal static DataEntities.tblPaymentNotification NewtblPaymentNotificationForChildren(DataEntities.tblPaymentTransaction passedtblPaymentTransaction, DataEntities.tblPaymentNotification passedtblPaymentNotification)
        {
            return new DataEntities.tblPaymentNotification()
            {
                PaymentReferenceNumber = passedtblPaymentNotification.PaymentReferenceNumber,
                FCTTrustAccountNumber = passedtblPaymentNotification.FCTTrustAccountNumber,
                OriginalorAccountNumber = passedtblPaymentNotification.OriginalorAccountNumber,
                PaymentDateTime = passedtblPaymentNotification.PaymentDateTime,
                Amount = passedtblPaymentTransaction.tblPaymentRequest.Sum(c=>c.PaymentAmount),
                OriginatorName = passedtblPaymentNotification.OriginatorName,
                AdditionalInfo = passedtblPaymentNotification.AdditionalInfo,
                NotificationType = passedtblPaymentNotification.NotificationType,
                StatusID = Constants.DataBase.Tables.tblEPSStatus.RECEIVED,
                PaymentStatusCode = passedtblPaymentNotification.PaymentStatusCode,
                LastModifyDate = passedtblPaymentNotification.LastModifyDate
            };
        }
    }
}
