using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblMortgageRegistrationDataField")]
    public partial class tblMortgageRegistrationDataField
    {
        public tblMortgageRegistrationDataField()
        {
            tblMortgagePaymentRegistrationDatas = new HashSet<tblMortgagePaymentRegistrationData>();
            tblMortgageRegistrationDatas = new HashSet<tblMortgageRegistrationData>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FieldID { get; set; }

        [Required]
        [StringLength(100)]
        public string FieldName { get; set; }

        [Required]
        [StringLength(50)]
        public string EntityRelateTo { get; set; }

        public virtual ICollection<tblMortgagePaymentRegistrationData> tblMortgagePaymentRegistrationDatas { get; set; }

        public virtual ICollection<tblMortgageRegistrationData> tblMortgageRegistrationDatas { get; set; }
    }
}
