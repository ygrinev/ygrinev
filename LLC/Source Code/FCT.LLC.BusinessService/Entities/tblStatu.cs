using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FCT.LLC.BusinessService.Entities
{
    public partial class tblStatu
    {
        public tblStatu()
        {
            tblStatusReasons = new HashSet<tblStatusReason>();
        }

        [Key]
        [StringLength(50)]
        public string Status { get; set; }

        public int? Code { get; set; }

        [StringLength(250)]
        public string ShortDesc { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public virtual ICollection<tblStatusReason> tblStatusReasons { get; set; }
    }
}
