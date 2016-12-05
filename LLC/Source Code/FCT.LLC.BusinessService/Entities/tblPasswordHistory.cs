using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPasswordHistory")]
    public partial class tblPasswordHistory
    {
        [Key]
        public int PasswordHitoryID { get; set; }

        [StringLength(20)]
        public string UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserType { get; set; }

        [Required]
        [MaxLength(200)]
        public byte[] Password { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] TimeUpdated { get; set; }

        public DateTime PasswordChangedDate { get; set; }
    }
}
