using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLawyerSavedAmendment")]
    public partial class tblLawyerSavedAmendment
    {
        [Key]
        public int LawyerSavedAmendmentID { get; set; }

        public int DealID { get; set; }

        public int? LenderEntityID { get; set; }

        [Required]
        [StringLength(500)]
        public string ComponentFieldName { get; set; }

        public int InternalComponentID { get; set; }

        public DateTime AmendmentTimeStamp { get; set; }

        public string OriginalValue { get; set; }

        public string UpdatedValue { get; set; }

        [StringLength(10)]
        public string SubmitType { get; set; }

        public virtual tblBusinessFieldMap tblBusinessFieldMap { get; set; }

        public virtual tblDeal tblDeal { get; set; }
    }
}
