using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.EPS.DataEntities
{
    public class tblAgentNames
    {
        public tblAgentNames()
        {
            tblPaymentScheduleRunLog = new HashSet<tblPaymentScheduleRunLog>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int AgentID { get; set; }

        [StringLength(100)]
        public string AgentName { get; set; }

        public virtual ICollection<tblPaymentScheduleRunLog> tblPaymentScheduleRunLog { get; set; }

    }
}
