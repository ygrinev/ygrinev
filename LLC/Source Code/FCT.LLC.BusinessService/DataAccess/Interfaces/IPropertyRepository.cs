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
   public interface IPropertyRepository:IRepository<tblProperty>
   {
       Property InsertProperty(Property property, int dealID);

       void UpdateProperty(Property property, int dealID );

       void UpdateProperty(tblProperty tEntity);

       int UpdatePropertyForOtherDeal(Property property, int dealID);

       Property GetPropertyByScope(int fundingDealId);
   }
}
