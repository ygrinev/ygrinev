namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPaymentScheduleRunLog")]
    public partial class tblPaymentScheduleRunLog
    {
        [Key, Column(Order = 1)]
        public int AgentID {get; set;}
        [Key, Column(Order = 2)]
        public DateTime RunTime {get; set;}
        public DateTime Created {get; set;}

        public virtual tblAgentNames tblAgentNames { get; set; }
    }
}
