using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentTypeMapping")]
    public partial class tblDocumentTypeMapping
    {
        public tblDocumentTypeMapping()
        {
            tblDocumentPackageMappings = new HashSet<tblDocumentPackageMapping>();
        }

        [Key]
        public int DocumentTypeMappingID { get; set; }

        public int DocumentTypeID { get; set; }

        [StringLength(100)]
        public string DocumentTypeLenderName { get; set; }

        public int LenderID { get; set; }

        public int? DocumentTemplateID { get; set; }

        public virtual ICollection<tblDocumentPackageMapping> tblDocumentPackageMappings { get; set; }

        public virtual tblDocumentTemplate tblDocumentTemplate { get; set; }

        public virtual tblDocumentType tblDocumentType { get; set; }

        public virtual tblLender tblLender { get; set; }
    }
}
