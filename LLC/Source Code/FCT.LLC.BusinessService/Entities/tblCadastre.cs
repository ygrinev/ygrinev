using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblCadastre")]
    public partial class tblCadastre
    {
        public tblCadastre()
        {
            tblSecondaryDesignations = new HashSet<tblSecondaryDesignation>();
        }

        [Key]
        public int CadastreID { get; set; }

        public int RegistryOfficeID { get; set; }

        [Required]
        [StringLength(100)]
        public string Cadastre { get; set; }

        public virtual tblRegistryOffice tblRegistryOffice { get; set; }

        public virtual ICollection<tblSecondaryDesignation> tblSecondaryDesignations { get; set; }
    }
}
