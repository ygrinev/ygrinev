using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
//using FCT.LLC.Portal.DTOs.Requests;
using System.Linq;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IDealBusinessLogic
    {        
        GetDealResponse GetDeal(GetDealRequest request);

        tblDeal GetTbDeal(GetDealRequest request);

        tblDeal GetTbDealByFCTURN(string FCTURN);

        tblDeal GetTbDealByDealId(int dealId);

        void UpdateDealStatus(UpdateDealStatusRequest request);
                
        GetNotesResponse GetNotes(GetNotesRequest request);
               
        GetDealHistoryResponse GetDealHistory(GetDealHistoryRequest request);

        GetMilestonesResponse GetDealMilestones(GetMilestonesRequest request);

        int GetOtherDealid(GetDealRequest request);

        //Added By MEHDI
        IQueryable<tblDealHistory> GetDealHistories(int dealId);
    }
}
