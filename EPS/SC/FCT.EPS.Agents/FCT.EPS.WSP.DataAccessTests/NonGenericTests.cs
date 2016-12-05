using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCT.EPS.DataEntities;

namespace FCT.EPS.WSP.DataAccess.Tests
{
    [TestClass()]
    public class NonGenericTests
    {
        [TestMethod()]
        public void NonGeneric_GetListOfPaymentRequestsForFinanceTest()
        {
            NonGeneric myNonGeneric = new NonGeneric();
            IList<tblPaymentRequest> myList = myNonGeneric.GetListOfFeePaymentRequestsForFinance(0, 2);

            Assert.IsNotNull(myList);
            Assert.IsNotNull(myList[0].tblFCTFeeSummaryRequest);




        }

    }
}
