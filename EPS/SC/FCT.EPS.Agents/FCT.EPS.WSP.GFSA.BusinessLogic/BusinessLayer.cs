using FCT.EPS.WSP.GFSA.Resources;
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
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;

namespace FCT.EPS.WSP.GFSA.BusinessLogic
{
    public class BusinessLayer
    {
        public static void ProcessFeeStatus(int passedNumberOfRecordsToGet, string passedEndpoint)
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
                        FCTFeeStatusResponse myFCTFeeStatusResponse;
                        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }))
                        {

                            StillMoreRecords = false;
                            //Get cheque station from finance database
                            myFCTFeeStatusResponse = myFinanceServiceClient.Instance.GetFCTFeeStatus(new FCTFeeStatusRequest() { EPSFCTFeeSummaryID = IdOfLastRecord, NumberOfRecords = passedNumberOfRecordsToGet });
                            SolutionTraceClass.WriteLineVerbose(string.Format("myFCTFeeStatusResponse returned {0} records", myFCTFeeStatusResponse.Count));

                            //Loop through the records
                            foreach (FCTFeeStatusInfo myFCTFeeStatusInfo in myFCTFeeStatusResponse)
                            {
                                StillMoreRecords = true;
                                //Set the Current records ID
                                IdOfLastRecord = myFCTFeeStatusInfo.EPSFCTFeeSummaryID;

                                tblPaymentNotification paymentNotification = Translate.tbFCTFeeStatusInfo2tblPaymentNotification(myFCTFeeStatusInfo); ;

                                //if the payment has errored then log this to the event log as an error
                                if (myFCTFeeStatusInfo.ProcessFlag < 0)
                                {
                                    SolutionTraceClass.WriteLineWarning(string.Format("Fee record shows an error in finance database.  Table name 'tbFCTFeeSummary' Primary Key = {0}", myFCTFeeStatusInfo.EPSFCTFeeSummaryID));
                                    LoggingHelper.LogErrorActivity(string.Format("Fee record shows an error in finance database.  Table name 'tbFCTFeeSummary' Primary Key = {0}", myFCTFeeStatusInfo.EPSFCTFeeSummaryID));
                                }

                                myUnitOfWork.TblPaymentNotificationRepository.Insert(paymentNotification);

                            }
                            myUnitOfWork.Save();
                            scope.Complete();
                        }
                        myFCTFeeStatusResponse = null;
                    }
                }
            }
            catch(Exception ex)
            {
                SolutionTraceClass.WriteLineError(String.Format("Exception.  Message was->{0}", ex.Message));
                LoggingHelper.LogErrorActivity(ex);
                throw;
            }

            SolutionTraceClass.WriteLineVerbose("End");

        }
    }
}
