using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    [Table("tblProperty")]
    public class LLCProperty: AuditEntityBase
    {
        [Key]
        public int PropertyID { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        [Column("DealID")]
        public int DealID { get; set; }
    }
}
