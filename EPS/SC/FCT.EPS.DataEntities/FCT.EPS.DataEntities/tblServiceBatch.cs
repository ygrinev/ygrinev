namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblServiceBatch")]
    public partial class tblServiceBatch
    {
        public tblServiceBatch()
        {
            tblPaymentTransaction = new HashSet<tblPaymentTransaction>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ServiceBatchID { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public virtual ICollection<tblPaymentTransaction> tblPaymentTransaction { get; set; }

        public virtual tblServiceBatchStatus tblServiceBatchStatus { get; set; }
    }
}
