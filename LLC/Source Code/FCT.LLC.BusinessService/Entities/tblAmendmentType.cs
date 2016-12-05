using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblAmendmentType")]
    public partial class tblAmendmentType
    {
        public tblAmendmentType()
        {
            tblAmendmentTracks = new HashSet<tblAmendmentTrack>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AmendmentTypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string AmendmentType { get; set; }

        public virtual ICollection<tblAmendmentTrack> tblAmendmentTracks { get; set; }
    }
}
