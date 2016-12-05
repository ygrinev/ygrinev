using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblAuditLog")]
    public partial class tblAuditLog
    {
        [Key]
        public int AuditLogID { get; set; }

        [Required]
        [StringLength(1000)]
        public string Activity { get; set; }

        public DateTime LogDate { get; set; }

        [Required]
        [StringLength(50)]
        public string UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserType { get; set; }

        [StringLength(50)]
        public string UserIPAddress { get; set; }
    }
}
