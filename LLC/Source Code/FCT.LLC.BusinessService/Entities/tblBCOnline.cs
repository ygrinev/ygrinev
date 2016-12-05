using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblBCOnline")]
    public partial class tblBCOnline
    {
        [Key]
        public int BCOnlineID { get; set; }

        public int? LawyerID { get; set; }

        [Required]
        [StringLength(50)]
        public string UserID { get; set; }

        [Required]
        [StringLength(25)]
        public string UserFolio { get; set; }

        [Required]
        [StringLength(100)]
        public string UserIP { get; set; }
    }
}
