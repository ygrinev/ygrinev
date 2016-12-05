using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPaymentNotification")]
    public class tblPaymentNotification
    {
        [Key]
        public int PaymentNotificationID { get; set; }

        public int PaymentRequestID { get; set; }

        [StringLength(20)]
        public string NotificationType { get; set; }

        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        public DateTime? PaymentDate { get; set; }

        [Column(TypeName = "money")]
        public decimal PaymentAmount { get; set; }

        [StringLength(20)]
        public string PaymentStatus { get; set; }

        [StringLength(16)]
        public string BatchID { get; set; }

        [StringLength(100)]
        public string BatchDescription { get; set; }

        public DateTime NotificationTimeStamp { get; set; }

        public virtual tblPaymentRequest tblPaymentRequest { get; set; }

    }
}
