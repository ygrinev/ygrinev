using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.ExternalResources.PaymentTrackingServiceReference;

namespace FCT.EPS.WSP.GCSA.Resources
{
    public class AgentConstants
    {
        public class Misc
        {
            public const string LOGGING_APPLICATION_TITLE = "FCT.EPS.WSP.GCSA";
            public const int ERRORED_CHEQUE_STATUS = -1;
            public const string SERVICE_DESCRIPTION = "Enterprise Payment Service";
            public const int LOGGING_BUSINESS_RULES_PRIORITY = 2;
       }

        public class EventID
        {
            public const int GET_CHEQUES_AGENT = 1007;
        }
    }
}
