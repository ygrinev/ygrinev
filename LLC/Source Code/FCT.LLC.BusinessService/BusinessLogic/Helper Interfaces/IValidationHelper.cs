using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;
using FCT.Services.LIM.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IValidationHelper
    {
        UserProfile GetActiveLawyer(int trustAccountId, List<ErrorCode> errorCodes = null);

        Task<Payment> ValidateVendorLawyer(DisbursementCollection disbursements, List<ErrorCode> errorCodes, FundingDeal deal);

        FundingDeal ValidateDealInDB(int dealId, List<ErrorCode> errorcodes, out DisbursementCollection disbursements);

        bool IsOnWeekend(DateTime easternTime);

        bool IsWithinWorkingHours(DateTime easternTime);

        bool IsSystemClosed(DateTime easternTime);

        bool IsTestUser(UserContext userContext);

        DateTime ValidatePaymentDate(UserContext userContext, List<ErrorCode> errorcodes);
    }
}
