namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPaymentBatchSchedule")]
    public partial class tblPaymentBatchSchedule
    {
        public DateTime BatchScheduleTime { get; set; }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int BatchScheduleID { get; set; }

        public int? PaymentRequestID { get; set; }

        [ForeignKey("PaymentRequestID")]
        public virtual tblPaymentRequest tblPaymentRequest { get; set; }
    }
}
