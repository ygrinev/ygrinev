using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FCT.EPS.FileSerializer.Common
{
    public class BaseProcess<T>  where T : new()
    {
        //mapping template object
        public SortedDictionary<int, DataField> fileFormatDictionary = new SortedDictionary<int, DataField>();
        public T datavalue;

        public BaseProcess()
        {
        }
        public static T DeserializeFromString(string s, bool isFixedLen = true)
        {
            //if (string.IsNullOrEmpty(s))
            //    throw new ArgumentNullException("ParseNValidate(string s, " + typeof(T).Name + " o): String parameter cannot be null or empty!");
            //if (string.IsNullOrEmpty(s))
            //    throw new ArgumentNullException("ParseNValidate(string s, " + typeof(T).Name + " o): Parameter of '" + typeof(T).Name + "' type cannot be null!");
            //int curPos = 0;
            //// rough validation
            //PropertyInfo[] piArr = typeof(T).GetProperties();
            //int len = piArr.Select(pi => typeof(T).GetProperty(pi.Name).GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().Single().MaximumLength).Sum();
            //if (isFixedLen ? s.Length != len : s.Length < len)
            //{
            //    throw new DataMisalignedException("DeserializeFromString(string s, bool isFixedLen = true): String parameter has invalid length [" + s.Length + "], must be [" + len + "]" + (isFixedLen ? "" : " or longer") + "!");
            //}

            // detailed validation, field by field
            T o = new T();
            Type type = o.GetType();
            StringBuilder errors = new StringBuilder();
            PropertyInfo[] piArr = typeof(T).GetProperties();
            int curPos = 0;
            Array.ForEach(piArr, pr =>
            {
                int curLen = typeof(StringLengthAttribute).GetProperty(pr.Name).GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().Single().MaximumLength;
                string value = s.Substring(curPos, curLen);
                curPos += curLen;
                try
                {
                    Validator.ValidateProperty(value, new ValidationContext(o, null, null) { MemberName = pr.Name });
                    type.GetProperty(pr.Name).SetValue(o, value);
                }
                catch (Exception ex)
                {
                    errors.Append(ex.Message + "'\r\n");
                }
            });
            if (errors.Length > 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException(errors.ToString());
            }
            return o;
        }

        public void DeserializeFromString(string fileContent, ref T emptyValue)
        {
            int offset = 0; // - useless var
            // read the file content for decoding the content
            using (TextReader reader = new StringReader(fileContent))
            {
                SortedDictionary<int, DataField> tempMap = new SortedDictionary<int, DataField>();

                //read the content into temp map for further processing
                foreach (var obj in fileFormatDictionary)
                {
                    DataField field = obj.Value;

                    char[] block = new char[field.fieldLength];
                    reader.ReadBlock(block, 0, field.fieldLength);

                    field.fieldValue = new string(block);
                    tempMap.Add(obj.Key, field);

                    offset = field.fieldLength + offset;
                }

                //iterate each value in map and construct MT910DataValues object
                foreach (var obj in tempMap)
                {
                    DataField field = obj.Value;
                    //   Console.WriteLine(field.fieldName + " -> " + field.fieldValue);

                    foreach (PropertyInfo pi in datavalue.GetType().GetProperties())
                    {
                        //   Console.WriteLine(pi.Name + "   " + pi.PropertyType);
                        TypeCode typeCode = Type.GetTypeCode(pi.PropertyType);
                        if ((field.fieldType == typeCode) && (pi.Name == field.fieldName))
                        {
                            // Apply custom type parse for each type
                            switch (typeCode)
                            {
                                case TypeCode.Boolean:
                                    {
                                        bool val = false;
                                        pi.SetValue(datavalue, bool.TryParse(field.fieldValue, out val) || val);
                                    }
                                    break;

                                case TypeCode.Double:
                                    {
                                        Double value;

                                        if (Double.TryParse(field.fieldValue.Trim(), out value))
                                        {
                                            pi.SetValue(datavalue, value);
                                        }
                                        else
                                        {
                                            pi.SetValue(datavalue, null);
                                        }
                                    }
                                    break;

                                case TypeCode.Decimal:
                                    {
                                        Decimal value;

                                        if (Decimal.TryParse(field.fieldValue.Trim(), out value))
                                        {
                                            pi.SetValue(datavalue, value);
                                        }
                                        else
                                        {
                                            pi.SetValue(datavalue, null);
                                        }
                                    }
                                    break;

                                case TypeCode.DateTime:
                                    {
                                        DateTime value;

                                        //   DateTime dt = DateTime.ParseExact(field.fieldValue.Trim(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                                        if (!string.IsNullOrEmpty(field.fieldUnFormater))
                                        {
                                            if (DateTime.TryParseExact(field.fieldValue.Trim(),
                                                                        field.fieldUnFormater,
                                                                        CultureInfo.InvariantCulture,
                                                                        DateTimeStyles.None, out value))
                                            {
                                                pi.SetValue(datavalue, value);
                                            }
                                        }
                                        else if (DateTime.TryParseExact(field.fieldValue.Trim(),
                                                                    "M/d/yyyy",
                                                                    CultureInfo.InvariantCulture,
                                                                    DateTimeStyles.None, out value))
                                        {
                                            pi.SetValue(datavalue, value);
                                        }
                                        else
                                        {
                                            pi.SetValue(datavalue, null);
                                        }
                                    }
                                    break;

                                case TypeCode.String:
                                    {
                                        pi.SetValue(datavalue, field.fieldValue.TrimEnd());
                                    }
                                    break;

                                case TypeCode.Int32:
                                    {
                                        int val;
                                        pi.SetValue(datavalue, Int32.TryParse(field.fieldValue, out val) ? val : 0);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }

            emptyValue = datavalue;

            //  return datavalue;
        }

        public string SerializeToStringFormat()
        {
            StringBuilder sb = new StringBuilder();
            if (fileFormatDictionary == null || fileFormatDictionary.Count == 0)
            {
                throw new Exception("File Format Dictionary is not initialized for " + typeof(T).ToString());
            }
            // get the mapping template
            foreach (var obj in fileFormatDictionary)
            {
                DataField field = obj.Value;

                foreach (PropertyInfo pi in datavalue.GetType().GetProperties())
                {
                    TypeCode typeCode = Type.GetTypeCode(pi.PropertyType);

                    if ((field.fieldType == typeCode) && (pi.Name == field.fieldName))
                    {
                        // Apply custom type format for each type
                        switch (typeCode)
                        {
                            case TypeCode.Boolean:

                                break;

                            case TypeCode.Double:
                                {
                                    Double value = Convert.ToDouble(pi.GetValue(datavalue));
                                    sb.Append(FileHelper.getNormalizedString(value,field.fieldFormatString));//12;
                                }
                                break;

                            case TypeCode.Decimal:
                                {
                                    Decimal value = Convert.ToDecimal(pi.GetValue(datavalue));
                                    sb.Append(FileHelper.getNormalizedString(value, field.fieldFormatString));//12;
                                }
                                break;

                            case TypeCode.DateTime:
                                {
                                    DateTime value = (DateTime)pi.GetValue(datavalue);
                                    sb.Append(FileHelper.getNormalizedString(value, field.fieldFormatString));//12;
                                }
                                break;

                            case TypeCode.String:
                                {
                                    string value = (string)pi.GetValue(datavalue);
                                    sb.Append(FileHelper.getNormalizedString(value, field.fieldFormatString));//12;
                                }
                                break;

                            case TypeCode.Int32:
                                {
                                    Int32 value = (Int32)pi.GetValue(datavalue);
                                    sb.Append(FileHelper.getNormalizedString(value, field.fieldFormatString));//12;
                                }
                                break;

                            default:

                                break;
                        }

                    }
                }
            }

            return sb.ToString();
        }

    }
}
