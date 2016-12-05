using System;
namespace FCT.EPS.WSP.DataAccess
{
    internal interface IUnitOfWork
    {
        void Dispose();
        void Save();
        //GenericRepository<FCT.EPS.FinanceService.DataEntities.tbEPSChequeRecord> TbEPSChequeRequestRepository { get; }
        //GenericRepository<FCT.EPS.FinanceService.DataEntities.tbFCTFeeSummaryRecord> TbFCTFeeSummaryRecordRepository { get; }
    }
}
