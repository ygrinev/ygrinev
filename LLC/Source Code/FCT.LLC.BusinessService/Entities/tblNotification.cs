using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblNotification")]
    public partial class tblNotification
    {
        [Key]
        public int NotificationID { get; set; }

        public int DealID { get; set; }

        public int? StandardNotificationID { get; set; }

        [StringLength(1000)]
        public string AdditionalInfo { get; set; }

        public int? RecipientID { get; set; }

        [StringLength(50)]
        public string RecipientType { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? NotifiedDate { get; set; }
    }
}
