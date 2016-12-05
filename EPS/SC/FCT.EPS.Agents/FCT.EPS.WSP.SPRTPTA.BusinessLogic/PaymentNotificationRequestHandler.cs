using System;
using System.Linq;
using System.Diagnostics;
using System.Transactions;
using FCT.EPS.Notification;
using FCT.EPS.DataEntities;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.DataAccess;
using System.Collections.Generic;
using FCT.EPS.WSP.SPRTPTA.Resources;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;


namespace FCT.EPS.WSP.SPRTPTA.BusinessLogic
{
    public class PaymentNotificationRequestHandler
    {
        #region Public Functions

        UnitOfWork _myUnitOfWork = new UnitOfWork();
        NonGeneric _nonGenericAccess;


        PaymentTrackingWebServiceClient _paymentTrackingServiceClient;

        public PaymentNotificationRequestHandler(
           PaymentTrackingWebServiceClient paymentTrackingServiceClient
            )
        {
            this._nonGenericAccess = new NonGeneric(_myUnitOfWork);
            this._paymentTrackingServiceClient = paymentTrackingServiceClient;
        }

        public void SubmitPaymentRequestToPaymentTracker(int numberOfRecords, int maxAllowedRetry)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            int lastRecordID = 0;
            bool StillRecordsLeft = true;
            List<string> invalidFieldsList = null;
            string invalidBusinessRules = string.Empty;
            string PaymentStatusForAuditLogging = string.Empty;
            string ServceEndPoint = string.Empty;
            string comments = string.Empty;
            int recordStatus = Constants.DataBase.Tables.tblEPSStatus.RECEIVED;
            string logMessage = string.Empty;

            PaymentNotificationInputRequestValidator paymentNotificationRequestValidator =
                                                        new PaymentNotificationInputRequestValidator();

