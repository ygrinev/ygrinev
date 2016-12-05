using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IPaymentNotificationRepository:IRepository<tblPaymentNotification>
    {
        void InsertNotificationRange(IEnumerable<PaymentNotification> paymentNotifications);
    }
}
