using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.EPS.DataEntities
{

    [Table("tblBatchPaymentReportInfo")]
    public partial class tblBatchPaymentReportInfo
    {

        [Key, Column(Order = 1)]
        public int PaymentRequestID { get; set; }

        [Key, Column(Order = 2)]
        [StringLength(200)]
        public string FieldName { get; set; }

        [StringLength(200)]
        public string FieldValue { get; set; }

        [ForeignKey("PaymentRequestID")]
        public virtual tblPaymentRequest tblPaymentRequest { get; set; }

    }
}
