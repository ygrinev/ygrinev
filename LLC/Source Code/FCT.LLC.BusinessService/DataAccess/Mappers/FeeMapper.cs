using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class FeeMapper:IEntityMapper<tblFee, Fee>
    {
        public Fee MapToData(tblFee tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var data = new Fee()
                {
                    Amount = Math.Round(tEntity.Amount, 2, MidpointRounding.AwayFromZero),
                    FeeID = tEntity.FeeID,
                    GST = Math.Round(tEntity.GST.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero),
                    HST = Math.Round(tEntity.HST.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero),
                    QST = Math.Round(tEntity.QST.GetValueOrDefault(), 2, MidpointRounding.AwayFromZero)
                };
                return data;
            }
            return null;
            
        }

        public tblFee MapToEntity(Fee tData)
        {
            if (tData != null)
            {
                var entity = new tblFee()
                {
                    Amount = Math.Round(tData.Amount, 2,MidpointRounding.AwayFromZero),
                    FeeID = tData.FeeID.GetValueOrDefault(),
                    GST = Math.Round(tData.GST, 2, MidpointRounding.AwayFromZero),
                    HST = Math.Round(tData.HST, 2, MidpointRounding.AwayFromZero),
                    QST = Math.Round(tData.QST, 2, MidpointRounding.AwayFromZero)
                };
                return entity;
            }
            return null;
        }
    }
}
