using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class LLCDeal
    {
        public int DealID { get; set; }
        public string BusinessModel { get; set; }
        public string Status { get; set; }
        public string StatusReason { get; set; }
        public int? StatusReasonID { get; set; }
        public string ActingFor { get; set; }
    }
}
