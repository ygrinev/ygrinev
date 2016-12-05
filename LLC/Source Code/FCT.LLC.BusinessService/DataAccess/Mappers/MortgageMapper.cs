using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    class MortgageMapper : IEntityMapper<tblMortgage, Mortgage>
    {
        public Mortgage MapToData(tblMortgage tEntity, object parameters = null)
        {
            if (tEntity != null && tEntity.MortgageAmount.HasValue)
            {
                return new Mortgage { MortgageAmount = tEntity.MortgageAmount.Value };
            }

            return null;
        }

        public tblMortgage MapToEntity(Mortgage tData)
        {
            tblMortgage mortgage = new tblMortgage();

            if (tData != null)
                mortgage.MortgageAmount = tData.MortgageAmount;

            return mortgage;
        }
    }
}
