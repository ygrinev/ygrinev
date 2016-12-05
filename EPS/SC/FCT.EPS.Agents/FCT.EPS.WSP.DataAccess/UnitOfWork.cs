using FCT.EPS.DataEntities;
using FCT.EPS.WSP.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.DataAccess
{
    public class UnitOfWork : IDisposable, FCT.EPS.WSP.DataAccess.IUnitOfWork
    {
        private EPSDBContext context;
        private GenericRepository<tblAddress> _tblAddressReporitory;
        private GenericRepository<tblAgentNames> _tblAgentNamesReporitory;
        private GenericRepository<tblCreditorList> _tblCreditorListRepository;
        private GenericRepository<tblCreditorListExcluded> _tblCreditorListExcludedRepository;
        private GenericRepository<tblCreditorRules> _tblCreditorRulesRepository;
        private GenericRepository<tblFCTAccount> _tblFCTAccountRepository;
        private GenericRepository<tblPayeeInfo> _tblPayeeInfoRepository;
        private GenericRepository<tblPaymentRequest> _tblPaymentRequestRepository;
        private GenericRepository<tblPaymentTransaction> _tblPaymentTransactionRepository;
        private GenericRepository<tblPaymentNotification> _tblPaymentNotificationRepository;
        private GenericRepository<tblPaymentServiceProvider> _tblPaymentServiceProviderRepository;
        private GenericRepository<tblPaymentAddress> _tblPaymentAddressRepository;
        private GenericRepository<tblPaymentBatchSchedule> _tblPaymentBatchScheduleRepository;
        private GenericRepository<tblPaymentStatus> _tblPaymentStatusRepository;
        private GenericRepository<tblPaymentReport> _tblPaymentReportRepository;
        private GenericRepository<tblPaymentReportInfo> _tblPaymentReportInfoRepository;
        private GenericRepository<tblPaymentScheduleRunLog> _tblPaymentScheduleRunLogRepository;
        private GenericRepository<tblRBCCreditorListStaging> _tblRBCCreditorListStagingRepository;
        private GenericRepository<tblSolutionSubscription> _tblSolutionSubscriptionRepository;
        private GenericRepository<tblSolution> _tblSolutionRepository;
        private GenericRepository<tblServiceBatch> _tblServiceBatchRepository;
        private GenericRepository<tblServiceBatchStatus> _tblServiceBatchStatusRepository;

        public UnitOfWork(string passedNameOrConnectionString)
        {
            SolutionTraceClass.WriteLineVerbose("Start");
            if (context == null)
            {
                if (string.IsNullOrEmpty(passedNameOrConnectionString))
                {
                    context = new EPSDBContext();
                }
                else
                {
                    context = new EPSDBContext(passedNameOrConnectionString);
                }
            }
            SolutionTraceClass.WriteLineInfo(string.Format("The Connection string is '{0}'", context.Database.Connection.ConnectionString));

            SolutionTraceClass.WriteLineVerbose("End");
        }

        public UnitOfWork() : this("")
        {
        }

        public EPSDBContext Context
        {
            get
            {
                return context;
            }
        }

        #region GenericRepositories
        public GenericRepository<tblCreditorList> TblCreditorListRepository
        {
            get
            {

                if (this._tblCreditorListRepository == null)
                {
                    this._tblCreditorListRepository = new GenericRepository<tblCreditorList>(context);
                }
                return _tblCreditorListRepository;
            }
        }
        public GenericRepository<tblCreditorListExcluded> TblCreditorListExcludedRepository
        {
            get
            {

                if (this._tblCreditorListExcludedRepository == null)
                {
                    this._tblCreditorListExcludedRepository = new GenericRepository<tblCreditorListExcluded>(context);
                }
                return _tblCreditorListExcludedRepository;
            }
        }
        public GenericRepository<tblCreditorRules> TblCreditorRulesRepository
        {
            get
            {

                if (this._tblCreditorRulesRepository == null)
                {
                    this._tblCreditorRulesRepository = new GenericRepository<tblCreditorRules>(context);
                }
                return _tblCreditorRulesRepository;
            }
        }
        public GenericRepository<tblRBCCreditorListStaging> TblRBCCreditorListStagingRepository
        {
            get
            {

                if (this._tblRBCCreditorListStagingRepository == null)
                {
                    this._tblRBCCreditorListStagingRepository = new GenericRepository<tblRBCCreditorListStaging>(context);
                }
                return _tblRBCCreditorListStagingRepository;
            }
        }
        public GenericRepository<tblPaymentRequest> TblPaymentRequestRepository
        {
            get
            {

                if (this._tblPaymentRequestRepository == null)
                {
                    this._tblPaymentRequestRepository = new GenericRepository<tblPaymentRequest>(context);
                }
                return _tblPaymentRequestRepository;
            }
        }
        public GenericRepository<tblPaymentNotification> TblPaymentNotificationRepository
        {
            get
            {

                if (this._tblPaymentNotificationRepository == null)
                {
                    this._tblPaymentNotificationRepository = new GenericRepository<tblPaymentNotification>(context);
                }
                return _tblPaymentNotificationRepository;
            }
        }
        public GenericRepository<tblPaymentTransaction> TblPaymentTransactionRepository
        {
            get
            {

                if (this._tblPaymentTransactionRepository == null)
                {
                    this._tblPaymentTransactionRepository = new GenericRepository<tblPaymentTransaction>(context);
                }
                return _tblPaymentTransactionRepository;
            }
        }
        public GenericRepository<tblFCTAccount> TblFCTAccountRepository
        {
            get
            {

                if (this._tblFCTAccountRepository == null)
                {
                    this._tblFCTAccountRepository = new GenericRepository<tblFCTAccount>(context);
                }
                return _tblFCTAccountRepository;
            }
        }
        public GenericRepository<tblSolution> TblSolutionRepository
        {
            get
            {

                if (this._tblSolutionRepository == null)
                {
                    this._tblSolutionRepository = new GenericRepository<tblSolution>(context);
                }
                return _tblSolutionRepository;
            }
        }
        public GenericRepository<tblPaymentStatus> TblPaymentStatusRepository
        {
            get
            {

                if (this._tblPaymentStatusRepository == null)
                {
                    this._tblPaymentStatusRepository = new GenericRepository<tblPaymentStatus>(context);
                }
                return _tblPaymentStatusRepository;
            }
        }
        public GenericRepository<tblPaymentServiceProvider> TblPaymentServiceProviderRepository
        {
            get
            {
                if (this._tblPaymentServiceProviderRepository == null)
                {
                    this._tblPaymentServiceProviderRepository = new GenericRepository<tblPaymentServiceProvider>(context);
                }
                return _tblPaymentServiceProviderRepository;
            }
        }
        public GenericRepository<tblPaymentAddress> TblPaymentAddressRepository
        {
            get
            {
                if (this._tblPaymentAddressRepository == null)
                {
                    this._tblPaymentAddressRepository = new GenericRepository<tblPaymentAddress>(context);
                }
                return _tblPaymentAddressRepository;
            }
        }
        public GenericRepository<tblSolutionSubscription> TblSolutionSubscriptionRepository
        {
            get
            {

                if (this._tblSolutionSubscriptionRepository == null)
                {
                    this._tblSolutionSubscriptionRepository = new GenericRepository<tblSolutionSubscription>(context);
                }
                return _tblSolutionSubscriptionRepository;
            }
        }
        public GenericRepository<tblPaymentBatchSchedule> TblPaymentBatchScheduleRepository
        {
            get
            {

                if (this._tblPaymentBatchScheduleRepository == null)
                {
                    this._tblPaymentBatchScheduleRepository = new GenericRepository<tblPaymentBatchSchedule>(context);
                }
                return _tblPaymentBatchScheduleRepository;
            }
        }
        public GenericRepository<tblPaymentReport> TblPaymentReportRepository
        {
            get
            {

                if (this._tblPaymentReportRepository == null)
                {
                    this._tblPaymentReportRepository = new GenericRepository<tblPaymentReport>(context);
                }
                return _tblPaymentReportRepository;
            }
        }
        public GenericRepository<tblPaymentReportInfo> TblPaymentReportInfoRepository
        {
            get
            {

                if (this._tblPaymentReportInfoRepository == null)
                {
                    this._tblPaymentReportInfoRepository = new GenericRepository<tblPaymentReportInfo>(context);
                }
                return _tblPaymentReportInfoRepository;
            }
        }
        public GenericRepository<tblPayeeInfo> TblPayeeInfoRepository
        {
            get
            {

                if (this._tblPayeeInfoRepository == null)
                {
                    this._tblPayeeInfoRepository = new GenericRepository<tblPayeeInfo>(context);
                }
                return _tblPayeeInfoRepository;
            }
        }
        public GenericRepository<tblPaymentScheduleRunLog> TblPaymentScheduleRunLogRepository
        {
            get
            {

                if (this._tblPaymentScheduleRunLogRepository == null)
                {
                    this._tblPaymentScheduleRunLogRepository = new GenericRepository<tblPaymentScheduleRunLog>(context);
                }
                return _tblPaymentScheduleRunLogRepository;
            }
        }
        public GenericRepository<tblServiceBatchStatus> TblServiceBatchStatusRepository
        {
            get
            {

                if (this._tblServiceBatchStatusRepository == null)
                {
                    this._tblServiceBatchStatusRepository = new GenericRepository<tblServiceBatchStatus>(context);
                }
                return _tblServiceBatchStatusRepository;
            }
        }
        public GenericRepository<tblServiceBatch> TblServiceBatchRepository
        {
            get
            {

                if (this._tblServiceBatchRepository == null)
                {
                    this._tblServiceBatchRepository = new GenericRepository<tblServiceBatch>(context);
                }
                return _tblServiceBatchRepository;
            }
        }
        public GenericRepository<tblAgentNames> TblAgentNamesRepository
        {
            get
            {

                if (this._tblAgentNamesReporitory == null)
                {
                    this._tblAgentNamesReporitory = new GenericRepository<tblAgentNames>(context);
                }
                return _tblAgentNamesReporitory;
            }
        }
        public GenericRepository<tblAddress> TblAddressRepository
        {
            get
            {

                if (this._tblAddressReporitory == null)
                {
                    this._tblAddressReporitory = new GenericRepository<tblAddress>(context);
                }
                return _tblAddressReporitory;
            }
        }
        #endregion

        public UnitOfWork(EPSDBContext passedEPSPaymentDBContext)
        {
            context = passedEPSPaymentDBContext;
        }
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
