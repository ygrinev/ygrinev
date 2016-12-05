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
   public interface IPINRepository:IRepository<tblPIN>
   {
        PinCollection InsertPINRange(IEnumerable<Pin> pins, int propertyID);

        void UpdatePINRange(IEnumerable<Pin> pins, int propertyID);

       void UpdatePINRangeForOtherDeal(IEnumerable<Pin> pins, int propertyID);

       void DeleteAndInsertPins(int PropertyID, int FromPropertyID);

       IEnumerable<tblPIN> GetPINs(int propertyID);
    }
}
