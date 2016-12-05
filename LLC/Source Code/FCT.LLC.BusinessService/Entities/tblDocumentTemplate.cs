using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentTemplate")]
    public partial class tblDocumentTemplate
    {
        public tblDocumentTemplate()
        {
            tblDocuments = new HashSet<tblDocument>();
            tblDocumentTypeMappings = new HashSet<tblDocumentTypeMapping>();
            tblProvinces = new HashSet<tblProvince>();
        }

        [Key]
        public int DocumentTemplateID { get; set; }

        public int LanguageID { get; set; }

        public int DocumentTypeID { get; set; }

        [StringLength(25)]
        public string FormID { get; set; }

        [StringLength(25)]
        public string Version { get; set; }

        [StringLength(250)]
        public string TemplateFileName { get; set; }

        [StringLength(250)]
        public string MappingFileName { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? UsableDate { get; set; }

        [Required]
        [StringLength(50)]
        public string FileFormat { get; set; }

        public bool IsStatic { get; set; }

        public int? FormFileFormat { get; set; }

        public int? FormWebContext { get; set; }

        public int? DocumentFileFormat { get; set; }

        public int? DocumentWebContext { get; set; }

        public virtual ICollection<tblDocument> tblDocuments { get; set; }

        public virtual tblDocumentFileOutputInfo tblDocumentFileOutputInfo { get; set; }
        
        public virtual tblDocumentMappingFile tblDocumentMappingFile { get; set; }

        public virtual tblDocumentTemplateFile tblDocumentTemplateFile { get; set; }

        public virtual tblDocumentType tblDocumentType { get; set; }

        public virtual tblLanguage tblLanguage { get; set; }

        public virtual ICollection<tblDocumentTypeMapping> tblDocumentTypeMappings { get; set; }

        public virtual ICollection<tblProvince> tblProvinces { get; set; }
    }
}
