using System.ComponentModel.DataAnnotations;

namespace FCT.LLC.BusinessService.Entities
{
    public partial class tblFundStatu
    {
        [Key]
        public int FundStatusID { get; set; }

        [StringLength(20)]
        public string FundStatus { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
