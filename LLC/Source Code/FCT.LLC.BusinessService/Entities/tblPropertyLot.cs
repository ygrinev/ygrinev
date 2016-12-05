using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPropertyLot")]
    public partial class tblPropertyLot
    {
        [Key]
        public int PropertyLotID { get; set; }

        public int PropertyID { get; set; }

        public bool IsOriginalLot { get; set; }

        [StringLength(50)]
        public string LotNumber { get; set; }

        [StringLength(100)]
        public string SubDivision1 { get; set; }

        [StringLength(100)]
        public string SubDivision2 { get; set; }

        [StringLength(100)]
        public string SubDivision3 { get; set; }

        [StringLength(100)]
        public string SubDivision4 { get; set; }

        public bool? IsPartOfLot { get; set; }

        [Column(TypeName = "text")]
        public string PartOfLotDescription { get; set; }

        [StringLength(100)]
        public string SecondaryDesignation { get; set; }

        [StringLength(100)]
        public string RegistryOffice { get; set; }

        [StringLength(100)]
        public string Cadastre { get; set; }

        [StringLength(5000)]
        public string Servitude { get; set; }

        public virtual tblProperty tblProperty { get; set; }
    }
}
