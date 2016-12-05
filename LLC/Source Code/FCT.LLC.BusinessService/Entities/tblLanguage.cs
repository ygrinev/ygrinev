using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLanguage")]
    public partial class tblLanguage
    {
        public tblLanguage()
        {
            tblCalculationPeriodDescriptions = new HashSet<tblCalculationPeriodDescription>();
            tblDocumentKeyValues = new HashSet<tblDocumentKeyValue>();
            tblDocumentTemplates = new HashSet<tblDocumentTemplate>();
            tblDocumentTypeDisplays = new HashSet<tblDocumentTypeDisplay>();
            tblEmailTemplateLists = new HashSet<tblEmailTemplateList>();
            tblLenderSchemaFieldsDescriptions = new HashSet<tblLenderSchemaFieldsDescription>();
            tblMortgagePaymentRegistrationDatas = new HashSet<tblMortgagePaymentRegistrationData>();
            tblMortgageRegistrationDatas = new HashSet<tblMortgageRegistrationData>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LanguageID { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        public virtual ICollection<tblCalculationPeriodDescription> tblCalculationPeriodDescriptions { get; set; }

        public virtual ICollection<tblDocumentKeyValue> tblDocumentKeyValues { get; set; }

        public virtual ICollection<tblDocumentTemplate> tblDocumentTemplates { get; set; }

        public virtual ICollection<tblDocumentTypeDisplay> tblDocumentTypeDisplays { get; set; }

        public virtual ICollection<tblEmailTemplateList> tblEmailTemplateLists { get; set; }

        public virtual ICollection<tblLenderSchemaFieldsDescription> tblLenderSchemaFieldsDescriptions { get; set; }

        public virtual ICollection<tblMortgagePaymentRegistrationData> tblMortgagePaymentRegistrationDatas { get; set; }

        public virtual ICollection<tblMortgageRegistrationData> tblMortgageRegistrationDatas { get; set; }
    }
}
