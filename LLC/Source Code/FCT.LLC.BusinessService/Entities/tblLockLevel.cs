using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLockLevel")]
    public partial class tblLockLevel
    {
        public tblLockLevel()
        {
            tblBranchContacts = new HashSet<tblBranchContact>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LockLevelId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<tblBranchContact> tblBranchContacts { get; set; }
    }
}
