using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Implementations;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class FeeRepositoryTests
    {
        private const decimal BaseEfFee = 60;

        [Test]//Integration test: read only config values in database
        public void CalculateFee_By_DisbursementCount()
        {
            var context = new EFBusinessContext();
            var feeRepo = new FeeRepository(context, new FeeMapper(), new ReadOnlyDataHelper(context));
            var fee = feeRepo.CalculateFee(3, "ON", true, 1);

            Assert.AreEqual(BaseEfFee/2, fee.Amount); //fee is split equally
            Assert.AreEqual(Convert.ToDecimal(Convert.ToInt32(BaseEfFee/2)*0.13), fee.HST); //ON province HST=13%
        }

        [Test]//Integration test: read only config values in database
        public void ReadBaseFeeFromConfig_ReturnFunds()
        {
            var context = new EFBusinessContext();
            var feeRepo = new FeeRepository(context, new FeeMapper(), new ReadOnlyDataHelper(context));
            decimal fee=feeRepo.GetBaseFeeFromConfiguration(EasyFundFee.FCTReturnFundsFee);
            Assert.AreEqual(fee, 0);
        }

        [Test]
        public void CalculateFee_ReturnFunds()
        {
            var context = new EFBusinessContext();
            var feeRepo = new FeeRepository(context, new FeeMapper(), new ReadOnlyDataHelper(context));
            var fee = feeRepo.CalculateFee("ON");
            Assert.AreEqual(fee.Amount,0);
        }
    }
}
