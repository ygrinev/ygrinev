using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess.Implementations
{
    public class FundedRepository:Repository<tblFundingDeal>, IFundedRepository
    {
        private readonly IEntityMapper<tblFundingDeal, FundedDeal> _fundsMapper;
        private readonly EFBusinessContext _context;

        public FundedRepository(EFBusinessContext dbContext, IEntityMapper<tblFundingDeal, FundedDeal> fundsMapper)
            : base(dbContext)
        {
            _fundsMapper = fundsMapper;
            _context = dbContext;
        }

        public int InsertFundedDeal(FundedDeal fundedDeal)
        {
           
            if (fundedDeal != null)
            {
                var entity = _fundsMapper.MapToEntity(fundedDeal);
                Insert(entity);
                return _context.SaveChanges();
            }
            return 0;
        }

        public void UpdateFundedDeal(FundedDeal fundedDeal)
        {
            if (fundedDeal != null)
            {
                var entity = _fundsMapper.MapToEntity(fundedDeal);
                Update(entity);
                _context.SaveChanges();
            }
        }

        public FundingMilestone UpdateMilestones(int fundedDealId, FundingMilestone fundingMilestone)
        {
            var fundedDeal = GetAll().SingleOrDefault(d => d.FundingDealID == fundedDealId);
            if (fundedDeal != null)
            {
                _context.Entry(fundedDeal).Property(d => d.InvitationSent).CurrentValue =
                    fundingMilestone.InvitationSent.HasValue
                        ? fundingMilestone.InvitationSent
                        : fundedDeal.InvitationSent;

                _context.Entry(fundedDeal).Property(d => d.InvitationAccepted).CurrentValue =
                    fundingMilestone.InvitationAccepted.HasValue
                        ? fundingMilestone.InvitationAccepted
                        : fundedDeal.InvitationAccepted;

                _context.Entry(fundedDeal).Property(d => d.SignedByPurchaser).CurrentValue =
                    fundingMilestone.SignedByPurchaser.HasValue
                        ? fundingMilestone.SignedByPurchaser
                        : fundedDeal.SignedByPurchaser;

                _context.Entry(fundedDeal).Property(d => d.SignedByVendor).CurrentValue =
                    fundingMilestone.SignedByVendor.HasValue
                        ? fundingMilestone.SignedByVendor
                        : fundedDeal.SignedByVendor;

                _context.Entry(fundedDeal).Property(d => d.Funded).CurrentValue =
                    fundingMilestone.Funded.HasValue
                        ? fundingMilestone.Funded
                        : fundedDeal.Funded;

                _context.Entry(fundedDeal).Property(d => d.Disbursed).CurrentValue =
                    fundingMilestone.Disbursed.HasValue
                        ? fundingMilestone.Disbursed
                        : fundedDeal.Disbursed;

                _context.Entry(fundedDeal).Property(d => d.PayoutSent).CurrentValue =
                    fundingMilestone.PayoutSent.HasValue
                        ? fundingMilestone.PayoutSent
                        : fundedDeal.PayoutSent;

                _context.Entry(fundedDeal).Property(d => d.SignedByPurchaserName).CurrentValue =
                    !string.IsNullOrEmpty(fundingMilestone.SignedByPurchaserName)
                        ? fundingMilestone.SignedByPurchaserName
                        : fundedDeal.SignedByPurchaserName;

                _context.Entry(fundedDeal).Property(d => d.SignedByVendorName).CurrentValue =
                   !string.IsNullOrEmpty(fundingMilestone.SignedByVendorName)
                        ? fundingMilestone.SignedByVendorName
                        : fundedDeal.SignedByVendorName;

                Update(fundedDeal);
                _context.SaveChanges();

                var data = _fundsMapper.MapToData(fundedDeal);
                return data.Milestone;
            }
            return null;
        }

        /// <summary>
        /// Resets the Milestones of an EasyFund deal.
        /// This is necessary because an EasyFund Combo deal can be cancelled & reinstructed
        /// </summary>
        /// <param name="fundedDealId">The ID of the funded deal</param>
        public void ResetMilestones(int fundedDealId)
        {
            var fundedDeal = GetAll().SingleOrDefault(d => d.FundingDealID == fundedDealId);
            if (fundedDeal != null)
            {
                _context.Entry(fundedDeal).Property(d => d.InvitationSent).CurrentValue = null;

                _context.Entry(fundedDeal).Property(d => d.InvitationAccepted).CurrentValue = null;

                _context.Entry(fundedDeal).Property(d => d.SignedByPurchaser).CurrentValue = null;

                _context.Entry(fundedDeal).Property(d => d.SignedByVendor).CurrentValue = null;

                _context.Entry(fundedDeal).Property(d => d.Funded).CurrentValue = null;

                _context.Entry(fundedDeal).Property(d => d.Disbursed).CurrentValue = null;

                _context.Entry(fundedDeal).Property(d => d.PayoutSent).CurrentValue = null;

                _context.Entry(fundedDeal).Property(d => d.SignedByPurchaserName).CurrentValue = string.Empty;

                _context.Entry(fundedDeal).Property(d => d.SignedByVendorName).CurrentValue = string.Empty;
                Update(fundedDeal);

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var objectContext = ((IObjectContextAdapter)_context).ObjectContext;
                    objectContext.Refresh(RefreshMode.ClientWins, _context.tblFundingDeals);
                    _context.SaveChanges();
                }
            }
        }

        public FundedDeal GetMilestonesByDeal(int dealId)
        {
            var fundedDeal = (from f in _context.tblFundingDeals.AsNoTracking()
                join dl in _context.tblDeals.AsNoTracking() on f.DealScopeID equals dl.DealScopeID
                where dl.DealID.Equals(dealId)
                select f).SingleOrDefault();
            return _fundsMapper.MapToData(fundedDeal);
        }

        public int GetFundingDealIdByScope(int dealscopeId)
        {
            var fundingDeal = GetAll().SingleOrDefault(f => f.DealScopeID == dealscopeId);
            if (fundingDeal != null)
                return fundingDeal.FundingDealID;
            return 0;
        }

        public FundedDeal GetFundingDealByScope(int dealscopeId)
        {
            var fundingDeal = GetAll().SingleOrDefault(f => f.DealScopeID == dealscopeId);
            return _fundsMapper.MapToData(fundingDeal);
        }

        public int GetFundingDealIdByDeal(int dealId)
        {
            var fundingDealId = (from f in _context.tblFundingDeals
                       join d in _context.tblDeals on f.DealScopeID equals d.DealScopeID
                       where d.DealID == dealId
                       select f.FundingDealID).SingleOrDefault();
            return fundingDealId;
        }

        public FundingMilestone UpdateDisbursedMilestone(int fundingDealId)
        {
            var fundedDeal = _context.tblFundingDeals.SingleOrDefault(d => d.FundingDealID == fundingDealId);
            if (fundedDeal != null)
            {
                if (fundedDeal.Disbursed.HasValue)
                {
                    throw new ValidationException(new List<ErrorCode>() { ErrorCode.ConcurrencyCheckFailed });
                }
                try
                {
                    fundedDeal.Disbursed = DateTime.Now;
                    Update(fundedDeal);
                    _context.SaveChanges();
                    var data = _fundsMapper.MapToData(fundedDeal);
                    return data.Milestone;
                }
                catch (Exception e)
                {
                    if (e is DbUpdateConcurrencyException || e is DBConcurrencyException ||
                        e is OptimisticConcurrencyException)
                    {
                        throw new ValidationException(new List<ErrorCode>(){ErrorCode.ConcurrencyCheckFailed});
                    }
                    throw e;
                }
            }
            return null;
          
        }
    }
}
