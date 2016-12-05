using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;

namespace FCT.LLC.BusinessService.DataAccess.Mappers
{
    public sealed class FundsAllocationMapper:IEntityMapper<tblDealFundsAllocation,DealFundsAllocation>
    {
        private readonly IEntityMapper<tblFee, Fee> _feeMapper; 
        public FundsAllocationMapper(IEntityMapper<tblFee, Fee> feeMapper)
        {
            _feeMapper = feeMapper;
        }
        public DealFundsAllocation MapToData(tblDealFundsAllocation tEntity, object parameters = null)
        {
            if (tEntity != null)
            {
                var data = new DealFundsAllocation()
                {
                    AllocationStatus = tEntity.AllocationStatus,
                    DealFundsAllocationID = tEntity.DealFundsAllocationID,
                    Amount = tEntity.Amount,
                    DepositDate = tEntity.DepositDate,
                    LawyerID = tEntity.LawyerID,
                    WireDepositDetails = tEntity.WireDepositDetails,
                    ReferenceNumber = tEntity.ReferenceNumber,
                    AccountNumber = tEntity.AccountNumber,
                    BankNumber = tEntity.BankNumber,
                    BranchNumber = tEntity.BranchNumber,
                    FundingDealID = tEntity.FundingDealID,
                    Reconciled = tEntity.Reconciled,
                    RecordType = tEntity.RecordType,
                    TrustAccountID = tEntity.TrustAccountID,
                    ReconciledBy = tEntity.ReconciledBy
                };
                if (tEntity.Fee != null)
                {
                    data.Fee = _feeMapper.MapToData(tEntity.Fee);
                }
                return data;
            }
            return null;
        }

        public tblDealFundsAllocation MapToEntity(DealFundsAllocation tData)
        {
            if (tData != null)
            {
                var entity = new tblDealFundsAllocation()
                {
                    AllocationStatus = tData.AllocationStatus,
                    DealFundsAllocationID = tData.DealFundsAllocationID.GetValueOrDefault(),
                    Amount = tData.Amount,
                    DepositDate = tData.DepositDate.GetValueOrDefault(),
                    WireDepositDetails = tData.WireDepositDetails,
                    LawyerID = tData.LawyerID,
                    BankNumber = tData.BankNumber,
                    BranchNumber = tData.BranchNumber,
                    AccountNumber = tData.AccountNumber,
                    TrustAccountID = tData.TrustAccountID,
                    ReferenceNumber = tData.ReferenceNumber,
                    RecordType = tData.RecordType,
                    FundingDealID = tData.FundingDealID,
                    Reconciled = tData.Reconciled,
                    ReconciledBy = tData.ReconciledBy
                };
                if (tData.Fee != null)
                {
                    entity.FeeID = tData.Fee.FeeID;
                    entity.Fee = _feeMapper.MapToEntity(tData.Fee);
                }
                return entity;
            }
            return null;
        }
    }
}
