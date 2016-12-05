using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.BusinessLogic;
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
    public class FundsAllocBusinessLogic:IFundsAllocBusinessLogic
    {
        private readonly IDealFundsAllocRepository _dealFundsAllocRepository;
        private readonly IFundingDealRepository _fundingDealRepository;
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly ILawyerRepository _lawyerRepository;
        private readonly IDisbursementSummaryRepository _disbursementSummaryRepository;
        private readonly IFundedRepository _fundedRepository;
        private readonly IAuditLogService _auditLogService;
        private readonly IEmailHelper _emailHelper;


        private const string EFSupportEmail = "EasyFundSupportEmail";

        public FundsAllocBusinessLogic(IDealFundsAllocRepository dealFundsAllocRepository,
            IFundingDealRepository fundingDealRepository, IDealHistoryRepository dealHistoryRepository,ILawyerRepository lawyerRepository,
            IDisbursementSummaryRepository disbursementSummaryRepository, IFundedRepository fundedRepository,
            IAuditLogService auditLogService, IEmailHelper emailHelper)
        {
            _dealFundsAllocRepository = dealFundsAllocRepository;
            _fundingDealRepository = fundingDealRepository;
            _dealHistoryRepository = dealHistoryRepository;
            _lawyerRepository = lawyerRepository;
            _disbursementSummaryRepository = disbursementSummaryRepository;
            _fundedRepository = fundedRepository;
            _auditLogService = auditLogService;
            _emailHelper = emailHelper;

        }
        public SearchFundsAllocationResponse SearchFundsAllocation(SearchFundsAllocationRequest request)
        {
            

            int totalRecords;
            var dealFundsAllocations = _dealFundsAllocRepository.SearchFunds(request, out totalRecords);
            var response = new SearchFundsAllocationResponse()
            {
                PageIndex = request.PageIndex,
                TotalRowsCount = totalRecords,
                SearchResults = new DealFundsAllocationCollection()
            };
            response.SearchResults.AddRange(dealFundsAllocations);
            return response;
        }


        public void UpdateFundsAllocation(UpdateFundsAllocationRequest request)
        {
            DealFundsAllocation fundsAllocation;

                if (request.FundingDealID.HasValue)
                {
                    var depositRequired =
                        _disbursementSummaryRepository.GetDepositRequired(request.FundingDealID.GetValueOrDefault());

                    var fundingDeal = _fundingDealRepository.GetFundingDeal(request.FundingDealID.GetValueOrDefault(), false);

                    if (fundingDeal != null)
                    {
                        decimal depositReceived;
                        using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                        {
                            fundsAllocation = _dealFundsAllocRepository.UpdateFundsAllocation(request);

                            _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(),
                                DealActivity.FundsAllocated, request.UserContext, fundsAllocation.Amount);

                            depositReceived =
                                _dealFundsAllocRepository.GetTotalAllocatedFunds(
                                    request.FundingDealID.GetValueOrDefault());
                            UpdateDealMilestone(request.FundingDealID.GetValueOrDefault(), depositRequired,
                                depositReceived, fundingDeal.DealID.GetValueOrDefault());

                            scope.Complete();
                        }

                        if (fundingDeal.ActingFor != LawyerActingFor.Both && fundingDeal.ActingFor!=LawyerActingFor.Mortgagor)
                        {
                            NotifyLawyers(depositRequired, depositReceived, fundingDeal);
                        }
                    }

                    
                }
                else if (request.LawyerID.HasValue)
                {
                    fundsAllocation = _dealFundsAllocRepository.UpdateFundsAllocation(request);

                    if (request.AllocationStatus == AllocationStatus.Assigned)
                    {
                        var userDetails = _lawyerRepository.GetNotificationDetails(request.LawyerID.GetValueOrDefault());

                        string mailingList = _emailHelper.GetEmailRecipientList(userDetails,"EASYFUND"); //, businessModel);
                        FundsAllocationHelper.SendNotificationEmail(fundsAllocation, userDetails.UserLanguage, mailingList);

                        UpdateAuditLogs(userDetails, fundsAllocation); 
                    }

                }
                else
                {
                    _dealFundsAllocRepository.UpdateFundsAllocation(request);

                    FundsAllocationHelper.SendNotificationEmail(ConfigurationManager.AppSettings[EFSupportEmail]);
                }

        }

        private void UpdateAuditLogs(LawyerProfile userDetails, DealFundsAllocation fundsAllocation)
        {
            string lawyername = string.Format("{0} {1}", userDetails.FirstName, userDetails.LastName);
            string message = string.Format("Unallocated Funds ({0}) assigned to {1}",
                fundsAllocation.Amount.ToString("C", CultureInfo.CurrentCulture), lawyername);
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

        private void UpdateDealMilestone(int fundingdealId, decimal depositRequired, decimal totalDeposit, int dealId)
        {
            if (totalDeposit == depositRequired)
            {
                var milestone = new FundingMilestone()
                {
                    Funded = DateTime.Now
                };
                _fundedRepository.UpdateMilestones(fundingdealId, milestone);
            }
            else
            {
                var fundedDeal = _fundedRepository.GetMilestonesByDeal(dealId);
                if (fundedDeal != null)
                {
                    fundedDeal.Milestone.Funded = null;
                    _fundedRepository.UpdateFundedDeal(fundedDeal);
                }
            }
        }

        private void NotifyLawyers(decimal depositRequired, decimal depositReceived, FundingDeal fundingDeal)
        {
            var notificationDetails = _lawyerRepository.GetNotificationDetails(fundingDeal.Lawyer.LawyerID);
            var mailingList = _emailHelper.GetEmailRecipientList(fundingDeal);

            if (depositReceived == depositRequired)
            {
                FundsAllocationHelper.SendNotificationEmail(fundingDeal, notificationDetails,
                    StandardNotificationKey.EFDepositFundsMatch, mailingList.GetValueOrDefault().Value);

                if (fundingDeal.ActingFor != LawyerActingFor.Both && fundingDeal.ActingFor!=LawyerActingFor.Mortgagor)
                {
                    var otherLawyerNotificationDetails =
                        _lawyerRepository.GetNotificationDetails(fundingDeal.OtherLawyer.LawyerID);

                    int otherDealId = _fundingDealRepository.GetOtherDealInScope(fundingDeal.DealID.GetValueOrDefault());

                    var emailList = _emailHelper.GetEmailRecipientList(fundingDeal, otherDealId);
                    var fundedDeal = _fundedRepository.GetMilestonesByDeal(fundingDeal.DealID.GetValueOrDefault());

                    if (fundedDeal != null &&
                        fundedDeal.Milestone.SignedByVendor > DateTime.MinValue &&
                        fundedDeal.Milestone.SignedByPurchaser > DateTime.MinValue)
                    {
                        FundsAllocationHelper.SendNotificationEmail(fundingDeal, otherLawyerNotificationDetails,
                            StandardNotificationKey.EFDepositVendor, emailList.GetValueOrDefault().Value);
                    }
                }
            }
            if (depositReceived != depositRequired)
            {
                FundsAllocationHelper.SendNotificationEmail(fundingDeal, notificationDetails,
                    StandardNotificationKey.EFDepositFundsMismatch, mailingList.GetValueOrDefault().Value);
            }
        }
    }
}
