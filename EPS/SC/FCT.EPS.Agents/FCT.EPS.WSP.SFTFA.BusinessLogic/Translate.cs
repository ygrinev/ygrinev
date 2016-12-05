using FCT.EPS.DataEntities;
using FCT.EPS.WSP.ExternalResources.FinanceService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.SFTFA.BusinessLogic
{
    class Translate
    {
        internal static FCTFeeSummary tblPaymentRequest2FCTFeeSummaryRequest(tblPaymentRequest mytblPaymentRequest)
        {
            return
                new FCTFeeSummary()
                {
                    FCTRefNumber = mytblPaymentRequest.FCTReferenceNumber,
                    DisbursementTransactionID = mytblPaymentRequest.PaymentTransactionID.HasValue ? mytblPaymentRequest.PaymentTransactionID.Value : -1,
                    ProgramID = mytblPaymentRequest.tblSolutionSubscription.tblFCTAccount.tblSolution.SolutionID,
                    Service = mytblPaymentRequest.tblFCTFeeSummaryRequest.FinanceServiceCode,
                    PayeeName = mytblPaymentRequest.PayeeName,
                    BillingPartyID1 = mytblPaymentRequest.tblFCTFeeSummaryRequest.LawyerCRMReference,
                    BillingPartyID2 = mytblPaymentRequest.tblFCTFeeSummaryRequest.LawyerLIMReference,
                    ProvinceCode = mytblPaymentRequest.tblFCTFeeSummaryRequest.PropertyProvinceCode,
                    Amount = mytblPaymentRequest.tblFCTFeeSummaryRequest.BaseAmount,
                    PST = mytblPaymentRequest.tblFCTFeeSummaryRequest.PST,
                    GST = mytblPaymentRequest.tblFCTFeeSummaryRequest.GST,
                    HST = mytblPaymentRequest.tblFCTFeeSummaryRequest.HST,
                    QST = mytblPaymentRequest.tblFCTFeeSummaryRequest.QST,
                    RST = mytblPaymentRequest.tblFCTFeeSummaryRequest.RST,
                    TotalFee = mytblPaymentRequest.PaymentAmount,
                    DateEntered = mytblPaymentRequest.PaymentRequestDate,
                    ServiceBatchID = mytblPaymentRequest.tblPaymentTransaction.ServiceBatchID.HasValue ? (int)mytblPaymentRequest.tblPaymentTransaction.ServiceBatchID.Value : -1
                };
        }
    }
}
