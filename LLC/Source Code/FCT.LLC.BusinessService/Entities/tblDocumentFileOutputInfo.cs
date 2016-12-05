using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentFileOutputInfo")]
    public partial class tblDocumentFileOutputInfo
    {
        public tblDocumentFileOutputInfo()
        {
            tblDocumentTemplates = new HashSet<tblDocumentTemplate>();
        }

        [Key]
        public int DocumentFileOutputInfo { get; set; }

        [Required]
        [StringLength(50)]
        public string Value { get; set; }

        public virtual ICollection<tblDocumentTemplate> tblDocumentTemplates { get; set; }
    }
}
