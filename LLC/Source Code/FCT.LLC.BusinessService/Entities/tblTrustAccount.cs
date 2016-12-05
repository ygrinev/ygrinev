using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblTrustAccount")]
    public partial class tblTrustAccount
    {
        public tblTrustAccount()
        {
            tblTrustAccountLenders = new HashSet<tblTrustAccountLender>();
        }

        [Key]
        public int TrustAccountID { get; set; }

        [StringLength(200)]
        public string BankName { get; set; }

        public int? OwnerID { get; set; }

        [StringLength(20)]
        public string OwnerType { get; set; }

        public int? BankAccID { get; set; }

        [StringLength(50)]
        public string FinanceAccount { get; set; }

        public bool? Active { get; set; }

        [StringLength(10)]
        public string BankNum { get; set; }

        [StringLength(10)]
        public string BranchNum { get; set; }

        [StringLength(20)]
        public string AccountNum { get; set; }

        [StringLength(200)]
        public string BankAddress { get; set; }

        public bool? IsCheque { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [StringLength(42)]
        public string AccountNumber { get; set; }

        public bool IsForAllLenders { get; set; }

        [StringLength(50)]
        public string HolderName { get; set; }

        public virtual ICollection<tblTrustAccountLender> tblTrustAccountLenders { get; set; }
    }
}
