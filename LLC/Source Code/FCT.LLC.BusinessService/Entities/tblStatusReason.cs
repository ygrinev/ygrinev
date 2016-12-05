using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblStatusReason")]
    public class tblStatusReason
    {
        [Key]
        public int StatusReasonID { get; set; }

        public string Status { get; set; }

        public string Reason { get; set; }

        public int ReasonID { get; set; }
    }
}
