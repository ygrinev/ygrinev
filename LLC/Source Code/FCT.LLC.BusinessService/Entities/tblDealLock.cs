using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblDealLock")]
    public partial class tblDealLock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DealID { get; set; }

        [Required]
        [StringLength(50)]
        public string LockedBy { get; set; }

        public DateTime LockedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int LockExpiresAfterMin { get; set; }

        public bool DeletionMark { get; set; }

        public virtual tblDeal tblDeal { get; set; }
    }
}
