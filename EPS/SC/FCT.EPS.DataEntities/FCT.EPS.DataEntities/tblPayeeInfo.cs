namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblPayeeInfo")]
    public partial class tblPayeeInfo
    {
        public tblPayeeInfo()
        {
            tblBatchSchedule = new HashSet<tblBatchSchedule>();
            tblPayeeAlias = new HashSet<tblPayeeAlias>();
            tblPayeeInfoHistory = new HashSet<tblPayeeInfoHistory>();
            tblPayeeReference = new HashSet<tblPayeeReference>();
            tblBPSReference = new HashSet<tblBPSReference>();
            tblPaymentRequest = new HashSet<tblPaymentRequest>();
        }

        [Key]
        public int PayeeInfoID { get; set; }

        [Required]
        [StringLength(100)]
        public string PayeeName { get; set; }

        [StringLength(100)]
        public string BankAccountHolderName { get; set; }

        [StringLength(40)]
        public string PayeeChequeName { get; set; }

        public int? PaymentRequestTypeID { get; set; }

        public int? PaymentMethodID { get; set; }

        public int? PayeeAddressID { get; set; }

        public int? AccountID { get; set; }

        [StringLength(50)]
        public string PayeeContact { get; set; }

        [StringLength(50)]
        public string PayeeContactPhoneNumber { get; set; }

        [StringLength(50)]
        public string PayeeEmail { get; set; }

        public bool PayeeAddressRequiredbyLawyer { get; set; }

        public DateTime? AgreementSignedDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime StatusChangeDate { get; set; }

        [Required]
        [StringLength(400)]
        public string PayeeComments { get; set; }

        [Required]
        public Boolean PaymentReportEnabled { get; set; }

        [ForeignKey("PayeeAddressID")]
        public virtual tblAddress tblAddress { get; set; }

        public virtual ICollection<tblBatchSchedule> tblBatchSchedule { get; set; }

        public virtual ICollection<tblBPSReference> tblBPSReference { get; set; }

        public virtual tblPayeeAccount tblPayeeAccount { get; set; }

        public virtual ICollection<tblPayeeAlias> tblPayeeAlias { get; set; }

        public virtual ICollection<tblPayeeInfoHistory> tblPayeeInfoHistory { get; set; }

        public virtual ICollection<tblPayeeReference> tblPayeeReference { get; set; }

        public virtual tblPaymentMethod tblPaymentMethod { get; set; }

        public virtual tblPaymentRequestType tblPaymentRequestType { get; set; }

        [ForeignKey("PayeeInfoID")]
        public virtual tblCreditorList tblCreditorList { get; set; }

        public virtual ICollection<tblPaymentRequest> tblPaymentRequest { get; set; }

    }
}
