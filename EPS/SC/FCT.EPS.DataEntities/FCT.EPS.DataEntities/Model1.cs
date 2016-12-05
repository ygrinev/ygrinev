namespace FCT.EPS.DataEntities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<tblAddress> tblAddress { get; set; }
        public virtual DbSet<tblBatchSchedule> tblBatchSchedule { get; set; }
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblAddress>()
                .Property(e => e.UnitNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblAddress>()
                .Property(e => e.StreetNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblAddress>()
                .Property(e => e.StreetAddress1)
                .IsUnicode(false);

            modelBuilder.Entity<tblAddress>()
                .Property(e => e.StreetAddress2)
                .IsUnicode(false);

            modelBuilder.Entity<tblAddress>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<tblAddress>()
                .Property(e => e.Province)
                .IsUnicode(false);

            modelBuilder.Entity<tblAddress>()
                .Property(e => e.ProvinceCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblAddress>()
                .Property(e => e.PostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblAddress>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<tblAddress>()
                .HasMany(e => e.tblPayeeAccount)
                .WithOptional(e => e.tblAddress)
                .HasForeignKey(e => e.AccountAddressID);

            modelBuilder.Entity<tblAddress>()
                .HasMany(e => e.tblPayeeInfo)
                .WithOptional(e => e.tblAddress)
                .HasForeignKey(e => e.PayeeAddressID);

            modelBuilder.Entity<tblEPSStatus>()
                .Property(e => e.StatusCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblEPSStatus>()
                .HasMany(e => e.tblPaymentNotification)
                .WithRequired(e => e.tblEPSStatus)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblEPSStatus>()
                .HasMany(e => e.tblPaymentRequest)
                .WithRequired(e => e.tblEPSStatus)
                .HasForeignKey(e => e.PaymentStatusID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblEPSStatus>()
                .HasMany(e => e.tblSolutionSubscription)
                .WithOptional(e => e.tblEPSStatus)
                .HasForeignKey(e => e.ServiceStatusID);

            modelBuilder.Entity<tblFCTAccount>()
                .Property(e => e.AccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblFCTAccount>()
                .Property(e => e.BankName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblFCTAccount>()
                .Property(e => e.TransitNumber)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblFCTAccount>()
                .Property(e => e.BranchNumber)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblFCTAccount>()
                .HasMany(e => e.tblSolutionSubscription)
                .WithRequired(e => e.tblFCTAccount)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblFCTFeeSummaryRequest>()
                .Property(e => e.LawyerCRMReference)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeAccount>()
                .Property(e => e.BankName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeAccount>()
                .Property(e => e.BankNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeAccount>()
                .Property(e => e.TransitNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeAccount>()
                .Property(e => e.AccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeAccount>()
                .Property(e => e.BankSWIFTCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeAccount>()
                .Property(e => e.CanadianClearingCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeAlias>()
                .Property(e => e.Alias)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfo>()
                .Property(e => e.PayeeName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfo>()
                .Property(e => e.BankAccountHolderName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfo>()
                .Property(e => e.PayeeChequeName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfo>()
                .Property(e => e.PayeeContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfo>()
                .Property(e => e.PayeeContactPhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfo>()
                .Property(e => e.PayeeEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfo>()
                .Property(e => e.PayeeComments)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfo>()
                .HasMany(e => e.tblBatchSchedule)
                .WithRequired(e => e.tblPayeeInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPayeeInfo>()
                .HasMany(e => e.tblPayeeAlias)
                .WithRequired(e => e.tblPayeeInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPayeeInfo>()
                .HasMany(e => e.tblPayeeInfoHistory)
                .WithRequired(e => e.tblPayeeInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPayeeInfo>()
                .HasMany(e => e.tblPayeeReference)
                .WithRequired(e => e.tblPayeeInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPayeeInfoHistory>()
                .Property(e => e.PayeeName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfoHistory>()
                .Property(e => e.PayeeType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfoHistory>()
                .Property(e => e.PaymendMethod)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfoHistory>()
                .Property(e => e.PaymentRequestType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfoHistory>()
                .Property(e => e.PayeeAddress)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfoHistory>()
                .Property(e => e.PayeeContact)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfoHistory>()
                .Property(e => e.PayeeContactPhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfoHistory>()
                .Property(e => e.PayeeEmail)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeInfoHistory>()
                .Property(e => e.PayeeComments)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeType>()
                .Property(e => e.PayeeTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPayeeType>()
                .HasMany(e => e.tblPayeeReference)
                .WithRequired(e => e.tblPayeeType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPayeeType>()
                .HasMany(e => e.tblPaymentFieldReference)
                .WithRequired(e => e.tblPayeeType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPaymentAddress>()
                .Property(e => e.UnitNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentAddress>()
                .Property(e => e.StreetNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentAddress>()
                .Property(e => e.StreetAddress1)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentAddress>()
                .Property(e => e.StreetAddress2)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentAddress>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentAddress>()
                .Property(e => e.ProvinceCode)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentAddress>()
                .Property(e => e.Province)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentAddress>()
                .Property(e => e.PostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentAddress>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentAddress>()
                .HasMany(e => e.tblPaymentRequest)
                .WithOptional(e => e.tblPaymentAddress)
                .HasForeignKey(e => e.PayeeBranchAddressID);

            modelBuilder.Entity<tblPaymentAddress>()
                .HasMany(e => e.tblPaymentRequest1)
                .WithOptional(e => e.tblPaymentAddress1)
                .HasForeignKey(e => e.PayeeAddressID);

            modelBuilder.Entity<tblPaymentFieldInfo>()
                .Property(e => e.PaymentFieldDefinition)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentFieldInfo>()
                .Property(e => e.FieldLabelEn)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentFieldInfo>()
                .Property(e => e.FieldLabelFr)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentFieldInfo>()
                .HasMany(e => e.tblPaymentFieldReference)
                .WithRequired(e => e.tblPaymentFieldInfo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPaymentFieldReference>()
                .Property(e => e.FieldLabelEn)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentFieldReference>()
                .Property(e => e.FieldLabelFr)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentMethod>()
                .Property(e => e.PaymentMethodName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.PaymentReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.FCTTrustAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.OriginatorName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.OriginalorAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.NotificationType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.AdditionalInfo)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .HasMany(e => e.tblPaymentStatus)
                .WithRequired(e => e.tblPaymentNotification)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.SubscriptionID)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.FCTReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.FCTURNShort)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.DisbursementRequestID)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.PaymentMethod)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.PaymentRequestType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.PayeeName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.PayeeBankName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.PayeeBankNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.PayeeTransitNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.PayeeAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.PayeeSWIFTBIC)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.PayeeCanadianClearingCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.RequestUsername)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.RequestClientIP)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.PayeeReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentRequest>()
                .HasOptional(e => e.tblFCTFeeSummaryRequest)
                .WithRequired(e => e.tblPaymentRequest);

            modelBuilder.Entity<tblPaymentRequestType>()
                .Property(e => e.PaymentRequestTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentServiceProvider>()
                .Property(e => e.ServiceProviderName)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentServiceProvider>()
                .HasMany(e => e.tblSolutionSubscription)
                .WithRequired(e => e.tblPaymentServiceProvider)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblPaymentStatus>()
                .Property(e => e.StatusDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentTransaction>()
                .Property(e => e.PaymentTransactionFile)
                .IsFixedLength();

            modelBuilder.Entity<tblSolution>()
                .Property(e => e.SolutionName)
                .IsUnicode(false);

            modelBuilder.Entity<tblSolution>()
                .HasMany(e => e.tblPayeeReference)
                .WithRequired(e => e.tblSolution)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblSolutionSubscription>()
                .Property(e => e.SubscriptionID)
                .IsUnicode(false);

            modelBuilder.Entity<tblSolutionSubscription>()
                .Property(e => e.ServceEndPoint)
                .IsUnicode(false);

            modelBuilder.Entity<tblSolutionSubscription>()
                .HasMany(e => e.tblPaymentRequest)
                .WithRequired(e => e.tblSolutionSubscription)
                .WillCascadeOnDelete(false);
        }
    }
}
