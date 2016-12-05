using System.Data.Entity;

namespace FCT.LLC.BusinessService.Entities
{
    public partial class EFBusinessContext : DbContext
    {
        public EFBusinessContext()
            : base("name=EFBusinessContext")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;

        }

        public virtual DbSet<dtproperty> dtproperties { get; set; }
        public virtual DbSet<tblAccessRole> tblAccessRoles { get; set; }
        public virtual DbSet<tblAddress> tblAddresses { get; set; }
        public virtual DbSet<tblAddressType> tblAddressTypes { get; set; }
        public virtual DbSet<tblAmendmentTrack> tblAmendmentTracks { get; set; }
        public virtual DbSet<tblAmendmentType> tblAmendmentTypes { get; set; }
        public virtual DbSet<tblApplicationConfigurationOption> tblApplicationConfigurationOptions { get; set; }
        public virtual DbSet<tblAttorney> tblAttorneys { get; set; }
        public virtual DbSet<tblAuditLog> tblAuditLogs { get; set; }
        public virtual DbSet<tblBCOnline> tblBCOnlines { get; set; }
        public virtual DbSet<tblBillingAmountDetail> tblBillingAmountDetails { get; set; }
        public virtual DbSet<tblBillingAmountDetailDiscount> tblBillingAmountDetailDiscounts { get; set; }
        public virtual DbSet<tblBillingAmountDetailTax> tblBillingAmountDetailTaxes { get; set; }
        public virtual DbSet<tblBranch> tblBranches { get; set; }
        public virtual DbSet<tblBridgeLoan> tblBridgeLoans { get; set; }
        public virtual DbSet<tblBusinessField> tblBusinessFields { get; set; }
        public virtual DbSet<tblBusinessFieldMap> tblBusinessFieldMaps { get; set; }
        public virtual DbSet<tblCadastre> tblCadastres { get; set; }
        public virtual DbSet<tblCalculationPeriodCode> tblCalculationPeriodCodes { get; set; }
        public virtual DbSet<tblCalculationPeriodDescription> tblCalculationPeriodDescriptions { get; set; }
        public virtual DbSet<tblChangeDetail> tblChangeDetails { get; set; }
        public virtual DbSet<tblCMHC> tblCMHCs { get; set; }
        public virtual DbSet<tblComment> tblComments { get; set; }
        public virtual DbSet<tblCompany> tblCompanies { get; set; }
        public virtual DbSet<tblCompanyIncorporationAuthority> tblCompanyIncorporationAuthorities { get; set; }
        public virtual DbSet<tblComponentFieldMapping> tblComponentFieldMappings { get; set; }
        public virtual DbSet<tblConfigurationCategory> tblConfigurationCategories { get; set; }
        public virtual DbSet<tblConfigurationOption> tblConfigurationOptions { get; set; }
        public virtual DbSet<tblContactInfo> tblContactInfoes { get; set; }
        public virtual DbSet<tblConversionHistory> tblConversionHistories { get; set; }
        public virtual DbSet<tblCreditCard> tblCreditCards { get; set; }
        public virtual DbSet<tblDealDocumentPackage> tblDealDocumentPackages { get; set; }
        public virtual DbSet<tblDealLock> tblDealLocks { get; set; }
        public virtual DbSet<tblDealTrustAccount> tblDealTrustAccounts { get; set; }
        public virtual DbSet<tblDocumentArchive> tblDocumentArchives { get; set; }
        public virtual DbSet<tblDocumentCache> tblDocumentCaches { get; set; }
        public virtual DbSet<tblDocumentCategory> tblDocumentCategories { get; set; }
        public virtual DbSet<tblDocumentComment> tblDocumentComments { get; set; }
        public virtual DbSet<tblDocumentField> tblDocumentFields { get; set; }
        public virtual DbSet<tblDocumentFileFormat> tblDocumentFileFormats { get; set; }
        public virtual DbSet<tblDocumentFileOutputInfo> tblDocumentFileOutputInfoes { get; set; }
        public virtual DbSet<tblDocumentKeyValue> tblDocumentKeyValues { get; set; }
        public virtual DbSet<tblDocumentMappingFile> tblDocumentMappingFiles { get; set; }
        public virtual DbSet<tblDocumentPackageMapping> tblDocumentPackageMappings { get; set; }
        public virtual DbSet<tblDocumentProvConfig> tblDocumentProvConfigs { get; set; }
        public virtual DbSet<tblDocumentTemplate> tblDocumentTemplates { get; set; }
        public virtual DbSet<tblDocumentTemplateFile> tblDocumentTemplateFiles { get; set; }
        public virtual DbSet<tblDocumentTypeCode> tblDocumentTypeCodes { get; set; }
        public virtual DbSet<tblDocumentTypeDisplay> tblDocumentTypeDisplays { get; set; }
        public virtual DbSet<tblDocumentTypeDisplayType> tblDocumentTypeDisplayTypes { get; set; }
        public virtual DbSet<tblDocumentTypeMapping> tblDocumentTypeMappings { get; set; }
        public virtual DbSet<tblDocumentTypeUserRole> tblDocumentTypeUserRoles { get; set; }
        public virtual DbSet<tblDropdown> tblDropdowns { get; set; }
        public virtual DbSet<tblEasyFundClosureDate> tblEasyFundClosureDates { get; set; }
        public virtual DbSet<tblEmailTemplate> tblEmailTemplates { get; set; }
        public virtual DbSet<tblEmailTemplateList> tblEmailTemplateLists { get; set; }
        public virtual DbSet<tblExistingMortgage> tblExistingMortgages { get; set; }
        public virtual DbSet<tblFinalReportClosingOption> tblFinalReportClosingOptions { get; set; }
        public virtual DbSet<tblFireInsurancePolicy> tblFireInsurancePolicies { get; set; }
        public virtual DbSet<tblFormNumber> tblFormNumbers { get; set; }
        public virtual DbSet<tblFundingPaymentMethod> tblFundingPaymentMethods { get; set; }
        public virtual DbSet<tblFundingRequest> tblFundingRequests { get; set; }
        public virtual DbSet<tblFundingRequestType> tblFundingRequestTypes { get; set; }
        public virtual DbSet<tblFundsDeliveryType> tblFundsDeliveryTypes { get; set; }
        public virtual DbSet<tblFundStatu> tblFundStatus { get; set; }
        public virtual DbSet<tblGuarantor> tblGuarantors { get; set; }
        public virtual DbSet<tblHoliday> tblHolidays { get; set; }
        public virtual DbSet<tblIdentification> tblIdentifications { get; set; }
        public virtual DbSet<tblInternalComponent> tblInternalComponents { get; set; }
        public virtual DbSet<tblLandLease> tblLandLeases { get; set; }
        public virtual DbSet<tblLanguage> tblLanguages { get; set; }
        public virtual DbSet<tblLawyerApplication> tblLawyerApplications { get; set; }
        public virtual DbSet<tblLawyerAuthorizedPaymentMethodType> tblLawyerAuthorizedPaymentMethodTypes { get; set; }
        public virtual DbSet<tblLawyerNotification> tblLawyerNotifications { get; set; }
        public virtual DbSet<tblLawyerNotificationEmail> tblLawyerNotificationEmails { get; set; }
        public virtual DbSet<tblLawyerPaymentAuthorizationMethod> tblLawyerPaymentAuthorizationMethods { get; set; }
        public virtual DbSet<tblLawyerSavedAmendment> tblLawyerSavedAmendments { get; set; }
        public virtual DbSet<tblLawyerSecurityQuestion> tblLawyerSecurityQuestions { get; set; }
        public virtual DbSet<tblLenderBusinessField> tblLenderBusinessFields { get; set; }
        public virtual DbSet<tblLenderChangeBridgeLoan> tblLenderChangeBridgeLoans { get; set; }
        public virtual DbSet<tblLenderChangeExistingMortgage> tblLenderChangeExistingMortgages { get; set; }
        public virtual DbSet<tblLenderChangeGuarantor> tblLenderChangeGuarantors { get; set; }
        public virtual DbSet<tblLenderChangeMortgagor> tblLenderChangeMortgagors { get; set; }
        public virtual DbSet<tblLenderChangePIN> tblLenderChangePINs { get; set; }
        public virtual DbSet<tblLenderChangeProperty> tblLenderChangeProperties { get; set; }
        public virtual DbSet<tblLenderChangeUnsecuredDebt> tblLenderChangeUnsecuredDebts { get; set; }
        public virtual DbSet<tblLenderClause> tblLenderClauses { get; set; }
        public virtual DbSet<tblLenderConfigurationOption> tblLenderConfigurationOptions { get; set; }
        public virtual DbSet<tblLenderFundingRequestType> tblLenderFundingRequestTypes { get; set; }
        public virtual DbSet<tblLenderInstruction> tblLenderInstructions { get; set; }
        public virtual DbSet<tblLenderSchemaField> tblLenderSchemaFields { get; set; }
        public virtual DbSet<tblLenderSchemaFieldsDescription> tblLenderSchemaFieldsDescriptions { get; set; }
        public virtual DbSet<tblLenderSecurityQuestion> tblLenderSecurityQuestions { get; set; }
        public virtual DbSet<tblLockLevel> tblLockLevels { get; set; }
        public virtual DbSet<tblMortgageFee> tblMortgageFees { get; set; }
        public virtual DbSet<tblMortgageFeeType> tblMortgageFeeTypes { get; set; }
        public virtual DbSet<tblMortgageILA> tblMortgageILAs { get; set; }
        public virtual DbSet<tblMortgagePayment> tblMortgagePayments { get; set; }
        public virtual DbSet<tblMortgagePaymentRegistrationData> tblMortgagePaymentRegistrationDatas { get; set; }
        public virtual DbSet<tblMortgageProduct> tblMortgageProducts { get; set; }
        public virtual DbSet<tblMortgageRegistrationData> tblMortgageRegistrationDatas { get; set; }
        public virtual DbSet<tblMortgageRegistrationDataField> tblMortgageRegistrationDataFields { get; set; }
        public virtual DbSet<tblMunicipality> tblMunicipalities { get; set; }
        public virtual DbSet<tblNotification> tblNotifications { get; set; }
        public virtual DbSet<tblPasswordHistory> tblPasswordHistories { get; set; }
        public virtual DbSet<tblPayeeTypeDocumentType> tblPayeeTypeDocumentTypes { get; set; }
        public virtual DbSet<tblPaymentAuthorization> tblPaymentAuthorizations { get; set; }
        public virtual DbSet<tblPaymentFrequencyType> tblPaymentFrequencyTypes { get; set; }
        public virtual DbSet<tblPerson> tblPersons { get; set; }
        public virtual DbSet<tblPlatformContactInfoMessage> tblPlatformContactInfoMessages { get; set; }
        public virtual DbSet<tblPlatformMessage> tblPlatformMessages { get; set; }
        public virtual DbSet<tblPOA> tblPOAs { get; set; }
        public virtual DbSet<tblPropertyLot> tblPropertyLots { get; set; }
        public virtual DbSet<tblProvince> tblProvinces { get; set; }
        public virtual DbSet<tblProvinceTax> tblProvinceTaxes { get; set; }
        public virtual DbSet<tblPublishedEventMessage> tblPublishedEventMessages { get; set; }
        public virtual DbSet<tblPublishedEventMessageBatchProcessing> tblPublishedEventMessageBatchProcessings { get; set; }
        public virtual DbSet<tblQuestionReference> tblQuestionReferences { get; set; }
        public virtual DbSet<tblRatioIndicatorType> tblRatioIndicatorTypes { get; set; }
        public virtual DbSet<tblRegisteredOwner> tblRegisteredOwners { get; set; }
        public virtual DbSet<tblRegistrationDataConfiguration> tblRegistrationDataConfigurations { get; set; }
        public virtual DbSet<tblRegistryOffice> tblRegistryOffices { get; set; }
        public virtual DbSet<tblRejectLenderAmendment> tblRejectLenderAmendments { get; set; }
        public virtual DbSet<tblResourceText> tblResourceTexts { get; set; }
        public virtual DbSet<tblSchemaFieldList> tblSchemaFieldLists { get; set; }
        public virtual DbSet<tblSchemaFieldListMapping> tblSchemaFieldListMappings { get; set; }
        public virtual DbSet<tblSecondaryDesignation> tblSecondaryDesignations { get; set; }
        public virtual DbSet<tblSharedFieldMap> tblSharedFieldMaps { get; set; }
        public virtual DbSet<tblSignatory> tblSignatories { get; set; }
        public virtual DbSet<tblSolicitorInstructionPOA> tblSolicitorInstructionPOAs { get; set; }
        public virtual DbSet<tblSolicitorSync> tblSolicitorSyncs { get; set; }
        public virtual DbSet<tblSolicitorSyncLastRunParameter> tblSolicitorSyncLastRunParameters { get; set; }
        public virtual DbSet<tblSolicitorSyncMarkedProfile> tblSolicitorSyncMarkedProfiles { get; set; }
        public virtual DbSet<tblSolicitorSyncReportData> tblSolicitorSyncReportDatas { get; set; }
        public virtual DbSet<tblStandardNotification> tblStandardNotifications { get; set; }
        public virtual DbSet<tblStandardNotificationList> tblStandardNotificationLists { get; set; }
        public virtual DbSet<tblStatu> tblStatus { get; set; }
        public virtual DbSet<tblSymmetricKey> tblSymmetricKeys { get; set; }
        public virtual DbSet<tblTitleInsurancePolicy> tblTitleInsurancePolicies { get; set; }
        public virtual DbSet<tblTitleInsuranceSelection> tblTitleInsuranceSelections { get; set; }
        public virtual DbSet<tblTrustAccount> tblTrustAccounts { get; set; }
        public virtual DbSet<tblTrustAccountLender> tblTrustAccountLenders { get; set; }
        public virtual DbSet<tblUIControlType> tblUIControlTypes { get; set; }
        public virtual DbSet<tblUnsecuredDebt> tblUnsecuredDebts { get; set; }
        public virtual DbSet<tblUserStatu> tblUserStatus { get; set; }
        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }
        public virtual DbSet<ImportedQuestion> ImportedQuestions { get; set; }
        public virtual DbSet<tblBusinessModel> tblBusinessModels { get; set; }
        public virtual DbSet<tblDocumentConfiguration> tblDocumentConfigurations { get; set; }


        public virtual DbSet<tblBuilderUnitLevel> tblBuilderUnitLevels { get; set; }
        public virtual DbSet<tblBuilderLegalDescription> tblBuilderLegalDescriptions { get; set; }
        public virtual DbSet<tblDeal> tblDeals { get; set; }
        public virtual DbSet<tblDealContact> tblDealContacts { get; set; }
        public virtual DbSet<tblDealScope> tblDealScopes { get; set; }
        public virtual DbSet<tblLawyer> tblLawyers { get; set; }
        public virtual DbSet<tblMortgage> tblMortgages { get; set; }
        public virtual DbSet<tblMortgagor> tblMortgagors { get; set; }
        public virtual DbSet<tblPIN> tblPINs { get; set; }
        public virtual DbSet<tblProperty> tblProperties { get; set; }
        public virtual DbSet<tblVendor> tblVendors { get; set; }
        public virtual DbSet<tblDealHistory> tblDealHistories { get; set; }
        public virtual DbSet<tblLender> tblLenders { get; set; }
        public virtual DbSet<tblMilestone> tblMilestones { get; set; }
        public virtual DbSet<tblMilestoneCode> tblMilestoneCodes { get; set; }
        public virtual DbSet<tblMilestoneLabel> tblMilestoneLabels { get; set; }
        public virtual DbSet<tblNote> tblNotes { get; set; }
        public virtual DbSet<tblBranchContact> tblBranchContacts { get; set; }
        public virtual DbSet<tblDealFundsAllocation> tblDealFundsAllocations { get; set; }
        public virtual DbSet<tblFundingDeal> tblFundingDeals { get; set; }
        public virtual DbSet<tblDisbursement> tblDisbursements { get; set; }
        public virtual DbSet<tblDisbursementSummary> tblDisbursementSummaries { get; set; }
        public virtual DbSet<vw_EFDisbursementSummary> vw_EFDisbursementSummaries { get; set; }
        public virtual DbSet<vw_Deal> vw_Deals { get; set; }
        public virtual DbSet<tblDisbursementDealDocumentType> tblDisbursementDealDocumentTypes { get; set; }
        public virtual DbSet<vw_PayoutLetterWorklist> vw_PayoutLetterWorklist { get; set; }
        public virtual DbSet<tblPaymentRequest> tblPaymentRequests { get; set; }
        public virtual DbSet<tblPaymentNotification> tblPaymentNotifications { get; set; }
        public virtual DbSet<vw_ReconciliationItem> vw_ReconciliationItems { get; set; }
        public virtual DbSet<tblGlobalization> tblGlobalizations { get; set; }
        public virtual DbSet<tblStatusReason> tblStatusReasons { get; set; }
        public virtual DbSet<tblLawyerClerk> tblLawyerClerks { get; set; }
        public virtual DbSet<tblFee> tblFees { get; set; }
        public virtual DbSet<tblDocumentType> tblDocumentType { get; set; }
        public virtual DbSet<tblDealDocumentType> tblDealDocumentType { get; set; }

        public virtual DbSet<tblFinancialInstitutionNumber> tblFinancialInstitutionNumbers { get; set; }

        public virtual DbSet<tblQuestion> tblQuestions { get; set; }
        public virtual DbSet<tblAnswerType> tblAnswerTypes { get; set; }
        public virtual DbSet<tblAnswer> tblAnswers { get; set; }

        public virtual DbSet<tblPifInfo> tblPifInfos { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<EFBusinessContext>(null);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.FCTRefNum)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.LenderRefNum)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.LenderComment)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.StatusUserType)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.StatusReason)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.LawyerMatterNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.LastModified)
                .IsFixedLength();

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.BusinessModel)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.LawyerApplication)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.Encumbrances)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.LenderDealRefNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.RFFComment)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.LenderRepresentativeFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.LenderRepresentativeLastName)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.LenderRepresentativeTitle)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.DistrictName)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.LenderSecurityType)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.LawyerActingFor)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .Property(e => e.DealType)
                .IsUnicode(false);

            modelBuilder.Entity<tblDeal>()
                .HasMany(e => e.tblDealContacts)
                .WithRequired(e => e.tblDeal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblDeal>()
                .HasMany(e => e.tblMortgagors)
                .WithRequired(e => e.tblDeal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblDeal>()
                .HasMany(e => e.tblProperties)
                .WithRequired(e => e.tblDeal)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<vw_Deal>()
                .Property(e => e.FCTRefNum)
                .IsUnicode(false);

            modelBuilder.Entity<vw_Deal>()
                .Property(e => e.LenderRefNum)
                .IsUnicode(false);

            modelBuilder.Entity<vw_Deal>()
                .Property(e => e.LLCRefNum)
                .IsUnicode(false);


            modelBuilder.Entity<vw_Deal>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<vw_Deal>()
                .Property(e => e.StatusUserType)
                .IsUnicode(false);

            modelBuilder.Entity<vw_Deal>()
                .Property(e => e.StatusReason)
                .IsUnicode(false);

            modelBuilder.Entity<vw_Deal>()
                .Property(e => e.LawyerMatterNumber)
                .IsUnicode(false);

            //modelBuilder.Entity<vw_Deal>()
            //    .Property(e => e.LastModified)
            //    .IsFixedLength();

            modelBuilder.Entity<vw_Deal>()
                .Property(e => e.BusinessModel)
                .IsUnicode(false);

            //modelBuilder.Entity<vw_Deal>()
            //    .Property(e => e.LawyerApplication)
             //   .IsUnicode(false);

            //modelBuilder.Entity<vw_Deal>()
            //    .Property(e => e.Encumbrances)
            //    .IsUnicode(false);

            //modelBuilder.Entity<vw_Deal>()
            //    .Property(e => e.LenderDealRefNumber)
            //    .IsUnicode(false);

            //modelBuilder.Entity<vw_Deal>()
            //    .Property(e => e.RFFComment)
            //    .IsUnicode(false);

            //modelBuilder.Entity<vw_Deal>()
            //    .Property(e => e.LenderRepresentativeFirstName)
            //    .IsUnicode(false);

            //modelBuilder.Entity<vw_Deal>()
            //    .Property(e => e.LenderRepresentativeLastName)
            //    .IsUnicode(false);

            //modelBuilder.Entity<vw_Deal>()
            //    .Property(e => e.LenderRepresentativeTitle)
            //    .IsUnicode(false);

           // modelBuilder.Entity<vw_Deal>()
           //     .Property(e => e.DistrictName)
           //     .IsUnicode(false);

           // modelBuilder.Entity<vw_Deal>()
           //     .Property(e => e.LenderSecurityType)
           //     .IsUnicode(false);

            modelBuilder.Entity<vw_Deal>()
                .Property(e => e.LawyerActingFor)
                .IsUnicode(false);

           // modelBuilder.Entity<vw_Deal>()
           //     .Property(e => e.DealType)
           //     .IsUnicode(false);


            modelBuilder.Entity<vw_PayoutLetterWorklist>()
                .Property(e => e.AssignedTo)
                .IsUnicode(false);

            modelBuilder.Entity<vw_PayoutLetterWorklist>()
                .Property(e => e.ChequeBatchDescription)
                .IsUnicode(false);

            modelBuilder.Entity<vw_PayoutLetterWorklist>()
                .Property(e => e.ChequeBatchNumber)
                .IsUnicode(false);

            modelBuilder.Entity<vw_PayoutLetterWorklist>()
                .Property(e => e.DealID);

            modelBuilder.Entity<vw_PayoutLetterWorklist>()
                .Property(e => e.DisbursementDate);

            modelBuilder.Entity<vw_PayoutLetterWorklist>()
                .Property(e => e.FCTURN)
                .IsUnicode(false);

            modelBuilder.Entity<vw_PayoutLetterWorklist>()
                .Property(e => e.NumberOfCheques);

            modelBuilder.Entity<tblDealScope>()
                .Property(e => e.FCTRefNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealScope>()
                .Property(e => e.ShortFCTRefNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealScope>()
                .HasMany(e => e.tblVendors)
                .WithRequired(e => e.tblDealScope)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblDealScope>()
                .HasMany(e => e.tblFundingDeals)
                .WithRequired(e => e.DealScope).WillCascadeOnDelete(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.LawFirm)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.UnitNo)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.Address2)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.Province)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.PostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.MobilePhone)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.EMail)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.LastModified)
                .IsFixedLength();

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.Comments)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.LawyerSoftwareUsed)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.Profession)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.StreetNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.UserLanguage)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.RequestSource)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.InternetBrowserUsed)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.LawSocietyFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.LawSocietyMiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.LawSocietyLastName)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .Property(e => e.SolicitorSyncLawSocietyStatus)
                .IsUnicode(false);

            modelBuilder.Entity<tblLawyer>()
                .HasMany(e => e.tblDeals)
                .WithRequired(e => e.tblLawyer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.MortgageNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.MortgageAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.MortgageType)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.InterestRate)
                .HasPrecision(9, 5);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.MaximumRate)
                .HasPrecision(9, 5);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.BaseRate)
                .HasPrecision(9, 5);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.MaximumAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.CalculationPeriod)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.StandardChargeTerm)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.PaymentDay)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.InterestAdjustAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.BonusDiscountAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.NetAdvance)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.BrokerName)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.BrokerPhone)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.RegisteredAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.MortgageInsurer)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.InterestRateType)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.EquivalentRate)
                .HasPrecision(9, 5);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.CashbackAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.PurchasePrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.IncrementAboveBelowPrime)
                .HasPrecision(9, 5);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.ActualMortgageRate)
                .HasPrecision(9, 5);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.MonthlyPayment)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.EarlyPaymentAmount)
                .HasPrecision(18, 5);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.LastModified)
                .IsFixedLength();

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.IncrementAboveBelowPrimeInstruction)
                .HasPrecision(9, 5);

            modelBuilder.Entity<tblMortgage>()
                .Property(e => e.MortgageAmountAdvanced)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.MortgagorType)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.CompanyName)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.Province)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.PostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.HomePhone)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.BusinessPhone)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.SpouseLastName)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.SpouseFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.SpouseMiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.Occupation)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.LastModified)
                .IsFixedLength();

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.UnitNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.StreetNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.Address2)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.Language)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.SpousalStatement)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.SpouseOccupation)
                .IsUnicode(false);

            modelBuilder.Entity<tblMortgagor>()
                .Property(e => e.CompanyProvinceOfIncorporation)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblPIN>()
                .Property(e => e.PINNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPIN>()
                .Property(e => e.LastModified)
                .IsFixedLength();

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.Province)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.PostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.HomePhone)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.BusinessPhone)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.LegalDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.ARN)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.EstateType)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.InstrumentNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.AmountOfTaxesPaid)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.PropertyType)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.OccupancyType)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.AnnualTaxAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.RegistryOffice)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.CondoLevel)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.CondoUnitNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.CondoCorporationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.LastModified)
                .IsFixedLength();

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.UnitNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.StreetNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.Address2)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.BookFolioRoll)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.PageFrame)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.CondoDeclarationRegistrationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.CondoBookNoOfDeclaration)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.CondoPageNumberOfDeclaration)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.CondoPlanNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.AssignmentOfRentsRegistrationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.MortgagePriority)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.OtherEstateTypeDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.CondoDeclarationModificationNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .Property(e => e.JudicialDistrict)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .HasMany(e => e.tblPINs)
                .WithRequired(e => e.tblProperty)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblVendor>()
                .Property(e => e.VendorType)
                .IsUnicode(false);

            modelBuilder.Entity<tblVendor>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblVendor>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<tblVendor>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<tblVendor>()
                .Property(e => e.CompanyName)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealHistory>()
                .Property(e => e.Activity)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealHistory>()
                .Property(e => e.ActivityFrench)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealHistory>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealHistory>()
                .Property(e => e.UserType)
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .Property(e => e.LenderCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .Property(e => e.Province)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .Property(e => e.PostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .Property(e => e.LogoName)
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .Property(e => e.BillingID)
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .Property(e => e.LastModified)
                .IsFixedLength();

            modelBuilder.Entity<tblLender>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<tblLender>()
                .HasMany(e => e.tblMilestoneLabels)
                .WithRequired(e => e.tblLender)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblMilestoneCode>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<tblMilestoneCode>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<tblMilestoneCode>()
                .Property(e => e.BusinessModel)
                .IsUnicode(false);

            modelBuilder.Entity<tblMilestoneCode>()
                .HasMany(e => e.tblMilestones)
                .WithRequired(e => e.tblMilestoneCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblMilestoneCode>()
                .HasMany(e => e.tblMilestoneLabels)
                .WithRequired(e => e.tblMilestoneCode)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblMilestoneLabel>()
                .Property(e => e.LabelEnglish)
                .IsUnicode(false);

            modelBuilder.Entity<tblMilestoneLabel>()
                .Property(e => e.LabelFrench)
                .IsUnicode(false);

            modelBuilder.Entity<tblNote>()
                .Property(e => e.Notes)
                .IsUnicode(false);

            modelBuilder.Entity<tblNote>()
                .Property(e => e.Usertype)
                .IsUnicode(false);

            modelBuilder.Entity<tblNote>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<tblNote>()
                .Property(e => e.NoteType)
                .IsUnicode(false);

            modelBuilder.Entity<tblBranchContact>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBranchContact>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBranchContact>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<tblBranchContact>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<tblBranchContact>()
                .Property(e => e.Extension)
                .IsUnicode(false);

            modelBuilder.Entity<tblBranchContact>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<tblBranchContact>()
                .Property(e => e.EMail)
                .IsUnicode(false);

            modelBuilder.Entity<tblBranchContact>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<tblBranchContact>()
                .Property(e => e.LastModified)
                .IsFixedLength();

            modelBuilder.Entity<tblBranchContact>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealFundsAllocation>()
                .Property(e => e.ReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealFundsAllocation>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblDealFundsAllocation>()
                .Property(e => e.BankNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealFundsAllocation>()
                .Property(e => e.BranchNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealFundsAllocation>()
                .Property(e => e.AccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealFundsAllocation>()
                .Property(e => e.WireDepositDetails)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealFundsAllocation>()
                .Property(e => e.ShortFCTRefNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealFundsAllocation>()
                .Property(e => e.AllocationStatus)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealFundsAllocation>()
                .Property(e => e.Version)
                .IsFixedLength().IsRowVersion();

            modelBuilder.Entity<tblDealFundsAllocation>()
                .Property(e => e.RecordType)
                .IsUnicode(false);

            modelBuilder.Entity<tblDealFundsAllocation>()
                .Property(e => e.ReconciledBy)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursementSummary>()
                .Property(e => e.Version)
                .IsFixedLength().IsRowVersion();

            modelBuilder.Entity<tblDisbursementSummary>()
                .Property(e => e.DepositAmountRequired)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblDisbursementSummary>()
                .Property(e => e.Comments)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.PayeeType)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.PayeeName)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.PayeeComments)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.PaymentMethod)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.NameOnCheque)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.UnitNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.StreetNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.Province)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.PostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.Country)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.ReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.AssessmentRollNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.BankNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.BranchNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.AccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.Instructions)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.AgentFirstName)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.AgentLastName)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.AccountAction)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.FCTFeeSplit)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.DisbursementComment)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.DisbursedAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.DisbursementStatus)
                .IsUnicode(false);

            modelBuilder.Entity<tblDisbursement>()
                .Property(e => e.ReconciledBy)
                .IsUnicode(false);

            modelBuilder.Entity<tblGlobalization>()
                .Property(e => e.LocaleID)
                .IsUnicode(false);

            modelBuilder.Entity<tblGlobalization>()
                .Property(e => e.ResourceSet)
                .IsUnicode(false);

            modelBuilder.Entity<tblGlobalization>()
                .Property(e => e.ResourceKey)
                .IsUnicode(false);

            modelBuilder.Entity<vw_EFDisbursementSummary>()
                .Property(e => e.DepositAmountReceived)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vw_EFDisbursementSummary>()
                .Property(e => e.DepositAmountRequired)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblFee>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblFee>()
                .Property(e => e.HST)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblFee>()
                .Property(e => e.GST)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblFee>()
                .Property(e => e.QST)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblPaymentRequest>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.PaymentAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.NotificationType)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.ReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.PaymentStatus)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.BatchDescription)
                .IsUnicode(false);

            modelBuilder.Entity<tblPaymentNotification>()
                .Property(e => e.BatchID)
                .IsUnicode(false);

            modelBuilder.Entity<vw_ReconciliationItem>()
                .Property(e => e.AmountIn)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vw_ReconciliationItem>()
                .Property(e => e.AmountOut)
                .HasPrecision(19, 4);

            /* Builder Details */

            modelBuilder.Entity<tblBuilderLegalDescription>()
                .Property(e => e.BuilderProjectReference)
                .IsUnicode(false);

            modelBuilder.Entity<tblBuilderLegalDescription>()
                .Property(e => e.BuilderLot)
                .IsUnicode(false);

            modelBuilder.Entity<tblBuilderLegalDescription>()
                .Property(e => e.Lot)
                .IsUnicode(false);

            modelBuilder.Entity<tblBuilderLegalDescription>()
                .Property(e => e.Plan)
                .IsUnicode(false);

            modelBuilder.Entity<tblBuilderUnitLevel>()
                .Property(e => e.Unit)
                .IsUnicode(false);

            modelBuilder.Entity<tblBuilderUnitLevel>()
                .Property(e => e.Level)
                .IsUnicode(false);

            modelBuilder.Entity<tblProperty>()
                .HasMany(e => e.tblBuilderLegalDescriptions)
                .WithRequired(e => e.tblProperty)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<tblBuilderLegalDescription>()
               .HasMany(e => e.tblBuilderUnitLevels)
               .WithRequired(e => e.tblBuilderLegalDescription)
               .WillCascadeOnDelete(false);
        }
    }
}
