using System.Linq;
using FCT.EPS.Notification;
using FCT.EPS.DataEntities;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.DataAccess;
using System.Collections.Generic;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;

namespace FCT.EPS.WSP.SPRTPTA.BusinessLogic
{
    public class PaymentNotificationRequestMapper
    {
        public PaymentTrackerRequestHelper GetPaymentNotificationRequestFromEntity(UnitOfWork myUnitOfWork, tblPaymentNotification paymentNotificationInput)
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
                //1. Findout all PaymentRequests for this paymentNotificationInput.PaymentTransactionID

                List<tblPaymentRequest> myPaymentRequests = myUnitOfWork.TblPaymentRequestRepository.Get(c =>  c.PaymentTransactionID == paymentNotificationInput.PaymentTransactionID).ToList<tblPaymentRequest>();
                tblPaymentStatus myPaymentStatus = null;
                myPaymentStatus = myUnitOfWork.TblPaymentStatusRepository.Get(c => c.PaymentStatusCode == paymentNotificationInput.PaymentStatusCode
                    && c.NotificationType == paymentNotificationInput.NotificationType).ToList<tblPaymentStatus>().FirstOrDefault();

                if (myPaymentRequests != null)
                {
                    foreach (tblPaymentRequest myPaymentRequest in myPaymentRequests)
                    {
                        PaymentNotification paymentNotification = new PaymentNotification();
                        paymentNotification.NotificationType = Utils.ParseEnum<PaymentNotification.NotificationTypeType>(paymentNotificationInput.NotificationType);
                        paymentNotification.PaymentReferenceNumber = paymentNotificationInput.PaymentReferenceNumber;
                        paymentNotification.PaymentDateTime = Utils.GetDateTime(paymentNotificationInput.PaymentDateTime);
                        paymentNotification.PaymentAmount = myPaymentRequest.PaymentAmount;
                        paymentNotification.PaymentOriginatorName = paymentNotificationInput.OriginatorName;
                        paymentNotification.PaymentBatchID = paymentNotificationInput.PaymentBatchID;
                        paymentNotification.PaymentBatchDescription = paymentNotificationInput.PaymentBatchDescription;
                        paymentNotification.AdditionalInformation = paymentNotificationInput.AdditionalInfo;
                        //paymentNotification.PaymentOriginatorAccount = new Account();

                        ////Apply the parsing algorithm split 
                        ////paymentNotification.PaymentOriginatorAccount.AccountNumber = paymentNotificationInput.OriginalorAccountNumber;
                        ////paymentNotification.PaymentOriginatorAccount.BankNumber
                        ////paymentNotification.PaymentOriginatorAccount.TransitNumber
                        if (!string.IsNullOrEmpty(paymentNotificationInput.OriginalorAccountNumber))
                        {
                            FCTAccountParser fctAccountParser = new FCTAccountParser(paymentNotificationInput.OriginalorAccountNumber);
                            paymentNotification.PaymentOriginatorAccount = new Account();
                            paymentNotification.PaymentOriginatorAccount.AccountNumber = fctAccountParser.AccountNumber;
                            paymentNotification.PaymentOriginatorAccount.TransitNumber = fctAccountParser.TransitNumber;
                        }
                        
                        if (myPaymentStatus != null)
                        {
                            paymentNotification.PaymentStatus = myPaymentStatus.StatusDescription;  //Get the right value based on NotificationType and PaymentStatusCode
                        }

                        paymentNotification.DisbursementRequestID = myPaymentRequest.DisbursementRequestID;

                        request.AddToPaymentNotificaitonList(paymentNotification);
                    }
                }
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return request;
        }
    }
}
