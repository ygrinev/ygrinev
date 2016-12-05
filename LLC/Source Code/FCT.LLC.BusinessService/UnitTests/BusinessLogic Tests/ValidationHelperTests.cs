using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.Services.LIM.DataContracts;
using FCT.Services.LIM.MessageContracts;
using Moq;
using NUnit.Framework;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestFixture]
    public class ValidationHelperTests
    {
        private UserProfile _userProfile;
        private FundingDeal _fundingDeal;

        [TestFixtureSetUp]
        public void Init()
        {
            _userProfile = new UserProfile()
            {
                Email = "test@email.com",
                FirmName = "Test LLC",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                UserID = 11,
                UserName = "TTest",
                Addresses = new AddressCollection()
                {
                    new Address()
                    {
                        AddressID = 1,
                        AddressLine1 = "121 Test Ave.",
                        City = "Test City",
                        Country = "TestLand",
                        PostalCode = "121212",
                        Province = "Test Province"
                    }
                }
            };
            _fundingDeal = new FundingDeal()
            {
                DealID = 1,
                ActingFor = LawyerActingFor.Vendor,
                Lawyer =
                    new Lawyer()
                    {
                        LawyerID = 11,
                        FirstName = "TestFirstName",
                        LastName = "TestLastName",
                        LawFirm = "Test LLC"
                    },
            };
        }

        [Test]
        public void ValidateVendorLawyer_DisbursementHasTrustAccount()
        {
            //ARRANGE
            var mockRepo = new MockRepository(MockBehavior.Default);
            var argsFactory = new ArgumentsFactory(mockRepo);

            SetupLIMMock(argsFactory, _userProfile);

            var disbursements = new DisbursementCollection()
            {
                new Disbursement()
                {
                    PayeeType = FeeDistribution.VendorLawyer,
                    TrustAccountID = 1001,
                    Amount = 1000,
                },
                new Disbursement()
                {
                    PayeeType = EasyFundFee.FeeName,
                    Amount = 113
                }
            };
            var expectedPaymentResult = new Payment()
            {
                LawyerProfile = _userProfile,
                LawyerTrustAccountId = 1001
            };
            var mockContext = new Mock<EFBusinessContext>();
            var readOnlyHelper = new ReadOnlyDataHelper(mockContext.Object);
            var errorcodes = new List<ErrorCode>();

            //ACT
            var validationHelper = new ValidationHelper(argsFactory.LimServiceMock.Object,
                argsFactory.FundingDealMock.Object, argsFactory.DisbursementMock.Object, readOnlyHelper,
                argsFactory.VendorLawyerMock.Object);

            var result = validationHelper.ValidateVendorLawyer(disbursements, errorcodes, new FundingDeal());

            //ASSERT
            Assert.IsNotNull(result.Result);
            CollectionAssert.AreEqual(expectedPaymentResult.LawyerProfile.Addresses, result.Result.LawyerProfile.Addresses);
            Assert.AreEqual(expectedPaymentResult.LawyerTrustAccountId, result.Result.LawyerTrustAccountId);
        }

        [Test]
        public void ValidateVendorLawyer_DisbursementDoesNotHaveTrustAccount()
        {
            //ARRANGE
            var mockRepo = new MockRepository(MockBehavior.Default);
            var argsFactory = new ArgumentsFactory(mockRepo);

            SetupLIMMock(argsFactory, _userProfile);
            var vendorDisbursement = new Disbursement()
            {
                PayeeType = FeeDistribution.VendorLawyer,
                TrustAccountID = null,
                Amount = 1000,
            };
            argsFactory.VendorLawyerMock.Setup(
                v => v.AssignActiveExistingTrustAccount(_fundingDeal.Lawyer, vendorDisbursement)).Returns(true);
            argsFactory.DisbursementMock.Setup(d => d.UpdateDisbursement(vendorDisbursement));

            var disbursements = new DisbursementCollection()
            {
                vendorDisbursement,
                new Disbursement()
                {
                    PayeeType = EasyFundFee.FeeName,
                    Amount = 113
                }
            };

            var mockContext = new Mock<EFBusinessContext>();
            var readOnlyHelper = new ReadOnlyDataHelper(mockContext.Object);
            var errorcodes = new List<ErrorCode>();

            //ACT
            var validationHelper = new ValidationHelper(argsFactory.LimServiceMock.Object,
                argsFactory.FundingDealMock.Object, argsFactory.DisbursementMock.Object, readOnlyHelper,
                argsFactory.VendorLawyerMock.Object);
            var result = validationHelper.ValidateVendorLawyer(disbursements, errorcodes, _fundingDeal);

            //ASSERT
            Assert.IsNull(result);
            argsFactory.VendorLawyerMock.Verify(
                v => v.AssignActiveExistingTrustAccount(_fundingDeal.Lawyer, vendorDisbursement), Times.Once);
            argsFactory.DisbursementMock.Verify(d => d.UpdateDisbursement(vendorDisbursement), Times.Once);
        }

        [Test]
        public void ValidateDealInDB_AllValidationPasses()
        {
            //ARRANGE
            var mockRepo = new MockRepository(MockBehavior.Default);
            var argsFactory = new ArgumentsFactory(mockRepo);
           var disbursements = new DisbursementCollection()
            {
                new Disbursement()
                {
                    PayeeType = FeeDistribution.VendorLawyer,
                    TrustAccountID = 1001,
                    Amount = 1000,
                },
                new Disbursement()
                {
                    PayeeType = EasyFundFee.FeeName,
                    Amount = 113
                }
            };
            argsFactory.FundingDealMock.Setup(f => f.GetFundingDeal(1)).Returns(_fundingDeal);
            argsFactory.DisbursementMock.Setup(d => d.GetDisbursements(1)).Returns(disbursements);
            var mockContext = new Mock<EFBusinessContext>();
            var readOnlyHelper = new ReadOnlyDataHelper(mockContext.Object);
            var errorcodes = new List<ErrorCode>();

            //ACT
            var validationHelper = new ValidationHelper(argsFactory.LimServiceMock.Object,
             argsFactory.FundingDealMock.Object, argsFactory.DisbursementMock.Object, readOnlyHelper,
             argsFactory.VendorLawyerMock.Object);
            DisbursementCollection expectedDisbursements;
            var result = validationHelper.ValidateDealInDB(1, errorcodes, out expectedDisbursements);

            //ASSERT
            Assert.AreEqual(_fundingDeal, result);
            CollectionAssert.AreEqual(expectedDisbursements, disbursements);
            Assert.That(errorcodes.Count==0);
        }

        [Test]
        [ExpectedException (typeof(ValidationException))]
        public void ValidateDealInDB_ValidationFails()
        {
            //ARRANGE
            var mockRepo = new MockRepository(MockBehavior.Default);
            var argsFactory = new ArgumentsFactory(mockRepo);
            argsFactory.FundingDealMock.Setup(f => f.GetFundingDeal(1)).Returns(_fundingDeal);
            var mockContext = new Mock<EFBusinessContext>();
            var readOnlyHelper = new ReadOnlyDataHelper(mockContext.Object);
            var errorcodes = new List<ErrorCode>();

            //ACT
            var validationHelper = new ValidationHelper(argsFactory.LimServiceMock.Object,
             argsFactory.FundingDealMock.Object, argsFactory.DisbursementMock.Object, readOnlyHelper,
             argsFactory.VendorLawyerMock.Object);
            DisbursementCollection expectedDisbursements;
            var result = validationHelper.ValidateDealInDB(1, errorcodes, out expectedDisbursements);

            //ASSERT
            Assert.AreEqual(_fundingDeal, result);
            Assert.That(errorcodes.Count == 1);
        }

        private void SetupLIMMock(ArgumentsFactory argsFactory, UserProfile userProfile)
        {
            var searchresponse = new SearchUserProfileResponse()
            {
                SearchResults = new UserProfileSearchResult()
                {
                    TotalRecordCount = 1,
                    UserProfiles = new UserProfileInfoCollection()
                    {
                        new UserProfileInfo()
                        {
                            AccountNumber = "111111",
                            BankNumber = "111",
                            BranchNumber = "00001",
                            City = "Test City",
                            Email = "test@email.com",
                            FirmName = "Test LLC",
                            FirstName = "TestFirstName",
                            LastName = "TestLastName",
                            UserID = 11,
                            UserName = "TTest",
                            Registrations = new UserRegistrationCollection()
                            {
                                new UserRegistration()
                                {
                                    UserStatusID = (int) UserStatus.Active
                                }
                            }
                        }
                    }
                }
            };
            argsFactory.LimServiceMock.Setup(l => l.SearchUserProfile(It.IsAny<SearchUserProfileRequest>()))
                .Returns(searchresponse);
            var getuserProfileResponse = new GetUserProfileByUserIDResponse()
            {
                UserProfile = userProfile
            };
            argsFactory.LimServiceMock.Setup(l => l.GetUserProfileByUserID(It.IsAny<GetUserProfileByUserIDRequest>()))
                .Returns(getuserProfileResponse);
        }
    }
}
