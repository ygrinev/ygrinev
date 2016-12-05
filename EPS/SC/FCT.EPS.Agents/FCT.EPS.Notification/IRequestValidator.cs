using FCT.EPS.DataEntities;
using System.Collections.Generic;

namespace FCT.EPS.Notification
{
    public interface IRequestValidator
    {
        bool IsValid(tblPaymentNotification PaymentRequest, ref List<string> invalidFieldsList);
    }
}
