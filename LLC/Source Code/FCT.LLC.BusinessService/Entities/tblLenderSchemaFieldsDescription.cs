using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderSchemaFieldsDescription")]
    public partial class tblLenderSchemaFieldsDescription
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

        [StringLength(1000)]
        public string AlternateDescription { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LanguageID { get; set; }

        public virtual tblLanguage tblLanguage { get; set; }

        public virtual tblLenderSchemaField tblLenderSchemaField { get; set; }
    }
}
