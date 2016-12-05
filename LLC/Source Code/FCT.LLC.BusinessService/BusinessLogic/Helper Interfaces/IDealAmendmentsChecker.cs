using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IDealAmendmentsChecker
    {
        bool CheckAmendments(FundingDeal oldFundingDeal, FundingDeal newFundingDeal, UserContext user);

        bool HasVendorAmendments(FundingDeal oldFundingDeal, FundingDeal newFundingDeal,
            List<UserHistory> histories, int dealId, bool hasOtherDeal, int otherDealId);

        bool CheckLLCAmendments(FundingDeal targetFundingDeal, FundingDeal sourceFundingDeal,
            UserContext user);

        List<UserHistory> GetDealHistoriesForAmendments(FundingDeal oldFundingDeal, FundingDeal newFundingDeal, out bool hasAmendments);
    }
}
