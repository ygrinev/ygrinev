using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using FCT.EPS.WSP.GCSA.Implementation;
namespace FCT.EPS.WSP.GetChequeStateAgent.Tests
{
    [ExcludeFromCodeCoverage,TestClass()]
    public class GetChequeStateTests
    {
        [ExcludeFromCodeCoverage, TestCategory("Int"), TestMethod()]
        public void GetChequeState_GetChequeStateTest()
        {
            ChequeStateAgent myGetChequeState = new ChequeStateAgent();
        }
    }
}
