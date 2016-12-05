using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    public partial class tblLenderSchemaField
    {
        public tblLenderSchemaField()
        {
            tblLenderSchemaFieldsDescriptions = new HashSet<tblLenderSchemaFieldsDescription>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SchemaFieldID { get; set; }

        [Required]
        [StringLength(1000)]
        public string SchemaFieldName { get; set; }

        [StringLength(1000)]
        public string LawyerSchemaField { get; set; }

        public bool? IsShared { get; set; }

        public virtual ICollection<tblLenderSchemaFieldsDescription> tblLenderSchemaFieldsDescriptions { get; set; }
    }
}
