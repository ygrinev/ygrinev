using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblFundingPaymentMethod")]
    public partial class tblFundingPaymentMethod
    {
        public tblFundingPaymentMethod()
        {
            //tblDeals = new HashSet<tblDeal>();
            tblMortgageFees = new HashSet<tblMortgageFee>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PaymentMethodID { get; set; }

        [StringLength(25)]
        public string PaymentMethod { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        //public virtual ICollection<tblDeal> tblDeals { get; set; }

        public virtual ICollection<tblMortgageFee> tblMortgageFees { get; set; }
    }
}
