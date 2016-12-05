namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblBatchSchedule")]
    public partial class tblBatchSchedule
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int BatchScheduleID { get; set; }

        public int PayeeInfoID { get; set; }

        public DateTime? ScheduledTime { get; set; }

        public virtual tblPayeeInfo tblPayeeInfo { get; set; }
    }
}
