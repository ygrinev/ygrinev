using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblInternalComponent")]
    public partial class tblInternalComponent
    {
        public tblInternalComponent()
        {
            tblBusinessFieldMaps = new HashSet<tblBusinessFieldMap>();
            tblComponentFieldMappings = new HashSet<tblComponentFieldMapping>();
        }

        [Key]
        public int InternalComponentID { get; set; }

        [Required]
        [StringLength(50)]
        public string ComponentName { get; set; }

        public virtual ICollection<tblBusinessFieldMap> tblBusinessFieldMaps { get; set; }

        public virtual ICollection<tblComponentFieldMapping> tblComponentFieldMappings { get; set; }
    }
}
