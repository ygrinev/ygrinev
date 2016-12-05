using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.FileSerializer.Common
{
    public class DataField
    {
        public DataField(string value, string name, TypeCode type, string formatString, int length, string replaceThisValueWithLengthForFormatString, string deseriliseFormatString, string replaceThisValueWithDeseriliseFormatString)
        {
            fieldName = name;
            fieldValue = value;
            fieldType = type;
            _fieldFormatString = formatString;
            fieldLength = length;
            _replaceThisValueWithLengthForFormatString = replaceThisValueWithLengthForFormatString;
            fieldUnFormater = deseriliseFormatString;
            _replaceThisValueWithDeseriliseFormatString = replaceThisValueWithDeseriliseFormatString;
        }
        public string fieldName;
        public string fieldValue;
        public TypeCode fieldType;
        public int fieldLength;
        private string _fieldFormatString;
        private string _replaceThisValueWithLengthForFormatString;
        public string fieldUnFormater;
        private string _replaceThisValueWithDeseriliseFormatString;

        public string fieldFormatString
        {
            get
            {
                string tempValue = _fieldFormatString;
                if (!string.IsNullOrEmpty(_replaceThisValueWithLengthForFormatString))
                {
                    tempValue = tempValue.Replace(_replaceThisValueWithLengthForFormatString, fieldLength.ToString());
                }
                if (!string.IsNullOrEmpty(_replaceThisValueWithDeseriliseFormatString) && !string.IsNullOrEmpty(tempValue))
                {
                    tempValue = tempValue.Replace(_replaceThisValueWithDeseriliseFormatString, fieldUnFormater);
                }
                return tempValue;
            }
        }
    }
}
