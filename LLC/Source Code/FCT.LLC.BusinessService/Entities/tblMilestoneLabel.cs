namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMilestoneLabel")]
    public partial class tblMilestoneLabel
    {
        [Key]
        public int MilestoneLabelID { get; set; }

        public int MilestoneCodeID { get; set; }

        public int LenderID { get; set; }

        public int? SequenceNumber { get; set; }

        [StringLength(100)]
        public string LabelEnglish { get; set; }

        [StringLength(100)]
        public string LabelFrench { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        public virtual tblLender tblLender { get; set; }

        public virtual tblMilestoneCode tblMilestoneCode { get; set; }
    }
}
