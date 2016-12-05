using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblAmendmentTrack")]
    public partial class tblAmendmentTrack
    {
        [Key]
        public int AmendmentTrackID { get; set; }

        public int AmendmentTypeID { get; set; }

        public int DealID { get; set; }

        public DateTime LastModification { get; set; }

        public DateTime? LastAcknowledgement { get; set; }

        public virtual tblAmendmentType tblAmendmentType { get; set; }
    }
}
