using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.EPS.DataEntities
{
    [Table("tblPaymentReportFields")]
    public partial class tblPaymentReportFields
    {

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int TemplateFieldID { get; set; }

        [Required]
        [StringLength(2)]
        public string LanguageCode { get; set; }

        [Required]
        public int PayeeInfoID { get; set; }

        [ForeignKey("PayeeInfoID")]
        public virtual tblPaymentReportInfo tblPaymentReportInfo { get; set; }

        [Required]
        public int TemplateFieldIndex { get; set; }

        [Required]
        public string TemplateFieldName { get; set; }

        [Required]
        public int ReportLabelID { get; set; }

        [ForeignKey("ReportLabelID,LanguageCode")]
        public virtual tblPaymentReportLabels tblPaymentReportLabels { get; set; }

        [Required]
        [StringLength(20)]
        public string TemplateFieldType { get; set; }

        [ForeignKey("TemplateFieldID,LanguageCode")]
        public virtual tblPaymentReportFieldFormat tblPaymentReportFieldFormat { get; set; }



    }
}
