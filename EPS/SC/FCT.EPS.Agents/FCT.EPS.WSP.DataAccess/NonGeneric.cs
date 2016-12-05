
using FCT.EPS.DataEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.SqlServer;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.DataAccess
{

    public class NonGeneric
    {
        UnitOfWork myUnitOfWork;

        public NonGeneric(UnitOfWork passedUnitOfWork)
        {
            myUnitOfWork = passedUnitOfWork;
        }

        public NonGeneric()
            : this(new UnitOfWork())
        { }

        public EPSDBContext Context
        {
            get
            {
                return myUnitOfWork.Context;
            }
        }

        #region Payment Request
        public IList<tblPaymentRequest> GetListOfFeePaymentRequestsForFinance(int passedPaymentRequestID, int NumberOfRecords)
        {
            return myUnitOfWork.TblPaymentRequestRepository.SqlQuery("dbo.spGetFCTFeePaymentRequest @PaymentRequestID, @NumberOfRecords", new SqlParameter() { ParameterName = "@PaymentRequestID", Value = passedPaymentRequestID }, new SqlParameter() { ParameterName = "@NumberOfRecords", Value = NumberOfRecords }).ToList<tblPaymentRequest>();
        }

        public IList<tblPaymentRequest> GetListOfChequePaymentRequestsForFinance(int passedPaymentRequestID, int NumberOfRecords)
        {
            return myUnitOfWork.TblPaymentRequestRepository.SqlQuery
                ("dbo.spGetFCTChequePaymentRequest @PaymentRequestID, @NumberOfRecords",
                new SqlParameter() { ParameterName = "@PaymentRequestID", Value = passedPaymentRequestID },
                new SqlParameter() { ParameterName = "@NumberOfRecords", Value = NumberOfRecords }
                ).ToList<tblPaymentRequest>();
        }

        public IList<tblPaymentRequest> GetListOfSingleWirePaymentRequest(int? paymentTransactionID, int numberOfRecords)
        {
            return myUnitOfWork.TblPaymentRequestRepository.SqlQuery("dbo.spGetFCTSingleWirePaymentRequest @PaymentTransactionID, @NumberOfRecords", new SqlParameter() { ParameterName = "@PaymentTransactionID", Value = paymentTransactionID }, new SqlParameter() { ParameterName = "@NumberOfRecords", Value = numberOfRecords }).ToList<tblPaymentRequest>();
        }

        public IList<tblPaymentRequest> GetListOfBatchWireRequests(int? lastPaymentTransactionID, int numberOfRecords)
        {
            return myUnitOfWork.TblPaymentRequestRepository.SqlQuery("dbo.spGetBatchWirePaymentRequest @LastPaymentTransactionID, @NumberOfNotifications", new SqlParameter() { ParameterName = "@LastPaymentTransactionID", Value = lastPaymentTransactionID }, new SqlParameter() { ParameterName = "@NumberOfNotifications", Value = numberOfRecords }).ToList<tblPaymentRequest>();
        }
        #endregion

        #region Create  Batch
        public void CreateFCTFeeTransactionBatch()
        {
            myUnitOfWork.Context.Database.ExecuteSqlCommand("dbo.spCreatetblPaymentTransactionRecordsForFeesIntblFCTPaymentRequest");
        }

        public void CreateSingleWireSWIFTBatch()
        {
            myUnitOfWork.Context.Database.ExecuteSqlCommand("dbo.spCreateSingleWireTransactionBatch");
        }

        public void CreateBatchWireSWIFTBatch(int timeSpanInSeconds)
        {
            myUnitOfWork.Context.Database.ExecuteSqlCommand("dbo.spCreateBatchWireTransaction @TimeSpanInSeconds", new SqlParameter() { ParameterName = "@TimeSpanInSeconds", Value = timeSpanInSeconds });
        }

        public void CreateFeeBatch(int timeSpanInSeconds, DateTime passedTimeOfDayToCreateBatch)
        {
            myUnitOfWork.Context.Database.ExecuteSqlCommand("dbo.spCreateFeesBatchTransaction @TimeSpanInSeconds, @TimeToCreateBatch", new SqlParameter() { ParameterName = "TimeSpanInSeconds", Value = timeSpanInSeconds }, new SqlParameter() { ParameterName = "TimeToCreateBatch", Value = passedTimeOfDayToCreateBatch.TimeOfDay });
        }

        public void CreateElectronicBatch()
        {
            myUnitOfWork.Context.Database.ExecuteSqlCommand("dbo.spCreateElectronicTransactionBatch");
        }
        #endregion

        #region Payment Notification Request
        public IList<tblPaymentNotification> GetListOfCreditNotificationRequest(int lastNotificationID, int numberOfNotifications)
        {
            return myUnitOfWork.TblPaymentNotificationRepository.SqlQuery("dbo.spGetCreditNotificationRequest @LastNotificationID, @NumberOfNotifications", new SqlParameter() { ParameterName = "@LastNotificationID", Value = lastNotificationID }, new SqlParameter() { ParameterName = "@NumberOfNotifications", Value = numberOfNotifications }).ToList<tblPaymentNotification>();
        }

        public IList<tblPaymentNotification> GetListOfPaymentNotificationRequest(int? lastNotificationID, int numberOfRecords)
        {
            return myUnitOfWork.TblPaymentNotificationRepository.SqlQuery("dbo.spGetPaymentNotificationRequest @LastNotificationID, @NumberOfNotifications", new SqlParameter() { ParameterName = "@LastNotificationID", Value = lastNotificationID }, new SqlParameter() { ParameterName = "@NumberOfNotifications", Value = numberOfRecords }).ToList<tblPaymentNotification>();
        }
        #endregion

        #region ReportMethods
        public IList<tblBatchPaymentReportInfo> GetACollectionOfReportDictionaries(int passedPaymentRequestID)
        {
            return myUnitOfWork.Context.Database.SqlQuery<tblBatchPaymentReportInfo>("dbo.spKeyValueReportData @PaymentTransactionID",
                new SqlParameter() { ParameterName = "@PaymentTransactionID", Value = passedPaymentRequestID }).ToList<tblBatchPaymentReportInfo>();
        }

        public IList<tblPaymentReport> GetEPSReportStatusRecords(int? passedlastPaymentTransactionID, int passedhowManyPaymentTransactionRecordsToGet)
        {
            return myUnitOfWork.TblPaymentReportRepository.SqlQuery("dbo.spGetRecordsThatNeedAReport @LastPaymentTransactionID,@HowManyPaymentTransactionRecordsToGet",
                new SqlParameter() { ParameterName = "@LastPaymentTransactionID", Value = passedlastPaymentTransactionID },
                new SqlParameter() { ParameterName = "@HowManyPaymentTransactionRecordsToGet", Value = passedhowManyPaymentTransactionRecordsToGet }).ToList<tblPaymentReport>();
        }
        #endregion

        public IList<tblPaymentTransaction> GetListOfElectronicRequest(int timeSpanInSeconds, DateTime passedTimeOfDayToCreateBatch)
        {
            return myUnitOfWork.TblPaymentTransactionRepository.SqlQuery("dbo.spGetFCTElectronicPaymentRequest @TimeSpanInSeconds, @TimeToCreateBatch", new SqlParameter() { ParameterName = "TimeSpanInSeconds", Value = timeSpanInSeconds }, new SqlParameter() { ParameterName = "TimeToCreateBatch", Value = passedTimeOfDayToCreateBatch.TimeOfDay }).ToList<tblPaymentTransaction>();
        }
        public IList<tblServiceBatchStatus> GetListOfElectronicRequestToSubmit()
        {
            return myUnitOfWork.TblServiceBatchStatusRepository.SqlQuery("dbo.spGetListOfElectronicRequestToSubmit").ToList<tblServiceBatchStatus>();
        }
        public IList<tblAgentNames> GetAgentRecord(string passedAgentName)
        {
            return myUnitOfWork.TblAgentNamesRepository.SqlQuery("dbo.spGetAgentIDEx @AgentName",
                new SqlParameter() { ParameterName = "@AgentName", Value = passedAgentName }).ToList<tblAgentNames>();
        }
        public Dictionary<string, List<string>> GetQListCreditorListStaging(DataTable dtNewCCKeys)
        {
            var db = Context.Database;
            var cmd = db.Connection.CreateCommand();
            cmd.CommandText = "dbo.spQListsCreditorListStaging";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter() { ParameterName = "@listCreditors", Value = dtNewCCKeys, SqlDbType = SqlDbType.Structured, TypeName = "dbo.udt_CreditorKey" });
            try
            {
                Dictionary<string, List<string>> ret = new Dictionary<string, List<string>>();
                db.Connection.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    foreach( string op in new string[]{ "UPDATE", "INSERT", "DELETE"})
                    {
                        ret[op] = new List<string>();
                        while(reader.Read())
                        {
                            ret[op].Add(reader.GetString(0));
                        }
                        reader.NextResult();
                    }
                }
                return ret;
            }
            finally
            {
                db.Connection.Close();
            }
        }
    }
}
