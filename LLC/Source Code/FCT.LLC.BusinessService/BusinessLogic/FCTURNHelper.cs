using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class FCTURNHelper
    {
        public const int WireDepositCodeLength = 5;

        /// <summary>
        /// Formats short FCT URN (digits to ##-###-######)
        /// </summary>
        /// <param name="shortFCTURN"></param>
        /// <returns></returns>
        public static string FormatShortFCTURN(string shortFCTURN)
        {

            if (string.IsNullOrEmpty(shortFCTURN)) return string.Empty;

            string formattedURN = shortFCTURN;

            int first5digits = Convert.ToInt16(shortFCTURN.Substring(0, 5)); //Grab first 5 digits
            string remainingDigits = shortFCTURN.Substring(5); //Get the digits starting from 6th character

            if (string.IsNullOrWhiteSpace(remainingDigits))
            {
                formattedURN = first5digits.ToString("##-###");
            }
            else
            {
                formattedURN = first5digits.ToString("##-###") + "-" + remainingDigits;
                // Format first 5 digits and concatenate the remainig with -
            }

            return formattedURN;
        }

        /// <summary>
        /// Unformats formatted FCT URN to digits (Removes hyphens
        /// </summary>
        /// <param name="shortFCTURN"></param>
        /// <returns></returns>
        public static string UnFormatShortFCTURN(string shortFCTURN)
        {

            if (string.IsNullOrEmpty(shortFCTURN)) return string.Empty;

            string unformattedURN = shortFCTURN.Replace("-", "");

            return unformattedURN;
        }

        public static string GenerateWireDepositCode()
        {
            var random = new Random((int) DateTime.Now.Ticks);
            //refer to ASCII table
            var vowels = new[] {65, 69, 73, 79, 85};

            var arr = new char[WireDepositCodeLength];
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0 || i%2 == 0)
                {
                    arr[i] = (char)(('0') + random.Next(2, 9));                   
                }
                else
                {
                    int ascii = Convert.ToInt32(Math.Floor(26*random.NextDouble() + 65));
                    if (vowels.Any(v => v.Equals(ascii)))
                    {
                        arr[i] = Convert.ToChar(ascii+1); 
                    }
                    else
                    {
                        arr[i] = Convert.ToChar(ascii);  
                    }                   
                    
                }
            }

           return new string(arr);
        }
    }


}
