using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace FCT.LLC.BusinessService.UnitTests
{
    //Vendor and DealContact Repositories both have similar methods and behaviors as Mortgagor Repository
    [TestClass]
    public class DealFundAllocationTest
    {
        private DealFundsAllocationCollection _dfAllocations;
        private List<tblDealFundsAllocation> _tbldfAllocations;
        private Mock<IEntityMapper<tblDealFundsAllocation, DealFundsAllocation>> _mapper;
        private Mock<IEntityMapper<tblDealFundsAllocation, PaymentNotification>> _paymapper;

        
        [TestInitialize]
        public void Init()
        {
            _dfAllocations = new DealFundsAllocationCollection()
            {
                new DealFundsAllocation() { AllocationStatus = "f", LawyerID =17, LawyerName = "naga",DealFundsAllocationID=12 },
                new DealFundsAllocation() { AllocationStatus = "f", LawyerID =17, LawyerName = "naga",DealFundsAllocationID=12}
            };

            _tbldfAllocations = new List<tblDealFundsAllocation>
            {
                new tblDealFundsAllocation() {AllocationStatus = "f", LawyerID =17, ReferenceNumber= "", DealFundsAllocationID=12},
                new tblDealFundsAllocation() {AllocationStatus = "f", LawyerID =17, ReferenceNumber= "", DealFundsAllocationID=12}
            };

            _mapper = new Mock<IEntityMapper<tblDealFundsAllocation, DealFundsAllocation>>();
            _mapper.Setup(m => m.MapToEntity(_dfAllocations[0])).Returns(_tbldfAllocations[0]);
            _mapper.Setup(m => m.MapToEntity(_dfAllocations[1])).Returns(_tbldfAllocations[1]);

            _paymapper=new Mock<IEntityMapper<tblDealFundsAllocation, PaymentNotification>>();
          //  _mapper.Setup(m => m.MapToData(_tblMortgagors[0])).Returns(_mortgagors[0]);
           // _mapper.Setup(m => m.MapToData(_tblMortgagors[1])).Returns(_mortgagors[1]);
        }

  /*      [Test]
        public void InsertRange_OK()
        {
            //ARRANGE

            _mortgagors = new MortgagorCollection()
            {
                new Mortgagor() {FirstName = "John", LastName = "Smith"},
                new Mortgagor() {FirstName = "Jane", LastName = "Doe"}
            };

            _tblMortgagors = new List<tblDealFundsAllocation>
            {
                new tblDealFundsAllocation() {FirstName = "John", LastName = "Smith"},
                new tblDealFundsAllocation() {FirstName = "Jane", LastName = "Doe"}
            };
            var mockSet = new Mock<DbSet<tblMortgagor>>();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblMortgagor>()).Returns(mockSet.Object);

            //ACT
            var repo = new DealFundsAllocationRepository(mockContext.Object, _mapper.Object);
            repo.InsertMortgagorRange(_dfAllocations, 1);

            //ASSERT
            mockSet.Verify(m=>m.Add(It.IsAny<tblMortgagor>()), Times.Exactly(2));
            mockContext.Verify(m=>m.SaveChanges(), Times.Never);
        }
        */
       [TestMethod]

        public void UpdateRange_ItemAdded_ItemDeleted_ItemUpdated()
        {
            //ARRANGE
            var dbset = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDealFundsAllocation>().AsNoTracking()).Returns(dbset.Object);
            mockContext.Setup(c => c.tblDealFundsAllocations.AsNoTracking()).Returns(dbset.Object);

            var newList = new DealFundsAllocationCollection()
            {
                new DealFundsAllocation()
                {
                    AllocationStatus = "f", LawyerID =17, LawyerName = "naga",DealFundsAllocationID=12
                },
                new DealFundsAllocation()
                {
                   AllocationStatus = "f", LawyerID =17, LawyerName = "naga",DealFundsAllocationID=12
                }
            };

            var entities = new List<tblDealFundsAllocation>
            {
                new tblDealFundsAllocation()
                {
                    AllocationStatus = "f", LawyerID =17, ShortFCTRefNumber = "1232",DealFundsAllocationID=12
                },
                new tblDealFundsAllocation()
                {
                     AllocationStatus = "f", LawyerID =17, ShortFCTRefNumber = "12312",DealFundsAllocationID=12
                }
            };


            _mapper.Setup(m => m.MapToEntity(newList[0])).Returns(entities[0]);
            _mapper.Setup(m => m.MapToEntity(newList[1])).Returns(entities[1]);

              
            //ACT
            var repo = new DealFundsAllocRepository(mockContext.Object, _mapper.Object, _paymapper.Object);
         //   repo.UpdateFundsAllocation(newList[1]);
            
            //ASSERT
       
            mockContext.Verify(m=>m.SaveChanges(), Times.Once);
        }

      [TestMethod]

        public void GetDealFundAllocation_For_DealId()
        {
          
            var dbset = GetMockDbSet();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDealFundsAllocation>().AsNoTracking()).Returns(dbset.Object);
            mockContext.Setup(c => c.tblDealFundsAllocations.AsNoTracking()).Returns(dbset.Object);

            var repo = new DealFundsAllocRepository(mockContext.Object, _mapper.Object, _paymapper.Object);
            var result=repo.GetAll();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count()==2);
        }

        private Mock<DbSet<tblDealFundsAllocation>> GetMockDbSet()
        {
            IQueryable<tblDealFundsAllocation> data = _tbldfAllocations.AsQueryable();
            var mockSet = new Mock<DbSet<tblDealFundsAllocation>>();
            mockSet.As<IQueryable<tblDealFundsAllocation>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tblDealFundsAllocation>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tblDealFundsAllocation>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tblDealFundsAllocation>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
    }
}
