using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentArchive")]
    public partial class tblDocumentArchive
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocumentID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Version { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(50)]
        public string DMSJobID { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public DateTime? DateOfNextPollStart { get; set; }

        public int? DMSStatusRetryCount { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }
    }
}
