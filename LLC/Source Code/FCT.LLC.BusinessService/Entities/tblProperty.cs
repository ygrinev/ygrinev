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
    [Table("tblProperty")]
    public partial class tblProperty
    {
        public tblProperty()
        {
            tblPINs = new HashSet<tblPIN>();
            tblBuilderLegalDescriptions = new HashSet<tblBuilderLegalDescription>();
        }

        [DataMember]
        //[Key, ForeignKey("tblBuilderLegalDescription")]
        [Key]
        public int PropertyID { get; set; }

        [DataMember]
        public int DealID { get; set; }

        [DataMember]
        [StringLength(100)]
        public string Address { get; set; }

        [DataMember]
        [StringLength(100)]
        public string City { get; set; }

        [DataMember]
        [StringLength(2)]
        public string Province { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [DataMember]
        [StringLength(15)]
        public string HomePhone { get; set; }

        [DataMember]
        [StringLength(15)]
        public string BusinessPhone { get; set; }

        [DataMember]
        [StringLength(4000)]
        public string LegalDescription { get; set; }

        [DataMember]
        [StringLength(250)]
        public string ARN { get; set; }

        [DataMember]
        public bool? IsCondo { get; set; }

        [DataMember]
        [StringLength(50)]
        public string EstateType { get; set; }

        [DataMember]
        [StringLength(50)]
        public string InstrumentNumber { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? RegistrationDate { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? AmountOfTaxesPaid { get; set; }

        [DataMember]
        public bool? TaxesPaidOnClosing { get; set; }

        [DataMember]
        [StringLength(50)]
        public string PropertyType { get; set; }

        [DataMember]
        public int? NumberOfUnits { get; set; }

        [DataMember]
        [StringLength(50)]
        public string OccupancyType { get; set; }

        [DataMember]
        public bool? IsNewHome { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? AnnualTaxAmount { get; set; }

        [DataMember]
        public bool? IsPrimaryProperty { get; set; }

        [DataMember]
        public bool? IsLenderToCollectPropertyTaxes { get; set; }

        [DataMember]
        [StringLength(100)]
        public string RegistryOffice { get; set; }

        [DataMember]
        [StringLength(50)]
        public string CondoLevel { get; set; }

        [DataMember]
        [StringLength(30)]
        public string CondoUnitNumber { get; set; }

        [DataMember]
        [StringLength(50)]
        public string CondoCorporationNumber { get; set; }

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
        [StringLength(20)]
        public string BookFolioRoll { get; set; }

        [DataMember]
        [StringLength(20)]
        public string PageFrame { get; set; }

        [DataMember]
        [StringLength(50)]
        public string CondoDeclarationRegistrationNumber { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? CondoDeclarationRegistrationDate { get; set; }

        [DataMember]
        [StringLength(100)]
        public string CondoBookNoOfDeclaration { get; set; }

        [DataMember]
        [StringLength(100)]
        public string CondoPageNumberOfDeclaration { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? CondoDeclarationAcceptedDate { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? CondoPlanRegistrationDate { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? CondoDeclarationDate { get; set; }

        [DataMember]
        [StringLength(50)]
        public string CondoPlanNumber { get; set; }

        [DataMember]
        [StringLength(25)]
        public string AssignmentOfRentsRegistrationNumber { get; set; }

        [DataMember]
        public bool? RentAssignment { get; set; }

        [DataMember]
        public int? LenderPropertyID { get; set; }

        [DataMember]
        [StringLength(10)]
        public string MortgagePriority { get; set; }

        [DataMember]
        [StringLength(100)]
        public string OtherEstateTypeDescription { get; set; }

        [DataMember]
        [StringLength(255)]
        public string Municipality { get; set; }

        [DataMember]
        [StringLength(100)]
        public string CondoDeclarationModificationNumber { get; set; }

        [DataMember]
        [StringLength(100)]
        public string JudicialDistrict { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? CondoDeclarationModificationDate { get; set; }

        [DataMember]
        public bool? IsCondominium { get; set; }

        [DataMember]
        public DateTime? AssignmentOfRentsRegistrationDate { get; set; }

        [DataMember]
        public bool? NewHomeWarranty { get; set; }

        [DataMember]
        public DateTime? TaxesPaidToDate { get; set; }

        [DataMember]
        public virtual tblDeal tblDeal { get; set; }

        [DataMember]
        public virtual vw_Deal vw_Deal { get; set; }

        [DataMember]
        public virtual ICollection<tblPIN> tblPINs { get; set; }

        [DataMember]
        public virtual ICollection<tblBuilderLegalDescription> tblBuilderLegalDescriptions { get; set; }
    }
}
