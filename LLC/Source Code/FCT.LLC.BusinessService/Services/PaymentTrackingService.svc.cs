#define INCLUDE_TRANS

using System;
using System.ServiceModel;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.BusinessLogic.Interfaces;
using FCT.LLC.BusinessService.Contracts;
using FCT.LLC.BusinessService.Contracts.FaultContracts;
using FCT.LLC.Logging;

namespace FCT.LLC.BusinessService
{

#if INCLUDE_TRANS
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, 
        TransactionIsolationLevel = System.Transactions.IsolationLevel.ReadCommitted)]
#else
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
#endif
    public class PaymentTrackingService : IPaymentTrackingService
    {
        private readonly IPaymentBusinessLogic _paymentBusinessLogic;
        private readonly ILogger _logger;

        public PaymentTrackingService(IPaymentBusinessLogic paymentBusinessLogic, ILogger logger)
        {
            _paymentBusinessLogic = paymentBusinessLogic;
            _logger = logger;
        }

#if INCLUDE_TRANS
        [OperationBehavior(TransactionScopeRequired = true)]
#endif
        public PaymentNotificationResponse NotifyPaymentConfirmation(PaymentNotificationRequest request)
        {
            try
            {
                //  UserContextHelper.SetUserContext( );
                _paymentBusinessLogic.ReceivePaymentNotification(request);
                var response = new PaymentNotificationResponse() {NotificationRecieved = true};
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogUnhandledError(ex);
                var fault = new ServiceNotAvailableFault {Message = ex.Message};
                throw new FaultException<ServiceNotAvailableFault>(fault, fault.Message);
            }
        }
    }
}
