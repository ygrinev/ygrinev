namespace FCT.LLC.BusinessService.DataAccess
{
    public class DealDocumentType
    {
        public int DealDocumentTypeId { get; set; }
        public int DealId { get; set; }
        public int DocumentTypeId { get; set; }
        public bool IsActive { get; set; }
        public int? DisplayNameSuffix { get; set; }
    }
}
