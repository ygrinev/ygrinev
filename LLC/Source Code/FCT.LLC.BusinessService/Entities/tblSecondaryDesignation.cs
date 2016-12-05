using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblSecondaryDesignation")]
    public partial class tblSecondaryDesignation
    {
        [Key]
        public int SecondaryDesignationID { get; set; }

        public int CadastreID { get; set; }

        [Required]
        [StringLength(100)]
        public string SecondaryDesignation { get; set; }

        public virtual tblCadastre tblCadastre { get; set; }
    }
}
