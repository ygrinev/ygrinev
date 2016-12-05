using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblMortgagePayment")]
    public partial class tblMortgagePayment
    {
        public tblMortgagePayment()
        {
            tblMortgagePaymentRegistrationDatas = new HashSet<tblMortgagePaymentRegistrationData>();
        }

        [Key]
        public int MortgagePaymentID { get; set; }

        public int MortgageID { get; set; }

        [Required]
        [StringLength(50)]
        public string MortgagePaymentType { get; set; }

        [Column(TypeName = "money")]
        public decimal? PaymentAmount { get; set; }

        [StringLength(100)]
        public string PaymentPeriod { get; set; }

        public DateTime? FirstPaymentDate { get; set; }

        [Column(TypeName = "smallmoney")]
        public decimal? PropertyTaxAmount { get; set; }

        [Column(TypeName = "smallmoney")]
        public decimal? InsuranceAmount { get; set; }

        public bool IsAccelerated { get; set; }

        [StringLength(50)]
        public string PaymentFrequency { get; set; }

        public virtual tblMortgage tblMortgage { get; set; }

        public virtual ICollection<tblMortgagePaymentRegistrationData> tblMortgagePaymentRegistrationDatas { get; set; }
    }
}
