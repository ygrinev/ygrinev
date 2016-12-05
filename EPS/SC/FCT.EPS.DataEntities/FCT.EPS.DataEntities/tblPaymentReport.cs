using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    
namespace FCT.EPS.DataEntities
{
    [Table("tblPaymentReport")]
    public partial class tblPaymentReport
    {
        [Key]
        public int NotificationID { get; set; }

        public byte[] PaymentReportFile { get; set; }

        [Required]
        public int StatusID { get; set; }

        [Required]
        public int NumberRetried { get; set; }

        [ForeignKey("StatusID")]
        public virtual tblEPSStatus tblEPSStatus { get; set; }

        [ForeignKey("NotificationID")]
        public virtual tblPaymentNotification tblPaymentNotification { get; set; }




    }
}
