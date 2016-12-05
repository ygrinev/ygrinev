using FCT.LLC.GenericRepository;

namespace FCT.LLC.BusinessService.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [Table("tblDealScope")]
    public partial class tblDealScope
    {
        public tblDealScope()
        {
            tblDeals = new HashSet<tblDeal>();
            tblVendors = new HashSet<tblVendor>();
        }

        [DataMember]
        [Key]
        public int DealScopeID { get; set; }

        [DataMember]
        [StringLength(11)]
        public string FCTRefNumber { get; set; }

        [DataMember]
        [StringLength(11)]
        public string ShortFCTRefNumber { get; set; }

        [DataMember]
        [StringLength(20)]
        public string WireDepositVerificationCode { get; set; }

        [DataMember]
        [StringLength(50)]
        public string WireDepositDetails { get; set; }

        [DataMember]
        public virtual ICollection<tblDeal> tblDeals { get; set; }

        [DataMember]
        public virtual ICollection<tblVendor> tblVendors { get; set; }

        [DataMember]
        public virtual ICollection<tblFundingDeal> tblFundingDeals { get; set; }
    }
}
