using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Implementations;
using FCT.LLC.Common.DataContracts;
using System.Data.Entity.Infrastructure;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class DealRepository : Repository<tblDeal>, IDealRepository
    {
        private readonly EFBusinessContext _context;

        public DealRepository(EFBusinessContext context)
            : base(context)
        {
            _context = context;
        }

        public tblDeal GetDealDetails(int DealID, UserContext userContext, bool DealInfoOnly = false)
        {
            tblDeal result;
            if (!DealInfoOnly)
            {
                result = _context.tblDeals.AsNoTracking()
                              .Where(d => d.DealID == DealID)
                              .Include(d => d.tblDealScope)
                              .Include(d => d.tblMortgagors)
                              .Include(d => d.tblLawyer)
                              .Include(d => d.tblLender)
                              .Include(d => d.tblMortgages)
                              .Include(d=>d.tblDealScope.tblVendors)
                              .Include(d => d.tblProperties).SingleOrDefault();
            }
            else
            {
                result = _context.tblDeals.AsNoTracking()
                                 .Include(d => d.tblDealScope)
                                 .Where(d => d.DealID == DealID)
                                 .SingleOrDefault();
            }

            return result;

        }

        public tblDeal GetDealDetailsByFCTURN(string FCTURN, bool DealInfoOnly = false)
        {
            tblDeal result;
            if (!DealInfoOnly)
            {
                result = _context.tblDeals.AsNoTracking()
                              .Where(d => d.FCTRefNum == FCTURN)
                              .Include(d => d.tblDealScope)
                              .Include(d => d.tblMortgagors)
                              .Include(d => d.tblLawyer)
                              .Include(d => d.tblLender)
                              .Include(d => d.tblMortgages)
                              .Include(d => d.tblDealScope.tblVendors)
                              .Include(d => d.tblProperties).SingleOrDefault();
            }
            else
            {
                result = _context.tblDeals.AsNoTracking()
                                 .Include(d => d.tblDealScope)
                                 .Where(d => d.FCTURN == FCTURN)
                                 .SingleOrDefault();
            }

            return result;

        }

        public tblDeal GetDealDetailsByDealId(int dealId, bool DealInfoOnly = false)
        {
            tblDeal result;
            if (!DealInfoOnly)
            {
                result = _context.tblDeals.AsNoTracking()
                              .Where(d => d.DealID == dealId)
                              .Include(d => d.tblDealScope)
                              .Include(d => d.tblMortgagors)
                              .Include(d => d.tblLawyer)
                              .Include(d => d.tblLender)
                              .Include(d => d.tblMortgages)
                              .Include(d => d.tblDealScope.tblVendors)
                              .Include(d => d.tblProperties).SingleOrDefault();
            }
            else
            {
                result = _context.tblDeals.AsNoTracking()
                                 .Include(d => d.tblDealScope)
                                 .Where(d => d.DealID == dealId)
                                 .SingleOrDefault();
            }

            return result;
        }

        public tblDeal DealSync(tblDeal sourceDeal, tblDeal targetDeal, UserContext userContext){
            // target deal will be returned after it is overwritten with source deal data
            // deal sync must be smart enough to know which fields get overwritten based upon the source deal and target deal types / metadata

            return targetDeal;
        }

        public tblDeal GetDealNotes(int DealID, UserContext userContext)
        {
            tblDeal result =
             _context.tblDeals.AsNoTracking()
                 .Where(d => d.DealID == DealID)
                 .Include(d => d.tblLawyer)
                 .Include(d => d.tblLender)
                 .Include(d => d.tblNotes)
                 .SingleOrDefault()
                 ;

            return result;
        }

        public tblDeal GetMilestones(int DealID, UserContext userContext)
        {
            tblDeal result = _context.tblDeals.AsNoTracking()
                 .Where(d => d.DealID == DealID)
                 .Include(d => d.tblMilestones)
                 .Include(d => d.tblMilestones.Select(p => p.tblMilestoneCode)).SingleOrDefault();

            if(result != null)
            {
                var labels = _context.tblMilestoneLabels.AsNoTracking()
                    .Where(l => l.LenderID == result.LenderID);

                if (labels.Any())
                {
                    var milestones = from m in result.tblMilestones
                                     join l in labels on m.MilestoneCodeID equals l.MilestoneCodeID
                                     orderby l.SequenceNumber
                                     select m;

                    result.tblMilestones = milestones.ToList();
                }
            }
            return result;
        }

        public tblDeal GetDealHistory(int DealID, UserContext userContext)
        {
            tblDeal result = _context.tblDeals.AsNoTracking()
                 .Where(d => d.DealID == DealID)
                 .Include(d => d.tblDealHistory)
                 .Include(d => d.tblLender)
                 .SingleOrDefault();

            return result;
        }

        public int GetOtherDealInScope(int dealID, int dealscopeID)
        {
            IEnumerable<int> result =
                GetAll()
                    .Where(d => d.DealScopeID == dealscopeID && d.DealID != dealID && d.Status != DealStatus.Cancelled && d.Status != DealStatus.CancelRequest && d.Status != DealStatus.Declined)
                    .Select(d => d.DealID)
                    .AsEnumerable();
            return result.SingleOrDefault();
        }

        public void UpdateDealStatus(int DealID, string status, UserContext userContext)
        {
            var deal = _context.tblDeals.FirstOrDefault(x => x.DealID == DealID);
            if (deal != null)
            {
                _context.Entry(deal).Property(d => d.Status).CurrentValue = status;
                _context.Entry(deal).Property(d => d.StatusDate).CurrentValue = DateTime.Now;
                _context.Entry(deal).Property(d => d.StatusUserType).CurrentValue = "SYSTEM";
                _context.Entry(deal).Property(d => d.LawyerDeclinedFlag).CurrentValue = status == "DECLINED";
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                throw new Exception("Can't find the Deal withe the id: " + DealID.ToString());
            }
        }

        public void UpdateDeal(tblDeal deal)
        {
            try
            {
                //Clear dealscope entity before updating the deal
                deal.tblDealScope = null;

                Update(deal);
                
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                TraceExceptionInDebugMode(dbEx);
                throw new Exception("Error in updating Deal" + Environment.NewLine + "Error Message: " + dbEx.Message.ToString());
            }           
            catch(Exception e)
            {
                throw new Exception("Error in updating Deal" + Environment.NewLine + "Error Message: " + e.Message.ToString());
            }
        }

        public tblBranchContact GetBranchContact (int ContactID)
        {
            return _context.tblBranchContacts.FirstOrDefault(b => b.ContactID == ContactID);
        }

        public void DeleteDeal(int DealID)
        {
            try
            {
                var dealToBeRemoved = _context.tblDeals.Find(DealID);
                if(null != dealToBeRemoved)
                {
                    _context.tblDeals.Remove(dealToBeRemoved);
                    _context.SaveChanges();
                }
            }
            catch(Exception e)
            {
                throw new Exception("Error in deleting Deal" + Environment.NewLine + "Error Message: " + e.Message.ToString());
            }
        }

        public void RemoveDealScopeFromDeal(int DealID)
        {
            try
            {
                var dealToBeUpdated = _context.tblDeals.Find(DealID);
                if (null != dealToBeUpdated)
                {
                    dealToBeUpdated.DealScopeID = null;
                    _context.Entry(dealToBeUpdated).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error in removing DealScopeID from tblDeal" + Environment.NewLine + "Error Message: " + e.Message.ToString());
            }
        }

        public bool IsTwoWayLender(int dealId)
        {
            var deals = _context.tblDeals.AsQueryable().Include(d => d.tblLender).Where(d => d.DealID == dealId);
            var deal = deals.SingleOrDefault();
            if (deal != null)
            {
                return deal.tblLender.Is2WayLender.GetValueOrDefault();
            }
            return false;
        }

        public bool IsFCTLender(int dealId)
        {
            var deals = _context.tblDeals.AsQueryable().Include(d => d.tblLender).Where(d => d.DealID == dealId);
            var deal = deals.SingleOrDefault();
            if (deal != null && deal.tblLender != null)
            {
                return (deal.tblLender.LenderCode == "FCT");
            }
            return false;
        }

        public tblDeal GetDealWithDealScopeDetails(int dealId)
        {
            tblDeal result;
            result =  _context.tblDeals.AsNoTracking()
                              .Where(d => d.DealID == dealId)
                              .Include(d => d.tblDealScope)
                              .SingleOrDefault();

            return result;

        }

        public tblFinancialInstitutionNumber GetFinancialInstitutionDetails(string FINumber)
        {
            return _context.tblFinancialInstitutionNumbers.FirstOrDefault(f => f.FINumber == FINumber);
        }

        [Conditional("DEBUG")]
        private static void TraceExceptionInDebugMode(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    Trace.TraceInformation("Deal: {0} Error: {1}", validationError.PropertyName,
                        validationError.ErrorMessage);
                }
            }
        }

        //Added By Mehdi
        public IQueryable<tblDealHistory> GetDealHistory(int DealID)
        {
            return _context.tblDealHistories.AsNoTracking().Where(d => d.DealID == DealID);
        }

        public tblDeal GetDeal(int DealID, bool DealInfoOnly = false)
        {
            tblDeal result;
            if (!DealInfoOnly)
            {
                result = _context.tblDeals.AsNoTracking()
                              .Where(d => d.DealID == DealID)
                              .Include(d => d.tblDealScope)
                              .Include(d => d.tblMortgagors)
                              .Include(d => d.tblLawyer)
                              .Include(d => d.tblLender)
                              .Include(d => d.tblMortgages)
                              .Include(d => d.tblDealScope.tblVendors)
                              .Include(d => d.tblProperties).SingleOrDefault();
            }
            else
            {
                result = _context.tblDeals.AsNoTracking()
                                 .Include(d => d.tblLender)
                                 .Include(d => d.tblDealHistory)
                                 .Where(d => d.DealID == DealID)
                                 .SingleOrDefault();
            }

            return result;

        }
    }
}
