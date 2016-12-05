using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblSolicitorInstructionPOA")]
    public partial class tblSolicitorInstructionPOA
    {
        public int DealID { get; set; }

        [Required]
        [StringLength(100)]
        public string DonorName { get; set; }

        public DateTime SignedDate { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] LastModified { get; set; }

        [Key]
        public int SolicitorInstructionPOAID { get; set; }

        public virtual tblDeal tblDeal { get; set; }
    }
}
