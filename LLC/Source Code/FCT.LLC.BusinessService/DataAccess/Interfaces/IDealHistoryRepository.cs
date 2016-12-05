using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess.Interfaces
{
    public interface IDealHistoryRepository:IRepository<tblDealHistory>
    {
        void CreateDealHistoryByDealHistoryEntry(int dealID, DealHistoryEntry entry, UserContext user, bool showOnLender);
        void CreateDealHistory(int dealID, UserContext user, string activity, string otherLawyerName=null);
        void CreateDealHistory(int dealID, string activity, UserContext user, decimal? amount );
        void CreateDealHistory(int dealID, string activity, UserContext user, DisbursementHistory disbursementHistory);
        void CreateDealHistory(int dealID, string status);
        void CreateDealHistory(int dealID, string entryKey, UserContext user);
        void CreateCancelDealHistory(int dealID, string entryKey, UserContext user);
        void CreateDealHistoryByStatus(int dealID, string fromStatus, string toStatus, string historyFor, UserContext user);
        void CreateDealChangeHistories(IEnumerable<UserHistory> userHistories, UserContext user);
        void CreateLLCDealHistory(int dealID, string activity, UserContext user, string addedOrRemovedValue, int statusReasonId = 0);
        void CreateDealHistories(int dealID, string resourceSet, IEnumerable<IDictionary<string, Variance>> dataChanges, UserContext user);
        void SyncDealHistories(int sourceDealId, int targetDealId);
        void CreateDealHistories(IEnumerable<UserHistory> userHistories, UserContext user);
        DealHistoryEntry GetDealHistoryEntry(string resourceSet, string resourceKey);
    }
}
