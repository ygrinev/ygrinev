using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.DataEntities
{
    [Table("tblServiceBatchStatus")]
    public class tblServiceBatchStatus
    {
        [Key, ForeignKey("tblServiceBatch")]
        public int ServiceBatchID { get; set; }
        
        [StringLength(100)]
        public string PaymentFileName { get; set; }
        public int BPSSequenceNumber { get; set; }
        public int StatusID { get; set; }
        public int NumberRetried { get; set; }

        [ForeignKey("StatusID")]
        public virtual tblEPSStatus tblEPSStatus { get; set; }

        [ForeignKey("ServiceBatchID")]
        public virtual tblServiceBatch tblServiceBatch { get; set; }

    }
}
