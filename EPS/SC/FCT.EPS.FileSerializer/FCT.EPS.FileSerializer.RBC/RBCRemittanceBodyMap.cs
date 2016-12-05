using FCT.EPS.FileSerializer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.FileSerializer.RBC
{
    public class RBCRemittanceBodyMap
    {
        private static SortedDictionary<int, DataField> FileBodyDictionary;

        public RBCRemittanceBodyMap()
        {
        }

        private static SortedDictionary<int, DataField> InitializeRBCFileBodyMap()
        {
            if (FileBodyDictionary == null)
            {
                FileBodyDictionary = new SortedDictionary<int, DataField>();
                FileBodyDictionary.Add(1, new DataField(string.Empty, "RecordType", TypeCode.Int32, "{0,#:00,FIX#}",2,"#","",""));
                FileBodyDictionary.Add(2, new DataField(string.Empty, "AccountNumber", TypeCode.String, "{0,-#:FIX#}",16,"#","",""));
                FileBodyDictionary.Add(3, new DataField(string.Empty, "Filler1", TypeCode.String, "{0,-#:FIX#}",3,"#","",""));
                FileBodyDictionary.Add(4, new DataField(string.Empty, "PrimaryName", TypeCode.String, "{0,-#:FIX#}",36,"#","",""));
                FileBodyDictionary.Add(5, new DataField(string.Empty, "CoApplicantName", TypeCode.String, "{0,-#:FIX#}",36,"#","",""));
                FileBodyDictionary.Add(6, new DataField(string.Empty, "AddressLine1", TypeCode.String, "{0,-#:FIX#}",40,"#","",""));
                FileBodyDictionary.Add(7, new DataField(string.Empty, "AddressLine2", TypeCode.String, "{0,-#:FIX#}",40,"#","",""));
                FileBodyDictionary.Add(8, new DataField(string.Empty, "AddressLine3", TypeCode.String, "{0,-#:FIX#}",40,"#","",""));
                FileBodyDictionary.Add(9, new DataField(string.Empty, "City", TypeCode.String, "{0,-#:FIX#}",24,"#","",""));
                FileBodyDictionary.Add(10, new DataField(string.Empty, "StateProvince", TypeCode.String, "{0,-#:FIX#}",19,"#","",""));
                FileBodyDictionary.Add(11, new DataField(string.Empty, "ZipPostalCode", TypeCode.String, "{0,-#:FIX#}",13,"#","",""));
                FileBodyDictionary.Add(12, new DataField(string.Empty, "Country", TypeCode.String, "{0,-#:FIX#}",3,"#","",""));
                FileBodyDictionary.Add(13, new DataField(string.Empty, "CreditLineLimit", TypeCode.Int32, "{0:00000000,FIX#}",8,"#","",""));
                FileBodyDictionary.Add(14, new DataField(string.Empty, "LetterNumber", TypeCode.Int32, "{0:000000,FIX#}",6,"#","",""));
                FileBodyDictionary.Add(15, new DataField(string.Empty, "FastCashIndicator", TypeCode.String, "{0,-#:FIX#}",1,"#","",""));
                FileBodyDictionary.Add(16, new DataField(string.Empty, "CCIN", TypeCode.String, "{0,-#:FIX#}",8,"#","",""));
                FileBodyDictionary.Add(17, new DataField(string.Empty, "Filler2", TypeCode.String, "{0,-#:FIX#}",1,"#","",""));
                FileBodyDictionary.Add(18, new DataField(string.Empty, "PayeeName", TypeCode.String, "{0,-#:FIX#}",21,"#","",""));
                FileBodyDictionary.Add(19, new DataField(string.Empty, "CompanyID", TypeCode.String, "{0,-#:FIX#}",10,"#","",""));
                FileBodyDictionary.Add(20, new DataField(string.Empty, "PayeeAddress1", TypeCode.String, "{0,-#:FIX#}",40,"#","",""));
                FileBodyDictionary.Add(21, new DataField(string.Empty, "PayeeAddress2", TypeCode.String, "{0,-#:FIX#}",40,"#","",""));
                FileBodyDictionary.Add(22, new DataField(string.Empty, "PayeeAddress3", TypeCode.String, "{0,-#:FIX#}",40,"#","",""));
                FileBodyDictionary.Add(23, new DataField(string.Empty, "PayeeCity", TypeCode.String, "{0,-#:FIX#}",25,"#","",""));
                FileBodyDictionary.Add(24, new DataField(string.Empty, "PayeeStateProvince", TypeCode.String, "{0,-#:FIX#}",19,"#","",""));
                FileBodyDictionary.Add(25, new DataField(string.Empty, "PayeePostalCode", TypeCode.String, "{0,-#:FIX#}",13,"#","",""));
                FileBodyDictionary.Add(26, new DataField(string.Empty, "PayeeCountry", TypeCode.String, "{0,-#:FIX#}",3,"#","",""));
                FileBodyDictionary.Add(27, new DataField(string.Empty, "PayeeAccountNumber", TypeCode.String, "{0,-#:FIX#}",30,"#","",""));
                FileBodyDictionary.Add(28, new DataField(string.Empty, "PayeeClearingAccountNumber", TypeCode.String, "{0,-#:FIX#}",9,"#","",""));
                FileBodyDictionary.Add(29, new DataField(string.Empty, "RequestedAmount", TypeCode.String, "{0,-#:FIX#}", 9, "#", "", ""));
                FileBodyDictionary.Add(30, new DataField(string.Empty, "BalanceTransferAmount", TypeCode.Int32, "{0:0000000000000,FIX#,RBCDEC}",13,"#","",""));
                FileBodyDictionary.Add(31, new DataField(string.Empty, "OutSortCode", TypeCode.String, "{0,-#:FIX#}",1,"#","",""));
                FileBodyDictionary.Add(32, new DataField(string.Empty, "SourceID", TypeCode.String, "{0,-#:FIX#}",8,"#","",""));
                FileBodyDictionary.Add(33, new DataField(string.Empty, "RequestedDate", TypeCode.DateTime, "{0:%,FIX#}", 8, "#", "yyyyMMdd", "%"));
                FileBodyDictionary.Add(34, new DataField(string.Empty, "ReservationSolicitationNumber", TypeCode.Int32, "{0:0000000000000000,FIX#}",16,"#","",""));
                FileBodyDictionary.Add(35, new DataField(string.Empty, "TandemAuthorizationCode", TypeCode.String, "{0,-#:FIX#}",6,"#","",""));
                FileBodyDictionary.Add(36, new DataField(string.Empty, "PaymentReferenceNumber", TypeCode.String, "{0,-#:FIX#}",19,"#","",""));
                FileBodyDictionary.Add(37, new DataField(string.Empty, "Filler3", TypeCode.String, "{0,-#:FIX#}",7,"#","",""));
                FileBodyDictionary.Add(38, new DataField(string.Empty, "EFTIdentifier", TypeCode.String, "{0,-#:FIX#}",1,"#","",""));
                FileBodyDictionary.Add(39, new DataField(string.Empty, "HomePhone", TypeCode.Int32, "{0:000000000,FIX#}",9,"#","",""));
                FileBodyDictionary.Add(40, new DataField(string.Empty, "BusinessPhone", TypeCode.Int32, "{0:000000000,FIX#}",9,"#","",""));
                FileBodyDictionary.Add(41, new DataField(string.Empty, "PositionLastName", TypeCode.Int32, "{0:000,FIX#}",3,"#","",""));
                FileBodyDictionary.Add(42, new DataField(string.Empty, "PositionFirstName", TypeCode.Int32, "{0:000,FIX#}",3,"#","",""));
                FileBodyDictionary.Add(43, new DataField(string.Empty, "PositionMiddleName", TypeCode.Int32, "{0:000,FIX#}",3,"#","",""));
                FileBodyDictionary.Add(44, new DataField(string.Empty, "PositionSuffix", TypeCode.Int32, "{0:000,FIX#}",3,"#","",""));
                FileBodyDictionary.Add(45, new DataField(string.Empty, "ClientNumber", TypeCode.Int32, "{0:0000,FIX#}",4,"#","",""));
                FileBodyDictionary.Add(46, new DataField(string.Empty, "ApplicationNumber", TypeCode.Int32, "{0:000000000000,FIX#}",12,"#","",""));
                FileBodyDictionary.Add(47, new DataField(string.Empty, "ApplicationSuffix", TypeCode.Int32, "{0:00,FIX#}",2,"#","",""));
                FileBodyDictionary.Add(48, new DataField(string.Empty, "Filler4", TypeCode.String, "{0,-#:FIX#}", 18, "#", "", ""));
            }
            return FileBodyDictionary;
        }


        public static SortedDictionary<int, DataField> GetMap()
        {
            return InitializeRBCFileBodyMap();
        }


    }
    public class RBCRemittanceBodyDataValues : BaseDataValues
    {

        public int RecordType { get; set; }
        [DefaultValue("")]
        public string AccountNumber { get; set; }
        [DefaultValue("")]
        public string Filler1 { get; set; }
        [DefaultValue("")]
        public string PrimaryName { get; set; }
        [DefaultValue("")]
        public string CoApplicantName { get; set; }
        [DefaultValue("")]
        public string AddressLine1 { get; set; }
        [DefaultValue("")]
        public string AddressLine2 { get; set; }
        [DefaultValue("")]
        public string AddressLine3 { get; set; }
        [DefaultValue("")]
        public string City { get; set; }
        [DefaultValue("")]
        public string StateProvince { get; set; }
        [DefaultValue("")]
        public string ZipPostalCode { get; set; }
        [DefaultValue("")]
        public string Country { get; set; }
        public int CreditLineLimit { get; set; }
        public int LetterNumber { get; set; }
        [DefaultValue("")]
        public string FastCashIndicator { get; set; }
        [DefaultValue("")]
        public string CCIN { get; set; }
        [DefaultValue("")]
        public string Filler2 { get; set; }
        [DefaultValue("")]
        public string PayeeName { get; set; }
        [DefaultValue("")]
        public string CompanyID { get; set; }
        [DefaultValue("")]
        public string PayeeAddress1 { get; set; }
        [DefaultValue("")]
        public string PayeeAddress2 { get; set; }
        [DefaultValue("")]
        public string PayeeAddress3 { get; set; }
        [DefaultValue("")]
        public string PayeeCity { get; set; }
        [DefaultValue("")]
        public string PayeeStateProvince { get; set; }
        [DefaultValue("")]
        public string PayeePostalCode { get; set; }
        [DefaultValue("")]
        public string PayeeCountry { get; set; }
        [DefaultValue("")]
        public string PayeeAccountNumber { get; set; }
        [DefaultValue("")]
        public string PayeeClearingAccountNumber { get; set; }
        [DefaultValue("")]
        public string RequestedAmount { get; set; }
        public Int32 BalanceTransferAmount { get; set; }
        [DefaultValue("")]
        public string OutSortCode { get; set; }
        [DefaultValue("")]
        public string SourceID { get; set; }
        public DateTime RequestedDate { get; set; }
        public Int32 ReservationSolicitationNumber { get; set; }
        [DefaultValue("")]
        public string TandemAuthorizationCode { get; set; }
        [DefaultValue("")]
        public string PaymentReferenceNumber { get; set; }
        [DefaultValue("")]
        public string Filler3 { get; set; }
        [DefaultValue("")]
        public string EFTIdentifier { get; set; }
        public int HomePhone { get; set; }
        public int BusinessPhone { get; set; }
        public int PositionLastName { get; set; }
        public int PositionFirstName { get; set; }
        public int PositionMiddleName { get; set; }
        public int PositionSuffix { get; set; }
        public int ClientNumber { get; set; }
        public int ApplicationNumber { get; set; }
        public int ApplicationSuffix { get; set; }
        [DefaultValue("")]
        public string Filler4 { get; set; }

        public RBCRemittanceBodyDataValues()
        { }
    }
    public class RBCRemittanceFileBodyProcessor<T> : BaseProcess<T>
    {
        public RBCRemittanceFileBodyProcessor(T value)
        {
            fileFormatDictionary = RBCRemittanceBodyMap.GetMap();
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
