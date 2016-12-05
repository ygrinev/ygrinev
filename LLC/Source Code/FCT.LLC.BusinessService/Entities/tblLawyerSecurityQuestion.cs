using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLawyerSecurityQuestion")]
    public partial class tblLawyerSecurityQuestion
    {
        [Key]
        public int LawyerSecurityQuestionID { get; set; }

        public int LawyerID { get; set; }

        [Required]
        [StringLength(256)]
        public string Question { get; set; }

        [Required]
        [MaxLength(256)]
        public byte[] Answer { get; set; }

        public virtual tblLawyer tblLawyer { get; set; }
    }
}
