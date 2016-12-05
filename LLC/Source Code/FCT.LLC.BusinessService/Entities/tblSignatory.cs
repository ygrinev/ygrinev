using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblSignatory")]
    public partial class tblSignatory
    {
        [Key]
        public int SignatoryID { get; set; }

        public int? MortgagorID { get; set; }

        public int? GuarantorID { get; set; }

        public int PersonID { get; set; }

        public virtual tblGuarantor tblGuarantor { get; set; }

        public virtual tblMortgagor tblMortgagor { get; set; }

        public virtual tblPerson tblPerson { get; set; }
    }
}
