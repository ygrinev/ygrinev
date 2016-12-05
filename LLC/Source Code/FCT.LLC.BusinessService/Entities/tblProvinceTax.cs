using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblProvinceTax")]
    public partial class tblProvinceTax
    {
        [Key]
        public int ProvinceTaxID { get; set; }

        [Required]
        [StringLength(2)]
        public string Province { get; set; }

        public decimal? GSTRate { get; set; }

        public decimal? HSTRate { get; set; }

        public decimal? QSTRate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime EffectiveDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? TerminationDate { get; set; }

        public virtual tblProvince tblProvince { get; set; }
    }
}
