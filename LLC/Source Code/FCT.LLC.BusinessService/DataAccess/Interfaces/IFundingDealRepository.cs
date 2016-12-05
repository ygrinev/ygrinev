using System;
using System.Collections.Generic;
using System.Linq;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IFundingDealRepository: IRepository<tblDeal>
    {
        FundingDeal GetFundingDeal(int dealId);

        int GetOtherDealInScope(int dealID, int dealscopeID = 0, string currentActingFor = null);

        int GetOtherCancelledDealInScope(int dealID, int dealscopeID = 0);
        DealDetails GetDeal(int dealscopeID, bool lawyerActingForVendor);

        FundingDeal GetFundingDeal(int fundingDealId, bool lawyerActingForVendor);

        FundingDeal InsertFundingDeal(FundingDeal deal, int dealScopeId);

        FundingDeal UpdateFundingDeal(FundingDeal deal, int dealScopeId);

        void UpdateDealStatus(string newStatus, int dealID);

        string GetStatus(int dealId);

        void DeleteFundingDeal(int dealId, bool autoTriggered);

        FundingDeal GetFundingDealForDisbursement(int dealId);
        void UpdateLLCDeal(FundingDeal llcDeal, int dealScopeID);

        void UpdateFundingDealAssignedTo(int dealID, string assignedTo);
        
        void UpdateFundingDealPayoutSent(int dealID, System.DateTime dateTime);

        void UpdateDealToLLC(LLCDeal deal);

        FundingDeal GetCancelledDeal(int dealId);

        int GetExistingDeal(FundingDeal deal, int dealScopeID);

        void UpdateOtherLawyer(Lawyer updatedLawyer, int otherDealId);

        FundingDeal GetOtherDeal(int dealId);

        void UpdateDealStatus(IEnumerable<int> dealIds, string newStatus);

        string GetLawyerActingFor(int dealID);

        Lookup GetOtherDealStatus(int dealId);

        void RemoveDealFromScope(int dealId);

        void UpdateOtherLawyerInfoFromDoProcess(FundingDeal fundingDeal, int dealScopeId);

        void UpdateDealContact(int deal, int contactId);

        void UpdateConfirmClosing(int dealID, bool isConfirmed);

        tblDealScope GetFundingDealByFCTURN(string fctUrn);
    }
}
