using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.Portal.DTOs.Dto
{
    public partial class DealDocument
    {
        public DealDocument() { }
        public DealDocument(object d)
        {
            if (d != null)
            {
                List<PropertyInfo> curProps = typeof(DealDocument).GetProperties().ToList();
                d.GetType().GetProperties().Where(p => curProps.Any(cp => cp.Name == p.Name && cp.PropertyType.Name == p.PropertyType.Name)).ToList().ForEach(pr =>
                {
                    typeof(DealDocument).GetProperty(pr.Name).SetValue(this, d.GetType().GetProperty(pr.Name).GetValue(d, null));
                });
            }
        }

        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 1)]
        public string BusinessModel { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 2)]
        public DateTime CreatedDate { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 3)]
        public int DealDocumentTypeId { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 4)]
        public int DealId { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 5)]
        public string DisplayName { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 6)]
        public int? DisplayNameSuffix { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 7)]
        public string DmsPath { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 8)]
        public int DocumentCategoryId { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 9)]
        public int DocumentId { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 10)]
        public string DocumentOriginator { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 11)]
        public int? DocumentTypeCodeId { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 12)]
        public int DocumentTypeDisplayTypeId { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 13)]
        public int DocumentTypeId { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 14)]
        public int DocumentVersion { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 15)]
        public string FileName { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 16)]
        public byte[] Hash { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 17)]
        public bool IsActive { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 18)]
        public bool IsDefaultType { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 19)]
        public bool IsGeneratable { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 20)]
        public bool IsGenerated { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 21)]
        public bool IsOther { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 22)]
        public bool IsPublishable { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 23)]
        public bool IsSignatureRequired { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 24)]
        public bool IsSigned { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 25)]
        public bool IsSubmitable { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 26)]
        public bool IsUploadable { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 27)]
        public int? LanguageId { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 28)]
        public DateTime LastModifiedDate { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 29)]
        public int LenderId { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 30)]
        public string Name { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 31)]
        public DateTime? PublishedDate { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 32)]
        public int? ReviewedByBranchContactID { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 33)]
        public System.Data.SqlTypes.SqlDateTime? ReviewedDate { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 34)]
        public int? SourceDocumentID { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 35)]
        public DateTime? SubmittedDate { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 36)]
        public object Tag { get; set; }
        //[System.Runtime.Serialization.DataMemberAttribute(IsRequired = true, Order = 37)]
        public string Value { get; set; }
    }
}
