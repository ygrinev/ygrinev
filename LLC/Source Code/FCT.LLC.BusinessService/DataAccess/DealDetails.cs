using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DealDetails
    {
        public int DealID { get; set; }
        public FundedDeal DealState { get; set; }
        public string DealStatus { get; set; }
    }
}
