using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("ChangeLog")]
    public partial class ChangeLog
    {
        [Key]
        [Column(Order = 0)]
        public int LogId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(256)]
        public string DatabaseName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string EventType { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(256)]
        public string ObjectName { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(25)]
        public string ObjectType { get; set; }

        [Key]
        [Column(Order = 5)]
        public string SqlCommand { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime EventDate { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(256)]
        public string LoginName { get; set; }
    }
}
