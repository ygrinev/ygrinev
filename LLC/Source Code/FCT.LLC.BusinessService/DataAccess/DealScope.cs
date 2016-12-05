using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DealScope
    {
        public int DealScopeId { get; set; }
        public string FCTRefNumber { get; set; }
        public string ShortFCTRefNumber { get; set; }
        public string WireDepositVerificationCode { get; set; }
        public string FormattedFCTRefNumber { get; set; }
    }
}
