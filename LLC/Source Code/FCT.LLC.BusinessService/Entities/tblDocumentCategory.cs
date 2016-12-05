using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentCategory")]
    public partial class tblDocumentCategory
    {
        public tblDocumentCategory()
        {
            tblDocumentTypes = new HashSet<tblDocumentType>();
        }

        [Key]
        public int DocumentCategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<tblDocumentType> tblDocumentTypes { get; set; }
    }
}
