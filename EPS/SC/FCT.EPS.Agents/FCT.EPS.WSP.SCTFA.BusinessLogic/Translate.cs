using FCT.EPS.DataEntities;
using FCT.EPS.WSP.ExternalResources.FinanceService;
using FCT.EPS.WSP.Resources;
using FCT.EPS.WSP.SCTFA.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCT.EPS.WSP.SCTFA.BusinessLogic
{
    class Translate
    {
        internal static ChequeRequest tblPaymentRequest2ChequeRequest(tblPaymentRequest mytblPaymentRequest)
        {
            IList<string> AddressLines = Utils.SplitNameField(mytblPaymentRequest.DebtorName,40);

            return new ChequeRequest()
            {
                FCTRefNum = mytblPaymentRequest.FCTReferenceNumber,
                ProgramID = mytblPaymentRequest.tblSolutionSubscription.tblFCTAccount.tblSolution.SolutionID,
                DisbursementTransactionID = mytblPaymentRequest.PaymentTransactionID.HasValue ?  mytblPaymentRequest.PaymentTransactionID.Value : -1 ,
                ServiceDescription = AgentConstants.Misc.SERVICE_DESCRIPTION,
                StatusID = AgentConstants.Misc.DEFAULT_CHEQUE_STATUS,
                PayeeName = mytblPaymentRequest.PayeeName,
                AddressLine1 = mytblPaymentRequest.FCTURNShort,
                AddressLine5 = mytblPaymentRequest.PayeeReferenceNumber,
                Amount = mytblPaymentRequest.PaymentAmount,
                DateEntered = mytblPaymentRequest.PaymentRequestDate,
                AddressLine2 = AddressLines.Count > 0 ? AddressLines[0].ToString() : string.Empty,
                AddressLine3 = AddressLines.Count > 1 ? AddressLines[1].ToString() : string.Empty
            };
        }
    }
}
