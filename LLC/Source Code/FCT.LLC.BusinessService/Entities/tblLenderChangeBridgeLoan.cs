using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderChangeBridgeLoan")]
    public partial class tblLenderChangeBridgeLoan
    {
        [Key]
        public int LenderChangeBridgeLoanID { get; set; }

        public int MortgageID { get; set; }

        public DateTime? MaturityDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? LoanAmount { get; set; }

        public DateTime? ApplicationDate { get; set; }

        public decimal? InterestRate { get; set; }

        public DateTime? FirstPaymentDate { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(50)]
        public string Province { get; set; }

        [StringLength(50)]
        public string PostalCode { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        [StringLength(25)]
        public string UnitNumber { get; set; }

        [StringLength(25)]
        public string StreetNumber { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        public DateTime? InterestAdjustmentDate { get; set; }

        public int? LenderChangeID { get; set; }

        public virtual tblMortgage tblMortgage { get; set; }
    }
}
