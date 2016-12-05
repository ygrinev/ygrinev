using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;
using FCT.Services.LIM.DataContracts;
using FCT.Services.LIM.MessageContracts;
using FCT.Services.LIM.ServiceContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class VendorLawyerHelper:IVendorLawyerHelper
    {
        private readonly IDisbursementRepository _disbursementRepository;
        private readonly ILIMServiceContract _limService;
        private readonly IDealHistoryRepository _dealHistoryRepository;
        private readonly IFundingDealRepository _fundingDealRepository;

        private UserContext _userContext;
        private int _dealID;
        private string _trustAccount;

        public VendorLawyerHelper(ILIMServiceContract limService, IDisbursementRepository disbursementRepository, IDealHistoryRepository dealHistoryRepository, IFundingDealRepository fundingDealRepository)
        {
            _limService = limService;
            _disbursementRepository = disbursementRepository;
            _dealHistoryRepository = dealHistoryRepository;
            _fundingDealRepository = fundingDealRepository;

        }

        public void UpdateVendorLawyerDisbursement(Lawyer vendorLawyer, int dealId, UserContext userContext,string currentActingFor, string oldActingFor)
        {
            _userContext = userContext;
            _dealID = dealId;

            var disbursements = _disbursementRepository.GetDisbursements(dealId);

            if (currentActingFor == LawyerActingFor.Both && currentActingFor != oldActingFor)
            {
                ClearVendorLawyerDisbursement(disbursements);
            }
            if (currentActingFor == LawyerActingFor.Purchaser && oldActingFor == LawyerActingFor.Vendor)
            {
                ClearVendorLawyerDisbursement(disbursements);
            }
            if(currentActingFor == oldActingFor && currentActingFor==LawyerActingFor.Purchaser)
            {
                UpdateDisbursement(vendorLawyer, disbursements, currentActingFor); 
            }
            if (currentActingFor == LawyerActingFor.Vendor && oldActingFor == LawyerActingFor.Purchaser)
            {
                UpdateDisbursement(vendorLawyer, disbursements, currentActingFor); 
            }
           
        }

        private void UpdateDisbursement(Lawyer vendorLawyer, IEnumerable<Disbursement> disbursements,
            string currentActingFor)
        {
            foreach (var disbursement in disbursements)
            {
                if (disbursement.PayeeType == FeeDistribution.VendorLawyer)
                {
                    string vendorLawyerName = string.Format("{0} {1}", vendorLawyer.FirstName, vendorLawyer.LastName);

                    if (string.IsNullOrEmpty(disbursement.PayeeName) || disbursement.PayeeName != vendorLawyerName)
                    {
                        disbursement.PayeeName = vendorLawyerName;
                        disbursement.NameOnCheque = vendorLawyerName;

                        bool trustAccountAssigned = AssignActiveExistingTrustAccount(vendorLawyer, disbursement);
                        _disbursementRepository.UpdateDisbursement(disbursement);

                        if (trustAccountAssigned)
                        {
                            var history = new DisbursementHistory()
                            {
                                Payee = vendorLawyerName,
                                TrustAccount = _trustAccount
                            };
                            if (currentActingFor == LawyerActingFor.Vendor)
                            {
                                _dealHistoryRepository.CreateDealHistory(_dealID, DealActivity.TrustAccountEdited,
                                    _userContext, history);
                            }
                            if (currentActingFor == LawyerActingFor.Purchaser)
                            {
                                int vendorDealId = _fundingDealRepository.GetOtherDealInScope(_dealID);
                                _dealHistoryRepository.CreateDealHistory(vendorDealId, DealActivity.TrustAccountEdited,
                                    _userContext, history);
                            }
                        }
                    }
                }
            }
        }

        public bool AssignActiveExistingTrustAccount(Lawyer vendorLawyer, Disbursement disbursement)
        {
            bool hasActiveTrustAccount = false;
            var trustAccountRequest = new GetTrustAccountsRequest() {UserID = vendorLawyer.LawyerID};
            var trustAccountResponse = _limService.GetTrustAccounts(trustAccountRequest);
            var activeAccounts =
                trustAccountResponse.TrustAccounts.Where(t => t.TrustAccountStatusID == (int) AccountStatus.Active);

            if (activeAccounts.Count() == 1)
            {
                var trustAccount = activeAccounts.SingleOrDefault();
                if (trustAccount != null)
                {
                    disbursement.TrustAccountID = trustAccount.TrustAccountID;
                    disbursement.AccountNumber = trustAccount.AccountNum;
                    disbursement.BankNumber = trustAccount.BankNum;
                    disbursement.BranchNumber = trustAccount.BranchNum;

                    _trustAccount = string.Format("{0}-{1}-{2}", trustAccount.BankNum, trustAccount.BranchNum,
                        trustAccount.AccountNum);
                    hasActiveTrustAccount = true;
                }
            }
            else
            {
                disbursement.TrustAccountID = null;
                disbursement.AccountNumber = null;
                disbursement.BankNumber = null;
                disbursement.BranchNumber = null;
            }
            return hasActiveTrustAccount;
        }

        public  void ClearVendorLawyerDisbursement(IEnumerable<Disbursement> disbursements)
        {
            foreach (var disbursement in disbursements)
            {
                if (disbursement.PayeeType == FeeDistribution.VendorLawyer)
                {
                    disbursement.PayeeName = null;
                    disbursement.NameOnCheque = null;
                    disbursement.TrustAccountID = null;
                    disbursement.AccountNumber = null;
                    disbursement.BankNumber = null;
                    disbursement.BranchNumber = null;

                    _disbursementRepository.UpdateDisbursement(disbursement);
                }
            }
        }

        public DisbursementCollection AdjustVendorLawyerDisbursement(DisbursementCollection disbursements, Fee fee,
            string feeSplit)
        {
            foreach (var disbursement in disbursements)
            {
                if (disbursement.PayeeType == FeeDistribution.VendorLawyer && fee != null)
                {
                    AssignVendorLawyerDisbursedAmount(fee, feeSplit, disbursement);
                }
            }
            return disbursements;
        }

        internal static void AssignVendorLawyerDisbursedAmount(Fee fee, string feeSplit, Disbursement disbursement)
        {
            var feeWithTaxes = fee.Amount + fee.GST + fee.HST + fee.QST;
            if (disbursement.Action == CRUDAction.None)
            {
                disbursement.Action=CRUDAction.Update;
            }
            switch (feeSplit)
            {
                case FeeDistribution.SplitEqually:
                case FeeDistribution.VendorLawyer:
                    disbursement.DisbursedAmount = disbursement.Amount -
                                                   Math.Round(feeWithTaxes, 2, MidpointRounding.AwayFromZero);
                    break;
                case FeeDistribution.PurchaserLawyer:
                    disbursement.DisbursedAmount = disbursement.Amount;
                    break;
            }
        }

        public UserProfile GetUserProfile(string userName)
        {
            var userProfileRequest = new GetUserProfileByUserNameRequest() {UserName = userName};
            var userProfileResponse = _limService.GetUserProfileByUserName(userProfileRequest);
            if (userProfileResponse != null)
            {
                return userProfileResponse.UserProfile;
            }
            return null;
        }

        public void SyncAnyLawyerActingForChanges(FundingDeal currentDeal, string oldActingFor)
        {
            if (FundingBusinessLogicHelper.IsLawyerActingForSwapped(currentDeal.ActingFor, oldActingFor))
            {
                int otherDealId=_fundingDealRepository.GetOtherDealInScope(currentDeal.DealID.GetValueOrDefault());
                string otherActingFor = _fundingDealRepository.GetLawyerActingFor(otherDealId);
                if (otherActingFor == currentDeal.ActingFor)
                {
                    _fundingDealRepository.RemoveDealFromScope(otherDealId);
                }
            }
        }
    }
}
