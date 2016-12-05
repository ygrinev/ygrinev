using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentTypeCode")]
    public partial class tblDocumentTypeCode
    {
        public tblDocumentTypeCode()
        {
            tblDocumentTypes = new HashSet<tblDocumentType>();
        }

        [Key]
        public int DocumentTypeCodeID { get; set; }

        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        public virtual ICollection<tblDocumentType> tblDocumentTypes { get; set; }
    }
}
