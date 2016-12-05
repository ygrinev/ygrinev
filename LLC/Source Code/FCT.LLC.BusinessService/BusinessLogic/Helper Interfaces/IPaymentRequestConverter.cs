using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IPaymentRequestConverter
    {
        Task<IDictionary<int, PaymentRequest>> ConvertToPaymentRequests(DisbursementCollection disbursements,
            FundingDeal fundingDeal,
            Payment payment, Common.DataContracts.UserContext userContext);

        IDictionary<int, PaymentRequest> ConvertToPaymentRequests(IEnumerable<DealFundsAllocation> dealFundsAllocations,
            FundingDeal fundingDeal, DateTime paymentDate, Common.DataContracts.UserContext userContext);

        IList<PaymentRequest> GetFeePaymentRequests(DisbursementCollection disbursements,
            FundingDeal fundingDeal, DateTime paymentDate, Common.DataContracts.UserContext userContext);

    }
}
