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
    public class CancelDealTest
    {
        [TestMethod]
        public void CancelDeal_SolicitorChange_LLC_Both()
        {
            Assert.IsTrue(CancelDealSuppressDealHistory("SOLICITOR_CHANGE", BusinessModel.LLC, BusinessModel.LLC, LawyerActingFor.Both));
        }
        [TestMethod]
        public void CancelDeal_SolicitorChange_Combo_Both()
        {
            Assert.IsTrue(CancelDealSuppressDealHistory("SOLICITOR_CHANGE", BusinessModel.LLC, BusinessModel.COMBO, LawyerActingFor.Both));
        }
        [TestMethod]
        public void CancelDeal_SolicitorChange_EasyFund_Both()
        {
            Assert.IsTrue(CancelDealSuppressDealHistory("SOLICITOR_CHANGE", BusinessModel.EASYFUND, BusinessModel.EASYFUND, LawyerActingFor.Both));
        }


        [TestMethod]
        public void CancelDeal_SolicitorChange_LLC_Vendor()
        {
            Assert.IsTrue(CancelDealSuppressDealHistory("SOLICITOR_CHANGE", BusinessModel.LLC, BusinessModel.LLC, LawyerActingFor.Vendor));
        }
        [TestMethod]
        public void CancelDeal_SolicitorChange_Combo_Vendor()
        {
            Assert.IsTrue(CancelDealSuppressDealHistory("SOLICITOR_CHANGE", BusinessModel.LLC, BusinessModel.COMBO, LawyerActingFor.Vendor));
        }
        [TestMethod]
        public void CancelDeal_SolicitorChange_EasyFund_Vendor()
        {
            Assert.IsTrue(CancelDealSuppressDealHistory("SOLICITOR_CHANGE", BusinessModel.EASYFUND, BusinessModel.EASYFUND, LawyerActingFor.Vendor));
        }


        [TestMethod]
        public void CancelDeal_SolicitorChange_LLC_Purchaser()
        {
            Assert.IsTrue(CancelDealSuppressDealHistory("SOLICITOR_CHANGE", BusinessModel.LLC, BusinessModel.LLC, LawyerActingFor.Purchaser));
        }
        [TestMethod]
        public void CancelDeal_SolicitorChange_Combo_Purchaser()
        {
            Assert.IsTrue(CancelDealSuppressDealHistory("SOLICITOR_CHANGE", BusinessModel.LLC, BusinessModel.COMBO, LawyerActingFor.Purchaser));
        }
        [TestMethod]
        public void CancelDeal_SolicitorChange_EasyFund_Purchaser()
        {
            Assert.IsTrue(CancelDealSuppressDealHistory("SOLICITOR_CHANGE", BusinessModel.EASYFUND, BusinessModel.EASYFUND, LawyerActingFor.Purchaser));
        }



        [TestMethod]
        public void CancelDeal_OtherReason_EasyFund_Purchaser()
        {
            Assert.IsFalse(CancelDealSuppressDealHistory("Other", BusinessModel.EASYFUND, BusinessModel.EASYFUND, LawyerActingFor.Purchaser));
        }



        private bool CancelDealSuppressDealHistory(string cancellationReason, string cancelProduct, string businessModel, string actingFor)
        {
            try
            {
                Mock<IFundingDealRepository> mockFundingDealRepository = new Mock<IFundingDealRepository>();
                Mock<IFundedRepository> mockFundedDealRepository = new Mock<IFundedRepository>();
                Mock<IDealFundsAllocRepository> mockDealFundsAllocRepository = new Mock<IDealFundsAllocRepository>();
                Mock<IDealScopeRepository> mockDealScopeRepository = new Mock<IDealScopeRepository>();
                Mock<ILawyerRepository> mockLawyerRepository = new Mock<ILawyerRepository>();
                Mock<IDealHistoryRepository> mockDealHistoryRepository = new Mock<IDealHistoryRepository>();
                Mock<ICancelDealBusinessLogic> mockCancelDealBusinessLogic = new Mock<ICancelDealBusinessLogic>();
                Mock<IMortgageRepository> mockMortgageRepository = new Mock<IMortgageRepository>();
                Mock<IMortgagorRepository> mockMortgagorRepository = new Mock<IMortgagorRepository>();
                Mock<IDealContactRepository> mockDealContactDetailsRepository = new Mock<IDealContactRepository>();
                Mock<ICancelDealHelper> mockCancelDealHelper = new Mock<ICancelDealHelper>();
                Mock<IPropertyRepository> mockPropertyRepository = new Mock<IPropertyRepository>();
                Mock<IPINRepository> mockPINRepository = new Mock<IPINRepository>();
                Mock<IDealRepository> mockDealRepository = new Mock<IDealRepository>();
                Mock<IGlobalizationRepository> mockGlobalizationRepository = new Mock<IGlobalizationRepository>();
                Mock<IAuditLogService> mockAuditLogService = new Mock<IAuditLogService>();
                Mock<IDealEventsPublisher> mockDealEventsPublisher = new Mock<IDealEventsPublisher>();
                Mock<IDealContactRepository> mockDealContactReposiory = new Mock<IDealContactRepository>();

                CancelDealBusinessLogic business = new CancelDealBusinessLogic(
                    mockFundingDealRepository.Object,
                    mockFundedDealRepository.Object,
                    mockDealFundsAllocRepository.Object,
                    mockDealScopeRepository.Object,
                    mockLawyerRepository.Object,
                    mockDealHistoryRepository.Object,
                    mockCancelDealHelper.Object,
                    mockMortgagorRepository.Object,
                    mockMortgageRepository.Object,
                    mockDealContactReposiory.Object,
                    mockPropertyRepository.Object,
                    mockPINRepository.Object,
                    mockDealRepository.Object,
                    mockGlobalizationRepository.Object,
                    mockAuditLogService.Object,
                    mockDealEventsPublisher.Object);

                CancelDealRequest request = new CancelDealRequest();
                request.CancellationReason = cancellationReason;
                request.DealID = 1;
                request.StatusReasonID = null;
                request.UserContext = new UserContext()
                {
                    UserID = "test",
                    UserType = UserType.LENDER
                };
                if (cancelProduct == BusinessModel.LLC)
                {
                    request.CancelledProduct = CancelledProduct.LLC;
                }
                if (cancelProduct == BusinessModel.EASYFUND)
                {
                    request.CancelledProduct = CancelledProduct.EF;
                }


                mockFundingDealRepository.Setup(g => g.GetFundingDeal(1)).Returns(new FundingDeal()
                {
                    DealID = 1,
                    BusinessModel = businessModel,
                    ActingFor = actingFor,
                    Lawyer = new Lawyer()
                    {
                        LawyerID = 1
                    },
                    DealStatus = DealStatus.Active,
                    DealFCTURN = "1"
                });

                mockFundedDealRepository.Setup(f => f.GetMilestonesByDeal(1)).Returns(new FundedDeal()
                {
                    Milestone = new FundingMilestone()
                    {
                        Funded = DateTime.Now
                    },
                    FundingDealId = 1
                });

                mockFundingDealRepository.Setup(f => f.GetOtherDealStatus(1)).Returns(new Lookup()
                {
                    Key = "2",
                    Value = "2"
                });
                mockDealRepository.Setup(d => d.GetDealDetails(1, It.IsAny<UserContext>(), It.IsAny<bool>())).Returns(new tblDeal()
                {
                    FCTRefNum = "1",
                    LenderRefNum = "2"
                });


                mockFundingDealRepository.Setup(f=>f.GetExistingDeal(It.IsAny<FundingDeal>(), It.IsAny<int>())).Returns(100);
                //mockDealHistoryRepository.Setup(d => d.CreateCancelDealHistory(1, It.IsAny<string>(), It.IsAny<UserContext>()));
                mockDealHistoryRepository.Setup(d => d.CreateCancelDealHistory(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserContext>())).Throws(new Exception("error"));
                mockDealHistoryRepository.Setup(d => d.CreateDealHistory(It.IsAny<int>(), It.IsAny<string>())).Throws(new Exception("error"));
                mockDealHistoryRepository.Setup(d => d.CreateDealHistory(It.IsAny<int>(), It.IsAny<UserContext>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("error"));
                mockDealHistoryRepository.Setup(d => d.CreateDealHistoryByDealHistoryEntry(It.IsAny<int>(), It.IsAny<DealHistoryEntry>(), It.IsAny<UserContext>(), It.IsAny<bool>())).Throws(new Exception("error"));
                mockDealHistoryRepository.Setup(d => d.CreateDealHistoryByStatus(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserContext>())).Throws(new Exception("error"));
                mockDealHistoryRepository.Setup(d => d.CreateLLCDealHistory(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UserContext>(), It.IsAny<string>(), It.IsAny<int>())).Throws(new Exception("error"));
                //ACT
                business.CancelDeal(request);
            }
            catch (Exception e)
            {
                if (e.Message == "error")
                {
                    return false;
                }
                else
                {
                    throw new Exception("exception", e);
                }

            }
            return true;

        }
    }
}
/*
IFundingDealRepository fundingDealRepository, IFundedRepository fundedRepository,
            IDealFundsAllocRepository dealFundsAllocRepository, IDealScopeRepository dealScopeRepository,
            ILawyerRepository lawyerRepository, IDealHistoryRepository dealHistoryRepository, ICancelDealHelper cancelDealHelper,
            IMortgagorRepository mortgagorRepository, IMortgageRepository mortgageRepository,
            IDealContactRepository dealContactRepository, IPropertyRepository propertyRepository,
            IPINRepository pinRepository, IDealRepository dealRepository,
            IGlobalizationRepository globalizationRepository,
            IAuditLogService auditLogService, IDealEventsPublisher dealEventsPublisher

*/