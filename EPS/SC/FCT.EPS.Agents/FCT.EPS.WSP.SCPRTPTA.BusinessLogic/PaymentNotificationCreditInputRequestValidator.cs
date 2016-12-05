
using System;
using FCT.EPS.Notification;
using FCT.EPS.WSP.Resources;
using FCT.EPS.DataEntities;
using System.Collections.Generic;
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;

namespace FCT.EPS.WSP.SCPRTPTA.BusinessLogic
{
    public class PaymentNotificationCreditInputRequestValidator : IRequestValidator
    {
        public bool IsValid(tblPaymentNotification paymentNotificationInput, ref List<string> invalidFieldsList)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            bool isValid = false;
            
            if (paymentNotificationInput != null)
            {
                try { 
                 PaymentNotification.NotificationTypeType  CreditNotificationType = 
                     Utils.ParseEnum<PaymentNotification.NotificationTypeType>(paymentNotificationInput.NotificationType); 
                    if(CreditNotificationType!= PaymentNotification.NotificationTypeType.CreditConfirmation)
                    {
                        invalidFieldsList.Add("Invalid Notification Type : must be CreditConfirmation");
                    }
                }
                catch (Exception aex)  
                {
                    invalidFieldsList.Add(aex.Message); 
                }
                
                if (string.IsNullOrWhiteSpace(paymentNotificationInput.FCTTrustAccountNumber))
                    invalidFieldsList.Add("FCTTrustAccountNumber");


                if (string.IsNullOrWhiteSpace(paymentNotificationInput.PaymentReferenceNumber))
                    invalidFieldsList.Add("PaymentReferenceNumber");

                //if (paymentNotificationInput.Amount <= 0)
                //    invalidFieldsList.Add("Amount");

                if (!string.IsNullOrWhiteSpace(paymentNotificationInput.OriginalorAccountNumber))
                {
                    try
                    {
                        FCTAccountParser fctAccountParser_BTA = new FCTAccountParser(paymentNotificationInput.OriginalorAccountNumber);
                    }
                    catch (Exception aex)
                    {
                        invalidFieldsList.Add(aex.Message);
                    }
                }

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
