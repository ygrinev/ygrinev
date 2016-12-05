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
    public interface IDisbursementSummaryRepository:IRepository<tblDisbursementSummary>
    {
        decimal GetDepositRequired(int fundingDealId);
        DisbursementSummary UpdateDisbursementSummary(DisbursementSummary disbursementSummary, int fundingDealId, decimal amount);
        void UpdateDisbursementSummary(DisbursementSummary disbursementSummary);
        DisbursementSummary InsertDisbursementSummary(int fundingDealId, decimal amount);
        DisbursementSummary GetDisbursementSummary(int fundingDealId);
        byte[] GetDisbursementSummaryVersion(int fundingDealId);
        void UpdateDisbursementSummary(int fundingDealId, decimal amount);
    }
}
