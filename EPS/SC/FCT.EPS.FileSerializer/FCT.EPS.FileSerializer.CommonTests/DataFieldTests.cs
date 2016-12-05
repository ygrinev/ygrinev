using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.FileSerializer.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FCT.EPS.FileSerializer.Common.Tests
{
    [TestClass()]
    public class DataFieldTests
    {
        [TestMethod()]
        public void DataFieldTest()
        {
            DataField myDataField = new DataField("1", "RecordType", TypeCode.Int32, "{0,#:FIX#}", 2,"#","","");


            string myString = myDataField.fieldFormatString;

            Assert.AreEqual<string>("{0,2:FIX2}", myString);

            Assert.AreEqual<string>("02", myString);

            
        }
    }
}
