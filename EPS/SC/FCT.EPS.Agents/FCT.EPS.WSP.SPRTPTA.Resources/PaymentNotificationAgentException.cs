using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace FCT.EPS.WSP.SPRTPTA.Resources
{
    [Serializable]
    public class PaymentNotificationAgentException : Exception
    {
        public PaymentNotificationAgentException()
            : base() { }

        public PaymentNotificationAgentException(string message)
            : base(message) { }

        public PaymentNotificationAgentException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public PaymentNotificationAgentException(string message, Exception innerException)
            : base(message, innerException) { }

        public PaymentNotificationAgentException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected PaymentNotificationAgentException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}