using System;
using FCT.EPS.WSP.Resources;
using System.Text.RegularExpressions;

namespace FCT.EPS.Notification
{
    public class FCTAccountParser
    {
        const byte TransitNumberFieldLength = 4;

        const byte TotalFieldCount = 2;

        private string TransitNumberField; //4

        private string AccountNumberField; //7

        //[TransitNumber][AccountNumber], 
        static string _FCTTrustAccountFormat = "{0}{1}";

        //[TransitNumber=4 digits][AccountNumber=one or more number of  digits], 
        static string _FCTTrustAccountPattern = "^([0-9]{4})([0-9]+)$";

        //4 + * 
        public FCTAccountParser(string FCTTrustAccount)
        {
            if(string.IsNullOrEmpty(FCTTrustAccount))
            {
                FCTTrustAccount = "";
            }

            Match m = Regex.Match(FCTTrustAccount, _FCTTrustAccountPattern);
            if (m.Success)
            {
                // check again
                if (m.Groups.Count != TotalFieldCount + 1 ||
                !m.Groups[1].Success ||
                !m.Groups[2].Success ||

                m.Groups[1].Length != TransitNumberFieldLength
               )
                {
                    throw new Exception("FCTAccountParser : Invalid FCTTrustAccount # " + Utils.GetString(FCTTrustAccount) + " # : expecting [TransitNumber=4 digits][AccountNumber=one or more number of  digits]");
                }

                TransitNumberField = m.Groups[1].Value;
                AccountNumberField = m.Groups[2].Value;
            }
            else
            {
                throw new Exception("FCTAccountParser : Invalid FCTTrustAccount # " + Utils.GetString(FCTTrustAccount) + " # : expecting [TransitNumber=4 digits][AccountNumber=one or more number of  digits]");

            }
        }


        public FCTAccountParser(string transitNumber, string accountNumber)
        {
            if (string.IsNullOrEmpty(transitNumber))
            {
                transitNumber = "";
            }

            Match m = Regex.Match(transitNumber, "^[0-9]{4}$");
            if (!m.Success)
            {
                throw new Exception("FCTAccountParser : Invalid TransitNumber # " + Utils.GetString(transitNumber) + " #: expecting [TransitNumber=4 digits]");
            }

            m = Regex.Match(accountNumber, "^[0-9]+$");
            if (!m.Success)
            {
                throw new Exception("FCTAccountParser : Invalid AccountNumber # " + Utils.GetString(accountNumber) + " #: expecting [AccountNumber=one or more number of  digits]");
            }

            TransitNumberField = transitNumber;
            AccountNumberField = accountNumber;
        }

        public string TransitNumber
        {
            get
            {
                return this.TransitNumberField;
            }
            set
            {
                this.TransitNumberField = value;
            }
        }

        public string AccountNumber
        {
            get
            {
                return this.AccountNumberField;
            }
            set
            {
                this.AccountNumberField = value;
            }
        }

        public override string ToString()
        {
            //[TransitNumber][AccountNumber], 
            return string.Format(_FCTTrustAccountFormat, TransitNumberField, AccountNumberField);
        }
    }
}
