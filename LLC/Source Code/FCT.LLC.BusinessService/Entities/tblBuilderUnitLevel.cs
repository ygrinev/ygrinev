using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [Table("tblBuilderUnitLevel")]
    public class tblBuilderUnitLevel
    {
        [DataMember]
        [Key]
        public int BuilderUnitLevelId { get; set; }

        [DataMember]
        [StringLength(20)]
        public string Unit { get; set; }

        [DataMember]
        [StringLength(20)]
        public string Level { get; set; }

        [DataMember]
        public int BuilderLegalDescriptionID { get; set; }

        [DataMember]
        public virtual tblBuilderLegalDescription tblBuilderLegalDescription { get; set; }
    }
}
