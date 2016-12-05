using FCT.EPS.FileSerializer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.FileSerializer.RBC
{
    public class RBCRemittanceFooterMap
    {
        private static SortedDictionary<int, DataField> FileFooterDictionary;

        public RBCRemittanceFooterMap()
        {
        }

        private static SortedDictionary<int, DataField> InitializeRBCFileFooterMap()
        {
            if (FileFooterDictionary == null)
            {
                FileFooterDictionary = new SortedDictionary<int, DataField>();
                FileFooterDictionary.Add(1, new DataField(string.Empty, "RecordType", TypeCode.Int32, "{0,#:00,FIX#}", 2, "#","",""));
                FileFooterDictionary.Add(2, new DataField(string.Empty, "ClientNumber", TypeCode.String, "{0,-#:FIX#}", 4, "#","",""));
                FileFooterDictionary.Add(3, new DataField(string.Empty, "TransmitID", TypeCode.String, "{0,-#:FIX#}", 8, "#","",""));
                FileFooterDictionary.Add(4, new DataField(string.Empty, "TotalNumberOfBalanceTransfers", TypeCode.Int32, "{0:0000000,FIX#}", 7, "#","",""));
                FileFooterDictionary.Add(5, new DataField(string.Empty, "TotalNumberOfFastCashRequests", TypeCode.Int32, "{0:0000000,FIX#}", 7, "#","",""));
                FileFooterDictionary.Add(6, new DataField(string.Empty, "TotalRecords", TypeCode.Int32, "{0:0000000,FIX#}", 7, "#","",""));
                FileFooterDictionary.Add(7, new DataField(string.Empty, "Filler", TypeCode.String, "{0,-#:FIX#}", 665, "#", "", ""));
            }
            return FileFooterDictionary;
        }

        public static SortedDictionary<int, DataField> GetMap()
        {
            return InitializeRBCFileFooterMap();
        }
    }
    public class RBCRemittanceFooterDataValues : BaseDataValues
    {
        public int RecordType { get; set; }
        public string ClientNumber { get; set; }
        [DefaultValue("")]
        public string TransmitID { get; set; }
        public int TotalNumberOfBalanceTransfers { get; set; }
        public int TotalNumberOfFastCashRequests { get; set; }
        public int TotalRecords { get; set; }
        public string Filler { get; set; }

        public RBCRemittanceFooterDataValues()
        { }
    }
    public class RBCRemittanceFileFooterProcessor<T> : BaseProcess<T>
    {
        public RBCRemittanceFileFooterProcessor(T value)
        {
            fileFormatDictionary = RBCRemittanceFooterMap.GetMap();
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
