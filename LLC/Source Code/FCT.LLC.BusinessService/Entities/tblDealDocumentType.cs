using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDealDocumentType")]
    public class tblDealDocumentType
    {
        [Key]
        public int DealDocumentTypeID { get; set; }

        public int DealID { get; set; }
        public int DocumentTypeID { get; set; }
        public bool IsActive { get; set; }
        public int? DisplayNameSuffix { get; set; }

        public virtual tblDocumentType tblDocumentType { get; set; }

    }
}
