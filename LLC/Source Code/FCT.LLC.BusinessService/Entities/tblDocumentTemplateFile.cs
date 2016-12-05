using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FCT.LLC.BusinessService.Entities
{
    public partial class tblDocumentTemplateFile
    {
        public tblDocumentTemplateFile()
        {
            tblDocumentTemplates = new HashSet<tblDocumentTemplate>();
        }

        [Key]
        [StringLength(250)]
        public string TemplateFileName { get; set; }

        [Required]
        [MaxLength(16)]
        public byte[] Hash { get; set; }

        [Required]
        public byte[] FileData { get; set; }

        public virtual ICollection<tblDocumentTemplate> tblDocumentTemplates { get; set; }
    }
}
