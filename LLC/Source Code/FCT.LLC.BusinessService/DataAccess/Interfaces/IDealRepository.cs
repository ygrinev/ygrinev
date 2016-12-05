using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Contracts;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IDealRepository : IRepository<tblDeal>
    {
        tblDeal GetDealDetails(int DealID, UserContext userContext, bool DealInfoOnly = false);

        tblDeal GetDealDetailsByFCTURN(string FCTURN, bool DealInfoOnly = false);

        tblDeal DealSync(tblDeal sourceDeal, tblDeal targetDeal, UserContext userContext);

        tblDeal GetDealNotes(int DealID, UserContext userContext);

        tblDeal GetDealHistory(int DealID, UserContext userContext);

        tblDeal GetMilestones(int DealID, UserContext userContext);

        void UpdateDealStatus(int DealID, string status, UserContext userContext);

        int GetOtherDealInScope(int dealID, int dealscopeID);

        void UpdateDeal(tblDeal deal);

        void RemoveDealScopeFromDeal(int DealID);

        void DeleteDeal(int DealID);

        tblBranchContact GetBranchContact(int ContactID);

        bool IsTwoWayLender(int dealId);

        bool IsFCTLender(int dealId);

        tblDeal GetDealWithDealScopeDetails(int dealId);

        tblFinancialInstitutionNumber GetFinancialInstitutionDetails(string FINumber);

        IQueryable<tblDealHistory> GetDealHistory(int DealID);

        tblDeal GetDeal(int DealID, bool DealInfoOnly = false);
    }
}
