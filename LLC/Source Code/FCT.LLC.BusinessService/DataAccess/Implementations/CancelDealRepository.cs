using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using System.Data.SqlClient;

namespace FCT.LLC.BusinessService.DataAccess
{
    public sealed partial class FundingDealRepository
    {
        public int GetOtherCancelledDealInScope(int dealID, int dealscopeID = 0)
        {
            //var deal = GetAll().SingleOrDefault(d => d.DealID == dealID);

            tblDeal deal = _context.tblDeals
                                   .AsNoTracking()
                                   .Where(d => d.DealID == dealID)
                                   .SingleOrDefault();
            string otherActingFor = string.Empty;

            if (deal != null)
            {
                otherActingFor = GetOtherDealActingFor(deal.LawyerActingFor);
            }

            if (dealscopeID > 0)
            {
                IEnumerable<int> result =
                    GetAll()
                        .Where(
                            d =>
                                d.DealScopeID == dealscopeID && d.DealID != dealID &&
                                d.LawyerActingFor == otherActingFor &&
                                (d.Status == DealStatus.Cancelled))
                        .Select(d => d.DealID)
                        .AsEnumerable();
                return result.FirstOrDefault();
            }

            int otherDealId = (from d in _context.tblDeals
                           join scope in
                               (from ds in _context.tblDealScopes
                                join d in _context.tblDeals on new { ds.DealScopeID, DealID = dealID } equals new { DealScopeID = d.DealScopeID.Value, d.DealID }
                                select ds)
                           on d.DealScopeID equals scope.DealScopeID
                           where d.DealID != dealID && 
                                 d.Status == DealStatus.Cancelled &&
                                 d.LawyerActingFor == otherActingFor
                                 orderby d.DealID descending
                               select d.DealID).FirstOrDefault();

            return otherDealId;
        }

        public FundingDeal GetCancelledDeal(int dealId)
        {
            tblDeal result =
                _context.tblDeals.AsNoTracking()
                    .Where(d => d.DealID == dealId)
                    .Include(d => d.tblDealScope.tblVendors)
                    .Include(d => d.tblMortgagors)
                    .Include(d => d.tblLawyer)
                    .Include(d => d.tblProperties).SingleOrDefault();

            //if deal is found
            if (result != null)
            {
                //retrieve closing date
                var closingDate =
                    _context.tblMortgages.AsNoTracking()
                        .Where(m => m.DealID == result.DealID)
                        .Select(m => m.ClosingDate).SingleOrDefault();
                FundingDeal deal = _dealMapper.MapToData(result);
                //get deal scope
                tblDealScope dealScope =
                    _context.tblDealScopes.AsNoTracking()
                        .Where(d => d.DealScopeID == result.DealScopeID)
                        .Include(d => d.tblDeals.Select(dl => dl.tblLawyer))
                        .SingleOrDefault();

                //Get other deal information
                if (dealScope != null)
                {
                    var otherDealActingFor = GetOtherDealActingFor(deal.ActingFor);
                    tblDeal otherdeal =
                        dealScope.tblDeals.FirstOrDefault(
                            d =>
                                d.DealID != dealId && d.LawyerActingFor == otherDealActingFor);
                    deal.FCTURN = dealScope.FCTRefNumber; //dealScope.ShortFCTRefNumber;
                    deal.ClosingDate = closingDate;
                    if (otherdeal != null)
                    {
                        deal.OtherLawyer = _lawyerMapper.MapToData(otherdeal.tblLawyer);
                        deal.OtherLawyerFileNumber = otherdeal.LawyerMatterNumber;
                    }
                }
                return deal;
            }
            return null;
        }

        public FundingDeal GetOtherDeal(int dealId)
        {
            //var deals = GetAll().Where(d => d.DealID == dealId)
            //    .Select(o => new {o.DealID, o.LawyerActingFor, o.DealScopeID});
            
            //var deal = deals.SingleOrDefault();

            tblDeal deal = _context.tblDeals
                               .AsNoTracking()
                               .Where(d => d.DealID == dealId)
                               .SingleOrDefault();
           
            if (deal != null)
            {
                string otherActingFor = GetOtherDealActingFor(deal.LawyerActingFor);
                if (deal.DealScopeID > 0 && otherActingFor!=LawyerActingFor.Both && otherActingFor!=LawyerActingFor.Mortgagor)
                {
                    IEnumerable<int> result =
                   GetAll()
                       .Where(
                           d =>
                               d.DealScopeID == deal.DealScopeID &&
                               d.LawyerActingFor == otherActingFor &&
                               d.Status != DealStatus.Cancelled && d.Status != DealStatus.CancelRequest &&
                                d.Status != DealStatus.Declined)
                       .Select(d => d.DealID)
                       .AsEnumerable();
                    int otherDealId = result.SingleOrDefault();
                    var otherDeal = GetFundingDeal(otherDealId);
                    return otherDeal;
                }
               
            }
            return null;
        }

        public void UpdateDealStatus(IEnumerable<int> dealIds, string newStatus)
        {
            var entities = (from d in _context.tblDeals where dealIds.Contains(d.DealID) select d);
            foreach (var entity in entities)
            {
                entity.Status = newStatus;
            }
            _context.SaveChanges();
        }

        public Lookup GetOtherDealStatus(int dealId)
        {
            int otherDealId = GetOtherDealInScope(dealId);
            var results =
                GetAll()
                    .Where(d => d.DealID == otherDealId)
                    .Select(o => new {o.DealID, o.Status})
                    .Select(o=>new Lookup{Key = o.DealID.ToString(), Value = o.Status});
            if (results.Any())
            {
                return results.SingleOrDefault();
            }
           return null;
        }
    }
}
