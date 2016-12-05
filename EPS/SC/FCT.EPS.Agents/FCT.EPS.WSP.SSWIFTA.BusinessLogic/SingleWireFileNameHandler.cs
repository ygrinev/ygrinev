using System;
using System.Data;
using FCT.EPS.WSP.Resources;

namespace FCT.EPS.WSP.SSWIFTA.BusinessLogic
{
    /** {0}                 {1}                 {2}               {3}
    [PaymentType]_[PaymentTransactionID]_[SolutionName]_[Timestamp:YYYYMMDD_HrMiSe_Mil]
    Example: WIRE_123456-_EasyFund_20150123_103030_123.txt
    * */
    public class SingleWireFileNameHandler
    {
        string _PaymentType;
        string _PaymentTransactionID;
        string _SolutionName;
        string _TimeStamp;

        static string _SingleWireFileNameFormat = "{0}_{1}_{2}_{3}.txt";
        public static string SingleWireFileFolderPath { get; set; }

        public SingleWireFileNameHandler(string PaymentType,
                                         string PaymentTransactionID,
                                         string SolutionName)
        {
            this._PaymentTransactionID = PaymentTransactionID;
            this._PaymentType = PaymentType;
            this._SolutionName = SolutionName;
        }
        
        public string GetSingleWireFileNameWithPath()
        {
            SolutionTraceClass.WriteLineVerbose("Start");

            if (string.IsNullOrWhiteSpace(SingleWireFileFolderPath))
                throw new ApplicationException("Error : Folder path not set for Single Wire File ");

            _TimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss_fff");

            SolutionTraceClass.WriteLineVerbose("End");

            return SingleWireFileFolderPath + "\\" + string.Format(_SingleWireFileNameFormat
                , _PaymentType
                , _PaymentTransactionID
                , _SolutionName
                , _TimeStamp
                );
        }
    }
}
