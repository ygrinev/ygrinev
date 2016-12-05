using ChinhDo.Transactions;
using FCT.EPS.WSP.GEDMA.Resources;
using FCT.EPS.WSP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FCT.EPS.DataEntities;
using FCT.EPS.FileSerializer.RBC;
using FCT.EPS.WSP.DataAccess;
using FCT.EPS.WSP.ExternalResources;


namespace FCT.EPS.WSP.GEDMA.BusinessLogic
{
    public class BusinessLayer
    {
        private class RejectedFileContents
        {
            public RejectedFileContents()
            {
                Errors = new List<string>();
            }
            public string Header { get; set; }
            public IList<string> Errors { get; private set; }

        }
        //public static void ProcessElectronicDeliveryStatus(string passedPathToElectronicFiles)
        //{
        //    SolutionTraceClass.WriteLineVerbose("Start");
        //    try
        //    {
        //        TxFileManager fileMgr = new TxFileManager();

        //        foreach (string File in System.IO.Directory.EnumerateFiles(passedPathToElectronicFiles, AgentConstants.Misc.ACCEPTED_FILE_MASK))
        //        {
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //    SolutionTraceClass.WriteLineVerbose("End");
        //    throw new NotImplementedException();
        //}
        public static void ProcessElectronicDeliveryRejectedTransactionsFiles(string passedPathToElectronicRejectedTransactionsFiles, string passedArchivePathToElectronicRejectedTransactionsFiles, EmailItems passedReportEmailInfo, string passedElectronicReportFileFolderPath)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {
                TxFileManager fileMgr = new TxFileManager();

                foreach (string File in System.IO.Directory.EnumerateFiles(passedPathToElectronicRejectedTransactionsFiles, AgentConstants.Misc.REJECTED_FILE_MASK))
                {
                    int serviceBatchID = -1;
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                    {
                        String fileName = System.IO.Path.GetFileName(File);
                        //string fileNameExtension = System.IO.Path.GetExtension(fileName);
                        //if (fileNameExtension.IndexOf(".") == 0)
                        //    fileNameExtension = fileNameExtension.Substring(1, fileNameExtension.Length - 1);
                        //try
                        //{
                        //    serviceBatchID = Convert.ToInt32(fileNameExtension); 
                        //}
                        //catch (Exception ex)
                        //{
                        //    SolutionTraceClass.WriteLineInfo(String.Format("'Electronic Rejected Transaction' File ({1}) could not retrieve service batch ID from file name will continue processing the file but no report will be generated.  Exception.  Message was->{0}", ex.Message, File));
                        //    LoggingHelper.LogErrorActivity(String.Format("'Electronic Rejected Transaction' File ({0}) could not retrieve service batch ID from file name will continue processing the file but no report will be generated.", File), ex);
                        //    serviceBatchID = -1;
                        //}
                        DateTime myDateTime = DateTime.Now;
                        String archiveFileName = System.IO.Path.Combine(new string[] { passedArchivePathToElectronicRejectedTransactionsFiles, myDateTime.Year.ToString("0000"), myDateTime.Month.ToString("00"), myDateTime.Day.ToString("00"), fileName });
                        try
                        {
                            //Create the dir
                            fileMgr.CreateDirectory(System.IO.Path.GetDirectoryName(archiveFileName));
                            //Move file to archive
                            fileMgr.Move(File, archiveFileName);
                        }
                        catch (System.IO.FileNotFoundException ex)
                        {
                            SolutionTraceClass.WriteLineInfo(String.Format("'Electronic Rejected Transaction' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, File));
                            LoggingHelper.LogErrorActivity(String.Format("'Electronic Rejected Transaction' File ({0}) could not be moved.", File), ex);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("'Electronic Rejected Transaction' File ({1}) could not be moved.  Exception.  Message was->{0}", ex.Message, File));
                            LoggingHelper.LogErrorActivity(String.Format("'Electronic Rejected Transaction' File ({0}) could not be moved.", File), ex);
                            continue;
                        }


                        IList<RejectedFileContents> myRejectedFileContentsList = new List<RejectedFileContents>();
                        try
                        {
                            //Read the file
                            string[] fileContents = System.IO.File.ReadAllLines(archiveFileName);

                            foreach (string line in fileContents)
                            {
                                if (line.Length > 2 && line.Substring(0, 2) == "05")
                                {
                                    myRejectedFileContentsList.Add(new RejectedFileContents() { Header = line });
                                }
                                else if (line.Length > 2)
                                {
                                    myRejectedFileContentsList.Last().Errors.Add(line);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("'Electronic Rejected Transaction' File ({1}) could not be read from disk.  Exception.  Message was->{0}", ex.Message, archiveFileName));
                            LoggingHelper.LogErrorActivity(String.Format("'Electronic Rejected Transaction' File ({0}) could not be read from disk.", archiveFileName), ex);
                            scope.Complete();
                            continue;
                        }


                        IList<RBCRejectedTranactionsBodyDataValues> myRBCRejectedTranactionsBodyDataValuesList = new List<RBCRejectedTranactionsBodyDataValues>();
                        try
                        {
                            //Transform the lines
                            foreach(RejectedFileContents myRejectedFileContents in  myRejectedFileContentsList)
                            {
                                RBCRejectedTranactionsBodyDataValues myRBCRejectedTranactionsBodyDataValues = new RBCRejectedTranactionsBodyDataValues();
                                RBCRejectedTranactionsFileBodyProcessor<RBCRejectedTranactionsBodyDataValues> myRBCRejectedTranactionsFileBodyProcessor = new RBCRejectedTranactionsFileBodyProcessor<RBCRejectedTranactionsBodyDataValues>(myRBCRejectedTranactionsBodyDataValues);
                                myRBCRejectedTranactionsFileBodyProcessor.DeserializeFromString(myRejectedFileContents.Header, ref myRBCRejectedTranactionsBodyDataValues);
                                foreach(string myErrorString in myRejectedFileContents.Errors)
                                {
                                    myRBCRejectedTranactionsBodyDataValues.ErrorStrings.Add(myErrorString);
                                }
                                myRBCRejectedTranactionsBodyDataValuesList.Add(myRBCRejectedTranactionsBodyDataValues);
                            }
                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("'Electronic Rejected Transaction' File ({1}) could not parse the string to a type of RBCRejectedTranactionsBodyDataValues.  Exception.  Message was->{0}", ex.Message, archiveFileName));
                            LoggingHelper.LogErrorActivity(String.Format("'Electronic Rejected Transaction' File ({0}) could not parse the string to a type of RBCRejectedTranactionsBodyDataValues.", archiveFileName), ex);
                            scope.Complete();
                            continue;
                        }


                        tblPaymentTransaction mytblPaymentTransaction = null;
                        string myPaymentTransactionID = string.Empty;
                        RBCRejectedTranactionsBodyDataValues myTempRBCRejectedTranactionsBodyDataValues = null;
                        try
                        {
                            foreach (RBCRejectedTranactionsBodyDataValues myRBCRejectedTranactionsBodyDataValues in myRBCRejectedTranactionsBodyDataValuesList)
                            {
                                myTempRBCRejectedTranactionsBodyDataValues = myRBCRejectedTranactionsBodyDataValues;
                                myPaymentTransactionID = myRBCRejectedTranactionsBodyDataValues.PaymentReferenceNumber;

                                //insert record into the datebase
                                UnitOfWork myUnitOfWork = new UnitOfWork();

                                //Translate/MAP the message
                                mytblPaymentTransaction = myUnitOfWork.TblPaymentTransactionRepository.GetByID(Convert.ToInt64(myPaymentTransactionID));

                                mytblPaymentTransaction.StatusID = Constants.DataBase.Tables.tblEPSStatus.RBCBPSError;
                                if(null != mytblPaymentTransaction.ServiceBatchID)
                                    serviceBatchID = (int)mytblPaymentTransaction.ServiceBatchID;  // ERROR file is per each service batch.
                                myUnitOfWork.Save();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (mytblPaymentTransaction == null)
                            {
                                SolutionTraceClass.WriteLineError(String.Format("'Electronic Rejected Transaction' exception when updating tblPaymentTransaction no valid  tblPaymentTransaction record. File location is '{0}'. File PaymentTransactionID = {1}.", archiveFileName, myPaymentTransactionID));
                                LoggingHelper.LogErrorActivity(String.Format("'Electronic Rejected Transaction' exception when updating tblPaymentTransaction no valid  tblPaymentTransaction record.  File location is '{0}'. File PaymentTransactionID = {1}.", archiveFileName, myPaymentTransactionID), ex);
                            }
                            else
                            {
                                StringBuilder myStringBuilder = new StringBuilder();
                                foreach(string myError in myTempRBCRejectedTranactionsBodyDataValues.ErrorStrings)
                                {
                                    myStringBuilder.Append(myError + "|");
                                }
                                SolutionTraceClass.WriteLineError(String.Format("'Electronic Rejected Transaction' exception when updating tblPaymentTransaction looking for PaymentTransactionID {1}, PayeeName {2}, Amount {3}, Pay date {4}, Error message {5}. File location is '{0}'", mytblPaymentTransaction.PaymentTransactionID, mytblPaymentTransaction.tblPaymentRequest.First().PayeeName, mytblPaymentTransaction.tblPaymentRequest.First().PaymentAmount, mytblPaymentTransaction.tblPaymentRequest.First().PaymentRequestDate, myStringBuilder.ToString(), archiveFileName));
                                LoggingHelper.LogErrorActivity(String.Format("'Electronic Rejected Transaction' exception when updating tblPaymentTransaction looking for PaymentTransactionID {1}, PayeeName {2}, Amount {3}, Pay date {4}, Error message {5}. File location is '{0}'", mytblPaymentTransaction.PaymentTransactionID, mytblPaymentTransaction.tblPaymentRequest.First().PayeeName, mytblPaymentTransaction.tblPaymentRequest.First().PaymentAmount, mytblPaymentTransaction.tblPaymentRequest.First().PaymentRequestDate, myStringBuilder.ToString(), archiveFileName), ex);
                            }
                            continue;
                        }

                        //Create a new file.
                        if (serviceBatchID > -1)
                        {
                            using (UnitOfWork myUnitOfWork2 = new UnitOfWork())
                            {
                                if (myUnitOfWork2.TblServiceBatchRepository.GetByID(serviceBatchID).tblPaymentTransaction.Where(c => c.StatusID == Constants.DataBase.Tables.tblEPSStatus.PROCESSING).SelectMany<tblPaymentTransaction, tblPaymentRequest>(c => c.tblPaymentRequest).Where(c=>c.tblPayeeInfo.tblCreditorList == null).Count() == 0)
                                {
                                    IList<tblPaymentRequest> thisServiceBatchstblPaymentRequests = myUnitOfWork2.TblServiceBatchRepository.GetByID(serviceBatchID).tblPaymentTransaction.SelectMany(c => c.tblPaymentRequest).OrderBy(p => p.tblPayeeInfo.tblCreditorList.CCIN).ToList();
                                   
                                    using (TransactionScope transactionScope3 = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                                    {
                                        string reportFilePath = BPSTransactionReportGenerator.GenerateReport(thisServiceBatchstblPaymentRequests, passedElectronicReportFileFolderPath);
                                        SendEmail(passedReportEmailInfo.EmailAddress, passedReportEmailInfo.EmailSubject, string.Format(passedReportEmailInfo.EmailBody, System.IO.Path.GetFileName(reportFilePath), System.IO.Path.GetDirectoryName(reportFilePath)), "");
                                        transactionScope3.Complete();
                                    }
                                }
                                else
                                {
                                    SolutionTraceClass.WriteLineError(String.Format("'Electronic Rejected Transaction' cannot produce new report for file '{0}' error file makes references to payee records that don't have a tblCreditorList record.", archiveFileName));
                                    LoggingHelper.LogErrorActivity(String.Format("'Electronic Rejected Transaction' cannot produce new report for file '{0}' error file makes references to payee records that don't have a tblCreditorList record.", archiveFileName));
                                }
                                myUnitOfWork2.Dispose();
                            }
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

        private static void SendEmail(string toEmailAddress, string subjectLine, string emailBody, string passedfilePath)
        {
            SystemServiceWrapper target = new SystemServiceWrapper(); // TODO: Initialize to an appropriate value
            string emailAddress = toEmailAddress;
            string dealId = "";
            string subject = subjectLine;
            string message = emailBody;
            string filePath = passedfilePath;
            string userId = ""; // TODO: Initialize to an appropriate value


            string jobID = target.SendEmail(emailAddress, dealId, subject, message, filePath, userId);
        }

    }
}
