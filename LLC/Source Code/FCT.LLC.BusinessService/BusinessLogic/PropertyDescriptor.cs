using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    [DataContract]
    public class PropertyDescriptor
    {
        [DataMember]
        public int? PropertyID { get; set; }

        [DataMember]
        public string UnitNumber { get; set; }

        [DataMember]
        public string StreetNumber { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string Address2 { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string Province { get; set; }

        [DataMember]
        public string PostalCode { get; set; }

        [DataMember]
        public string Country { get; set; }
    }
}
