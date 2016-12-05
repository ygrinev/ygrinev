using ChinhDo.Transactions;
using FCT.EPS.DataEntities;
using FCT.EPS.FileSerializer.RBC;
using FCT.EPS.Notification;
using FCT.EPS.WSP.DataAccess;
using FCT.EPS.WSP.ExternalResources;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.SETBA.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;


namespace FCT.EPS.WSP.SETBA.BusinessLogic
{
    public class SendElectronicToBankHandler
    {
        UnitOfWork myUnitOfWork = new UnitOfWork();
        NonGeneric _nonGenericAccess;

        public SendElectronicToBankHandler()
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            this._nonGenericAccess = new NonGeneric(myUnitOfWork);
            SolutionTraceClass.WriteLineVerbose("End");
        }
        public void CreateElectronicTransactions()
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                new NonGeneric().CreateElectronicBatch();
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }
        public void CreateElectronicRequest(int passedMaxRetryCount, string passedElectronicRequestFileArchivePath, int passedTimeSpanInSeconds, DateTime passedTimeOfDayToCreateBatch, string passedRoutingHeader)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            SolutionTraceClass.WriteLineVerbose("CreateElectronicRequest passedElectronicRequestFileArchivePath=>'" + passedElectronicRequestFileArchivePath + "'");
            
            StringBuilder theMessageToSend = new StringBuilder();
            bool StillRecordsLeft = true;


            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
            {
                IEnumerable<string> AllBodyLines;

                IList<tblPaymentTransaction> validatedListOfElectronicRequest = validateRecords(_nonGenericAccess.GetListOfElectronicRequest(passedTimeSpanInSeconds, passedTimeOfDayToCreateBatch));
                if (validatedListOfElectronicRequest.Count > 0)
                {

                    //Create List that need swift and list that don't.
                    IList<tblPaymentTransaction> requireSwift = validatedListOfElectronicRequest.Where<tblPaymentTransaction>(PT => PT.tblPaymentRequest.All(PR => PR.BPSWithWirePayment == true)).ToList();
                    IList<tblPaymentTransaction> noSwift = validatedListOfElectronicRequest.Where<tblPaymentTransaction>(PT => PT.tblPaymentRequest.All(PR => PR.BPSWithWirePayment != true)).ToList(); ;

                    tblBPSReference mytblBPSReference;

                    //Create BPS file that needs Swift
                    if (requireSwift.Count > 0)
                    {
                        mytblBPSReference = requireSwift.First().tblPaymentRequest.First().tblSolutionSubscription.tblFCTAccount.tblBPSReference;
                        AllBodyLines = CreateBody(passedMaxRetryCount, ref StillRecordsLeft, requireSwift, Constants.DataBase.Tables.tblEPSStatus.PROCESSING);
                        if (AllBodyLines.Count() > 0)
                        {
                            tblServiceBatchStatus mytblServiceBatchStatus = GettblServiceBatchStatus(requireSwift, Constants.DataBase.Tables.tblEPSStatus.PROCESSING);
                            string header = CreateHeader(mytblBPSReference, mytblServiceBatchStatus.BPSSequenceNumber);
                            string footer = CreateFooter(mytblBPSReference, AllBodyLines.Count());

                            StringBuilder FileBody = new StringBuilder();
                            FileBody.AppendLine(passedRoutingHeader);
                            FileBody.AppendLine(header);
                            foreach (string bodyLine in AllBodyLines)
                            {
                                FileBody.AppendLine(bodyLine);
                            }
                            FileBody.AppendLine(footer);

                            SolutionTraceClass.WriteLineVerbose("CreateElectronicRequest passedElectronicRequestFileArchivePath=>'" + passedElectronicRequestFileArchivePath + "'");
                            mytblServiceBatchStatus.PaymentFileName = SendFile(FileBody.ToString(), passedElectronicRequestFileArchivePath, mytblServiceBatchStatus.tblServiceBatch.ServiceBatchID);
                            CreateSwiftMessage(requireSwift, mytblBPSReference.PayeeInfoID);
                            myUnitOfWork.Save();
                        }
                        else
                        {
                            SolutionTraceClass.WriteLineError("Stored proc return records but none were processed For Swift required.");
                            LoggingHelper.LogAuditingActivity("Stored proc return records but none were processed For Swift required.");

                        }
                    }

                    if (noSwift.Count > 0)
                    {
                        //Create BPS file that does not need Swift
                        mytblBPSReference = null;
                        mytblBPSReference = noSwift.First().tblPaymentRequest.First().tblSolutionSubscription.tblFCTAccount.tblBPSReference;
                        //Setting status to SWIFTReceived so that the process that moves the file knows that it can.  That process is triggered on the file having the swift status updated.
                        AllBodyLines = CreateBody(passedMaxRetryCount, ref StillRecordsLeft, noSwift, Constants.DataBase.Tables.tblEPSStatus.SWIFTReceived);
                        if (AllBodyLines.Count() > 0)
                        {
                            tblServiceBatchStatus mytblServiceBatchStatus = GettblServiceBatchStatus(noSwift, Constants.DataBase.Tables.tblEPSStatus.SWIFTReceived);
                            string header = CreateHeader(mytblBPSReference, mytblServiceBatchStatus.BPSSequenceNumber);
                            string footer = CreateFooter(mytblBPSReference, AllBodyLines.Count());

                            StringBuilder FileBody = new StringBuilder();
                            FileBody.AppendLine(passedRoutingHeader);
                            FileBody.AppendLine(header);
                            foreach (string bodyLine in AllBodyLines)
                            {
                                FileBody.AppendLine(bodyLine);
                            }
                            FileBody.AppendLine(footer);

                            mytblServiceBatchStatus.PaymentFileName = SendFile(FileBody.ToString(), passedElectronicRequestFileArchivePath, mytblServiceBatchStatus.tblServiceBatch.ServiceBatchID);
                            myUnitOfWork.Save();
                        }
                        else
                        {
                            SolutionTraceClass.WriteLineError("Stored proc return records but none were processed For NonSwift required.");
                            LoggingHelper.LogAuditingActivity("Stored proc return records but none were processed For NonSwift required.");

                        }
                    }
                }
                transactionScope.Complete();
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }
        private void CreateSwiftMessage(IList<tblPaymentTransaction> validatedListOfElectronicRequest, int passedPayeeInfoID)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            IList<tblPaymentTransaction> succesfullRecords = validatedListOfElectronicRequest.Where(c => c.StatusID == Constants.DataBase.Tables.tblEPSStatus.PROCESSING).ToList<tblPaymentTransaction>();
            if (succesfullRecords.Count > 0)
            {
                decimal amount = succesfullRecords.Sum(c => c.tblPaymentRequest.Sum(p => p.PaymentAmount));
                tblPayeeInfo mytblPayeeInfo = myUnitOfWork.TblPayeeInfoRepository.GetByID(passedPayeeInfoID);
                if (mytblPayeeInfo == null)
                {
                    SolutionTraceClass.WriteLineError(string.Format("Error in CreateSwiftMessage for BPS transactions.  PayeeInfoID value returned a null Payeeinfo Record. PayeeInfoID = {0}", passedPayeeInfoID));
                    LoggingHelper.LogErrorActivity(string.Format("Error in CreateSwiftMessage for BPS transactions.  PayeeInfoID value returned a null Payeeinfo Record. PayeeInfoID = {0}", passedPayeeInfoID));
                    throw new Exception(string.Format("Error in CreateSwiftMessage for BPS transactions.  PayeeInfoID value returned a null Payeeinfo Record. PayeeInfoID = {0}", passedPayeeInfoID));
                }
                IList<tblPaymentRequest> mySuccessfillPaymentRequestRecords = succesfullRecords.SelectMany(c => c.tblPaymentRequest).ToList<tblPaymentRequest>();
                
                tblPaymentRequest mytblPaymentRequest = Translate.CreateBPStblPaymentRequest(mytblPayeeInfo, amount, mySuccessfillPaymentRequestRecords.First().SubscriptionID, mySuccessfillPaymentRequestRecords);
                myUnitOfWork.TblPaymentRequestRepository.Insert(mytblPaymentRequest);
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }
        public void SubmitElectronicFiles(int passedMaxRetryCount, string passedElectronicRequestFileArchivePath, string passedElectronicRequestFileFolderPath, string passedElectronicReportFileFolderPath, EmailItems passedReportEmailInfo, EmailItems passedSentBPSFileEmailInfo)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            bool StillRecordsLeft = true;

            while (StillRecordsLeft)
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                {
                    StillRecordsLeft = false;

                    IList<tblServiceBatchStatus> listOfElectronicRequest = _nonGenericAccess.GetListOfElectronicRequestToSubmit();

                    foreach (tblServiceBatchStatus currenttblServiceBatchStatus in listOfElectronicRequest)
                    {
                        int recordStatus = Constants.DataBase.Tables.tblEPSStatus.PROCESSING;
                        StillRecordsLeft = true;

                        try
                        {
                            TxFileManager fileMgr = new TxFileManager();
                            string currentFileLocation = string.Empty;
                            using (TransactionScope transactionScope2 = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                            {
                                //Create the Electronic file and save it.
                                DateTime myDateTime = DateTime.Now;
                                string myFileName = System.IO.Path.GetFileName(currenttblServiceBatchStatus.PaymentFileName);
                                //string myFilePath = System.IO.Path.Combine(new string[] { passedElectronicRequestFileFolderPath, myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00") });

                                fileMgr.CreateDirectory(passedElectronicRequestFileFolderPath);

                                currentFileLocation = System.IO.Path.Combine(new string[] { passedElectronicRequestFileArchivePath, currenttblServiceBatchStatus.PaymentFileName });
                                string destinationFileLocation = System.IO.Path.Combine(new string[] { passedElectronicRequestFileFolderPath, myFileName });
                                fileMgr.Copy(currentFileLocation, destinationFileLocation, true);
                                transactionScope2.Complete();
                                recordStatus = Constants.DataBase.Tables.tblEPSStatus.SUBMITTED;
                            }

                            using (TransactionScope transactionScope3 = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                            {
                                if (currenttblServiceBatchStatus.tblServiceBatch.tblPaymentTransaction.SelectMany(c => c.tblPaymentRequest).Where(c => c.tblPayeeInfo.tblCreditorList == null).Count() == 0)
                                {
                                    string reportFilePath = BPSTransactionReportGenerator.GenerateReport(currenttblServiceBatchStatus.tblServiceBatch.tblPaymentTransaction.SelectMany(c => c.tblPaymentRequest).OrderBy(p => p.tblPayeeInfo.tblCreditorList.CCIN).ToList(), passedElectronicReportFileFolderPath);
                                    SendEmail(passedReportEmailInfo.EmailAddress, passedReportEmailInfo.EmailSubject, string.Format(passedReportEmailInfo.EmailBody, System.IO.Path.GetFileName(reportFilePath), System.IO.Path.GetDirectoryName(reportFilePath)));
                                }
                                else
                                {
                                    SolutionTraceClass.WriteLineError(String.Format("'Electronic Rejected Transaction' cannot produce new report for ServiceBatchID=> {0} records make references to payee records that don't have a tblCreditorList record.", currenttblServiceBatchStatus.ServiceBatchID));
                                    LoggingHelper.LogErrorActivity(String.Format("'Electronic Rejected Transaction' cannot produce new report for ServiceBatchID=> {0} records make references to payee records that don't have a tblCreditorList record.", currenttblServiceBatchStatus.ServiceBatchID));
                                }

                                transactionScope3.Complete();
                            }

                            SendEmail(passedSentBPSFileEmailInfo.EmailAddress, passedSentBPSFileEmailInfo.EmailSubject, string.Format(passedSentBPSFileEmailInfo.EmailBody, currentFileLocation));

                            currenttblServiceBatchStatus.NumberRetried++;
                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineError("Receive an exception while move the file for Electronic.. Error Message is->" + ex.Message);
                            string logMessage = string.Empty;
                            string PaymentStatusForAuditLogging = string.Empty;
                            if (currenttblServiceBatchStatus != null)
                            {
                                if (currenttblServiceBatchStatus.NumberRetried >= passedMaxRetryCount)
                                {
                                    recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                    PaymentStatusForAuditLogging = "FAILED";
                                }
                                else
                                {
                                    currenttblServiceBatchStatus.NumberRetried++;
                                    PaymentStatusForAuditLogging = "RETRY";
                                }

                                logMessage = auditLogMessage(currenttblServiceBatchStatus, PaymentStatusForAuditLogging, "Failed to submit deal to RBC.");
                            }
                            else
                            {
                                recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                PaymentStatusForAuditLogging = "FAILED";
                                logMessage = "Receive an exception while moving the file for Electronic. No record to report. # PaymenStatus # " + PaymentStatusForAuditLogging;
                            }
                            LoggingHelper.LogErrorActivity(logMessage, ex);
                            continue;
                        }
                        finally
                        {
                            UpdateServiceBatchStatus(currenttblServiceBatchStatus, recordStatus);
                        }
                    }
                    transactionScope.Complete();
                }
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }

        #region PrivateMethods
        private IList<tblPaymentTransaction> validateRecords(IList<tblPaymentTransaction> listOfElectronicRequest)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            foreach (tblPaymentTransaction mytblPaymentTransaction in listOfElectronicRequest)
            {
                //Test for tblBPSReference
                if (mytblPaymentTransaction.tblPaymentRequest.Count == 0)
                {
                    mytblPaymentTransaction.StatusID = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                    LoggingHelper.LogErrorActivity(auditLogMessage(mytblPaymentTransaction, "FAILED", "tblPaymentTransaction record does not have an associated tblPaymentRequest record and cannot be processed"));
                    continue;
                }
                else
                {
                    foreach (tblPaymentRequest mytblPaymentRequest in mytblPaymentTransaction.tblPaymentRequest)
                    {
                        if (mytblPaymentRequest.tblSolutionSubscription == null)
                        {
                            mytblPaymentTransaction.StatusID = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                            LoggingHelper.LogErrorActivity(auditLogMessage(mytblPaymentTransaction, "FAILED", "tblPaymentTransaction record does not have an associated tblSolutionSubscription record and cannot be processed"));
                            break;
                        }
                        else
                        {
                            if (mytblPaymentRequest.tblSolutionSubscription.tblFCTAccount == null)
                            {
                                mytblPaymentTransaction.StatusID = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                LoggingHelper.LogErrorActivity(auditLogMessage(mytblPaymentTransaction, "FAILED", "tblPaymentTransaction record does not have an associated tblFCTAccount record and cannot be processed"));
                                break;
                            }
                            else if (mytblPaymentRequest.tblSolutionSubscription.tblFCTAccount.tblBPSReference == null)
                            {
                                mytblPaymentTransaction.StatusID = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                LoggingHelper.LogErrorActivity(auditLogMessage(mytblPaymentTransaction, "FAILED", "tblPaymentTransaction record does not have an associated tblBPSReference record and cannot be processed"));
                                break;
                            }
                        }
                    }
                }

            }

            myUnitOfWork.Save();
            SolutionTraceClass.WriteLineVerbose("End");
            return listOfElectronicRequest.Where(c => c.StatusID == Constants.DataBase.Tables.tblEPSStatus.RECEIVED).ToList<tblPaymentTransaction>();
        }
        private tblServiceBatchStatus GettblServiceBatchStatus(IList<tblPaymentTransaction> listOfElectronicRequest, int passedSuccessFullStatus)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            tblServiceBatch mytblServiceBatch = new tblServiceBatch()
            {
                LastModifiedDate = DateTime.Now
            };
            mytblServiceBatch.tblServiceBatchStatus = new tblServiceBatchStatus() { StatusID = Constants.DataBase.Tables.tblEPSStatus.PROCESSING, PaymentFileName = "", NumberRetried = 0 };
            int maxtblServiceBatchStatus;
            if (myUnitOfWork.TblServiceBatchStatusRepository.Get().Count() == 0)
            {
                maxtblServiceBatchStatus = 0;
            }
            else
            {
                maxtblServiceBatchStatus = myUnitOfWork.TblServiceBatchStatusRepository.Get().Max(c => c.BPSSequenceNumber);
            }
            if (maxtblServiceBatchStatus > 999)
            {
                mytblServiceBatch.tblServiceBatchStatus.BPSSequenceNumber = 1;
            }
            else
            {
                mytblServiceBatch.tblServiceBatchStatus.BPSSequenceNumber = (int)maxtblServiceBatchStatus + 1;
            }

            foreach (tblPaymentTransaction mytblPaymentTransaction in listOfElectronicRequest)
            {
                if (mytblPaymentTransaction.StatusID == passedSuccessFullStatus)
                {
                    mytblPaymentTransaction.tblServiceBatch = mytblServiceBatch;
                }
            }
            myUnitOfWork.Save();
            SolutionTraceClass.WriteLineVerbose("End");
            return mytblServiceBatch.tblServiceBatchStatus;
        }
        private string SendFile(String passedFileString, string passedElectronicRequestFileArchivePath, int passedServiceBatchID)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            SolutionTraceClass.WriteLineVerbose("SendFile passedElectronicRequestFileArchivePath->'" + passedElectronicRequestFileArchivePath + "'");
            string FilePathAndFileName = string.Empty;
            if (passedFileString.Length > 0)
            {
                try
                {
                    TxFileManager fileMgr = new TxFileManager();
                    //Create the swift file and save it.
                    DateTime myDateTime = DateTime.Now;
                    String fileName = "ElectronicDelivery_EasyFund_" + myDateTime.ToString("yyyyMMdd_HHmmss_fff_") + passedServiceBatchID.ToString() + ".txt";
                    string FilePath = System.IO.Path.Combine(new string[] { myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00") });
                    string archivePath = System.IO.Path.Combine(new string[] { passedElectronicRequestFileArchivePath, FilePath });
                    SolutionTraceClass.WriteLineVerbose("SendFile archivePath->'" + archivePath + "'");
                    FilePathAndFileName = System.IO.Path.Combine(new string[] { FilePath, fileName });
                    string archiveFileNameAndFullPath = System.IO.Path.Combine(archivePath, fileName);
                    System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
                    fileMgr.CreateDirectory(archivePath);
                    fileMgr.WriteAllBytes(archiveFileNameAndFullPath, ascii.GetBytes(passedFileString));
                }
                catch (Exception ex)
                {
                    SolutionTraceClass.WriteLineError("Receive an exception while creating the file for Electronic.. Error Message is->" + ex.Message);
                    LoggingHelper.LogErrorActivity("Receive an exception while creating the file for Electronic", ex);
                    throw;
                }
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return FilePathAndFileName;
        }
        private string auditLogMessage(tblPaymentRequest passedtblPaymentRequest, string paymenStatus, string comments)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            SolutionTraceClass.WriteLineVerbose("End");
            return string.Format("PaymentTransactionID # {0} , PaymentMethod # {1} ,PaymentRequestType # {2}, NumberRetried# {3} , StatusID # {4}, Comments # {5}", passedtblPaymentRequest.PaymentTransactionID,
                passedtblPaymentRequest.PaymentMethod, passedtblPaymentRequest.PaymentRequestType,
                passedtblPaymentRequest.tblPaymentTransaction != null ? passedtblPaymentRequest.tblPaymentTransaction.NumberRetried : -1, paymenStatus, comments);
        }
        private string auditLogMessage(tblPaymentTransaction passedtblPaymentTransaction, string paymenStatus, string comments)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            SolutionTraceClass.WriteLineVerbose("End");
            return string.Format("PaymentTransactionID # {0} , NumberRetried# {1} , StatusID # {2}, Comments # {3}", passedtblPaymentTransaction.PaymentTransactionID,
                passedtblPaymentTransaction.NumberRetried, paymenStatus, comments);
        }
        private string auditLogMessage(tblServiceBatchStatus passedtblServiceBatchStatus, string paymenStatus, string comments)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            SolutionTraceClass.WriteLineVerbose("End");
            return string.Format("ServiceBatchID # {0} , NumberRetried# {1} , StatusID # {2}, Comments # {3}",
                passedtblServiceBatchStatus.ServiceBatchID,
                passedtblServiceBatchStatus.NumberRetried,
                paymenStatus,
                comments);
        }
        private void UpdateElectronicTransactionStatus(tblPaymentTransaction passedtblPaymentTransaction, int recordStatus)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            passedtblPaymentTransaction.StatusID = recordStatus;
            passedtblPaymentTransaction.LastModifiedDate = DateTime.Now;
            myUnitOfWork.Save();
            SolutionTraceClass.WriteLineVerbose("End");
        }
        private void UpdateServiceBatchStatus(tblServiceBatchStatus passedtblServiceBatchStatus, int recordStatus)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {
                passedtblServiceBatchStatus.StatusID = recordStatus;
                passedtblServiceBatchStatus.NumberRetried++;
                myUnitOfWork.Save();
            }
            catch(Exception ex)
            {

                SolutionTraceClass.WriteLineError("Received an exception while trying to update tblServiceBatchStatus record.  Exception is ->" + ex.Message);
                string logMessage = string.Empty;
                string PaymentStatusForAuditLogging = string.Empty;


                logMessage = "Received an exception while trying to update tblServiceBatchStatus record. tblServiceBatchStatus.ServiceBatchID is ->" + passedtblServiceBatchStatus.ServiceBatchID;

                LoggingHelper.LogErrorActivity(logMessage, ex);
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }
        private String CreateHeader(tblBPSReference passedtblBPSReference, int bpsSequenceNumber)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            string MessageToBeSent = string.Empty;

            try
            {
                //Transalate from EPS entity objects to file converter objects
                HeaderValues myHeaderValues = new HeaderValues()
                {
                    RecordType = AgentConstants.Misc.RBC_HEADER_RECORD_TYPE,
                    ClientNumber = passedtblBPSReference.ClientNumber,
                    TransmitID = passedtblBPSReference.TransmitID,
                    TransmissionDate = DateTime.Now,
                    SequenceNumber = bpsSequenceNumber
                };
                RBCRemittanceHeaderDataValues RBCHeaderRecord = Translate.CreateRBCMessageHeaderRecord(myHeaderValues);
                RBCRemittanceFileHeaderProcessor<RBCRemittanceHeaderDataValues> RBCHeaderProcessor = new RBCRemittanceFileHeaderProcessor<RBCRemittanceHeaderDataValues>(RBCHeaderRecord);

                //Convert to string
                MessageToBeSent = RBCHeaderProcessor.SerializeToStringFormat();
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Received the exception while process footer for Electronic.. Error Message is->" + ex.Message);

                string logMessage = string.Empty;
                string PaymentStatusForAuditLogging = string.Empty;


                logMessage = "Received the exception while processing footer records for electronic.";

                LoggingHelper.LogErrorActivity(logMessage, ex);
                throw;
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return MessageToBeSent;
        }
        private IEnumerable<string> CreateBody(int passedMaxRetryCount, ref bool passedStillRecordsLeft, IList<tblPaymentTransaction> passedListOfElectronicRequest, int passedSuccessFullStatus)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            IList<string> MessageToBeSent = new List<string>();

            foreach (tblPaymentTransaction currenttblPaymentTransaction in passedListOfElectronicRequest)
            {
                passedStillRecordsLeft = true;
                int recordStatus = Constants.DataBase.Tables.tblEPSStatus.RECEIVED;
                try
                {
                    foreach (tblPaymentRequest currenttblPaymentRequest in currenttblPaymentTransaction.tblPaymentRequest)
                    {
                        tblPayeeInfo payeeInfo = myUnitOfWork.TblPayeeInfoRepository.GetByID(currenttblPaymentRequest.PayeeInfoID);
                        //Transalate from EPS entity objects to file converter objects
                        RBCRemittanceBodyDataValues RBCBodyRecord = Translate.CreateRBCMessageBodyRecord(currenttblPaymentRequest, payeeInfo);
                        RBCRemittanceFileBodyProcessor<RBCRemittanceBodyDataValues> RBCBodyProcessor = new RBCRemittanceFileBodyProcessor<RBCRemittanceBodyDataValues>(RBCBodyRecord);

                        //Convert to string
                        MessageToBeSent.Add(RBCBodyProcessor.SerializeToStringFormat());
                    }
                    recordStatus = passedSuccessFullStatus;
                }
                catch (Exception ex)
                {
                    SolutionTraceClass.WriteLineError("Received the exception while process the Records for Electronic.. Error Message is->" + ex.Message);

                    string logMessage = string.Empty;
                    string PaymentStatusForAuditLogging = string.Empty;


                    if (currenttblPaymentTransaction != null)
                    {
                        recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                        PaymentStatusForAuditLogging = "FAILED";

                        logMessage = auditLogMessage(currenttblPaymentTransaction, PaymentStatusForAuditLogging, "");
                    }
                    else
                    {
                        recordStatus = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                        PaymentStatusForAuditLogging = "FAILED";
                        logMessage = "Received the exception while processing the records for electronic. No record to report. # PaymenStatus # " + PaymentStatusForAuditLogging;
                    }

                    LoggingHelper.LogErrorActivity(logMessage, ex);
                    continue;

                }
                finally
                {
                    UpdateElectronicTransactionStatus(currenttblPaymentTransaction, recordStatus);
                }
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return MessageToBeSent;
        }
        private String CreateFooter(tblBPSReference passedtblBPSReference, int passedTotalBodyRecords)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            string MessageToBeSent = string.Empty;

            try
            {
                //Transalate from EPS entity objects to file converter objects
                FooterValues myFooterValues = new FooterValues()
                {
                    RecordType = AgentConstants.Misc.RBC_FOOTER_RECORD_TYPE,
                    ClientNumber = passedtblBPSReference.ClientNumber,
                    TransmitID = passedtblBPSReference.TransmitID,
                    TotalNumberOfBalanceTransfers = passedTotalBodyRecords,
                    TotalRecords = passedTotalBodyRecords + 2
                };
                RBCRemittanceFooterDataValues RBCFooterRecord = Translate.CreateRBCMessageFooterRecord(myFooterValues);
                RBCRemittanceFileFooterProcessor<RBCRemittanceFooterDataValues> RBCFooterProcessor = new RBCRemittanceFileFooterProcessor<RBCRemittanceFooterDataValues>(RBCFooterRecord);

                //Convert to string
                MessageToBeSent = RBCFooterProcessor.SerializeToStringFormat();
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError("Received the exception while process footer for Electronic.. Error Message is->" + ex.Message);

                string logMessage = string.Empty;
                string PaymentStatusForAuditLogging = string.Empty;


                logMessage = "Received the exception while processing footer records for electronic.";

                LoggingHelper.LogErrorActivity(logMessage, ex);

                throw;
            }
            SolutionTraceClass.WriteLineVerbose("End");
            return MessageToBeSent;
        }
        private string SendEmail(string toEmailAddress, string subjectLine, string emailBody)
        {
            SystemServiceWrapper target = new SystemServiceWrapper(); // TODO: Initialize to an appropriate value
            string emailAddress = toEmailAddress;
            string dealId = "";
            string subject = subjectLine;
            string message = emailBody;
            string userId = ""; // TODO: Initialize to an appropriate value


            return target.SendEmail(emailAddress, dealId, subject, message, userId, "");
        }

        #endregion


    }
}
