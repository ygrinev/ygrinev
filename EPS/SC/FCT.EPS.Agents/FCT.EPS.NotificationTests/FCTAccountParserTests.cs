using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.Notification;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FCT.EPS.Notification.Tests
{
    [TestClass()]
    public class FCTAccountParserTests
    {
        [TestMethod()]
        public void FCTAccountParser_FCTAccountParserTest()
        {
            FCTAccountParser myFCTAccountParser = new FCTAccountParser(null);
        }

        [TestMethod()]
        public void FCTAccountParser_FCTAccountParserTest1()
        {
            FCTAccountParser myFCTAccountParser = new FCTAccountParser(null,null);
            FCTAccountParser myFCTAccountParser2 = new FCTAccountParser("", null);
            FCTAccountParser myFCTAccountParser3 = new FCTAccountParser(null, "");
            FCTAccountParser myFCTAccountParser4 = new FCTAccountParser("65424", "65465465654");
        }
    }
}
