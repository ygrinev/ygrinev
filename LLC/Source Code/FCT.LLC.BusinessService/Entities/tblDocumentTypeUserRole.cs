using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentTypeUserRole")]
    public partial class tblDocumentTypeUserRole
    {
        [Key]
        public int DocumentTypeUserRoleID { get; set; }

        public int DocumentTypeID { get; set; }

        [Required]
        [StringLength(20)]
        public string UserRole { get; set; }

        public virtual tblDocumentType tblDocumentType { get; set; }
    }
}
