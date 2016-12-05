using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IDealScopeRepository:IRepository<tblDealScope>
    {
        int InsertDealScope(DealScope dealScope);

        int GetDealScope(string FCTRefNumberShort, string wireDepositCode = null);

        string GetFCTRefNumber(string FCTRefNumberShort);

        void UpdateWireDepositDetails(DealScope dealScope);

        void UpdateDealScope(DealScope dealScope);

        void OverwriteDealScopeDetails(DealScope dealScope);

        void DeleteDealScope(int DealScopeID);
    }
}
