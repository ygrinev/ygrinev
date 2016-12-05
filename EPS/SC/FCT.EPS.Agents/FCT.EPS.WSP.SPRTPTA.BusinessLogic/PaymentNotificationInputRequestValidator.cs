using System;
using FCT.EPS.Notification;
using FCT.EPS.DataEntities;
using FCT.EPS.WSP.Resources;
using System.Collections.Generic;
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;

namespace FCT.EPS.WSP.SPRTPTA.BusinessLogic
{
    public class PaymentNotificationInputRequestValidator : IRequestValidator
    {
        public bool IsValid(tblPaymentNotification paymentNotificationInput, ref List<string> invalidFieldsList)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            bool isValid = false;

            if (paymentNotificationInput != null)
            {
                try
                {
                    PaymentNotification.NotificationTypeType _notificationType =
                        Utils.ParseEnum<PaymentNotification.NotificationTypeType>(paymentNotificationInput.NotificationType);
                    if (
                       !( _notificationType == PaymentNotification.NotificationTypeType.DebitConfirmation ||
                        _notificationType == PaymentNotification.NotificationTypeType.ChequeConfirmation ||
                        _notificationType == PaymentNotification.NotificationTypeType.FCTFeeConfirmation)
                        )
                    {
                        invalidFieldsList.Add("Invalid Notification Type : must be DebitConfirmation or ChequeConfirmation or FCTFeeConfirmation");
                    }
                }
                catch (Exception aex)
                {
                    invalidFieldsList.Add(aex.Message);
                }

                //if (string.IsNullOrWhiteSpace(paymentNotificationInput.FCTTrustAccountNumber))
                //    invalidFieldsList.Add("FCTTrustAccountNumber");

                if (string.IsNullOrWhiteSpace(paymentNotificationInput.PaymentReferenceNumber))
                    invalidFieldsList.Add("PaymentReferenceNumber");

                //if (paymentNotificationInput.Amount <= 0)
                //    invalidFieldsList.Add("Amount");

                if (!paymentNotificationInput.PaymentDateTime.HasValue ||
                    (paymentNotificationInput.PaymentDateTime.HasValue &&
                    paymentNotificationInput.PaymentDateTime == new DateTime()))
                    invalidFieldsList.Add("PaymentDateTime");

                isValid = invalidFieldsList.Count == 0;
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return isValid;
        }
    }
}
