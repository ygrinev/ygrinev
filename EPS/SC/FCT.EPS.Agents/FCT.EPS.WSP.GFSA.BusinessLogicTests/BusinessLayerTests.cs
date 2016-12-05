using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.GFSA.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace FCT.EPS.WSP.GFSA.BusinessLogic.Tests
{
    [TestClass()]
    public class BusinessLayerTests
    {
        [TestMethod()]
        public void ProcessFeeStatusTest()
        {
            FCT.EPS.WSP.GFSA.BusinessLogic.BusinessLayer.ProcessFeeStatus(2, "EPSNetTcpEndpoint");
        }
    }
}
