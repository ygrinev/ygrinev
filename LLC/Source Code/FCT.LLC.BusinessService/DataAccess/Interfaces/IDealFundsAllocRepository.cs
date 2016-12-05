using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IDealFundsAllocRepository: IRepository<tblDealFundsAllocation>
    {
        IEnumerable<DealFundsAllocation> SearchFunds(SearchFundsAllocationRequest request, out int totalRecordsCount);
        DealFundsAllocation UpdateFundsAllocation(UpdateFundsAllocationRequest request);
        void SavePayments(IDictionary<PaymentNotification, Allocation> payments);
        decimal GetTotalAllocatedFunds(int fundingDealId);
        IEnumerable<DealFundsAllocation> InsertDealFundsAllocationRange(IEnumerable<DealFundsAllocation> dealFundsAllocations);
        void UpdateStatus(IEnumerable<DealFundsAllocation> dealFundAllocations, string allocationStatus);
        IEnumerable<DealFundsAllocation> SearchReturns(string allocationStatus, string recordType, int fundingDealId);
        DealFundsAllocation GetDealFundsAllocation(int dealFundsAllocationId);
        void SetReconciledDate(int dealFundAllocationId, bool reconciled, string userId);
        DealFundsAllocationCollection GetDeposits(int dealID);
        bool IsDuplicateDeposit(string paymentReferenceNumber);
        void UpdateDealAllocationsFromNotifications(IDictionary<int, PaymentNotification> paymentNotifications);
        string GetFCTURNByItemID(int itemID);
    }
}
