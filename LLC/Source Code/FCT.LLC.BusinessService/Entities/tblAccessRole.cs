using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblAccessRole")]
    public partial class tblAccessRole
    {
        public tblAccessRole()
        {
            tblBranchContacts = new HashSet<tblBranchContact>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccessRoleID { get; set; }

        [StringLength(50)]
        public string AccessRole { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        public virtual ICollection<tblBranchContact> tblBranchContacts { get; set; }
    }
}
