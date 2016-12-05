using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Logging;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class FundingDealRepositoryTests
    {
        private IFundingDealRepository _repository;
        private EFBusinessContext _context;

        //This is an integration test due to Moq's inability to mock Include() method. set DealID to an actual deal ID with dealscope
        [Test]
        public void GetDealFunding_via_Context()
        {
            _context = new EFBusinessContext();
            var contactmapper = new ContactMapper();
            var lawyermapper = new LawyerMapper(contactmapper);
            var mortgagormapper = new MortgagorMapper();
            var vendormapper = new VendorMapper();
            var pinmapper = new PINMapper();
            var propertymapper = new PropertyMapper(pinmapper);
            var dealmapper = new FundingDealMapper(lawyermapper, mortgagormapper, vendormapper, propertymapper);
            var logger = new EnterpriseLibraryLogger();
            //_repository = new FundingDealRepository(_context, dealmapper, lawyermapper, logger);
            FundingDeal result = _repository.GetFundingDeal(1);
            Assert.Null(result);

        }

        [Test]
        public void InsertDealFunding_via_Context()
        {
            var deal = new FundingDeal {DealID = 3};
            var tblDeal = new tblDeal {DealID = 3};
            var mockSet = new Mock<DbSet<tblDeal>>();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDeal>()).Returns(mockSet.Object);

            var dealmapper = new Mock<IEntityMapper<tblDeal, FundingDeal>>();
            dealmapper.Setup(d => d.MapToEntity(deal)).Returns(tblDeal);

            var lawyermapper = new Mock<IEntityMapper<tblLawyer, Lawyer>>();

            var logger = new Mock<ILogger>();

            //var repo = new FundingDealRepository(mockContext.Object, dealmapper.Object, lawyermapper.Object,
            //    logger.Object);
            //repo.InsertFundingDeal(deal, 1);
            mockSet.Verify(m => m.Add(It.IsAny<tblDeal>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce);
        }

        [Test]
        public void UpdateDealFunding_via_Context()
        {
            var deal = new FundingDeal {DealID = 3};
            var tblDeal = new tblDeal {DealID = 3};
            var mockSet = new Mock<DbSet<tblDeal>>();

            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDeal>()).Returns(mockSet.Object);

            var dealmapper = new Mock<IEntityMapper<tblDeal, FundingDeal>>();
            dealmapper.Setup(d => d.MapToEntity(deal)).Returns(tblDeal);

            var lawyermapper = new Mock<IEntityMapper<tblLawyer, Lawyer>>();

            var logger = new Mock<ILogger>();

            //var repo = new FundingDealRepository(mockContext.Object, dealmapper.Object, lawyermapper.Object,
            //    logger.Object);
            //repo.UpdateFundingDeal(deal,0);
            mockContext.Verify(m => m.SaveChanges(), Times.AtLeastOnce());
        }

        [Test]
        public void Get_OtherDeal_From_DealScope()
        {
            var dbset = GetMockDbSet();
            var mockContext = new Mock<EFBusinessContext>();
            mockContext.Setup(c => c.Set<tblDeal>().AsNoTracking()).Returns(dbset.Object);
            mockContext.Setup(c => c.tblDeals.AsNoTracking()).Returns(dbset.Object);
            var dealmapper = new Mock<IEntityMapper<tblDeal, FundingDeal>>();

            var lawyermapper = new Mock<IEntityMapper<tblLawyer, Lawyer>>();

            var logger = new Mock<ILogger>();

            //var repo = new FundingDealRepository(mockContext.Object, dealmapper.Object, lawyermapper.Object,
            //    logger.Object);
            //var result = repo.GetOtherDealInScope(1, 1);

            //Assert.IsTrue(result==2);
        }

       // [Test]
        [ExpectedException(typeof(DbUpdateException))]
        public void Insert_FundingDeal_throws_DataAccessException()
        {
            _context = new EFBusinessContext();
            var contactmapper = new ContactMapper();
            var lawyermapper = new LawyerMapper(contactmapper);
            var mortgagormapper = new MortgagorMapper();
            var vendormapper = new VendorMapper();
            var pinmapper = new PINMapper();
            var propertymapper = new PropertyMapper(pinmapper);
            var dealmapper = new FundingDealMapper(lawyermapper, mortgagormapper, vendormapper, propertymapper);
            var logger = new EnterpriseLibraryLogger();
            //_repository = new FundingDealRepository(_context, dealmapper, lawyermapper, logger);
            var deal = new FundingDeal { DealID = 3 , Lawyer = new Lawyer(){LawyerID = 1}};
            var result= _repository.InsertFundingDeal(deal, 1); 
        }

        private Mock<DbSet<tblDeal>> GetMockDbSet()
        {
            var deals = new List<tblDeal>
            {
                new tblDeal() {DealID = 1, DealScopeID = 1},
                new tblDeal() {DealID = 2, DealScopeID = 1}
            };
            IQueryable<tblDeal> data = deals.AsQueryable();
            var mockSet = new Mock<DbSet<tblDeal>>();
            mockSet.As<IQueryable<tblDeal>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<tblDeal>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<tblDeal>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<tblDeal>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
    }
}
