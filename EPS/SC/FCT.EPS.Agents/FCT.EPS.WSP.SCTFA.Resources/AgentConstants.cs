using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.SCTFA.Resources
{
    public class AgentConstants
    {
        public class Misc
        {
            public const string LOGGING_APPLICATION_TITLE = "FCT.EPS.WSP.SCTFA";
            public const int DEFAULT_CHEQUE_STATUS = 1;
            public const string SERVICE_DESCRIPTION = "Enterprise Payment Service";
            public const int LOGGING_BUSINESS_RULES_PRIORITY = 2;
        }

        public class EventID
        {
            public const int SEND_CHEQUES_AGENT = 1007;
        }

    }
}
