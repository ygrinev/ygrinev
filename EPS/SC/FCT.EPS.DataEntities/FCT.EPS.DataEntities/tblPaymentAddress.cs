namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPaymentAddress")]
    public partial class tblPaymentAddress
    {
        public tblPaymentAddress()
        {
            tblPaymentRequest_PayeeBranchAddress = new HashSet<tblPaymentRequest>();
            tblPaymentRequest_PayeeAddress = new HashSet<tblPaymentRequest>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int PaymentAddressID { get; set; }

        [StringLength(20)]
        public string UnitNumber { get; set; }

        [StringLength(40)]
        public string StreetNumber { get; set; }

        [StringLength(40)]
        public string StreetAddress1 { get; set; }

        [StringLength(40)]
        public string StreetAddress2 { get; set; }

        [StringLength(40)]
        public string City { get; set; }

        [StringLength(2)]
        public string ProvinceCode { get; set; }

        [StringLength(40)]
        public string Province { get; set; }

        [StringLength(7)]
        public string PostalCode { get; set; }

        [StringLength(40)]
        [Required()]
        public string Country { get; set; }

        [InverseProperty("tblPaymentAddress_PayeeBranchAddress")]
        public virtual ICollection<tblPaymentRequest> tblPaymentRequest_PayeeBranchAddress { get; set; }

        [InverseProperty("tblPaymentAddress_PayeeAddress")]
        public virtual ICollection<tblPaymentRequest> tblPaymentRequest_PayeeAddress { get; set; }
    }
}
