using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FCT.EPS.WSP.Resources.Tests
{
    [TestClass()]
    public class UtilsTests
    {
        [TestMethod()]
        public void ExcelColumnFromNumberTest()
        {
            for (int Counter = 1; Counter < 500; Counter++)
            {
                System.Diagnostics.Debug.WriteLine(Utils.ExcelColumnFromNumber(Counter));
            }
        }

        [TestMethod()]
        public void ConvertAndFormatTest()
        {
            string myString = Utils.ConvertAndFormat("#,##0.00", "1000", "DECIMAL");

        }

        [TestMethod()]
        public void SplitNameFieldTest()
        {
            string[] TestStrings = new string[] { "this only", "this,is,a,test, of, the, standard, string, lenght, with, commas, daa, daa, daa, doo, doo", "LastName1ChequePaymentMethodLastName1Sam,LastName2ChequePaymentMethodLastNameSam" };

            foreach(string tempString in TestStrings)
            {
                IList<string> myStrings = Utils.SplitNameField(tempString, 40);
            }
        }
    }
}
