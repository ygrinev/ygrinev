using FCT.EPS.FileSerializer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.FileSerializer.RBC
{
    public class RBCPayeeListBodyMap
    {
        private static SortedDictionary<int, DataField> FileBodyDictionary;

        public RBCPayeeListBodyMap()
        {
        }

        private static SortedDictionary<int, DataField> InitializeRBCFileBodyMap()
        {
            if (FileBodyDictionary == null)
            {
                FileBodyDictionary = new SortedDictionary<int, DataField>();
                FileBodyDictionary.Add(1, new DataField(string.Empty, "CompanyID", TypeCode.String, "{0,#:FIX#}", 6, "#","",""));
                FileBodyDictionary.Add(2, new DataField(string.Empty, "CompanyName", TypeCode.String, "{0,-#:FIX#}", 35, "#","",""));
                FileBodyDictionary.Add(3, new DataField(string.Empty, "Department", TypeCode.String, "{0:FIX#}", 35, "#","",""));
                FileBodyDictionary.Add(4, new DataField(string.Empty, "Address", TypeCode.String, "{0,-#:FIX#}", 35, "#","",""));
                FileBodyDictionary.Add(5, new DataField(string.Empty, "City", TypeCode.String, "{0,-#:FIX#}", 19, "#","",""));
                FileBodyDictionary.Add(6, new DataField(string.Empty, "Province", TypeCode.String, "{0,-#:FIX#}", 2, "#","",""));
                FileBodyDictionary.Add(7, new DataField(string.Empty, "PostalCode", TypeCode.String, "{0,-#:FIX#}", 7, "#","",""));
                FileBodyDictionary.Add(8, new DataField(string.Empty, "ContactName", TypeCode.String, "{0,-#:FIX#}", 35, "#","",""));
                FileBodyDictionary.Add(9, new DataField(string.Empty, "ContactPhone", TypeCode.String, "{0,-#:FIX#}", 14, "#", "", ""));
                FileBodyDictionary.Add(10, new DataField(string.Empty, "AccountNumberExactLenght", TypeCode.Int32, "{0,-#:FIX#}", 2, "#","",""));
                FileBodyDictionary.Add(11, new DataField(string.Empty, "AccountNumberMinimumLength", TypeCode.Int32, "{0,-#:FIX#}", 2, "#","",""));
                FileBodyDictionary.Add(12, new DataField(string.Empty, "AccountNumberMaximumLength", TypeCode.Int32, "{0,-#:FIX#}", 2, "#","",""));
                FileBodyDictionary.Add(13, new DataField(string.Empty, "EditRulesForAccountNumbers", TypeCode.String, "{0:-#,FIX#}", 10, "#","",""));
                FileBodyDictionary.Add(14, new DataField(string.Empty, "DataTypeFormatForAccountNumbers", TypeCode.String, "{0:FIX#}", 30, "#","",""));
                FileBodyDictionary.Add(15, new DataField(string.Empty, "ValidStartForAccountNumbers", TypeCode.String, "{0,-#:FIX#}", 1000, "#","",""));
                FileBodyDictionary.Add(16, new DataField(string.Empty, "InvalidStartForAccountNumbers", TypeCode.String, "{0,-#:FIX#}", 1000, "#","",""));
                FileBodyDictionary.Add(17, new DataField(string.Empty, "ValidEndForAccountnumbers", TypeCode.String, "{0,-#:FIX#}", 1000, "#","",""));
                FileBodyDictionary.Add(18, new DataField(string.Empty, "InvalidEndForAccountNumbers", TypeCode.String, "{0,-#:FIX#}", 1000, "#","",""));
                FileBodyDictionary.Add(19, new DataField(string.Empty, "Timestamp", TypeCode.DateTime, "{0:%,FIX#}", 26, "#", "yyyy-MM-dd-HH.mm.ss.ffffff","%"));
                FileBodyDictionary.Add(20, new DataField(string.Empty, "CCIN", TypeCode.Int32, "{0,-#:FIX#}", 8, "{0,-#:FIX#}", "", ""));
                FileBodyDictionary.Add(21, new DataField(string.Empty, "CompanyFrenchName", TypeCode.String, "{0,-#:FIX#}", 35, "#","",""));
                FileBodyDictionary.Add(22, new DataField(string.Empty, "CompanyEnglishShortName", TypeCode.String, "{0,-#:FIX#}", 15, "#","",""));
                FileBodyDictionary.Add(23, new DataField(string.Empty, "CompanyFrenchShortName", TypeCode.String, "{0,-#:FIX#}", 15, "#","",""));
                FileBodyDictionary.Add(24, new DataField(string.Empty, "LanguageIndicator", TypeCode.String, "{0,-#:FIX#}", 1, "#","",""));
                FileBodyDictionary.Add(25, new DataField(string.Empty, "EffectiveDate", TypeCode.DateTime, "{0:%,FIX#}", 10, "#", "yyyy-MM-dd","%"));
                FileBodyDictionary.Add(26, new DataField(string.Empty, "CurrentRecord", TypeCode.String, "{0,-#:FIX#}", 1, "#","",""));
                FileBodyDictionary.Add(27, new DataField(string.Empty, "CreditorType", TypeCode.String, "{0,-#:FIX#}", 4, "#","",""));
            }
            return FileBodyDictionary;
        }


        public static SortedDictionary<int, DataField> GetMap()
        {
            return InitializeRBCFileBodyMap();
        }
    }
    public class RBCPayeeListBodyDataValues : BaseDataValues
    {

        [DefaultValue("")]
        public string CompanyID { get; set; }
        [DefaultValue("")]
        public string CompanyName { get; set; }
        [DefaultValue("")]
        public string Department { get; set; }
        [DefaultValue("")]
        public string Address { get; set; }
        [DefaultValue("")]
        public string City { get; set; }
        [DefaultValue("")]
        public string Province { get; set; }
        [DefaultValue("")]
        public string PostalCode { get; set; }
        [DefaultValue("")]
        public string ContactName { get; set; }
        [DefaultValue("")]
        public string ContactPhone { get; set; }
        public int AccountNumberExactLenght { get; set; }
        public int AccountNumberMinimumLength { get; set; }
        public int AccountNumberMaximumLength { get; set; }
        [DefaultValue("")]
        public string EditRulesForAccountNumbers { get; set; }
        [DefaultValue("")]
        public string DataTypeFormatForAccountNumbers { get; set; }
        [DefaultValue("")]
        public string ValidStartForAccountNumbers { get; set; }
        [DefaultValue("")]
        public string InvalidStartForAccountNumbers { get; set; }
        [DefaultValue("")]
        public string ValidEndForAccountnumbers { get; set; }
        [DefaultValue("")]
        public string InvalidEndForAccountNumbers { get; set; }
        public DateTime Timestamp { get; set; }
        public int CCIN { get; set; }
        [DefaultValue("")]
        public string CompanyFrenchName { get; set; }
        [DefaultValue("")]
        public string CompanyEnglishShortName { get; set; }
        [DefaultValue("")]
        public string CompanyFrenchShortName { get; set; }
        [DefaultValue("")]
        public string LanguageIndicator { get; set; }
        public DateTime EffectiveDate { get; set; }
        [DefaultValue("")]
        public string CurrentRecord { get; set; }
        [DefaultValue("")]
        public string CreditorType { get; set; }
        public int NumberRetried { get; set; }


        public RBCPayeeListBodyDataValues()
        {
        }
    }
    public class RBCPayeeListFileBodyProcessor<T> : BaseProcess<T>
    {
        public RBCPayeeListFileBodyProcessor(T value)
        {
            fileFormatDictionary = RBCPayeeListBodyMap.GetMap();
            datavalue = value;
        }

        //public string FormattedDate(DateTime date)
        //{
        //    string formattedDate = string.Empty;

        //    if (date != null)
        //    {
        //        formattedDate = date.ToString("yyyymmdd");
        //    }
        //    return formattedDate;
        //}
    }

}
