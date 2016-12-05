using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Implementations;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class FeeCalculatorTests
    {
        [Test]
        public void RecalculateAndSave_OK()
        {
            //ARRANGE
            var propertyRepo = new Mock<IPropertyRepository>();
            propertyRepo.Setup(p => p.GetPropertyByScope(1)).Returns(new Property() {Province = "ON"});
            var disbursementRepo = new Mock<IDisbursementRepository>();
            var purchaserFee = new Fee() {FeeID = 10, Amount = 100, HST = 13};
            var vendorFee = new Fee() {Amount = 100, FeeID = 11, HST = 13};
            var disbursement = new Disbursement()
            {
                Province = "ON",
                FCTFeeSplit = FeeDistribution.SplitEqually,
                PurchaserFee = purchaserFee,
                VendorFee = vendorFee,
                DisbursementID = 2
            };
            disbursementRepo.Setup(d => d.GetDisbursement(2)).Returns(disbursement);
            var feeRepo = new Mock<IFeeRepository>();
            feeRepo.Setup(f => f.UpdateFee(4, "ON", true, 10)).Returns(purchaserFee);
            feeRepo.Setup(f => f.UpdateFee(4, "ON", true, 11)).Returns(vendorFee);

            //ACT
            var feecalculator = new FeeCalculator(feeRepo.Object, disbursementRepo.Object, propertyRepo.Object,
                new Mock<IFundedRepository>().Object, new Mock<IDisbursementSummaryRepository>().Object);
            var fee=feecalculator.RecalculateAndSaveFees(disbursement, 4, 1);

            //ASSERT
            Assert.That(fee.PurchaserFee==purchaserFee);
            Assert.That(fee.VendorFee==vendorFee);
        }

        [Test]
        public void ReCalculateFees_RecalculateOnly()
        {
            //ARRANGE
            var propertyRepo = new Mock<IPropertyRepository>();
            propertyRepo.Setup(p => p.GetPropertyByScope(1)).Returns(new Property() { Province = "ON" });
            var disbursementRepo = new Mock<IDisbursementRepository>();
            var purchaserFee = new Fee() { FeeID = 10, Amount = 100, HST = 13 };
            var vendorFee = new Fee() { Amount = 100, FeeID = 11, HST = 13 };
            var disbursement = new Disbursement()
            {
                Province = "ON",
                FCTFeeSplit = FeeDistribution.SplitEqually,
                PurchaserFee = purchaserFee,
                VendorFee = vendorFee,
                DisbursementID = 2
            };
            disbursementRepo.Setup(d => d.GetDisbursement(2)).Returns(disbursement);
            var feeRepo = new Mock<IFeeRepository>();
            feeRepo.Setup(f => f.CalculateFee(4, "ON", true, 10)).Returns(purchaserFee);
            feeRepo.Setup(f => f.CalculateFee(4, "ON", true, 11)).Returns(vendorFee);

            //ACT
            var feecalculator = new FeeCalculator(feeRepo.Object, disbursementRepo.Object, propertyRepo.Object,
                new Mock<IFundedRepository>().Object, new Mock<IDisbursementSummaryRepository>().Object);
            var fee = feecalculator.RecalculateFee(disbursement, 4, 1);

            //ASSERT
            Assert.That(fee == purchaserFee);


        }
    }
}
