namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class vw_PayoutLetterWorklist
    {
        public vw_PayoutLetterWorklist()
        {
        }


        public int DealID { get; set; }

        [Key]
        public int FundingDealID { get; set; }

        [StringLength(100)]
        public string ChequeBatchDescription { get; set; }

        [StringLength(16)]
        public string ChequeBatchNumber{ get; set; }

        public string AssignedTo { get; set; }

        public DateTime? DisbursementDate { get; set; }

        [StringLength(11)]
        public string FCTURN { get; set; }

        public int NumberOfCheques { get; set; }
    }
}
