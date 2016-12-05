using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblRegisteredOwner")]
    public partial class tblRegisteredOwner
    {
        [Key]
        public int RegisteredOwnerID { get; set; }

        public int PropertyID { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string CompanyName { get; set; }

        public virtual tblProperty tblProperty { get; set; }
    }
}
