using System;
using System.Collections.Generic;
using System.Linq;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class DealAmendmentsCheckerTests
    {
        [Test]
        public void GetDealHistoriesForAmendments_ReturnsList_ForPurchaser()
        {
            //ARRANGE
            var dealscopeMock = new Mock<IDealScopeRepository>();
            dealscopeMock.Setup(d => d.GetDealScope("151011", null)).Returns(1);
            var dealMock = new Mock<IFundingDealRepository>();
            dealMock.Setup(d => d.GetOtherDealInScope(10, 1, null)).Returns(11);


            var olddeal = new FundingDeal
            {
                DealID = 10,
                FCTURN = "151011",
                ActingFor = LawyerActingFor.Purchaser,
                ClosingDate = DateTime.Today,
                BusinessModel = BusinessModel.EASYFUND,
                Property =
                    new Property
                    {
                        Address = "Test St",
                        City = "Test City",
                        PostalCode = "T1T1T1",
                        Country = "Canada",
                        Province = "ON",
                        StreetNumber = "111",
                        Pins = new PinCollection {new Pin {PINNumber = "12121", PinID = 1}}
                    },
                Mortgagors =
                    new MortgagorCollection
                    {
                        new Mortgagor
                        {
                            CompanyName = "Test Co.",
                            FirstName = "Firstname",
                            LastName = "Lastname",
                            MortgagorType = PartyType.Business,
                            MortgagorID = 1
                        }
                    },
            };
            var newdeal = new FundingDeal
            {
                DealID = 10,
                FCTURN = "151011",
                ActingFor = LawyerActingFor.Purchaser,
                ClosingDate = DateTime.Today,
                BusinessModel = BusinessModel.EASYFUND,
                Property =
                    new Property
                    {
                        Address = "Test St",
                        City = "Test City",
                        PostalCode = "T1T1T1",
                        Country = "Canada",
                        Province = "ON",
                        StreetNumber = "112",
                        Pins =
                            new PinCollection
                            {
                                new Pin {PINNumber = "12121", PinID = 1},
                                new Pin {PINNumber = "2323", PinID = 2}
                            }
                    },
                Mortgagors =
                    new MortgagorCollection
                    {
                        new Mortgagor
                        {
                            CompanyName = "Test Company Ltd.",
                            FirstName = "Firstname",
                            LastName = "Lastname",
                            MortgagorType = PartyType.Business,
                            MortgagorID = 1
                        },
                        new Mortgagor
                        {
                            MortgagorID = 2,
                            MortgagorType = PartyType.Person,
                            FirstName = "PersonFIrst",
                            LastName = "PersonLast"
                        }
                    },
            };

            //ACT
            var amendsChecker = new DealAmendmentsChecker(dealscopeMock.Object, dealMock.Object,
                new Mock<IDealHistoryRepository>().Object);
            bool hasamendments;
            List<UserHistory> dealhistories = amendsChecker.GetDealHistoriesForAmendments(olddeal, newdeal,out hasamendments);

            //ASSERT
            Assert.That(dealhistories.Count > 0);
            Assert.That(dealhistories.Count(d => d.DealId == 10) == dealhistories.Count(d => d.DealId == 11));
            Assert.IsTrue(hasamendments);
        }

        [Test]
        public void GetDealHistoriesForAmendments_ReturnsList_ForVendor()
        {
            //ARRANGE
            var dealscopeMock = new Mock<IDealScopeRepository>();
            dealscopeMock.Setup(d => d.GetDealScope("151011", null)).Returns(1);
            var dealMock = new Mock<IFundingDealRepository>();
            dealMock.Setup(d => d.GetOtherDealInScope(10, 1, null)).Returns(11);


            var olddeal = new FundingDeal
            {
                DealID = 10,
                FCTURN = "151011",
                ActingFor = LawyerActingFor.Vendor,
                ClosingDate = DateTime.Today,
                BusinessModel = BusinessModel.EASYFUND,
                Property =
                    new Property
                    {
                        Address = "Test St",
                        City = "Test City",
                        PostalCode = "T1T1T1",
                        Country = "Canada",
                        Province = "ON",
                        StreetNumber = "111",
                        Pins = new PinCollection { new Pin { PINNumber = "12121", PinID = 1 } }
                    },
               Vendors = 
                    new VendorCollection()
                    {
                        new Vendor()
                        {
                            CompanyName = "Test Co.",
                            FirstName = "Firstname",
                            LastName = "Lastname",
                            VendorType = PartyType.Business,
                            VendorID = 1
                        }
                    },
            };
            var newdeal = new FundingDeal
            {
                DealID = 10,
                FCTURN = "151011",
                ActingFor = LawyerActingFor.Vendor,
                ClosingDate = DateTime.Today,
                BusinessModel = BusinessModel.EASYFUND,
                Property =
                    new Property
                    {
                        Address = "Test St",
                        City = "Test City",
                        PostalCode = "T1T1T1",
                        Country = "Canada",
                        Province = "ON",
                        StreetNumber = "112",
                        Pins =
                            new PinCollection
                            {
                                new Pin {PINNumber = "12121", PinID = 1},
                                new Pin {PINNumber = "2323", PinID = 2}
                            }
                    },
               Vendors = 
                    new VendorCollection()
                    {
                        new Vendor()
                        {
                            CompanyName = "Test Company Ltd.",
                            FirstName = "Firstname",
                            LastName = "Lastname",
                            VendorType = PartyType.Business,
                            VendorID = 1
                        },
                        new Vendor()
                        {
                            VendorID = 2,
                           VendorType = PartyType.Person,
                            FirstName = "PersonFIrst",
                            LastName = "PersonLast"
                        }
                    },
            };

            //ACT
            var amendsChecker = new DealAmendmentsChecker(dealscopeMock.Object, dealMock.Object,
                new Mock<IDealHistoryRepository>().Object);
            bool hasamendments;
            List<UserHistory> dealhistories = amendsChecker.GetDealHistoriesForAmendments(olddeal, newdeal,out hasamendments);

            //ASSERT
            Assert.That(dealhistories.Count > 0);
            Assert.That(dealhistories.Count(d => d.DealId == 10) == dealhistories.Count(d => d.DealId == 11));
            Assert.IsTrue(hasamendments);
        }
    }
}
