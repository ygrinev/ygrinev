using System;
using System.Data;
using FCT.EPS.Swift;
using System.Transactions;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.DataAccess;
using FCT.EPS.WSP.SBWTSA.Resources;
using System.Diagnostics;
using FCT.EPS.DataEntities;
using System.Collections.Generic;
using System.Linq;
using ChinhDo.Transactions;

namespace FCT.EPS.WSP.SBWTSA.BusinessLogic
{
    public static class BatchWireSWIFTNotificationRequestHandler
    {
        public static void SubmitWireRequestToSWIFT(int numberOfRecords, int maxAllowedRetry, string passedDestinationFilePath)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            int? lastPaymentTransactionID = 0;
            bool stillMoreRecords = true;

            try
            {
                while (stillMoreRecords)
                {
                    stillMoreRecords = false;
                    using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                    {
                        UnitOfWork myUnitOfWork = new UnitOfWork();
                        NonGeneric myNonGeneric = new NonGeneric(myUnitOfWork);

                        IList<Grouping> myGroupedList = null;

                        try
                        {
                            //Get the list of records to process
                            IList<tblPaymentRequest> mytblPaymentRequestList = myNonGeneric.GetListOfBatchWireRequests(lastPaymentTransactionID, numberOfRecords);

                            //Merge the batch into one swift record
                            myGroupedList = (from p in mytblPaymentRequestList
                                             group p by new { p.PaymentTransactionID }
                                                 into grp
                                                 select new Grouping()
                                                 {
                                                     PaymentTransactionID = grp.Key.PaymentTransactionID,
                                                     PaymentAmount = grp.Sum(p => p.PaymentAmount)
                                                 }).ToList();
                        }
                        catch (Exception ex)
                        {
                            SolutionTraceClass.WriteLineWarning(String.Format("Exception trying to retieve tblPaymentRequest Records.  Message was->{0}", ex.Message));
                            LoggingHelper.LogErrorActivity("Exception trying to retieve tblPaymentRequest Records.", ex);
                            throw;
                        }

                        foreach (Grouping myPaymentObject in myGroupedList)
                        {
                            lastPaymentTransactionID = myPaymentObject.PaymentTransactionID;
                            stillMoreRecords = true;
                            tblPaymentTransaction mytblPaymentTransaction = null;
                            //Create a different unit of work to handle updates
                            UnitOfWork my2ndUnitOfWork = new UnitOfWork();
                            string mySwiftString = string.Empty;
                            tblPaymentRequest myFirsttblPaymentRequest = null;
                            int statusCode = Constants.DataBase.Tables.tblEPSStatus.SUBMITTED;

                            try
                            {

                                //Get the record I will need to edit
                                mytblPaymentTransaction = my2ndUnitOfWork.TblPaymentTransactionRepository.GetByID(myPaymentObject.PaymentTransactionID);
                                myFirsttblPaymentRequest = mytblPaymentTransaction.tblPaymentRequest.First();

                            }
                            catch (Exception ex)
                            {
                                SolutionTraceClass.WriteLineWarning(String.Format("Exception trying to retieve tblPaymentRequest Records for tblPaymentTransaction '{1}'.  Message was->{0}", ex.Message, myPaymentObject.PaymentTransactionID));
                                LoggingHelper.LogErrorActivity(String.Format("Exception trying to retieve tblPaymentRequest Records for tblPaymentTransaction '{0}'.", myPaymentObject.PaymentTransactionID), ex);
                                statusCode = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                            }

                            try
                            {
                                if (statusCode == Constants.DataBase.Tables.tblEPSStatus.SUBMITTED)
                                {
                                    //Create the swift string.
                                    MT101DataValues myMT101DataValues = Translate.EPSDataToMT101DataValues(myPaymentObject, myFirsttblPaymentRequest);

                                    string missingFields = string.Empty;
                                    ValidateSwift.IsValid(myMT101DataValues, out missingFields);

                                    if (!string.IsNullOrWhiteSpace(missingFields))
                                    {
                                        string message = String.Format("Missing fields swift cannot be processed for tblPaymentTransaction '{1}'.  Message missing fields are ->{0}", missingFields, myPaymentObject.PaymentTransactionID);
                                        SolutionTraceClass.WriteLineWarning(message);
                                        LoggingHelper.LogWarningActivity(message);
                                        throw new Exception(message);
                                    }

                                    mySwiftString = new ProcessMT101File<MT101DataValues>(myMT101DataValues).SerializeToSwiftFormat();
                                }
                            }
                            catch (Exception ex)
                            {
                                SolutionTraceClass.WriteLineWarning(String.Format("Exception trying to convert to MT101 for tblPaymentTransaction '{1}'.  Message was->{0}", ex.Message, myPaymentObject.PaymentTransactionID));
                                LoggingHelper.LogWarningActivity(String.Format("Exception trying to convert to MT101 for tblPaymentTransaction '{0}'.", myPaymentObject.PaymentTransactionID), ex);
                                statusCode = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                            }

                            try
                            {
                                if (statusCode == Constants.DataBase.Tables.tblEPSStatus.SUBMITTED)
                                {
                                    TxFileManager fileMgr = new TxFileManager();
                                    using (TransactionScope transactionScope2 = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                                    {
                                        //Create the swift file and save it.
                                        string myFileName = System.IO.Path.Combine(new string[] { passedDestinationFilePath, myFirsttblPaymentRequest.PaymentMethod + "_" + myPaymentObject.PaymentTransactionID + "_" + myFirsttblPaymentRequest.tblSolutionSubscription.tblFCTAccount.tblSolution.SolutionName + "_" + string.Format("{0:yyyyMMdd_HHmmss_fff}", DateTime.Now) + ".txt" });
                                        System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
                                        fileMgr.WriteAllBytes(myFileName, ascii.GetBytes(mySwiftString));
                                        transactionScope2.Complete();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                SolutionTraceClass.WriteLineWarning(String.Format("Exception trying to create swift file for tblPaymentTransaction '{1}'.  Message was->{0}", ex.Message, myPaymentObject.PaymentTransactionID));
                                LoggingHelper.LogWarningActivity(String.Format("Exception trying to create swift file for tblPaymentTransaction '{0}'.", myPaymentObject.PaymentTransactionID), ex);
                                statusCode = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                            }

                            try
                            {

                                //Update the records in the database
                                if (statusCode == Constants.DataBase.Tables.tblEPSStatus.FAILED)
                                {
                                    mytblPaymentTransaction.NumberRetried++;
                                    if (mytblPaymentTransaction.NumberRetried < maxAllowedRetry)
                                    {
                                        statusCode = Constants.DataBase.Tables.tblEPSStatus.RECEIVED;
                                    }
                                    else
                                    {
                                        LoggingHelper.LogErrorActivity(String.Format("Exception trying to convert to MT101 for tblPaymentTransaction '{0}'.", myPaymentObject.PaymentTransactionID), AgentConstants.Misc.LOGGING_BUSINESS_RULES_PRIORITY);
                                    }
                                }
                                mytblPaymentTransaction.StatusID = statusCode;
                                mytblPaymentTransaction.LastModifiedDate = DateTime.Now;
                                if (statusCode == Constants.DataBase.Tables.tblEPSStatus.SUBMITTED)
                                {
                                    mytblPaymentTransaction.DateTransactionSubmitted = mytblPaymentTransaction.LastModifiedDate;
                                }

                                my2ndUnitOfWork.TblPaymentTransactionRepository.Update(mytblPaymentTransaction);
                                my2ndUnitOfWork.Save();

                            }
                            catch (Exception ex)
                            {
                                SolutionTraceClass.WriteLineWarning(String.Format("Exception trying to update tblPaymentTransaction with id '{1}'.  Message was->{0}", ex.Message, myPaymentObject.PaymentTransactionID));
                                LoggingHelper.LogErrorActivity(String.Format("Exception trying to update tblPaymentTransaction with id '{0}'.", myPaymentObject.PaymentTransactionID), ex);
                                continue;
                            }

                        }

                        myUnitOfWork.Save();
                        transactionScope.Complete();
                    }
                }
            }
            catch(Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception trying create SWIFT batch.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity("Exception trying create SWIFT batch.", ex);
                throw;
            }
            SolutionTraceClass.WriteLineVerbose("End");
        }

        public static void CreateWireSWIFTBatch(int timeSpanInSeconds)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                {

                    NonGeneric myNonGeneric = new NonGeneric();

                    myNonGeneric.CreateBatchWireSWIFTBatch(timeSpanInSeconds);

                    transactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }
    }
}
