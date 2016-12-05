using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblCompany")]
    public partial class tblCompany
    {
        public tblCompany()
        {
            tblGuarantors = new HashSet<tblGuarantor>();
            tblMortgageILAs = new HashSet<tblMortgageILA>();
        }

        [Key]
        public int CompanyID { get; set; }

        [StringLength(200)]
        public string CompanyName { get; set; }

        public int? AddressID { get; set; }

        public int? PrimaryContactID { get; set; }

        public int? ContactPersonID { get; set; }

        public virtual tblAddress tblAddress { get; set; }

        public virtual tblContactInfo tblContactInfo { get; set; }

        public virtual tblPerson tblPerson { get; set; }

        public virtual ICollection<tblGuarantor> tblGuarantors { get; set; }

        public virtual ICollection<tblMortgageILA> tblMortgageILAs { get; set; }
    }
}
