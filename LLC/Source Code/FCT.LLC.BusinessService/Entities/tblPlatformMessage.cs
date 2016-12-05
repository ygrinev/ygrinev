using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPlatformMessage")]
    public partial class tblPlatformMessage
    {
        [Key]
        public int PlatformMessageID { get; set; }

        public bool IsNewPlatformMessage { get; set; }

        public DateTime DateTime { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Body { get; set; }

        [Required]
        [StringLength(50)]
        public string Source { get; set; }
    }
}
