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
   public interface IDealContactRepository:IRepository<tblDealContact>
   {
       ContactCollection InsertDealContactRange(IEnumerable<Contact> contacts, int dealID);

       void UpdateDealContactRange(IEnumerable<Contact> contacts, int dealID);

       IEnumerable<tblDealContact> GetDealContacts(int dealId);

       IEnumerable<int> GetDealContactIDs(int dealId);
   }
}
