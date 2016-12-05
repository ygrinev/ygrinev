using FCT.LLC.GenericRepository;

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
    [Table("tblLawyer")]
    public partial class tblLawyer
    {
        public tblLawyer()
        {
            tblDeals = new HashSet<tblDeal>();
            tblDealContacts = new HashSet<tblDealContact>();
        }

        [DataMember]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LawyerID { get; set; }

        [DataMember]
        [StringLength(50)]
        public string LastName { get; set; }

        [DataMember]
        [StringLength(100)]
        public string FirstName { get; set; }

        [DataMember]
        [StringLength(50)]
        public string MiddleName { get; set; }

        [DataMember]
        [StringLength(50)]
        public string LawFirm { get; set; }

        [DataMember]
        [StringLength(50)]
        public string UnitNo { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Address { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Address2 { get; set; }

        [DataMember]
        [StringLength(50)]
        public string City { get; set; }

        [DataMember]
        [StringLength(2)]
        public string Province { get; set; }

        [DataMember]
        [StringLength(7)]
        public string PostalCode { get; set; }

        [DataMember]
        [StringLength(15)]
        public string Phone { get; set; }

        [DataMember]
        [StringLength(15)]
        public string MobilePhone { get; set; }

        [DataMember]
        [StringLength(15)]
        public string Fax { get; set; }

        [DataMember]
        [StringLength(100)]
        public string EMail { get; set; }

        [DataMember]
        [StringLength(20)]
        public string UserID { get; set; }

        [DataMember]
        [MaxLength(256)]
        public byte[] Password { get; set; }

        [DataMember]
        public bool? PasswordReset { get; set; }

        [DataMember]
        public bool? Active { get; set; }

        [DataMember]
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        [DataMember]
        [StringLength(1000)]
        public string Comments { get; set; }

        [DataMember]
        public bool? IsAssistant { get; set; }

        [DataMember]
        [StringLength(100)]
        public string LawyerSoftwareUsed { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Profession { get; set; }

        [DataMember]
        public DateTime? RegistrationDate { get; set; }

        [DataMember]
        public DateTime? AgreementReceivedDate { get; set; }

        [DataMember]
        public DateTime? PasswordSetDate { get; set; }

        [DataMember]
        [StringLength(50)]
        public string StreetNumber { get; set; }

        [DataMember]
        [StringLength(50)]
        public string UserLanguage { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Country { get; set; }

        [DataMember]
        public int? BillingAddressID { get; set; }

        [DataMember]
        public int LawyerCode { get; set; }

        [DataMember]
        public DateTime ProfileCreateDateTime { get; set; }

        [DataMember]
        public DateTime? ProfileActiveDateTime { get; set; }

        [DataMember]
        public DateTime? ProfileDeactiveDateTime { get; set; }

        [DataMember]
        public DateTime? ProfileModifyDateTime { get; set; }

        [DataMember]
        public bool? ValidatedByFCT { get; set; }

        [DataMember]
        [StringLength(50)]
        public string RequestSource { get; set; }

        [DataMember]
        public int UserStatusID { get; set; }

        [DataMember]
        [StringLength(50)]
        public string InternetBrowserUsed { get; set; }

        [DataMember]
        [StringLength(100)]
        public string LawSocietyFirstName { get; set; }

        [DataMember]
        [StringLength(50)]
        public string LawSocietyMiddleName { get; set; }

        [DataMember]
        [StringLength(50)]
        public string LawSocietyLastName { get; set; }

        [DataMember]
        [StringLength(50)]
        public string SolicitorSyncLawSocietyStatus { get; set; }

        [DataMember]
        public bool? TestAccount { get; set; }

        [DataMember]
        public virtual ICollection<tblDeal> tblDeals { get; set; }

        [DataMember]
        public virtual ICollection<tblDealContact> tblDealContacts { get; set; }
    }
}
