using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblCompanyIncorporationAuthority")]
    public partial class tblCompanyIncorporationAuthority
    {
        public tblCompanyIncorporationAuthority()
        {
            //tblMortgagors = new HashSet<tblMortgagor>();
        }

        [Key]
        [StringLength(2)]
        public string CompanyIncorporationAuthorityID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string FrenchName { get; set; }

        public int PresentationOrder { get; set; }

        //public virtual ICollection<tblMortgagor> tblMortgagors { get; set; }
    }
}
