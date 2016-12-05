using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Logging;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class FundingBusinessLogicTests
    {
        [Test]
        public void GetFundingDeal_OK()
        {
            ArgumentsFactory argsFactory;
            var business = GetConstructor(out argsFactory);
            argsFactory.FundingDealMock.Setup(fd => fd.GetFundingDeal(1)).Returns(new FundingDeal() { DealID = 1 });
            
            var req = new GetFundingDealRequest() {DealID = 1, UserContext = new UserContext() {UserID = "test"}};
            var result = business.GetFundingDeal(req);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Deal.DealID == 1);
        }

        [Test]
        public void SaveFundingDeal_LawyerActingForVendor_Update_OK()
        {
            var currentlawyer=new Lawyer() {LawyerID = 1, DealContacts = new ContactCollection(){new Contact(){ContactID = 11}}};
            var testdeal = new FundingDeal()
            {
                DealID =1,
                FCTURN = "111111",
                ActingFor = LawyerActingFor.Vendor,
                BusinessModel = BusinessModel.EASYFUND,
                DealStatus = DealStatus.Active,
                DealType = DealType.PurchaseSale,
                Lawyer = currentlawyer,
                OtherLawyer = new Lawyer() {LawyerID = 2, DealContacts = new ContactCollection(){new Contact(){ContactID = 22}}},
                Property = new Property() {PropertyID = 1, Pins = new PinCollection(){new Pin(){PinID = 1, PINNumber = "123"}}},
                
                Vendors = new VendorCollection() {new Vendor() {VendorID = 11}},
                Mortgagors = new MortgagorCollection() {new Mortgagor() {MortgagorID = 12}}
            };

            var expectedDeal = new FundingDeal()
            {
                DealID = 1,
                FCTURN = "111111",
                ActingFor = LawyerActingFor.Vendor,
                BusinessModel = BusinessModel.EASYFUND,
                DealStatus = DealStatus.Active,
                DealType = DealType.PurchaseSale,
                Lawyer = currentlawyer,
                OtherLawyer = new Lawyer() { LawyerID = 2, DealContacts = new ContactCollection() { new Contact() { ContactID = 22 } } },
                Property = new Property() { PropertyID = 1, Pins = new PinCollection() { new Pin() { PinID = 1, PINNumber = "123" } } },

                Vendors = new VendorCollection() { new Vendor() { VendorID = 11 } },
                Mortgagors = new MortgagorCollection() { new Mortgagor() { MortgagorID = 12 } }
                
            };
            DealManagementBusinessLogic business;
            var argsFactory = ArrangeDealForTest(testdeal, expectedDeal, currentlawyer, out business, LawyerActingFor.Vendor, LawyerActingFor.Vendor);

            var req = new SaveFundingDealRequest() { Deal = testdeal, UserContext = new UserContext() { UserID = "test" } };
                var result = business.SaveFundingDeal(req);

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Deal == expectedDeal);
                argsFactory.MortgageMock.Verify(m => m.UpdateMortgage(1, It.IsAny<DateTime>()), Times.Never);
                argsFactory.PropertyMock.Verify(p => p.UpdateProperty(It.IsAny<Property>(), It.IsAny<int>()), Times.Never);
                argsFactory.MortgagorMock.Verify(m => m.UpdateMorgagorRange(It.IsAny<IEnumerable<Mortgagor>>(), It.IsAny<int>()), Times.Never);
                argsFactory.VendorMock.Verify(v => v.UpdateVendorRange(It.IsAny<IEnumerable<Vendor>>(), 10), Times.Once);
                argsFactory.DealScopeMock.Verify(d => d.GetDealScope("111111", null), Times.Once);


            
        }

        private static ArgumentsFactory ArrangeDealForTest(FundingDeal testdeal, FundingDeal expectedDeal, Lawyer currentlawyer,
            out DealManagementBusinessLogic business, string currentActingFor, string oldActingFor)
        {
            ArgumentsFactory argsFactory;
            business = GetConstructor(out argsFactory);

            argsFactory.DealScopeMock.Setup(d => d.GetDealScope("111111", null)).Returns(10);
            argsFactory.FundingDealMock.Setup(fd => fd.UpdateFundingDeal(testdeal, 10)).Returns(expectedDeal);
            argsFactory.FundingDealMock.Setup(fd => fd.GetOtherDealInScope(1, 10, "")).Returns(2);
            argsFactory.FundingDealMock.Setup(fd => fd.GetStatus(1)).Returns(DealStatus.Active);
            argsFactory.FundingDealMock.Setup(fd => fd.GetFundingDeal(1)).Returns(expectedDeal);

            var disbursements = new DisbursementCollection()
            {
                new Disbursement() {DisbursementID = 121, PayeeType = FeeDistribution.VendorLawyer}
            };
            var disbursementMock = new Mock<IDisbursementRepository>(MockBehavior.Loose);
            argsFactory.DisbursementMock = disbursementMock;
            argsFactory.DisbursementMock.Setup(d => d.GetDisbursements(1)).Returns(disbursements);

            argsFactory.LawyerMock.Setup(l => l.GetNotificationDetails(2))
                .Returns(new LawyerProfile() {Email = "abc@test.com", UserLanguage = "English"});
            argsFactory.VendorLawyerMock.Setup(
                v =>
                    v.UpdateVendorLawyerDisbursement(currentlawyer, 1,new UserContext(),  currentActingFor,
                        oldActingFor));
            return argsFactory;
        }

        [Test]
        public void SaveFundingDeal_LawyerActingForPurchaser_Update_OK()
        {
            var currentlawyer = new Lawyer()
            {
                LawyerID = 1,
                DealContacts = new ContactCollection() {new Contact() {ContactID = 11}}
            };
            var property = new Property()
            {
                PropertyID = 1,
                Pins = new PinCollection() {new Pin() {PinID = 1, PINNumber = "123"}}
            };
            var testdeal = new FundingDeal()
            {
                DealID = 1,
                FCTURN = "111111",
                ActingFor = LawyerActingFor.Purchaser,
                BusinessModel = BusinessModel.EASYFUND,
                DealStatus = DealStatus.Active,
                DealType = DealType.PurchaseSale,
                Lawyer = currentlawyer,
                OtherLawyer = new Lawyer() { LawyerID = 2, DealContacts = new ContactCollection() { new Contact() { ContactID = 22 } } },
                Property = property,
                Vendors = new VendorCollection() { new Vendor() { VendorID = 11 } },
                ClosingDate = DateTime.Today,
                Mortgagors = new MortgagorCollection() { new Mortgagor() { MortgagorID = 12 } }
            };

            var expectedDeal = new FundingDeal()
            {
                DealID = 1,
                ActingFor = LawyerActingFor.Purchaser,
                BusinessModel = BusinessModel.EASYFUND,
                DealStatus = DealStatus.Active,
                FCTURN = "111111",
                ClosingDate = DateTime.Today,
                DealType = DealType.PurchaseSale,
                Lawyer = currentlawyer,
                OtherLawyer = new Lawyer() { LawyerID = 2, DealContacts = new ContactCollection() { new Contact() { ContactID = 22 } } },
                Property = property,
                Vendors = new VendorCollection() { new Vendor() { VendorID = 11 } },
                Mortgagors = new MortgagorCollection() { new Mortgagor() { MortgagorID = 12 } }

            };
                DealManagementBusinessLogic business;
                var argsFactory = ArrangeDealForTest(testdeal, expectedDeal, currentlawyer, out business, LawyerActingFor.Purchaser, LawyerActingFor.Purchaser);
                argsFactory.MortgageMock.Setup(m => m.UpdateMortgage(1, DateTime.Today));
                argsFactory.MortgageMock.Setup(m => m.UpdateMortgage(2, DateTime.Today));
                argsFactory.PropertyMock.Setup(p => p.UpdateProperty(property, 1));
                argsFactory.PropertyMock.Setup(p => p.UpdateProperty(property, 2));

                var req = new SaveFundingDealRequest() { Deal = testdeal, UserContext = new UserContext() { UserID = "test" } };
                var result = business.SaveFundingDeal(req);

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Deal == expectedDeal);
                argsFactory.MortgageMock.Verify(m => m.UpdateMortgage(It.IsAny<int>(), It.IsAny<DateTime>()), Times.Exactly(2));
                argsFactory.PropertyMock.Verify(p => p.UpdateProperty(It.IsAny<Property>(), It.IsAny<int>()), Times.Once);
                argsFactory.MortgagorMock.Verify(m => m.UpdateMorgagorRange(It.IsAny<IEnumerable<Mortgagor>>(), It.IsAny<int>()), Times.Once);
                argsFactory.VendorMock.Verify(v => v.UpdateVendorRange(It.IsAny<IEnumerable<Vendor>>(), It.IsAny<int>()), Times.Never);

        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SaveFundingDeal_SubmitValidation_Fails()
        {
            ArgumentsFactory argsFactory;
            var business = GetConstructor(out argsFactory);
            var testdeal = new FundingDeal()
            {
                DealID = 1,
                Property = new Property(),
                DealType = DealType.PurchaseSale,
                DealStatus = DealStatus.Active,
                Lawyer = new Lawyer(),
                BusinessModel = BusinessModel.EASYFUND,
                ActingFor = LawyerActingFor.Both
            };

            var req = new SaveFundingDealRequest() { Deal = testdeal, UserContext = new UserContext() { UserID = "test" } };
            var result = business.SaveFundingDeal(req);
            Assert.IsNull(result);
        }

        [Test]
        public void SaveFundingDeal_DraftValidation_Passes()
        {
            DealManagementBusinessLogic business;
            var vendors = new VendorCollection() {new Vendor() {VendorID = 11}};
            var currentlawyer = new Lawyer()
            {
                DealContacts = new ContactCollection() {new Contact() {ContactID = 11}},
                LawyerID = 1
            };
            var testdeal = new FundingDeal()
            {
                DealID = 1,
                FCTURN = "111111",
                Property = new Property(),
                ClosingDate = DateTime.Today,
                DealType = DealType.PurchaseSale,
                DealStatus = DealStatus.UserDraft,
                Lawyer = currentlawyer,
                ActingFor = LawyerActingFor.Vendor,
                BusinessModel = BusinessModel.EASYFUND,
                Vendors = vendors
            };

            var expectedDeal=new FundingDeal()
            {
                DealID = 1,
                FCTURN = "111111",
                Property = new Property(),
                ClosingDate = DateTime.Today,
                DealType = DealType.PurchaseSale,
                DealStatus = DealStatus.UserDraft,
                Lawyer = currentlawyer,
                ActingFor = LawyerActingFor.Vendor,
                BusinessModel = BusinessModel.EASYFUND,
                Vendors = vendors
            };

            ArgumentsFactory argsFactory = ArrangeDealForTest(testdeal, expectedDeal, currentlawyer, out business, LawyerActingFor.Vendor,
                LawyerActingFor.Vendor);
            var req = new SaveFundingDealRequest() { Deal = testdeal, UserContext = new UserContext() { UserID = "test" } };
            var result = business.SaveFundingDeal(req);

            Assert.IsNotNull(result);
            argsFactory.VendorMock.Verify(v => v.UpdateVendorRange(vendors, It.IsAny<int>()), Times.Once);
            argsFactory.FundingDealMock.Verify(fd => fd.UpdateFundingDeal(It.IsAny<FundingDeal>(), It.IsAny<int>()), Times.Once);
        }

        private static DealManagementBusinessLogic GetConstructor(out ArgumentsFactory factory)
        {
            var mockRepository = new MockRepository(MockBehavior.Default);
            var argsFactory = new ArgumentsFactory(mockRepository);
            //var logger = new EnterpriseLibraryLogger();
            var business = new DealManagementBusinessLogic(argsFactory.FundingDealMock.Object,
                argsFactory.DealScopeMock.Object, argsFactory.PropertyMock.Object,
                argsFactory.VendorMock.Object, argsFactory.MortgagorMock.Object, argsFactory.ContactMock.Object,
                argsFactory.MortgageMock.Object, argsFactory.PinMock.Object,
                argsFactory.LawyerMock.Object, argsFactory.DealHistoryMock.Object, argsFactory.FundedMock.Object,
                argsFactory.AmendMock.Object, argsFactory.FeeMock.Object,argsFactory.EmailMock.Object ,
                argsFactory.VendorLawyerMock.Object, argsFactory.MilestoneUpdaterMock.Object);
            factory = argsFactory;
            return business;
        }
    }
}
