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
    [Table("tblPIN")]
    public partial class tblPIN
    {
        [DataMember]
        [Key]
        public int PINID { get; set; }

        [DataMember]
        public int PropertyID { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string PINNumber { get; set; }

        [DataMember]
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        [DataMember]
        public int? SourceID { get; set; }

        [DataMember]
        public virtual tblProperty tblProperty { get; set; }
    }
}
