using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblComment")]
    public partial class tblComment
    {
        [Key]
        public int CommentID { get; set; }

        public int DealID { get; set; }

        [StringLength(100)]
        public string DocumentType { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual tblDeal tblDeal { get; set; }
    }
}
