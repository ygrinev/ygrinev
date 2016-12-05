using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDealTrustAccount")]
    public partial class tblDealTrustAccount
    {
        public tblDealTrustAccount()
        {
            tblDeals = new HashSet<tblDeal>();
            tblMortgages = new HashSet<tblMortgage>();
        }

        [Key]
        public int DealTrustAccountID { get; set; }

        [StringLength(200)]
        public string BankName { get; set; }

        public int? BankAccID { get; set; }

        [StringLength(50)]
        public string FinanceAccount { get; set; }

        [StringLength(10)]
        public string BankNum { get; set; }

        [StringLength(10)]
        public string BranchNum { get; set; }

        [StringLength(20)]
        public string AccountNum { get; set; }

        [StringLength(200)]
        public string BankAddress { get; set; }

        [StringLength(50)]
        public string HolderName { get; set; }

        public virtual ICollection<tblDeal> tblDeals { get; set; }

        public virtual ICollection<tblMortgage> tblMortgages { get; set; }
    }
}
