using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.EPS.DataEntities
{
    [Table("tblReportFileFormat")]
    public partial class tblReportFileFormat
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int ReportFileFormatID { get; set; }

        [Required]
        [StringLength(20)]
        public string ReportFileFormat { get; set; }

        [Required]
        [StringLength(20)]
        public string ReportFileExtesion { get; set; }
    }
}
