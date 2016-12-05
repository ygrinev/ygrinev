using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderChangeUnsecuredDebt")]
    public partial class tblLenderChangeUnsecuredDebt
    {
        [Key]
        public int LenderChangeUnsecuredDebtID { get; set; }

        public int LenderChangeID { get; set; }

        public int LenderChangeMortgagorID { get; set; }

        [StringLength(50)]
        public string Action { get; set; }

        [StringLength(50)]
        public string Creditor { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
    }
}
