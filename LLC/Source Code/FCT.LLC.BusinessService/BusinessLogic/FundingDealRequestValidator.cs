using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    internal static class FundingDealRequestValidator
    {
        internal static bool ValidateRequest(SaveFundingDealRequest request, out string invalidFields)
        {
            var missingFieldsList = new List<string>();
            if (request.Deal != null)
            {
                bool acceptedStatus = ValidateDealStatus(request);
                if (!acceptedStatus)
                {
                    missingFieldsList.Add("DealStatus");
                }
                else
                {
                    if (request.Deal.DealStatus == DealStatus.UserDraft)
                    {
                        bool hasbasicRequirements = ValidateBasicRequirements(request.Deal, missingFieldsList);
                        invalidFields = string.Join(",", missingFieldsList);
                        if (hasbasicRequirements)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        bool hasbasicRequirements = ValidateBasicRequirements(request.Deal, missingFieldsList);
                        bool hasVendors = request.Deal.Vendors != null;

                        bool hasPurchasers = request.Deal.Mortgagors != null;

                        bool hasPins = request.Deal.Property != null && request.Deal.Property.Pins != null;
                        if (!hasPins)
                        {
                            missingFieldsList.Add("PINs");
                        }

                        bool hasOtherlawyer;
                        //Combo deal may have EasyFund portion in draft but LLC deal active.
                        if (request.Deal.ActingFor.ToUpper() != LawyerActingFor.Both && 
                            request.Deal.ActingFor.ToUpper() != LawyerActingFor.Mortgagor && 
                            request.Deal.OtherLawyerDealStatus != DealStatus.SystemDraft)
                        {
                            hasOtherlawyer = request.Deal.OtherLawyer != null;
                            if (!hasOtherlawyer)
                            {
                                missingFieldsList.Add("Other Lawyer");
                            }
                        }
                        else
                        {
                            hasOtherlawyer = true;
                        }
                        if (hasbasicRequirements && hasOtherlawyer && hasPins && (hasPurchasers || hasVendors))
                        {
                            invalidFields = string.Join(",", missingFieldsList);
                            return true;
                        }
                    }
                }
            }
            invalidFields = string.Join(",", missingFieldsList);
            return false;
        }

        private static bool ValidateDealStatus(SaveFundingDealRequest request)
        {
            bool acceptedStatus = false;
            if (!string.IsNullOrWhiteSpace(request.Deal.DealStatus))
            {
                acceptedStatus = (request.Deal.DealStatus.ToUpper() == DealStatus.Active) ||
                                 (request.Deal.DealStatus.ToUpper() == DealStatus.CancelRequest) ||
                                 (request.Deal.DealStatus.ToUpper() == DealStatus.UserDraft);
            }
            return acceptedStatus;
        }

        private static bool ValidateBasicRequirements(FundingDeal deal, IList<string> missingFieldsList)
        {
            bool actingFor = !string.IsNullOrWhiteSpace(deal.ActingFor);
            if (!actingFor)
            {
                missingFieldsList.Add("ActingFor");
            }
            bool haslawyer = deal.Lawyer != null;
            if (!haslawyer)
            {
                missingFieldsList.Add("Lawyer");
            }
            bool hasType = !string.IsNullOrWhiteSpace(deal.DealType);
            if (!hasType)
            {
                missingFieldsList.Add("DealType");
            }
            bool hasbusinessModel = !string.IsNullOrWhiteSpace(deal.BusinessModel);
            if (!hasbusinessModel)
            {
                missingFieldsList.Add("BusinessModel");
            }
            bool hasproperty = deal.Property != null;
            if (!hasproperty)
            {
                missingFieldsList.Add("Property");
            }
            if (actingFor && hasbusinessModel && hasType && haslawyer && hasproperty)
            {
                return true;
            }
            return false;
        }
    }
}
