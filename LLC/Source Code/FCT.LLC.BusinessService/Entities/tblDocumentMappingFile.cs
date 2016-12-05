using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    public partial class tblDocumentMappingFile
    {
        public tblDocumentMappingFile()
        {
            tblDocumentTemplates = new HashSet<tblDocumentTemplate>();
        }

        [Key]
        [StringLength(250)]
        public string MappingFileName { get; set; }

        [Required]
        [MaxLength(16)]
        public byte[] Hash { get; set; }

        [Column(TypeName = "xml")]
        [Required]
        public string FileData { get; set; }

        public virtual ICollection<tblDocumentTemplate> tblDocumentTemplates { get; set; }
    }
}
