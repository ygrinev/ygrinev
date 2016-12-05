using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblFundingRequest")]
    public partial class tblFundingRequest
    {
        [Key]
        public int FundingRequestID { get; set; }

        public int DealID { get; set; }

        public int FundingRequestTypeID { get; set; }

        [Column(TypeName = "money")]
        public decimal? RequestedAmount { get; set; }

        public int? FundsDeliveryTypeID { get; set; }

        [StringLength(10)]
        public string BranchNumber { get; set; }

        public DateTime? ILASignedDate { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        public bool? ReturnMortgageProceeds { get; set; }

        public bool ChangeNotification { get; set; }

        public bool NotifyClosingDateChanged { get; set; }

        public virtual tblDeal tblDeal { get; set; }

        public virtual tblFundingRequestType tblFundingRequestType { get; set; }

        public virtual tblFundsDeliveryType tblFundsDeliveryType { get; set; }
    }
}
