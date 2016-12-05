using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLawyerApplication")]
    public partial class tblLawyerApplication
    {
        [Key]
        [StringLength(50)]
        public string LawyerApplication { get; set; }
    }
}
