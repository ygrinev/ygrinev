using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class EPSToLLCPaymentMapper:IEntityMapper<tblPaymentNotification, PaymentNotification>
    {
        public PaymentNotification MapToData(tblPaymentNotification tEntity, object parameters = null)
        {
            throw new NotImplementedException();
        }

        public tblPaymentNotification MapToEntity(PaymentNotification tData)
        {
            if (tData != null)
            {
                var entity = new tblPaymentNotification()
                {
                    BatchDescription = tData.PaymentBatchDescription,
                    BatchID = tData.PaymentBatchID,
                    ReferenceNumber = tData.PaymentReferenceNumber,
                    PaymentAmount = tData.PaymentAmount,
                    PaymentDate = tData.PaymentDateTime,
                    NotificationType = tData.NotificationType.ToString(),
                    PaymentStatus = tData.PaymentStatus,
                    PaymentRequestID = Convert.ToInt32(tData.DisbursementRequestID)
                };
                return entity;
            }
            return null;
        }
    }
}
