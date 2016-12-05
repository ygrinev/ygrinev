using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblCalculationPeriodCode")]
    public partial class tblCalculationPeriodCode
    {
        public tblCalculationPeriodCode()
        {
            tblCalculationPeriodDescriptions = new HashSet<tblCalculationPeriodDescription>();
            tblMortgages = new HashSet<tblMortgage>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CalculationPeriodID { get; set; }

        [Required]
        [StringLength(50)]
        public string CalculationPeriodCode { get; set; }

        public int LenderID { get; set; }

        public virtual tblLender tblLender { get; set; }

        public virtual ICollection<tblCalculationPeriodDescription> tblCalculationPeriodDescriptions { get; set; }

        public virtual ICollection<tblMortgage> tblMortgages { get; set; }
    }
}
