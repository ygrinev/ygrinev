using FCT.EPS.WSP.GSMA.Resources;
using FCT.EPS.WSP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FCT.EPS.WSP.DataAccess;
using FCT.EPS.DataEntities;
using ChinhDo.Transactions;

namespace FCT.EPS.WSP.GSMA.BusinessLogic
{
    public class BusinessLayer
    {
        public static void ProcessSwiftFilesStatus(FileLocations SwiftFileLocations)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {
                //Credit Process
                ProcessCreditFiles(SwiftFileLocations.SwiftCreditLocation, SwiftFileLocations.SwiftArchiveCreditLocation);

                //Debit Process
                ProcessDebitFiles(SwiftFileLocations.SwiftDebitLocation, SwiftFileLocations.SwiftArchiveDebitLocation);

                //Ack/Nack process
                ProcessAckNackFiles(SwiftFileLocations.SwiftAckNackLocation, SwiftFileLocations.SwiftArchiveAckNackLocation);

                //AutoClient Error process
                ProcessAutoClientErrorFiles(SwiftFileLocations.SwiftAutoClientErrorLocation, SwiftFileLocations.SwiftArchiveAutoClientErrorLocation);

                //Converter Error Process
                ProcessConverterErrorFiles(SwiftFileLocations.SwiftConverterErrorLocation, SwiftFileLocations.SwiftArchiveConverterErrorLocation);
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private static void ProcessConverterErrorFiles(string FileLocation, string FileArchiveLocation)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                TxFileManager fileMgr = new TxFileManager();
                foreach (string File in System.IO.Directory.EnumerateFiles(FileLocation,AgentConstants.Misc.ERR_FILE_MASK))
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        String fileName = System.IO.Path.GetFileName(File);
                        DateTime myDateTime = DateTime.Now;
                        String archiveFileName = System.IO.Path.Combine(new string[] { FileArchiveLocation, myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), fileName });
                        try
                        {
                            //Create the dir
                            fileMgr.CreateDirectory(System.IO.Path.GetDirectoryName(archiveFileName));
                            //Move file to archive
                            fileMgr.Move(File, archiveFileName);
                        }
                        catch (System.IO.FileNotFoundException ex)
                        {
                            SolutionTraceClass.WriteLineInfo(String.Format("'ConverterErrors' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, File));
                            LoggingHelper.LogAuditingActivity(String.Format("'ConverterErrors' File ({0}) could not be moved.", File), ex);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("'ConverterErrors' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, File));
                            LoggingHelper.LogErrorActivity(String.Format("'ConverterErrors' File ({0}) could not be moved.", File), ex);
                            continue;
                        }

                        int myPaymentTransactionID = 0;
                        try
                        {
                            //Determine File Type and action
                            string FileName = System.IO.Path.GetFileName(File);
                            
                            //If payment then set the Payment Transaction to failed, log the error and achive the file.
                            //All others simply archive the file and log the issue.
                            if (FileName.StartsWith(AgentConstants.Misc.WIRE_FILE_STARTS_WITH,StringComparison.CurrentCultureIgnoreCase))
                            {
                                //Get the PyamentTransactionID from the file name
                                int myStatusID = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                string PaymentTransactionIDPartOfFileName = FileName.Substring(AgentConstants.Misc.WIRE_FILE_STARTS_WITH.Length+1);

                                PaymentTransactionIDPartOfFileName = PaymentTransactionIDPartOfFileName.Substring(0,PaymentTransactionIDPartOfFileName.IndexOf("_"));
                                myPaymentTransactionID = Convert.ToInt32(PaymentTransactionIDPartOfFileName);

                                //Update the datebase
                                UnitOfWork myUnitOfWork = new UnitOfWork();
                                tblPaymentTransaction mytblPaymentTransaction = myUnitOfWork.TblPaymentTransactionRepository.GetByID(myPaymentTransactionID);
                                if (mytblPaymentTransaction != null)
                                {
                                    mytblPaymentTransaction.StatusID = myStatusID;
                                    myUnitOfWork.TblPaymentTransactionRepository.Update(mytblPaymentTransaction);
                                    myUnitOfWork.Save();
                                }
                                //Log the error
                                SolutionTraceClass.WriteLineError(String.Format("'ConverterErrors' WIRE transfer failed in the swift converter for tblPaymentTransaction '{1}'.  File location is '{0}'", archiveFileName, myPaymentTransactionID));
                                LoggingHelper.LogErrorActivity(String.Format("'ConverterErrors' WIRE transfer failed in the swift converter for tblPaymentTransaction '{1}'.  File location is '{0}'", archiveFileName, myPaymentTransactionID));
                            }
                            else if (FileName.StartsWith(AgentConstants.Misc.EFT_FILE_STARTS_WITH, StringComparison.CurrentCultureIgnoreCase))
                            {
                                SolutionTraceClass.WriteLineError(String.Format("'ConverterErrors' Transaction failed in the swift converter.  EFT payment type not implemented.  File location is '{0}'", archiveFileName));
                                LoggingHelper.LogErrorActivity(String.Format("'ConverterErrors' Transaction failed in the swift converter.  EFT payment type not implemented.  File location is '{0}'", archiveFileName));
                            }
                            else
                            {
                                //Log the error
                                SolutionTraceClass.WriteLineError(String.Format("'ConverterErrors' Transaction failed in the swift converter.  File location is '{0}'", archiveFileName));
                                LoggingHelper.LogErrorActivity(String.Format("'ConverterErrors' Transaction failed in the swift converter.  File location is '{0}'", archiveFileName));
                            }

                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineError(String.Format("'ConverterErrors' Unable to update that database for tblPaymentException ID '{0}' File location is '{1}'.  Message was->{2}", myPaymentTransactionID, File, ex.Message));
                            LoggingHelper.LogErrorActivity(String.Format("'ConverterErrors' Unable to update that database for tblPaymentException ID '{0}' File location is '{1}'.", myPaymentTransactionID, File), ex);
                            continue;
                        }

                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("'ConverterErrors' Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private static void ProcessAutoClientErrorFiles(string FileLocation, string FileArchiveLocation)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                TxFileManager fileMgr = new TxFileManager();
                foreach (string File in System.IO.Directory.EnumerateFiles(FileLocation))
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        String fileName = System.IO.Path.GetFileName(File);
                        DateTime myDateTime = DateTime.Now;
                        String archiveFileName = System.IO.Path.Combine(new string[] { FileArchiveLocation, myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), fileName });
                        try
                        {
                            //Create the dir
                            fileMgr.CreateDirectory(System.IO.Path.GetDirectoryName(archiveFileName));
                            //Move file to archive
                            fileMgr.Move(File, archiveFileName);
                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("'AutoClientErrors' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, File));
                            LoggingHelper.LogErrorActivity(String.Format("'AutoClientErrors' File ({0}) could not be moved.", File), ex);
                            continue;
                        }

                        //Log the error
                        SolutionTraceClass.WriteLineError(String.Format("'AutoClientErrors' Transaction failed in the swift AutoClient.  File location is '{0}'", archiveFileName));
                        LoggingHelper.LogErrorActivity(String.Format("'AutoClientErrors' Transaction failed in the swift AutoClient.  File location is '{0}'", archiveFileName));

                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("'AutoClientErrors' Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private static void ProcessAckNackFiles(string FileLocation, string FileArchiveLocation)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                TxFileManager fileMgr = new TxFileManager();
                foreach (string File in System.IO.Directory.EnumerateFiles(FileLocation))
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        String fileName = System.IO.Path.GetFileName(File);
                        DateTime myDateTime = DateTime.Now;
                        String archiveFileName = System.IO.Path.Combine(new string[] { FileArchiveLocation, myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), fileName });
                        try
                        {
                            //Create the dir
                            fileMgr.CreateDirectory(System.IO.Path.GetDirectoryName(archiveFileName));
                            //Move file to archive
                            fileMgr.Move(File, archiveFileName);
                        }
                        catch (System.IO.FileNotFoundException ex)
                        {
                            SolutionTraceClass.WriteLineInfo(String.Format("File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, File));
                            LoggingHelper.LogErrorActivity(String.Format("File ({0}) could not be moved.", File), ex);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, File));
                            LoggingHelper.LogErrorActivity(String.Format("File ({0}) could not be moved.", File), ex);
                            continue;
                        }

                        int paymentTransactionID = 0;
                        int messageType = 0;
                        int statusID = 0;
                        string swiftErrorCode = string.Empty;

                        try
                        {
                            //Read the file
                            string fileContents = System.IO.File.ReadAllText(archiveFileName);

                            //Convert File
                            FCT.EPS.Swift.ProcessSwiftAck myProcessSwiftAck = new FCT.EPS.Swift.ProcessSwiftAck();
                            myProcessSwiftAck.GetSwiftACKValues(fileContents);

                            paymentTransactionID = Convert.ToInt32(myProcessSwiftAck.PaymentTransactionID);
                            messageType = Convert.ToInt32(myProcessSwiftAck.SwiftMessageType);

                            if (messageType == AgentConstants.Misc.NACK_STATUS)
                            {
                                statusID = Constants.DataBase.Tables.tblEPSStatus.SWIFTError;
                                swiftErrorCode = myProcessSwiftAck.SwiftErrorCode;
                            }
                            else if (messageType == AgentConstants.Misc.ACK_STATUS)
                            {
                                statusID = Constants.DataBase.Tables.tblEPSStatus.SWIFTReceived;
                            }
                            else
                            {
                                SolutionTraceClass.WriteLineWarning(String.Format("File ({1}) had an unknown ProcessSwiftAck.PaymentTransactionStatus type of {0}", messageType, archiveFileName));
                                LoggingHelper.LogErrorActivity(String.Format("File ({1}) had an unknown ProcessSwiftAck.PaymentTransactionStatus type of {0}", messageType, archiveFileName));
                                scope.Complete();
                                continue;
                            }

                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("File ({1}) could not be read from disk or parsed when converting string values to int.  Exception.  Message was->{0}", ex.Message, archiveFileName));
                            LoggingHelper.LogErrorActivity(String.Format("File ({0}) could not be read from disk or parsed when converting string values to int.", archiveFileName), ex);
                            scope.Complete();
                            continue;
                        }

                        try
                        {
                            //Update the datebase
                            UnitOfWork myUnitOfWork = new UnitOfWork();
                            tblPaymentTransaction mytblPaymentTransaction = myUnitOfWork.TblPaymentTransactionRepository.GetByID(paymentTransactionID);
                            if (mytblPaymentTransaction != null)
                            {
                                mytblPaymentTransaction.StatusID = statusID;

                                UpdateChildRecords(mytblPaymentTransaction, myUnitOfWork);

                                myUnitOfWork.TblPaymentTransactionRepository.Update(mytblPaymentTransaction);
                                myUnitOfWork.Save();

                                if (statusID == Constants.DataBase.Tables.tblEPSStatus.SWIFTError)
                                {
                                    SolutionTraceClass.WriteLineWarning(String.Format("Nack was sent for tblPaymentTransaction record '{1}' in file '{0}' with swift error code of {2}.", File, paymentTransactionID, swiftErrorCode));
                                    LoggingHelper.LogErrorActivity(String.Format("Nack was sent for tblPaymentTransaction record '{1}' in file '{0}' with swift error code of {2}.", File, paymentTransactionID, swiftErrorCode));
                                }
                            }
                            else
                            {
                                SolutionTraceClass.WriteLineWarning(String.Format("Could not find tblPaymentTransaction record '{1}' while processing Ack/Nack file '{0}'.", File, paymentTransactionID));
                                LoggingHelper.LogErrorActivity(String.Format("Could not find tblPaymentTransaction record '{1}' while processing Ack/Nack file '{0}'.", File, paymentTransactionID));
                            }
                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineError(String.Format("Ack/Nack exception when access database.  PaymentTransactionID is '{0}'. File location is '{1}'", paymentTransactionID, File));
                            LoggingHelper.LogErrorActivity(String.Format("Ack/Nack exception when access database.  PaymentTransactionID is '{0}'. File location is '{1}'", paymentTransactionID, File), ex);
                            continue;
                        }

                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private static void UpdateChildRecords(tblPaymentTransaction passedTblPaymentTransaction, UnitOfWork passedUnitOfWork)
        {
            IList<tblPaymentTransaction> mytblPaymentTransactionList = passedTblPaymentTransaction.tblPaymentRequest.First().tblChildPaymentRequest.Select<tblPaymentRequest, tblPaymentTransaction>(c => c.tblPaymentTransaction).ToList();
            foreach (tblPaymentTransaction mytblPaymentTransaction in mytblPaymentTransactionList)
            {
                mytblPaymentTransaction.StatusID = passedTblPaymentTransaction.StatusID;
            }
            passedUnitOfWork.Save();
        }


        private static void ProcessDebitFiles(string FileLocation, string FileArchiveLocation)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                TxFileManager fileMgr = new TxFileManager();

                foreach (string File in System.IO.Directory.EnumerateFiles(FileLocation,AgentConstants.Misc.DEBIT_FILE_MASK))
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        String fileName = System.IO.Path.GetFileName(File);
                        DateTime myDateTime = DateTime.Now;
                        String archiveFileName = System.IO.Path.Combine(new string[] { FileArchiveLocation, myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), fileName });
                        try
                        {
                            //Create the dir
                            fileMgr.CreateDirectory(System.IO.Path.GetDirectoryName(archiveFileName));
                            //Move file to archive
                            fileMgr.Move(File, archiveFileName);
                        }
                        catch (System.IO.FileNotFoundException ex)
                        {
                            SolutionTraceClass.WriteLineInfo(String.Format("'Debit' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, File));
                            LoggingHelper.LogErrorActivity(String.Format("'Debit' File ({0}) could not be moved.", File), ex);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("'Debit' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, File));
                            LoggingHelper.LogErrorActivity(String.Format("'Debit' File ({0}) could not be moved.", File), ex);
                            continue;
                        }

                        //string paymentReferenceNumber = string.Empty;
                        //string originatroAccountNumber = string.Empty;
                        //DateTime? paymentDateTime = null;
                        //decimal amount = 0;
                        //string originatorName = string.Empty;
                        tblPaymentNotification mytblPaymentNotification = null;
                        FCT.EPS.Swift.MT900DataValues myMT900DataValues = null;

                        try
                        {
                            //Read the file
                            string fileContents = System.IO.File.ReadAllText(archiveFileName);

                            //Convert File
                            myMT900DataValues = new FCT.EPS.Swift.MT900DataValues();
                            FCT.EPS.Swift.ProcessMT900File<FCT.EPS.Swift.MT900DataValues> myMT900process = new FCT.EPS.Swift.ProcessMT900File<FCT.EPS.Swift.MT900DataValues>(myMT900DataValues);
                            myMT900process.DeserializeFromSwiftFormat(fileContents, ref myMT900DataValues);


                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("'Debit' File ({1}) could not be read from disk or parsed when converting string values to int.  Exception.  Message was->{0}", ex.Message, archiveFileName));
                            LoggingHelper.LogErrorActivity(String.Format("'Debit' File ({0}) could not be read from disk or parsed when converting string values to int.", archiveFileName), ex);
                            scope.Complete();
                            continue;
                        }

                        try
                        {
                            //Translate/MAP the message
                            mytblPaymentNotification = Translate.tbFCTFeeStatusInfo2tblPaymentNotification(myMT900DataValues);

                            //insert record into the datebase
                            UnitOfWork myUnitOfWork = new UnitOfWork();
                           
                            myUnitOfWork.TblPaymentNotificationRepository.Insert(mytblPaymentNotification);

                            myUnitOfWork.Save();

                            UpdateChildRecords(mytblPaymentNotification, myUnitOfWork);

                        }
                        catch (Exception ex)
                        {
                            if (mytblPaymentNotification == null)
                            {
                                SolutionTraceClass.WriteLineError(String.Format("'Debit' exception when translating file. File location is '{0}'", archiveFileName));
                                LoggingHelper.LogErrorActivity(String.Format("'Debit' exception when translating file.  File location is '{0}'", archiveFileName), ex);
                            }
                            else
                            {
                                SolutionTraceClass.WriteLineError(String.Format("'Debit' exception when access database.  tblPaymentNotification ID is '{0}'. File location is '{1}'", mytblPaymentNotification.PaymentTransactionID, archiveFileName));
                                LoggingHelper.LogErrorActivity(String.Format("'Debit' exception when access database.  tblPaymentNotification ID is '{0}'. File location is '{1}'", mytblPaymentNotification.PaymentTransactionID, archiveFileName), ex);
                            }
                            continue;
                        }

                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("'Debit' Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        private static void UpdateChildRecords(tblPaymentNotification passedtblPaymentNotification, UnitOfWork passedUnitOfWork)
        {
            if(null != passedtblPaymentNotification.PaymentTransactionID)
            {
                tblPaymentRequest parentPaymentRequest = passedUnitOfWork.TblPaymentRequestRepository.Get(pr => (pr.PaymentTransactionID != null && pr.PaymentTransactionID == passedtblPaymentNotification.PaymentTransactionID)).FirstOrDefault();
                if (null != parentPaymentRequest)
                {
                    IList<tblPaymentRequest> childPaymentRequests = passedUnitOfWork.TblPaymentRequestRepository.Get(pr => ( pr.ParentPaymentRequestID != null && pr.ParentPaymentRequestID == parentPaymentRequest.PaymentRequestID)).ToList();
                    foreach (tblPaymentRequest paymentRequest in childPaymentRequests)
                    {
                        //Create PaymentNotification
                        paymentRequest.tblPaymentTransaction.tblPaymentNotification.Add(Translate.NewtblPaymentNotificationForChildren(paymentRequest.tblPaymentTransaction, passedtblPaymentNotification));
                        //Update Parent Request status so that it is not processed
                        passedtblPaymentNotification.StatusID = Constants.DataBase.Tables.tblEPSStatus.SUBMITTED;
                    }
                    passedUnitOfWork.Save();
                }
            }
        }

        private static void ProcessCreditFiles(string FileLocation, string FileArchiveLocation)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                TxFileManager fileMgr = new TxFileManager();
                foreach (string File in System.IO.Directory.EnumerateFiles(FileLocation, AgentConstants.Misc.CREDIT_FILE_MASK))
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        String fileName = System.IO.Path.GetFileName(File);
                        DateTime myDateTime = DateTime.Now;
                        String archiveFileName = System.IO.Path.Combine(new string[] { FileArchiveLocation, myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), fileName });
                        try
                        {
                            //Create the dir
                            fileMgr.CreateDirectory(System.IO.Path.GetDirectoryName(archiveFileName));
                            //Move file to archive
                            fileMgr.Move(File, archiveFileName);
                        }
                        catch (System.IO.FileNotFoundException ex)
                        {
                            SolutionTraceClass.WriteLineInfo(String.Format("'Credit' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, File));
                            LoggingHelper.LogErrorActivity(String.Format("'Credit' File ({0}) could not be moved.", File), ex);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("'Credit' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, File));
                            LoggingHelper.LogErrorActivity(String.Format("'Credit' File ({0}) could not be moved.", File), ex);
                            continue;
                        }

                        string paymentReferenceNumber = string.Empty;
                        string fctTrustAccountNumber = string.Empty;
                        DateTime? paymentDateTime = null;
                        //int statusID = 0;
                        decimal amount = 0;
                        string originatorName = string.Empty;
                        string additionalInformation = string.Empty;

                        try
                        {
                            //Read the file
                            string fileContents = System.IO.File.ReadAllText(archiveFileName);

                            //Convert File
                            FCT.EPS.Swift.MT910DataValues myMT910DataValues = new FCT.EPS.Swift.MT910DataValues();
                            FCT.EPS.Swift.ProcessMT910File<FCT.EPS.Swift.MT910DataValues> myMT910process = new FCT.EPS.Swift.ProcessMT910File<FCT.EPS.Swift.MT910DataValues>(myMT910DataValues);
                            myMT910process.DeserializeFromSwiftFormat(fileContents, ref myMT910DataValues);


                            paymentReferenceNumber = myMT910DataValues.TDCreditTransactionID;
                            fctTrustAccountNumber = myMT910DataValues.ReceiverBankAccountID;

                            paymentDateTime = myMT910DataValues.CreditReceivedDate;
                            amount = Convert.ToDecimal(myMT910DataValues.Amount);
                            originatorName = myMT910DataValues.PayorName;
                            additionalInformation = myMT910DataValues.DealReference;

                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("'Credit' File ({1}) could not be read from disk or parsed when converting string values to int.  Exception.  Message was->{0}", ex.Message, archiveFileName));
                            LoggingHelper.LogErrorActivity(String.Format("'Credit' File ({0}) could not be read from disk or parsed when converting string values to int.", archiveFileName), ex);
                            scope.Complete();
                            continue;
                        }

                        try
                        {
                            UnitOfWork myUnitOfWork = new UnitOfWork();
                            //Check if the record exists already.  If it does then raise the issue, don't insert the record but keep the file achived so it will not be processed again.
                            IList<tblPaymentNotification> mytblPaymentNotifications = myUnitOfWork.TblPaymentNotificationRepository.Get(c => c.PaymentReferenceNumber == paymentReferenceNumber && c.FCTTrustAccountNumber == fctTrustAccountNumber && c.NotificationType == AgentConstants.Misc.CREDIT_NOTIFICATION_TYPE).ToList();
                            if (mytblPaymentNotifications != null && mytblPaymentNotifications.Count() > 0)
                            {
                                string message = String.Format("'Credit' File ({1}) is a duplicate and cannot be processed. PaymentReferenceNumber = '{0}'. FCTTrustAccountNumber = '{2}'. NotificationType = '{3}'", paymentReferenceNumber, archiveFileName, fctTrustAccountNumber, AgentConstants.Misc.CREDIT_NOTIFICATION_TYPE);
                                SolutionTraceClass.WriteLineWarning(message);
                                LoggingHelper.LogErrorActivity(message);
                            }
                            else
                            {
                                //insert record into the datebase
                                tblPaymentNotification mytblPaymentNotification = new tblPaymentNotification()
                                {
                                    PaymentReferenceNumber = paymentReferenceNumber,
                                    FCTTrustAccountNumber = fctTrustAccountNumber,
                                    PaymentDateTime = paymentDateTime,
                                    Amount = amount,
                                    OriginatorName = originatorName,
                                    AdditionalInfo = additionalInformation,
                                    StatusID = Constants.DataBase.Tables.tblEPSStatus.RECEIVED,
                                    LastModifyDate = DateTime.Now,
                                    NotificationType = AgentConstants.Misc.CREDIT_NOTIFICATION_TYPE
                                };

                                myUnitOfWork.TblPaymentNotificationRepository.Insert(mytblPaymentNotification);

                                myUnitOfWork.Save();
                            }
                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineError(String.Format("'Credit' exception when access database.  tblPaymentNotification ID is '{0}'. File location is '{1}'", paymentReferenceNumber, File));
                            LoggingHelper.LogErrorActivity(String.Format("'Credit' exception when access database.  tblPaymentNotification ID is '{0}'. File location is '{1}'", paymentReferenceNumber, File), ex);
                            continue;
                        }

                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("'Credit' Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }
    }
}
