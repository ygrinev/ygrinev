using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPayeeTypeDocumentType")]
    public partial class tblPayeeTypeDocumentType
    {
        [Required]
        [StringLength(50)]
        public string PayeeType { get; set; }

        public int DocumentTypeID { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [StringLength(50)]
        public string AccountAction { get; set; }

        [Key]
        public int PayeeTypeDocumentTypeID { get; set; }

        public virtual tblDocumentType tblDocumentType { get; set; }
    }
}
