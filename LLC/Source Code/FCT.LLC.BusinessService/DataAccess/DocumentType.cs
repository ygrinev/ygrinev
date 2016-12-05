namespace FCT.LLC.BusinessService.DataAccess
{
    public class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public int DocumentCategoryId { get; set; }
        public int? DocumentTypeCodeId { get; set; }
        public bool IsSignatureRequired { get; set; }
        public bool IsUploadable { get; set; }
        public bool IsGeneratable { get; set; }
        public string Name { get; set; }
        public bool IsSubmitable { get; set; }
        public bool IsOther { get; set; }
        public bool IsDefaultType { get; set; }
        public int? LenderId { get; set; }
        public bool IsEditable { get; set; }
        public bool IsCacheable { get; set; }
        public bool VirusScan { get; set; }
        public string IsSubmitableCondition { get; set; }
        public bool IsArchivable { get; set; }
        public string IsUploadableCondition { get; set; }
        public string IsArchivableCondition { get; set; }
        public bool IsPublishable { get; set; }
        public string IsPublishableCondition { get; set; }
        public string BusinessModel { get; set; }
    }
}
