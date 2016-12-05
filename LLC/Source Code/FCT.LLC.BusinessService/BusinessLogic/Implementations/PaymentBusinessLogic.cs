using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.BusinessLogic.Interfaces;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;
using FCT.Services.AuditLog.DataContracts;
using FCT.Services.AuditLog.MessageContracts;
using FCT.Services.AuditLog.ServiceContracts;
using FCT.Services.LIM.DataContracts;
using FCT.Services.LIM.MessageContracts;
using FCT.Services.LIM.ServiceContracts;

namespace FCT.LLC.BusinessService.BusinessLogic.Implementations
{
    public class PaymentBusinessLogic : IPaymentBusinessLogic
    {
        private readonly IDealFundsAllocRepository _dealFundsAllocRepository;
        private readonly IDealScopeRepository _dealScopeRepository;
        private readonly IFundingDealRepository _fundingDealRepository;
        private readonly IFundedRepository _fundedRepository;
        private readonly IDisbursementSummaryRepository _disbursementSummaryRepository;
        private readonly ILawyerRepository _lawyerRepository;
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly ILIMServiceContract _limService;
        private readonly IAuditLogService _auditLogService;
        private readonly IPaymentNotificationRepository _notificationRepository;
        private readonly IDisbursementRepository _disbursementRepository;
        private readonly IEmailHelper _emailHelper;


        private IDictionary<PaymentNotification, Allocation> _allocations;
        private IList<PaymentNotification> _paymentNotifications;
        private bool _moveToUnAllocated;
        private decimal _totalDeposit;

        private const string EFSupportEmail = "EasyFundSupportEmail";
        private const string WireDepositSeparator = "WireDepositSeparator";

        public PaymentBusinessLogic(IDealFundsAllocRepository dealFundsAllocRepository,
            IDealScopeRepository dealScopeRepository, IFundingDealRepository fundingDealRepository,
            IFundedRepository fundedRepository, IDisbursementSummaryRepository disbursementSummaryRepository,
            ILawyerRepository lawyerRepository, IDealHistoryRepository dealHistoryRepository,
            ILIMServiceContract limService, IAuditLogService auditLogService,
            IPaymentNotificationRepository paymentNotificationRepository, IDisbursementRepository disbursementRepository,
            IEmailHelper emailHelper)
        {
            _dealFundsAllocRepository = dealFundsAllocRepository;
            _dealScopeRepository = dealScopeRepository;
            _fundingDealRepository = fundingDealRepository;
            _fundedRepository = fundedRepository;
            _disbursementSummaryRepository = disbursementSummaryRepository;
            _lawyerRepository = lawyerRepository;
            _dealHistoryRepository = dealHistoryRepository;
            _limService = limService;
            _auditLogService = auditLogService;
            _notificationRepository = paymentNotificationRepository;
            _disbursementRepository = disbursementRepository;
            _emailHelper = emailHelper;
        }


