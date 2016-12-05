using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblFireInsurancePolicy")]
    public partial class tblFireInsurancePolicy
    {
        [Key]
        public int FireInsurancePolicyID { get; set; }

        public int PropertyID { get; set; }

        [StringLength(50)]
        public string PolicyNumber { get; set; }

        public DateTime? PolicyActiveDate { get; set; }

        public DateTime? ExpiryDate { get; set; }

        [StringLength(50)]
        public string AgentFirstName { get; set; }

        [StringLength(50)]
        public string AgentLastName { get; set; }

        [StringLength(50)]
        public string Broker { get; set; }

        [StringLength(50)]
        public string InsuranceAmount { get; set; }

        [StringLength(50)]
        public string InsuranceCompanyName { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(15)]
        public string Fax { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(2)]
        public string Province { get; set; }

        [StringLength(7)]
        public string PostalCode { get; set; }

        [StringLength(10)]
        public string UnitNumber { get; set; }

        [StringLength(10)]
        public string StreetNumber { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(15)]
        public string BrokerPhoneNumber { get; set; }

        public virtual tblProperty tblProperty { get; set; }
    }
}
