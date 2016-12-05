using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PayeeService.DataContracts;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.Common.DataContracts;
using Moq;
using NUnit.Framework;
using UserContext = FCT.LLC.Common.DataContracts.UserContext;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class PaymentRequestConverterTests
    {
        [Test]
        public void ConvertToPaymentRequest_RegularDisbursementsToEPSRequest()
        {
            //ARRANGE
            var disbursements = new DisbursementCollection
            {
                new Disbursement()
                {
                    PayeeName = "test bank",
                    PayeeID = 123,
                    Amount = 500,
                    PaymentMethod = "WIRE",
                    DisbursementID = 10,
                    PayeeType = "Mortgagee"
                },
                new Disbursement()
                {
                    PayeeName = "test builder",
                    PayeeID = 121,
                    Amount = 100,
                    PaymentMethod = "CHEQUE",
                    DisbursementID = 11,
                    PayeeType = "Builder"
                }
            };
            var fundingDeal = new FundingDeal()
            {
                DealID = 1,
                FCTURN = "151413",
                BusinessModel = BusinessModel.EASYFUND,
                ActingFor = LawyerActingFor.Both
            };

            var payeeInfo = new EPS.PayeeService.DataContracts.PayeeInfo()
            {
                PayeeAccount = new FCT.EPS.PayeeService.DataContracts.Account()
                {
                    AccountNumber = "121212",
                    BankAddress = new FCT.EPS.PayeeService.DataContracts.Address()
                    {
                        City = "test city",
                        StreetAddress1 = "111 Test St",
                        ProvinceCode = "ON"
                    },
                    BankNumber = "101",
                    TransitNumber = "11111"
                },
                PayeeRequestTypeName = "WIRE",
                PayeeBankAccountHolderName = "Test Bank Holdings Inc.",
                BatchScheduleTimeList =
                    new FCT.EPS.PayeeService.DataContracts.BatchScheduleTimeList() {new FCT.EPS.PayeeService.DataContracts.BatchScheduleTime() {BatchScheduleTimeInfo = new DateTime()}},
            };
            var payeeResult = new GetPayeeResult()
            {
                PayeeInfo = payeeInfo
            };
            var argsfactory = new ArgumentsFactory(new MockRepository(MockBehavior.Default));
            argsfactory.DealScopeMock.Setup(d => d.GetFCTRefNumber("151413")).Returns("15014013");
            argsfactory.PayeeServiceMock.Setup(p => p.GetPayee(It.IsAny<GetPayeeRequest>())).Returns(payeeResult);
            argsfactory.ReportMock.Setup(r => r.GetBatchPaymentReportDetails(It.IsAny<Disbursement>()))
                .Returns(new BatchPaymentReport());

            //ACT
            var paymentReqConverter = new PaymentRequestConverter(argsfactory.LimServiceMock.Object,
                argsfactory.DealScopeMock.Object, argsfactory.PayeeServiceMock.Object, argsfactory.ValidationMock.Object,
                argsfactory.ReportMock.Object);
            var results=paymentReqConverter.ConvertToPaymentRequests(disbursements, fundingDeal,
                new Payment() {PaymentDate = DateTime.Today},
                new UserContext() {UserID = "ttest", UserType = UserType.Lawyer});
            
            //ASSERT
            Assert.That(results.Result.Count==2);
            Assert.IsNotNull(results.Result[10].Payee.PayeeAccount.BankAddress);
            Assert.IsNotNull(results.Result[10].Payee.PayeeAccount);
        }
    }
}
