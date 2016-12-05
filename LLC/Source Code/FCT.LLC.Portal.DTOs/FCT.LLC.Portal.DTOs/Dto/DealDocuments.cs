using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.Portal.DTOs.Dto
{
    public partial class DealDocuments
    {

        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public List<DealDocument> lenderDocs { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public List<DealDocument> lawyerDocs { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public List<DealDocument> lawyerPublishedDocs { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public List<DealDocument> lawyerSubmittedDocs { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public List<DealDocument> fctDocs { get; set; }
    }
}
