using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDealDocumentPackage")]
    public partial class tblDealDocumentPackage
    {
        [Key]
        public int DealDocumentPackageID { get; set; }

        public int DealID { get; set; }

        [Required]
        [StringLength(10)]
        public string PackageLenderName { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual tblDeal tblDeal { get; set; }
    }
}
