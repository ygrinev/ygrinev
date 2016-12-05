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
    [Table("tblVendor")]
    public partial class tblVendor
    {
        [DataMember]
        [Key]
        public int VendorID { get; set; }

        [DataMember]
        public int DealScopeID { get; set; }

        [DataMember]
        [StringLength(10)]
        public string VendorType { get; set; }

        [DataMember]
        [StringLength(100)]
        public string FirstName { get; set; }

        [DataMember]
        [StringLength(50)]
        public string MiddleName { get; set; }

        [DataMember]
        [StringLength(50)]
        public string LastName { get; set; }

        [DataMember]
        [StringLength(50)]
        public string CompanyName { get; set; }

        [DataMember]
        public virtual tblDealScope tblDealScope { get; set; }
    }
}
