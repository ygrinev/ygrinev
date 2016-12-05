using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentComment")]
    public partial class tblDocumentComment
    {
        [Key]
        public int DocumentCommentID { get; set; }

        public int DealDocumentTypeID { get; set; }

        [Required]
        [StringLength(1000)]
        public string Comment { get; set; }

        public DateTime EnterDate { get; set; }

        public virtual tblDealDocumentType tblDealDocumentType { get; set; }
    }
}
