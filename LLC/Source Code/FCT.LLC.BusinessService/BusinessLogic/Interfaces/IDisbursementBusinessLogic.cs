using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IDisbursementBusinessLogic
    {
        GetDisbursementsResponse GetDisbursements(GetDisbursementsRequest request);
        SaveDisbursementsResponse SaveDisbursements(SaveDisbursementsRequest request);
        CalculateFCTFeeResponse CalculateFctFee(CalculateFCTFeeRequest request);
        Task<DisburseFundsResponse> DisburseFunds(DisburseFundsRequest request);
        void SavePayoutComments(SavePayoutCommentsRequest request);
    }
}
