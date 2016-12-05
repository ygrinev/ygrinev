using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblRejectLenderAmendment")]
    public partial class tblRejectLenderAmendment
    {
        [Key]
        public int RejectLenderAmendmentID { get; set; }

        public int DealID { get; set; }

        public int? LenderEntityID { get; set; }

        [Required]
        [StringLength(500)]
        public string ComponentFieldName { get; set; }

        public int InternalComponentID { get; set; }

        public DateTime RejectionTimeStamp { get; set; }

        public string RejectedValue { get; set; }

        public virtual tblBusinessFieldMap tblBusinessFieldMap { get; set; }

        public virtual tblDeal tblDeal { get; set; }
    }
}
