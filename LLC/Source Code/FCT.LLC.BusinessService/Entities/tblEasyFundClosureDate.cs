using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblEasyFundClosureDate")]
    public partial class tblEasyFundClosureDate
    {
        [Key]
        public int EasyFundClosureDateID { get; set; }

        [Required]
        [StringLength(20)]
        public string Category { get; set; }

        public DateTime ClosureDate { get; set; }

        [StringLength(10)]
        public string Description { get; set; }
    }
}