        public void ReceivePaymentNotification(PaymentNotificationRequest request)
        {
            _allocations = new Dictionary<PaymentNotification, Allocation>();
            _paymentNotifications = new List<PaymentNotification>();

            foreach (var paymentNotification in request.PaymentNotifications)
            {
                if (paymentNotification.NotificationType == PaymentNotification.NotificationTypeType.CreditConfirmation)
                {
                    if (_dealFundsAllocRepository.IsDuplicateDeposit(paymentNotification.PaymentReferenceNumber))
                    {
                        throw new DuplicateNameException(
                            FundsAllocationHelper.FormatDuplicatesErrorMessage(paymentNotification));
                    }
                    bool addedtoalloactions = false;
                    if (!string.IsNullOrWhiteSpace(paymentNotification.AdditionalInformation))
                    {
                        addedtoalloactions = AllocateFundsToDeal(paymentNotification);
                    }
                    if (!addedtoalloactions && paymentNotification.PaymentOriginatorAccount != null && !_moveToUnAllocated)
                    {
                        addedtoalloactions = AssignFundsToLawyer(paymentNotification);
                    }
                    if (!addedtoalloactions)
                    {
                        _allocations.Add(paymentNotification, null);
                    }
                }
                else
                {
                    _paymentNotifications.Add(paymentNotification);
                }
            }
            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                _dealFundsAllocRepository.SavePayments(_allocations);

                foreach (var allocation in _allocations)
                {
                    if (allocation.Value != null && allocation.Value.LawyerInfo == null)
                    {
                        var depositRequired =
                            _disbursementSummaryRepository.GetDepositRequired(allocation.Value.FundingDealId);

                        _dealHistoryRepository.CreateDealHistory(allocation.Value.DealId, DealActivity.DepositReceived);

                        UpdateDealMilestone(allocation, depositRequired, _totalDeposit);
                    }
                }

                if (_paymentNotifications != null && _paymentNotifications.Any())
                {
                    _notificationRepository.InsertNotificationRange(_paymentNotifications);

                    var notificationDict = ConvertToPaymentDictionary(_paymentNotifications);
                    _disbursementRepository.UpdateDisbursementsFromNotifications(notificationDict);
                    _dealFundsAllocRepository.UpdateDealAllocationsFromNotifications(notificationDict);
                }
                scope.Complete();
            }
            //no need of transaction for sending emails or updating auditlogs
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                NotifyAndUpdateLogs();
            }
            
        }

        private bool AssignFundsToLawyer(PaymentNotification paymentNotification)
        {
            var account = paymentNotification.PaymentOriginatorAccount;
            if (!string.IsNullOrWhiteSpace(account.BankNumber) &&
                !string.IsNullOrWhiteSpace(account.TransitNumber) &&
                !string.IsNullOrWhiteSpace(account.AccountNumber))
            {
                var userProfileResponse = SearchUserProfileByTrustAccount(account);
                if (userProfileResponse.SearchResults != null && userProfileResponse.SearchResults.TotalRecordCount > 0)
                {
                    if (userProfileResponse.SearchResults.TotalRecordCount == 1)
                    {
                        var user = userProfileResponse.SearchResults.UserProfiles.SingleOrDefault();
                        var allocation = new Allocation()
                        {
                            LawyerInfo = user
                        };
                        _allocations.Add(paymentNotification, allocation);
                        return true;
                    }
                }
            }
            return false;
        }

        private void NotifyLawyerAndDelegates(PaymentNotification paymentNotification, UserProfileInfo user)
        {
            var lawyerProfile = new LawyerProfile()
            {
                UserLanguage = user.Language,
                Email = user.Email,
                UserName = user.UserName,
            };
            // string businessModel = _fundsAllocationHelper.GetLawyerRegistration(user.UserName);
            string mailingList = _emailHelper.GetEmailRecipientList(lawyerProfile,"EASYFUND"); //, businessModel);
            var funds = new DealFundsAllocation()
            {
                Amount = paymentNotification.PaymentAmount,
                DepositDate = Convert.ToDateTime(paymentNotification.PaymentDateTime),
                WireDepositDetails = paymentNotification.AdditionalInformation
            };

            FundsAllocationHelper.SendNotificationEmail(funds, user.Language, mailingList);
        }

        private SearchUserProfileResponse SearchUserProfileByTrustAccount(Account account)
        {
            var profileRequest = new SearchUserProfileRequest()
            {
                PageIndex = 1,
                RecordsPerPage = 100,
                SortBy = "UserID",
                SortDirection = "ASC",
                SearchCriteria = new UserProfileSearchCriteria()
                {
                    AccountNumber = account.AccountNumber,
                    BankNumber = account.BankNumber,
                    BranchNumber = account.TransitNumber,
                    SolutionID = (int) SolutionType.EASYFUND,
                    UserStatusID = (int) UserStatus.Active,
                    TrustAccountStatus = (int) AccountStatus.Active
                }
            };
            var userProfileResponse = _limService.SearchUserProfile(profileRequest);
            return userProfileResponse;
        }

        private bool AllocateFundsToDeal(PaymentNotification paymentNotification)
        {
            string fctURN = string.Empty;
            string wireDepositCode = string.Empty;

            string identifier = paymentNotification.AdditionalInformation.Trim(); 

            int index = identifier.Length - FCTURNHelper.WireDepositCodeLength;

            string possibleDepositCode = identifier.Substring(index);
            string possibleFCTRefIdentifier = identifier.Remove(index - 1);

            string probableFCTRef = FundsAllocationHelper.IsProbableFCTURN(possibleFCTRefIdentifier);

            if (FundsAllocationHelper.IsProbableWireCode(possibleDepositCode))
            {
                wireDepositCode = possibleDepositCode; 
            }
            if (!string.IsNullOrEmpty(probableFCTRef))
            {
                fctURN = probableFCTRef;
            }
                      
            if (!string.IsNullOrEmpty(fctURN) && !string.IsNullOrEmpty(wireDepositCode))
            {
                int dealScopeId = _dealScopeRepository.GetDealScope(fctURN, wireDepositCode);
                var dealInfo = _fundingDealRepository.GetDeal(dealScopeId, false);

                if (dealScopeId > 0 && dealInfo != null)
                {
                    var fundedDeal = dealInfo.DealState;
                    if (fundedDeal.Milestone.Disbursed > DateTime.MinValue)
                    {
                        _moveToUnAllocated = true;
                    }
                    else if (dealInfo.DealStatus == DealStatus.Cancelled ||
                             dealInfo.DealStatus == DealStatus.CancelRequest ||
                             dealInfo.DealStatus == DealStatus.Complete)
                    {
                        _moveToUnAllocated = true;
                    }
                    else
                    {
                        var allocation = new Allocation()
                        {
                            FundingDealId = fundedDeal.FundingDealId,
                            DealId = dealInfo.DealID,
                            ShortFCTURN = fctURN
                        };
                        _allocations.Add(paymentNotification, allocation);
                        _totalDeposit = _dealFundsAllocRepository.GetTotalAllocatedFunds(fundedDeal.FundingDealId);
                        return true;
                    }
                }
            }
            return false;
        }

        private void NotifyAndUpdateLogs()
        {
            foreach (var allocation in _allocations)
            {
                if (allocation.Value == null)
                {
                    FundsAllocationHelper.SendNotificationEmail(ConfigurationManager.AppSettings[EFSupportEmail]);
                }
                else if (allocation.Value.LawyerInfo == null)
                {
                    var depositRequired =
                        _disbursementSummaryRepository.GetDepositRequired(allocation.Value.FundingDealId);

                    var fundingDeal = _fundingDealRepository.GetFundingDeal(allocation.Value.DealId);

                    NotifyLawyers(depositRequired, _totalDeposit, allocation.Key, fundingDeal);
                }
                else if (allocation.Value.LawyerInfo != null)
                {
                    NotifyLawyerAndDelegates(allocation.Key, allocation.Value.LawyerInfo);

                    UpdateAuditLogs(allocation.Value.LawyerInfo, allocation.Key.PaymentAmount);
                }
            }
        }

        private void UpdateDealMilestone(KeyValuePair<PaymentNotification, Allocation> allocation,
            decimal depositRequired, decimal lastTotalDeposit)
        {
            if (lastTotalDeposit + allocation.Key.PaymentAmount == depositRequired)
            {
                var milestone = new FundingMilestone()
                {
                    Funded = DateTime.Now
                };
                _fundedRepository.UpdateMilestones(allocation.Value.FundingDealId, milestone);
            }
            else
            {
                var fundedDeal = _fundedRepository.GetMilestonesByDeal(allocation.Value.DealId);
                if (fundedDeal != null)
                {
                    fundedDeal.Milestone.Funded = null;
                    _fundedRepository.UpdateFundedDeal(fundedDeal);
                }
            }
        }

        private void NotifyLawyers(decimal depositRequired, decimal lastTotalDeposit, PaymentNotification payment,
            FundingDeal fundingDeal)
        {
            var notificationDetails = _lawyerRepository.GetNotificationDetails(fundingDeal.Lawyer.LawyerID);
            
            var mailingList = _emailHelper.GetEmailRecipientList(fundingDeal);

            if (payment.PaymentAmount + lastTotalDeposit == depositRequired)
            {                
                FundsAllocationHelper.SendNotificationEmail(fundingDeal, notificationDetails,
                    StandardNotificationKey.EFDepositFundsMatch, mailingList.GetValueOrDefault().Value);

                if (fundingDeal.ActingFor != LawyerActingFor.Both && fundingDeal.ActingFor!=LawyerActingFor.Mortgagor)
                {
                    var fundedDeal = _fundedRepository.GetMilestonesByDeal(fundingDeal.DealID.GetValueOrDefault());
                    if (fundedDeal != null &&
                        fundedDeal.Milestone.SignedByVendor > DateTime.MinValue &&
                        fundedDeal.Milestone.SignedByPurchaser > DateTime.MinValue)
                    {
                        var otherLawyerNotificationDetails =
                            _lawyerRepository.GetNotificationDetails(fundingDeal.OtherLawyer.LawyerID);

                        int otherDealId = _fundingDealRepository.GetOtherDealInScope(fundingDeal.DealID.GetValueOrDefault());

                        var emailList = _emailHelper.GetEmailRecipientList(fundingDeal, otherDealId);

                        FundsAllocationHelper.SendNotificationEmail(fundingDeal, otherLawyerNotificationDetails,
                            StandardNotificationKey.EFDepositVendor, emailList.GetValueOrDefault().Value);
                    }
                }
            }
            if (payment.PaymentAmount + lastTotalDeposit != depositRequired)
            {
                FundsAllocationHelper.SendNotificationEmail(fundingDeal, notificationDetails,
                    StandardNotificationKey.EFDepositFundsMismatch, mailingList.GetValueOrDefault().Value);
            }
        }

        private void UpdateAuditLogs(UserProfileInfo userDetails, decimal amount)
        {
            string lawyername = string.Format("{0} {1}", userDetails.FirstName, userDetails.LastName);
            string message = string.Format("Unallocated Funds ({0}) assigned to {1}",
                amount.ToString("C", CultureInfo.CurrentCulture), lawyername);
            string activity = "Unallocated Funds Assignment";

            var auditLogRequest = new WriteLogRequest
            {
                LogEntry = new LogEntry()
                {
                    Activity = activity,
                    ActivityDate = DateTime.Now,
                    Message = message,
                    UserName = userDetails.UserName,
                    IPAddress = FundsAllocationHelper.GetIPAddress()
                }
            };
            _auditLogService.WriteLog(auditLogRequest);
        }

        private static Dictionary<int, PaymentNotification> ConvertToPaymentDictionary(
            IEnumerable<PaymentNotification> paymentNotifications)
        {
            var notificationDict = new Dictionary<int, PaymentNotification>();
            foreach (var paymentNotification in paymentNotifications)
            {
                int key = Convert.ToInt32(paymentNotification.DisbursementRequestID);
                if (notificationDict.ContainsKey(key))
                {
                    notificationDict[key] = paymentNotification;
                }
                else
                {
                    notificationDict.Add(key, paymentNotification);
                }
            }
            return notificationDict;
        }
    }
}
