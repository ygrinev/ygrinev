using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblSolicitorSyncMarkedProfile")]
    public partial class tblSolicitorSyncMarkedProfile
    {
        public int ID { get; set; }

        public int LawyerID { get; set; }

        public int LenderID { get; set; }

        public bool IsNewOperation { get; set; }

        public DateTime OperationDateTime { get; set; }

        public Guid? DeliveredInMessageID { get; set; }

        public bool IsProcessing { get; set; }

        public DateTime? EventDeliveryTimestamp { get; set; }

        public virtual tblLawyer tblLawyer { get; set; }

        public virtual tblLender tblLender { get; set; }
    }
}
