using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblExistingMortgage")]
    public partial class tblExistingMortgage
    {
        [Key]
        public int ExistingMortgageID { get; set; }

        public int PropertyID { get; set; }

        [StringLength(100)]
        public string EncumbranceType { get; set; }

        [StringLength(200)]
        public string MortgageeName { get; set; }

        [StringLength(50)]
        public string MortgageAction { get; set; }

        [Column(TypeName = "money")]
        public decimal? MortgageAmount { get; set; }

        [StringLength(30)]
        public string MortgageNumber { get; set; }

        [StringLength(14)]
        public string RegistrationNumber { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? RegistrationDate { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        public virtual tblProperty tblProperty { get; set; }
    }
}
