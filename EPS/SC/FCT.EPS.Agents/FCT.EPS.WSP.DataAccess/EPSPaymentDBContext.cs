using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using FCT.EPS.DataEntities;
using FCT.EPS.WSP.Resources;
using System.Linq;


namespace FCT.EPS.WSP.DataAccess
{
    public partial class EPSDBContext : DbContext
    {
        public EPSDBContext(string passedNameOrConnectionString)
            : base(passedNameOrConnectionString)
        {
            Database.SetInitializer<EPSDBContext>(null);
        }

        public EPSDBContext()
            : this(Constants.Misc.DATABASE_CONNECTION_STRING)
        { }
        public virtual DbSet<tblAddress> tblAddress { get; set; }
        public virtual DbSet<tblAgentNames> tblAgentNames { get; set; }
        public virtual DbSet<tblBatchSchedule> tblBatchSchedule { get; set; }
        public virtual DbSet<tblCreditorList> tblCreditorList { get; set; }
        public virtual DbSet<tblCreditorListExcluded> tblCreditorListExcluded { get; set; }
        public virtual DbSet<tblRBCCreditorListStaging> tblRBCCreditorListStaging { get; set; }
        public virtual DbSet<tblCreditorRules> tblCreditorRules { get; set; }
        public virtual DbSet<tblEPSStatus> tblEPSStatus { get; set; }
        public virtual DbSet<tblFCTAccount> tblFCTAccount { get; set; }
        public virtual DbSet<tblFCTFeeSummaryRequest> tblFCTFeeSummaryRequest { get; set; }
        public virtual DbSet<tblPayeeAccount> tblPayeeAccount { get; set; }
        public virtual DbSet<tblPayeeAlias> tblPayeeAlias { get; set; }
        public virtual DbSet<tblPayeeInfo> tblPayeeInfo { get; set; }
        public virtual DbSet<tblPayeeInfoHistory> tblPayeeInfoHistory { get; set; }
        public virtual DbSet<tblPayeeReference> tblPayeeReference { get; set; }
        public virtual DbSet<tblPayeeType> tblPayeeType { get; set; }
        public virtual DbSet<tblPaymentAddress> tblPaymentAddress { get; set; }
        public virtual DbSet<tblPaymentBatchSchedule> tblPaymentBatchSchedule { get; set; }
        public virtual DbSet<tblPaymentFieldInfo> tblPaymentFieldInfo { get; set; }
        public virtual DbSet<tblPaymentFieldReference> tblPaymentFieldReference { get; set; }
        public virtual DbSet<tblPaymentMethod> tblPaymentMethod { get; set; }
        public virtual DbSet<tblPaymentNotification> tblPaymentNotification { get; set; }
        public virtual DbSet<tblPaymentRequest> tblPaymentRequest { get; set; }
        public virtual DbSet<tblPaymentRequestType> tblPaymentRequestType { get; set; }
        public virtual DbSet<tblPaymentServiceProvider> tblPaymentServiceProvider { get; set; }
        public virtual DbSet<tblPaymentStatus> tblPaymentStatus { get; set; }
        public virtual DbSet<tblPaymentTransaction> tblPaymentTransaction { get; set; }
        public virtual DbSet<tblSolution> tblSolution { get; set; }
        public virtual DbSet<tblSolutionSubscription> tblSolutionSubscription { get; set; }
        public virtual DbSet<tblPaymentReport> tblPaymentReport { get; set; }
        public virtual DbSet<tblPaymentReportInfo> tblPaymentReportInfo { get; set; }
        public virtual DbSet<tblPaymentReportFields> tblPaymentReportFields { get; set; }
        public virtual DbSet<tblReportFileFormat> tblReportFileFormat { get; set; }
        public virtual DbSet<tblPaymentReportFieldFormat> tblPaymentReportFieldFormat { get; set; }
        public virtual DbSet<tblPaymentReportLabels> tblPaymentReportLabels { get; set; }
        public virtual DbSet<tblServiceBatch> tblServiceBatch { get; set; }
        public virtual DbSet<tblServiceBatchStatus> tblServiceBatchStatus { get; set; }
        public virtual DbSet<tblPaymentScheduleRunLog> tblPaymentScheduleRunLog { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
     }
}
