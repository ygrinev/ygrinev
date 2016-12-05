using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDealFundsAllocation")]
    public partial class tblDealFundsAllocation
    {
        [Key]
        public int DealFundsAllocationID { get; set; }

        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        public DateTime DepositDate { get; set; }

        [StringLength(3)]
        public string BankNumber { get; set; }

        [StringLength(5)]
        public string BranchNumber { get; set; }

        [StringLength(20)]
        public string AccountNumber { get; set; }

        [StringLength(400)]
        public string WireDepositDetails { get; set; }

        public DateTime NotificationTimeStamp { get; set; }

        [StringLength(11)]
        public string ShortFCTRefNumber { get; set; }

        public int? FundingDealID { get; set; }

        public int? LawyerID { get; set; }

        [Required]
        [StringLength(50)]
        public string AllocationStatus { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }


        [StringLength(10)]
        public string RecordType { get; set; }

        public int? TrustAccountID { get; set; }

        public int? FeeID { get; set; }

        public DateTime? Reconciled { get; set; }

        [StringLength(100)]
        public string ReconciledBy { get; set; }

        [StringLength(50)]
        public string PaymentOriginatorName { get; set; }

        public virtual tblFundingDeal tblFundingDeal { get; set; }

        public virtual tblFee Fee { get; set; }
    }
}
