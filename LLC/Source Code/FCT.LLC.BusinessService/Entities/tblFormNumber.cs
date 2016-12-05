using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblFormNumber")]
    public partial class tblFormNumber
    {
        public int ID { get; set; }

        [StringLength(2)]
        public string ProvinceCode { get; set; }

        [StringLength(50)]
        public string FormNumber { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }
    }
}
