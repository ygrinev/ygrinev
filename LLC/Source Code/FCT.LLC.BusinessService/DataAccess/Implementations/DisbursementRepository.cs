using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using FCT.EPS.PaymentTrackingService.DataContracts;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess.Implementations
{
    public class DisbursementRepository: Repository<tblDisbursement>,IDisbursementRepository
    {
        private readonly EFBusinessContext _context;
        private readonly IEntityMapper<tblDisbursement, Disbursement> _disbursementMapper;

        public DisbursementRepository(EFBusinessContext dbContext,
            IEntityMapper<tblDisbursement, Disbursement> disbursementMapper)
            : base(dbContext)
        {
            _disbursementMapper = disbursementMapper;
            _context = dbContext;
        }
        public DisbursementCollection SaveDisbursements(SaveDisbursementsRequest request, int fundingDealId = 0, DisbursementFee disbursementFee = null)
        {
            var toBeAdded = new List<tblDisbursement>();
            var tobeUpdated = new List<tblDisbursement>();
            var toBeDeleted = new List<int>();

            foreach (var disbursement in request.Disbursements)
            {
                var entity = _disbursementMapper.MapToEntity(disbursement);
                switch (disbursement.Action)
                {
                    case CRUDAction.Create:
                        if (entity.PayeeType == EasyFundFee.FeeName && disbursementFee != null)
                        {
                            CreateFeeDisbursement(disbursementFee, entity);
                        }
                        entity.FundingDealID = fundingDealId;
                        entity.ChainDealID = request.DealID;
                        toBeAdded.Add(entity);
                        break;
                    case CRUDAction.Update:
                        if (entity.PayeeType == EasyFundFee.FeeName && disbursementFee != null)
                        {
                            CreateFeeDisbursement(disbursementFee, entity);
                        }
                        if (entity.DisbursementID > 0)
                        {
                            entity.FundingDealID = fundingDealId;
                            tobeUpdated.Add(entity);
                        }
                        break;
                    case CRUDAction.Delete:
                        if (entity.DisbursementID > 0)
                        {
                            toBeDeleted.Add(entity.DisbursementID);
                        }
                        break;
                }
            }
            try
            {
                if (toBeDeleted.Any())
                {
                    DeleteDisbursements(toBeDeleted, fundingDealId);
                }

                _context.Configuration.AutoDetectChangesEnabled = false;
                if (toBeAdded.Any())
                {
                    InsertRange(toBeAdded);
                }
                if (tobeUpdated.Any())
                {
                    foreach (var item in tobeUpdated)
                    {
                        Update(item);
                    }
                }
                _context.Configuration.AutoDetectChangesEnabled = true;
                _context.SaveChanges();

                var coll = new DisbursementCollection();
                coll.AddRange(toBeAdded.Select(e => _disbursementMapper.MapToData(e)));
                coll.AddRange(tobeUpdated.Select(e => _disbursementMapper.MapToData(e)));
                if (toBeAdded.Count <= 0 && tobeUpdated.Count <= 0 && toBeDeleted.Count > 0)
                {
                    coll.AddRange(
                        toBeDeleted.Select(
                            item => new Disbursement() { DisbursementID = item, Action = CRUDAction.Delete }));
                }
                return coll;
            }
            catch (Exception ex)
            {
                throw new DataAccessException() { BaseException = ex };
            }

        }
        public string GetFCTURNByItemID(int ItemID)
        {
            string FCTURN = string.Empty;
            try
            {
                var tblDisbursement = _context.tblDisbursements.AsNoTracking().FirstOrDefault(d => d.DisbursementID == ItemID);
                if (tblDisbursement != null)
                {
                    int fundingDealID = tblDisbursement.FundingDealID;
                    var tblFundingDeal = _context.tblFundingDeals.FirstOrDefault(f => f.FundingDealID == fundingDealID);
                    if (tblFundingDeal != null)
                    {
                        int dealScopeID = tblFundingDeal.DealScopeID;
                        //FCTURN = _context.tblDealScopes.FirstOrDefault(d => d.DealScopeID == dealScopeID).ShortFCTRefNumber;
                        FCTURN = _context.tblDealScopes.FirstOrDefault(d => d.DealScopeID == dealScopeID).FCTRefNumber;
                    }
                }
            }
            catch (Exception e)
            {
                FCTURN = "ERROR RETRIEVING FCT Ref Number Stack Trace--> " + e.ToString();
            }
            return FCTURN;

        }

        public bool IsDisbursementDocumentGenerated(int dealDocumentTypeId)
        {
            var isGenerated = false;

            if (dealDocumentTypeId > 0)
            {
                const string query = "[dbo].[sp_tblDocument_IsPdfGenerated] @DealDocumentTypeID";
                var result = _context.Database.SqlQuery<bool?>(query,
                    new SqlParameter("@DealDocumentTypeID", dealDocumentTypeId)).FirstOrDefault();

                if (result != null)
                {
                    isGenerated = (bool) result;
                }

            }

            return isGenerated;
        }

        private static void CreateFeeDisbursement(DisbursementFee disbursementFee, tblDisbursement entity)
        {
            switch (entity.FCTFeeSplit)
            {
                case FeeDistribution.SplitEqually:
                    entity.PurchaserFeeID = disbursementFee.PurchaserFee.FeeID;
                    entity.VendorFeeID = disbursementFee.VendorFee.FeeID;
                    break;
                case FeeDistribution.VendorLawyer:
                    entity.VendorFeeID = disbursementFee.VendorFee.FeeID;
                    break;
                case FeeDistribution.PurchaserLawyer:
                    entity.PurchaserFeeID = disbursementFee.PurchaserFee.FeeID;
                    break;
            }
        }

        private void DeleteDisbursements(IEnumerable<int> toBeDeleted, int fundingDealId)
        {
            string tobeDeletedIds = string.Join(",", toBeDeleted);
            ExecuteQuery("dbo.sp_EasyFund_DeleteDisbursements @p0,@p1", tobeDeletedIds, fundingDealId);
        }

        public DisbursementCollection GetDisbursements(int dealId)
        {
            var deal = _context.tblDeals.FirstOrDefault(dl => dl.DealID == dealId);
            if (deal != null && deal.DealScopeID.HasValue)
            {
                var fundingDeal = _context.tblFundingDeals.SingleOrDefault(f => f.DealScopeID == deal.DealScopeID);
                if (fundingDeal != null)
                {
                    var fundingDealId = fundingDeal.FundingDealID;
                    var coll = new DisbursementCollection();
                    var entities = GetAll()
                        .Include(d => d.VendorFee)
                        .Include(d => d.PurchaserFee)
                        .Where(d => d.FundingDealID == fundingDealId).AsEnumerable();
                    coll.AddRange(entities.Select(e => _disbursementMapper.MapToData(e)));
                    return coll;
                }
            }
            return null;
        }

        public Disbursement GetDisbursement(int disbursementId)
        {
            var entity =
                GetAll()
                    .Include(d => d.VendorFee)
                    .Include(d => d.PurchaserFee)
                    .SingleOrDefault(d => d.DisbursementID == disbursementId);
            return _disbursementMapper.MapToData(entity);
        }

        public DisbursementCollection GetDisbursementsByType(string payeeType, int fundingDealId)
        {
            var disbursements =
                GetAll().Where(d => d.PayeeType == payeeType && d.FundingDealID == fundingDealId).AsEnumerable();
            var coll = new DisbursementCollection();
            coll.AddRange(disbursements.Select(d => _disbursementMapper.MapToData(d)));
            return coll;
        }


        public GetPayoutLetterDateResponse GetPayoutLetterDate(GetPayoutLetterDateRequest request)
        {
            return (from d in _context.tblDeals
                    join fd in _context.tblFundingDeals on d.DealScopeID equals fd.DealScopeID
                    join di in _context.tblDisbursements on fd.FundingDealID equals di.FundingDealID
                    join ddt in _context.tblDisbursementDealDocumentTypes on di.DisbursementID equals ddt.DisbursementID
                    where ddt.DealDocumentTypeID == request.DocumentID
                    select new GetPayoutLetterDateResponse { PayoutLetterDate = ddt.PayoutLetterDate.Value, DisbursementID = ddt.DisbursementID }).FirstOrDefault();
        }

        public void UpdateDisbursementsFromNotifications(IDictionary<int, PaymentNotification> notificationDict)
        {
            try
            {
                var disbursementMap = (from p in _context.tblPaymentRequests
                                       where notificationDict.Keys.Contains(p.PaymentRequestID) & p.DisbursementID != null
                                       select p).ToDictionary(x => x.PaymentRequestID, y => y.DisbursementID);

                var duplicates =
                    disbursementMap.GroupBy(x => x.Value).Where(group => group.Count() > 1).Select(group => group.Key);

                var disbursementsToUpdate =
                    (from item in disbursementMap where !duplicates.Contains(item.Value) select item).ToDictionary(
                        x => x.Value, x => x.Key);

                var disbursements = from d in _context.tblDisbursements
                                    where disbursementsToUpdate.Keys.Contains(d.DisbursementID)
                                    select d;
                foreach (var disbursement in disbursements)
                {
                    int paymentRequestId;
                    if (disbursementsToUpdate.TryGetValue(disbursement.DisbursementID, out paymentRequestId))
                    {
                        var notification = notificationDict[paymentRequestId];
                        disbursement.DisbursementStatus = notification.PaymentStatus;
                        disbursement.PaymentReferenceNumber = notification.PaymentReferenceNumber;
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException() { BaseException = ex };
            }
        }

        public void SetReconciliationData(int itemID, string userID, bool reconciled)
        {
            ExecuteQuery("dbo.sp_EF_SetReconciliationData_Disbursement_v2 @p0,@p1,@p2", itemID, userID,reconciled);
        }

        public void UpdateFeeDisbursement(Disbursement disbursement)
        {
            var entity = _context.tblDisbursements.SingleOrDefault(d => d.DisbursementID == disbursement.DisbursementID);
            if (entity != null)
            {
                entity.Province = disbursement.Province;
                entity.Amount = disbursement.Amount;
                entity.FCTFeeSplit = disbursement.FCTFeeSplit;
                if (disbursement.VendorFee == null && entity.VendorFeeID.HasValue)
                {
                    entity.VendorFeeID = null;
                }
                if (disbursement.PurchaserFee != null)
                {
                    entity.PurchaserFeeID = disbursement.PurchaserFee.FeeID;
                }
                _context.SaveChanges();
            }
        }

        public void UpdateDisbursement(Disbursement disbursement)
        {
            var entity = _disbursementMapper.MapToEntity(disbursement);
            Update(entity);
            _context.SaveChanges();
        }
    }
}
