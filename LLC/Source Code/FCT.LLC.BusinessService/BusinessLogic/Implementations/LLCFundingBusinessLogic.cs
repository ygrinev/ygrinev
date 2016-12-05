using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.NotificationEmailDispatching.Entities;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public partial class DealManagementBusinessLogic
    {
        public void SyncFundingDealData(SyncFundingDealDataRequest request)
        {
            _userContext = request.UserContext;

            int targetDealId = _dealFundingRepository.GetOtherDealInScope(request.DealID);

            //If other deal does not exists then do not continue with the process
            if (targetDealId <= 0) return;

            //this is for syncing LLC deal data changes to EasyFund deal. this is called from Mortgagor tab and Property tab and lender updates
            var sourceDeal = _dealFundingRepository.GetFundingDeal(request.DealID);
            int dealScopeId = _dealScopeRepository.GetDealScope(sourceDeal.FCTURN);
            var targetDeal = _dealFundingRepository.GetFundingDeal(targetDealId);
            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                //update target deal
                UpdateOtherDeal(request.DealID, sourceDeal, dealScopeId, LawyerActingFor.Vendor, targetDeal.DealStatus);
                sourceDeal = _dealFundingRepository.GetFundingDeal(request.DealID);
                //find amendments and record history in target deal
                bool hasAmendments = _amendmentsChecker.CheckLLCAmendments(targetDeal, sourceDeal, request.UserContext);
                //If there is amendments, remove signature for both parties
                if (hasAmendments) RemoveSignature(sourceDeal.DealID.GetValueOrDefault());

                scope.Complete();
            }
        }

        private FundingDeal CreateFundingDealForLLC(FundingDeal requestDeal)
        {
            int otherDealId = 0;
            string longFCTURN = null;
            int dealScopeId = 0;

            string wireDepositCode = FCTURNHelper.GenerateWireDepositCode();
            Entities.tblDeal dealDetails = _dealRepository.GetDealWithDealScopeDetails((int)requestDeal.DealID);

            if (dealDetails != null && dealDetails.tblDealScope != null && dealDetails.tblDealScope.DealScopeID > 0)
            {
                dealScopeId = dealDetails.tblDealScope.DealScopeID;
                longFCTURN = dealDetails.tblDealScope.FCTRefNumber;
                globalFCTURN = dealDetails.tblDealScope.ShortFCTRefNumber;
            }
            else
            {
                longFCTURN = FundingBusinessLogicHelper.GetFCTURN();
                globalFCTURN = FundingBusinessLogicHelper.GetShortFCTURN(longFCTURN);
            }

            bool isDraft = true;
            bool isNotificationToBeSent = false;

            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
            {
                //change business model from LLC to LLC/EASYFUND or MMS to MMS/EASYFUND
                requestDeal.BusinessModel = (requestDeal.BusinessModel == BusinessModel.MMS) ? BusinessModel.MMSCOMBO : BusinessModel.LLCCOMBO;

                //create a new deal scope if it does not exist
                if (dealScopeId <= 0)
                {
                    dealScopeId = CreateDealScope(longFCTURN, wireDepositCode);
                }
                else
                {
                    UpdateWireDepositDetails(longFCTURN, wireDepositCode);
                }

                //Update LLC deal with new deal scope and business model
                _dealFundingRepository.UpdateLLCDeal(requestDeal, dealScopeId);

                //Update deal contacts since they may already exist for LLC deal.
                _dealContactRepository.UpdateDealContactRange(requestDeal.Lawyer.DealContacts, requestDeal.DealID.Value);

                //Add a deal history entry for current deal for adding easyfund
                var dealhistories = new List<UserHistory>
                {
                    new UserHistory()
                    {
                        Activity = DealActivity.EFAddedToDeal,
                        DealId = requestDeal.DealID.GetValueOrDefault()
                    }
                };

                switch (requestDeal.ActingFor)
                {
                    case LawyerActingFor.Both:
                        //add vendor information
                        _vendorRepository.InsertVendorRange(requestDeal.Vendors, dealScopeId);
                        isDraft = false;
                        break;
                    case LawyerActingFor.Mortgagor:
                        isDraft = false;
                        break;
                    case LawyerActingFor.Purchaser:
                        //add vendor information
                        _vendorRepository.InsertVendorRange(requestDeal.Vendors, dealScopeId);
                        //Only create a second deal if acting for purchaser and the other lawyer has been assigned. Not when acting for both
                        if ((requestDeal.OtherLawyer != null) && (requestDeal.OtherLawyer.LawyerID > 0))
                        {
                            //create the other deal for vendor
                            otherDealId = CreateOtherDeal(requestDeal, dealScopeId, LawyerActingFor.Vendor,
                                requestDeal.OtherLawyerDealStatus);
                            //send notification and log history if deal status is new

                            if (otherDealId > 0)
                            {
                                if (requestDeal.OtherLawyerDealStatus == DealStatus.New)
                                {
                                    isDraft = false;
                                    isNotificationToBeSent = true;
                                    FundingBusinessLogicHelper.LogHistoryForNewDeal(requestDeal, dealhistories, otherDealId);
                                }
                            }
                        }
                        break;

                }

                //update milestone or add a blank funding deal record.
                UpdateMilestones(dealScopeId, false, isDraft);

                //Create document metadata for the LLC deal
                GenerateDocumentMetadata(requestDeal.DealID.GetValueOrDefault());

                //log deal history
                _dealHistoryRepository.CreateDealHistories(dealhistories, _userContext);

                //Send business model to MMS
                if (!string.IsNullOrEmpty(requestDeal.BusinessModel) && requestDeal.BusinessModel.ToUpper().Contains(BusinessModel.MMS))
                {
                    _dealEventsPublisher.SendDealBusinessModel((int)requestDeal.DealID, requestDeal.Lawyer.LawyerID);
                }

                scope.Complete();
            }

            //Notify the other user
            if (isNotificationToBeSent)
            {
                NotifyLawyerAndDelegates(LawyerActingFor.Purchaser, requestDeal, otherDealId);
            }

            return requestDeal;
        }     

        private FundingDeal UpdateFundingDealForLLC(FundingDeal requestDeal, UserContext user)
        {
            //this is to handel LLC/EasyFund combo deal, calling from Transaction Details tab. So only Vendor and Closing Date may be affected.
            bool hasAmendments = false;
            const string TYPE_BUSINESS = "BUSINESS";
            //get deal scope
            int dealScopeId = _dealScopeRepository.GetDealScope(requestDeal.FCTURN);
            int dealId = requestDeal.DealID.Value;
            var histories = new List<UserHistory>();

            switch (requestDeal.ActingFor)
            {
                case LawyerActingFor.Both:
                case LawyerActingFor.Mortgagor:

                    //get old data for compare so that deal history can record data changes
                    FundingDeal oldFundingDeal = _dealFundingRepository.GetFundingDeal(dealId);

                    using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                    {
                        //update deal contacts
                        _dealContactRepository.UpdateDealContactRange(requestDeal.Lawyer.DealContacts, dealId);
                        if (requestDeal.PrimaryDealContact.ContactID > 0)
                            _dealFundingRepository.UpdateDealContact(dealId, requestDeal.PrimaryDealContact.ContactID);

                        if (requestDeal.ActingFor == LawyerActingFor.Both)
                        {
                            //update vendor information
                            _vendorRepository.UpdateVendorRange(requestDeal.Vendors, dealScopeId);
                            _amendmentsChecker.HasVendorAmendments(oldFundingDeal, requestDeal, histories, dealId, false, 0);
                            _dealHistoryRepository.CreateDealChangeHistories(histories, user);
                        }

                        //If there is amendments, remove signature for both parties
                        //FIX FOR DEFECT# 41232
                        if (requestDeal.HasAmendments)
                        {
                            RemoveSignature(dealId);
                        }

                        if (oldFundingDeal.ActingFor != requestDeal.ActingFor)
                        {
                            _dealFundingRepository.UpdateLLCDeal(requestDeal, dealScopeId);
                        }
                        scope.Complete();
                    }
                    break;
                case LawyerActingFor.Purchaser:
                    int targetDealId = _dealFundingRepository.GetOtherDealInScope(dealId);

                    //update vendors if invitation has not been sent
                    var fundedDeal=_fundedRepository.GetFundingDealByScope(dealScopeId);
                    if (fundedDeal != null &&
                        (fundedDeal.Milestone.InvitationSent == null || fundedDeal.Milestone.InvitationSent <= DateTime.MinValue))
                    {
                        _vendorRepository.UpdateVendorRange(requestDeal.Vendors, dealScopeId);
                    }

                    _dealContactRepository.UpdateDealContactRange(requestDeal.Lawyer.DealContacts, dealId);
                    _dealFundingRepository.UpdateDealContact(dealId, requestDeal.PrimaryDealContact.ContactID);

                    if (requestDeal.PrimaryDealContact.ContactID > 0)
                        _dealFundingRepository.UpdateDealContact(dealId, requestDeal.PrimaryDealContact.ContactID);

                    FundingDeal oldFundingDeal1 = _dealFundingRepository.GetFundingDeal(dealId);
                    if (oldFundingDeal1.ActingFor != requestDeal.ActingFor)
                    {
                        _dealFundingRepository.UpdateLLCDeal(requestDeal, dealScopeId);
                    }


                    if (targetDealId > 0)
                    {                        
                        var targetDeal = _dealFundingRepository.GetFundingDeal(targetDealId);
                        bool passiveUserSignRemoved = false;
                        if (targetDeal != null)
                        {
                            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                            {
                                //update target deal
                                UpdateOtherDeal(dealId, requestDeal, dealScopeId, LawyerActingFor.Vendor,
                                    targetDeal.DealStatus, fundedDeal);

                                //Update Closing Date for other deal
                                if (targetDeal.ClosingDate != requestDeal.ClosingDate)
                                {
                                    hasAmendments = true;
                                    histories.Add(new UserHistory()
                                    {
                                        Activity = HistoryMessage.ClosingDateChange,
                                        DealId = targetDealId,
                                        OldValue =
                                            targetDeal.ClosingDate == null
                                                ? string.Empty
                                                : targetDeal.ClosingDate.Value.ToLongDateString(),
                                        NewValue =
                                            requestDeal.ClosingDate == null
                                                ? string.Empty
                                                : requestDeal.ClosingDate.Value.ToLongDateString()
                                    });
                                }
                                _dealHistoryRepository.CreateDealChangeHistories(histories, user);

                                if (hasAmendments || requestDeal.HasAmendments)
                                {
                                    var milestonesUpdated = _milestoneUpdater.UpdateMilestones(dealId,
                                        LawyerActingFor.Purchaser);

                                    int otherDealId = _dealFundingRepository.GetOtherDealInScope(dealId);

                                    passiveUserSignRemoved = UpdateDealHistoriesSignRemoved(requestDeal,
                                        milestonesUpdated, dealId, otherDealId);
                                }

                                scope.Complete();
                            }
                            if (passiveUserSignRemoved)
                            {
                                SendNotificationEmail(requestDeal);
                            }

                            //update milestone, send notification and log history if the other deal status is changed from System Draft to New
                            if (targetDeal.DealStatus == DealStatus.SystemDraft &&
                                requestDeal.OtherLawyerDealStatus == DealStatus.New)
                            {
                                ActOnDealStatusChange(requestDeal, dealScopeId, targetDealId);
                            }
                           
                            
                        }
                    }
                    else
                    {
                        //create a second deal if not created before when the easyfund part was saved as a draft previously
                        if ((requestDeal.OtherLawyer != null) && (requestDeal.OtherLawyer.LawyerID > 0))
                        {
                            int otherDealId;
                            using (var scope = TransactionScopeBuilder.CreateReadCommitted())
                            {
                                //create the other deal for vendor
                                otherDealId = CreateOtherDeal(requestDeal, dealScopeId, LawyerActingFor.Vendor,
                                    requestDeal.OtherLawyerDealStatus);
                                scope.Complete();
                            }

                            //update milestone, send notification and log history if the other deal status is  New
                            if (requestDeal.OtherLawyerDealStatus == DealStatus.New)
                            {
                                ActOnDealStatusChange(requestDeal, dealScopeId, otherDealId);
                            }
                        }
                    }
                    break;
            }

            return requestDeal;
        }

        private void ActOnDealStatusChange(FundingDeal requestDeal, int dealScopeId, int targetDealId)
        {
            UpdateMilestones(dealScopeId, true, false);
            NotifyLawyerAndDelegates(LawyerActingFor.Purchaser, requestDeal, targetDealId);
            var histories=new List<UserHistory>();
            FundingBusinessLogicHelper.LogHistoryForNewDeal(requestDeal, histories, targetDealId);
            _dealHistoryRepository.CreateDealHistories(histories, _userContext);
        }

        private bool UpdateDealHistoriesSignRemoved(FundingDeal requestDeal, MilestoneUpdated milestonesUpdated,
            int dealId,
            int otherDealId)
        {
            bool passiveUserSingRemoved = false;

            if (milestonesUpdated.ActiveUserSignRemoved &&
                milestonesUpdated.PassiveUserSignRemoved)
            {
                UpdatehistoriesAllSignaturesRemoved(requestDeal, dealId, otherDealId);
                passiveUserSingRemoved = true;
            }
            else if (milestonesUpdated.ActiveUserSignRemoved &&
                     !milestonesUpdated.PassiveUserSignRemoved)
            {
                _dealHistoryRepository.CreateDealHistory(dealId, _userContext,
                    DealActivity.EFDealSignatureRemoved,
                    string.Format("{0} {1}", requestDeal.Lawyer.FirstName,
                        requestDeal.Lawyer.LastName));
                if (otherDealId > 0)
                {
                    _dealHistoryRepository.CreateDealHistory(otherDealId, _userContext,
                        DealActivity.EFDealSignatureRemoved,
                        string.Format("{0} {1}", requestDeal.Lawyer.FirstName,
                            requestDeal.Lawyer.LastName));
                }
            }
            else if (milestonesUpdated.PassiveUserSignRemoved &&
                     !milestonesUpdated.ActiveUserSignRemoved)
            {
                if ((requestDeal.OtherLawyer != null) && (requestDeal.OtherLawyer.LawyerID > 0))
                {
                    _dealHistoryRepository.CreateDealHistory(dealId, _userContext,
                        DealActivity.EFDealSignatureRemoved,
                        string.Format("{0} {1}", requestDeal.OtherLawyer.FirstName,
                            requestDeal.OtherLawyer.LastName));
                    if (otherDealId > 0)
                    {
                        _dealHistoryRepository.CreateDealHistory(otherDealId, _userContext,
                            DealActivity.EFDealSignatureRemoved,
                            string.Format("{0} {1}", requestDeal.OtherLawyer.FirstName,
                                requestDeal.OtherLawyer.LastName));
                    }
                }
                passiveUserSingRemoved = true;
            }
            return passiveUserSingRemoved;
        }

        private void UpdatehistoriesAllSignaturesRemoved(FundingDeal requestDeal, int dealId, int otherDealId)
        {
            _dealHistoryRepository.CreateDealHistory(dealId, _userContext,
                DealActivity.EFDealSignatureRemoved,
                string.Format("{0} {1}", requestDeal.Lawyer.FirstName,
                    requestDeal.Lawyer.LastName));
            if (otherDealId > 0)
            {
                _dealHistoryRepository.CreateDealHistory(otherDealId, _userContext,
                    DealActivity.EFDealSignatureRemoved,
                    string.Format("{0} {1}", requestDeal.Lawyer.FirstName,
                        requestDeal.Lawyer.LastName));
            }

            if ((requestDeal.OtherLawyer != null) && (requestDeal.OtherLawyer.LawyerID > 0))
            {
                _dealHistoryRepository.CreateDealHistory(dealId, _userContext,
                    DealActivity.EFDealSignatureRemoved,
                    string.Format("{0} {1}", requestDeal.OtherLawyer.FirstName,
                        requestDeal.OtherLawyer.LastName));

                if (otherDealId > 0)
                {
                    _dealHistoryRepository.CreateDealHistory(otherDealId, _userContext,
                        DealActivity.EFDealSignatureRemoved,
                        string.Format("{0} {1}", requestDeal.OtherLawyer.FirstName,
                            requestDeal.OtherLawyer.LastName));
                }
            }
        }


        public void RemoveSignature(int dealId)
        {
            //default, don't send notification, don't log deal history
            bool isNotificationRequired = false;
            bool isHistoryRequired = false;
            bool isHistoryRequiredForOtherDeal = false;
            var fundedDeal = _fundedRepository.GetMilestonesByDeal(dealId);
            if (fundedDeal != null && fundedDeal.Milestone.Disbursed.HasValue && fundedDeal.Milestone.Disbursed > DateTime.MinValue)
            {
                return;
            }
            var fundingDeal = _dealFundingRepository.GetFundingDeal(dealId);
            int otherDealId = _dealFundingRepository.GetOtherDealInScope(dealId);
            if (fundingDeal != null)
            {
                switch (fundingDeal.ActingFor)
                {
                    case LawyerActingFor.Purchaser:
                        if (fundedDeal.Milestone.SignedByPurchaser != null)
                        {
                            //remove own signature
                            fundedDeal.Milestone.SignedByPurchaser = null;
                            fundedDeal.Milestone.SignedByPurchaserName = null;
                            isHistoryRequired = true;
                        }
                        if (fundedDeal.Milestone.SignedByVendor != null)
                        {
                            fundedDeal.Milestone.SignedByVendor = null;
                            fundedDeal.Milestone.SignedByVendorName = null;
                            isHistoryRequiredForOtherDeal = true;
                            isNotificationRequired = true;
                        }
                        break;
                    case LawyerActingFor.Vendor:
                        if (fundedDeal.Milestone.SignedByVendor != null)
                        {
                            //remove own signature
                            fundedDeal.Milestone.SignedByVendor = null;
                            fundedDeal.Milestone.SignedByVendorName = null;
                            isHistoryRequired = true;
                        }
                        if (fundedDeal.Milestone.SignedByPurchaser != null)
                        {
                            //remove own signature
                            fundedDeal.Milestone.SignedByPurchaser = null;
                            fundedDeal.Milestone.SignedByPurchaserName = null;
                            isHistoryRequiredForOtherDeal = true;
                            isNotificationRequired = true;
                        }
                        break;
                    case LawyerActingFor.Both:
                    case LawyerActingFor.Mortgagor:
                        if (fundedDeal.Milestone.SignedByVendor != null ||
                            fundedDeal.Milestone.SignedByPurchaser != null)
                        {
                            //remove own signature
                            fundedDeal.Milestone.SignedByVendor = null;
                            fundedDeal.Milestone.SignedByPurchaser = null;
                            fundedDeal.Milestone.SignedByVendorName = null;
                            fundedDeal.Milestone.SignedByPurchaserName = null;
                            isHistoryRequired = true;
                        }
                        break;
                }
                _fundedRepository.UpdateFundedDeal(fundedDeal);

                if (isNotificationRequired)
                {
                    SendNotificationEmail(fundingDeal);
                }

                if (isHistoryRequired)
                {
                    //remove signature of current deal lawyer and record history in current deal
                    _dealHistoryRepository.CreateDealHistory(dealId, _userContext,
                        DealActivity.EFDealSignatureRemoved,
                        string.Format("{0} {1}", fundingDeal.Lawyer.FirstName, fundingDeal.Lawyer.LastName));

                    //remove signature of current deal lawyer and record history in other lawyer's deal
                    if (otherDealId > 0 && fundingDeal.OtherLawyer != null)
                    {
                        _dealHistoryRepository.CreateDealHistory(otherDealId, _userContext,
                            DealActivity.EFDealSignatureRemoved,
                            string.Format("{0} {1}", fundingDeal.Lawyer.FirstName, fundingDeal.Lawyer.LastName));
                    }
                }
                if (isHistoryRequiredForOtherDeal)
                {
                    //remove signature of the other lawyer and record history in current deal
                    _dealHistoryRepository.CreateDealHistory(dealId, _userContext,
                        DealActivity.EFDealSignatureRemoved,
                        string.Format("{0} {1}", fundingDeal.OtherLawyer.FirstName, fundingDeal.OtherLawyer.LastName));

                    //remove signature of the other lawyer and record history in other lawyer's deal
                    if (otherDealId > 0 && fundingDeal.OtherLawyer != null)
                    {
                        _dealHistoryRepository.CreateDealHistory(otherDealId, _userContext,
                            DealActivity.EFDealSignatureRemoved,
                            string.Format("{0} {1}", fundingDeal.OtherLawyer.FirstName, fundingDeal.OtherLawyer.LastName));
                    }
                }
            }
        }

        private void SendNotificationEmail(FundingDeal fundingDeal)
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
                StandardNotificationKey.EFUpdatedDealUnSigned);
        }


        private string GetNotificationRecipients(FundingDeal _fundingDeal)
        {
            int OtherDealID = _dealFundingRepository.GetOtherDealInScope(_fundingDeal.DealID.Value);

            var mailerDetails = _emailHelper.GetEmailRecipientList(_fundingDeal, OtherDealID);

            string sEmailRecipients = mailerDetails.GetValueOrDefault().Value;

            return sEmailRecipients;
        }
    }
}
