using System.Collections.Generic;
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
    [Table("tblBuilderLegalDescription")]
    public partial class tblBuilderLegalDescription
    {
        public tblBuilderLegalDescription()
        {
            tblBuilderUnitLevels = new HashSet<tblBuilderUnitLevel>();
        }

        [DataMember]
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int BuilderLegalDescriptionID { get; set; }

        [DataMember]
        [StringLength(30)]
        public string BuilderProjectReference { get; set; }

        [DataMember]
        [StringLength(50)]
        public string BuilderLot { get; set; }

        [DataMember]
        [StringLength(30)]
        public string Lot { get; set; }

        [DataMember]
        [StringLength(30)]
        public string Plan { get; set; }

        [DataMember]
        public int PropertyID { get; set; }

        [DataMember]
        public virtual tblProperty tblProperty { get; set; }

        [DataMember]
        public virtual ICollection<tblBuilderUnitLevel> tblBuilderUnitLevels { get; set; }
    }
}
