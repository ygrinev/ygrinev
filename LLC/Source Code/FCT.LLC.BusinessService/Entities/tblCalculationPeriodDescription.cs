using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblCalculationPeriodDescription")]
    public partial class tblCalculationPeriodDescription
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CalculationPeriodID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LanguageID { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        public virtual tblCalculationPeriodCode tblCalculationPeriodCode { get; set; }

        public virtual tblLanguage tblLanguage { get; set; }
    }
}
