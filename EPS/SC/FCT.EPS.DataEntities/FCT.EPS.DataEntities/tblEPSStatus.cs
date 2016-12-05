namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("tblEPSStatus")]
    public partial class tblEPSStatus
    {
        [Key]
        public int StatusID { get; set; }

        [StringLength(10)]
        public string StatusCode { get; set; }

        public virtual ICollection<tblPaymentNotification> tblPaymentNotification { get; set; }
        public virtual ICollection<tblPaymentTransaction> tblPaymentTransaction { get; set; }
        public virtual ICollection<tblServiceBatchStatus> tblServiceBatchStatus { get; set; }
    }
}
