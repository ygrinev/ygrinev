namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("tblPaymentTransaction")]
    public partial class tblPaymentTransaction
    {
        public tblPaymentTransaction()
        {
            tblPaymentRequest = new HashSet<tblPaymentRequest>();
            tblPaymentNotification = new HashSet<tblPaymentNotification>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int PaymentTransactionID { get; set; }

        [MaxLength(10)]
        public byte[] PaymentTransactionFile { get; set; }

        public int? ServiceProviderID { get; set; }

        public DateTime? DateTransactionSubmitted { get; set; }

        [Required]
        public int NumberRetried { get; set; }

        public int? StatusID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastModifiedDate { get; set; }

        public int? ServiceBatchID { get; set; }


        [ForeignKey("ServiceBatchID")]
        public virtual tblServiceBatch tblServiceBatch { get; set; }

        [ForeignKey("StatusID")]
        public virtual tblEPSStatus tblEPSStatus { get; set; }

        public virtual ICollection<tblPaymentRequest> tblPaymentRequest { get; set; }

        public virtual ICollection<tblPaymentNotification> tblPaymentNotification { get; set; }
    }
}
