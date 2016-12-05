using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace FCT.EPS.WSP.Resources
{
    public static class Utils
    {
        public static TEnum ParseEnum<TEnum>(
            object objectValue, bool ignorecase = true) where TEnum : struct
        {
            string stringValue = GetString(objectValue);

            TEnum tenumResult = default(TEnum);

            if (!Enum.TryParse<TEnum>(stringValue, ignorecase, out tenumResult))
            {
                throw new Exception(
                    string.Format("UnSupported  Enum Value => {0} for Enum Type => {1}",
                                                        stringValue, typeof(TEnum).Name));
            }

            return tenumResult;
        }

        public static string GetString(object obj)
        {
            return (obj != null) ? obj.ToString().Trim() : string.Empty;
        }

        public static int GetInt(object obj)
        {
            return (obj != null) ? Convert.ToInt32(obj) : 0;
        }

        public static decimal GetDecimal(object obj)
        {
            return (obj != null) ? Convert.ToDecimal(obj) : 0;
        }

        public static double GetDouble(object obj)
        {
            return (obj != null) ? Convert.ToDouble(obj) : 0;
        }

        public static DateTime GetDateTime(object obj)
        {
            return (obj != null) ? Convert.ToDateTime(obj) : DateTime.MinValue;
        }

        public static DateTime? GetNullableDateTime(object obj)
        {
            DateTime? dt = null;
            if (obj != null)
            {
                dt = Convert.ToDateTime(obj);
            }
            return dt;
        }

        /// <summary>
        /// 1 -> A<br/>
        /// 2 -> B<br/>
        /// 3 -> C<br/>
        /// ...
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static string ExcelColumnFromNumber(int column)
        {
            string columnString = "";
            decimal columnNumber = column;
            while (columnNumber > 0)
            {
                decimal currentLetterNumber = (columnNumber - 1) % 26;
                char currentLetter = (char)(currentLetterNumber + 65);
                columnString = currentLetter + columnString;
                columnNumber = (columnNumber - (currentLetterNumber + 1)) / 26;
            }
            return columnString;
        }

        /// <summary>
        /// A -> 1<br/>
        /// B -> 2<br/>
        /// C -> 3<br/>
        /// ...
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public static int NumberFromExcelColumn(string column)
        {
            int retVal = 0;
            string col = column.ToUpper();
            for (int iChar = col.Length - 1; iChar >= 0; iChar--)
            {
                char colPiece = col[iChar];
                int colNum = colPiece - 64;
                retVal = retVal + colNum * (int)Math.Pow(26, col.Length - (iChar + 1));
            }
            return retVal;
        }

        public static string ConvertAndFormat(string FormatString, string Value, string NetType)
        {
            string Resaults = "";
            switch (NetType.ToUpper())
            {
                case "BYTE":
                    Resaults = Convert.ToByte(Value).ToString(FormatString);
                    break;

                case "SBYTE":
                    Resaults = Convert.ToSByte(Value).ToString(FormatString);
                    break;

                case "DECIMAL":
                    Resaults = Convert.ToDecimal(Value).ToString(FormatString);
                    break;

                case "DOUBLE":
                    Resaults = Convert.ToDouble(Value).ToString(FormatString);
                    break;

                case "SINGLE":
                case "FLOAT":
                    Resaults = Convert.ToSingle(Value).ToString(FormatString);
                    break;

                case "INT32":
                case "INT":
                    Resaults = Convert.ToInt32(Value).ToString(FormatString);
                    break;

                case "UINT32":
                case "UINT":
                    Resaults = Convert.ToUInt32(Value).ToString(FormatString);
                    break;

                case "INT64":
                case "LONG":
                    Resaults = Convert.ToInt64(Value).ToString(FormatString);
                    break;

                case "UINT64":
                case "ULONG":
                    Resaults = Convert.ToUInt64(Value).ToString(FormatString);
                    break;

                case "INT16":
                case "SHORT":
                    Resaults = Convert.ToInt16(Value).ToString(FormatString);
                    break;

                case "UINT16":
                case "USHORT":
                    Resaults = Convert.ToUInt16(Value).ToString(FormatString);
                    break;

                case "DATE":
                    Resaults = Convert.ToDateTime(Value).ToString(FormatString);
                    break;

                default:
                    Resaults = Value.ToString();
                    break;
            }

            return Resaults;

        }

        public static string GetLocalIPs()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        public static IList<string> SplitNameField(string stringToSplit, int numberOfCharacterToSplitAt)
        {
            List<string> myStringList = new List<string>(); 
            //Split the tblPaymentRequest.DebtorName into two 40 character fields without cutting names in the middle
            //You can however drop names if they don't fit.
            string AddressLine = string.Empty;
            string theRemainder = string.Empty;
            if (stringToSplit.Length > 40)
            {
                int firstLineCutoff = stringToSplit.LastIndexOf(',', 40);
                if (firstLineCutoff > 0)
                {
                    myStringList.Add(stringToSplit.Substring(0, firstLineCutoff));
                    theRemainder = stringToSplit.Remove(0, firstLineCutoff);
                    if (theRemainder.Substring(0, 1) == ",")
                    {
                        theRemainder = theRemainder.Substring(1, theRemainder.Length - 1).Trim();
                    }
                }
            }
            else
            {
                myStringList.Add(stringToSplit);
                theRemainder = string.Empty;
            }

            if (theRemainder.Length > 0)
            {
                myStringList.AddRange(SplitNameField(theRemainder, numberOfCharacterToSplitAt));
            }

            return myStringList;
        }
    }

}
