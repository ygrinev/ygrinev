using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.SBWTSA.Resources
{
    public class AgentConstants
    {
        public class Misc
        {
            public const string LOGGING_APPLICATION_TITLE = "FCT.EPS.WSP.SBWTSA";
            public const string TBLPAYMENTREQUEST_PAYMENTMETHOD = "Wire";
            public const string TBLPAYMENTREQUEST_PAYMENTREQUESTTYPE = "Batch";
            public const string PAYMENT_CURRENCY = "CAD";
            public const int    LOGGING_BUSINESS_RULES_PRIORITY = 1;
            public const string CONTRACT_TYPE = "B";
            public const string PAYEE_TYPE = "C";
            public const string TD_BANK_NUMBER = "004";
            public const string TD_PAYMENT_DESTINATION = "T";
            public const string OTHER_PAYMENT_DESTINATION = "N";
            public const string CANADIAN_CLEARING_CODE_PREFIX = "CC0";
        }

        public class EventID
        {
            public const int SEND_BATCH_WIRE_SWIFT_AGENT = 1031;
        }
    }
}
