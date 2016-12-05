using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class FundedDeal
    {
        public FundingMilestone Milestone { get; set; }
        public int DealScopeId { get; set; }
        public string FundingStatus { get; set; }
        public int FundingDealId { get; set; }
        public string AssignedTo { get; set; }
        public FundedDeal()
        {
            Milestone=new FundingMilestone();
        }
    }
}
