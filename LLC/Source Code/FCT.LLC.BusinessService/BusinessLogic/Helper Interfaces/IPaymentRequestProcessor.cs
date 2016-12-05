using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public interface IPaymentRequestProcessor
    {
        Task<PaymentRequestList> ProcessPaymentRequests(DisbursementCollection disbursements, FundingDeal fundingDeal,
            Payment payment, Common.DataContracts.UserContext userContext);

        PaymentRequestList ProcessPaymentRequests(IEnumerable<DealFundsAllocation> fundsToReturn,
            FundingDeal fundingDeal,
            DateTime paymentDate, Common.DataContracts.UserContext userContext);
    }
}
