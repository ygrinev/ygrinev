using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class MilestoneUpdater:IMilestoneUpdater
    {
        private readonly IFundedRepository _fundedRepository;
        private readonly IDealFundsAllocRepository _dealFundsAllocRepository;
        private readonly IDisbursementSummaryRepository _disbursementSummaryRepository;
        public MilestoneUpdater(IFundedRepository fundedRepository, IDisbursementSummaryRepository disbursementSummaryRepository, IDealFundsAllocRepository dealFundsAllocRepository)
        {
            _fundedRepository = fundedRepository;
            _disbursementSummaryRepository = disbursementSummaryRepository;
            _dealFundsAllocRepository = dealFundsAllocRepository;
        }

        public MilestoneUpdated UpdateMilestones(int dealId, string lawyerActingFor)
        {
            var fundedDeal = _fundedRepository.GetMilestonesByDeal(dealId);

            if (fundedDeal.Milestone.Disbursed > DateTime.MinValue)
            {
                return new MilestoneUpdated(); 
            }

            if (fundedDeal.Milestone.SignedByPurchaser > DateTime.MinValue &&
                fundedDeal.Milestone.SignedByVendor > DateTime.MinValue)
            {
                fundedDeal.Milestone.SignedByPurchaser = null;
                fundedDeal.Milestone.SignedByVendor = null;
                fundedDeal.Milestone.SignedByPurchaserName = null;
                fundedDeal.Milestone.SignedByVendorName = null;

                UpdateMilestoneDepositReceived(fundedDeal);

                return new MilestoneUpdated
                {
                    ActiveUserSignRemoved = true,
                    PassiveUserSignRemoved = true
                };
            }
            if (fundedDeal.Milestone.SignedByPurchaser > DateTime.MinValue &&
                (fundedDeal.Milestone.SignedByVendor <= DateTime.MinValue ||
                fundedDeal.Milestone.SignedByVendor == null))
            {
                fundedDeal.Milestone.SignedByPurchaser = null;
                fundedDeal.Milestone.SignedByPurchaserName = null;

                UpdateMilestoneDepositReceived(fundedDeal);

                if (lawyerActingFor == LawyerActingFor.Vendor)
                {
                    return new MilestoneUpdated
                    {
                        PassiveUserSignRemoved = true,
                    };
                }
                return new MilestoneUpdated
                {
                    ActiveUserSignRemoved = true,
                };
            }
            if (fundedDeal.Milestone.SignedByVendor > DateTime.MinValue &&
                (fundedDeal.Milestone.SignedByPurchaser <= DateTime.MinValue ||
                 fundedDeal.Milestone.SignedByPurchaser == null))
            {
                fundedDeal.Milestone.SignedByVendor = null;
                fundedDeal.Milestone.SignedByVendorName = null;

                UpdateMilestoneDepositReceived(fundedDeal);

                if (lawyerActingFor == LawyerActingFor.Purchaser)
                {
                    return new MilestoneUpdated
                    {
                        PassiveUserSignRemoved = true,
                    };
                }
                return new MilestoneUpdated
                {
                    ActiveUserSignRemoved = true,
                };
            }

            UpdateMilestoneDepositReceived(fundedDeal);
            return new MilestoneUpdated();

        }

        public void UpdateMilestoneDepositReceived(FundedDeal milestones)
        {
            var depositRequired = _disbursementSummaryRepository.GetDepositRequired(milestones.FundingDealId);
            var depositReceived = _dealFundsAllocRepository.GetTotalAllocatedFunds(milestones.FundingDealId);

            if (depositReceived == depositRequired && depositReceived>0)
            {
                milestones.Milestone.Funded = DateTime.Now;
            }
            else
            {
                milestones.Milestone.Funded = null;
            }
            _fundedRepository.UpdateFundedDeal(milestones);
        }

    }
}
