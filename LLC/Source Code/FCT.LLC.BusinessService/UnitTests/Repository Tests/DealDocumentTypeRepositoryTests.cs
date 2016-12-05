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
    public class DealDocumentTypeRepositoryTests
    {
        #region Private Members
        private List<tblDealDocumentType> _tblDealDocumentTypes;
        private List<DealDocumentType> _DealDocumentTypes;
        private Mock<IEntityMapper<tblDealDocumentType, DealDocumentType>> _mapper;
        #endregion

        #region Setup

        [TestFixtureSetUp]
        public void Init()
        {
            _tblDealDocumentTypes = new List<tblDealDocumentType>();
        }
        #endregion

        #region Tests

        [Test]
        public void VerifyInsertDealDocumentType()
        {
            const int validDealId = 13934;
            const int validDealDocumentTypeId = 107929;
            const int validDocumentTypeId = 2509;

            var data = new tblDealDocumentType
            {
                DisplayNameSuffix = null,
                IsActive = false,
                DealDocumentTypeID = validDealDocumentTypeId,
                DealID = validDealId,
                DocumentTypeID = validDocumentTypeId
            };

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDealDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDealDocumentType.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDealDocumentType, DealDocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDealDocumentType>(), null)).Returns(It.IsAny<DealDocumentType>());

            var repository = new DealDocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.InsertDealDocumentType(data);
            Assert.AreEqual(actual, validDealDocumentTypeId);
        }

        [Test]
        public void VerifyGetByDealDocumentTypeId()
        {
            const int validDealId = 13934;
            const int validDealDocumentTypeId = 107929;
            const int validLanguageId = 0;

            var expected = new DealDocumentType
            {
                DisplayNameSuffix = null,
                IsActive = false,
                DealDocumentTypeId = validDealDocumentTypeId,
                DealId = validDealId,
                DocumentTypeId = 2509
            };

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDealDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDealDocumentType.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDealDocumentType, DealDocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDealDocumentType>(), null)).Returns(expected);

            var repository = new DealDocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByDealDocumentTypeId(validDealId, validDealDocumentTypeId, validLanguageId);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void VerifyGetByDealDocumentTypeIdReturnsNull_DealIdInvalid()
        {
            const int invalidDealId = 909090909;
            const int validDealDocumentTypeId = 107929;
            const int validLanguageId = 0;

            DealDocumentType expected = null;

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDealDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDealDocumentType.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDealDocumentType, DealDocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDealDocumentType>(), null)).Returns(expected);

            var repository = new DealDocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByDealDocumentTypeId(invalidDealId, validDealDocumentTypeId, validLanguageId);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void VerifyGetByDealDocumentTypeIdReturnsNull_DocumentTypeIdInvalid()
        {
            const int validDealId = 13934;
            const int invalidDealDocumentTypeId = 909090909;
            const int validLanguageId = 0;

            DealDocumentType expected = null;

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDealDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDealDocumentType.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDealDocumentType, DealDocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDealDocumentType>(), null)).Returns(expected);

            var repository = new DealDocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByDealDocumentTypeId(validDealId, invalidDealDocumentTypeId, validLanguageId);
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void VerifyGetByDealDocumentTypeIdReturnsNull_LanguageIdInvalid()
        {
            const int validDealId = 13934;
            const int validDealDocumentTypeId = 107929;
            const int invalidLanguageId = 100;

            DealDocumentType expected = null;

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDealDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDealDocumentType.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDealDocumentType, DealDocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDealDocumentType>(), null)).Returns(expected);

            var repository = new DealDocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByDealDocumentTypeId(validDealId, validDealDocumentTypeId, invalidLanguageId);
            Assert.AreEqual(actual, expected);
        }
        #endregion

        #region Private Methods

        private Mock<DbSet<tblDealDocumentType>> GetMockDbSet()
        {
            IQueryable<tblDealDocumentType> data = _tblDealDocumentTypes.AsQueryable();
            var mockSet = new Mock<DbSet<tblDealDocumentType>>();
            mockSet.As<IQueryable<tblDealDocumentType>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tblDealDocumentType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tblDealDocumentType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tblDealDocumentType>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
        #endregion
    }
}
