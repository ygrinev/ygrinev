using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblBusinessField")]
    public partial class tblBusinessField
    {
        public tblBusinessField()
        {
            tblBusinessFieldMaps = new HashSet<tblBusinessFieldMap>();
            tblLenderBusinessFields = new HashSet<tblLenderBusinessField>();
        }

        [Key]
        public int BusinessFieldID { get; set; }

        [Required]
        [StringLength(500)]
        public string BusinessFieldName { get; set; }

        [StringLength(10)]
        public string SubmitType { get; set; }

        public virtual ICollection<tblBusinessFieldMap> tblBusinessFieldMaps { get; set; }

        public virtual ICollection<tblLenderBusinessField> tblLenderBusinessFields { get; set; }
    }
}
