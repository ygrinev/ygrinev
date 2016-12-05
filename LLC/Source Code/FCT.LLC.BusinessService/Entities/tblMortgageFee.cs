using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblMortgageFee")]
    public partial class tblMortgageFee
    {
        [Key]
        public int MortgageFeeID { get; set; }

        public int? MortgageID { get; set; }

        public int? FeeTypeID { get; set; }

        [Column(TypeName = "money")]
        public decimal? Amount { get; set; }

        public int? PaymentMethodID { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        public int? CMHCID { get; set; }

        [Column(TypeName = "money")]
        public decimal? Tax { get; set; }

        [StringLength(10)]
        public string Payer { get; set; }

        [StringLength(10)]
        public string Recipient { get; set; }

        [StringLength(50)]
        public string OtherFeeType { get; set; }

        public virtual tblFundingPaymentMethod tblFundingPaymentMethod { get; set; }

        public virtual tblMortgage tblMortgage { get; set; }

        public virtual tblMortgageFeeType tblMortgageFeeType { get; set; }
    }
}
