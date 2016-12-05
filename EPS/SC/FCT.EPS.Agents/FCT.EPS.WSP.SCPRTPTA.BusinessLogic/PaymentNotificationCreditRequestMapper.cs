using FCT.EPS.Notification;
using FCT.EPS.DataEntities;
using FCT.EPS.WSP.Resources;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;

namespace FCT.EPS.WSP.SCPRTPTA.BusinessLogic
{
    public class PaymentNotificationCreditRequestMapper
    {
        public PaymentTrackerRequestHelper GetPaymentNotificationCreditRequestFromEntity(tblPaymentNotification paymentNotificationInput)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            PaymentTrackerRequestHelper request = new PaymentTrackerRequestHelper();                        

            if (paymentNotificationInput != null)
            {
                //header/metadata for calling the payment tracking service, this data will not be send to payment tracking service
                request.NumberRetried = paymentNotificationInput.NumberRetried;
                request.NotificationID = paymentNotificationInput.NotificationID;
                request.FinancePaymentBatchID = paymentNotificationInput.PaymentBatchID;            
                
                //Request object for Payment tracking service , this will be send to payment tracking service
                PaymentNotification paymentNotification = new PaymentNotification();
                paymentNotification.NotificationType = Utils.ParseEnum<PaymentNotification.NotificationTypeType>(paymentNotificationInput.NotificationType);
                paymentNotification.PaymentReferenceNumber = paymentNotificationInput.PaymentReferenceNumber;
                paymentNotification.PaymentDateTime = Utils.GetDateTime(paymentNotificationInput.PaymentDateTime);
                paymentNotification.PaymentAmount = paymentNotificationInput.Amount;
                paymentNotification.PaymentOriginatorName = paymentNotificationInput.OriginatorName;
                paymentNotification.PaymentBatchID = paymentNotificationInput.PaymentBatchID;
                paymentNotification.PaymentBatchDescription = paymentNotificationInput.PaymentBatchDescription;
                paymentNotification.AdditionalInformation = paymentNotificationInput.AdditionalInfo;

                if (!string.IsNullOrEmpty(paymentNotificationInput.OriginalorAccountNumber))
                {
                    FCTAccountParser fctAccountParser = new FCTAccountParser(paymentNotificationInput.OriginalorAccountNumber);
                    paymentNotification.PaymentOriginatorAccount = new Account();
                    paymentNotification.PaymentOriginatorAccount.AccountNumber = fctAccountParser.AccountNumber;
                    paymentNotification.PaymentOriginatorAccount.TransitNumber = fctAccountParser.TransitNumber;
                }

                request.AddToPaymentNotificaitonList(paymentNotification);                
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return request;
        }
    }
}
