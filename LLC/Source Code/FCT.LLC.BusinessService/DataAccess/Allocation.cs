using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.Services.LIM.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess
{
   public class Allocation
    {
        public int DealId { get; set; }
        public int FundingDealId { get; set; }
        public string ShortFCTURN { get; set; }
        public UserProfileInfo LawyerInfo { get; set; }

    }
}
