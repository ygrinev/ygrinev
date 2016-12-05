using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblFinancialInstitutionNumber")]
    public partial class tblFinancialInstitutionNumber
    {
        public tblFinancialInstitutionNumber()
        {
            //this.tblMortgages = new HashSet<tblMortgage>();
        }

        [Key]
        public int FINumberID { get; set; }
        public string FINumber { get; set; }
        public string FINameEnglish { get; set; }
        public string FINameFrench { get; set; }
        public string FICode { get; set; }
        public Nullable<int> LenderID { get; set; }

        public virtual tblLender tblLender { get; set; }
        //public virtual ICollection<tblMortgage> tblMortgages { get; set; }
    }
}
