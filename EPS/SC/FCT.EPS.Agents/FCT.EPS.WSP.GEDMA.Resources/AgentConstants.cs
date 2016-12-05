using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.GEDMA.Resources
{
    public class AgentConstants
    {
        public class Misc
        {
            public const string LOGGING_APPLICATION_TITLE = "FCT.EPS.WSP.GEDMA";
            //public const string SERVICE_DESCRIPTION = "Enterprise Payment Service";
            //public const string WIRE_FILE_STARTS_WITH = "WIRE";
            //public const string EFT_FILE_STARTS_WITH = "EFT";
            //public const string DEBIT_FILE_MASK = "DebitConf*.txt";
            //public const string CREDIT_FILE_MASK = "CreditConf*.txt";
            //public const string ERR_FILE_MASK = "*.err";
            //public const int NACK_STATUS = 1;
            //public const int ACK_STATUS = 0;
            //public const string CREDIT_NOTIFICATION_TYPE = "CreditConfirmation";
            //public const string DEBIT_NOTIFICATION_TYPE = "DebitConfirmation";
            public const int LOGGING_BUSINESS_RULES_PRIORITY = 1;

            public const string REJECTED_FILE_MASK = "ERRFILE.*";

            public const string ACCEPTED_FILE_MASK = "*.FIN";
        }

        public class EventID
        {
            public const int GET_SWIFT_AGENT = 6013;
        }

    }
}
