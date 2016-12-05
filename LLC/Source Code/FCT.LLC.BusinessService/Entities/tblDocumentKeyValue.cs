using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentKeyValue")]
    public partial class tblDocumentKeyValue
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string KeyID { get; set; }

        [Required]
        [StringLength(50)]
        public string KeyValue { get; set; }

        public int LanguageID { get; set; }

        public int? LenderID { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        public virtual tblLanguage tblLanguage { get; set; }

        public virtual tblLender tblLender { get; set; }
    }
}
