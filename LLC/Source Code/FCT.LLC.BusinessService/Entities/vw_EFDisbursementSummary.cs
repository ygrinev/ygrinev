namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vw_EFDisbursementSummary
    {
        public DateTime? SignedByVendor { get; set; }

        public DateTime? SignedByPurchaser { get; set; }

        public DateTime? Funded { get; set; }

        public DateTime? Disbursed { get; set; }

        public DateTime? InvitationSent { get; set; }

        public DateTime? InvitationAccepted { get; set; }

        public DateTime? PayoutSent { get; set; }

        [Column(TypeName = "money")]
        public decimal? DepositAmountReceived { get; set; }


        [Column(Order = 0, TypeName = "money")]
        public decimal? DepositAmountRequired { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FundingDealID { get; set; }


        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? DisbursementSummaryID { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        [StringLength(50)]
        public string WireDepositDetails { get; set; }
        
        [StringLength(4000)]
        public string PayoutComments { get; set; }

        [StringLength(200)]
        public string SignedByPurchaserName { get; set; }

        [StringLength(200)]
        public string SignedByVendorName { get; set; }
        
    }
}
