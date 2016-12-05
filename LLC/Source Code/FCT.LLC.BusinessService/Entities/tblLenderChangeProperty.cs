using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.LLC.BusinessService.Entities
{
    [Table("tblLenderChangeProperty")]
    public partial class tblLenderChangeProperty
    {
        public tblLenderChangeProperty()
        {
            tblLenderChangeExistingMortgages = new HashSet<tblLenderChangeExistingMortgage>();
        }

        [Key]
        public int LenderChangePropertyID { get; set; }

        public int LenderChangeID { get; set; }

        public int DealID { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(100)]
        public string City { get; set; }

        [StringLength(2)]
        public string Province { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(15)]
        public string HomePhone { get; set; }

        [StringLength(15)]
        public string BusinessPhone { get; set; }

        [StringLength(4000)]
        public string LegalDescription { get; set; }

        [StringLength(19)]
        public string ARN { get; set; }

        public bool? IsCondo { get; set; }

        [StringLength(50)]
        public string EstateType { get; set; }

        [StringLength(50)]
        public string InstrumentNumber { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? RegistrationDate { get; set; }

        [Column(TypeName = "money")]
        public decimal? AmountOfTaxesPaid { get; set; }

        public bool? TaxesPaidOnClosing { get; set; }

        [StringLength(50)]
        public string PropertyType { get; set; }

        public int? NumberOfUnits { get; set; }

        [StringLength(50)]
        public string OccupancyType { get; set; }

        public bool? IsNewHome { get; set; }

        [StringLength(50)]
        public string AnnualTaxAmount { get; set; }

        public bool? IsPrimaryProperty { get; set; }

        public bool? IsLenderToCollectPropertyTaxes { get; set; }

        [StringLength(50)]
        public string RegistryOffice { get; set; }

        [StringLength(50)]
        public string CondoLevel { get; set; }

        [StringLength(30)]
        public string CondoUnitNumber { get; set; }

        [StringLength(50)]
        public string CondoCorporationNumber { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        [StringLength(25)]
        public string UnitNumber { get; set; }

        [StringLength(25)]
        public string StreetNumber { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(50)]
        public string CondoPlanNumber { get; set; }

        public int? LenderPropertyID { get; set; }

        [StringLength(10)]
        public string MortgagePriority { get; set; }

        [StringLength(20)]
        public string BookFolioRoll { get; set; }

        [StringLength(20)]
        public string PageFrame { get; set; }

        [StringLength(50)]
        public string CondoDeclarationRegistrationNumber { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CondoDeclarationRegistrationDate { get; set; }

        [StringLength(100)]
        public string CondoBookNoOfDeclaration { get; set; }

        [StringLength(100)]
        public string CondoPageNumberOfDeclaration { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CondoDeclarationAcceptedDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CondoPlanRegistrationDate { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CondoDeclarationDate { get; set; }

        [StringLength(25)]
        public string AssignmentOfRentsRegistrationNumber { get; set; }

        public bool? RentAssignment { get; set; }

        [StringLength(100)]
        public string OtherEstateTypeDescription { get; set; }

        [StringLength(255)]
        public string Municipality { get; set; }

        [StringLength(100)]
        public string JudicialDistrict { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CondoDeclarationModificationDate { get; set; }

        public bool? IsCondominium { get; set; }

        public virtual tblDeal tblDeal { get; set; }

        public virtual ICollection<tblLenderChangeExistingMortgage> tblLenderChangeExistingMortgages { get; set; }
    }
}
