namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    
    [Table("tblPaymentNotification")]
    public partial class tblPaymentNotification
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int NotificationID { get; set; }

        public int? PaymentTransactionID { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentReferenceNumber { get; set; }

        [StringLength(50)]
        public string FCTTrustAccountNumber { get; set; }

        public decimal Amount { get; set; }

        public DateTime? PaymentDateTime { get; set; }

        [StringLength(50)]
        public string OriginatorName { get; set; }

        [StringLength(50)]
        public string OriginalorAccountNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string NotificationType { get; set; }

        public int StatusID { get; set; }

        [StringLength(400)]
        public string AdditionalInfo { get; set; }

        [StringLength(16)]
        public string PaymentBatchID { get; set; }

        public int NumberRetried { get; set; }

        public DateTime LastModifyDate { get; set; }

        [StringLength(100)]
        public string PaymentBatchDescription { get; set; }

        public int? PaymentStatusCode { get; set; }

        [ForeignKey("StatusID")]
        public virtual tblEPSStatus tblEPSStatus { get; set; }

        [ForeignKey("NotificationType,PaymentStatusCode")]
        public virtual tblPaymentStatus tblPaymentStatus { get; set; }

        [ForeignKey("PaymentTransactionID")]
        public virtual tblPaymentTransaction tblPaymentTransaction { get; set; }


    }
}
