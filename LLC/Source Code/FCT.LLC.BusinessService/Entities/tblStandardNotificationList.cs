using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblStandardNotificationList")]
    public partial class tblStandardNotificationList
    {
        public tblStandardNotificationList()
        {
            tblEmailTemplateLists = new HashSet<tblEmailTemplateList>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StandardNotificationID { get; set; }

        [Required]
        [StringLength(100)]
        public string StandardNotificationName { get; set; }

        [StringLength(50)]
        public string GlobalizationKey { get; set; }

        [Required]
        [StringLength(50)]
        public string Recipient { get; set; }

        public bool IsHighPriority { get; set; }

        public bool Active { get; set; }

        public bool IsOptional { get; set; }

        public virtual ICollection<tblEmailTemplateList> tblEmailTemplateLists { get; set; }
    }
}
