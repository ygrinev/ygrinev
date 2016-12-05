using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblBillingAmountDetail")]
    public partial class tblBillingAmountDetail
    {
        public tblBillingAmountDetail()
        {
            tblBillingAmountDetailDiscounts = new HashSet<tblBillingAmountDetailDiscount>();
            tblBillingAmountDetailTaxes = new HashSet<tblBillingAmountDetailTax>();
        }

        [Key]
        public int BillingAmountDetailID { get; set; }

        [Column(TypeName = "money")]
        public decimal SubTotalAmount { get; set; }

        [StringLength(200)]
        public string TotalDescription { get; set; }

        [Column(TypeName = "money")]
        public decimal TotalAmount { get; set; }

        public virtual ICollection<tblBillingAmountDetailDiscount> tblBillingAmountDetailDiscounts { get; set; }

        public virtual ICollection<tblBillingAmountDetailTax> tblBillingAmountDetailTaxes { get; set; }
    }
}
