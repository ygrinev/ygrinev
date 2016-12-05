using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DealFundsAllocRepository:Repository<tblDealFundsAllocation>, IDealFundsAllocRepository
    {
        private readonly IEntityMapper<tblDealFundsAllocation, DealFundsAllocation> _fundsmapper;
        private readonly IEntityMapper<tblDealFundsAllocation, PaymentNotification> _paymentMapper; 
        private readonly EFBusinessContext _context;

        public DealFundsAllocRepository(EFBusinessContext context,
            IEntityMapper<tblDealFundsAllocation, DealFundsAllocation> fundsMapper,
            IEntityMapper<tblDealFundsAllocation, PaymentNotification> paymentMapper) : base(context)
        {
            _fundsmapper = fundsMapper;
            _paymentMapper = paymentMapper;
            _context = context;

        }

        public bool IsDuplicateDeposit(string paymentReferenceNumber)
        {
            var duplicateDeposits =
                _context.tblDealFundsAllocations.AsNoTracking()
                    .Where(g => g.ReferenceNumber == paymentReferenceNumber && g.RecordType == RecordType.Deposit);
            if (duplicateDeposits.Any())
            {
                return true;
            }
            return false;
        }

        public string GetFCTURNByItemID(int itemID)
        {
            string FCTURN = string.Empty;
            try
            {
                var tblDealFundsAllocation = _context.tblDealFundsAllocations.AsNoTracking().FirstOrDefault(d => d.DealFundsAllocationID == itemID);
                if (tblDealFundsAllocation != null)
                {
                    int? fundingDealID = tblDealFundsAllocation.FundingDealID;
                    if (fundingDealID.HasValue)
                    {
                        var tblFundingDeal = _context.tblFundingDeals.AsNoTracking().FirstOrDefault(f => f.FundingDealID == fundingDealID.Value);
                        if (tblFundingDeal != null)
                        {
                            int dealScopeID = tblFundingDeal.DealScopeID;
                            //FCTURN = _context.tblDealScopes.AsNoTracking().FirstOrDefault(d => d.DealScopeID == dealScopeID).ShortFCTRefNumber;
                            FCTURN = _context.tblDealScopes.AsNoTracking().FirstOrDefault(d => d.DealScopeID == dealScopeID).FCTRefNumber;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                FCTURN = "ERROR RETRIEVING FCT Ref Number Stack Trace--> " + e.ToString();
            }
            return FCTURN;
        }
        public void UpdateDealAllocationsFromNotifications(IDictionary<int, PaymentNotification> notificationDict)
        {
            try
            {
                var dealAllocationMap = (from p in _context.tblPaymentRequests
                                         where notificationDict.Keys.Contains(p.PaymentRequestID) & p.DealFundsAllocationID != null
                                         select p).ToDictionary(x => x.PaymentRequestID, y => y.DealFundsAllocationID);

                var duplicates =
                    dealAllocationMap.GroupBy(x => x.Value).Where(group => group.Count() > 1).Select(group => group.Key);

                var dealAllocationsToUpdate =
                    (from item in dealAllocationMap where !duplicates.Contains(item.Value) select item).ToDictionary(
                        x => x.Value, x => x.Key);

                var dealAllocations = from d in _context.tblDealFundsAllocations
                                      where dealAllocationsToUpdate.Keys.Contains(d.DealFundsAllocationID)
                                      select d;
                foreach (var dealAllocation in dealAllocations)
                {
                    int paymentRequestId;
                    if (dealAllocationsToUpdate.TryGetValue(dealAllocation.DealFundsAllocationID, out paymentRequestId))
                    {
                        var notification = notificationDict[paymentRequestId];
                        dealAllocation.ReferenceNumber = notification.PaymentReferenceNumber;
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException() { BaseException = ex };
            }
        }

        public decimal GetTotalAllocatedFunds(int fundingDealId)
        {
            var depositAmounts =
                _context.tblDealFundsAllocations.AsNoTracking()
                    .Where(
                        d =>
                            d.FundingDealID == fundingDealId && d.RecordType == RecordType.Deposit &&
                            d.AllocationStatus == AllocationStatus.Allocated)
                    .Select(o => o.Amount);
            var totalDeposit = Enumerable.Aggregate<decimal, decimal>(depositAmounts, 0,
                (current, depositAmount) => current + depositAmount);

            var returnAmounts =
                _context.tblDealFundsAllocations.AsNoTracking()
                    .Where(
                        d =>
                            d.FundingDealID == fundingDealId &&
                            (d.RecordType == RecordType.Return || d.RecordType == RecordType.FCTFee) &&
                            d.AllocationStatus == AllocationStatus.Disbursed)
                    .Select(o => o.Amount);
            var totalReturn = Enumerable.Aggregate<decimal, decimal>(returnAmounts, 0,
                (current, returnAmount) => current + returnAmount);
            var result = totalDeposit - totalReturn;
            return result;
        }

        public DealFundsAllocationCollection GetDeposits(int dealID)
        {
            var coll = new DealFundsAllocationCollection();
            var deal = _context.tblDeals.AsNoTracking().FirstOrDefault(d => d.DealID == dealID);
            if (deal != null)
            {
                if (deal.DealScopeID.HasValue)
                {
                    var fundingDeal =
                        _context.tblFundingDeals.AsNoTracking().SingleOrDefault(fd => fd.DealScopeID == deal.DealScopeID.Value);
                    if (fundingDeal != null)
                    {
                        List<tblDealFundsAllocation> dealFundsAllocations =
                            _context.tblDealFundsAllocations.AsNoTracking().Where(
                                df =>
                                    df.FundingDealID == fundingDeal.FundingDealID &&
                                    (df.RecordType == RecordType.Deposit || df.RecordType == RecordType.Return) &&
                                    (df.AllocationStatus == AllocationStatus.Disbursed ||
                                     df.AllocationStatus == AllocationStatus.PendingAckowledgement ||
                                     df.AllocationStatus == AllocationStatus.Allocated))
                                .ToList();
                        coll.AddRange(dealFundsAllocations.Select(dealFundAlloc => _fundsmapper.MapToData(dealFundAlloc)));
                    }
                }
            }
            return coll;
        }

        public IEnumerable<DealFundsAllocation> InsertDealFundsAllocationRange(IEnumerable<DealFundsAllocation> dealFundsAllocations)
        {
            try
            {
                var entities = dealFundsAllocations.Select(v => _fundsmapper.MapToEntity(v)).ToList();
                foreach (var tblDealFundsAllocation in entities)
                {
                    tblDealFundsAllocation.NotificationTimeStamp = DateTime.Now;
                    tblDealFundsAllocation.DepositDate = DateTime.Now;
                }
                InsertRange(entities);
                _context.SaveChanges();
                return entities.Select(e => _fundsmapper.MapToData(e));
            }
            catch (DbEntityValidationException dbEx)
            {
                TraceExceptionInDebugMode(dbEx);
            }
            return null;
        }

        public void UpdateStatus(IEnumerable<DealFundsAllocation> dealFundAllocations, string allocationStatus)
        {
            try
            {
                var ids = dealFundAllocations.Select(r => r.DealFundsAllocationID.GetValueOrDefault());
                var entities = (from d in _context.tblDealFundsAllocations where ids.Contains(d.DealFundsAllocationID) select d);
                foreach (var entity in entities)
                {
                    entity.AllocationStatus = allocationStatus;
                }
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                if (e is DbUpdateConcurrencyException || e is OptimisticConcurrencyException ||
                    e is DBConcurrencyException)
                {
                    throw new ValidationException(new List<ErrorCode>(){ErrorCode.ConcurrencyCheckFailed});
                }
                throw e;
            }
          
        }

        public IEnumerable<DealFundsAllocation> SearchReturns(string allocationStatus, string recordType,
            int fundingDealId)
        {
            var records = _context.tblDealFundsAllocations.AsNoTracking().Include(r => r.Fee)
                .Where(
                    f =>
                        f.FundingDealID == fundingDealId && f.AllocationStatus == allocationStatus &&
                        (f.RecordType == recordType || f.RecordType == RecordType.FCTFee)).AsEnumerable();
            return records.Select(tblDealFundsAllocation => _fundsmapper.MapToData(tblDealFundsAllocation)).ToList();
        }

        public DealFundsAllocation GetDealFundsAllocation(int dealFundsAllocationId)
        {
            var dealFundsAllocation = GetAll().SingleOrDefault(d => d.DealFundsAllocationID == dealFundsAllocationId);
            return _fundsmapper.MapToData(dealFundsAllocation);

        }

        public void SetReconciledDate(int dealFundsAllocationId, bool reconciled, string userID)
        {
            tblDealFundsAllocation dfAlloc = _context.tblDealFundsAllocations.Single(d=>d.DealFundsAllocationID == dealFundsAllocationId);
            if (reconciled ^ dfAlloc.Reconciled.HasValue) // there is the change
            {
                if (dfAlloc.Reconciled.HasValue)
                    dfAlloc.Reconciled = null;
                else
                    dfAlloc.Reconciled = DateTime.Now;

                dfAlloc.ReconciledBy = userID;
            }
            _context.SaveChanges();
            

        }

        public DealFundsAllocation UpdateFundsAllocation(UpdateFundsAllocationRequest request)
        {
            tblDealFundsAllocation dfAlloc =
                GetAll().Single(x => x.DealFundsAllocationID == request.DealFundsAllocationID);

            _context.Entry(dfAlloc).Property(d => d.FundingDealID).CurrentValue = request.FundingDealID.HasValue
                ? request.FundingDealID
                : null;
            _context.Entry(dfAlloc).Property(d => d.LawyerID).CurrentValue = request.LawyerID.HasValue ? request.LawyerID : null;

            _context.Entry(dfAlloc).Property(d => d.AllocationStatus).CurrentValue = request.AllocationStatus;

            Update(dfAlloc);

            if (_context.SaveChanges() >= 1)
                return _fundsmapper.MapToData(dfAlloc);
            return null;
        }

        public IEnumerable<DealFundsAllocation> SearchFunds(SearchFundsAllocationRequest request,
            out int totalRecordsCount)
        {
            int startIndex = (request.PageIndex - 1)*request.PageSize + 1;
            int endIndex = (startIndex + request.PageSize) - 1;

            var sb = new StringBuilder();

            var specs = (from orderBySpec in request.OrderBySpecifications
                let column = orderBySpec.OrderByColumn.ToString()
                let direction = orderBySpec.OrderByDirection.ToString()
                select string.Format("{0} {1}", column, direction)).ToList();

            if (specs.Count > 0)
            {
                sb.Append("ORDER BY ");
                sb.Append(String.Join(",", specs));
            }

            var output = new SqlParameter()
            {
                ParameterName = "@TotalRecordsCount",
                SqlDbType = SqlDbType.Int,
                IsNullable = true,
                Direction = ParameterDirection.Output
            };

            var results =_context.Database.SqlQuery<DealFundsAllocation>(
                "dbo.sp_EasyFund_SearchFundsAllocation @LawyerId, @AllocationStatus, @StartIndex, @EndIndex, @SortStatement, @TotalRecordsCount OUT",
                new SqlParameter("@LawyerId", request.LawyerID.HasValue ? (object) request.LawyerID : DBNull.Value),
                new SqlParameter("@AllocationStatus",
                    string.IsNullOrWhiteSpace(request.AllocationStatus)
                        ? DBNull.Value
                        : (object) request.AllocationStatus),
                new SqlParameter("@StartIndex", startIndex),
                new SqlParameter("@EndIndex", endIndex),
                new SqlParameter("@SortStatement", sb.ToString()),
                output).ToList();

            totalRecordsCount = (int) output.Value;
            return results;
        }

        public void SavePayments(IDictionary<PaymentNotification, Allocation> payments)
        {
            try
            {
                foreach (var payment in payments)
                {
                    var entity = _paymentMapper.MapToEntity(payment.Key);
                    entity.RecordType = RecordType.Deposit;
                    if (payment.Value == null)
                    {
                        entity.AllocationStatus = AllocationStatus.UnAssigned;
                        entity.RecordType = RecordType.Deposit;
                    }
                    else
                    {
                        if (payment.Value.FundingDealId > 0 && !string.IsNullOrWhiteSpace(payment.Value.ShortFCTURN))
                        {
                            entity.FundingDealID = payment.Value.FundingDealId;
                            entity.ShortFCTRefNumber = payment.Value.ShortFCTURN;
                            entity.AllocationStatus = AllocationStatus.Allocated;
                            entity.RecordType = RecordType.Deposit;
                        }
                        else if (payment.Value.LawyerInfo != null)
                        {
                            entity.LawyerID = payment.Value.LawyerInfo.UserID;
                            entity.AllocationStatus = AllocationStatus.Assigned;
                            entity.RecordType = RecordType.Deposit;
                        }
                    }

                    Insert(entity);
                }
                _context.SaveChanges();

            }
            catch (DbEntityValidationException dbEx)
            {
                TraceExceptionInDebugMode(dbEx);
            }

        }

        [Conditional("DEBUG")]
        private static void TraceExceptionInDebugMode(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName,
                        validationError.ErrorMessage);
                }
            }
        }

    }
}
