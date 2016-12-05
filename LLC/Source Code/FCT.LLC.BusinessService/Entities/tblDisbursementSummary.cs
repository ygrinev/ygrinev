namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblDisbursementSummary")]
    public partial class tblDisbursementSummary
    {
        [Key]
        public int DisbursementSummaryID { get; set; }

        public int FundingDealID { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }

        [Column(TypeName = "money")]
        public decimal DepositAmountRequired { get; set; }
        [MaxLength(4000)]
        public string Comments { get; set; }

        public virtual tblFundingDeal tblFundingDeal { get; set; }
    }
}
