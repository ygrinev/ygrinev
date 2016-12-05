using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblUnsecuredDebt")]
    public partial class tblUnsecuredDebt
    {
        [Key]
        public int UnsecuredDebtID { get; set; }

        public int? MortgagorID { get; set; }

        [StringLength(50)]
        public string Action { get; set; }

        [StringLength(100)]
        public string Creditor { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }
    }
}
