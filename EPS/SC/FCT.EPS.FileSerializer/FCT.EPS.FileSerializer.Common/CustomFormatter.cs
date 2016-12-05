using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.FileSerializer.Common
{
    public class CustomFormatter : IFormatProvider, ICustomFormatter
    {
        
        // IFormatProvider.GetFormat implementation.
        public object GetFormat(Type formatType)
        {
            // Determine whether custom formatting object is requested.
            if (formatType == typeof(ICustomFormatter))
                return this;
            else
                return null;
        }

        // Format string into upper or lower case using format string of TU for upper and TL for lower.
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            // Handle null or empty format string, string with precision specifier.
            string fullFmt = String.Empty;


            if (!String.IsNullOrEmpty(format))
                fullFmt = format;

            string[] fmts = fullFmt.Split(',');

            foreach (string thisFmt in fmts)
            {
                //Convet to upper
                if (thisFmt.Length > 2 && thisFmt.ToUpper() == "TU")
                {
                    if (arg is IFormattable)
                        arg = ((IFormattable)arg).ToString().ToUpper(CultureInfo.CurrentCulture);
                    else if (arg != null)
                        arg = arg.ToString().ToUpper(CultureInfo.CurrentCulture);
                }
                //Convert to lower
                else if (thisFmt.Length > 2 && thisFmt.ToUpper() == "TL")
                {
                    if (arg is IFormattable)
                        arg = ((IFormattable)arg).ToString().ToLower(CultureInfo.CurrentCulture);
                    else if (arg != null)
                        arg = arg.ToString().ToLower(CultureInfo.CurrentCulture);
                }
                //Convert to title case
                else if (thisFmt.Length > 3 && thisFmt.ToUpper() == "TTC")
                {
                    if (arg is IFormattable)
                        arg = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(((IFormattable)arg).ToString().ToLower());
                    else if (arg != null)
                        arg = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(arg.ToString().ToLower());
                }
                //else if (thisFmt.ToUpper() == "ST")
                //{
                //    if (arg != null)
                //    {
                //        if (Convert.ToDateTime(arg).Day == 1)
                //        {
                //            if (CultureInfo.CurrentCulture.Name == "fr-CA")
                //                arg = "er";
                //            else
                //                arg = "st";
                //        }
                //    }
                //}

                //Convert to a fixed lenght string.
                else if (thisFmt.Length > 3 && thisFmt.ToUpper().Substring(0, 3) == "FIX")
                {
                    if (arg != null)
                    {
                        int maxLength = Convert.ToInt32(thisFmt.Substring(3, thisFmt.Length - 3));
                        string value = arg.ToString();
                        if (value.Length > maxLength)
                        {
                            arg = arg.ToString().Substring(0, maxLength);
                        }
                    }
                }
                else if (thisFmt.Length > 5 && thisFmt.ToUpper().Substring(0, 6) == "RBCDEC")
                {
                    string value = arg.ToString();
                    arg = value.Substring(0, value.Length - 1);
                    switch (value.Substring(value.Length - 1, 1))
                    {
                        case "0":
                            arg = arg + "{";
                            break;
                        case "1":
                            arg = arg + "A";
                            break;
                        case "2":
                            arg = arg + "B";
                            break;
                        case "3":
                            arg = arg + "C";
                            break;
                        case "4":
                            arg = arg + "D";
                            break;
                        case "5":
                            arg = arg + "E";
                            break;
                        case "6":
                            arg = arg + "F";
                            break;
                        case "7":
                            arg = arg + "G";
                            break;
                        case "8":
                            arg = arg + "H";
                            break;
                        case "9":
                            arg = arg + "I";
                            break;
                    }
                }
                else
                {
                    try
                    {
                        arg = HandleOtherFormats(thisFmt, arg);
                    }
                    catch (FormatException e)
                    {
                        throw new FormatException(String.Format("The format of '{0}' is invalid.", format), e);
                    }
                }
            }
            return arg == null?string.Empty:arg.ToString();
        }

        private string HandleOtherFormats(string format, object arg)
        {
            if (arg is IFormattable)
                return ((IFormattable)arg).ToString(format, CultureInfo.CurrentCulture);
            else if (arg != null)
                return arg.ToString();
            else
                return String.Empty;
        }
    }
}
