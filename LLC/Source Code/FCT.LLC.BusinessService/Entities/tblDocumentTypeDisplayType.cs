using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentTypeDisplayType")]
    public partial class tblDocumentTypeDisplayType
    {
        public tblDocumentTypeDisplayType()
        {
            tblDocumentTypeDisplays = new HashSet<tblDocumentTypeDisplay>();
        }

        [Key]
        public int DocumentTypeDisplayTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<tblDocumentTypeDisplay> tblDocumentTypeDisplays { get; set; }
    }
}
