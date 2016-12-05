namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPaymentRequestType")]
    public partial class tblPaymentRequestType
    {
        public tblPaymentRequestType()
        {
            tblPayeeInfo = new HashSet<tblPayeeInfo>();
        }

        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int PaymentRequestTypeID { get; set; }

        [StringLength(10)]
        public string PaymentRequestTypeName { get; set; }

        public virtual ICollection<tblPayeeInfo> tblPayeeInfo { get; set; }
    }
}
