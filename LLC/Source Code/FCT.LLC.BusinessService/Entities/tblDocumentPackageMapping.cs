using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentPackageMapping")]
    public partial class tblDocumentPackageMapping
    {
        [Key]
        public int DocumentPackageMappingID { get; set; }

        [Required]
        [StringLength(10)]
        public string PackageName { get; set; }

        public int DocumentTypeMappingID { get; set; }

        public int LenderID { get; set; }

        public virtual tblDocumentTypeMapping tblDocumentTypeMapping { get; set; }

        public virtual tblLender tblLender { get; set; }
    }
}
