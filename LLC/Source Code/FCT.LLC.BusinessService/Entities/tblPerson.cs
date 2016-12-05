using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblPerson")]
    public partial class tblPerson
    {
        public tblPerson()
        {
            tblAttorneys = new HashSet<tblAttorney>();
            tblBranches = new HashSet<tblBranch>();
            tblCompanies = new HashSet<tblCompany>();
            tblGuarantors = new HashSet<tblGuarantor>();
            tblLandLeases = new HashSet<tblLandLease>();
            tblMortgageILAs = new HashSet<tblMortgageILA>();
            //tblPerson1 = new HashSet<tblPerson>();
            tblSignatories = new HashSet<tblSignatory>();
        }

        [Key]
        public int PersonID { get; set; }

        [StringLength(10)]
        public string Title { get; set; }

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string MaidenName { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(50)]
        public string Occupation { get; set; }

        public int? PrimaryIdentificationID { get; set; }

        public int? SecondaryIdentificationID { get; set; }

        public int? HomeAddressID { get; set; }

        public int? PrimaryContactInfoID { get; set; }

        public int? SpouseID { get; set; }

        [StringLength(50)]
        public string Language { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        public virtual tblAddress tblAddress { get; set; }

        public virtual ICollection<tblAttorney> tblAttorneys { get; set; }

        public virtual ICollection<tblBranch> tblBranches { get; set; }

        public virtual ICollection<tblCompany> tblCompanies { get; set; }

        public virtual tblContactInfo tblContactInfo { get; set; }

        public virtual ICollection<tblGuarantor> tblGuarantors { get; set; }

        public virtual tblIdentification tblIdentification { get; set; }

        public virtual tblIdentification tblIdentification1 { get; set; }

        public virtual ICollection<tblLandLease> tblLandLeases { get; set; }

        public virtual ICollection<tblMortgageILA> tblMortgageILAs { get; set; }

        //public virtual ICollection<tblPerson> tblPerson1 { get; set; }

        public virtual tblPerson tblPerson2 { get; set; }

        public virtual ICollection<tblSignatory> tblSignatories { get; set; }
    }
}
