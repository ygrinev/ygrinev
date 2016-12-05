using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLawyerNotification")]
    public partial class tblLawyerNotification
    {
        public tblLawyerNotification()
        {
            tblLawyerNotificationEmails = new HashSet<tblLawyerNotificationEmail>();
        }

        [Key]
        public int LawyerNotificationID { get; set; }

        public int LawyerID { get; set; }

        public int StandardNotificationID { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        public virtual tblLawyer tblLawyer { get; set; }

        public virtual tblStandardNotification tblStandardNotification { get; set; }

        public virtual ICollection<tblLawyerNotificationEmail> tblLawyerNotificationEmails { get; set; }
    }
}
