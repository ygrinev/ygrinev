using System.Collections.Generic;
using System.Linq;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;


namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class LenderMapper : IEntityMapper<tblLender,Lender>
    {

        public Lender MapToData(tblLender tEntity, object parameters = null)
        {
            if (tEntity != null)
            {

                Lender lender = new Lender
                {
                    LenderName = tEntity.Name,                    
                    Phone = tEntity.Phone,
                    LenderCode = tEntity.ShortName,
                    Fax = tEntity.Fax
                };                
                return lender;
            }
            return null;
        }

        public tblLender MapToEntity(Lender tData)
        {
            var lender = new tblLender
            {
                Name = tData.LenderName,
                Phone = tData.Phone,
                ShortName = tData.LenderCode,
                Fax = tData.Fax
            };
            
            return lender;
        }

    }
}
