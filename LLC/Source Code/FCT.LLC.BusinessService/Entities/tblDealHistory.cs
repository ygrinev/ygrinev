namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;

    [DataContract]
    [Table("tblDealHistory")]
    public partial class tblDealHistory
    {
        [Key]
        [DataMember]
        public int DealHistoryID { get; set; }

        [DataMember]
        public int DealID { get; set; }

        [DataMember]
        [StringLength(4000)]
        public string Activity { get; set; }

        [DataMember]
        [StringLength(4000)]
        public string ActivityFrench { get; set; }

        [DataMember]
        public DateTime LogDate { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string UserID { get; set; }

        [DataMember]
        [Required]
        [StringLength(50)]
        public string UserType { get; set; }

        [DataMember]
        public bool IsShowOnLender { get; set; }
    }
}
