using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.Common.DataContracts
{
    public partial class DisbursementSummary
    {
        [DataMember(IsRequired = true)]
        public byte[] Version { get; set; }
    }
}
