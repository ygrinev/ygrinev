using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblProvince")]
    public partial class tblProvince
    {
        public tblProvince()
        {
            tblAddresses = new HashSet<tblAddress>();
            tblBranches = new HashSet<tblBranch>();
            tblLenderChangeMortgagors = new HashSet<tblLenderChangeMortgagor>();
            tblMortgagors = new HashSet<tblMortgagor>();
            tblProperties = new HashSet<tblProperty>();
            tblProvinceTaxes = new HashSet<tblProvinceTax>();
            tblQuestionReferences = new HashSet<tblQuestionReference>();
            tblTitleInsuranceSelections = new HashSet<tblTitleInsuranceSelection>();
            tblDocumentTemplates = new HashSet<tblDocumentTemplate>();
        }

        [Key]
        [StringLength(2)]
        public string Province { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string FrenchName { get; set; }

        public int? YellowWarningStatusDays { get; set; }

        public int? RedWarningStatusDays { get; set; }

        public virtual ICollection<tblAddress> tblAddresses { get; set; }

        public virtual ICollection<tblBranch> tblBranches { get; set; }

        public virtual ICollection<tblLenderChangeMortgagor> tblLenderChangeMortgagors { get; set; }

        public virtual ICollection<tblMortgagor> tblMortgagors { get; set; }

        public virtual ICollection<tblProperty> tblProperties { get; set; }

        public virtual ICollection<tblProvinceTax> tblProvinceTaxes { get; set; }

        public virtual ICollection<tblQuestionReference> tblQuestionReferences { get; set; }

        public virtual ICollection<tblTitleInsuranceSelection> tblTitleInsuranceSelections { get; set; }

        public virtual ICollection<tblDocumentTemplate> tblDocumentTemplates { get; set; }
    }
}
