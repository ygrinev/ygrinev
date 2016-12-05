using FCT.EPS.FileSerializer.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.FileSerializer.RBC
{
    public class RBCRemittanceHeaderMap
    {
        private static SortedDictionary<int, DataField> FileHeaderDictionary;

        public RBCRemittanceHeaderMap()
        {
        }

        private static SortedDictionary<int, DataField> InitializeRBCFileHeaderMap()
        {
            if (FileHeaderDictionary == null)
            {
                FileHeaderDictionary = new SortedDictionary<int, DataField>();
                FileHeaderDictionary.Add(1, new DataField(string.Empty, "RecordType", TypeCode.Int32, "{0,#:00,FIX#}",2,"#","",""));
                FileHeaderDictionary.Add(2, new DataField(string.Empty, "ClientNumber", TypeCode.String, "{0,-#:FIX#}",4,"#","",""));
                FileHeaderDictionary.Add(3, new DataField(string.Empty, "TransmitID", TypeCode.String, "{0,-#:FIX#}", 8, "#","",""));
                FileHeaderDictionary.Add(4, new DataField(string.Empty, "TransmissionDate", TypeCode.DateTime, "{0,-#:%,FIX#}", 8, "#", "yyyyMMdd", "%"));
                FileHeaderDictionary.Add(5, new DataField(string.Empty, "SequenceNumber", TypeCode.Int32, "{0:000,FIX#}", 3, "#", "", ""));
                FileHeaderDictionary.Add(6, new DataField(string.Empty, "Filler", TypeCode.String, "{0,-#:FIX#}", 675, "#", "", ""));
            }
            return FileHeaderDictionary;
        }

        public static SortedDictionary<int, DataField> GetMap()
        {
            return InitializeRBCFileHeaderMap();
        }
    }
    public class RBCRemittanceHeaderDataValues : BaseDataValues
    {
        public int RecordType { get; set; }
        public string ClientNumber { get; set; }
        [DefaultValue("")]
        public string TransmitID { get; set; }
        public DateTime TransmissionDate { get; set; }
        public int SequenceNumber { get; set; }
        [DefaultValue("")]
        public string Filler { get; set; }


        public RBCRemittanceHeaderDataValues()
        {
        }
    }
    public class RBCRemittanceFileHeaderProcessor<T> : BaseProcess<T>
    {
        public RBCRemittanceFileHeaderProcessor(T value)
        {
            fileFormatDictionary = RBCRemittanceHeaderMap.GetMap();
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
