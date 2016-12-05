using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPaymentFrequencyType")]
    public partial class tblPaymentFrequencyType
    {
        public tblPaymentFrequencyType()
        {
            //tblMortgages = new HashSet<tblMortgage>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PaymentFrequencyTypeID { get; set; }

        [Required]
        [StringLength(20)]
        public string Description { get; set; }

        //public virtual ICollection<tblMortgage> tblMortgages { get; set; }
    }
}
