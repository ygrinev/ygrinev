using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.DataAccess.Interfaces;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DisbursementSummaryRepository: Repository<tblDisbursementSummary>,IDisbursementSummaryRepository
    {
        private readonly EFBusinessContext _context;
        private readonly IEntityMapper<tblDisbursementSummary, DisbursementSummary> _disbursementSummaryMapper;

        public DisbursementSummaryRepository(EFBusinessContext dbContext,
            IEntityMapper<tblDisbursementSummary, DisbursementSummary> disbursementSummmaryMapper) : base(dbContext)
        {
            _context = dbContext;
            _disbursementSummaryMapper = disbursementSummmaryMapper;
        }

        public decimal GetDepositRequired(int fundingDealId)
        {
            var disbursementSummary = GetAll().SingleOrDefault(f => f.FundingDealID == fundingDealId);
            if (disbursementSummary != null)
            {
                var money =
                    disbursementSummary.DepositAmountRequired;
                return money;
            }
            return 0;
        }
        public void UpdateDisbursementSummary(DisbursementSummary disbursementSummary)
        {
            var entity = _disbursementSummaryMapper.MapToEntity(disbursementSummary); 
            UpdateDisbursementSummary(disbursementSummary, disbursementSummary.FundingDealID, disbursementSummary.RequiredDepositAmount);
        }
        public DisbursementSummary UpdateDisbursementSummary(DisbursementSummary disbursementSummary, int fundingDealId, decimal amount)
        {
            var entity = _disbursementSummaryMapper.MapToEntity(disbursementSummary);           
            if (entity != null)
            {
                entity.FundingDealID = fundingDealId;
                entity.DepositAmountRequired = amount;
                if (entity.DisbursementSummaryID > 0 && entity.Version.Length > 0)
                {
                    Update(entity);                   
                }
                else if (entity.DisbursementSummaryID <= 0 && !string.IsNullOrWhiteSpace(entity.Comments))
                {
                    //38563 – Ops portal Save & Cancel not saved when navigating away after adding Comments
                    // When there are no disbursements for the deal, system will not save comments
                    Insert(entity);
                }
                _context.SaveChanges();
                
            }
            return GetDisbursementSummary(fundingDealId);
        }

        public DisbursementSummary InsertDisbursementSummary(int fundingDealId, decimal amount)
        {
            var entity = new tblDisbursementSummary()
            {
                FundingDealID = fundingDealId,
                DepositAmountRequired = amount
            };
            Insert(entity);
            _context.SaveChanges();
            return GetDisbursementSummary(fundingDealId);
        }

        public DisbursementSummary GetDisbursementSummary(int fundingDealId, DisbursementSummary disbursementSummary)
        {
            var summary = _context.vw_EFDisbursementSummaries.FirstOrDefault(f => f.FundingDealID == fundingDealId);
            var tbl = new tblDisbursementSummary();
            var data = _disbursementSummaryMapper.MapToData(tbl, summary);
            return data;
        }
        public DisbursementSummary GetDisbursementSummary(int fundingDealId)
        {
            var summary = _context.vw_EFDisbursementSummaries.FirstOrDefault(f => f.FundingDealID == fundingDealId);
            var data = _disbursementSummaryMapper.MapToData(null, summary);
            return data;
        }

        public byte[] GetDisbursementSummaryVersion(int fundingDealId)
        {
            var disbursementSummary=GetAll().FirstOrDefault(s => s.FundingDealID == fundingDealId);
            if (disbursementSummary != null)
            {
                return disbursementSummary.Version;
            }
            return new byte[0];
        }

        public void UpdateDisbursementSummary(int fundingDealId, decimal amount)
        {
            var summary=_context.tblDisbursementSummaries.SingleOrDefault(f => f.FundingDealID == fundingDealId);
            if (summary != null)
            {
                summary.DepositAmountRequired = amount;
            }
            _context.SaveChanges();
        }
    }
}
