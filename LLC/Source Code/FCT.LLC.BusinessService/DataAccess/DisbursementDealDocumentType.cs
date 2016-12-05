using System;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DisbursementDealDocumentType
    {
        public int DisbursementDealDocumentTypeId { get; set; }
        public int DisbursementId { get; set; }
        public int DealDocumentTypeId { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
