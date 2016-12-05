using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderChangeGuarantor")]
    public partial class tblLenderChangeGuarantor
    {
        [Key]
        public int LenderChangeGuarantorID { get; set; }

        public int LenderChangeID { get; set; }

        public int? DealID { get; set; }

        [StringLength(50)]
        public string GuarantorType { get; set; }

        public int? CompanyID { get; set; }

        public int? PersonID { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        public bool? IsILARequired { get; set; }

        public int? LenderGuarantorID { get; set; }

        public virtual tblDeal tblDeal { get; set; }
    }
}
