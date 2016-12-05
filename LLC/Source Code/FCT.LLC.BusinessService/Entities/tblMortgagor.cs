using FCT.LLC.GenericRepository;

namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [Table("tblMortgagor")]
    public partial class tblMortgagor
    {
        [DataMember]
        [Key]
        public int MortgagorID { get; set; }

        [DataMember]
        public int DealID { get; set; }

        [DataMember]
        [StringLength(50)]
        public string MortgagorType { get; set; }

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
        [StringLength(200)]
        public string CompanyName { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Address { get; set; }

        [DataMember]
        [StringLength(100)]
        public string City { get; set; }

        [DataMember]
        [StringLength(2)]
        public string Province { get; set; }

        [DataMember]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [DataMember]
        [StringLength(50)]
        public string HomePhone { get; set; }

        [DataMember]
        [StringLength(50)]
        public string BusinessPhone { get; set; }

        [DataMember]
        public bool? HasSpouse { get; set; }

        [DataMember]
        public bool? IsGuarantor { get; set; }

        [DataMember]
        [StringLength(50)]
        public string SpouseLastName { get; set; }

        [DataMember]
        [StringLength(100)]
        public string SpouseFirstName { get; set; }

        [DataMember]
        [StringLength(50)]
        public string SpouseMiddleName { get; set; }

        [DataMember]
        public DateTime? BirthDate { get; set; }

        [DataMember]
        [StringLength(50)]
        public string Occupation { get; set; }

        [DataMember]
        public int? PrimaryIdentificationID { get; set; }

        [DataMember]
        public int? SecondaryIdentificationID { get; set; }

        [DataMember]
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        [DataMember]
        [StringLength(25)]
        public string UnitNumber { get; set; }

        [DataMember]
        [StringLength(25)]
        public string StreetNumber { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Address2 { get; set; }

        [DataMember]
        [StringLength(50)]
        public string Country { get; set; }

        [DataMember]
        [StringLength(50)]
        public string Language { get; set; }

        [DataMember]
        public bool? IsILARequired { get; set; }

        [DataMember]
        public int? SpousePrimaryIdentificationID { get; set; }

        [DataMember]
        public int? SpouseSecondaryIdentificationID { get; set; }

        [DataMember]
        public bool? IsSpouseILARequired { get; set; }

        [DataMember]
        public int? LenderMortgagorID { get; set; }

        [DataMember]
        public int? PriorityIndicator { get; set; }

        [DataMember]
        [StringLength(1000)]
        public string SpousalStatement { get; set; }

        [DataMember]
        [StringLength(50)]
        public string SpouseOccupation { get; set; }

        [DataMember]
        [StringLength(2)]
        public string CompanyProvinceOfIncorporation { get; set; }

        [DataMember]
        public int? SourceID { get; set; }

        [DataMember]
        public virtual tblDeal tblDeal { get; set; }

        [DataMember]
        public virtual vw_Deal vw_Deal { get; set; }
    }
}
