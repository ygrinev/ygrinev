using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblCMHC")]
    public partial class tblCMHC
    {
        [Key]
        public int CMHCID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
