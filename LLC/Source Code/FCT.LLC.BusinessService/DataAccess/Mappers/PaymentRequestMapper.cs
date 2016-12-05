using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class PaymentRequestMapper:IEntityMapper<tblPaymentRequest, LLCPaymentRequest>
    {
        public LLCPaymentRequest MapToData(tblPaymentRequest tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var data = new LLCPaymentRequest()
                {
                    DealFundsAllocationID = tEntity.DealFundsAllocationID.GetValueOrDefault(),
                    DisbursementID = tEntity.DisbursementID.GetValueOrDefault(),
                    Message = tEntity.Message,
                    PaymentRequestID = tEntity.PaymentRequestID,
                    RequestDate = tEntity.RequestDate
                };
                return data;
            }
           return null;
        }

        public tblPaymentRequest MapToEntity(LLCPaymentRequest tData)
        {
            if (tData != null)
            {
                var entity = new tblPaymentRequest()
                {
                    RequestDate = tData.RequestDate,
                    Message = tData.Message
                };
                if (tData.DisbursementID > 0)
                {
                    entity.DisbursementID = tData.DisbursementID;
                }
                else
                {
                    entity.DisbursementID = null;
                }
                if (tData.DealFundsAllocationID > 0)
                {
                    entity.DealFundsAllocationID = tData.DealFundsAllocationID;
                }
                else
                {
                    entity.DealFundsAllocationID = null;
                }
                return entity;
            }
           return null;
        }
    }
}
