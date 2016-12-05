using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPaymentRequest")]
    public class tblPaymentRequest
    {
        [Key]
        public int PaymentRequestID { get; set; }

        public int? DisbursementID { get; set; }

        public int? DealFundsAllocationID { get; set; }

        public string Message { get; set; }

        public DateTime RequestDate { get; set; }

        public virtual tblDealFundsAllocation tblDealFundsAllocation { get; set; }

        public virtual tblDisbursement tblDisbursement { get; set; }
    }
}
