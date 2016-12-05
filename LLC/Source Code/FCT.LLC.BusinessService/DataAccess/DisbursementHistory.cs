using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DisbursementHistory
    {
        public string Payee { get; set; }
        public decimal Amount { get; set; }
        public string OldPayee { get; set; }
        public decimal OldAmount { get; set; }
        public string TrustAccount { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string OldDisbursementStatus { get; set; }
    }
}
