using System;
using FCT.EPS.Swift;
using System.Diagnostics;
using System.Transactions;
using FCT.EPS.DataEntities;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.DataAccess;
using System.Collections.Generic;
using FCT.EPS.WSP.SSWIFTA.Resources;

namespace FCT.EPS.WSP.SSWIFTA.BusinessLogic
{
    public class SingleWireSWIFTNotificationRequestHandler
    {
        UnitOfWork myUnitOfWork = new UnitOfWork();
        NonGeneric _nonGenericAccess;

        public SingleWireSWIFTNotificationRequestHandler()
        {
            this._nonGenericAccess = new NonGeneric(myUnitOfWork);
        }
        public void SubmitWireRequestToSWIFT(int numberOfRecords, int maxAllowedRetry, string SingleWireFileFolderPath)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            int lastRecordID = 0;
            string invalidBusinessRules = string.Empty;
            string PaymentStatusForAuditLogging = string.Empty;
            string ServceEndPoint = string.Empty;
            string comments = string.Empty;
            int recordStatus = Constants.DataBase.Tables.tblEPSStatus.RECEIVED;
            bool StillRecordsLeft = true;

            string logMessage = string.Empty;
           // TraceEventType eventType = TraceEventType.Information;

            while (StillRecordsLeft)
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                {
                    StillRecordsLeft = false;

                    string PaymentType = string.Empty;
                    string PaymentTransactionID = string.Empty;
                    string SolutionName = string.Empty;
                  
                    IList<tblPaymentRequest> listOfSingleWirePaymentRequest =
                                        _nonGenericAccess.GetListOfSingleWirePaymentRequest(lastRecordID, numberOfRecords);

                    SingleWireFileHandler singleWireFileHandler = null;
                    MT101DataValues mt101DataValues = null;
                    SingleWireFileNameHandler singleWireFileNameHandler = null;
                    SingleWireFileNameHandler.SingleWireFileFolderPath = SingleWireFileFolderPath;

                    foreach (tblPaymentRequest objtblPaymentRequest in listOfSingleWirePaymentRequest)
                    {
                        try
                        {
                            StillRecordsLeft = true;
                            comments = string.Empty;
                            lastRecordID = objtblPaymentRequest.PaymentTransactionID == null ? 0 : (int)objtblPaymentRequest.PaymentTransactionID;
                            recordStatus = Constants.DataBase.Tables.tblEPSStatus.RECEIVED;
                            PaymentStatusForAuditLogging = "RECEIVED";
                            ServceEndPoint = string.Empty;
                            //Forward to llc
                            tblPaymentAddress payeeAddress = myUnitOfWork.TblPaymentAddressRepository.GetByID(objtblPaymentRequest.PayeeAddressID);

                            PaymentType = objtblPaymentRequest.PaymentMethod;
                            PaymentTransactionID = Utils.GetString(objtblPaymentRequest.PaymentTransactionID);

                            tblPaymentAddress bankAddress = myUnitOfWork.TblPaymentAddressRepository.GetByID(objtblPaymentRequest.PayeeBranchAddressID);

                            mt101DataValues = Translate.EPSDataToMT101DataValues(objtblPaymentRequest);
                            //mt101DataValues = MT101DataValuesMapper.MapFromEPSPaymentRequest(objtblPaymentRequest, payeeAddress, bankAddress);

                            //tblPaymentTransaction paymentTransaction = myUnitOfWork.TblPaymentTransactionRepository.GetByID(objtblPaymentRequest.PaymentTransactionID);

                            singleWireFileNameHandler = new SingleWireFileNameHandler(PaymentType, PaymentTransactionID, SolutionName);

                            singleWireFileHandler = new SingleWireFileHandler(singleWireFileNameHandler.GetSingleWireFileNameWithPath(), mt101DataValues);

                            if (singleWireFileHandler.SendWire())
                            {
                                recordStatus = Constants.DataBase.Tables.tblEPSStatus.SUBMITTED;
                                PaymentStatusForAuditLogging = "SUBMITTED";
                            }
                            else
                            {
                                comments += "Send Wire failed.";

                                if (objtblPaymentRequest.tblPaymentTransaction.NumberRetried >= maxAllowedRetry)
                                {
                                    recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                    PaymentStatusForAuditLogging = "FAILED";
                                    logMessage = auditLogMessage(objtblPaymentRequest, PaymentStatusForAuditLogging, comments);
                                    LoggingHelper.LogErrorActivity(logMessage);
                                }
                                else
                                {
                                    objtblPaymentRequest.tblPaymentTransaction.NumberRetried++;
                                    PaymentStatusForAuditLogging = "RETRY";
                                    logMessage = auditLogMessage(objtblPaymentRequest, PaymentStatusForAuditLogging, comments);
                                    LoggingHelper.LogErrorActivity(logMessage);
                                }                                  
                            }
                        }
                        catch (Exception ex)
                        {
                            // log and re-try 
                            //Write Warning
                            SolutionTraceClass.WriteLineError("Received the exception  while sending wire.. Error Message is->" + ex.Message);
                            comments += "Received the exception while sending wire.";

                            if (objtblPaymentRequest != null && objtblPaymentRequest.tblPaymentTransaction != null)
                            {
                                if (objtblPaymentRequest.tblPaymentTransaction.NumberRetried >= maxAllowedRetry)
                                {
                                    recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                    PaymentStatusForAuditLogging = "FAILED";                                   
                                }
                                else
                                {
                                    objtblPaymentRequest.tblPaymentTransaction.NumberRetried++;
                                    PaymentStatusForAuditLogging = "RETRY";                                    
                                }                               

                                logMessage = auditLogMessage(objtblPaymentRequest, PaymentStatusForAuditLogging, comments);
                            }
                            else
                            {
                                recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                PaymentStatusForAuditLogging = "FAILED";
                                logMessage = "Received the exception while sending wire. # PaymenStatus # " + PaymentStatusForAuditLogging;
                            }

                            LoggingHelper.LogErrorActivity(logMessage, ex);
                        }
                        finally
                        {
                            UpdateSingleWireTransactionStatus(objtblPaymentRequest, recordStatus);

                            if (objtblPaymentRequest != null)
                            {
                                comments += "processed";

                                logMessage = auditLogMessage(objtblPaymentRequest, PaymentStatusForAuditLogging, comments);
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

        private string auditLogMessage(tblPaymentRequest objtblPaymentRequest, string paymenStatus, string comments)
        {
            //public static string SingleWireSWIFTNotificationAuditMessageStringFormat = "PaymentTransactionID # {0} , PaymentMethod # {1} ,PaymentRequestType # {2}, NumberRetried# {3} , StatusID # {4}, Comments # {5}";
            return string.Format("PaymentTransactionID # {0} , PaymentMethod # {1} ,PaymentRequestType # {2}, NumberRetried# {3} , StatusID # {4}, Comments # {5}", objtblPaymentRequest.PaymentTransactionID,
                objtblPaymentRequest.PaymentMethod, objtblPaymentRequest.PaymentRequestType,
                objtblPaymentRequest.tblPaymentTransaction != null ? objtblPaymentRequest.tblPaymentTransaction.NumberRetried : -1,
                                   paymenStatus, comments);
        }
        private void UpdateSingleWireTransactionStatus(tblPaymentRequest objtblPaymentRequest, int recordStatus)
        {
            objtblPaymentRequest.tblPaymentTransaction.StatusID = recordStatus;
            objtblPaymentRequest.tblPaymentTransaction.LastModifiedDate = DateTime.Now;
            myUnitOfWork.Save();
        }

        private string GetSolutionName(tblPaymentRequest objtblPaymentRequest)
        {
            UnitOfWork myUnitOfWork = new UnitOfWork();

            string SolutionName = string.Empty;

            if (objtblPaymentRequest.tblSolutionSubscription != null)
            {
                tblFCTAccount FCTAccount = myUnitOfWork.TblFCTAccountRepository.GetByID(objtblPaymentRequest.tblSolutionSubscription.FCTAccountID);
                if (FCTAccount != null)
                {
                    tblSolution Solution = myUnitOfWork.TblSolutionRepository.GetByID(FCTAccount.SolutionID);
                    if (Solution != null)
                    {
                        SolutionName = Solution.SolutionName;
                    }
                }
            }
            return SolutionName;
        }

        public void CreateSingleWireSWIFTBatch()
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                new NonGeneric().CreateSingleWireSWIFTBatch();
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }
    }
}
