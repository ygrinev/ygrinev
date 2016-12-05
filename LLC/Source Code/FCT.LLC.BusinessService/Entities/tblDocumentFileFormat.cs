using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentFileFormat")]
    public partial class tblDocumentFileFormat
    {
        public tblDocumentFileFormat()
        {
            tblLenders = new HashSet<tblLender>();
        }

        [Key]
        public int FileFormatID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Extension { get; set; }

        public virtual ICollection<tblLender> tblLenders { get; set; }
    }
}
