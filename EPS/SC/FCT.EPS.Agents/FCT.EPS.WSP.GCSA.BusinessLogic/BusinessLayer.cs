using FCT.EPS.WSP.GCSA.Resources;
using FCT.EPS.WSP.ExternalResources.FinanceService;
using FCT.EPS.WSP.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FCT.EPS.WSP.DataAccess;
using FCT.EPS.DataEntities;

namespace FCT.EPS.WSP.GCSA.BusinessLogic
{
    public class BusinessLayer
    {
        public static void ProcessChequeStatus(int passedNumberOfRecordsToGet, string passedEndpoint)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            try
            {
                bool StillMoreRecords = true;
                int IdOfLastRecord = 0;
                UnitOfWork myUnitOfWork = new UnitOfWork();

                using (SafeCommunicationDisposal<FinanceServiceClient> myFinanceServiceClient = new SafeCommunicationDisposal<FinanceServiceClient>(new FinanceServiceClient(passedEndpoint)))
                {

                    while (StillMoreRecords)
                    {
                        ChequeStatusResponseList myChequeStatusResponseList;
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                        {

                            StillMoreRecords = false;
                            //Get cheque station from finance database
                            myChequeStatusResponseList = myFinanceServiceClient.Instance.GetListOfUpdatedCheques(new ChequeStatusRequest() { EPSChequeRequestID = IdOfLastRecord, NumberOfRecords = passedNumberOfRecordsToGet });
                            SolutionTraceClass.WriteLineVerbose(string.Format("myChequeStatusResponseList returned {0} records", myChequeStatusResponseList.Count));

                            //Loop through the records
                            foreach (ChequeStatusResponse myChequeStatusResponse in myChequeStatusResponseList)
                            {
                                StillMoreRecords = true;
                                //Set the Current records ID
                                IdOfLastRecord = myChequeStatusResponse.EPSChequeRequestID;

                                //Update cheque status in EPS database
                                tblPaymentNotification paymentNotification = Translate.tbEPSChequeRecord2ChequeStatusResponse(myChequeStatusResponse);
                                if (paymentNotification.PaymentStatusCode == AgentConstants.Misc.ERRORED_CHEQUE_STATUS)
                                {
                                    SolutionTraceClass.WriteLineWarning(String.Format("Cheque Failed in Finance database.  PaymentTransactionID = '{0}'", paymentNotification.PaymentTransactionID));
                                    LoggingHelper.LogErrorActivity(String.Format("Cheque Failed in Finance database.  PaymentTransactionID = '{0}'", paymentNotification.PaymentTransactionID));

                                }
                                myUnitOfWork.TblPaymentNotificationRepository.Insert(paymentNotification);

                            }
                            myUnitOfWork.Save();
                            scope.Complete();
                        }
                        myChequeStatusResponseList = null;
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
