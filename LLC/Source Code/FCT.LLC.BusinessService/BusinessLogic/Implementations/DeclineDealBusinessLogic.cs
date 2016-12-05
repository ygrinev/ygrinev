using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.DocumentService.Common;
using FCT.LLC.DocumentService.Client;
using FCT.LLC.Common.NotificationEmailDispatching.Client;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;
using UserType = FCT.LLC.Common.DataContracts.UserType;

namespace FCT.LLC.BusinessService.BusinessLogic.Implementations
{
    public class DeclineDealBusinessLogic : IDeclineDealBusinessLogic
    {
        private readonly IDealRepository _dealRepository;
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly IFundingDealRepository _dealFundingRepository;
        private readonly ILawyerRepository _lawyerRepository;
        private readonly IFundedRepository _fundedRepository;
        private readonly IEmailHelper _emailHelper;
        private readonly IVendorLawyerHelper _vendorLawyerHelper;
        private readonly IDisbursementRepository _disbursementRepository;

        public DeclineDealBusinessLogic(IDealRepository dealRepository, IDealHistoryRepository dealHistoryRepository
            , IFundingDealRepository dealFundingRepository, ILawyerRepository lawyerRepository,
            IFundedRepository fundedRepository, IEmailHelper emailHelper, IVendorLawyerHelper vendorLawyerHelper, IDisbursementRepository disbursementRepository
            )
        {
            _dealRepository = dealRepository;
            _dealHistoryRepository = dealHistoryRepository;
            _dealFundingRepository = dealFundingRepository;
            _lawyerRepository = lawyerRepository;
            _fundedRepository = fundedRepository;
            _emailHelper = emailHelper;
            _vendorLawyerHelper = vendorLawyerHelper;
            _disbursementRepository = disbursementRepository;
        }

        public void DeclineDeal(DeclineDealRequest request)
        {
            

            tblDeal invitedLawyerEasyFundDeal = _dealRepository.GetDealDetails(request.DealID, request.UserContext,true);

            if (null != invitedLawyerEasyFundDeal)
            {
                var DealID = SetInvitedLawyerDeal(request, invitedLawyerEasyFundDeal);

                int? dealScopeId = invitedLawyerEasyFundDeal.DealScopeID;

                int originatingLawyerDealId = _dealRepository.GetOtherDealInScope(invitedLawyerEasyFundDeal.DealID, dealScopeId.Value);

                tblDeal originatingLawyerDeal = _dealRepository.GetDealDetails(originatingLawyerDealId, request.UserContext, true);
                originatingLawyerDeal.StatusDate = DateTime.Now;
                originatingLawyerDeal.Status = DealStatus.UserDraft;

                var disbursements = _disbursementRepository.GetDisbursements(request.DealID);

                using (var scope=TransactionScopeBuilder.CreateReadCommitted())
                {
                    _dealRepository.UpdateDeal(invitedLawyerEasyFundDeal);

                    _dealRepository.UpdateDeal(originatingLawyerDeal);

                    //Clear vendor lawyer info if there are any vendor lawyer disbursements

                    if (disbursements.Count > 0)
                    {
                        _vendorLawyerHelper.ClearVendorLawyerDisbursement(disbursements);
                    }

                    //Update Deal History

                    _dealHistoryRepository.CreateDealHistory(request.DealID, request.UserContext, DealActivity.EFDealDeclined);

                    _dealHistoryRepository.CreateDealHistory(originatingLawyerDealId, request.UserContext, DealActivity.EFDealDeclined);

                    //Update Invitation Sent Milestone in tblFundingDeal Table
                    UpdateInvitationSentMilestone(DealID);

                    scope.Complete();
                }
                
                SendNotification(originatingLawyerDeal, DealID, dealScopeId);
            }
        }

        private void SendNotification(tblDeal originatingLawyerDeal, int DealID, int? dealScopeId)
        {
//Send Email Notification
            var recipientProfile = _lawyerRepository.GetNotificationDetails(originatingLawyerDeal.LawyerID);
            var fundingDeal = _dealFundingRepository.GetFundingDeal(DealID);

            //Get email Notification Recipients to avoid calling it more than once
            string sOtherNotificationRecipients = GetNotificationRecipientsForOtherLawyer(fundingDeal, dealScopeId);

            //update with notification recipients received before
            if (!string.IsNullOrEmpty(sOtherNotificationRecipients))
            {
                recipientProfile.Email = sOtherNotificationRecipients;
            }

            string recipientActingFor = fundingDeal.ActingFor == LawyerActingFor.Purchaser
                ? LawyerActingFor.Vendor
                : LawyerActingFor.Purchaser;
            IDictionary<string, object> tokens = _emailHelper.CreateTokens(fundingDeal, recipientActingFor);

            EmailHelper.SendStandardNotificationwithTokens(fundingDeal, recipientProfile, recipientActingFor,
                StandardNotificationKey.EFDealInviteDeclined, tokens);
        }

        private static int SetInvitedLawyerDeal(DeclineDealRequest request, tblDeal invitedLawyerEasyFundDeal)
        {
            int DealID = request.DealID;
            invitedLawyerEasyFundDeal.StatusDate = DateTime.Now;
            invitedLawyerEasyFundDeal.Status = DealStatus.Declined;
            invitedLawyerEasyFundDeal.StatusReason = "Declined by Lawyer";
            invitedLawyerEasyFundDeal.UserNotification = true;
            //invitedLawyerEasyFundDeal.StatusUserId = PageHelper.CurrentIdentity.UserId;
            invitedLawyerEasyFundDeal.StatusUserType = UserType.Lawyer;
            invitedLawyerEasyFundDeal.LawyerDeclinedFlag = true;
            invitedLawyerEasyFundDeal.LawyerAcceptDeclinedDate = DateTime.Now;
            invitedLawyerEasyFundDeal.LawyerApplication = LawyerApplication.Portal;
            return DealID;
        }

        private void UpdateInvitationSentMilestone(int DealID)
        {
            FundedDeal _fundedDeal = new FundedDeal();
            _fundedDeal = _fundedRepository.GetMilestonesByDeal(DealID);
            
            if(_fundedDeal != null)
            {
                _fundedDeal.Milestone.InvitationSent = null;
                _fundedRepository.UpdateFundedDeal(_fundedDeal);
            }
        }

        private string GetNotificationRecipientsForOtherLawyer(FundingDeal _fundingDeal, int? DealScopeID)
        {
            int OtherDealID = 0;
            if (DealScopeID.HasValue && _fundingDeal.DealID.HasValue)
            {
                OtherDealID = _dealRepository.GetOtherDealInScope(_fundingDeal.DealID.Value, DealScopeID.Value);
            }

            var mailerDetails = _emailHelper.GetEmailRecipientList(_fundingDeal, OtherDealID);

            string sEmailRecipients = mailerDetails.GetValueOrDefault().Value;

            return sEmailRecipients;

        }

    }
}
