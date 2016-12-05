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
using FCT.EPS.WSP.SCTFA.Resources;
using FCT.EPS.WSP.Resources;
using System.Transactions;

namespace FCT.EPS.WSP.SCTFA.BusinessLogic
{
    public class BusinessLayer
    {

        public static DateTime lastBatchCreatedAt;
        public static void StartProcess(int passedHowManyRecordToGet, String passedEndPointName, int passedMaxRetries)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {

                CreateCheckBatch();

                SendChequesToFinance(passedHowManyRecordToGet, passedEndPointName, passedMaxRetries);

            }
            catch (Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");
        }

        public static void SendChequesToFinance(int passedHowManyRecordToGet, String passedEndPointName, int passedMaxRetries)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {

                int lastRecordID = 0;
                bool StillRecordsLeft = true;
                UnitOfWork myUnitOfWork = new UnitOfWork();
                NonGeneric myNonGeneric = new NonGeneric(myUnitOfWork);


                using (SafeCommunicationDisposal<FinanceServiceClient> myFinanceServiceClient = new SafeCommunicationDisposal<FinanceServiceClient>(new FinanceServiceClient(passedEndPointName)))
                {

                    while (StillRecordsLeft)
                    {
                        //Start a transaction
                        using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                        {
                            StillRecordsLeft = false;

                            //Get cheques from EPS
                            IList<tblPaymentRequest> myPaymentList = myNonGeneric.GetListOfChequePaymentRequestsForFinance(lastRecordID, passedHowManyRecordToGet);

                            //Loop through fees and call web service to insert fees to finance


                            foreach (tblPaymentRequest mytblPaymentRequest in myPaymentList)
                            {
                                ChequeRequestList myChequeRequestList = null;
                                StillRecordsLeft = true;
                                if (myChequeRequestList == null)
                                    myChequeRequestList = new ChequeRequestList();

                                myChequeRequestList.Add(Translate.tblPaymentRequest2ChequeRequest(mytblPaymentRequest));
                                lastRecordID = mytblPaymentRequest.PaymentTransactionID == null ? 0 : (int)mytblPaymentRequest.PaymentTransactionID;

                                int statusID = Constants.DataBase.Tables.tblEPSStatus.FAILED;
                                try
                                {
                                    if (myChequeRequestList != null)
                                    {
                                        myFinanceServiceClient.Instance.CreateCheques(myChequeRequestList);
                                        statusID = Constants.DataBase.Tables.tblEPSStatus.SUBMITTED;
                                    }
                                }
                                catch (System.ServiceModel.FaultException<ServiceFault> ex)
                                {
                                    string Message = String.Format("Exception caused by paymentTransactionID {2}.  Message was->{0}.  Detail was ->{1}.", ex.Message, ex.Detail.Description, mytblPaymentRequest.PaymentTransactionID);
                                    SolutionTraceClass.WriteLineError(Message);
                                    LoggingHelper.LogErrorActivity(Message, ex);
                                }
                                catch (Exception ex)
                                {
                                    string Message = String.Format("Exception caused by paymentTransactionID {1}.  Message was->{0}.", ex.Message, mytblPaymentRequest.PaymentTransactionID);
                                    SolutionTraceClass.WriteLineError(Message);
                                    LoggingHelper.LogErrorActivity(Message, ex);
                                }

                                try
                                {
                                    //Update PaymentTransactionID object of tblPaymentRequest
                                    int tempStatusID = statusID;
                                    if (tempStatusID == Constants.DataBase.Tables.tblEPSStatus.FAILED)
                                    {
                                        if (mytblPaymentRequest.tblPaymentTransaction.NumberRetried < passedMaxRetries)
                                        {
                                            tempStatusID = Constants.DataBase.Tables.tblEPSStatus.RECEIVED;
                                            mytblPaymentRequest.tblPaymentTransaction.NumberRetried++;
                                        }
                                    }
                                    else
                                    {
                                        mytblPaymentRequest.tblPaymentTransaction.DateTransactionSubmitted = DateTime.Now;
                                    }
                                    mytblPaymentRequest.tblPaymentTransaction.StatusID = tempStatusID;
                                    myUnitOfWork.TblPaymentRequestRepository.Update(mytblPaymentRequest);

                                    //Commit all changes.
                                    myUnitOfWork.Save();
                                }
                                catch (Exception ex)
                                {
                                    string Message = String.Format("Exception caused by paymentTransactionID {1}.  Message was->{0}.", ex.Message, mytblPaymentRequest.PaymentTransactionID);
                                    SolutionTraceClass.WriteLineError(Message);
                                    LoggingHelper.LogErrorActivity(Message,ex);
                                    throw;
                                }
                            }
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

        public static void CreateCheckBatch()
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted }))
                {

                    NonGeneric myNonGeneric = new NonGeneric();
                    UnitOfWork myUnitOfWork = new UnitOfWork();

                    List<tblPaymentRequest> myPaymentRequests = myUnitOfWork.TblPaymentRequestRepository.Get(c => c.PaymentMethod == "Cheque" && c.PaymentTransactionID == null).ToList<tblPaymentRequest>();

                    foreach (tblPaymentRequest myPaymentRequest in myPaymentRequests)
                    {
                        myPaymentRequest.tblPaymentTransaction = new tblPaymentTransaction() { StatusID = Constants.DataBase.Tables.tblEPSStatus.RECEIVED, NumberRetried = 0 };
                    }

                    myUnitOfWork.Save();
                    transactionScope.Complete();
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
