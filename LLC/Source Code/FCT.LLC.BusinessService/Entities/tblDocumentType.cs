using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentType")]
    public partial class tblDocumentType
    {

        public tblDocumentType()
        {
            tblDealDocumentType = new HashSet<tblDealDocumentType>();
        }

        [Key]
        public int DocumentTypeID { get; set; }
        public int DocumentCategoryID { get; set; }
        public int? DocumentTypeCodeID { get; set; }
        public bool IsSignatureRequired { get; set; }
        public bool IsUploadable { get; set; }
        public bool IsGeneratable { get; set; }
        public string Name { get; set; }
        public bool IsSubmitable { get; set; }
        public bool IsOther { get; set; }
        public bool IsDefaultType { get; set; }
        public int? LenderID { get; set; }
        public bool IsEditable { get; set; }
        public bool IsCacheable { get; set; }
        public bool VirusScan { get; set; }

        [StringLength(2000)]
        public string IsSubmitableCondition { get; set; }
        public bool IsArchivable { get; set; }

        [StringLength(2000)]
        public string IsUploadableCondition { get; set; }

        [StringLength(2000)]
        public string IsArchivableCondition { get; set; }
        public bool IsPublishable { get; set; }
        public string IsPublishableCondition { get; set; }
        public string BusinessModel { get; set; }

        public virtual ICollection<tblDealDocumentType> tblDealDocumentType { get; set; }

    }
}
