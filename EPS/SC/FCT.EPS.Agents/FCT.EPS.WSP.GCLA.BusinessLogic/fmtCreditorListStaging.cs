using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FCT.EPS.WSP.GCLA.BusinessLogic
{
    public class fmtCreditorListStaging
    {
        // 6, 35, 35, 35, 19, 2, 7, 35, 14, 2, 2, 2, 10, 30, 1000, 1000, 1000, 1000, 26, 8, 35, 15, 15, 1, 10, 1, 4
        [StringLength(6), RegularExpression(@"^\d{6}$", ErrorMessage = "'CompanyID' must be 6-digit string.")]
        public string CompanyID { get; set; }
        [Required, StringLength(35)]
        public string CompanyName { get; set; }
        [StringLength(35)]
        public string Department { get; set; }
        [Required, StringLength(35)]
        public string Address { get; set; }
        [Required, StringLength(19)]
        public string City { get; set; }
        [Required, StringLength(2), RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "'Province' must be 2 capital letters.")]
        public string Province { get; set; }
        [Required, StringLength(7), RegularExpression(@"^[A-Z][0-9][A-Z][ ][0-9][A-Z][0-9]$", ErrorMessage = "'PostalCode' must be formatted as '[A-Z][0-9][A-Z][ ][0-9][A-Z][0-9]'.")]
        public string PostalCode { get; set; }
        [Required, StringLength(35)]
        public string ContactName { get; set; }
        [Required, StringLength(14), RegularExpression(@"^\d{14}$", ErrorMessage = "'ContactPhone' may contain only digits and have length=14.")]
        public string ContactPhone { get; set; }
        [Required, StringLength(2), RegularExpression(@"^[0-9 ]{2}$", ErrorMessage = "'AccountNumberExactLength' must be 2-digit string.")]
        public string AccountNumberExactLength { get; set; }
        [Required, StringLength(2), RegularExpression(@"^[0-9 ]{2}$", ErrorMessage = "'AccountNumberMinLength' must be 2-digit string.")]
        public string AccountNumberMinLength { get; set; }
        [Required, StringLength(2), RegularExpression(@"^[0-9 ]{2}$", ErrorMessage = "'AccountNumberMaxLength' must be 2-digit string.")]
        public string AccountNumberMaxLength { get; set; }
        [StringLength(10)]
        public string AccountNumberEditRules { get; set; }
        [StringLength(30), RegularExpression(@"^[A-Z ]{30}$", ErrorMessage = "'AccountNumberDataTypeFormat' may contain only letters and space.")]
        public string AccountNumberDataTypeFormat { get; set; }
        [StringLength(1000)]
        public string AccountNumberValidStart { get; set; }
        [StringLength(1000)]
        public string AccountNumberInValidStart { get; set; }
        [StringLength(1000)]
        public string AccountNumberValidEnd { get; set; }
        [StringLength(1000)]
        public string AccountNumberInvalidEnd { get; set; }
        [StringLength(26), RegularExpression(@"^(1[0-9]{3}|20[0-9]{2})\-(0[0-9]|1[0-2])\-([0-2][0-9]|3[01])\-([01][0-9]|2[0-4])\.([0-5][0-9]\.){2}\d{6}$", 
            ErrorMessage = "'Timestamp' is invalid.")]
        public string Timestamp { get; set; }
        [StringLength(8), RegularExpression(@"^\d{8}$", ErrorMessage = "'CCIN' must be 8-digit string.")]
        public string CCIN { get; set; }
        [StringLength(35)]
        public string CompanyNameFr { get; set; }
        [StringLength(15)]
        public string CompanyShortNameEn { get; set; }
        [StringLength(15)]
        public string CompanyShortNameFr { get; set; }
        [Required, StringLength(1), RegularExpression(@"^[EF]$", ErrorMessage = "'LanguageIndicator' must be a letter: 'E' or 'F'.")]
        public string LanguageIndicator { get; set; }
        [Required, StringLength(10), RegularExpression(@"^(1[0-9]{3}|20[0-9]{2})\-(0[1-9]|1[0-2])\-([0-2][0-9]|3[01])$", 
            ErrorMessage = "'EffectiveDate' is invalid.")]
        public string EffectiveDate { get; set; }
        [Required, StringLength(1), RegularExpression(@"^[YN]$", ErrorMessage = "'CurrentRecord' must 'Y' or 'N'.")]
        public string CurrentRecord { get; set; }
        [Required, StringLength(4), RegularExpression(@"^(CR[ ]{2}|VSA[ ])$", ErrorMessage = "'CreditorType' must be 'CR  ' or 'VSA ' string of 4 chars.")]
        public string CreditorType { get; set; }
        public fmtCreditorListStaging(string s)
        {
            EFValidator.EFValidator.ParseNValidate<fmtCreditorListStaging>(s, this);
            // adjust Timestamp to .NET type:
            Timestamp = Timestamp.Substring(0,10) + " " + Timestamp.Substring(11,2) + ":" + Timestamp.Substring(14,2) + ":" + Timestamp.Substring(17,9);
        }
    }
}
