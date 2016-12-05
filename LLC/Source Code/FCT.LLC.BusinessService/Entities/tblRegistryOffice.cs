using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblRegistryOffice")]
    public partial class tblRegistryOffice
    {
        public tblRegistryOffice()
        {
            tblCadastres = new HashSet<tblCadastre>();
        }

        [Key]
        public int RegistryOfficeID { get; set; }

        [Required]
        [StringLength(100)]
        public string RegistryOffice { get; set; }

        public virtual ICollection<tblCadastre> tblCadastres { get; set; }
    }
}
