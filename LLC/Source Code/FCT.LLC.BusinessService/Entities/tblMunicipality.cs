using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblMunicipality")]
    public partial class tblMunicipality
    {
        [Key]
        public int MunicipalityID { get; set; }

        [Required]
        [StringLength(100)]
        public string Municipality { get; set; }

        [StringLength(2)]
        public string ProvinceCode { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        public int? LRONumber { get; set; }
    }
}
