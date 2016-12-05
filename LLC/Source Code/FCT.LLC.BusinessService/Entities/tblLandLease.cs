using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLandLease")]
    public partial class tblLandLease
    {
        [Key]
        public int LandLeaseID { get; set; }

        public int PropertyID { get; set; }

        public int LandlordPersonID { get; set; }

        [StringLength(100)]
        public string LandlordCompanyName { get; set; }

        [StringLength(50)]
        public string Term { get; set; }

        public string Clauses { get; set; }

        public bool? NoticeProvided { get; set; }

        [StringLength(50)]
        public string RegistrationNumber { get; set; }

        public DateTime? RegistrationDate { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] LastModified { get; set; }

        public virtual tblPerson tblPerson { get; set; }

        public virtual tblProperty tblProperty { get; set; }
    }
}