            while (StillRecordsLeft)
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                {
                    Transaction myTransaction = System.Transactions.Transaction.Current;
                    StillRecordsLeft = false;

                    IList<tblPaymentNotification> paymentTrackerRequestRequestHelperTable = _nonGenericAccess.GetListOfPaymentNotificationRequest(lastRecordID, numberOfRecords);

                    foreach (tblPaymentNotification objtblPaymentNotification in paymentTrackerRequestRequestHelperTable)
                    {
                            try
                            {
                                using (TransactionScope transactionScopePerRecord = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                                {

                                    StillRecordsLeft = true;
                                    comments = string.Empty;
                                    lastRecordID = objtblPaymentNotification.NotificationID;

                                    recordStatus = Constants.DataBase.Tables.tblEPSStatus.RECEIVED;
                                    PaymentStatusForAuditLogging = "RECEIVED";
                                    ServceEndPoint = string.Empty;
                                    //Forward to llc
                                    invalidFieldsList = new List<string>();

                                    if (paymentNotificationRequestValidator.IsValid(objtblPaymentNotification, ref invalidFieldsList))
                                    {
                                        tblPaymentRequest objtblPaymentRequest = _myUnitOfWork.TblPaymentRequestRepository.Get(c => c.PaymentTransactionID == objtblPaymentNotification.PaymentTransactionID).FirstOrDefault();

                                        ServceEndPoint = GetServceEndPoint(objtblPaymentRequest);

                                        if (string.IsNullOrWhiteSpace(ServceEndPoint))
                                        {
                                            invalidFieldsList.Add("Payment Tracker servceEndPoint not set");
                                        }
                                    }

                                    if (invalidFieldsList.Count != 0)
                                    {
                                        //Mark Failed - No point in retrying
                                        recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                        PaymentStatusForAuditLogging = "FAILED";

                                        comments += "Missing/Invalid required fields ==>  " + string.Join(",", invalidFieldsList);
                                        logMessage = auditLogPaymentNotificationAuditMessage(objtblPaymentNotification, PaymentStatusForAuditLogging, comments);
                                        LoggingHelper.LogErrorActivity(logMessage);
                                    }
                                    else
                                    {
                                        PaymentTrackerRequestHelper paymentTrackerRequestRequestHelper =
                                             new PaymentNotificationRequestMapper().
                                                    GetPaymentNotificationRequestFromEntity(_myUnitOfWork, objtblPaymentNotification);

                                        paymentTrackerRequestRequestHelper.ServceEndPoint = ServceEndPoint;

                                        //all inputs are validated now     

                                        if (SubmitPaymentRequest(maxAllowedRetry,
                                                                    objtblPaymentNotification,
                                                                    paymentTrackerRequestRequestHelper))
                                        {
                                            recordStatus = Constants.DataBase.Tables.tblEPSStatus.SUBMITTED;
                                            PaymentStatusForAuditLogging = "SUBMITTED";
                                        }
                                        else
                                        {
                                            comments += "Unable to proceed, Payment Tracker Service returned false.";
                                            if (objtblPaymentNotification.NumberRetried >= maxAllowedRetry)
                                            {
                                                recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                                PaymentStatusForAuditLogging = "FAILED";
                                                logMessage = auditLogPaymentNotificationAuditMessage(objtblPaymentNotification, PaymentStatusForAuditLogging, comments);
                                                LoggingHelper.LogErrorActivity(logMessage);
                                            }
                                            else
                                            {
                                                PaymentStatusForAuditLogging = "RETRY";
                                                objtblPaymentNotification.NumberRetried++;
                                                logMessage = auditLogPaymentNotificationAuditMessage(objtblPaymentNotification, PaymentStatusForAuditLogging, comments);
                                                LoggingHelper.LogErrorActivity(logMessage);
                                            }
                                        }
                                    }
                                    transactionScopePerRecord.Complete();
                                }
                            }
                            catch (Exception ex)
                            {
                                // log and re-try 
                                //Write Warning
                                SolutionTraceClass.WriteLineError("Received the exception from PaymentTracking Service. Error Message is->" + ex.Message);

                                if (objtblPaymentNotification != null)
                                {
                                    if (objtblPaymentNotification.NumberRetried >= maxAllowedRetry)
                                    {
                                        recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                        PaymentStatusForAuditLogging = "FAILED";
                                    }
                                    else
                                    {
                                        objtblPaymentNotification.NumberRetried++;
                                        PaymentStatusForAuditLogging = "RETRY";
                                    }

                                    comments += "Received the exception from PaymentTracking Service.";
                                    logMessage = auditLogPaymentNotificationAuditMessage(objtblPaymentNotification, PaymentStatusForAuditLogging, comments);
                                }
                                else
                                {
                                    recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                    PaymentStatusForAuditLogging = "FAILED";
                                    logMessage = "Received the exception from PaymentTracking Service. # PaymenStatus # " + PaymentStatusForAuditLogging;
                                }

                                LoggingHelper.LogErrorActivity(logMessage, ex);
                            }
                            finally
                            {
                                using (TransactionScope updateTransactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                                {
                                    UpdatePaymentNotificationStatus(objtblPaymentNotification, recordStatus);
                                    updateTransactionScope.Complete();
                                }

                                if (objtblPaymentNotification != null)
                                {
                                    comments += "processed";
                                    logMessage = auditLogPaymentNotificationAuditMessage(objtblPaymentNotification, PaymentStatusForAuditLogging, comments);
                                }
                                else
                                {
                                    logMessage = "Processed # PaymenStatus # " + PaymentStatusForAuditLogging;
                                }
                                LoggingHelper.LogAuditingActivity(logMessage);
                            }
                            
                    }
                    transactionScope.Complete();
                }
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }

        #endregion

        #region Private Functions

        private void UpdatePaymentNotificationStatus(tblPaymentNotification objtblPaymentNotification, int recordStatus)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            objtblPaymentNotification.StatusID = recordStatus;
            objtblPaymentNotification.LastModifyDate = DateTime.Now;
            _myUnitOfWork.Save();

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private bool SubmitPaymentRequest(int maxAllowedRetry,
                                                tblPaymentNotification objtblPaymentNotification,
                                                PaymentTrackerRequestHelper paymentTrackerRequestRequestHelper)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            //submit payment tracker request by calling the web service      
            PaymentNotificationResponse paymentStatus = _paymentTrackingServiceClient.SubmitRequestToPaymentTrackerWebService(
                                paymentTrackerRequestRequestHelper.GetPaymentNotificationRequest(),
                                paymentTrackerRequestRequestHelper.ServceEndPoint);
            SolutionTraceClass.WriteLineVerbose("End");
            return paymentStatus.NotificationRecieved;
        }
        private string auditLogPaymentNotificationAuditMessage(tblPaymentNotification objtblPaymentNotification, string paymenStatus, string comments)
        {
            //Logging.PaymentNotificationAuditMessageStringFormat = "NotificationID # {0} , NotificationType # {1} , NumberRetried# {2} , StatusID # {3}, Comments # {4}";
            return string.Format("NotificationID # {0} , NotificationType # {1} , NumberRetried# {2} , StatusID # {3}, Comments # {4}", objtblPaymentNotification.NotificationID, objtblPaymentNotification.NotificationType,
                                   objtblPaymentNotification.NumberRetried, paymenStatus, comments);
        }
        private string GetServceEndPoint(tblPaymentRequest objtblPaymentRequest)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            string ServceEndPoint = string.Empty;

            if (objtblPaymentRequest != null)
            {
                tblSolutionSubscription solutionSubscription = _myUnitOfWork.TblSolutionSubscriptionRepository.Get(
                                        c => c.SubscriptionID == objtblPaymentRequest.SubscriptionID).FirstOrDefault();
                if (solutionSubscription != null)
                {
                    ServceEndPoint = solutionSubscription.ServceEndPoint;
                }
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return ServceEndPoint;
        }


        #endregion
    }
}
