using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentService.DataContracts;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class PaymentRequestProcessor : IPaymentRequestProcessor
    {
        private readonly IPaymentRequestConverter _paymentRequestConverter;
        private readonly IPaymentRequestRepository _paymentRequestRepository;
        private readonly IFundedRepository _fundedRepository;

        public PaymentRequestProcessor(IPaymentRequestConverter paymentRequestConverter,
            IPaymentRequestRepository paymentRequestRepository, IFundedRepository fundedRepository)
        {
            _paymentRequestConverter = paymentRequestConverter;
            _paymentRequestRepository = paymentRequestRepository;
            _fundedRepository = fundedRepository;
        }

        public async Task<PaymentRequestList> ProcessPaymentRequests(DisbursementCollection disbursements, FundingDeal fundingDeal,
            Payment payment, Common.DataContracts.UserContext userContext)
        {
            int feeDisbursementId = 0;
            var list = new PaymentRequestList();

            var paymentRequestsAsync = _paymentRequestConverter.ConvertToPaymentRequests(disbursements, fundingDeal,
                payment, userContext);
            var feePaymentRequests = _paymentRequestConverter.GetFeePaymentRequests(disbursements, fundingDeal,
                payment.PaymentDate, userContext);

            var feeDisbursement = disbursements.SingleOrDefault(d => d.PayeeType == EasyFundFee.FeeName);
            if (feeDisbursement != null)
            {
                feeDisbursementId = feeDisbursement.DisbursementID.GetValueOrDefault();
            }
            var paymentRequests = await paymentRequestsAsync;

            var llcPaymentRequests = SavePaymentRequests(paymentRequests, feePaymentRequests, feeDisbursementId,
                disbursements.FirstOrDefault().FundingDealID);

            foreach (var llcPaymentRequest in llcPaymentRequests)
            {
                PaymentRequest request;
                if (paymentRequests.TryGetValue(llcPaymentRequest.DisbursementID, out request))
                {
                    request.DisbursementTransactionID = llcPaymentRequest.PaymentRequestID.ToString();
                    list.Add(request);
                }
            }

            var feePaymentRequestIds =
                llcPaymentRequests.Where(p => p.DisbursementID == feeDisbursementId)
                    .Select(p => p.PaymentRequestID)
                    .ToList();
            if (feePaymentRequestIds.Count() == feePaymentRequests.Count)
            {
                for (int i = 0; i < feePaymentRequests.Count; i++)
                {
                    feePaymentRequests[i].DisbursementTransactionID = feePaymentRequestIds[i].ToString();
                }
            }

            list.AddRange(feePaymentRequests);
            return list;
        }

        internal IEnumerable<LLCPaymentRequest> SavePaymentRequests(
            IDictionary<int, PaymentRequest> paymentRequests, IEnumerable<PaymentRequest> feePaymentRequests,
            int feeDisbursementId, int fundingDealId)
        {
            var llcPayments = (from paymentRequest in paymentRequests
                let requestMessage = Serializer.XMLSerialize(paymentRequest.Value)
                select new LLCPaymentRequest()
                {
                    Message = requestMessage,
                    DisbursementID = paymentRequest.Key,
                    RequestDate = DateTime.Now
                }).ToList();

            llcPayments.AddRange(from feePaymentRequest in feePaymentRequests
                where feeDisbursementId > 0
                select new LLCPaymentRequest()
                {
                    Message = Serializer.XMLSerialize(feePaymentRequest),
                    DisbursementID = feeDisbursementId,
                    RequestDate = DateTime.Now
                });

            //update milestones as late as possible but before any other update and check concurrency
          //  milestone = _fundedRepository.UpdateDisbursedMilestone(fundingDealId);

            return _paymentRequestRepository.InsertPaymentRequestRange(llcPayments);
        }

        public PaymentRequestList ProcessPaymentRequests(IEnumerable<DealFundsAllocation> fundsToReturn,
            FundingDeal fundingDeal,
            DateTime paymentDate, Common.DataContracts.UserContext userContext)
        {
            var list = new PaymentRequestList();
            var paymentRequests = _paymentRequestConverter.ConvertToPaymentRequests(fundsToReturn, fundingDeal,
                paymentDate, userContext);

            var llcPayments = paymentRequests.Select(paymentRequest => new LLCPaymentRequest()
            {
                DealFundsAllocationID = paymentRequest.Key,
                Message = Serializer.XMLSerialize(paymentRequest.Value),
                RequestDate = DateTime.Now
            }).ToList();
            var llcPaymentRequests = _paymentRequestRepository.InsertPaymentRequestRange(llcPayments);
            foreach (var llcPaymentRequest in llcPaymentRequests)
            {
                PaymentRequest request;
                if (paymentRequests.TryGetValue(llcPaymentRequest.DealFundsAllocationID, out request))
                {
                    request.DisbursementTransactionID = llcPaymentRequest.PaymentRequestID.ToString();
                    list.Add(request);
                }
            }
            return list;
        }
    }
}
