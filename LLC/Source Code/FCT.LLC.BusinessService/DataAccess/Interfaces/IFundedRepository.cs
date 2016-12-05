using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess.Interfaces
{
    public interface IFundedRepository:IRepository<tblFundingDeal>
    {
        int InsertFundedDeal(FundedDeal fundedDeal);
        void UpdateFundedDeal(FundedDeal fundedDeal);
        FundingMilestone UpdateMilestones(int fundedDealId, FundingMilestone fundingMilestone);
        FundedDeal GetMilestonesByDeal(int dealId);
        int GetFundingDealIdByScope(int dealscopeId);
        FundedDeal GetFundingDealByScope(int dealscopeId);
        int GetFundingDealIdByDeal(int dealId);
        FundingMilestone UpdateDisbursedMilestone(int fundingDealId);
        void ResetMilestones(int fundedDealId);
    }
}
