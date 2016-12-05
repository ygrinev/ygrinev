using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblFundsDeliveryType")]
    public partial class tblFundsDeliveryType
    {
        public tblFundsDeliveryType()
        {
            tblFundingRequests = new HashSet<tblFundingRequest>();
            tblLenders = new HashSet<tblLender>();
        }

        [Key]
        public int FundsDeliveryTypeID { get; set; }

        [Required]
        [StringLength(25)]
        public string FundsDeliveryTypeCode { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public virtual ICollection<tblFundingRequest> tblFundingRequests { get; set; }

        public virtual ICollection<tblLender> tblLenders { get; set; }
    }
}
