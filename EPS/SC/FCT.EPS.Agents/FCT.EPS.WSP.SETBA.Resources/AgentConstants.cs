using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.SETBA.Resources
{
    public class AgentConstants
    {
        public class Misc
        {
            public const string LOGGING_APPLICATION_TITLE = "FCT.EPS.WSP.SETBA";
            public const int LOGGING_BUSINESS_RULES_PRIORITY = 1;
            public const int RBC_FOOTER_RECORD_TYPE = 10;
            public const int RBC_BODY_RECORD_TYPE = 05;
            public const int RBC_HEADER_RECORD_TYPE = 01;
        }

        public class EventID
        {
            public const int SEND_ELECTRONIC_TO_BANK_AGENT = 4101;
        }
    }
}
