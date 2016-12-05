using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblTitleInsurancePolicy")]
    public partial class tblTitleInsurancePolicy
    {
        [Key]
        public int TitleInsurancePolicyID { get; set; }

        public int? DealID { get; set; }

        [StringLength(50)]
        public string PolicyType { get; set; }

        [StringLength(50)]
        public string PolicyNumber { get; set; }

        [StringLength(50)]
        public string InsuranceCompany { get; set; }

        [Column(TypeName = "money")]
        public decimal? InsuredAmount { get; set; }

        public DateTime? PolicyDate { get; set; }

        [StringLength(200)]
        public string NameOfInsured { get; set; }

        public string ScheduleBExceptions { get; set; }

        public virtual tblDeal tblDeal { get; set; }
    }
}
