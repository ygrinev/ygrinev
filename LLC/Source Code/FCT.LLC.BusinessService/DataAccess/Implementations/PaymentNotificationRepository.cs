using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class PaymentNotificationRepository:Repository<tblPaymentNotification>, IPaymentNotificationRepository
    {
        private readonly EFBusinessContext _context;
        private readonly IEntityMapper<tblPaymentNotification, PaymentNotification> _paymentMapper;
        public PaymentNotificationRepository(EFBusinessContext context, IEntityMapper<tblPaymentNotification, PaymentNotification> paymentMapper) : base(context)
        {
            _paymentMapper = paymentMapper;
            _context = context;
        }

        public void InsertNotificationRange(IEnumerable<PaymentNotification> paymentNotifications)
        {
            var entities = paymentNotifications.Select(p => _paymentMapper.MapToEntity(p)).ToList();
            foreach (var entity in entities)
            {
                entity.NotificationTimeStamp = DateTime.Now;
            }
            InsertRange(entities);
            _context.SaveChanges();

        }
    }
}
