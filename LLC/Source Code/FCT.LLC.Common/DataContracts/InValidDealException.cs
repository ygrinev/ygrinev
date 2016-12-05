using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.Common.DataContracts
{
    public class InValidDealException:Exception
    {
        public ErrorCode ViolationCode { get; set; }
        public Exception BaseException { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
