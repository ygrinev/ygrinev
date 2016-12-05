namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblMilestone")]
    public partial class tblMilestone
    {
        [Key]
        public int MilestoneID { get; set; }

        public int DealID { get; set; }

        public int MilestoneCodeID { get; set; }

        public DateTime? CompletedDateTime { get; set; }

        public virtual tblMilestoneCode tblMilestoneCode { get; set; }
    }
}
