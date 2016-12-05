using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDisbursementDealDocumentType")]
    public partial class tblDisbursementDealDocumentType
    {
        [Key]
        public int DisbursementDealDocumentType{ get; set; }
        public int DisbursementID { get; set; }
        public int DealDocumentTypeID { get; set; }
        public DateTime? PayoutLetterDate { get; set; }
    }
}
