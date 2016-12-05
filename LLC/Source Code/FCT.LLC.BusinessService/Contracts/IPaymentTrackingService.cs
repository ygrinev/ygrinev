#define INCLUDE_TRANS

using System.ServiceModel;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.Contracts.FaultContracts;

namespace FCT.LLC.BusinessService.Contracts
{
    [ServiceContract]
    public interface IPaymentTrackingService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceNotAvailableFault))]
#if INCLUDE_TRANS
        [TransactionFlow(TransactionFlowOption.Allowed)]
#endif
        PaymentNotificationResponse NotifyPaymentConfirmation(PaymentNotificationRequest request);
    }
}
