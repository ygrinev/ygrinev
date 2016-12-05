using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocument")]
    public partial class tblDocument
    {
        [Key]
        public int DocumentID {get; set; }
        public int DealDocumentTypeID {get; set; }

        [StringLength(200)]
        public string DisplayName {get; set; }
        public bool IsGenerated {get; set; }

        [StringLength(250)]
        public string DMSPath {get; set; }
        public bool IsSigned {get; set; }
        public DateTime CreatedDate {get; set; }
        public DateTime LastModifiedDate {get; set; }

        [StringLength(255)]
        public string FileName {get; set; }
        [MaxLength(16)]
        public byte[] Hash {get; set; }

        [StringLength(25)]  
        public string DocumentOriginator {get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] Version { get; set; }
        public DateTime? SubmittedDate {get; set; }
        public int? DocumentTemplateID {get; set; }
        public int? SubmittedByLawyerID {get; set; }
        public DateTime? ReviewedDate {get; set; }
        public int? ReviewedByBranchContactID {get; set; }
        public bool IsDocumentActive {get; set; }
      
        [StringLength(255)]
        public string ArchiveDMSPath {get; set; }

        [MaxLength(16)]
        public byte[] ArchiveHash { get; set; }
        public DateTime? PublishedDate {get; set; }
        public int? SourceDocumentID {get; set; }
    }
}
