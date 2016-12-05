using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblMortgageILA")]
    public partial class tblMortgageILA
    {
        [Key]
        public int MortgageILAID { get; set; }

        public int MortgageID { get; set; }

        public int? CompanyID { get; set; }

        public int? PersonID { get; set; }

        public virtual tblCompany tblCompany { get; set; }

        public virtual tblMortgage tblMortgage { get; set; }

        public virtual tblPerson tblPerson { get; set; }
    }
}
