using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentProvConfig")]
    public partial class tblDocumentProvConfig
    {
        [Key]
        [StringLength(2)]
        public string Province { get; set; }

        public bool IsArchivable { get; set; }
    }
}
