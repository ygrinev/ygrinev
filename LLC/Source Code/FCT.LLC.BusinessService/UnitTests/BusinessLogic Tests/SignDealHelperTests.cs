using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.Common.DataContracts;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class SignDealHelperTests
    {
        [Test]
        public void AreAMatch_CompareUniqueDeals()
        {
            var sourcedeal = CreateUniqueDeal();
            var targetdeal = CreateUniqueDeal();
            targetdeal.Mortgagors.First().LastName = "m";

            bool match = SignDealHelper.AreAMatch(sourcedeal, targetdeal);
            Assert.IsFalse(match);
        }

        [Test]
        public void AreAMatch_CompareDisbursments()
        {
            var sourceDisbursements = CreateDisbursements();
            var targetDisbursements = CreateDisbursements();
            targetDisbursements.Last().PayeeName = "Joker Broker";

            bool match=SignDealHelper.AreAMatch(sourceDisbursements, targetDisbursements);
            Assert.IsFalse(match);
        }

        private static UniqueDealDescriptor CreateUniqueDeal()
        {
            return new UniqueDealDescriptor
            {
                ClosingDate = DateTime.Today,
                Mortgagors =
                    new MortgagorCollection
                    {
                        new Mortgagor
                        {
                            MortgagorType = PartyType.Person,
                            FirstName = "Ishita",
                            LastName = "M",
                            MortgagorID = 1
                        }
                    },
                Vendors =
                    new VendorCollection
                    {
                        new Vendor {VendorType = PartyType.Person, FirstName = "Peter", LastName = "Pan", VendorID = 2}
                    },
                Property =
                    new PropertyDescriptor
                    {
                        Address = "Willow St",
                        City = "Narnia",
                        Country = "Canada",
                        PropertyID = 1,
                        StreetNumber = "12"
                    },
                Pins =
                    new PinCollection
                    {
                        new Pin {PINNumber = "1212", PinID = 12},
                        new Pin {PinID = 13, PINNumber = "13131"}
                    },
            };
        }

        private static DisbursementCollection CreateDisbursements()
        {
            return new DisbursementCollection()
            {
                new Disbursement()
                {
                    PayeeType = PayeeType.Builder,
                    PayeeName = "Castle Builders",
                    Amount = 4000,
                    DisbursementID = 1,
                    PaymentMethod = "WIRE"
                },
                new Disbursement()
                {
                    PayeeType = PayeeType.MortgageBroker,
                    PayeeName = "Joker Brokers",
                    Amount = 2000,
                    DisbursementID = 2,
                    PaymentMethod = "CHEQUE"
                }
            };
        }
    }
}
