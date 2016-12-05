using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPIFInfo")]
    public class tblPifInfo
    {
        [Key]
        public int PifInfoId { get; set; } // PIFInfoID (Primary key)
        public int PropertyId { get; set; } // PropertyID
        public bool AdditionalScheduleAttached { get; set; } // AdditionalScheduleAttached
        public bool PropertyTaxNotAssessed { get; set; } // PropertyTaxNotAssessed
        public int? AnnualTaxAmountYear { get; set; } // AnnualTaxAmountYear
        public decimal? CurrentYearTaxAmount { get; set; } // CurrentYearTaxAmount
        public decimal? TaxAmountPaidClosing { get; set; } // TaxAmountPaidClosing
        public decimal? MortgagePortionUsedByTaxPayAmount { get; set; } // MortgagePortionUsedByTaxPayAmount
        public bool? MortgagePortionUsedByTaxPay { get; set; } // MortgagePortionUsedByTaxPay
        public bool? NoTaxPaidOut { get; set; } // NoTaxPaidOut
        public string Arn2 { get; set; } // ARN2 (length: 250)
        public decimal? AnnualTaxAmount2 { get; set; } // AnnualTaxAmount2
        public decimal? TaxPaid2 { get; set; } // TaxPaid2
        public decimal? OutstandingAmount2 { get; set; } // OutstandingAmount2
        public string SchoolBoard { get; set; } // SchoolBoard (length: 100)
        public System.DateTime? SubmissionDate { get; set; } // SubmissionDate
        public int? LroNumber { get; set; } // LRONumber
        public bool? TaxPaidOnClosingforLenderInstruction { get; set; } // TaxPaidOnClosingforLenderInstruction
        public System.DateTime LastModified { get; set; } // LastModified

        // Foreign keys
        public virtual tblProperty TblProperty { get; set; } // FK_tblPIFInfo_PropertyID
    }
}
