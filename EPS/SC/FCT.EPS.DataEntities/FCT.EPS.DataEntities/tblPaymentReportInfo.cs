using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.EPS.DataEntities
{
    [Table("tblPaymentReportinfo")]
    public partial class tblPaymentReportInfo
    {
        public tblPaymentReportInfo()
        {
            tblPaymentReportFields = new HashSet<tblPaymentReportFields>();
        }
        [Key]
        public int PayeeInfoID { get; set; }

        [Required]
        [StringLength(1000)]
        public string PaymentReportEmailAddresses { get; set; }

        [StringLength(5000)]
        public string PaymentReportBody { get; set; }

        [StringLength(1000)]
        public string PaymentReportEmailSubject { get; set; }

        [Required]
        public int ReportFileFormatID { get; set; }

        [ForeignKey("PayeeInfoID")]
        public virtual ICollection<tblPaymentReportFields> tblPaymentReportFields { get; set; }

        [ForeignKey("PayeeInfoID")]
        public virtual tblPayeeInfo tblPayeeInfo { get; set; }

        [ForeignKey("ReportFileFormatID")]
        public virtual tblReportFileFormat tblReportFileFormat { get; set; }
    }
}
