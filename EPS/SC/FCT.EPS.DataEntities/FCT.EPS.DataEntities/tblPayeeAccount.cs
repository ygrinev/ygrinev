namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPayeeAccount")]
    public partial class tblPayeeAccount
    {
        public tblPayeeAccount()
        {
            tblPayeeInfo = new HashSet<tblPayeeInfo>();
        }

        [Key]
        public int AccountID { get; set; }

        public int? AccountAddressID { get; set; }

        [StringLength(50)]
        public string BankName { get; set; }

        [StringLength(3)]
        public string BankNumber { get; set; }

        [StringLength(10)]
        public string TransitNumber { get; set; }

        [StringLength(20)]
        public string AccountNumber { get; set; }

        [StringLength(11)]
        public string BankSWIFTCode { get; set; }

        [StringLength(20)]
        public string CanadianClearingCode { get; set; }

        [ForeignKey("AccountAddressID")]
        public virtual tblAddress tblAddress { get; set; }

        public virtual ICollection<tblPayeeInfo> tblPayeeInfo { get; set; }
    }
}
