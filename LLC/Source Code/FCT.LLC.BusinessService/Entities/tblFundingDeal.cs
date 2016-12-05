using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblFundingDeal")]
    public partial class tblFundingDeal
    {
        public tblFundingDeal()
        {
            tblDealFundsAllocations = new HashSet<tblDealFundsAllocation>();
        }

        [Key]
        public int FundingDealID { get; set; }

        public int DealScopeID { get; set; }

        public DateTime? InvitationSent { get; set; }

        public DateTime? InvitationAccepted { get; set; }

        public DateTime? SignedByVendor { get; set; }

        public DateTime? SignedByPurchaser { get; set; }

        public DateTime? Funded { get; set; }

        [ConcurrencyCheck]
        public DateTime? Disbursed { get; set; }

        public DateTime? PayoutSent { get; set; }

        [StringLength(100)]
        public string AssignedTo { get; set; }

        [StringLength(200)]
        public string SignedByPurchaserName { get; set; }

        [StringLength(200)]
        public string SignedByVendorName { get; set; }

        [StringLength(100)]
        public string OtherLawyerFirstName { get; set; }

        [StringLength(100)]
        public string OtherLawyerLastName { get; set; }

        [StringLength(50)]
        public string OtherLawyerFirmName { get; set; }


        public virtual ICollection<tblDealFundsAllocation> tblDealFundsAllocations { get; set; }

        public virtual tblDealScope DealScope { get; set; }
    }
}
