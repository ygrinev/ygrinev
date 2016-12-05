using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.ExternalResources;
using FCT.EPS.WSP.ExternalResources.FinanceService;
using FCT.EPS.WSP.DataAccess;
using FCT.EPS.DataEntities;
using System.ServiceModel;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.SFTFA.Resources;
using System.Transactions;

namespace FCT.EPS.WSP.SFTFA.BusinessLogic
{
    public class BusinessLayer
    {

        public static DateTime lastBatchCreatedAt;
        public static void StartProcess(int passedHowManyRecordToGet, String passedEndPointName, int passedMaxRetries, DateTime passedTimeOfDayToCreateBatch, int passedTimeSpan )
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {

                CreateFeeBatch(passedTimeSpan, passedTimeOfDayToCreateBatch);

                SendFeesToFinance(passedHowManyRecordToGet, passedEndPointName, passedMaxRetries);

            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(String.Format("Exception.  Message was->{0}", ex.Message), ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        public static void CreateFeeBatch(int timeSpanInSeconds, DateTime passedTimeOfDayToCreateBatch)
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                {

                    NonGeneric myNonGeneric = new NonGeneric();

                    myNonGeneric.CreateFeeBatch(timeSpanInSeconds, passedTimeOfDayToCreateBatch);

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

        public static void SendFeesToFinance(int passedHowManyRecordToGet, String passedEndPointName, int passedMaxRetries)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {

                int lastRecordID = 0;
                bool StillRecordsLeft = true;
                UnitOfWork myUnitOfWork = new UnitOfWork();
                NonGeneric myNonGeneric = new NonGeneric(myUnitOfWork);

                using (SafeCommunicationDisposal<FinanceServiceClient> myFinanceServiceClient = new SafeCommunicationDisposal<FinanceServiceClient> ( new FinanceServiceClient(passedEndPointName)))
                {
                    while (StillRecordsLeft)
                    {
                        //Start a transaction
                        using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                        {
                            StillRecordsLeft = false;

                            //Get Fees from EPS
                            IList<tblPaymentRequest> myPaymentList = myNonGeneric.GetListOfFeePaymentRequestsForFinance(lastRecordID, passedHowManyRecordToGet);


                            //Loop through fees and call web service to insert fees to finance

                            FCTFeeSummaryRequestList myFCTFeeSummaryRequestList = null;
                            foreach (tblPaymentRequest mytblPaymentRequest in myPaymentList)
                            {
                                StillRecordsLeft = true;
                                if (myFCTFeeSummaryRequestList == null) myFCTFeeSummaryRequestList = new FCTFeeSummaryRequestList();

                                myFCTFeeSummaryRequestList.Add(Translate.tblPaymentRequest2FCTFeeSummaryRequest(mytblPaymentRequest));
                                lastRecordID = mytblPaymentRequest.PaymentTransactionID == null ? 0 : (int)mytblPaymentRequest.PaymentTransactionID;

                                int statusID = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                try
                                {
                                    if (myFCTFeeSummaryRequestList != null)
                                    {
                                        myFinanceServiceClient.Instance.CreateFees(myFCTFeeSummaryRequestList);
                                        statusID = Constants.DataBase.Tables.tblEPSStatus.SUBMITTED;
                                    }
                                }
                                catch (System.ServiceModel.FaultException<ServiceFault> ex)
                                {
                                    SolutionTraceClass.WriteLineError(String.Format("Exception caused by paymentTransactionID {2}.  Message was->{0}.  Detail was ->{1}.", ex.Message, ex.Detail.Description, mytblPaymentRequest.PaymentTransactionID));
                                    LoggingHelper.LogErrorActivity(ex);
                                }
                                catch (Exception ex)
                                {
                                    SolutionTraceClass.WriteLineError(String.Format("Exception caused by paymentTransactionID {1}.  Message was->{0}.", ex.Message, mytblPaymentRequest.PaymentTransactionID));
                                    LoggingHelper.LogErrorActivity(ex);
                                }

                                try
                                {
                                    tblPaymentTransaction mytblPaymentTransaction = myUnitOfWork.TblPaymentTransactionRepository.GetByID(lastRecordID);
                                    int tempStatusID = statusID;
                                    if (tempStatusID == Constants.DataBase.Tables.tblEPSStatus.FAILED)
                                    {
                                        if (mytblPaymentTransaction.NumberRetried < passedMaxRetries)
                                        {
                                            tempStatusID = Constants.DataBase.Tables.tblEPSStatus.RECEIVED;
                                            mytblPaymentTransaction.NumberRetried++;
                                        }
                                    }
                                    mytblPaymentTransaction.StatusID = tempStatusID;
                                    myUnitOfWork.TblPaymentTransactionRepository.Update(mytblPaymentTransaction);

                                    //Commit all changes.
                                    myUnitOfWork.Save();
                                }
                                catch (Exception ex)
                                {
                                    SolutionTraceClass.WriteLineError(String.Format("Exception caused by paymentTransactionID {1}.  Message was->{0}.", ex.Message, mytblPaymentRequest.PaymentTransactionID));
                                    LoggingHelper.LogErrorActivity(ex);
                                    throw;
                                }
                            }
                            //Commit transaction
                            transactionScope.Complete();
                        }
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
    }
}
