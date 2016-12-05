using System;
using System.Linq;
using System.Diagnostics;
using System.Transactions;
using FCT.EPS.Notification;
using FCT.EPS.DataEntities;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.DataAccess;
using System.Collections.Generic;
using FCT.EPS.WSP.SCPRTPTA.Resources;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;

namespace FCT.EPS.WSP.SCPRTPTA.BusinessLogic
{
    public class CreditNotificationRequestHandler
    {
        #region Public Functions

        UnitOfWork myUnitOfWork = new UnitOfWork();
        NonGeneric _nonGenericAccess;

        PaymentTrackingWebServiceClient _paymentTrackingServiceClient;

        public CreditNotificationRequestHandler(
           PaymentTrackingWebServiceClient paymentTrackingServiceClient
            )
        {
            this._nonGenericAccess = new NonGeneric(myUnitOfWork);
            this._paymentTrackingServiceClient = paymentTrackingServiceClient;
        }

        public void SubmitCreditPaymentRequestToPaymentTracker(int numberOfRecords, int maxAllowedRetry)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            int lastRecordID = 0;
            List<string> invalidFieldsList = null;
            string invalidBusinessRules = string.Empty;
            string PaymentStatusForAuditLogging = string.Empty;
            string ServceEndPoint = string.Empty;
            string comments = string.Empty;
            int recordStatus = Constants.DataBase.Tables.tblEPSStatus.RECEIVED;
            bool StillRecordsLeft = true;


            string logMessage = string.Empty;

            PaymentNotificationCreditInputRequestValidator paymentNotificationCreditRequestValidator =
                                                        new PaymentNotificationCreditInputRequestValidator();
            while (StillRecordsLeft)
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                {
                    StillRecordsLeft = false;

                    IList<tblPaymentNotification> paymentTrackerRequestRequestHelperTable =
                        _nonGenericAccess.GetListOfCreditNotificationRequest(lastRecordID, numberOfRecords);

                    foreach (tblPaymentNotification objtblPaymentNotification in paymentTrackerRequestRequestHelperTable)
                    {
                        try
                        {
                            using (TransactionScope transactionScopePerRecord = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                            {

                                comments = string.Empty;
                                StillRecordsLeft = true;
                                lastRecordID = objtblPaymentNotification.PaymentTransactionID == null ? 0 : (int)objtblPaymentNotification.PaymentTransactionID;

                                recordStatus = Constants.DataBase.Tables.tblEPSStatus.RECEIVED;
                                PaymentStatusForAuditLogging = "RECEIVED";
                                ServceEndPoint = string.Empty;
                                //Forward to llc
                                invalidFieldsList = new List<string>();

                                if (paymentNotificationCreditRequestValidator.IsValid(
                                                        objtblPaymentNotification, ref invalidFieldsList))
                                {
                                    ServceEndPoint = GetServceEndPoint(objtblPaymentNotification);

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
                                                                    new PaymentNotificationCreditRequestMapper().
                                                                        GetPaymentNotificationCreditRequestFromEntity(objtblPaymentNotification);

                                    paymentTrackerRequestRequestHelper.ServceEndPoint = ServceEndPoint;

                                    //all inputs are validated now                  
                                    if (SubmitCreditPaymentRequest(maxAllowedRetry,
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
                                            objtblPaymentNotification.NumberRetried++;
                                            PaymentStatusForAuditLogging = "RETRY";
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
                            SolutionTraceClass.WriteLineError("Received an exception. Error Message is->" + ex.Message);
                            
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
                                comments += "Received an exception.";
                                logMessage = auditLogPaymentNotificationAuditMessage(objtblPaymentNotification, PaymentStatusForAuditLogging, comments);

                            }
                            else
                            {
                                recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                PaymentStatusForAuditLogging = "FAILED";
                                logMessage = "Received an exception.. # PaymenStatus # " + PaymentStatusForAuditLogging;
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
            objtblPaymentNotification.StatusID = recordStatus;
            objtblPaymentNotification.LastModifyDate = DateTime.Now;
            myUnitOfWork.Save();
        }

        private bool SubmitCreditPaymentRequest(int maxAllowedRetry,
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

        private string auditLogPaymentNotificationAuditMessage(tblPaymentNotification objtblPaymentNotification, string creditPaymenStatus, string comments)
        {
            //Logging.AuditMessageStringFormat = "NotificationID # {0} , NotificationType # {1} , NumberRetried# {2} , StatusID # {3}, Comments # {4}";
            return string.Format("NotificationID # {0} , NotificationType # {1} , NumberRetried# {2} , StatusID # {3}, Comments # {4}", objtblPaymentNotification.NotificationID, objtblPaymentNotification.NotificationType,
                                   objtblPaymentNotification.NumberRetried, creditPaymenStatus, comments);
        }

        private string GetServceEndPoint(tblPaymentNotification objtblPaymentNotification)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            string ServceEndPoint = string.Empty;
            tblPaymentServiceProvider mytblPaymentServiceProvider = myUnitOfWork.TblPaymentServiceProviderRepository.Get(
               c => c.ServiceProviderName.Trim().ToUpper() == "SWIFT").FirstOrDefault();

            if (mytblPaymentServiceProvider != null)
            {
                IList<tblFCTAccount> mytblFCTAccounts = myUnitOfWork.TblFCTAccountRepository.Get(null).
                Where(c => objtblPaymentNotification.FCTTrustAccountNumber.Trim() ==
                (c.TransitNumber.Trim() + c.AccountNumber.Trim())).ToList<tblFCTAccount>();

                if (mytblFCTAccounts != null && mytblFCTAccounts.Count > 0)
                {
                    int FCTAccountID = mytblFCTAccounts[0].FCTAccountID;
                    tblSolutionSubscription solutionSubscription = myUnitOfWork.TblSolutionSubscriptionRepository.Get(
                                            c => c.ServiceProviderID == mytblPaymentServiceProvider.ServiceProviderID &&
                                                                           c.FCTAccountID == FCTAccountID).FirstOrDefault();
                    if (solutionSubscription != null)
                    {
                        ServceEndPoint = solutionSubscription.ServceEndPoint;
                    }
                }
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return ServceEndPoint;
        }

        #endregion
    }
}
