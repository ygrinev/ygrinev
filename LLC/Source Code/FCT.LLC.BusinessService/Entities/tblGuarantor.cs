using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblGuarantor")]
    public partial class tblGuarantor
    {
        public tblGuarantor()
        {
            tblSignatories = new HashSet<tblSignatory>();
        }

        [Key]
        public int GuarantorID { get; set; }

        public int DealID { get; set; }

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

        [StringLength(1000)]
        public string SpousalStatement { get; set; }

        public virtual tblCompany tblCompany { get; set; }

        public virtual tblDeal tblDeal { get; set; }

        public virtual tblPerson tblPerson { get; set; }

        public virtual ICollection<tblSignatory> tblSignatories { get; set; }
    }
}
