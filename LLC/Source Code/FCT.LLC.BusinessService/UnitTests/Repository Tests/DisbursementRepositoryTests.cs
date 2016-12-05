using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Implementations;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class DisbursementRepositoryTests
    {
        private List<tblDisbursement> _tblDisbursements;

        [TestFixtureSetUp]
        public void Init()
        {
            LoadDummyData();
        }

        #region Tests
        [Test]
        public void SaveDisbursements_Added_Updated_WithFee()
        {
            //ARRANGE
            var inputDisbursements = new DisbursementCollection()
            {
                new Disbursement()
                {
                    DisbursementID = 2,
                    Amount = 1000,
                    Action = CRUDAction.Create,
                    PayeeType = FeeDistribution.VendorLawyer,
                    PayeeName = "TestFirstname TestLastName"
                },
                new Disbursement()
                {
                    DisbursementID = 3,
                    PayeeType = EasyFundFee.FeeName,
                    Action = CRUDAction.Create,
                    FCTFeeSplit = FeeDistribution.SplitEqually,
                    ChainDealID = 10,

                }
            };
            var outputDisbursements = new DisbursementCollection()
            {
                new Disbursement()
                {
                    DisbursementID = 2,
                    ChainDealID = 1,
                    Amount = 1000,
                    Action = CRUDAction.None,
                    PayeeType = FeeDistribution.VendorLawyer,
                    PayeeName = "TestFirstname TestLastName",
                    FundingDealID = 5
                },
                new Disbursement()
                {
                    DisbursementID = 3,
                    PayeeType = EasyFundFee.FeeName,
                    Action = CRUDAction.None,
                    FCTFeeSplit = FeeDistribution.SplitEqually,
                    ChainDealID = 1,
                    FundingDealID = 5,
                    PurchaserFee = new Fee() {FeeID = 101, Amount = 100, HST = 13},
                    VendorFee = new Fee(){FeeID = 102, Amount = 100, HST = 13}

                }
            };
            var tblDisbursements = new List<tblDisbursement>()
            {
                new tblDisbursement()
                {
                    DisbursementID = 2,
                    ChainDealID = 1,
                    Amount = 1000,
                    PayeeType = FeeDistribution.VendorLawyer,
                    PayeeName = "TestFirstname TestLastName"
                },
                new tblDisbursement()
                {
                    DisbursementID = 3,
                    PayeeType = EasyFundFee.FeeName,
                    ChainDealID = 1,
                    Amount = 226,
                    FCTFeeSplit = FeeDistribution.SplitEqually

                }
            };
            var request = new SaveDisbursementsRequest()
            {
                DealID = 1,
                Disbursements =inputDisbursements
            };

            //var disbursementFee = new DisbursementFee()
            //{
            //    PurchaserFee = new Fee() {FeeID = 101, Amount = 100, HST = 13},
            //    VendorFee = new Fee() {FeeID = 102, Amount = 100, HST = 13}
            //};

            var mockContext = new Mock<EFBusinessContext>();
            var mockset = GetDisbursementMockDbSet();
            mockContext.Setup(m => m.Set<tblDisbursement>()).Returns(mockset.Object);
            mockContext.Setup(c => c.tblDisbursements.AsNoTracking()).Returns(mockset.Object);
            mockContext.Setup(m => m.SaveChanges()).Returns(1);

            var genericRepo = new Mock<IRepository<tblDisbursement>>();
            genericRepo.Setup(g => g.InsertRange(tblDisbursements));
            var disbursementMapper = new Mock<IEntityMapper<tblDisbursement, Disbursement>>();
            disbursementMapper.Setup(d => d.MapToData(tblDisbursements[0], null)).Returns(outputDisbursements[0]);
            disbursementMapper.Setup(d => d.MapToData(tblDisbursements[1], null)).Returns(outputDisbursements[1]);
            disbursementMapper.Setup(d => d.MapToEntity(inputDisbursements[0])).Returns(tblDisbursements[0]);
            disbursementMapper.Setup(d => d.MapToEntity(inputDisbursements[1])).Returns(tblDisbursements[1]);

            //ACT
            //var repo = new DisbursementRepository(mockContext.Object, disbursementMapper.Object);
            //var results=repo.SaveDisbursements(request, 5, disbursementFee);

            //ASSERT
            //Assert.That(results.Any());
            //CollectionAssert.AreEqual(outputDisbursements,results);
        }

        [Test]
        public void VerifyIsDisbursementDocumentGenerated()
        {
            const int validDealDocumentTypeId = 108391;
   
            var mockDataSet = GetDisbursementMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDisbursement>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDisbursements.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDisbursement, Disbursement>>();

            var repository = new DisbursementRepository(mockContext.Object, mapper.Object);
            var actual = repository.IsDisbursementDocumentGenerated(validDealDocumentTypeId);
            Assert.IsTrue(actual);
        }

        [Test]
        public void VerifyIsDisbursementDocumentGenerated_ReturnsFalseIdfInvalidDisbursementDealDocumentTypeId()
        {
            const int validDisbursementDealDocumentTypeId = 909090909;

            var mockDataSet = GetDisbursementMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDisbursement>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDisbursements.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDisbursement, Disbursement>>();

            var repository = new DisbursementRepository(mockContext.Object, mapper.Object);
            var actual = repository.IsDisbursementDocumentGenerated(validDisbursementDealDocumentTypeId);
            Assert.IsFalse(actual);
        }
        #endregion

        #region Private Methods
        private void LoadDummyData()
        {
            _tblDisbursements = new List<tblDisbursement>()
            {
                new tblDisbursement()
                {
                    DisbursementID = 1,
                    FundingDealID = 5,
                    PayeeType = "Builder",
                    PayeeName = "Test Builder",
                    Amount = 500,
                    ChainDealID = 1
                }
            };

        }
        private Mock<DbSet<tblDisbursement>> GetDisbursementMockDbSet()
        {
            IQueryable<tblDisbursement> data = _tblDisbursements.AsQueryable();
            var mockSet = new Mock<DbSet<tblDisbursement>>();
            mockSet.As<IQueryable<tblDisbursement>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tblDisbursement>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tblDisbursement>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tblDisbursement>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
        #endregion
    }
}
