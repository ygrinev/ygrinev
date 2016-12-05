namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblLender")]
    public partial class tblLender
    {
        public tblLender()
        {
            tblMilestoneLabels = new HashSet<tblMilestoneLabel>();
            tblFinancialInstitutionNumbers = new HashSet<tblFinancialInstitutionNumber>();
        }

        [Key]
        public int LenderID { get; set; }

        [Required]
        [StringLength(6)]
        public string LenderCode { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(2)]
        public string Province { get; set; }

        [StringLength(7)]
        public string PostalCode { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(15)]
        public string Fax { get; set; }

        public bool Active { get; set; }

        [StringLength(50)]
        public string LogoName { get; set; }

        [StringLength(32)]
        public string BillingID { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        public int? DepositToFCTAccountID { get; set; }

        [Required]
        [StringLength(10)]
        public string ShortName { get; set; }

        public bool? Is2WayLender { get; set; }

        public bool? IsRealLender { get; set; }

        public bool? TestLender { get; set; }

        public virtual ICollection<tblMilestoneLabel> tblMilestoneLabels { get; set; }

        public virtual ICollection<tblFinancialInstitutionNumber> tblFinancialInstitutionNumbers { get; set; }
    }
}
