namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [Table("tblDeal")]
    public partial class tblDeal
    {
        public tblDeal()
        {
            tblDealContacts = new HashSet<tblDealContact>();
            tblMortgagors = new HashSet<tblMortgagor>();
            tblMortgages = new HashSet<tblMortgage>();
            tblProperties = new HashSet<tblProperty>();            
        }

        [DataMember]
        [Key]
        public int DealID { get; set; }

        [DataMember]
        [StringLength(11)]
        public string FCTRefNum { get; set; }

        [DataMember]
        [NotMapped]
        public string FCTURN { get; set; }

        [DataMember]
        [StringLength(50)]
        public string LenderRefNum { get; set; }

        [DataMember]
        public int? LenderID { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? RFIReceiveDate { get; set; }

        [DataMember]
        public int? BranchID { get; set; }

        [DataMember]
        public int? ContactID { get; set; }

        [DataMember]
        public int? MortgageCentreID { get; set; }

        [DataMember]
        public int? MtgCentreContactID { get; set; }

        [DataMember]
        [StringLength(6000)]
        public string LenderComment { get; set; }

        [DataMember]
        [StringLength(50)]
        public string Status { get; set; }

        [DataMember]
        public DateTime? StatusDate { get; set; }

        [DataMember]
        public int? StatusUserID { get; set; }

        [DataMember]
        [StringLength(50)]
        public string StatusUserType { get; set; }

        [DataMember]
        [StringLength(1000)]
        public string StatusReason { get; set; }

        [DataMember]
        public int? StatusReasonID { get; set; }

        [DataMember]
        public int LawyerID { get; set; }

        [DataMember]
        public bool? LawyerDeclinedFlag { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? LawyerAcceptDeclinedDate { get; set; }

      

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? FinalDocsPostedDate { get; set; }

        [DataMember]
        public bool? UserNotification { get; set; }

        [DataMember]
        public bool? LendersAttentionFlag { get; set; }

        [DataMember]
        public bool? Promotion { get; set; }

        [DataMember]
        public int? FundStatusID { get; set; }

        [DataMember]
        public int? Sequence { get; set; }

        [DataMember]
        public bool? FundsDisbursed { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? FundRequestDate { get; set; }

        [DataMember]
        [StringLength(20)]
        public string LawyerMatterNumber { get; set; }

        [DataMember]
        public bool? IsLLC { get; set; }

        [DataMember]
        public bool? LenderNewNotes { get; set; }

        [DataMember]
        public bool? LenderUpdated { get; set; }

        [DataMember]
        public int? CreditCardID { get; set; }

        [DataMember]
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        [DataMember]
        // [Required]
        [StringLength(20)]
        public string BusinessModel { get; set; }

        [DataMember]
        public int? BCOnlineID { get; set; }

        [DataMember]
        public int? BillingAmountDetailID { get; set; }

        [DataMember]
        [StringLength(50)]
        public string LawyerApplication { get; set; }

        [DataMember]
        public int? ActionableNotesCompleted { get; set; }

        [DataMember]
        public int? FinalReportNotificationNo { get; set; }

        [DataMember]
        public int? ServiceAddressTypeID { get; set; }

        [DataMember]
        public int? MailingAddressTypeID { get; set; }

        [DataMember]
        public int? MortgageOwningBranchID { get; set; }

        [DataMember]
        public bool? IsRFF { get; set; }

        [DataMember]
        [StringLength(2000)]
        public string Encumbrances { get; set; }

        [DataMember]
        public int? DealTrustAccountID { get; set; }

        [DataMember]
        public int? MtgOwningBranchContactID { get; set; }

        [DataMember]
        public int? FundingPaymentMethodID { get; set; }

        [DataMember]
        [StringLength(25)]
        public string LenderDealRefNumber { get; set; }

        [DataMember]
        public bool? LawyerAmendmentImmediate { get; set; }

        [DataMember]
        public bool? LawyerAmendmentSROT { get; set; }

        [DataMember]
        [StringLength(1000)]
        public string RFFComment { get; set; }

        [DataMember]
        public DateTime? RFFClosingDate { get; set; }

        [DataMember]
        public bool? IsSubmissionPending { get; set; }

        [DataMember]
        public DateTime? LawyerAppointmentDate { get; set; }

        [DataMember]
        [StringLength(100)]
        public string LenderRepresentativeFirstName { get; set; }

        [DataMember]
        [StringLength(50)]
        public string LenderRepresentativeLastName { get; set; }

        [DataMember]
        [StringLength(100)]
        public string LenderRepresentativeTitle { get; set; }

        [DataMember]
        [StringLength(100)]
        public string DistrictName { get; set; }

        [DataMember]
        public int? DealClosingOptionID { get; set; }

        [DataMember]
        public int? SavedDocumentClosingOptionID { get; set; }

        [DataMember]
        [StringLength(20)]
        public string LenderSecurityType { get; set; }

        [DataMember]
        public DateTime? RFFNotifiedDate { get; set; }

        [DataMember]
        public int? LenderDealAlternateID { get; set; }

        [DataMember]
        public int? DealScopeID { get; set; }

        [DataMember]
        public int? PrimaryDealContactID { get; set; }

        [DataMember]
        [StringLength(20)]
        public string LawyerActingFor { get; set; }

        [DataMember]
        [StringLength(20)]
        public string DealType { get; set; }

        [DataMember]
        public string LenderFINumber { get; set; }

        [DataMember]
        public bool IsLawyerConfirmedClosing { get; set; }

        [DataMember]
        public virtual tblDealScope tblDealScope { get; set; }

        [DataMember]
        public virtual tblLawyer tblLawyer { get; set; }

        [DataMember]
        public virtual ICollection<tblDealContact> tblDealContacts { get; set; }

        [DataMember]
        public virtual ICollection<tblMortgagor> tblMortgagors { get; set; }

        [DataMember]
        public virtual ICollection<tblProperty> tblProperties { get; set; }

        [DataMember]
        public virtual ICollection<tblMortgage> tblMortgages { get; set; }

        [DataMember]
        public virtual tblLender tblLender { get; set; }

        [DataMember]
        public virtual ICollection<tblNote> tblNotes { get; set; }

        [DataMember]
        public virtual ICollection<tblMilestone> tblMilestones { get; set; }

        [DataMember]
        public virtual ICollection<tblDealHistory> tblDealHistory { get; set; }
    }
}
