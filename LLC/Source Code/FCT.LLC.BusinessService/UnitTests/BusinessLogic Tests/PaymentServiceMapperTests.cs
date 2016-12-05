using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Logging;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class PaymentServiceMapperTests
    {
        [Test]
        public void MapDebtorName_8Vendors()
        {
            var context = new EFBusinessContext();
            var repository = new FundingDealRepository(context,
                new FundingDealMapper(new LawyerMapper(new ContactMapper()), new MortgagorMapper(), new VendorMapper(),
                    new PropertyMapper(new PINMapper())), new LawyerMapper(new ContactMapper()), new Mock<ILogger>().Object,
                new FundedDealMapper());
            var fundingDeal = repository.GetFundingDeal(29457);
            var result = PaymentServiceMapper.MapDebtorName(fundingDeal, "Municipal or Utility");
        }
    }
}
 