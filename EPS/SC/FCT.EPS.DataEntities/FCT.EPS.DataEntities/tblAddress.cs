namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblAddress")]
    public partial class tblAddress
    {
        public tblAddress()
        {
            tblFCTAccount = new HashSet<tblFCTAccount>();
            tblPayeeAccount = new HashSet<tblPayeeAccount>();
            tblPayeeInfo = new HashSet<tblPayeeInfo>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int AddressID { get; set; }

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

        [StringLength(40)]
        public string Province { get; set; }

        [StringLength(2)]
        public string ProvinceCode { get; set; }

        [StringLength(7)]
        public string PostalCode { get; set; }

        [StringLength(40)]
        public string Country { get; set; }

        public virtual ICollection<tblFCTAccount> tblFCTAccount { get; set; }

        public virtual ICollection<tblPayeeAccount> tblPayeeAccount { get; set; }

        public virtual ICollection<tblPayeeInfo> tblPayeeInfo { get; set; }
    }
}
