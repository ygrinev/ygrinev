using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblEmailTemplateList")]
    public partial class tblEmailTemplateList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TemplateID { get; set; }

        public int? StandardNotificationID { get; set; }

        public int LanguageID { get; set; }

        [Required]
        public string SubjectTemplate { get; set; }

        [Required]
        public string BodyTemplate { get; set; }

        public virtual tblLanguage tblLanguage { get; set; }

        public virtual tblStandardNotificationList tblStandardNotificationList { get; set; }
    }
}
