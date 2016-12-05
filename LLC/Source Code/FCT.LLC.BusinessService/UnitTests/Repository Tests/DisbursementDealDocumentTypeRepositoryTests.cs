using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Implementations;
using FCT.LLC.BusinessService.Entities;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests.Repository_Tests
{
    [TestFixture]
    public class DisbursementDealDocumentTypeRepositoryTests
    {
        #region Private Members
        private List<tblDisbursementDealDocumentType> _tblDisbursementDealDocumentTypes;
        private List<DisbursementDealDocumentType> _disbursementDealDocumentTypes;
        private Mock<IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType>> _mapper;
        #endregion

        #region Setup

        [TestFixtureSetUp]
        public void Init()
        {
            _tblDisbursementDealDocumentTypes = new List<tblDisbursementDealDocumentType>();
        }
        #endregion

        #region Tests
        [Test]
        public void VerifyInsertDisbursementDealDocumentType()
        {
            const int validDisbursementId = 400;
            const int validDealDocumentTypeId = 107929;
            const int validDisbursementDealDocumentTypeId = 12345678;

            var data = new tblDisbursementDealDocumentType
            {
                DisbursementDealDocumentType = validDisbursementDealDocumentTypeId,
                DisbursementID = validDisbursementId,
                DealDocumentTypeID = validDealDocumentTypeId,
                PayoutLetterDate = DateTime.Now
            };

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDisbursementDealDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDisbursementDealDocumentTypes.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDisbursementDealDocumentType>(), null)).Returns(It.IsAny<DisbursementDealDocumentType>());

            var repository = new DisbursementDealDocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.InsertDisbursementDealDocumentType(data);
            Assert.AreEqual(actual, validDisbursementDealDocumentTypeId);
        }

        [Test]
        public void VerifyGetByDisbursementIdDealDocumentTypeId()
        {
            const int validDealId = 0;
            const int validDisbursementId = 480;
            const int validDealDocumentTypeId = 108391;

            var expected = new DisbursementDealDocumentType
            {
                DisbursementDealDocumentTypeId = 76,
                DisbursementId = validDisbursementId,
                DealDocumentTypeId = validDealDocumentTypeId
            };

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDisbursementDealDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDisbursementDealDocumentTypes.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDisbursementDealDocumentType>(), null)).Returns(expected);

            var repository = new DisbursementDealDocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByDisbursementIdDocumentTypeId(validDealId, validDisbursementId, 2616);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void VerifyGetByDisbursementIdDealDocumentTypeIdReturnsNull_InvalidDisbursementId()
        {
            const int invalidDealId = 0;
            const int invalidDisbursementId = 919191298;
            const int validDealDocumentTypeId = 108391;

            DisbursementDealDocumentType expected = null;

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDisbursementDealDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDisbursementDealDocumentTypes.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDisbursementDealDocumentType>(), null)).Returns(expected);

            var repository = new DisbursementDealDocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByDisbursementIdDocumentTypeId(invalidDealId, invalidDisbursementId, validDealDocumentTypeId);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void VerifyGetByDisbursementIdDealDocumentTypeIdReturnsNull_InvalidDealDocumentId()
        {
            const int validDealId = 0;
            const int validDisbursementId = 400;
            const int invalidDealDocumentTypeId = 919191298;

            DisbursementDealDocumentType expected = null;

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDisbursementDealDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDisbursementDealDocumentTypes.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDisbursementDealDocumentType>(), null)).Returns(expected);

            var repository = new DisbursementDealDocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByDisbursementIdDocumentTypeId(validDealId, validDisbursementId, invalidDealDocumentTypeId);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void VerifyGetByDisbursementDealDocumentTypeId()
        {
            const int validDisbursementId = 400;
            const int validDisbursementDealDocumentTypeId = 31;
            const int validDealDocumentTypeId = 107929;

            var expected = new DisbursementDealDocumentType
            {
                DisbursementDealDocumentTypeId = validDisbursementDealDocumentTypeId,
                DisbursementId = validDisbursementId,
                DealDocumentTypeId = validDealDocumentTypeId
            };

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDisbursementDealDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDisbursementDealDocumentTypes.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDisbursementDealDocumentType>(), null)).Returns(expected);

            var repository = new DisbursementDealDocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByDisbursementDealDocumentTypeId(validDisbursementDealDocumentTypeId);
            Assert.AreEqual(actual, expected);
        }

        public void VerifyGetByDisbursementDealDocumentTypeIdReturnsNull_InvalidDisbursementDealDocumentTypeId()
        {
            const int invalidDisbursementDealDocumentTypeId = 909090909;

            DisbursementDealDocumentType expected = null;

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDisbursementDealDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDisbursementDealDocumentTypes.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDisbursementDealDocumentType, DisbursementDealDocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDisbursementDealDocumentType>(), null)).Returns(expected);

            var repository = new DisbursementDealDocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByDisbursementDealDocumentTypeId(invalidDisbursementDealDocumentTypeId);
            Assert.AreEqual(actual, expected);
        }
        #endregion

        #region Private Methods

        private Mock<DbSet<tblDisbursementDealDocumentType>> GetMockDbSet()
        {
            IQueryable<tblDisbursementDealDocumentType> data = _tblDisbursementDealDocumentTypes.AsQueryable();
            var mockSet = new Mock<DbSet<tblDisbursementDealDocumentType>>();
            mockSet.As<IQueryable<tblDisbursementDealDocumentType>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tblDisbursementDealDocumentType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tblDisbursementDealDocumentType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tblDisbursementDealDocumentType>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
        #endregion
    }
}
