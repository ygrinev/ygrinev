using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.EPS.DataEntities
{
    [Table("tblPaymentReportLabels")]
    public partial class tblPaymentReportLabels
    {
        [Key]
        [Column(Order=0)]
        public int ReportLabelID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string LanguageCode { get; set; }

        [Required]
        [StringLength(50)]
        public string FieldLabelName { get; set; }
    }
}
