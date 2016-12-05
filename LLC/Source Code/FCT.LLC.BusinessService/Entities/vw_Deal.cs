namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    public partial class vw_Deal
    {
        public vw_Deal()
        {
        }

        [DataMember]
        [Key]
        public int DealID { get; set; }

        [DataMember]
        [StringLength(11)]
        public string LLCRefNum { get; set; }

        [DataMember]
        [StringLength(11)]
        public string FCTRefNum { get; set; }

        [DataMember]
        public int? DealScopeID { get; set; }

        [DataMember]
        public int LawyerID { get; set; }

        [DataMember]
        [StringLength(50)]
        public string LenderRefNum { get; set; }

        [DataMember]
        [StringLength(50)]
        public string Status { get; set; }

        [DataMember]
        [StringLength(50)]
        public string StatusUserType { get; set; }

        [DataMember]
        [StringLength(1000)]
        public string StatusReason { get; set; }

        [DataMember]
        public string Address { get; set; }

        [DataMember]
        public string VendorLastName { get; set; }

        [DataMember]
        [StringLength(20)]
        public string LawyerMatterNumber { get; set; }

        [DataMember]
        // [Required]
        [StringLength(20)]
        public string BusinessModel { get; set; }

        [DataMember]
        public string ClientName { get; set; }

        [DataMember]
        [StringLength(20)]
        public string LawyerActingFor { get; set; }

        [DataMember]
        public DateTime? ClosingDate { get; set; }

        [DataMember]
        [StringLength(50)]
        public string MortgageNumber { get; set; }

        [DataMember]
        public string LawyerName { get; set; }

        [DataMember]
        [StringLength(20)]
        public string WireDepositVerificationCode { get; set; }

        [DataMember]
        public virtual tblDealScope tblDealScope { get; set; }

        [DataMember]
        public virtual tblLawyer tblLawyer { get; set; }

    }
}
