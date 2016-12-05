using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderChangePIN")]
    public partial class tblLenderChangePIN
    {
        [Key]
        public int PINID { get; set; }

        public int LenderChangePropertyID { get; set; }

        [Required]
        [StringLength(50)]
        public string PINNumber { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }
    }
}
