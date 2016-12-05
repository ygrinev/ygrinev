using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Contracts.FaultContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.Services.LIM.DataContracts;
using FCT.Services.LIM.MessageContracts;
using FCT.Services.LIM.ServiceContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class ValidationHelper:IValidationHelper
    {
        private readonly ILIMServiceContract _limService;
        private readonly IFundingDealRepository _fundingDealRepository;
        private readonly IDisbursementRepository _disbursementRepository;
        private readonly ReadOnlyDataHelper _readOnlyHelper;
        private readonly IVendorLawyerHelper _vendorLawyerHelper;

        const string EasternTime = "Eastern Standard Time";
        private const string OpsTester = "OpsTester";

        public ValidationHelper(ILIMServiceContract limServiceContract, IFundingDealRepository fundingDealRepository,
            IDisbursementRepository disbursementRepository, ReadOnlyDataHelper readOnlyHelper, IVendorLawyerHelper vendorLawyerHelper)
        {
            _limService = limServiceContract;
            _fundingDealRepository = fundingDealRepository;
            _disbursementRepository = disbursementRepository;
            _readOnlyHelper = readOnlyHelper;
            _vendorLawyerHelper = vendorLawyerHelper;
        }

        public UserProfile GetActiveLawyer(int trustAccountId, List<ErrorCode> errorCodes = null)
        {
            var searchRequest = new SearchUserProfileRequest()
            {
                PageIndex = 1,
                RecordsPerPage = 500,
                SortBy = "UserID",
                SortDirection = "ASC",
                SearchCriteria = new UserProfileSearchCriteria()
                {
                    SolutionID = (int)SolutionType.EASYFUND,
                    TrustAccountID = trustAccountId,
                    TrustAccountStatus = (int)AccountStatus.Active
                }
            };
            var searchResponse = _limService.SearchUserProfile(searchRequest);
            if (searchResponse.SearchResults.TotalRecordCount <= 0)
            {
                if (errorCodes != null)
                {
                    errorCodes.Add(ErrorCode.TrustAccountNotActive);
                }
            }
            if (searchResponse.SearchResults.TotalRecordCount == 1)
            {
                var userProfileInfo = searchResponse.SearchResults.UserProfiles.SingleOrDefault();
                if (userProfileInfo != null)
                {
                    var userRegistration = userProfileInfo.Registrations.FirstOrDefault();
                    if (userRegistration != null && userRegistration.UserStatusID != (int)UserStatus.Active)
                    {
                        if (errorCodes != null)
                        {
                            errorCodes.Add(ErrorCode.InactiveUser);
                        }
                        
                    }
                    else
                    {
                        var userId = userProfileInfo.UserID;
                        var userProfileByIdRequest = new GetUserProfileByUserIDRequest { UserID = userId };
                        var userProfileResponse = _limService.GetUserProfileByUserID(userProfileByIdRequest);

                        return userProfileResponse.UserProfile;
                    }
                   
                }
            }
           
            return null;
        }

        public async Task<Payment> ValidateVendorLawyer(DisbursementCollection disbursements, List<ErrorCode> errorCodes, FundingDeal deal)
        {
            Disbursement vendorDisbursement = null;
            Disbursement feeDisbursement = null;
            Payment payment = null;

            foreach (var disbursement in disbursements)
            {
                if (disbursement.PayeeType == FeeDistribution.VendorLawyer)
                {
                    vendorDisbursement = disbursement;
                }
                if (disbursement.PayeeType == EasyFundFee.FeeName)
                {
                    feeDisbursement = disbursement;
                }
            }
            if (feeDisbursement != null && (feeDisbursement.FCTFeeSplit == FeeDistribution.SplitEqually ||
                                            feeDisbursement.FCTFeeSplit == FeeDistribution.VendorLawyer))
            {
                if (vendorDisbursement == null)
                {
                    errorCodes.Add(ErrorCode.VendorLawyerDisbursementNotFound);
                }
                else
                {
                    if (feeDisbursement.VendorFee!=null)
                    {
                        var vendorFee = feeDisbursement.VendorFee.Amount + feeDisbursement.VendorFee.GST +
                                        feeDisbursement.VendorFee.HST + feeDisbursement.VendorFee.QST;
                        if (vendorDisbursement.Amount < vendorFee)
                        {
                            errorCodes.Add(ErrorCode.VendorLawyerAmountInadequate); 
                        }
                        
                    }
                }
            }
            if (vendorDisbursement != null)
            {
                if (vendorDisbursement.TrustAccountID.HasValue)
                {
                    var vendorLawyerprofile =
                        GetActiveLawyer(vendorDisbursement.TrustAccountID.GetValueOrDefault(), errorCodes);

                    if (vendorLawyerprofile!=null)
                    {
                        vendorDisbursement.PayeeName = vendorLawyerprofile.FullName;
                        var lawyerAddress = vendorLawyerprofile.Addresses.FirstOrDefault();
                        vendorDisbursement.UnitNumber = lawyerAddress.UnitNumber;
                        vendorDisbursement.StreetAddress1 = lawyerAddress.AddressLine1;
                        vendorDisbursement.StreetAddress2 = lawyerAddress.AddressLine2;
                        vendorDisbursement.StreetNumber = lawyerAddress.StreetNumber;
                        vendorDisbursement.City = lawyerAddress.City;
                        vendorDisbursement.PostalCode = lawyerAddress.PostalCode;
                        vendorDisbursement.Country = lawyerAddress.Country;
                        vendorDisbursement.Province = lawyerAddress.Province;

                        disbursements.Remove(disbursements.Single(d => d.PayeeType == FeeDistribution.VendorLawyer));
                        disbursements.Add(vendorDisbursement);

                        payment = new Payment()
                        {
                            LawyerProfile = vendorLawyerprofile,
                            LawyerTrustAccountId = vendorDisbursement.TrustAccountID.GetValueOrDefault()
                        };
                    }
                }
                else
                {
                    bool activeTrustAccountAssigned=false;
                    switch (deal.ActingFor)
                    {
                        case LawyerActingFor.Vendor: 
                        case LawyerActingFor.Both:
                            activeTrustAccountAssigned=_vendorLawyerHelper.AssignActiveExistingTrustAccount(deal.Lawyer, vendorDisbursement);
                            break;
                        case LawyerActingFor.Purchaser:
                            activeTrustAccountAssigned=_vendorLawyerHelper.AssignActiveExistingTrustAccount(deal.OtherLawyer, vendorDisbursement);
                            break;
                    }
                    if (activeTrustAccountAssigned)
                    {
                        _disbursementRepository.UpdateDisbursement(vendorDisbursement);
                    }
                    else
                    {
                        errorCodes.Add(ErrorCode.NoTrustAccountID);
                    }
                    
                }
            }
            return payment;
        }

        public FundingDeal ValidateDealInDB(int dealId, List<ErrorCode> errorcodes,
            out DisbursementCollection disbursements)
        {
            var fundingDeal = _fundingDealRepository.GetFundingDeal(dealId);
            if (fundingDeal == null)
            {
                errorcodes.Add(ErrorCode.DealDoesNotExist);
            }
            else
            {
                if (!FundingBusinessLogicHelper.IsValidDealForUpdate(fundingDeal.DealStatus))
                {
                    errorcodes.Add(ErrorCode.DealCancelledOrDeclined);
                }
                if (fundingDeal.ClosingDate == null || fundingDeal.ClosingDate <= DateTime.MinValue)
                {
                    errorcodes.Add(ErrorCode.ClosingDateMissing);
                }
            }           
            disbursements = _disbursementRepository.GetDisbursements(dealId);
            if (disbursements == null || disbursements.Count <= 0)
            {
                errorcodes.Add(ErrorCode.DisbursementsNotFound);
            }
            if (errorcodes.Count > 0)
            {
                throw new ValidationException(errorcodes);
            }
            return fundingDeal;
        }

        public bool IsOnWeekend(DateTime easternTime)
        {
            if (easternTime.DayOfWeek == DayOfWeek.Saturday || easternTime.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            return false;
        }

        public bool IsWithinWorkingHours(DateTime easternTime)
        {
            var configs = _readOnlyHelper.GetConfigurationValues();
            int hourEnd = Convert.ToInt32(configs[FCTHoursConfiguration.FCTHourEnd]);
            int minuteEnd = Convert.ToInt32(configs[FCTHoursConfiguration.FCTMinuteEnd]);
            int hourStart = Convert.ToInt32(configs[FCTHoursConfiguration.FCTHourStart]);
            int minuteStart = Convert.ToInt32(configs[FCTHoursConfiguration.FCTMinuteStart]);

            var start = new TimeSpan(hourStart, minuteStart, 0);
            var end = new TimeSpan(hourEnd, minuteEnd, 0);
            var now = easternTime.TimeOfDay;
            if (now > start && now < end)
            {
                return true;
            }
            return false;
        }

        public bool IsSystemClosed(DateTime easternTime)
        {
            bool closed = _readOnlyHelper.IsSystemClosed(easternTime);
            return closed;
        }

        public bool IsTestUser(UserContext userContext)
        {
            bool isOpsTester;
            if (bool.TryParse(ConfigurationManager.AppSettings[OpsTester].ToLower(), out isOpsTester))
            {
                if (userContext.UserType == UserType.FCTAdmin)
                {
                    if (isOpsTester)
                    {
                        return true;
                    }
                }
                else
                {
                    var userProfileRequest = new GetUserProfileByUserNameRequest()
                    {
                        UserName = userContext.UserID
                    };
                    var userProfileResponse = _limService.GetUserProfileByUserName(userProfileRequest);
                    return userProfileResponse.UserProfile.TestAccount;
                }
            }
            return false;
        }

        public DateTime ValidatePaymentDate(UserContext userContext, List<ErrorCode> errorcodes)
        {            
            var disbursementDate = DateTime.UtcNow;
            TimeZoneInfo etZone = TimeZoneInfo.FindSystemTimeZoneById(EasternTime);
            var easternTime = TimeZoneInfo.ConvertTimeFromUtc(disbursementDate, etZone);

            if (!IsTestUser(userContext))
            {
                if (IsOnWeekend(easternTime))
                {
                    errorcodes.Add(ErrorCode.Weekend);
                }
                if (IsSystemClosed(easternTime))
                {
                    errorcodes.Add(ErrorCode.CPASystemClosure);
                }
                if (!IsWithinWorkingHours(easternTime))
                {
                    errorcodes.Add(ErrorCode.NotWithinWorkingHours);
                }
            }
            return easternTime;
        }
    }
}
