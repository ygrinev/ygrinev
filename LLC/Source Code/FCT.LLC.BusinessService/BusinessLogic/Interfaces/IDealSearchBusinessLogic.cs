using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IDealSearchBusinessLogic
    {
        SearchDealResponse SearchDeal(SearchDealRequest request);        
    }
}
