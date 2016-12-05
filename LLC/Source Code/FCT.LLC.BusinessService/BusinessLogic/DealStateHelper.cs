using System.Collections.Generic;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public static class DealStateHelper
    {
        public static bool isEasyFundDeal(string businessModel)
        {
            return (businessModel??"").Contains(BusinessModel.EASYFUND);
        }
        public static bool isLLCDeal(string businessModel)
        {
            return (businessModel??"").Contains(BusinessModel.LLC);
        }
        public static bool isDealReadOnly(string dealStatus)
        {
            return (dealStatus == DealStatus.Complete || dealStatus == DealStatus.New || dealStatus == DealStatus.Cancelled);
        }
        public static bool isDealCompleted(string dealStatus)
        {
            return (dealStatus == DealStatus.Complete);
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

        public static List<string> GetDealProducts(string businessModel)
        {
            List<string> _products = new List<string>();
            if (!string.IsNullOrEmpty(businessModel))
            {
                //Check LLC
                if (businessModel.Contains(BusinessModel.LLC))
                {
                    _products.Add(BusinessModel.LLC);
                }

                //Check MMS
                if (businessModel.Contains(BusinessModel.MMS))
                {
                    _products.Add(BusinessModel.MMS);
                }

                //Check EasyFund
                if (businessModel.Contains(BusinessModel.EASYFUND))
                {
                    _products.Add(BusinessModel.EASYFUND);
                }
            }
            return _products;
        }

        public static bool isMMSDeal(string businessModel)
        {
            return (businessModel??"").Contains(BusinessModel.MMS);
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
