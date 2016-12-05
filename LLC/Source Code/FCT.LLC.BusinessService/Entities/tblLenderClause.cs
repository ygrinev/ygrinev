using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderClause")]
    public partial class tblLenderClause
    {
        [Key]
        public int ClauseID { get; set; }

        public int? DealID { get; set; }

        [StringLength(2000)]
        public string Clause { get; set; }

        public int? UserID { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DateAdded { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DateCompleted { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        [StringLength(10)]
        public string ClauseSequence { get; set; }

        public virtual tblDeal tblDeal { get; set; }
    }
}
