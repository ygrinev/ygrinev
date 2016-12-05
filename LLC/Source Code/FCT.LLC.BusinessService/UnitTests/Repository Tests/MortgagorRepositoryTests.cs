using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    //Vendor and DealContact Repositories both have similar methods and behaviors as Mortgagor Repository
    [TestFixture]
    public class MortgagorRepositoryTests
    {
        private MortgagorCollection _mortgagors;
        private List<tblMortgagor> _tblMortgagors;
        private Mock<IEntityMapper<tblMortgagor, Mortgagor>> _mapper;
        
        [TestFixtureSetUp]
        public void Init()
        {
            _mortgagors = new MortgagorCollection()
            {
                new Mortgagor() {FirstName = "John", LastName = "Smith", MortgagorID = 1, MortgagorType = PartyType.Person},
                new Mortgagor() {FirstName = "Jane", LastName = "Doe", MortgagorID = 2, MortgagorType = PartyType.Person}
            };

            _tblMortgagors = new List<tblMortgagor>
            {
                new tblMortgagor() {FirstName = "John", LastName = "Smith", DealID = 1, MortgagorID = 1, MortgagorType = PartyType.Person},
                new tblMortgagor() {FirstName = "Jane", LastName = "Doe", MortgagorID = 2, DealID = 1, MortgagorType = PartyType.Person},
                new tblMortgagor(){FirstName = "Snow", LastName = "White", MortgagorID = 3, DealID = 2, MortgagorType = PartyType.Person}
            };

            _mapper= new Mock<IEntityMapper<tblMortgagor, Mortgagor>>();
            _mapper.Setup(m => m.MapToEntity(_mortgagors[0])).Returns(_tblMortgagors[0]);
            _mapper.Setup(m => m.MapToEntity(_mortgagors[1])).Returns(_tblMortgagors[1]);
          //  _mapper.Setup(m => m.MapToData(_tblMortgagors[0])).Returns(_mortgagors[0]);
           // _mapper.Setup(m => m.MapToData(_tblMortgagors[1])).Returns(_mortgagors[1]);
        }

        [Test]
        public void InsertRange_OK()
        {
            //ARRANGE

            _mortgagors = new MortgagorCollection()
            {
                new Mortgagor() {FirstName = "John", LastName = "Smith"},
                new Mortgagor() {FirstName = "Jane", LastName = "Doe"}
            };

            _tblMortgagors = new List<tblMortgagor>
            {
                new tblMortgagor() {FirstName = "John", LastName = "Smith"},
                new tblMortgagor() {FirstName = "Jane", LastName = "Doe"}
            };
            var mockSet = new Mock<DbSet<tblMortgagor>>();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblMortgagor>()).Returns(mockSet.Object);

            //ACT
            var repo = new MortgagorRepository(mockContext.Object, _mapper.Object);
            repo.InsertMortgagorRange(_mortgagors, 1);

            //ASSERT
            mockSet.Verify(m=>m.AddRange(It.IsAny<List<tblMortgagor>>()), Times.Exactly(2));
            mockContext.Verify(m=>m.SaveChanges(), Times.Never);
        }

        [Test]
        public void UpdateRange_ItemAdded_ItemDeleted_ItemUpdated()
        {
            //ARRANGE
            var dbset = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblMortgagor>().AsNoTracking()).Returns(dbset.Object);
            mockContext.Setup(c => c.tblMortgagors.AsNoTracking()).Returns(dbset.Object);

            var newList = new MortgagorCollection()
            {
               
                new Mortgagor()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    MortgagorID = 2,
                    MortgagorType = PartyType.Person
                },
                new Mortgagor()
                {
                    FirstName = "Ella",
                    LastName = "Cinder",
                    MortgagorType = PartyType.Person
                }
            };

            var entities = new List<tblMortgagor>
            {
                new tblMortgagor()
                {
                    FirstName = "John",
                    MortgagorID = 1,
                    LastName = "Smith"
                },
                new tblMortgagor()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    MortgagorID = 3
                }
            };


            _mapper.Setup(m => m.MapToEntity(newList[0])).Returns(entities[0]);
            _mapper.Setup(m => m.MapToEntity(newList[1])).Returns(entities[1]);

            //ACT
            var repo = new MortgagorRepository(mockContext.Object, new MortgagorMapper());
            repo.UpdateMorgagorRange(newList, 1);
            
            //ASSERT
            dbset.Verify(m=>m.Remove(It.IsAny<tblMortgagor>()), Times.Once);
            dbset.Verify(m=>m.Add(It.IsAny<tblMortgagor>()), Times.Once);
            mockContext.Verify(m=>m.SaveChanges(), Times.Once);
        }

        [Test]
        public void GetMortgagors_For_DealId()
        {
          
            var dbset = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblMortgagor>().AsNoTracking()).Returns(dbset.Object);
            mockContext.Setup(c => c.tblMortgagors.AsNoTracking()).Returns(dbset.Object);

       var repo = new MortgagorRepository(mockContext.Object, _mapper.Object);
            var result=repo.GetMortgagors(1);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count()==2);
        }

        [Test]//No assertions, test to understand insert process with sourceID
        public void InsertMortgagorsForOtherDeal_Mortgagors_Added_With_SourceID()
        {
            var dbSetMock = GetMockDbSet();
            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblMortgagor>()).Returns( dbSetMock.Object);
            mockContext.Setup(c => c.tblMortgagors).Returns( dbSetMock.Object);

            var repo = new MortgagorRepository(mockContext.Object, new MortgagorMapper());
            var newmortgagors = new List<Mortgagor>
            {
                new Mortgagor {MortgagorType = PartyType.Person, FirstName = "Snow", LastName = "White"}
            };
            repo.InsertMortgagorRangeForOtherDeal(newmortgagors, 1, 2);
           
        }

        private Mock<DbSet<tblMortgagor>> GetMockDbSet()
        {
            IQueryable<tblMortgagor> data = _tblMortgagors.AsQueryable();
            var mockSet = new Mock<DbSet<tblMortgagor>>();
            mockSet.As<IQueryable<tblMortgagor>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tblMortgagor>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tblMortgagor>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tblMortgagor>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
    }
}
