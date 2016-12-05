
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace FCT.EPS.WSP.SCPRTPTA.Resources
{
    [Serializable]
    public class CreditNotificationAgentException : Exception
    {
        public CreditNotificationAgentException()
            : base() { }

        public CreditNotificationAgentException(string message)
            : base(message) { }

        public CreditNotificationAgentException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public CreditNotificationAgentException(string message, Exception innerException)
            : base(message, innerException) { }

        public CreditNotificationAgentException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected CreditNotificationAgentException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}