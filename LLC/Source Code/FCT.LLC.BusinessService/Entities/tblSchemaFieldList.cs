using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblSchemaFieldList")]
    public partial class tblSchemaFieldList
    {
        public tblSchemaFieldList()
        {
            tblSchemaFieldListMappings = new HashSet<tblSchemaFieldListMapping>();
        }

        [Key]
        public int SchemaFieldID { get; set; }

        [Required]
        [StringLength(1000)]
        public string SchemaFieldName { get; set; }

        [StringLength(1000)]
        public string LawyerSchemaField { get; set; }

        public bool? IsShared { get; set; }

        public virtual ICollection<tblSchemaFieldListMapping> tblSchemaFieldListMappings { get; set; }
    }
}
