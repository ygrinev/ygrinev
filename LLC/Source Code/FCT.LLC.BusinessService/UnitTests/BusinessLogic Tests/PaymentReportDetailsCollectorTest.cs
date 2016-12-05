using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class PaymentReportDetailsCollectorTest
    {
        [Test]
        public void GetBatchpaymentReportDetails_OK()
        {
            //ARRANGE
            var fundingDeal = SetupTestDeal();
            var l1 =new LawyerProfile()
            {
                LawyerID = 10,
                FirstName = "abc",
                LastName = "xyz",
                Email = "test1@email.com",
                Fax = "998-998-9898",
                UserLanguage = "english", 
                UserName = "axyz",
                UserType = UserType.Lawyer
            };
            var l2 = new LawyerProfile()
            {
                LawyerID = 11,
                LastName = "poi",
                FirstName = "ljk",
                Email = "test2@email.com",
                UserLanguage = "French",
                Fax = "121-121-1212",
                UserName = "lpoi",
                UserType = UserType.Lawyer
            };

            var fundingDealRepo = new Mock<IFundingDealRepository>();
            fundingDealRepo.Setup(f => f.GetFundingDeal(1, false)).Returns(fundingDeal);
            var lawyerRepo = new Mock<ILawyerRepository>();
            lawyerRepo.Setup(l => l.GetNotificationDetails(10)).Returns(l1);
            lawyerRepo.Setup(l => l.GetNotificationDetails(11)).Returns(l2);

            //ACT
            var disbursement = new Disbursement()
            {
                PayeeType = PayeeType.LineOfCredit,
                AccountAction = "Reduce Account Limit",
                ReferenceNumber = "584958495",
                FundingDealID = 1
            };
            var collector = new PaymentReportDetailsCollector(fundingDealRepo.Object, lawyerRepo.Object);
            var batchreportDetails=collector.GetBatchPaymentReportDetails(disbursement);

            //ASSERT
            Assert.IsNotNull(batchreportDetails);
            //other asserts based on input parameters can be added here

        }

        private static FundingDeal SetupTestDeal()
        {
            var fundingDeal = new FundingDeal()
            {
                ActingFor = LawyerActingFor.Purchaser,
                DealID = 2,
                DealStatus = DealStatus.Active,
                ClosingDate = DateTime.Today,
                DealType = DealType.PurchaseSale,
                BusinessModel = BusinessModel.EASYFUND,
                FCTURN = "15-100-1",
                Lawyer = new Lawyer()
                {
                    LawyerID = 10,
                    FirstName = "abc",
                    LastName = "xyz",
                    LawFirm = "abc inc",
                    Phone = "999-999-9999"
                },
                LawyerFileNumber = "qwerty123",
                OtherLawyer =
                    new Lawyer()
                    {
                        LawyerID = 11,
                        LawFirm = "asdf inc",
                        LastName = "poi",
                        FirstName = "ljk",
                        Phone = "111-111-1111"
                    },
                OtherLawyerFileNumber = "zxxnzxcbz123",
                Property = new Property()
                {
                    UnitNumber = "101",
                    StreetNumber = "1",
                    Address = "Hastings Ave",
                    City = "Star City",
                    Country = "Canada",
                    PostalCode = "B1B 1B1",
                    Pins = new PinCollection()
                    {
                        new Pin() {PinID = 100, PINNumber = "lkjhgfd"},
                    }
                },
                Mortgagors =
                    new MortgagorCollection()
                    {
                        new Mortgagor() {FirstName = "Ishita", LastName = "M", MortgagorType = PartyType.Person},
                        new Mortgagor()
                        {
                            CompanyName = "ANT INC",
                            FirstName = "Ant",
                            LastName = "Man",
                            MortgagorType = PartyType.Business
                        }
                    },
                Vendors =
                    new VendorCollection()
                    {
                        new Vendor()
                        {
                            CompanyName = "Wayne Enterprises",
                            FirstName = "Bruce",
                            LastName = "Wayne",
                            VendorType = PartyType.Business
                        }
                    }
            };
            return fundingDeal;
        }
    }
}
