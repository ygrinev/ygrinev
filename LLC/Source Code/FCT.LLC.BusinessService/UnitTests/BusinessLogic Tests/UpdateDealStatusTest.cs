using Moq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.Services.AuditLog.ServiceContracts;

namespace FCT.LLC.BusinessService.UnitTests.BusinessLogic_Tests
{
    [TestClass]
    public class UpdateDealStatusTest
    {
        [TestMethod]
        public void UpdateDealStatusTest_CancelTest_Active_Cancelled_MMSCOMBO()
        {            
            // ASSERT
            Assert.IsFalse(IsDealHistoryCalledOnCancel(DealStatus.Active, DealStatus.Cancelled, BusinessModel.MMSCOMBO));
        }

        [TestMethod]
        public void UpdateDealStatusTest_CancelTest_Active_Cancelled_MMS()
        {
            // ASSERT
            Assert.IsFalse(IsDealHistoryCalledOnCancel(DealStatus.Active, DealStatus.Cancelled, BusinessModel.MMS));
        }

        [TestMethod]
        public void UpdateDealStatusTest_CancelTest_Active_Cancelled_LLC()
        {
            // ASSERT
            Assert.IsTrue(IsDealHistoryCalledOnCancel(DealStatus.Active, DealStatus.Cancelled, BusinessModel.LLC));
        }

        [TestMethod]
        public void UpdateDealStatusTest_CancelTest_Active_CancelRequest_MMS()
        {
            // ASSERT
            Assert.IsTrue(IsDealHistoryCalledOnCancel(DealStatus.Active, DealStatus.CancelRequest, BusinessModel.MMS));
        }

        private bool IsDealHistoryCalledOnCancel(string fromDealStatus, string toDealStatus, string businessModel) {
            int dealHistoryCalled = 0;
            // ARRANGE
                var mockRepository = new MockRepository(MockBehavior.Default);
                var argsFactory = new ArgumentsFactory(mockRepository);
                Mock<IDealRepository> mockDealRepository = new Mock<IDealRepository>();
                Mock<IEntityMapper<tblDeal, Deal>> mockDealEntityMapper = new Mock<IEntityMapper<tblDeal, Deal>>();
                Mock<IDealHistoryRepository> mockDealHistoryRepository = new Mock<IDealHistoryRepository>();
                Mock<IFundedRepository> mockFundedRepository = new Mock<IFundedRepository>();
                Mock<IAuditLogService> mockAuditLogService = new Mock<IAuditLogService>();
                Mock<IGlobalizationRepository> mockGlobalizationRepository = new Mock<IGlobalizationRepository>();
                Mock<IFundingDealRepository> mockFundingDealRepository = new Mock<IFundingDealRepository>();

                var business = new DealBusinessLogic(mockDealRepository.Object, mockDealEntityMapper.Object, mockDealHistoryRepository.Object,
                    mockFundedRepository.Object, mockAuditLogService.Object, mockGlobalizationRepository.Object, mockFundingDealRepository.Object);

                UpdateDealStatusRequest request = new UpdateDealStatusRequest()
                {
                    DealID = 1,
                    DealStatus = toDealStatus,
                    UserContext = new UserContext()
                    {
                        UserID = "test",
                        UserType = UserType.SYSTEM
                    }
                };

                mockDealRepository.Setup(g => g.GetDealDetails(1, request.UserContext, false)).Returns(new tblDeal()
                {
                    DealID = 1,
                    Status = fromDealStatus,
                    BusinessModel = businessModel
                });

                mockDealHistoryRepository.Setup(d => d.CreateDealHistoryByStatus(1, fromDealStatus, toDealStatus, It.IsAny<string>(), request.UserContext)).Callback(() => { dealHistoryCalled = 1; });

                // ACT
                business.UpdateDealStatus(request);
                return dealHistoryCalled == 1;
        }

    }
}
