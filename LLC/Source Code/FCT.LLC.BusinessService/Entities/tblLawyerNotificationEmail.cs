using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLawyerNotificationEmail")]
    public partial class tblLawyerNotificationEmail
    {
        [Key]
        public int NotificationEmailID { get; set; }

        public int LawyerNotificationID { get; set; }

        [Required]
        [StringLength(200)]
        public string Email { get; set; }

        public virtual tblLawyerNotification tblLawyerNotification { get; set; }
    }
}
