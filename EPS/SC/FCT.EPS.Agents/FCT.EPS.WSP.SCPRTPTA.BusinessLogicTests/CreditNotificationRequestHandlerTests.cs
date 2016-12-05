using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.WSP.SCPRTPTA.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCT.EPS.Notification;
namespace FCT.EPS.WSP.SCPRTPTA.BusinessLogic.Tests
{
    [TestClass()]
    public class CreditNotificationRequestHandlerTests
    {
        [TestMethod()]
        public void SubmitCreditPaymentRequestToPaymentTrackerTest()
        {
            PaymentTrackingWebServiceClient myPaymentTrackingWebServiceClient = new PaymentTrackingWebServiceClient("wsHttpEndpoint_PaymentTrackingService");
            CreditNotificationRequestHandler myCreditNotificationRequestHandler = new CreditNotificationRequestHandler(myPaymentTrackingWebServiceClient);

            myCreditNotificationRequestHandler.SubmitCreditPaymentRequestToPaymentTracker(5, 5);

            Assert.Fail();
        }
    }
}
