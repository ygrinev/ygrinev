using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblSchemaFieldListMapping")]
    public partial class tblSchemaFieldListMapping
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LenderID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SchemaFieldID { get; set; }

        [Required]
        [StringLength(1000)]
        public string BusinessDescription { get; set; }

        public bool IsInclude { get; set; }

        public virtual tblLender tblLender { get; set; }

        public virtual tblSchemaFieldList tblSchemaFieldList { get; set; }
    }
}
