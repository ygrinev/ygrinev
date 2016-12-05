using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic.Interfaces
{
    public interface IPaymentBusinessLogic
    {
        void ReceivePaymentNotification(PaymentNotificationRequest request);
    }
}
