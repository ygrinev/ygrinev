namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPayeeInfoHistory")]
    public partial class tblPayeeInfoHistory
    {
        [Key]
        public int PayeeHistoryID { get; set; }

        public int PayeeInfoID { get; set; }

        [StringLength(40)]
        public string PayeeName { get; set; }

        [StringLength(50)]
        public string PayeeType { get; set; }

        [StringLength(10)]
        public string PaymendMethod { get; set; }

        [StringLength(10)]
        public string PaymentRequestType { get; set; }

        [StringLength(200)]
        public string PayeeAddress { get; set; }

        [StringLength(50)]
        public string PayeeContact { get; set; }

        [StringLength(50)]
        public string PayeeContactPhoneNumber { get; set; }

        [StringLength(50)]
        public string PayeeEmail { get; set; }

        public bool? PayeeAddressRequiredByLawyer { get; set; }

        public bool? Status { get; set; }

        public DateTime? StatusDate { get; set; }

        [StringLength(400)]
        public string PayeeComments { get; set; }

        public virtual tblPayeeInfo tblPayeeInfo { get; set; }
    }
}
