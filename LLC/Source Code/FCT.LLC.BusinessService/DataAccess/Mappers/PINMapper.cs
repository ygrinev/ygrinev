using System.Collections.Generic;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class PINMapper : IEntityMapper<tblPIN, Pin>
    {
        public Pin MapToData(tblPIN tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var pin = new Pin() {PinID = tEntity.PINID, PINNumber = tEntity.PINNumber, SourceID = tEntity.SourceID};
                return pin;
            }
            return null;
        }

        public tblPIN MapToEntity(Pin tData)
        {
            var pin = new tblPIN() {PINNumber = tData.PINNumber};
            if (tData.PinID.HasValue)
            {
                pin.PINID = (int) tData.PinID;
                pin.SourceID = tData.SourceID;
            }
            return pin;
        }
    }
}
