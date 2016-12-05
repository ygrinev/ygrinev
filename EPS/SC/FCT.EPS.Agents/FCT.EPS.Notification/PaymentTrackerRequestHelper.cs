using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;

namespace FCT.EPS.PaymentTrackingService.DataContracts
{
    public class PaymentTrackerRequestHelper
    {
         PaymentNotificationList _paymentNotificationsList;
        public PaymentTrackerRequestHelper()
        {
            _paymentNotificationsList = new PaymentNotificationList();
            MaxAllowedRetry = 5;
        }

        #region properties

        public PaymentNotificationList NotificationList { get { return _paymentNotificationsList; } }
        public int Count { get { return _paymentNotificationsList != null ? _paymentNotificationsList.Count : 0; } }
        public int NotificationID { get; set; }
        public string FinancePaymentBatchID { get; set; }
        public int PaymentTransactionID { get; set; }
        public int NumberRetried { get; set; }
        public int MaxAllowedRetry { get; set; }
        public bool RetryAgain { get { return MaxAllowedRetry - NumberRetried > 0; } }
        public string ServceEndPoint { get; set; }

        #endregion

        #region properties
        public void AddToPaymentNotificaitonList(PaymentNotification paymentNotification)
        {
            if (paymentNotification != null)
            {
                _paymentNotificationsList.Add(paymentNotification);
            }
        }
        
        public PaymentNotificationRequest GetPaymentNotificationRequest()
        {
            PaymentNotificationRequest paymentTrackerRequest = new PaymentNotificationRequest();
            paymentTrackerRequest.PaymentNotifications = _paymentNotificationsList;
            return paymentTrackerRequest;
        }

        #endregion
    }
}
