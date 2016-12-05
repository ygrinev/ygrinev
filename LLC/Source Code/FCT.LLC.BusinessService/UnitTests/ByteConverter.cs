using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.LLC.BusinessService.UnitTests
{
    internal class ByteConverter
    {
        internal static byte[] StringToByteArray(string rowVersion)
        {
            if (rowVersion.Length != 18)
            {
                throw new Exception();
            }
            byte[] retVal = new byte[8];

            for (int index = 2; index < 18; index += 2)
            {
                retVal[(index / 2) - 1] =
                (byte)int.Parse(
                rowVersion.Substring(index, 2),

                System.Globalization.NumberStyles.HexNumber);
            }

            return retVal;
        }
    }
}
