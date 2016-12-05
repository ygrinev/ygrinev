using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblEmailTemplate")]
    public partial class tblEmailTemplate
    {
        public tblEmailTemplate()
        {
            tblStandardNotifications = new HashSet<tblStandardNotification>();
        }

        [Key]
        public int TemplateID { get; set; }

        [Required]
        [StringLength(50)]
        public string TemplateKey { get; set; }

        public int? LanguageID { get; set; }

        public string SubjectTemplate { get; set; }

        public string BodyTemplate { get; set; }

        public virtual ICollection<tblStandardNotification> tblStandardNotifications { get; set; }
    }
}
