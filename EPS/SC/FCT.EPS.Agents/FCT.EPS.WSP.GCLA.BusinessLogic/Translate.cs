using FCT.EPS.DataEntities;
using FCT.EPS.FileSerializer.RBC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.GCLA.BusinessLogic
{
    class Translate
    {
        public tblRBCCreditorListStaging this[RBCPayeeListBodyDataValues o]
        {
            get
            {
                return new tblRBCCreditorListStaging
                {
                    AccountNumberDataTypeFormat = o.DataTypeFormatForAccountNumbers,
                    AccountNumberEditRules = o.EditRulesForAccountNumbers,
                    AccountNumberExactLength = o.AccountNumberExactLenght,
                    AccountNumberInvalidEnd = o.InvalidEndForAccountNumbers,
                    AccountNumberInValidStart = o.InvalidStartForAccountNumbers,
                    AccountNumberMaxLength = o.AccountNumberMaximumLength,
                    AccountNumberMinLength = o.AccountNumberMinimumLength,
                    AccountNumberValidEnd = o.ValidEndForAccountnumbers,
                    AccountNumberValidStart = o.ValidStartForAccountNumbers,
                    Address = o.Address,
                    CCIN = o.CCIN == 0 ? null : (int?)o.CCIN,
                    City = o.City,
                    CompanyID = o.CompanyID,
                    CompanyName = o.CompanyName,
                    CompanyNameFr = o.CompanyFrenchName,
                    CompanyShortNameEn = o.CompanyEnglishShortName,
                    CompanyShortNameFr = o.CompanyFrenchShortName,
                    ContactName = o.ContactName,
                    ContactPhone = o.ContactPhone,
                    CreditorType = o.CreditorType,
                    CurrentRecord = o.CurrentRecord,
                    Department = o.Department,
                    EffectiveDate = o.EffectiveDate,
                    LanguageIndicator = o.LanguageIndicator,
                    LastModifiedDate = DateTime.Now,
                    PostalCode = o.PostalCode,
                    Province = o.Province,
                    Timestamp = o.Timestamp,
                    NumberRetried = o.NumberRetried
                };
            }
        }
        public tblRBCCreditorListStaging this[string s]
        {
            get
            {
                var src = new fmtCreditorListStaging(s); // validated in ctor
                DateTime dt;
                bool isValidDT = DateTime.TryParse(src.Timestamp, out dt);
                return new tblRBCCreditorListStaging
                {
                    AccountNumberDataTypeFormat = src.AccountNumberDataTypeFormat,
                    AccountNumberEditRules = src.AccountNumberEditRules,
                    AccountNumberExactLength = int.Parse(src.AccountNumberExactLength),
                    AccountNumberInvalidEnd = src.AccountNumberInvalidEnd,
                    AccountNumberInValidStart = src.AccountNumberInValidStart,
                    AccountNumberMaxLength = int.Parse(src.AccountNumberMaxLength),
                    AccountNumberMinLength = int.Parse(src.AccountNumberMinLength),
                    AccountNumberValidEnd = src.AccountNumberValidEnd,
                    AccountNumberValidStart = src.AccountNumberValidStart,
                    Address = src.Address,
                    CCIN = string.IsNullOrEmpty(src.CCIN) ? null : (int?)int.Parse(src.CCIN),
                    City = src.City,
                    CompanyID = src.CompanyID,
                    CompanyName = src.CompanyName,
                    CompanyNameFr = src.CompanyNameFr,
                    CompanyShortNameEn = src.CompanyShortNameEn,
                    CompanyShortNameFr = src.CompanyShortNameFr,
                    ContactName = src.ContactName,
                    ContactPhone = src.ContactPhone,
                    CreditorType = src.CreditorType,
                    CurrentRecord = src.CurrentRecord,
                    Department = src.Department,
                    EffectiveDate = DateTime.Parse(src.EffectiveDate),
                    LanguageIndicator = src.LanguageIndicator,
                    LastModifiedDate = DateTime.Now,
                    PostalCode = src.PostalCode,
                    Province = src.Province,
                    Timestamp = DateTime.Parse(src.Timestamp),
                    NumberRetried = 0
                };
            }
        }

        internal static void UpdateRow(tblRBCCreditorListStaging src, tblRBCCreditorListStaging rowOld)
        {
            rowOld.CompanyID = src.CompanyID;
            rowOld.CompanyName = src.CompanyName;
            rowOld.Department = src.Department;
            rowOld.Address = src.Address;
            rowOld.City = src.City;
            rowOld.Province = src.Province;
            rowOld.PostalCode = src.PostalCode;
            rowOld.ContactName = src.ContactName;
            rowOld.ContactPhone = src.ContactPhone;
            rowOld.AccountNumberExactLength = src.AccountNumberExactLength;
            rowOld.AccountNumberMinLength = src.AccountNumberMinLength;
            rowOld.AccountNumberMaxLength = src.AccountNumberMaxLength;
            rowOld.AccountNumberEditRules = src.AccountNumberEditRules;
            rowOld.AccountNumberDataTypeFormat = src.AccountNumberDataTypeFormat;
            rowOld.AccountNumberValidStart = src.AccountNumberValidStart;
            rowOld.AccountNumberInValidStart = src.AccountNumberInValidStart;
            rowOld.AccountNumberValidEnd = src.AccountNumberValidEnd;
            rowOld.AccountNumberInvalidEnd = src.AccountNumberInvalidEnd;
            rowOld.Timestamp = src.Timestamp;
            rowOld.CCIN = src.CCIN;
            rowOld.CompanyNameFr = src.CompanyNameFr;
            rowOld.CompanyShortNameEn = src.CompanyShortNameEn;
            rowOld.CompanyShortNameFr = src.CompanyShortNameFr;
            rowOld.LanguageIndicator = src.LanguageIndicator;
            rowOld.EffectiveDate = src.EffectiveDate;
            rowOld.CurrentRecord = src.CurrentRecord;
            rowOld.CreditorType = src.CreditorType;
            rowOld.LastModifiedDate = src.LastModifiedDate;
            rowOld.NumberRetried = src.NumberRetried;
        }
    }
}
