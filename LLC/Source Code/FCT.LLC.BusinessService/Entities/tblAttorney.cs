using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblAttorney")]
    public partial class tblAttorney
    {
        [Key]
        public int AttorneyID { get; set; }

        public int POAID { get; set; }

        public int PersonID { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] LastModified { get; set; }

        public virtual tblPerson tblPerson { get; set; }

        public virtual tblPOA tblPOA { get; set; }
    }
}
