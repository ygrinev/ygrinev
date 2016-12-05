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
    public class DocumentTypeRepositoryTests
    {
        #region Private Members
        private List<tblDocumentType> _tblDocumentTypes;
        private List<DocumentType> _documentTypes;
        private Mock<IEntityMapper<tblDocumentType, DocumentType>> _mapper;
        #endregion

        #region Setup

        [TestFixtureSetUp]
        public void Init()
        {
            _tblDocumentTypes = new List<tblDocumentType>();
        }
        #endregion

        #region Tests
        [Test]
        public void VerifyCanGetByName()
        {
            var expected = new DocumentType
            {
                DocumentTypeId = 2616,
                DocumentCategoryId = 9
            };

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDocumentType.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDocumentType, DocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDocumentType>(), null)).Returns(expected);

            var repository = new DocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByName("Confirmation Letter", "EasyFund");
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void VerifyCanGetByNameReturnsNullIfDocumentTypeNameNotFound()
        {
            var expected = new DocumentType
            {
                DocumentTypeId = 2616,
                DocumentCategoryId = 96,
                BusinessModel = "EASYFUND"
            };

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDocumentType.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDocumentType, DocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDocumentType>(), null)).Returns(expected);

            var repository = new DocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByName("Confirmation Letter", "EasyFund", "EASYFUND");
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void VerifyCanGetByNameReturnsNullIfCategoryNameNotFound()
        {
            DocumentType expected = null;

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDocumentType.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDocumentType, DocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDocumentType>(), null)).Returns(expected);

            var repository = new DocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByName("Confirmation Letter", "HardFund");
            Assert.AreEqual(actual, expected);
        }

        [Test]
        public void VerifyCanGetByNameReturnsNullIfBusinessModelNotFound()
        {
            DocumentType expected = null;

            var mockDataSet = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDocumentType>().AsNoTracking()).Returns(mockDataSet.Object);
            mockContext.Setup(c => c.tblDocumentType.AsNoTracking()).Returns(mockDataSet.Object);
            var mapper = new Mock<IEntityMapper<tblDocumentType, DocumentType>>();
            mapper.Setup(m => m.MapToData(It.IsAny<tblDocumentType>(), null)).Returns(expected);

            var repository = new DocumentTypeRepository(mockContext.Object, mapper.Object);
            var actual = repository.GetByName("Confirmation Letter", "EasyFund", "HARDFUND");
            Assert.AreEqual(actual, expected);
        }

        #endregion

        #region Private Methods

        private Mock<DbSet<tblDocumentType>> GetMockDbSet()
        {
            IQueryable<tblDocumentType> data = _tblDocumentTypes.AsQueryable();
            var mockSet = new Mock<DbSet<tblDocumentType>>();
            mockSet.As<IQueryable<tblDocumentType>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tblDocumentType>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tblDocumentType>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tblDocumentType>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
        #endregion
    }
}
