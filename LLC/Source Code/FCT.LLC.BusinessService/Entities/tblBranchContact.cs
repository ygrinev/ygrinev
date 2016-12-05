using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblBranchContact")]
    public partial class tblBranchContact
    {
        [Key]
        public int ContactID { get; set; }

        public int BranchID { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(6)]
        public string Extension { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string EMail { get; set; }

        public int? AccessRoleID { get; set; }

        public int? FundingRoleID { get; set; }

        public bool? Active { get; set; }

        [StringLength(20)]
        public string UserID { get; set; }

        [MaxLength(256)]
        public byte[] Password { get; set; }

        public bool? PasswordReset { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? PasswordNotifiedDate { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        public DateTime? PasswordSetDate { get; set; }

        public int UserStatusID { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; }

        public int LockLevelID { get; set; }

        public int FailedLoginCount { get; set; }

        public int FailedVerificationCount { get; set; }
    }
}
