using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPublishedEventMessageBatchProcessing")]
    public partial class tblPublishedEventMessageBatchProcessing
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(11)]
        public string FCTRefNum { get; set; }

        [Required]
        [StringLength(100)]
        public string LenderDealRefNumber { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string FINumber { get; set; }

        public DateTime ProcessingStartTime { get; set; }

        public bool IsProcessing { get; set; }
    }
}
