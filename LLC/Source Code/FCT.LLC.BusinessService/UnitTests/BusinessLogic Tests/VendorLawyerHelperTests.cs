using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.Services.LIM.DataContracts;
using FCT.Services.LIM.MessageContracts;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class VendorLawyerHelperTests
    {
        private DisbursementCollection _disbursementCollection;

        [Test]
        public void AssignExistingTrustAccount_UniqueTrustAccountFound()
        {
            //ARRANGE
            var trustAccountsResponse = new GetTrustAccountsResponse()
            {
                TrustAccounts = new TrustAccountCollection()
                {
                    new TrustAccount()
                    {
                        TrustAccountID=1,
                        AccountNum = "1111122222",
                        BankNum = "101",
                        BranchNum = "11111",
                        TrustAccountStatusID = (int) AccountStatus.Active,
                    }
                }
            };
            var mockRepo = new MockRepository(MockBehavior.Default);
            var argsfactory = new ArgumentsFactory(mockRepo);
            argsfactory.LimServiceMock.Setup(l => l.GetTrustAccounts(It.IsAny<GetTrustAccountsRequest>()))
                .Returns(trustAccountsResponse);
            var vendordisbursement = new Disbursement()
            {
                PayeeType = FeeDistribution.VendorLawyer,
                Amount = 1000,
            };
            //ACT
            var vendorHelper = new VendorLawyerHelper(argsfactory.LimServiceMock.Object,
                argsfactory.DisbursementMock.Object, argsfactory.DealHistoryMock.Object, argsfactory.FundingDealMock.Object);
            var success = vendorHelper.AssignActiveExistingTrustAccount(new Lawyer() { LawyerID = 1 }, vendordisbursement);

            //ASSERT           
            Assert.IsTrue(success);
            Assert.IsTrue(vendordisbursement.TrustAccountID==1);
            Assert.IsTrue(vendordisbursement.AccountNumber == "1111122222");
            Assert.IsTrue(vendordisbursement.BankNumber=="101");
            Assert.IsTrue(vendordisbursement.BranchNumber == "11111");

        }

    [Test]
        public void AssignExistingTrustAccount_ActiveTrustAccountNotFound()
        {
            //ARRANGE
            var trustAccountsResponse = new GetTrustAccountsResponse()
            {
                TrustAccounts = new TrustAccountCollection()
                {
                    new TrustAccount()
                    {
                        TrustAccountID=1,
                        AccountNum = "1111122222",
                        BankNum = "101",
                        BranchNum = "11111",
                        TrustAccountStatusID = (int) AccountStatus.InActive,
                    }
                }
            };
            var mockRepo = new MockRepository(MockBehavior.Default);
            var argsfactory = new ArgumentsFactory(mockRepo);
            argsfactory.LimServiceMock.Setup(l => l.GetTrustAccounts(It.IsAny<GetTrustAccountsRequest>()))
                .Returns(trustAccountsResponse);
            var vendordisbursement = new Disbursement()
            {
                PayeeType = FeeDistribution.VendorLawyer,
                Amount = 1000,
                TrustAccountID = 1
            };
            //ACT
            var vendorHelper = new VendorLawyerHelper(argsfactory.LimServiceMock.Object,
                argsfactory.DisbursementMock.Object, argsfactory.DealHistoryMock.Object, argsfactory.FundingDealMock.Object);
            var success = vendorHelper.AssignActiveExistingTrustAccount(new Lawyer() { LawyerID = 1 }, vendordisbursement);

            //ASSERT
            
            Assert.IsFalse(success);
            Assert.IsNull(vendordisbursement.TrustAccountID);
        }

        [Test]
        public void UpdateVendorLawyerDisbursement_UpdatedToLawyerActingForBoth()
        {
            //ARRANGE
             var disbursements = new DisbursementCollection()
            {
                new Disbursement()
                {
                    PayeeType = FeeDistribution.VendorLawyer,
                    TrustAccountID = 1001,
                    Amount = 1000,
                    PayeeName = "Test Lawyer"
                },
                new Disbursement()
                {
                    PayeeType = EasyFundFee.FeeName,
                    Amount = 113
                }
            };
            var expectedVendorDisbursement = new Disbursement()
            {
                PayeeType = FeeDistribution.VendorLawyer,
                TrustAccountID = null,
                Amount = 1000,
                PayeeName = null
            };
            var mockRepo = new MockRepository(MockBehavior.Default);
            var argsfactory = new ArgumentsFactory(mockRepo);
            argsfactory.DisbursementMock.Setup(d => d.GetDisbursements(1)).Returns(disbursements);
            argsfactory.DisbursementMock.Setup(d => d.UpdateDisbursement(expectedVendorDisbursement));

            //ACT
            var vendorHelper = new VendorLawyerHelper(argsfactory.LimServiceMock.Object,
                argsfactory.DisbursementMock.Object, argsfactory.DealHistoryMock.Object, argsfactory.FundingDealMock.Object);
            vendorHelper.UpdateVendorLawyerDisbursement(new Lawyer(), 1, new UserContext(),  LawyerActingFor.Both, LawyerActingFor.Vendor);

            //ASSERT
            argsfactory.DisbursementMock.Verify(d=>d.GetDisbursements(1), Times.Once);
            argsfactory.DisbursementMock.Verify(d=>d.UpdateDisbursement(It.IsAny<Disbursement>()), Times.Once);
        }

        [Test]
        public void UpdateVendorLawyerDisbursement_NewVendorLawyerAssigned()
        {
            //ARRANGE
            var disbursements = new DisbursementCollection()
            {
                new Disbursement()
                {
                    PayeeType = FeeDistribution.VendorLawyer,
                    TrustAccountID = 1001,
                    Amount = 1000,
                    PayeeName = "Test Lawyer",
                    NameOnCheque = "Test Lawyer"
                },
                new Disbursement()
                {
                    PayeeType = EasyFundFee.FeeName,
                    Amount = 113
                }
            };
            var trustAccountsResponse = new GetTrustAccountsResponse()
            {
                TrustAccounts = new TrustAccountCollection()
                {
                    new TrustAccount()
                    {
                        TrustAccountID=1,
                        AccountNum = "1111122222",
                        BankNum = "101",
                        BranchNum = "11111",
                        TrustAccountStatusID = (int) AccountStatus.Active,
                    }
                }
            };
            var vendorLawyer = new Lawyer() {LawyerID = 1, FirstName = "Vendor", LastName = "Lawyer"};
            var mockRepo = new MockRepository(MockBehavior.Default);
            var argsfactory = new ArgumentsFactory(mockRepo);
            argsfactory.DisbursementMock.Setup(d => d.GetDisbursements(1)).Returns(disbursements);
            argsfactory.DisbursementMock.Setup(d => d.UpdateDisbursement(It.IsAny<Disbursement>()));
            argsfactory.LimServiceMock.Setup(l => l.GetTrustAccounts(It.IsAny<GetTrustAccountsRequest>()))
    .Returns(trustAccountsResponse);

            //ACT
            var vendorHelper = new VendorLawyerHelper(argsfactory.LimServiceMock.Object,
                argsfactory.DisbursementMock.Object, argsfactory.DealHistoryMock.Object, argsfactory.FundingDealMock.Object);
            vendorHelper.UpdateVendorLawyerDisbursement(vendorLawyer,1, new UserContext(), LawyerActingFor.Vendor, LawyerActingFor.Purchaser);

            //ASSERT
            argsfactory.LimServiceMock.Verify(l=>l.GetTrustAccounts(It.IsAny<GetTrustAccountsRequest>()), Times.Once);
            argsfactory.DisbursementMock.Verify(d=>d.UpdateDisbursement(It.IsAny<Disbursement>()));
        }
    }
}
