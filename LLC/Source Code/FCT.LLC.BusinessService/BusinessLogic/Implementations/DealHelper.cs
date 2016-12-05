using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using System.Collections.Generic;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public static class DealHelper
    {
        public static bool isEasyFundDeal(tblDeal deal)
        {
            return deal.BusinessModel.Contains(BusinessModel.EASYFUND);
        }
        public static bool isLLCDeal(tblDeal deal)
        {
            return deal.BusinessModel.Contains(BusinessModel.LLC);
        }
        public static bool isDealReadOnly(tblDeal deal)
        {
            return (deal.Status == DealStatus.Complete || deal.Status == DealStatus.New || deal.Status == DealStatus.Cancelled);
        }
        public static bool isDealCompleted(tblDeal deal)
        {
            return (deal.Status == DealStatus.Complete);
        }
        //public static bool isWesternProtocol(Deal deal)
        //{
        //    if (deal.DealClosingOptionId != null)
        //    {
        //        if (deal.DealClosingOptionId == (int)Constants.FinalReportClosingOption.WESTERN_PROTOCOL)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public static List<string> GetDealProducts(string DealBusinessModel)
        {
            List<string> _products = new List<string>();
            if (!string.IsNullOrEmpty(DealBusinessModel))
            {
                //Check LLC
                if (DealBusinessModel.Contains(BusinessModel.LLC))
                {
                    _products.Add(BusinessModel.LLC);
                }

                //Check MMS
                if (DealBusinessModel.Contains(BusinessModel.MMS))
                {
                    _products.Add(BusinessModel.MMS);
                }

                //Check EasyFund
                if (DealBusinessModel.Contains(BusinessModel.EASYFUND))
                {
                    _products.Add(BusinessModel.EASYFUND);
                }
            }
            return _products;
        }

        public static bool isMMSDeal(tblDeal deal)
        {
            var isMMSDeal = false;

            if (deal != null)
            {
                isMMSDeal = deal.BusinessModel.Contains(BusinessModel.MMS);
            }

            return isMMSDeal;
        }


        //public static bool IsDealFunded(int dealId)
        //{
        //    const string DEAL_FUNDED = "DEAL_FUNDED";
        //    var milestoneCodeService = new MilestoneCodeService();
        //    var code = milestoneCodeService.GetByName(DEAL_FUNDED);
        //    var milestoneService = new MilestoneService();
        //    var milestones = milestoneService.GetByDealId(SessionHelper.CurrentDealID);

        //    var dealFundedMilestone = milestones.SingleOrDefault(m => m.MilestoneCodeId == code.MilestoneCodeId);
        //    if (dealFundedMilestone != null && dealFundedMilestone.CompletedDateTime != null)
        //        return true;

        //    return false;
        //}
    }
}