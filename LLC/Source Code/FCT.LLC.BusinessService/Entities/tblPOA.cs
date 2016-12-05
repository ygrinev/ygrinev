using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPOA")]
    public partial class tblPOA
    {
        public tblPOA()
        {
            tblAttorneys = new HashSet<tblAttorney>();
        }

        [Key]
        public int POAID { get; set; }

        public int DealID { get; set; }

        public int? MortgagorID { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] LastModified { get; set; }

        public virtual ICollection<tblAttorney> tblAttorneys { get; set; }

        public virtual tblDeal tblDeal { get; set; }

        public virtual tblMortgagor tblMortgagor { get; set; }
    }
}
