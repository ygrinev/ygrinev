using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IMilestoneUpdater
    {
        MilestoneUpdated UpdateMilestones(int dealId, string lawyerActingFor);

        void UpdateMilestoneDepositReceived(FundedDeal milestones);
    }
}
