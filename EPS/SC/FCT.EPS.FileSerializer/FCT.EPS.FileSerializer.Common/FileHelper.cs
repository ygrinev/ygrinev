using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.FileSerializer.Common
{
    public class FileHelper
    {
        public FileHelper()
        {
        }
        public static string GetStringFormatted(object value, string fieldFormat)
        {
            return string.Format(new CustomFormatter(), fieldFormat, value);
        }
        public static string getNormalizedString(object value, string fieldFormat)
        {
            string returnValue = GetStringFormatted(value, fieldFormat);

            return returnValue;
            
        }

    }
}
