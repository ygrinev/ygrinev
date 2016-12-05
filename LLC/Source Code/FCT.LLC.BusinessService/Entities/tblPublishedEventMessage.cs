using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPublishedEventMessage")]
    public partial class tblPublishedEventMessage
    {
        public int ID { get; set; }

        public int LenderID { get; set; }

        public int DealID { get; set; }

        public DateTime EventTimeStamp { get; set; }

        [Column(TypeName = "xml")]
        [Required]
        public string EventMessage { get; set; }

        [StringLength(50)]
        public string EventType { get; set; }

        public Guid? DeliveredInMessageID { get; set; }

        public bool IsProcessing { get; set; }

        public DateTime? EventDeliveryTimestamp { get; set; }

        public virtual tblDeal tblDeal { get; set; }

        public virtual tblLender tblLender { get; set; }
    }
}
