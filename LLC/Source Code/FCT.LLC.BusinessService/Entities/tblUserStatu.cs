using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    public partial class tblUserStatu
    {
        public tblUserStatu()
        {
            tblBranchContacts = new HashSet<tblBranchContact>();
            tblLawyers = new HashSet<tblLawyer>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserStatusID { get; set; }

        [Required]
        [StringLength(10)]
        public string UserStatus { get; set; }

        public virtual ICollection<tblBranchContact> tblBranchContacts { get; set; }

        public virtual ICollection<tblLawyer> tblLawyers { get; set; }
    }
}
