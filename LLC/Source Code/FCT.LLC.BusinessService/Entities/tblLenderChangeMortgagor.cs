using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderChangeMortgagor")]
    public partial class tblLenderChangeMortgagor
    {
        [Key]
        public int LenderChangeMortgagorID { get; set; }

        public int LenderChangeID { get; set; }

        public int DealID { get; set; }

        [StringLength(50)]
        public string MortgagorType { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(200)]
        public string CompanyName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(2)]
        public string Province { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string HomePhone { get; set; }

        [StringLength(50)]
        public string BusinessPhone { get; set; }

        public bool? HasSpouse { get; set; }

        public bool? IsGuarantor { get; set; }

        [StringLength(50)]
        public string SpouseLastName { get; set; }

        [StringLength(100)]
        public string SpouseFirstName { get; set; }

        [StringLength(50)]
        public string SpouseMiddleName { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(50)]
        public string Occupation { get; set; }

        public int? PrimaryIdentificationID { get; set; }

        public int? SecondaryIdentificationID { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        [StringLength(25)]
        public string UnitNumber { get; set; }

        [StringLength(25)]
        public string StreetNumber { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string Language { get; set; }

        public bool? IsILARequired { get; set; }

        public int? SpousePrimaryIdentificationID { get; set; }

        public int? SpouseSecondaryIdentificationID { get; set; }

        public bool? IsSpouseILARequired { get; set; }

        public int? LenderMortgagorID { get; set; }

        public int? PriorityIndicator { get; set; }

        [StringLength(1000)]
        public string SpousalStatement { get; set; }

        [StringLength(50)]
        public string SpouseOccupation { get; set; }

        public virtual tblDeal tblDeal { get; set; }

        public virtual tblProvince tblProvince { get; set; }
    }
}
