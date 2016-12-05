namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDisbursement")]
    public partial class tblDisbursement
    {
        [Key]
        public int DisbursementID { get; set; }

        public int FundingDealID { get; set; }

        public int? PayeeID { get; set; }

        [StringLength(50)]
        public string PayeeType { get; set; }

        [StringLength(100)]
        public string PayeeName { get; set; }

        [StringLength(400)]
        public string PayeeComments { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [StringLength(30)]
        public string PaymentMethod { get; set; }

        [StringLength(40)]
        public string NameOnCheque { get; set; }

        [StringLength(10)]
        public string UnitNumber { get; set; }

        [StringLength(10)]
        public string StreetNumber { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(2)]
        public string Province { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        [StringLength(50)]
        public string AssessmentRollNumber { get; set; }

        public int? TrustAccountID { get; set; }

        [StringLength(3)]
        public string BankNumber { get; set; }

        [StringLength(5)]
        public string BranchNumber { get; set; }

        [StringLength(20)]
        public string AccountNumber { get; set; }

        [StringLength(100)]
        public string Instructions { get; set; }

        [StringLength(100)]
        public string AgentFirstName { get; set; }

        [StringLength(50)]
        public string AgentLastName { get; set; }

        [StringLength(50)]
        public string AccountAction { get; set; }

        [StringLength(20)]
        public string FCTFeeSplit { get; set; }

        [StringLength(100)]
        public string DisbursementComment { get; set; }

        [Column(TypeName = "money")]
        public decimal? DisbursedAmount { get; set; }

        [StringLength(20)]
        public string DisbursementStatus { get; set; }

        public int? VendorFeeID { get; set; }

        public int? PurchaserFeeID { get; set; }

        [StringLength(100)]
        public string StreetAddress1 { get; set; }

        [StringLength(100)]
        public string StreetAddress2 { get; set; }

        public int? ChainDealID { get; set; }

        public virtual tblFundingDeal tblFundingDeal { get; set; }

        [ForeignKey("VendorFeeID")]
        public virtual tblFee VendorFee { get; set; }

        [ForeignKey("PurchaserFeeID")]
        public virtual tblFee PurchaserFee { get; set; }

        public DateTime? Reconciled { get; set; }

        [StringLength(100)]
        public string ReconciledBy { get; set; }

        [StringLength(50)]
        public string PaymentReferenceNumber { get; set; }

        [StringLength(200)]
        public string AccountHolderName { get; set; }

        [StringLength(100)]
        public string Token { get; set; }
    }
}
