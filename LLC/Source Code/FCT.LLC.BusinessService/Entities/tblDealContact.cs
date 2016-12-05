using FCT.LLC.GenericRepository;

namespace FCT.LLC.BusinessService.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [Table("tblDealContact")]
    public partial class tblDealContact
    {
        [DataMember]
        [Key]
        public int DealContactID { get; set; }

        [DataMember]
        public int DealID { get; set; }

        [DataMember]
        public int LawyerID { get; set; }

        [DataMember]
        public virtual tblDeal tblDeal { get; set; }

        [DataMember]
        public virtual vw_Deal vw_Deal { get; set; }

        [DataMember]
        public virtual tblLawyer tblLawyer { get; set; }
    }
}
