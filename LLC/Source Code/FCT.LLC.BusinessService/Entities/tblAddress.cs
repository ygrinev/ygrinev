using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblAddress")]
    public partial class tblAddress
    {
        public tblAddress()
        {
            tblCompanies = new HashSet<tblCompany>();
            tblContactInfoes = new HashSet<tblContactInfo>();
            //tblMortgages = new HashSet<tblMortgage>();
            //tblMortgages1 = new HashSet<tblMortgage>();
            //tblMortgages2 = new HashSet<tblMortgage>();
            tblPersons = new HashSet<tblPerson>();
        }

        [Key]
        public int AddressID { get; set; }

        [StringLength(25)]
        public string UnitNumber { get; set; }

        [StringLength(25)]
        public string StreetNumber { get; set; }

        [StringLength(25)]
        public string StreetType { get; set; }

        [StringLength(25)]
        public string StreetDirection { get; set; }

        [StringLength(100)]
        public string StreetName { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(2)]
        public string Province { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] LastModified { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(100)]
        public string StreetAddress1 { get; set; }

        [StringLength(100)]
        public string StreetAddress2 { get; set; }

        public virtual tblProvince tblProvince { get; set; }

        public virtual ICollection<tblCompany> tblCompanies { get; set; }

        public virtual ICollection<tblContactInfo> tblContactInfoes { get; set; }

        //public virtual ICollection<tblMortgage> tblMortgages { get; set; }

        //public virtual ICollection<tblMortgage> tblMortgages1 { get; set; }

        //public virtual ICollection<tblMortgage> tblMortgages2 { get; set; }

        public virtual ICollection<tblPerson> tblPersons { get; set; }
    }
}
