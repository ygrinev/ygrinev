using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblHoliday")]
    public partial class tblHoliday
    {
        [Key]
        public DateTime Holiday { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
