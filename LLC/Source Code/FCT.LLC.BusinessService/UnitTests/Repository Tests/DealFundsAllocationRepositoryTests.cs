using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class DealFundsAllocationRepositoryTests
    {
        private List<tblDealFundsAllocation> _tblDealFundsAllocations ;
        private List<tblFundingDeal> _tblFundingDeals;
        private List<tblDealScope> _tblDealScopes;
        private List<tblDeal> _tblDeals; 
 
        private DealFundsAllocationCollection _dealFundsAllocations;
        private Mock<IEntityMapper<tblDealFundsAllocation, DealFundsAllocation>> _fundsMapper;
        private Mock<IEntityMapper<tblDealFundsAllocation, PaymentNotification>> _paymentMapper;

        [TestFixtureSetUp]
        public void Init()
        {
            LoadDummyData();
        }

        [Test]
        public void GetTotalAllocatedFunds_WithFee()
        {
            //ARRANGE
            var mockSet = GetDealFundsAllocationMockDbSet();
            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDealFundsAllocation>()).Returns(mockSet.Object);
            mockContext.Setup(c => c.tblDealFundsAllocations.AsNoTracking()).Returns(mockSet.Object);
            //ACT
            var repo = new DealFundsAllocRepository(mockContext.Object, _fundsMapper.Object, _paymentMapper.Object);
            decimal total = repo.GetTotalAllocatedFunds(10);

            //ASSERT
            Assert.AreEqual(total,40);
        }

        [Test]
        public void IsDuplicateDeposit_FoundDuplicate()
        {
            //ARRANGE
            var mockSet = GetDealFundsAllocationMockDbSet();
            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDealFundsAllocation>()).Returns(mockSet.Object);
            mockContext.Setup(c => c.tblDealFundsAllocations.AsNoTracking()).Returns(mockSet.Object);

            //ACT
            var repo = new DealFundsAllocRepository(mockContext.Object, _fundsMapper.Object, _paymentMapper.Object);
            bool duplicateDeposit = repo.IsDuplicateDeposit("qwerty1234");

            //ASSERT
            Assert.That(duplicateDeposit);

        }

        [Test]
        public void GetFCTURNFromDealFundsAllocationID_Succeeds()
        {
            //ARRANGE
            var mockSet = GetDealFundsAllocationMockDbSet();
            var mockFundsSet = GetFundingDealMockDbSet();
            var mockScopeSet = GetDealScopeMockDbSet();
            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDealFundsAllocation>()).Returns(mockSet.Object);
            mockContext.Setup(c => c.Set<tblFundingDeal>()).Returns(mockFundsSet.Object);
            mockContext.Setup(c => c.Set<tblDealScope>()).Returns(mockScopeSet.Object);
            mockContext.Setup(c => c.tblDealFundsAllocations.AsNoTracking()).Returns(mockSet.Object);
            mockContext.Setup(c => c.tblFundingDeals.AsNoTracking()).Returns(mockFundsSet.Object);
            mockContext.Setup(c => c.tblDealScopes.AsNoTracking()).Returns(mockScopeSet.Object);

            //ACT
            var repo = new DealFundsAllocRepository(mockContext.Object, _fundsMapper.Object, _paymentMapper.Object);
            string fctRef = repo.GetFCTURNByItemID(1);

            //ASSERT
            Assert.IsTrue(fctRef == "15-111-01");
        }

    [Test]
        public void GetDeposits_ValidDealID()
        {
        //ARRANGE
        var mockSet = GetDealFundsAllocationMockDbSet();
        var dealMockDbSet = GetDealMockDbSet();
        var mockFundingDealSet = GetFundingDealMockDbSet();
        var mockContext = new Mock<EFBusinessContext>();
        mockContext.Setup(c => c.Set<tblDealFundsAllocation>()).Returns(mockSet.Object);
        mockContext.Setup(c => c.Set<tblDeal>()).Returns(dealMockDbSet.Object);
        mockContext.Setup(c => c.Set<tblFundingDeal>()).Returns(mockFundingDealSet.Object);
        mockContext.Setup(c => c.tblDealFundsAllocations.AsNoTracking()).Returns(mockSet.Object);
        mockContext.Setup(c => c.tblDeals.AsNoTracking()).Returns(dealMockDbSet.Object);
        mockContext.Setup(c => c.tblFundingDeals.AsNoTracking()).Returns(mockFundingDealSet.Object);

            //ACT
            var repo = new DealFundsAllocRepository(mockContext.Object, _fundsMapper.Object, _paymentMapper.Object);
            var results = repo.GetDeposits(101);

            //ASSERT
            Assert.That(results.Any());
        }

        private void LoadDummyData()
        {
            _dealFundsAllocations = new DealFundsAllocationCollection()
            {
                new DealFundsAllocation()
                {
                    DealFundsAllocationID = 1,
                    FundingDealID = 10,
                    Amount = 200,
                    AllocationStatus = AllocationStatus.Allocated,
                    RecordType = RecordType.Deposit,
                    ReferenceNumber = "qwerty1234"
                },
                new DealFundsAllocation()
                {
                    DealFundsAllocationID = 2,
                    Amount = 100,
                    AllocationStatus = AllocationStatus.UnAssigned,
                    RecordType = RecordType.Deposit,
                    ReferenceNumber = "asdf1234"
                },
                new DealFundsAllocation()
                {
                    DealFundsAllocationID = 3,
                    AllocationStatus = AllocationStatus.PendingAckowledgement,
                    RecordType = RecordType.Return,
                    Amount = 100,                   
                    FundingDealID = 12
                },
                new DealFundsAllocation()
                {
                    DealFundsAllocationID = 4,
                    AllocationStatus = AllocationStatus.Disbursed,
                    RecordType = RecordType.Return,
                    Amount = 120,
                    FundingDealID = 10,
                },
                 new DealFundsAllocation()
                {
                    DealFundsAllocationID = 5,
                    AllocationStatus = AllocationStatus.Disbursed,
                    RecordType = RecordType.FCTFee,
                    Amount = 40,
                    FundingDealID = 10,
                },
                  new DealFundsAllocation()
                {
                    DealFundsAllocationID = 5,
                    AllocationStatus = AllocationStatus.PendingAckowledgement,
                    RecordType = RecordType.FCTFee,
                    Amount = Convert.ToDecimal(41.30),
                     Fee = new Fee()
                    {
                        FeeID = 1,
                        Amount = 40,
                        HST = 13
                    },
                    FundingDealID = 12,
                },
            };

            _tblDealFundsAllocations = new List<tblDealFundsAllocation>()
            {
                new tblDealFundsAllocation()
                {
                    DealFundsAllocationID = 1,
                    FundingDealID = 10,
                    Amount = 200,
                    AllocationStatus = AllocationStatus.Allocated,
                    RecordType = RecordType.Deposit,
                    ReferenceNumber = "qwerty1234"
                },
                new tblDealFundsAllocation()
                {
                    DealFundsAllocationID = 2,
                    Amount = 100,
                    AllocationStatus = AllocationStatus.UnAssigned,
                    RecordType = RecordType.Deposit,
                    ReferenceNumber = "asdf1234"
                },
                new tblDealFundsAllocation()
                {
                    DealFundsAllocationID = 3,
                    AllocationStatus = AllocationStatus.PendingAckowledgement,
                    RecordType = RecordType.Return,
                    Amount = 100,
                    FundingDealID = 12
                },
                new tblDealFundsAllocation()
                {
                    DealFundsAllocationID = 4,
                    AllocationStatus = AllocationStatus.Disbursed,
                    RecordType = RecordType.Return,
                    Amount = 120,
                    FundingDealID = 10,
                },
                  new tblDealFundsAllocation()
                {
                    DealFundsAllocationID = 5,
                    AllocationStatus = AllocationStatus.Disbursed,
                    RecordType = RecordType.FCTFee,
                    Amount = 40,
                    FundingDealID = 10,
                },
                    new tblDealFundsAllocation()
                {
                    DealFundsAllocationID = 5,
                    AllocationStatus = AllocationStatus.PendingAckowledgement,
                    RecordType = RecordType.FCTFee,
                    Amount = Convert.ToDecimal(41.30),
                    FeeID = 1,
                    FundingDealID = 12,
                },
            };

            _tblFundingDeals = new List<tblFundingDeal>()
            {
                new tblFundingDeal()
                {
                    FundingDealID = 10,
                    DealScopeID = 1
                }
            };
            _tblDealScopes = new List<tblDealScope>()
            {
                new tblDealScope()
                {
                    DealScopeID = 1,
                    ShortFCTRefNumber = "15-111-01"
                }
            };
           _tblDeals= new List<tblDeal>()
            {
               new tblDeal()
               {
                   DealID = 101,
                   DealScopeID = 1
               }
            };

            _fundsMapper = new Mock<IEntityMapper<tblDealFundsAllocation, DealFundsAllocation>>();
            _paymentMapper=new Mock<IEntityMapper<tblDealFundsAllocation, PaymentNotification>>();
            _fundsMapper.Setup(f => f.MapToData(_tblDealFundsAllocations[0], null)).Returns(_dealFundsAllocations[0]);
            _fundsMapper.Setup(f => f.MapToData(_tblDealFundsAllocations[1], null)).Returns(_dealFundsAllocations[1]);
            _fundsMapper.Setup(f => f.MapToData(_tblDealFundsAllocations[2], null)).Returns(_dealFundsAllocations[2]);
            _fundsMapper.Setup(f => f.MapToData(_tblDealFundsAllocations[3], null)).Returns(_dealFundsAllocations[3]);
            _fundsMapper.Setup(f => f.MapToData(_tblDealFundsAllocations[4], null)).Returns(_dealFundsAllocations[4]);
            _fundsMapper.Setup(f => f.MapToData(_tblDealFundsAllocations[5], null)).Returns(_dealFundsAllocations[5]);

            _fundsMapper.Setup(f => f.MapToEntity(_dealFundsAllocations[0])).Returns(_tblDealFundsAllocations[0]);
            _fundsMapper.Setup(f => f.MapToEntity(_dealFundsAllocations[1])).Returns(_tblDealFundsAllocations[1]);
            _fundsMapper.Setup(f => f.MapToEntity(_dealFundsAllocations[2])).Returns(_tblDealFundsAllocations[2]);
            _fundsMapper.Setup(f => f.MapToEntity(_dealFundsAllocations[3])).Returns(_tblDealFundsAllocations[3]);
            _fundsMapper.Setup(f => f.MapToEntity(_dealFundsAllocations[4])).Returns(_tblDealFundsAllocations[4]);
            _fundsMapper.Setup(f => f.MapToEntity(_dealFundsAllocations[5])).Returns(_tblDealFundsAllocations[5]);
        }

        private Mock<DbSet<tblDealFundsAllocation>> GetDealFundsAllocationMockDbSet()
        {
            IQueryable<tblDealFundsAllocation> data = _tblDealFundsAllocations.AsQueryable();
            var mockSet = new Mock<DbSet<tblDealFundsAllocation>>();
            mockSet.As<IQueryable<tblDealFundsAllocation>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tblDealFundsAllocation>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tblDealFundsAllocation>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tblDealFundsAllocation>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<tblFundingDeal>> GetFundingDealMockDbSet()
        {
            IQueryable<tblFundingDeal> data = _tblFundingDeals.AsQueryable();
            var mockSet = new Mock<DbSet<tblFundingDeal>>();
            mockSet.As<IQueryable<tblFundingDeal>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tblFundingDeal>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tblFundingDeal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tblFundingDeal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<tblDealScope>> GetDealScopeMockDbSet()
        {
            IQueryable<tblDealScope> data = _tblDealScopes.AsQueryable();
            var mockSet = new Mock<DbSet<tblDealScope>>();
            mockSet.As<IQueryable<tblDealScope>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tblDealScope>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tblDealScope>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tblDealScope>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }

        private Mock<DbSet<tblDeal>> GetDealMockDbSet()
        {
            IQueryable<tblDeal> data = _tblDeals.AsQueryable();
            var mockSet = new Mock<DbSet<tblDeal>>();
            mockSet.As<IQueryable<tblDeal>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tblDeal>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tblDeal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tblDeal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
    }

    
}
