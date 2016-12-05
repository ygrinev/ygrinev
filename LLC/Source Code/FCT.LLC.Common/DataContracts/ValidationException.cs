using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.Common.DataContracts
{
    public class ValidationException:InvalidOperationException
    {
        public List<ErrorCode> ErrorCodes { get; set; }

        public ValidationException(List<ErrorCode> errorcodes)
        {
            ErrorCodes = errorcodes;
        }
    }
}
