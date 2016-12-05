using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblMortgagePaymentRegistrationData")]
    public partial class tblMortgagePaymentRegistrationData
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MortgagePaymentID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int MortgageRegistrationDataFieldID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LanguageID { get; set; }

        [Required]
        [StringLength(1000)]
        public string TextValue { get; set; }

        public virtual tblLanguage tblLanguage { get; set; }

        public virtual tblMortgagePayment tblMortgagePayment { get; set; }

        public virtual tblMortgageRegistrationDataField tblMortgageRegistrationDataField { get; set; }
    }
}
