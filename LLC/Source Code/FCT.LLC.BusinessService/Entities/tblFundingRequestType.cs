using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblFundingRequestType")]
    public partial class tblFundingRequestType
    {
        public tblFundingRequestType()
        {
            tblFundingRequests = new HashSet<tblFundingRequest>();
        }

        [Key]
        public int FundingRequestTypeID { get; set; }

        [Required]
        [StringLength(100)]
        public string FundingRequestTypeCode { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public virtual ICollection<tblFundingRequest> tblFundingRequests { get; set; }
    }
}
