using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblSolicitorSyncReportData")]
    public partial class tblSolicitorSyncReportData
    {
        [Key]
        public int SolicitorSyncReportDataID { get; set; }

        public int SolicitorSyncID { get; set; }

        [Required]
        [StringLength(15)]
        public string FCTUniqueID { get; set; }

        [Required]
        [StringLength(15)]
        public string Operation { get; set; }

        [StringLength(50)]
        public string Firmname { get; set; }

        [StringLength(25)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(25)]
        public string MiddleName { get; set; }

        [StringLength(50)]
        public string BusinessEmail { get; set; }

        [StringLength(150)]
        public string BusinessSteetAddress1 { get; set; }

        [StringLength(100)]
        public string BusinessStreetAddress2 { get; set; }

        [StringLength(50)]
        public string SuiteUnitNumber { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(2)]
        public string Province { get; set; }

        [StringLength(7)]
        public string PostalCode { get; set; }

        [StringLength(100)]
        public string Country { get; set; }

        [StringLength(150)]
        public string BusinessPhone { get; set; }

        [StringLength(15)]
        public string BusinessFax { get; set; }

        [StringLength(15)]
        public string Cellular { get; set; }

        [StringLength(20)]
        public string TrustAccountNumber { get; set; }

        [StringLength(10)]
        public string TrustAccountBranchTransitNumber { get; set; }

        [StringLength(10)]
        public string TrustAccountBankNumber { get; set; }

        [StringLength(200)]
        public string TrustAccountBankName { get; set; }

        [StringLength(10)]
        public string LanguagePreference { get; set; }

        [StringLength(256)]
        public string LawSocietyStatus { get; set; }

        [StringLength(25)]
        public string LawSocietyFirstName { get; set; }

        [StringLength(25)]
        public string LawSocietyMiddleName { get; set; }

        [StringLength(50)]
        public string LawSocietyLastName { get; set; }

        public virtual tblSolicitorSync tblSolicitorSync { get; set; }
    }
}
