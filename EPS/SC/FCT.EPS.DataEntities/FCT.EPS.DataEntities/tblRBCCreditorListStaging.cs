namespace FCT.EPS.DataEntities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tblRBCCreditorListStaging")]
    public partial class tblRBCCreditorListStaging
    {
        [Key, Column(Order = 1), StringLength(6), RegularExpression(@"^\d{6}$", ErrorMessage = "'CompanyID' must be 6-digit string.")]
        public string CompanyID { get; set; }
        [Required, Column(Order = 2), StringLength(35)]
        public string CompanyName { get; set; }
        [Column(Order = 3), StringLength(35)]
        public string Department { get; set; }
        [Required, Column(Order = 4), StringLength(35)]
        public string Address { get; set; }
        [Required, Column(Order = 5), StringLength(19)]
        public string City { get; set; }
        [Required, Column(Order = 6), StringLength(2), RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "'Province' must be 2 capital letters.")]
        public string Province { get; set; }
        [Required, Column(Order = 7), StringLength(7), RegularExpression(@"^[A-Z][0-9][A-Z][ ][0-9][A-Z][0-9]$", ErrorMessage = "'PostalCode' must be formatted as '[A-Z][0-9][A-Z][ ][0-9][A-Z][0-9]'.")]
        public string PostalCode { get; set; }
        [Required, Column(Order = 8), StringLength(35)]
        public string ContactName { get; set; }
        [Required, Column(Order = 9), StringLength(14), RegularExpression(@"^\d{14}$", ErrorMessage = "'ContactPhone' must only contain 14 digits.")]
        public string ContactPhone { get; set; }
        public int AccountNumberExactLength 
        { 
            get 
            { 
                return int.Parse(strAccountNumberExactLength); 
            } 
            set 
            { 
                strAccountNumberExactLength = value.ToString("D2"); 
            } 
        }
        public int AccountNumberMinLength 
        { 
            get 
            { 
                return int.Parse(strAccountNumberMinLength); 
            } 
            set 
            { 
                strAccountNumberMinLength = value.ToString("D2"); 
            } 
        }
        public int AccountNumberMaxLength 
        { 
            get 
            { 
                return int.Parse(strAccountNumberMaxLength); 
            } 
            set 
            { 
                strAccountNumberMaxLength = value.ToString("D2"); 
            } 
        }
        [NotMapped, Required, Column(Order = 10), StringLength(2), RegularExpression(@"^[0-9 ]{2}$", ErrorMessage = "'AccountNumberExactLength' must be 2-digit string.")]
        public string strAccountNumberExactLength { get; set; }
        [NotMapped, Required, Column(Order = 11), StringLength(2), RegularExpression(@"^[0-9 ]{2}$", ErrorMessage = "'AccountNumberMinLength' must be 2-digit string.")]
        public string strAccountNumberMinLength { get; set; }
        [NotMapped, Required, Column(Order = 12), StringLength(2), RegularExpression(@"^[0-9 ]{2}$", ErrorMessage = "'AccountNumberMaxLength' must be 2-digit string.")]
        public string strAccountNumberMaxLength { get; set; }
        [Column(Order = 13), StringLength(10)]
        public string AccountNumberEditRules { get; set; }
        [Column(Order = 14), StringLength(30), RegularExpression(@"^[A-Z ]{30}$", ErrorMessage = "'AccountNumberDataTypeFormat' may contain only letters and space.")]
        public string AccountNumberDataTypeFormat { get; set; }
        [Column(Order = 15), StringLength(1000)]
        public string AccountNumberValidStart { get; set; }
        [Column(Order = 16), StringLength(1000)]
        public string AccountNumberInValidStart { get; set; }
        [Column(Order = 17), StringLength(1000)]
        public string AccountNumberValidEnd { get; set; }
        [Column(Order = 18), StringLength(1000)]
        public string AccountNumberInvalidEnd { get; set; }
        public DateTime Timestamp 
        { 
            get { return DateTime.Parse(strTimestamp.Substring(0, 10) + " " + strTimestamp.Substring(11, 2) + ":" + strTimestamp.Substring(14, 2) + ":" + strTimestamp.Substring(17, 9)); }
            set { strTimestamp = value.ToString("yyyy-MM-dd-HH.mm.ss.ffffff"); } 
        }
        [NotMapped, Column(Order = 19), StringLength(26), RegularExpression(@"^(1[0-9]{3}|20[0-9]{2})\-(0[0-9]|1[0-2])\-([0-2][0-9]|3[01])\-([01][0-9]|2[0-4])\.([0-5][0-9]\.){2}\d{6}$",
            ErrorMessage = "'Timestamp' is invalid.")]
        public string strTimestamp { get; set; }
        public int? CCIN
        {
            get { return string.IsNullOrEmpty((strCCIN ?? "").Trim()) ? null : (int?)int.Parse(strCCIN); }
            set { strCCIN = value == null ? null : ((int)value).ToString("D8"); }
        }
        [NotMapped, Column(Order = 20), StringLength(8), RegularExpression(@"^\d{8}$", ErrorMessage = "'CCIN' must be 8-digit string.")]
        public string strCCIN { get; set; }

        [Column(Order = 21), StringLength(35)]
        public string CompanyNameFr { get; set; }
        [Column(Order = 22), StringLength(15)]
        public string CompanyShortNameEn { get; set; }
        [Column(Order = 23), StringLength(15)]
        public string CompanyShortNameFr { get; set; }
        [Required, Column(Order = 24), StringLength(1), RegularExpression(@"^[EF]$", ErrorMessage = "'LanguageIndicator' must be a letter: 'E' or 'F'.")]
        public string LanguageIndicator { get; set; }
        public DateTime EffectiveDate { get { return DateTime.Parse(strEffectiveDate); } set { strEffectiveDate = value.ToString("yyyy-MM-dd"); } }
        [NotMapped, Required, Column(Order = 25), StringLength(10), RegularExpression(@"^(1[0-9]{3}|20[0-9]{2})\-(0[1-9]|1[0-2])\-([0-2][0-9]|3[01])$",
            ErrorMessage = "'EffectiveDate' is invalid.")]
        public string strEffectiveDate { get; set; }
        [Required, Column(Order = 26), StringLength(1), RegularExpression(@"^[YN]$", ErrorMessage = "'CurrentRecord' must 'Y' or 'N'.")]
        public string CurrentRecord { get; set; }
        [Required, Column(Order = 27), StringLength(4), RegularExpression(@"^(CR[ ]{2}|VSA[ ])$", ErrorMessage = "'CreditorType' must be 'CR  ' or 'VSA ' string of 4 chars.")]
        public string CreditorType { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int NumberRetried { get; set; }
        public tblRBCCreditorListStaging() { }
        public tblRBCCreditorListStaging(string s)
        {
            FCT.EPS.FileSerializer.Common.EFValidator.ParseNValidate<tblRBCCreditorListStaging>(s, this);
            LastModifiedDate = DateTime.Now;
        }
        //[Required]
        //public virtual tblCreditorList tblCreditorList { get; set; }
        //[Required]
        //public virtual tblCreditorRules tblCreditorRules { get; set; }
        //[Required]
        //public virtual tblPayeeInfo tblPayeeInfo { get; set; }
        //[Required]
        //public virtual tblPaymentAddress tblPaymentAddress { get; set; }
    }
}