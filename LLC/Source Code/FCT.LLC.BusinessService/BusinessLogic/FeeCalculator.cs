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
    public class FeeCalculator : IFeeCalculator
    {
        private readonly IFeeRepository _feeRepository;
        private readonly IDisbursementRepository _disbursementRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IFundedRepository _fundedRepository;
        private readonly IDisbursementSummaryRepository _disbursementSummaryRepository;

        public FeeCalculator(IFeeRepository feeRepository, IDisbursementRepository disbursementRepository,
            IPropertyRepository propertyRepository, IFundedRepository fundedRepository,
            IDisbursementSummaryRepository disbursementSummaryRepository)
        {
            _feeRepository = feeRepository;
            _disbursementRepository = disbursementRepository;
            _propertyRepository = propertyRepository;
            _fundedRepository = fundedRepository;
            _disbursementSummaryRepository = disbursementSummaryRepository;
        }

        public Fee RecalculateFee(Disbursement disbursement, int disbursementCount, int fundedDealId)
        {
            Fee fee = null;
            var property = _propertyRepository.GetPropertyByScope(fundedDealId);
            if (property != null)
            {
                disbursement.Province = property.Province;
            }

            switch (disbursement.FCTFeeSplit)
            {
                case FeeDistribution.VendorLawyer:

                    if (disbursement.PurchaserFee != null)
                    {
                        fee = _feeRepository.CalculateFee(disbursementCount, disbursement.Province, false,
                            disbursement.PurchaserFee.FeeID.GetValueOrDefault());
                    }
                    else if (disbursement.VendorFee != null)
                    {
                        fee = _feeRepository.CalculateFee(disbursementCount, disbursement.Province, false,
                            disbursement.VendorFee.FeeID.GetValueOrDefault());
                    }
                    break;

                case FeeDistribution.PurchaserLawyer:

                    if (disbursement.VendorFee != null)
                    {
                        fee = _feeRepository.CalculateFee(disbursementCount, disbursement.Province, false,
                            disbursement.VendorFee.FeeID.GetValueOrDefault());
                    }
                    else if (disbursement.PurchaserFee != null)
                    {
                        fee = _feeRepository.CalculateFee(disbursementCount, disbursement.Province, false,
                            disbursement.PurchaserFee.FeeID.GetValueOrDefault());
                    }
                    break;
                case FeeDistribution.SplitEqually:
                    if (disbursement.PurchaserFee != null)
                    {
                        fee = _feeRepository.CalculateFee(disbursementCount, disbursement.Province, true,
                            disbursement.PurchaserFee.FeeID.GetValueOrDefault());
                    }
                    else if (disbursement.VendorFee != null)
                    {
                        fee = _feeRepository.CalculateFee(disbursementCount, disbursement.Province, true,
                            disbursement.VendorFee.FeeID.GetValueOrDefault());
                    }
                    break;
            }

            return fee;
        }

        public void ReCalculateFee(string changedProvince, int dealId)
        {
            var milestones = _fundedRepository.GetMilestonesByDeal(dealId);
            if (milestones != null &&
                (milestones.Milestone.Disbursed == null || milestones.Milestone.Disbursed <= DateTime.MinValue))
            {
                var disbursements = _disbursementRepository.GetDisbursements(dealId);

                Disbursement vendorDisbursement = null;
                Disbursement feeDisbursement = null;
                Fee fee = null;
                decimal currentDepositRequired = 0;
                foreach (var disbursement in disbursements)
                {
                    if (disbursement.PayeeType == EasyFundFee.FeeName)
                    {
                        feeDisbursement = disbursement;
                    }
                    else if (disbursement.PayeeType == FeeDistribution.VendorLawyer)
                    {
                        vendorDisbursement = disbursement;
                        currentDepositRequired = disbursement.Amount + currentDepositRequired;
                    }
                    else
                    {
                        currentDepositRequired = disbursement.Amount + currentDepositRequired;
                    }
                }
                if (feeDisbursement != null)
                {
                    int disbursementsToBeCharged = disbursements.Count - 1;

                    fee = UpdateFeeDisbursement(changedProvince, feeDisbursement, disbursementsToBeCharged);
                }
                if (vendorDisbursement != null && fee != null)
                {
                    VendorLawyerHelper.AssignVendorLawyerDisbursedAmount(fee, feeDisbursement.FCTFeeSplit,
                        vendorDisbursement);

                    _disbursementRepository.UpdateDisbursement(vendorDisbursement);
                }


                if (fee != null && feeDisbursement.FCTFeeSplit != FeeDistribution.VendorLawyer)
                {
                    var feeWithTaxes = fee.Amount + fee.GST + fee.HST + fee.QST;
                    currentDepositRequired = currentDepositRequired + feeWithTaxes;
                }

                var previousDepositRequired = _disbursementSummaryRepository.GetDepositRequired(milestones.FundingDealId);
                if (previousDepositRequired != currentDepositRequired)
                {
                    _disbursementSummaryRepository.UpdateDisbursementSummary(milestones.FundingDealId,
                        currentDepositRequired);
                }
            }
        }

        private Fee UpdateFeeDisbursement(string changedProvince, Disbursement feeDisbursement,
            int disbursementsToBeCharged)
        {
            Fee fee = null;
            switch (feeDisbursement.FCTFeeSplit)
            {
                case FeeDistribution.VendorLawyer:
                    if (feeDisbursement.VendorFee != null)
                    {
                        fee = _feeRepository.UpdateFee(disbursementsToBeCharged, changedProvince, false,
                            feeDisbursement.VendorFee.FeeID.GetValueOrDefault());
                    }
                    break;
                case FeeDistribution.PurchaserLawyer:
                    if (feeDisbursement.PurchaserFee != null)
                    {
                        fee = _feeRepository.UpdateFee(disbursementsToBeCharged, changedProvince, false,
                            feeDisbursement.PurchaserFee.FeeID.GetValueOrDefault());
                    }
                    break;
                case FeeDistribution.SplitEqually:
                    if (feeDisbursement.PurchaserFee != null)
                    {
                        fee = _feeRepository.UpdateFee(disbursementsToBeCharged, changedProvince, true,
                            feeDisbursement.PurchaserFee.FeeID.GetValueOrDefault());
                    }
                    break;
            }
            feeDisbursement.Province = changedProvince;
            AssignFeeWithTaxes(fee, feeDisbursement);

            _disbursementRepository.UpdateFeeDisbursement(feeDisbursement);
            return fee;
        }

        public void ReAssignFee(int dealId)
        {
            var milestones = _fundedRepository.GetMilestonesByDeal(dealId);
            if (milestones != null &&
                (milestones.Milestone.Disbursed == null || milestones.Milestone.Disbursed <= DateTime.MinValue))
            {
                var disbursements = _disbursementRepository.GetDisbursements(dealId);

                decimal currentDepositRequired = 0;
                int fundingDealID = 0;

                foreach (var disbursement in disbursements)
                {
                    if (disbursement.PayeeType == EasyFundFee.FeeName)
                    {
                        Fee fee = null;
                        switch (disbursement.FCTFeeSplit)
                        {
                            case FeeDistribution.VendorLawyer:
                                if (disbursement.VendorFee != null)
                                {
                                    fee = disbursement.VendorFee;
                                    disbursement.PurchaserFee = fee;
                                    disbursement.VendorFee = null;
                                }
                                break;

                            case FeeDistribution.SplitEqually:
                                if (disbursement.PurchaserFee != null && disbursement.VendorFee != null)
                                {
                                    fee = _feeRepository.UpdateFee(disbursements.Count - 1, disbursement.Province,
                                        false, disbursement.PurchaserFee.FeeID.GetValueOrDefault());
                                    disbursement.PurchaserFee = fee;
                                    disbursement.VendorFee = null;
                                }
                                break;
                        }

                        disbursement.FCTFeeSplit = FeeDistribution.PurchaserLawyer;
                        disbursement.PayeeName = UserType.FCTAdmin;
                        disbursement.PaymentMethod = RecordType.FCTFee;
                        decimal sum = AssignFeeWithTaxes(fee, disbursement);

                        currentDepositRequired = currentDepositRequired + sum;
                        fundingDealID = disbursement.FundingDealID;
                        _disbursementRepository.UpdateFeeDisbursement(disbursement);
                    }
                    else
                    {
                        currentDepositRequired = currentDepositRequired + disbursement.Amount;
                    }
                }
                if (fundingDealID > 0)
                {
                    var previousDepositRequired = _disbursementSummaryRepository.GetDepositRequired(fundingDealID);
                    if (previousDepositRequired != currentDepositRequired)
                    {
                        _disbursementSummaryRepository.UpdateDisbursementSummary(fundingDealID, currentDepositRequired);
                    }
                }
            }
        }

        internal static decimal AssignFeeWithTaxes(Fee fee, Disbursement disbursement)
        {
            if (fee != null)
            {
                var feeWithTaxes = fee.Amount + fee.GST + fee.HST + fee.QST;
                disbursement.Amount = disbursement.FCTFeeSplit == FeeDistribution.SplitEqually
                    ? Math.Round(feeWithTaxes * 2, 2, MidpointRounding.AwayFromZero)
                    : Math.Round(feeWithTaxes, 2, MidpointRounding.AwayFromZero);
                return feeWithTaxes;
            }
            return 0;
        }

        public DisbursementFee CalculateDefaultFees(int disbursementcount, string province, string actingFor)
        {
            bool splitEqually = actingFor != LawyerActingFor.Mortgagor && actingFor != LawyerActingFor.Both;
            var disbursementFee = _feeRepository.InsertFees(disbursementcount, province,
               splitEqually);

            return disbursementFee;
        }

        public DisbursementFee RecalculateAndSaveFees(Disbursement disbursement, int disbursementCount,
            int fundedDealId)
        {
            Fee vendorFee;
            Fee purchaserFee;
            var property = _propertyRepository.GetPropertyByScope(fundedDealId);
            if (property != null)
            {
                disbursement.Province = property.Province;
            }
            var disbursementFee = new DisbursementFee();

            var dbDisbursement = _disbursementRepository.GetDisbursement(disbursement.DisbursementID.GetValueOrDefault());
            switch (disbursement.FCTFeeSplit)
            {
                case FeeDistribution.SplitEqually:
                    vendorFee = CalculateVendorFee(disbursement, disbursementCount, dbDisbursement, true);
                    purchaserFee = CalculatePurchaserFee(disbursement, disbursementCount, dbDisbursement, true);
                    disbursementFee.PurchaserFee = purchaserFee;
                    disbursementFee.VendorFee = vendorFee;
                    break;

                case FeeDistribution.VendorLawyer:
                    vendorFee = CalculateVendorFee(disbursement, disbursementCount, dbDisbursement, false);
                    disbursementFee.VendorFee = vendorFee;
                    disbursementFee.PurchaserFee = null;
                    break;

                case FeeDistribution.PurchaserLawyer:
                    purchaserFee = CalculatePurchaserFee(disbursement, disbursementCount, dbDisbursement, false);
                    disbursementFee.PurchaserFee = purchaserFee;
                    disbursementFee.VendorFee = null;
                    break;
            }
            return disbursementFee;
        }

        private Fee CalculatePurchaserFee(Disbursement disbursement, int disbursementCount, Disbursement dbDisbursement, bool splitEqually)
        {
            Fee purchaserFee;
            if (dbDisbursement.PurchaserFee != null && dbDisbursement.PurchaserFee.FeeID.HasValue)
            {
                purchaserFee = _feeRepository.UpdateFee(disbursementCount, disbursement.Province, splitEqually,
                    dbDisbursement.PurchaserFee.FeeID.GetValueOrDefault());
            }
            else
            {
                purchaserFee = _feeRepository.InsertFee(disbursementCount, disbursement.Province, splitEqually);
            }
            return purchaserFee;
        }

        private Fee CalculateVendorFee(Disbursement disbursement, int disbursementCount, Disbursement dbDisbursement, bool splitEqually)
        {
            Fee vendorFee;
            if (dbDisbursement.VendorFee != null && dbDisbursement.VendorFee.FeeID.HasValue)
            {
                vendorFee = _feeRepository.UpdateFee(disbursementCount, disbursement.Province, splitEqually,
                    dbDisbursement.VendorFee.FeeID.GetValueOrDefault());
            }
            else
            {
                vendorFee = _feeRepository.InsertFee(disbursementCount, disbursement.Province, splitEqually);
            }
            return vendorFee;
        }
    }
}
