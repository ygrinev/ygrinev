namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
   

    [Table("tblPaymentRequest")]
    public partial class tblPaymentRequest
    {
        public tblPaymentRequest()
        {
            tblPaymentBatchSchedule = new HashSet<tblPaymentBatchSchedule>();
            tblBatchPaymentReportInfos=new HashSet<tblBatchPaymentReportInfo>();
            tblChildPaymentRequest = new HashSet<tblPaymentRequest>();

        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int PaymentRequestID { get; set; }

        [Required]
        [StringLength(10)]
        public string SubscriptionID { get; set; }

        [Required]
        [StringLength(11)]
        public string FCTReferenceNumber { get; set; }

        [StringLength(11)]
        public string FCTURNShort { get; set; }

        [Required]
        [StringLength(50)]
        public string DisbursementRequestID { get; set; }

        public decimal PaymentAmount { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        public DateTime PaymentRequestDate { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentRequestType { get; set; }

        [Required]
        [StringLength(100)]
        public string PayeeName { get; set; }

        [StringLength(100)]
        public string PayeeBankAccountHolderName { get; set; }

        [StringLength(50)]
        public string PayeeBankName { get; set; }

        [StringLength(3)]
        public string PayeeBankNumber { get; set; }

        [StringLength(10)]
        public string PayeeTransitNumber { get; set; }

        [StringLength(20)]
        public string PayeeAccountNumber { get; set; }

        [StringLength(11)]
        public string PayeeSWIFTBIC { get; set; }

        [StringLength(50)]
        public string PayeeCanadianClearingCode { get; set; }

        [Required]
        [StringLength(100)]
        public string RequestUsername { get; set; }

        [Required]
        [StringLength(20)]
        public string RequestClientIP { get; set; }

        [StringLength(40)]
        public string PayeeReferenceNumber { get; set; }

        public int? PayeeBranchAddressID { get; set; }

        public int? PayeeAddressID { get; set; }

        public int? PaymentTransactionID { get; set; }

        [StringLength(50)]
        public string PayeeContact { get; set; }

        [StringLength(50)]
        public string PayeeContactPhoneNumber { get; set; }

        [StringLength(50)]
        public string PayeeContactEmailAddress { get; set; }

        public DateTime LastModifiedDate { get; set; }

        [StringLength(80)]
        public string DebtorName { get; set; }

        [StringLength(100)]
        public string TokenNumber { get; set; }

        public bool? BPSWithWirePayment { get; set; }

        public int? PayeeInfoID { get; set; }

        public int? ParentPaymentRequestID { get; set; }

        public virtual tblFCTFeeSummaryRequest tblFCTFeeSummaryRequest { get; set; }

        [ForeignKey("PayeeBranchAddressID")]
        public virtual tblPaymentAddress tblPaymentAddress_PayeeBranchAddress { get; set; }

        [ForeignKey("PayeeAddressID")]
        public virtual tblPaymentAddress tblPaymentAddress_PayeeAddress { get; set; }

        public virtual ICollection<tblPaymentBatchSchedule> tblPaymentBatchSchedule { get; set; }

        [ForeignKey("PaymentTransactionID")]
        public virtual tblPaymentTransaction tblPaymentTransaction { get; set; }

        [ForeignKey("SubscriptionID")]
        public virtual tblSolutionSubscription tblSolutionSubscription { get; set; }
        public virtual ICollection<tblBatchPaymentReportInfo> tblBatchPaymentReportInfos { get; set; }


        [ForeignKey("ParentPaymentRequestID")]
        public virtual tblPaymentRequest tblParentPaymentRequest { get; set; }

        public virtual ICollection<tblPaymentRequest> tblChildPaymentRequest { get; set; }

        [ForeignKey("PayeeInfoID")]
        public virtual tblPayeeInfo tblPayeeInfo { get; set; }
    }
}
