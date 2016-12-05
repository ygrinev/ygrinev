using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.Contracts.FaultContracts
{
    [DataContract]
    public class ValidationFault
    {
        [DataMember(Name = "Message", IsRequired = true, Order = 1)]
        public string Message { get; set; }

        [DataMember(Name = "ErrorCodes", IsRequired = true, Order = 2)]
        public List<ErrorCode> ErrorCodes { get; set; }
    }
}
