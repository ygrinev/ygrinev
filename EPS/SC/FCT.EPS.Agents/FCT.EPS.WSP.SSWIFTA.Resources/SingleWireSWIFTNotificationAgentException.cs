using System;
using System.Runtime.Serialization;

namespace FCT.EPS.WSP.SSWIFTA.Resources
{
    [Serializable]
    public class SingleWireSWIFTNotificationAgentException : Exception
    {
        public SingleWireSWIFTNotificationAgentException()
            : base() { }

        public SingleWireSWIFTNotificationAgentException(string message)
            : base(message) { }

        public SingleWireSWIFTNotificationAgentException(string format, params object[] args)
            : base(string.Format(format, args)) { }

        public SingleWireSWIFTNotificationAgentException(string message, Exception innerException)
            : base(message, innerException) { }

        public SingleWireSWIFTNotificationAgentException(string format, Exception innerException, params object[] args)
            : base(string.Format(format, args), innerException) { }

        protected SingleWireSWIFTNotificationAgentException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}