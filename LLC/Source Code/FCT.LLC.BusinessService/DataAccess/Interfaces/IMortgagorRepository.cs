using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
   public interface IMortgagorRepository:IRepository<tblMortgagor>
   {
       MortgagorCollection InsertMortgagorRange(IEnumerable<Mortgagor> mortgagors, int dealId);

       void UpdateMorgagorRange(IEnumerable<Mortgagor> mortgagors, int dealId);

       void UpdateMortgagorRangeForOtherDeal(int dealId, int otherDealId);

       void InsertMortgagorRangeForOtherDeal(IEnumerable<Mortgagor> mortgagors, int dealId, int otherDealId);

       IEnumerable<Mortgagor> GetMortgagors(int dealId);

   }
}
