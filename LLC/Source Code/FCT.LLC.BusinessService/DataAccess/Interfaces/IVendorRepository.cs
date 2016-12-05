using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
   public interface IVendorRepository:IRepository<tblVendor>
   {
       VendorCollection InsertVendorRange(IEnumerable<Vendor> vendors, int dealScopeId);

       void UpdateVendorRange(IEnumerable<Vendor> vendors, int dealScopeId);

       IEnumerable<Vendor> GetVendors(int dealScopeId);

       IEnumerable<Vendor> GetVendorsByDeal(int dealId);

       void DeleteRange(IEnumerable<Vendor> vendors);
   }
}
