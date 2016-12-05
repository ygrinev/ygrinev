using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    [DataContract]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [Table("tblMortgage")]
    public partial class tblMortgage
    {
        [DataMember]
        [Key]
        public int MortgageID { get; set; }

        [DataMember]
        public int DealID { get; set; }

        [DataMember]
        [StringLength(50)]
        public string MortgageNumber { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? MortgageAmount { get; set; }

        [DataMember]
        public int? MortgageTerm { get; set; }

        [DataMember]
        [StringLength(50)]
        public string MortgageType { get; set; }

        [DataMember]
        [StringLength(50)]
        public string TransactionType { get; set; }

        [DataMember]
        public int? MortgageProductID { get; set; }

        [DataMember]
        public decimal? InterestRate { get; set; }

        [DataMember]
        public decimal? MaximumRate { get; set; }

        [DataMember]
        public float? PrimeRatePlusMinus { get; set; }

        [DataMember]
        public decimal? BaseRate { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? MaximumAmount { get; set; }

        [DataMember]
        [StringLength(50)]
        public string CalculationPeriod { get; set; }

        [DataMember]
        public DateTime? InterestAdjustDate { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? AgreementDate { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? AcceptanceDate { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? LastPaymentDate { get; set; }

        [DataMember]
        public DateTime? MaturityDate { get; set; }

        [DataMember]
        public int? AmortizationPeriod { get; set; }

        [DataMember]
        public bool? IsFundingRequired { get; set; }

        [DataMember]
        public DateTime? ClosingDate { get; set; }

        [DataMember]
        public DateTime? Lender_ClosingDate { get; set; }

        [DataMember]
        [StringLength(100)]
        public string StandardChargeTerm { get; set; }

        [DataMember]
        [StringLength(150)]
        public string PaymentDay { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? InterestAdjustAmount { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? BonusDiscountAmount { get; set; }

        [DataMember]
        public float? ResidentialMortgageRate { get; set; }

        [DataMember]
        public float? LineOfCreditRate { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? NetAdvance { get; set; }

        [DataMember]
        public bool? ConstructionMortgage { get; set; }

        [DataMember]
        public bool? AssignmentOfRents { get; set; }

        [DataMember]
        [StringLength(100)]
        public string BrokerName { get; set; }

        [DataMember]
        [StringLength(20)]
        public string BrokerPhone { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? RegisteredAmount { get; set; }

        [DataMember]
        [Column(TypeName = "smalldatetime")]
        public DateTime? OldClosingDate { get; set; }

        [DataMember]
        [StringLength(100)]
        public string MortgageInsurer { get; set; }

        [DataMember]
        public bool? IsAccelerated { get; set; }

        [DataMember]
        [StringLength(50)]
        public string InterestRateType { get; set; }

        [DataMember]
        public decimal? EquivalentRate { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? CashbackAmount { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? PurchasePrice { get; set; }

        [DataMember]
        public int? LenderAddressForMortgageID { get; set; }

        [DataMember]
        public int? LenderAddressForMailingID { get; set; }

        [DataMember]
        public DateTime? DateOfLoanApplication { get; set; }

        [DataMember]
        public decimal? IncrementAboveBelowPrime { get; set; }

        [DataMember]
        public decimal? ActualMortgageRate { get; set; }

        [DataMember]
        public int? AddressForServiceForMortagagorID { get; set; }

        [DataMember]
        public int? MortgageSpecialistID { get; set; }

        [DataMember]
        public int? DealTrustAccountID { get; set; }

        [DataMember]
        public int? LenderChangeID { get; set; }

        [DataMember]
        public DateTime? RateExpiryDate { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? MonthlyPayment { get; set; }

        [DataMember]
        public bool? IsMortgageMailAddress { get; set; }

        [DataMember]
        public bool? IsMortgageServiceAddress { get; set; }

        [DataMember]
        public int? LenderAddressForServiceForMortagagorID { get; set; }

        [DataMember]
        public int? RegistrationPaymentFrequencyTypeID { get; set; }

        [DataMember]
        public decimal? EarlyPaymentAmount { get; set; }

        [DataMember]
        public int? RatioIndicatorTypeID { get; set; }

        [DataMember]
        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] LastModified { get; set; }

        [DataMember]
        public int? MortgageeFINumberID { get; set; }

        [DataMember]
        public int? CalculationPeriodID { get; set; }

        [DataMember]
        public decimal? IncrementAboveBelowPrimeInstruction { get; set; }

        [DataMember]
        [Column(TypeName = "money")]
        public decimal? MortgageAmountAdvanced { get; set; }

        [DataMember]
        public virtual tblDeal tblDeal { get; set; }

        //[DataMember]
        //public virtual tblFinancialInstitutionNumber tblFinancialInstitutionNumber { get; set; }
    }
}
