using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblStandardNotification")]
    public partial class tblStandardNotification
    {
        public tblStandardNotification()
        {
            tblLawyerNotifications = new HashSet<tblLawyerNotification>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int StandardNotificationID { get; set; }

        [StringLength(100)]
        public string Message { get; set; }

        [StringLength(50)]
        public string GlobalizationKey { get; set; }

        [StringLength(100)]
        public string TriggerTiming { get; set; }

        [StringLength(50)]
        public string Method { get; set; }

        [StringLength(50)]
        public string Destination { get; set; }

        [StringLength(100)]
        public string SubjectLine { get; set; }

        [StringLength(1000)]
        public string EmailContent { get; set; }

        [StringLength(255)]
        public string Salutation { get; set; }

        [StringLength(500)]
        public string Criteria { get; set; }

        [StringLength(50)]
        public string AdditionalInfoField { get; set; }

        [StringLength(50)]
        public string SQLTrigger { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ScheduledTime { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? LastBatchSent { get; set; }

        public bool? Active { get; set; }

        public bool? Configurable { get; set; }

        public int? EmailTemplateId { get; set; }

        public virtual tblEmailTemplate tblEmailTemplate { get; set; }

        public virtual ICollection<tblLawyerNotification> tblLawyerNotifications { get; set; }
    }
}
