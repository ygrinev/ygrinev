using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblBillingAmountDetailDiscount")]
    public partial class tblBillingAmountDetailDiscount
    {
        [Key]
        public int DiscountID { get; set; }

        public int BillingAmountDetailID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public virtual tblBillingAmountDetail tblBillingAmountDetail { get; set; }
    }
}
