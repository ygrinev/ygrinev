namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public partial class tblPaymentStatus
    {
        public tblPaymentStatus()
        {
            tblPaymentNotification = new HashSet<tblPaymentNotification>();
        }

        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string NotificationType { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PaymentStatusCode { get; set; }

        [Required]
        [StringLength(20)]
        public string StatusDescription { get; set; }

        public virtual ICollection<tblPaymentNotification> tblPaymentNotification { get; set; }
    }
}
