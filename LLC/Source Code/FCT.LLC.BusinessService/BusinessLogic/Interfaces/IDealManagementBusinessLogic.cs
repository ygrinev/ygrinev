using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IDealManagementBusinessLogic
    {
        GetFundingDealResponse GetFundingDeal(GetFundingDealRequest request);

        SaveFundingDealResponse SaveFundingDeal(SaveFundingDealRequest request);

        void DeleteDraftDeal(DeleteDraftDealRequest request);

        void SyncFundingDealData(SyncFundingDealDataRequest request);
    }
}
