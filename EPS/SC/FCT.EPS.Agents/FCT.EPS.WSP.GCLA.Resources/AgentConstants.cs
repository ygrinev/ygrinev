using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.GCLA.Resources
{
    public class AgentConstants
    {
        public struct Misc
        {
            public const string LOGGING_APPLICATION_TITLE = "FCT.EPS.WSP.GCLA";
            public const int LOGGING_BUSINESS_RULES_PRIORITY = 2;
            public static readonly TimeSpan CCLIST_MAX_PROCESSING_TIME = new TimeSpan(1,0,0);
        }
        public class EventID
        {
            public const int GET_CCLIST_AGENT = 1077;
        }
    }
}
