using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblBranch")]
    public partial class tblBranch
    {
        public tblBranch()
        {
            tblBranchContacts = new HashSet<tblBranchContact>();
        }

        [Key]
        public int BranchID { get; set; }

        public int LenderID { get; set; }

        [StringLength(500)]
        public string BranchName { get; set; }

        [StringLength(10)]
        public string TransitNumber { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(2)]
        public string Province { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string District { get; set; }

        public bool? Active { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        public int? ContactPersonID { get; set; }

        [StringLength(25)]
        public string StreetNumber { get; set; }

        [StringLength(100)]
        public string StreetAddress1 { get; set; }

        [StringLength(100)]
        public string StreetAddress2 { get; set; }

        [StringLength(50)]
        public string UnitNumber { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        public bool? IsMortgageSpecialist { get; set; }

        public virtual tblLender tblLender { get; set; }

        public virtual tblPerson tblPerson { get; set; }

        public virtual tblProvince tblProvince { get; set; }

        public virtual ICollection<tblBranchContact> tblBranchContacts { get; set; }
    }
}
