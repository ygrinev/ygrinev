using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderSecurityQuestion")]
    public partial class tblLenderSecurityQuestion
    {
        [Key]
        public int LenderSecurityQuestionID { get; set; }

        public int ContactID { get; set; }

        [Required]
        [StringLength(256)]
        public string Question { get; set; }

        [Required]
        [MaxLength(256)]
        public byte[] Answer { get; set; }

        public virtual tblBranchContact tblBranchContact { get; set; }
    }
}
