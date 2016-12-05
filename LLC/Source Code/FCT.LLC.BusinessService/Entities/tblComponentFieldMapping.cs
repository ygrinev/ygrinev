using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblComponentFieldMapping")]
    public partial class tblComponentFieldMapping
    {
        [Key]
        public int ComponentFieldMappingID { get; set; }

        public int SourceComponentID { get; set; }

        [Required]
        public string SourceName { get; set; }

        public int TargetComponentID { get; set; }

        [Required]
        public string TargetName { get; set; }

        public virtual tblInternalComponent tblInternalComponent { get; set; }
        
    }
}
