using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.BusinessService.Contracts.FaultContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;
using FCT.Services.LIM.DataContracts;
using FCT.Services.LIM.MessageContracts;
using FCT.Services.LIM.ServiceContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class SignDealBusinessLogic : ISignDealBusinessLogic
    {
        private readonly IDisbursementRepository _disbursementRepository;
        private readonly IDisbursementSummaryRepository _disbursementSummaryRepository;
        private readonly IFundedRepository _fundedRepository;
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly ILawyerRepository _lawyerRepository;
        private readonly IValidationHelper _validationHelper;
        private readonly IFundingDealRepository _fundingDealRepository;
        private readonly IEmailHelper _emailHelper;
        private readonly IMilestoneUpdater _milestoneUpdater;

        public SignDealBusinessLogic(IDisbursementRepository disbursementRepository,
            IDisbursementSummaryRepository disbursementSummaryRepository, IFundedRepository fundedRepository,
            IDealHistoryRepository dealHistoryRepository, IFundingDealRepository fundingDealRepository,
            ILawyerRepository lawyerRepository, IValidationHelper validationHelper, IEmailHelper emailHelper, IMilestoneUpdater milestoneUpdater)
        {
            _disbursementRepository = disbursementRepository;
            _disbursementSummaryRepository = disbursementSummaryRepository;
            _fundedRepository = fundedRepository;
            _dealHistoryRepository = dealHistoryRepository;
            _fundingDealRepository = fundingDealRepository;
            _lawyerRepository = lawyerRepository;
            _validationHelper = validationHelper;
            _emailHelper = emailHelper;
            _milestoneUpdater = milestoneUpdater;
        }

        public async Task SignDeal(SignDealRequest request)
        {
            var fundedDeal = _fundedRepository.GetMilestonesByDeal(request.Deal.DealID.GetValueOrDefault());
            if (fundedDeal != null &&
                (fundedDeal.Milestone.Disbursed == null || fundedDeal.Milestone.Disbursed <= DateTime.MinValue))
            {
                var errorcodes = new List<ErrorCode>();
                var version =
                    _disbursementSummaryRepository.GetDisbursementSummaryVersion(
                        request.DisbursementSummary.FundingDealID);
                if (!version.SequenceEqual(request.DisbursementSummary.Version))
                {
                    throw new DBConcurrencyException("Data has been updated since last GET");
                }            

                DisbursementCollection disbursements;
                var fundingDeal = _validationHelper.ValidateDealInDB(request.Deal.DealID.GetValueOrDefault(), errorcodes,
                    out disbursements);
                if (request.Deal.Mortgagors.Count <= 0)
                {
                    errorcodes.Add(ErrorCode.PurchaserMissing);
                }
                if (request.Deal.Vendors.Count <= 0 && request.Deal.ActingFor!=LawyerActingFor.Mortgagor)
                {
                    errorcodes.Add(ErrorCode.VendorMissing);
                }
               var taskResult= _validationHelper.ValidateVendorLawyer(disbursements, errorcodes, fundingDeal);

                var existingDeal = UniqueDealDescriptor.ToUniqueDeal(fundingDeal);
                var requestDeal = UniqueDealDescriptor.ToUniqueDeal(request.Deal);

                if (!SignDealHelper.AreAMatch(existingDeal, requestDeal))
                {
                    errorcodes.Add(ErrorCode.DealsDoNotMatch);
                }

                if (!SignDealHelper.AreAMatch(disbursements, request.Disbursements))
                {
                    errorcodes.Add(ErrorCode.DisbursementsDoNotMatch);
                }

                if ((request.Deal.ActingFor == LawyerActingFor.Vendor || request.Deal.ActingFor == LawyerActingFor.Purchaser) &&
                    ((request.Deal.OtherLawyer == null) || (request.Deal.OtherLawyer != null && request.Deal.OtherLawyer.LawyerID <= 0)))
                {
                    errorcodes.Add(ErrorCode.OtherLawyerMissing);
                }

                await taskResult;

                if (errorcodes.Count > 0 || request.IsValidateOnly)
                {
                    throw new ValidationException(errorcodes);
                }

                LawyerProfile recipientProfile;
                LawyerProfile vendorLawyerProfile = null;               
                string mailingList;
                string lawyerName;
                int otherDealId;

                LawyerProfile userProfile = _lawyerRepository.GetUserDetails(request.UserContext.UserID);

                switch (fundingDeal.ActingFor)
                {
                    case LawyerActingFor.Purchaser:

                        recipientProfile = _lawyerRepository.GetNotificationDetails(fundingDeal.OtherLawyer.LawyerID);
                        vendorLawyerProfile = recipientProfile;
                        lawyerName = string.Format("{0} {1}", userProfile.FirstName, userProfile.LastName);
                        otherDealId =
                            _fundingDealRepository.GetOtherDealInScope(fundingDeal.DealID.GetValueOrDefault());

                        using (var scope=TransactionScopeBuilder.CreateReadCommitted())
                        {
                            fundedDeal.Milestone.SignedByPurchaser = DateTime.Now;
                            fundedDeal.Milestone.SignedByPurchaserName = string.Format("{0} {1}", userProfile.FirstName,
                                userProfile.LastName);
                            _milestoneUpdater.UpdateMilestoneDepositReceived(fundedDeal);

                            _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(), request.UserContext,
                                DealActivity.EFDealSigned, lawyerName);

                            if (recipientProfile != null)
                            {
                                _dealHistoryRepository.CreateDealHistory(otherDealId, request.UserContext,
                                    DealActivity.EFDealSigned, lawyerName);
                            }
                            scope.Complete();
                        }
                       
                        //Get Email List including Delegates
                        mailingList = GetNotificationRecipients(fundingDeal);
                        if (!string.IsNullOrEmpty(mailingList))
                        {
                            recipientProfile.Email = mailingList;
                        }
                        _emailHelper.SendStandardNotification(fundingDeal, recipientProfile, LawyerActingFor.Vendor,
                            StandardNotificationKey.EFDealSigned);
                        break;

                    case LawyerActingFor.Vendor:

                        recipientProfile = _lawyerRepository.GetNotificationDetails(fundingDeal.OtherLawyer.LawyerID);
                        vendorLawyerProfile = _lawyerRepository.GetNotificationDetails(fundingDeal.Lawyer.LawyerID);
                        lawyerName = string.Format("{0} {1}", userProfile.FirstName, userProfile.LastName);
                        otherDealId =
                            _fundingDealRepository.GetOtherDealInScope(fundingDeal.DealID.GetValueOrDefault());

                        using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                        {
                            fundedDeal.Milestone.SignedByVendor = DateTime.Now;
                            fundedDeal.Milestone.SignedByVendorName = string.Format("{0} {1}", userProfile.FirstName,
                                userProfile.LastName);

                            _milestoneUpdater.UpdateMilestoneDepositReceived(fundedDeal);

                            _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(),
                                request.UserContext,
                                DealActivity.EFDealSigned, lawyerName);
                            if (recipientProfile != null)
                            {
                                _dealHistoryRepository.CreateDealHistory(otherDealId, request.UserContext,
                                    DealActivity.EFDealSigned, lawyerName);
                            }
                            scope.Complete();
                        }                      

                        //Get Email List including Delegates
                        mailingList = GetNotificationRecipients(fundingDeal);
                        if (!string.IsNullOrEmpty(mailingList))
                        {
                            recipientProfile.Email = mailingList;
                        }
                        _emailHelper.SendStandardNotification(fundingDeal, recipientProfile, LawyerActingFor.Purchaser,
                            StandardNotificationKey.EFDealSigned);
                        break;

                    case LawyerActingFor.Both:
                    case LawyerActingFor.Mortgagor:

                        lawyerName = string.Format("{0} {1}", userProfile.FirstName, userProfile.LastName);
                        using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                        {
                            var milestone = new FundingMilestone()
                            {
                                SignedByPurchaser = DateTime.Now,
                                SignedByPurchaserName =
                                    string.Format("{0} {1}", userProfile.FirstName, userProfile.LastName),
                            };
                            if (fundingDeal.ActingFor == LawyerActingFor.Both)
                            {
                                milestone.SignedByVendor = DateTime.Now;
                                milestone.SignedByVendorName = string.Format("{0} {1}", userProfile.FirstName,
                                    userProfile.LastName);
                            }
                            _fundedRepository.UpdateMilestones(request.DisbursementSummary.FundingDealID, milestone);
                            _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(),
                                request.UserContext,
                                DealActivity.EFDealSigned, lawyerName);
                            scope.Complete();
                        }
                        break;
                }

                if (fundingDeal.ActingFor != LawyerActingFor.Both && 
                    fundingDeal.ActingFor!=LawyerActingFor.Mortgagor && 
                    vendorLawyerProfile!=null)
                {
                    var updatedFundedDeal =
                        _fundedRepository.GetMilestonesByDeal(request.Deal.DealID.GetValueOrDefault());
                                        
                    otherDealId = _fundingDealRepository.GetOtherDealInScope(fundingDeal.DealID.GetValueOrDefault());
                    var emailList = _emailHelper.GetEmailRecipientList(fundingDeal, otherDealId);

                    if (updatedFundedDeal.Milestone.SignedByPurchaser > DateTime.MinValue &&
                        updatedFundedDeal.Milestone.SignedByVendor > DateTime.MinValue &&
                        updatedFundedDeal.Milestone.Funded > DateTime.MinValue)
                    {
                        FundsAllocationHelper.SendNotificationEmail(fundingDeal, vendorLawyerProfile,
                            StandardNotificationKey.EFDepositVendor, emailList.GetValueOrDefault().Value);
                    }
                } 
            }

        }


        public void UnsignDeal(UnsignDealRequest request)
        {
            var fundedDeal = _fundedRepository.GetMilestonesByDeal(request.DealID);
            if (fundedDeal != null && 
                (fundedDeal.Milestone.Disbursed == null || fundedDeal.Milestone.Disbursed <= DateTime.MinValue))
            {
                var fundingDeal = _fundingDealRepository.GetFundingDeal(request.DealID);

                using (var scope=TransactionScopeBuilder.CreateReadCommitted())
                {
                    switch (fundingDeal.ActingFor)
                    {
                        case LawyerActingFor.Purchaser:
                            fundedDeal.Milestone.SignedByPurchaser = null;
                            fundedDeal.Milestone.SignedByPurchaserName = null;
                            break;
                        case LawyerActingFor.Vendor:
                            fundedDeal.Milestone.SignedByVendor = null;
                            fundedDeal.Milestone.SignedByVendorName = null;
                            break;
                        case LawyerActingFor.Both:
                        case LawyerActingFor.Mortgagor:
                            fundedDeal.Milestone.SignedByPurchaser = null;
                            fundedDeal.Milestone.SignedByVendor = null;
                            fundedDeal.Milestone.SignedByPurchaserName = null;
                            fundedDeal.Milestone.SignedByVendorName = null;
                            break;
                    }
                    _fundedRepository.UpdateFundedDeal(fundedDeal);

                    _dealHistoryRepository.CreateDealHistory(fundingDeal.DealID.GetValueOrDefault(), request.UserContext,
                        DealActivity.EFDealSignatureRemoved, string.Format("{0} {1}", fundingDeal.Lawyer.FirstName, fundingDeal.Lawyer.LastName));

                    if (fundingDeal.ActingFor != LawyerActingFor.Both && fundingDeal.ActingFor!=LawyerActingFor.Mortgagor)
                    {
                        int otherDealId = _fundingDealRepository.GetOtherDealInScope(fundingDeal.DealID.GetValueOrDefault());

                        _dealHistoryRepository.CreateDealHistory(otherDealId, request.UserContext,
                            DealActivity.EFDealSignatureRemoved, string.Format("{0} {1}", fundingDeal.Lawyer.FirstName, fundingDeal.Lawyer.LastName));
                    }
                    scope.Complete();
                }

                if (fundingDeal.ActingFor != LawyerActingFor.Both && fundingDeal.ActingFor != LawyerActingFor.Mortgagor)
                {
                    EmailOtherLawyer(fundingDeal);
                }
            }
        }

        private void EmailOtherLawyer(FundingDeal fundingDeal)
        {
            var recipientProfile = _lawyerRepository.GetNotificationDetails(fundingDeal.OtherLawyer.LawyerID);
            string recipientActingFor = fundingDeal.ActingFor == LawyerActingFor.Purchaser
                ? LawyerActingFor.Vendor
                : LawyerActingFor.Purchaser;

            //Get Email List including Delegates
            string mailingList = GetNotificationRecipients(fundingDeal);
            if (!string.IsNullOrEmpty(mailingList))
            {
                recipientProfile.Email = mailingList;
            }
            _emailHelper.SendStandardNotification(fundingDeal, recipientProfile, recipientActingFor,
                StandardNotificationKey.EFDealUnSigned);
        }

        private string GetNotificationRecipients(FundingDeal _fundingDeal)
        {
            int OtherDealID = _fundingDealRepository.GetOtherDealInScope(_fundingDeal.DealID.Value);

            var mailerDetails = _emailHelper.GetEmailRecipientList(_fundingDeal, OtherDealID);

            string sEmailRecipients = mailerDetails.GetValueOrDefault().Value;

            return sEmailRecipients;

        }
    }
}
