using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblContactInfo")]
    public partial class tblContactInfo
    {
        public tblContactInfo()
        {
            tblCompanies = new HashSet<tblCompany>();
            tblPersons = new HashSet<tblPerson>();
        }

        [Key]
        public int ContactInfoID { get; set; }

        public int? MailingAddressID { get; set; }

        [StringLength(50)]
        public string WorkPhone { get; set; }

        [StringLength(50)]
        public string MobilePhone { get; set; }

        [StringLength(50)]
        public string HomePhone { get; set; }

        [StringLength(50)]
        public string Fax { get; set; }

        [StringLength(50)]
        public string EMail { get; set; }

        [StringLength(1000)]
        public string Comments { get; set; }

        public virtual tblAddress tblAddress { get; set; }

        public virtual ICollection<tblCompany> tblCompanies { get; set; }

        public virtual ICollection<tblPerson> tblPersons { get; set; }
    }
}
