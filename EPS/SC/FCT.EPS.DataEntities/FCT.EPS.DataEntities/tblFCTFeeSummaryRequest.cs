namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblFCTFeeSummaryRequest")]
    public partial class tblFCTFeeSummaryRequest
    {
        [Key, ForeignKey("tblPaymentRequest")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PaymentRequestID { get; set; }

        [Required]
        [StringLength(32)]
        public string LawyerCRMReference { get; set; }

        public int LawyerLIMReference { get; set; }

        public int FinanceServiceCode { get; set; }

        [Required]
        [StringLength(2)]
        public string PropertyProvinceCode { get; set; }

        public decimal PST { get; set; }

        public decimal GST { get; set; }

        public decimal HST { get; set; }

        public decimal QST { get; set; }

        public decimal RST { get; set; }

        public decimal BaseAmount { get; set; }

        [ForeignKey("PaymentRequestID")]
        public virtual tblPaymentRequest tblPaymentRequest { get; set; }
    }
}
