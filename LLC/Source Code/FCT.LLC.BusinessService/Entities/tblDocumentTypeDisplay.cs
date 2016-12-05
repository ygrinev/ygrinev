using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentTypeDisplay")]
    public partial class tblDocumentTypeDisplay
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LanguageID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocumentTypeID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocumentTypeDisplayTypeID { get; set; }

        [Required]
        [StringLength(200)]
        public string Value { get; set; }

        public virtual tblDocumentType tblDocumentType { get; set; }

        public virtual tblDocumentTypeDisplayType tblDocumentTypeDisplayType { get; set; }

        public virtual tblLanguage tblLanguage { get; set; }
    }
}
