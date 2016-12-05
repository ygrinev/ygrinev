using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class DisbursementHelper
    {
        public static bool HasCreditCardNumberChanged(Disbursement presavingDisbursement,
            Disbursement afterSavingDisbursement)
        {
            if (afterSavingDisbursement.PayeeType == PayeeType.CreditCard)
            {
                if (presavingDisbursement.ReferenceNumber != afterSavingDisbursement.ReferenceNumber)
                {
                    return true;
                }
                if (presavingDisbursement.ReferenceNumber == afterSavingDisbursement.ReferenceNumber &&
                    !string.IsNullOrEmpty(presavingDisbursement.Token) &&
                    !string.IsNullOrEmpty(afterSavingDisbursement.Token) &&
                    presavingDisbursement.Token != afterSavingDisbursement.Token)
                {
                    return true;
                }
            }
            return false;
        }

        public static void ValidateDealMilestones(DisbursementSummary summary, string lawyerActingFor, List<ErrorCode> errorcodes)
        {
            if (summary.DepositAmountReceived != summary.RequiredDepositAmount)
            {
                errorcodes.Add(ErrorCode.FundsDoNotMatch);
            }
            if (summary.FundingMilestone.SignedByPurchaser == DateTime.MinValue ||
                summary.FundingMilestone.SignedByPurchaser == null)
            {
                errorcodes.Add(ErrorCode.PurchaserHasNotSigned);
            }
            if (lawyerActingFor != LawyerActingFor.Mortgagor && (summary.FundingMilestone.SignedByVendor == DateTime.MinValue ||
                summary.FundingMilestone.SignedByVendor == null))
            {
                errorcodes.Add(ErrorCode.VendorHasNotSigned);
            }
        }

        private static IEnumerable<ErrorCode> ValidateChainDeal(Disbursement disbursement, int dealId, UserContext userContext,
            IFundingDealRepository fundingDealRepository, IDealRepository dealRepository)
        {
            var errorcodes = new List<ErrorCode>();

            //Check if FCT Ref Number is LLC Only deal
            var llcDeal = dealRepository.GetDealDetailsByFCTURN(disbursement.ReferenceNumber);

            // 3. ensure EASYFUND is present in associated deal business model.
            if (llcDeal !=  null && (llcDeal.BusinessModel != BusinessModel.LLC || !llcDeal.BusinessModel.Contains(BusinessModel.EASYFUND)))
            {
                errorcodes.Add(ErrorCode.ChainDealNotEasyFund);
                return errorcodes;
            }


            // Get DealScope record for MMS, EasyFund deals
            var dealscope = fundingDealRepository.GetFundingDealByFCTURN(disbursement.ReferenceNumber);
         
            // 1. ensure dealscope is not null; validate associated deal FCT Number exists.
            if (null == dealscope)
            {
                errorcodes.Add(ErrorCode.ChainDealFctRefNumDoesNotExist);
                return errorcodes;
            }

            // purchase lawyer for the second deal (associated deal)
            var associatedDeal = dealscope.tblDeals.FirstOrDefault(m => m.LawyerActingFor == LawyerActingFor.Purchaser || m.LawyerActingFor == LawyerActingFor.Both);

            //var currentDeal = dealRepository.GetDealDetails(dealId, userContext);
            var curDealWithDealScope = dealRepository.GetDealWithDealScopeDetails(dealId);


            var associatedDealRefinance =
                dealscope.tblDeals.FirstOrDefault(
                    m => m.LawyerActingFor == LawyerActingFor.Mortgagor && m.DealType == DealType.Refinance);


            // 4. if associated deal has payee type 'chain deal' and transaction type 'refinance' then validation fails. 
            if (associatedDealRefinance != null)
            {
                errorcodes.Add(ErrorCode.ChainDealRefinanceTransType);
            }
            else if (associatedDeal == null)
            {
                errorcodes.Add(ErrorCode.ChainDealNotValidPurchaser);
            }
            // 2. ensure FCT Ref # is not attached to current deal - no circular refernce.
            else if (disbursement.ReferenceNumber == curDealWithDealScope.tblDealScope.FCTRefNumber)
            {
                errorcodes.Add(ErrorCode.ChainDealCircularReference);
            }
            // 3. ensure EASYFUND is present in associated deal business model.
            else if (!associatedDeal.BusinessModel.Contains(BusinessModel.EASYFUND))
            {
                errorcodes.Add(ErrorCode.ChainDealNotEasyFund);
            }
            // 5. if current deal's vendor lawyer is also the vendor lawyer of the second deal then validation fails.
            else if (curDealWithDealScope.LawyerID != associatedDeal.LawyerID)
            {
                errorcodes.Add(ErrorCode.ChainDealNotValidPurchaser);
            }

            return errorcodes;  // return empty errorcode.
        }

        public static IEnumerable<ErrorCode> ValidateDisbursement(Disbursement disbursement, int dealId, UserContext userContext, 
            IFundingDealRepository fundingDealRepository, IDealRepository dealRepository)
        {
            var errorcodes = new List<ErrorCode>();

            // if it is a chain deal then complete validation based on chain deal.
            if (PayeeType.ChainDeal == disbursement.PayeeType)
            {
                errorcodes = ValidateChainDeal(disbursement, dealId, userContext, fundingDealRepository, dealRepository).ToList();    
            }

            return errorcodes;
            
        }

    }
}
