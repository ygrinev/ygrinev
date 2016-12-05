using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.Resources
{
    public class Constants
    {
        public class Misc
        {
            public const string DATABASE_CONNECTION_STRING = "EPSConnectionString";
            //public const string DATABASE_LOG_CONNECTION_STRING = "EPSLogConnectionString";
            public const string LOGGING_APPLICATION_TITLE = "FCT.EPS.WSP.SendAgents";
            public const string ENTITY_VALIDATION_ERRORS = "EntityValidationErrors";
            //public const int DEFAULT_TIMER_INTERVAL = 10000;
            //public const int DEFAULT_RECORDS_TO_GET = 2;
            //public const string DEFAULT_ENDPOINT_NAME = "EPSNetTcpEndpoint";
            //public const string SERVICE_DESCRIPTION = "Enterprise Payment Service";
            //public const int DEFAULT_CHEQUE_STATUS = 1;
        }

        public class DataBase
        {
            public class Tables
            {
                public class tblEPSStatus
                {
                    public const int RECEIVED = 1;
                    public const int SUBMITTED = 2;
                    public const int FAILED = 3;
                    public const int SWIFTReceived = 4;
                    public const int SWIFTError = 5;
                    public const int RBCBPSReceived = 6;
                    public const int RBCBPSError = 7;
                    public const int PROCESSING = 8;
                }

                public class tblPaymentStatus
                {
                    public const int CHEQUECONFIRMATION_ERROR = -1;
                    public const int CHEQUECONFIRMATION_VOID = 0;
                    public const int CHEQUECONFIRMATION_SUBMITTED = 1;
                    public const int CHEQUECONFIRMATION_PAYMENT_IN_PROCESS = 2;
                    public const int CHEQUECONFIRMATION_PAID = 3;
                    public const int CREDITCONFIRMATION_PAID = 1;
                    public const int DEBITCONFIRMATION_PAID = 1;
                    public const int FCTFEECONFIRMATION_ERROR = -1;
                    public const int FCTFEECONFIRMATION_PAYMENT_IN_PROCESS = 0;
                    public const int FCTFEECONFIRMATION_PAID = 1;
                }
            }
        }

    }
}
