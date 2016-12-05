using System.Collections.Generic;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IDisbursementRepository:IRepository<tblDisbursement>
    {
        DisbursementCollection SaveDisbursements(SaveDisbursementsRequest request, int fundingDealId = 0, DisbursementFee fee = null);
        DisbursementCollection GetDisbursements(int dealId);
        Disbursement GetDisbursement(int disbursementId);
        DisbursementCollection GetDisbursementsByType(string payeeType, int fundingDealId);
        GetPayoutLetterDateResponse GetPayoutLetterDate(GetPayoutLetterDateRequest request);
        void UpdateDisbursementsFromNotifications(IDictionary<int, PaymentNotification> paymentNotifications);
        void SetReconciliationData(int itemID, string userID, bool reconciled);
        void UpdateFeeDisbursement(Disbursement disbursement);
        void UpdateDisbursement(Disbursement disbursement);
        string GetFCTURNByItemID(int ItemID);
        bool IsDisbursementDocumentGenerated(int dealDocumentTypeId);
    }
}
