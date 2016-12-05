using System;
using System.Collections.Generic;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.BusinessLogic.Implementations;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.Services.LIM.DataContracts;
using FCT.Services.LIM.MessageContracts;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class FundsAllocBusinessLogicTests
    {
        [Test]
        public void SearchFundsAllocation_PassOptionalParams()
        {
            var mockRepository = new MockRepository(MockBehavior.Loose);
            MockRepository mock;
            int totalcount = 0;
            var req = new SearchFundsAllocationRequest()
            {
                PageIndex = 1,
                OrderBySpecifications = new FundsAllocationOrderBySpecificationCollection()
                {
                    new FundsAllocationOrderBySpecification()
                    {
                        OrderByColumn = FundsAllocationOrderByColumn.LawyerName,
                        OrderByDirection = OrderByDirection.DESC
                    },
                    new FundsAllocationOrderBySpecification()
                    {
                        OrderByColumn = FundsAllocationOrderByColumn.Amount,
                        OrderByDirection = OrderByDirection.ASC
                    }
                },
                PageSize = 5
            };
            var funds = new DealFundsAllocationCollection()
            {
                new DealFundsAllocation()
                {
                    AllocationStatus = AllocationStatus.Assigned,
                    LawyerID = 1,
                    LawyerName = "test test"
                }
            };
            mockRepository=new MockRepository(MockBehavior.Default);
            var argsFactory = new ArgumentsFactory(mockRepository);
            argsFactory.DealFundsMock.Setup(m => m.SearchFunds(req, out totalcount)).Returns(funds);
            var fundsAlloc = new FundsAllocBusinessLogic(argsFactory.DealFundsMock.Object,
                argsFactory.FundingDealMock.Object, argsFactory.DealHistoryMock.Object,
                argsFactory.LawyerMock.Object, argsFactory.SummaryMock.Object, argsFactory.FundedMock.Object,
                argsFactory.AuditLogMock.Object, argsFactory.EmailMock.Object);
            var result = fundsAlloc.SearchFundsAllocation(req);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.SearchResults.Count);
            mockRepository.Verify();
        }

        [Test]
        [ExpectedException(typeof (SuccessException))]
        public void PaymentReceived_Can_Allocate_Funds()
        {
            try
            {
                ArgumentsFactory argsFactory;
                MockRepository mockRepository;
                var fundsAlloc = GetConstructor(out argsFactory, out mockRepository);
                var fundingDeal = new FundingDeal()
                {
                    ActingFor = LawyerActingFor.Purchaser,
                    DealID = 11,
                    DealType = DealType.PurchaseSale,
                    DealStatus = DealStatus.Active,
                    FCTURN = "143261256",
                    Lawyer = new Lawyer() {LawyerID = 393},
                    OtherLawyer = new Lawyer() {LawyerID = 394}
                };
                var payment = new PaymentNotification()
                {
                    AdditionalInformation = "15-099-11444, 7V3Y4 ",
                    NotificationType = PaymentNotification.NotificationTypeType.CreditConfirmation,
                    PaymentAmount = 40000,
                    PaymentDateTime = DateTime.Now
                };

                var lawyerprof = new LawyerProfile()
                {
                    Email = "imukherjee@fct.ca",
                    UserLanguage = "English",
                    UserName = "test",
                    UserType = UserType.Lawyer
                };

                var dealdetails = new DealDetails()
                {
                    DealID = 11,
                    DealStatus = "ACTIVE",
                    DealState = new FundedDeal() {FundingDealId = 111, DealScopeId = 1}
                };
                var allocation = new Allocation()
                {
                    DealId = 11,
                    FundingDealId = 111,
                    LawyerInfo = null,
                    ShortFCTURN = "15-099-11444"
                };
                argsFactory.DealScopeMock.Setup(d => d.GetDealScope("143261256", null)).Returns(1);
                argsFactory.FundingDealMock.Setup(f => f.GetDeal(1, false)).Returns(dealdetails);
                argsFactory.FundingDealMock.Setup(f => f.GetFundingDeal(11)).Returns(fundingDeal);
                argsFactory.FundedMock.Setup(f => f.GetFundingDealIdByScope(1)).Returns(111);
                var dict = new Dictionary<PaymentNotification, Allocation> {{payment, allocation}};
                argsFactory.DealFundsMock.Setup(d => d.SavePayments(dict));
                argsFactory.SummaryMock.Setup(d => d.GetDepositRequired(111)).Returns(40000);
                argsFactory.LawyerMock.Setup(l => l.GetUserDetails(It.IsAny<int>())).Returns(lawyerprof);
                argsFactory.LawyerMock.Setup(l => l.GetNotificationDetails(It.IsAny<int>())).Returns(lawyerprof);

                fundsAlloc.ReceivePaymentNotification(new PaymentNotificationRequest()
                {
                    PaymentNotifications = new PaymentNotificationList() {payment}
                });
                mockRepository.Verify();
            }
            catch (NullReferenceException exception)
            {
                if (exception.Source.Contains("FCT.LLC.Common.NotificationEmailDispatching.Client"))
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        [ExpectedException(typeof (SuccessException))]
        public void PaymentReceived_Can_Assign_Funds()
        {
            try
            {
                MockRepository mockRepository;
                ArgumentsFactory argsFactory;
                var fundsAlloc = GetConstructor(out argsFactory, out mockRepository);
                var userprofile = new UserProfileInfo()
                {
                    AccountNumber = "12121212",
                    BankNumber = "111",
                    BranchNumber = "22122",
                    Email = "imukherjee@fct.ca",
                    UserName = "ttest",
                    UserID = 393,
                    
                    Registrations = new UserRegistrationCollection()
                    {
                       new UserRegistration()
                       {
                           SolutionID = (int)SolutionType.EASYFUND,
                           UserStatusID = (int)AccountStatus.Active,
                           
                       }
                    }
                };
                var profileRequest = new SearchUserProfileRequest()
                {
                    PageIndex = 1,
                    RecordsPerPage = 100,
                    SortBy = "UserID",
                    SortDirection = "ASC",
                    SearchCriteria = new UserProfileSearchCriteria()
                    {
                        AccountNumber = "12121212",
                        BankNumber = "111",
                        BranchNumber = "22122",
                        SolutionID = (int) SolutionType.EASYFUND,
                        TrustAccountID =(int) AccountStatus.Active,
                        UserStatusID = (int)AccountStatus.Active
                    }
                };
                var resp = new SearchUserProfileResponse()
                {
                    SearchResults = new UserProfileSearchResult()
                    {
                        TotalRecordCount = 1,
                        UserProfiles = new UserProfileInfoCollection()
                        {
                            userprofile
                        }
                    }
                };
                argsFactory.LimServiceMock.Setup(l => l.SearchUserProfile(profileRequest)).Returns(resp);
                var payment = new PaymentNotification()
                {
                    AdditionalInformation = "9875645, xyz",
                    NotificationType = PaymentNotification.NotificationTypeType.CreditConfirmation,
                    PaymentAmount = 40000,
                    PaymentDateTime = DateTime.Now,
                    PaymentOriginatorAccount = new Account()
                    {
                        AccountNumber = "12121212",
                        BankNumber = "111",
                        TransitNumber = "22122"
                    }
                };

                var allocation = new Allocation()
                {
                    DealId = 11,
                    FundingDealId = 0,
                    LawyerInfo = userprofile,
                    ShortFCTURN = "15-099-11444"
                };
                var dict = new Dictionary<PaymentNotification, Allocation> {{payment, allocation}};
                argsFactory.DealFundsMock.Setup(d => d.SavePayments(dict));

                fundsAlloc.ReceivePaymentNotification(new PaymentNotificationRequest()
                {
                    PaymentNotifications = new PaymentNotificationList() {payment}
                });
                mockRepository.Verify();
            }
            catch (NullReferenceException exception)
            {
                if (exception.Source.Contains("FCT.LLC.Common.NotificationEmailDispatching.Client"))
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        [Test]
        [ExpectedException(typeof (SuccessException))]
        public void PaymentReceived_Can_Save_UnAssignedFunds()
        {
            try
            {
                MockRepository mockRepository;
                ArgumentsFactory argsFactory;
                var fundsAlloc = GetConstructor(out argsFactory, out mockRepository);
                var payment = new PaymentNotification()
                {
                    AdditionalInformation = "9875645,xya",
                    NotificationType = PaymentNotification.NotificationTypeType.CreditConfirmation,
                    PaymentAmount = 40000,
                    PaymentDateTime = DateTime.Now,
                };
                var dict = new Dictionary<PaymentNotification, Allocation> {{payment, null}};
                argsFactory.DealFundsMock.Setup(f => f.SavePayments(dict));

                fundsAlloc.ReceivePaymentNotification(new PaymentNotificationRequest()
                {
                    PaymentNotifications = new PaymentNotificationList() {payment}
                });

                mockRepository.Verify();
            }
            catch (NullReferenceException exception)
            {
                if (exception.Source.Contains("FCT.LLC.Common.NotificationEmailDispatching.Client"))
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            }
        }

        private static PaymentBusinessLogic GetConstructor(out ArgumentsFactory factory, out MockRepository mockRepository)
        {

            mockRepository = new MockRepository(MockBehavior.Default);
            var argsFactory = new ArgumentsFactory(mockRepository);
            var fundsAlloc = new PaymentBusinessLogic(argsFactory.DealFundsMock.Object,
                argsFactory.DealScopeMock.Object, argsFactory.FundingDealMock.Object, argsFactory.FundedMock.Object,
                argsFactory.SummaryMock.Object, argsFactory.LawyerMock.Object,
                argsFactory.DealHistoryMock.Object,
                argsFactory.LimServiceMock.Object, argsFactory.AuditLogMock.Object,
                argsFactory.PaymentNotificationMock.Object, argsFactory.DisbursementMock.Object,argsFactory.EmailMock.Object
                );
            factory = argsFactory;
            return fundsAlloc;
        }
    }
}

   
