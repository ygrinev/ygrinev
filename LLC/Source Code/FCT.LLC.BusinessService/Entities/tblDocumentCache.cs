using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDocumentCache")]
    public partial class tblDocumentCache
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DocumentID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Version { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateOfNextMoveStart { get; set; }

        [StringLength(50)]
        public string DMSJobID { get; set; }

        [Required]
        public byte[] DocumentFile { get; set; }

        public int? DMSStatusRetryCount { get; set; }

        public bool CanBeRemoved { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        public virtual tblDocument tblDocument { get; set; }
    }
}
