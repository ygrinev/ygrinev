using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;
using FCT.LLC.Logging;

namespace FCT.LLC.BusinessService.DataAccess
{
    public sealed partial class FundingDealRepository : Repository<tblDeal>, IFundingDealRepository
    {
        private readonly EFBusinessContext _context;
        private readonly IEntityMapper<tblDeal, FundingDeal> _dealMapper;
        private readonly IEntityMapper<tblLawyer, Lawyer> _lawyerMapper;
        private readonly ILogger _logger;
        private readonly IEntityMapper<tblFundingDeal, FundedDeal> _fundedMapper;
        private readonly IEntityMapper<tblBuilderLegalDescription, BuilderLegalDescription> _builderLegalDescriptionMapper;

        public FundingDealRepository(EFBusinessContext context, IEntityMapper<tblDeal, FundingDeal> dealMapper,
            IEntityMapper<tblLawyer, Lawyer> lawyerMapper, ILogger logger, 
            IEntityMapper<tblFundingDeal, FundedDeal> fundedMapper,
            IEntityMapper<tblBuilderLegalDescription, BuilderLegalDescription> builderLegalDescriptionMapper)
            : base(context)
        {
            if (context == null)
            {
                throw new ArgumentException("context");
            }
            if (context.Database.Connection == null)
            {
                throw  new ArgumentException("connection cannot be null");
            }
            _context = context;
            _dealMapper = dealMapper;
            _lawyerMapper = lawyerMapper;
            _logger = logger;
            _fundedMapper = fundedMapper;
            _builderLegalDescriptionMapper = builderLegalDescriptionMapper;
        }

        public FundingDeal GetFundingDeal(int dealId)
        {
            try
            {
                tblDeal result =
                    _context.tblDeals.AsNoTracking()
                        .Where(d => d.DealID == dealId)
                        .Include(d => d.tblDealScope.tblVendors)
                        .Include(d => d.tblMortgagors)
                        .Include(d => d.tblLawyer)
                        .Include(d => d.tblDealContacts)
                        .Include(d => d.tblProperties.Select(p => p.tblPINs))
                        .Include(d => d.tblProperties.Select(b => b.tblBuilderLegalDescriptions))
                        .SingleOrDefault();

                //if deal is found
                if (result != null)
                {
                    var closingDate =
                        _context.tblMortgages.AsNoTracking()
                            .Where(m => m.DealID == result.DealID)
                            .Select(m => m.ClosingDate).SingleOrDefault();

                    //get deal contacts
                    result.tblLawyer.tblDealContacts = result.tblDealContacts;
                    var lawyerIds = result.tblDealContacts.Select(l => l.LawyerID);
                    var contacts =
                        _context.tblLawyers.AsNoTracking()
                            .Where(l => lawyerIds.Contains(l.LawyerID))
                            .Select(o => new {o.FirstName, o.LastName, o.LawyerID})
                            .AsEnumerable()
                            .Select(
                                o =>
                                    new DealContactDetails()
                                    {
                                        ContactFirstName = o.FirstName,
                                        ContactLastName = o.LastName,
                                        LawyerContactID = o.LawyerID
                                    });


                    
                    //Set Builder Unit Levels for Builder Description
                    var property = result.tblProperties.FirstOrDefault();
                    var builderLegalDescriptions = property.tblBuilderLegalDescriptions;
                    var builderLegalDescription = builderLegalDescriptions.FirstOrDefault();
                    if (builderLegalDescription != null)
                    {
                        var builderUnitLevels =
                                _context.tblBuilderUnitLevels.AsNoTracking()
                                        .Where(ul => ul.BuilderLegalDescriptionID.Equals(builderLegalDescription.BuilderLegalDescriptionID));
                        if (builderUnitLevels != null)
                        {
                            builderLegalDescription.tblBuilderUnitLevels = builderUnitLevels.ToList();
                            builderLegalDescriptions.Add(builderLegalDescription);
                        }
                    }

              
                    //construct funding deal
                    FundingDeal deal = _dealMapper.MapToData(result, contacts);

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
                            dealScope.tblDeals.SingleOrDefault(
                                d =>
                                    d.DealID != dealId && d.LawyerActingFor == otherDealActingFor &&
                                    d.Status != DealStatus.Cancelled && d.Status != DealStatus.CancelRequest && d.Status != DealStatus.Declined);
                        deal.FCTURN = dealScope.FCTRefNumber; //dealScope.ShortFCTRefNumber;
                        deal.DealFCTURN = result.FCTRefNum;
                        deal.ClosingDate = closingDate;
                        deal.WireDepositDetails = dealScope.WireDepositDetails;
                        if (otherdeal != null)
                        {
                            var otherContacts =
                                _context.tblDealContacts.AsNoTracking().Where(dc => dc.DealID == otherdeal.DealID);
                            var otherLawyerIds = otherContacts.Select(l => l.LawyerID);
                            var otherDealContacts = _context.tblLawyers.AsNoTracking()
                                .Where(l => otherLawyerIds.Contains(l.LawyerID))
                                .Select(o => new {o.FirstName, o.LastName, o.LawyerID})
                                .AsEnumerable()
                                .Select(
                                    o =>
                                        new DealContactDetails()
                                        {
                                            ContactFirstName = o.FirstName,
                                            ContactLastName = o.LastName,
                                            LawyerContactID = o.LawyerID
                                        });

                            otherdeal.tblLawyer.tblDealContacts = otherContacts.ToList();
                            deal.OtherLawyer = _lawyerMapper.MapToData(otherdeal.tblLawyer, otherDealContacts);
                            deal.OtherLawyerFileNumber = otherdeal.LawyerMatterNumber;
                            deal.OtherLawyerDealStatus = otherdeal.Status;

                            

                        }
                        
                        //Set Other Lawyer Fields to data received from conveyancer when Other Lawyer is not assigned to the deal
                        if( (deal.OtherLawyer == null) || (deal.OtherLawyer != null && deal.OtherLawyer.LawyerID <= 0))
                        {
                            tblFundingDeal otherLawyerInfo =  _context.tblFundingDeals
                                                                        .FirstOrDefault(d => d.DealScopeID == dealScope.DealScopeID);

                            if (otherLawyerInfo != null)
                            {
                                deal.OtherLawyer = new Lawyer();
                                deal.OtherLawyer.FirstName = otherLawyerInfo.OtherLawyerFirstName;
                                deal.OtherLawyer.LastName = otherLawyerInfo.OtherLawyerLastName;
                                deal.OtherLawyer.LawFirm = otherLawyerInfo.OtherLawyerFirmName;   
                            }
                        }
                    }

                    return deal;
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex);
                throw new DataAccessException() {BaseException = ex};
            }


            return null;
        }

        private static string GetOtherDealActingFor(string currentDealActingFor)
        {
            string otherDealActingFor = string.Empty;
            switch (currentDealActingFor)
            {
                case LawyerActingFor.Vendor:
                    otherDealActingFor = LawyerActingFor.Purchaser;
                    break;
                case LawyerActingFor.Purchaser:
                    otherDealActingFor = LawyerActingFor.Vendor;
                    break;
                case LawyerActingFor.Both:
                case LawyerActingFor.Mortgagor:
                    otherDealActingFor = currentDealActingFor;
                    break;
            }
            return otherDealActingFor;
        }

        public int GetOtherDealInScope(int dealID, int dealscopeID = 0, string currentActingFor=null)
        {
            var deal = GetAll().SingleOrDefault(d => d.DealID == dealID);
            string otherActingFor = string.Empty;
            int otherDealId = 0;
            if (string.IsNullOrEmpty(currentActingFor))
            {
                if (deal != null)
                {
                    otherActingFor = GetOtherDealActingFor(deal.LawyerActingFor);
                }  
            }
            else
            {
                otherActingFor = GetOtherDealActingFor(currentActingFor);
            }

            if (dealscopeID > 0)
            {
                //IEnumerable<int> result =
                //    GetAll()
                //        .Where(
                //            d =>
                //                d.DealScopeID == dealscopeID && d.DealID != dealID &&
                //                d.LawyerActingFor == otherActingFor &&
                //                (d.Status != DealStatus.Cancelled && d.Status != DealStatus.CancelRequest &&
                //                 d.Status != DealStatus.Declined))
                //        .Select(d => d.DealID)
                //        .AsEnumerable();

                //return result.SingleOrDefault();

                // Above code returns multiple records when expecting only one record for the conditions provided. 
                // Modified code to use basic linq query
                otherDealId = (from d in _context.tblDeals
                         where d.DealScopeID == dealscopeID &&
                               d.DealID != dealID &&
                               (d.Status != DealStatus.Cancelled && d.Status != DealStatus.CancelRequest &&
                                  d.Status != DealStatus.Declined)
                         select d.DealID).SingleOrDefault();

                return otherDealId;   
            }

            otherDealId = (from d in _context.tblDeals
                               join scope in
                                   (from ds in _context.tblDealScopes
                                    join d in _context.tblDeals on new { ds.DealScopeID, DealID = dealID } equals new { DealScopeID = d.DealScopeID.Value, d.DealID }
                                    select ds)
                                    on d.DealScopeID equals scope.DealScopeID
                               where d.DealID != dealID && d.Status != DealStatus.Cancelled 
                               && d.Status != DealStatus.CancelRequest
                               && d.Status != DealStatus.Declined 
                               && d.LawyerActingFor == otherActingFor
                               select d.DealID).SingleOrDefault();

            return otherDealId;
        }


        public DealDetails GetDeal(int dealscopeID, bool lawyerActingForVendor)
        {
            tblDeal deal;
            IQueryable<tblDeal> deals;
             /*
             Modified logic to handle when there are multiple deals for Vendor/Purchaser Lawyers
             This can happen in case of Combo Deals (LLC/MMS + EF)
              Scenario: 
              Add EF to MMS 
              Invite Other Lawyer (Accepts deal)
              Cancel EF deal
              Add EF deal and assign to lawyer 
            */
            if (lawyerActingForVendor)
            {
                 deals =
                    GetAll()
                        .Include(d => d.tblDealScope.tblFundingDeals)
                        .Where(d => d.DealScopeID == dealscopeID && 
                                    d.Status != DealStatus.Declined &&
                                    d.LawyerActingFor == LawyerActingFor.Vendor);
            }
            else
            {
                deals =
                    GetAll()
                        .Include(d => d.tblDealScope.tblFundingDeals)
                        .Where(d => d.DealScopeID == dealscopeID && d.Status != DealStatus.Declined &&
                                              (d.LawyerActingFor ==
                                               LawyerActingFor.Purchaser ||
                                               d.LawyerActingFor ==
                                               LawyerActingFor.Both ||
                                               d.LawyerActingFor ==
                                               LawyerActingFor.Mortgagor));
            }

            if (deals.Count() > 1)
                deal = deals.SingleOrDefault(d => d.Status == DealStatus.Active);
            else if (deals.Count() == 1)
                deal = deals.SingleOrDefault();
            else
                deal = null;

            if (deal != null)
            {
                var tblFundingDeal = deal.tblDealScope.tblFundingDeals.FirstOrDefault();
                var fundedDeal = _fundedMapper.MapToData(tblFundingDeal);
                var details = new DealDetails() {DealID = deal.DealID, DealState = fundedDeal, DealStatus = deal.Status};
                return details;
            }
            return null;
        }

        public void UpdateFundingDealPayoutSent(int dealID, System.DateTime payoutSentDate)
        {
            int? dealScopeId = _context.tblDeals.First(deal => deal.DealID == dealID).DealScopeID;
            tblFundingDeal fundingDeal = _context.tblFundingDeals.First(f => f.DealScopeID == dealScopeId.Value);
            fundingDeal.PayoutSent = payoutSentDate;

            if (fundingDeal.Disbursed == null)
            {
                Exception e =
                    new Exception(
                        "Deal not Disbursed exception - Payout Sent can only be set on a funding deal after it has disbursed");
                e.Source = "FCT.LLC.BusinessService";
                throw e;
            }

            _context.SaveChanges();
            return;
        }

        public void UpdateDealToLLC(LLCDeal deal)
        {

            var entity = GetAll().SingleOrDefault(d => d.DealID == deal.DealID);
            _context.Entry(entity).Property(d => d.BusinessModel).CurrentValue = deal.BusinessModel;
            _context.Entry(entity).Property(d => d.Status).CurrentValue = deal.Status;
            _context.Entry(entity).Property(d => d.StatusDate).CurrentValue = DateTime.Now;
            _context.Entry(entity).Property(d => d.StatusReason).CurrentValue = deal.StatusReason;
            _context.Entry(entity).Property(d => d.StatusReasonID).CurrentValue = deal.StatusReasonID;
            _context.Entry(entity).Property(d => d.LawyerActingFor).CurrentValue = deal.ActingFor;

            if(!deal.BusinessModel.Contains(BusinessModel.MMS))
            {
                _context.Entry(entity).Property(d => d.DealScopeID).CurrentValue = null;
            }

            Update(entity);
            _context.SaveChanges();
        }

        public void UpdateFundingDealAssignedTo(int dealID, string assignedTo)
        {
            int? dealScopeId = _context.tblDeals.First(deal => deal.DealID == dealID).DealScopeID;
            tblFundingDeal fundingDeal = _context.tblFundingDeals.First(f => f.DealScopeID == dealScopeId.Value);
            fundingDeal.AssignedTo = assignedTo;
            _context.SaveChanges();
            return;
        }

        public FundingDeal GetFundingDeal(int fundingDealId, bool lawyerActingForVendor)
        {
            var fundedDeal =
                _context.tblFundingDeals.Include(d => d.DealScope).FirstOrDefault(d => d.FundingDealID == fundingDealId);

            if (fundedDeal != null)
            {
                tblDeal deal;
                if (lawyerActingForVendor)
                {
                    deal = GetAll()
                        .SingleOrDefault(
                            d =>
                                d.DealScopeID == fundedDeal.DealScopeID &&
                                d.LawyerActingFor == LawyerActingFor.Vendor);
                }
                else
                {
                    deal = GetAll()
                        .SingleOrDefault(
                            d => d.DealScopeID == fundedDeal.DealScopeID && (d.LawyerActingFor ==
                                                                             LawyerActingFor.Purchaser ||
                                                                             d.LawyerActingFor ==
                                                                             LawyerActingFor.Both ||
                                                                             d.LawyerActingFor==
                                                                             LawyerActingFor.Mortgagor));
                }

                if (deal != null)
                {
                    var fundingDeal = GetFundingDeal(deal.DealID);
                    return fundingDeal;
                }
            }
            return null;
        }

        public FundingDeal InsertFundingDeal(FundingDeal deal, int dealScopeId)
        {
            try
            {
                tblDeal entity = _dealMapper.MapToEntity(deal);
                entity.DealScopeID = dealScopeId;
                entity.RFIReceiveDate = DateTime.Now;
                entity.StatusDate = DateTime.Now;
                if (entity.Status == DealStatus.Active)
                {
                    entity.LawyerDeclinedFlag = false;
                    entity.LawyerAcceptDeclinedDate = DateTime.Now;
                }
                Insert(entity);
                _context.SaveChanges();
                var data = _dealMapper.MapToData(entity);
                return data;
            }

            catch (DbEntityValidationException dbEx)
            {
                TraceExceptionInDebugMode(dbEx);
            }
            catch (DbUpdateException ex)
            {
                LogException(ex);

            }
            return null;
        }

        private void LogException(DbUpdateException ex)
        {
            if (ex.InnerException is UpdateException)
            {
                _logger.LogError(ex.InnerException);
                throw new DataAccessException() {BaseException = ex.InnerException};
            }
            if (ex.InnerException.InnerException is SqlException)
            {
                _logger.LogError(ex.InnerException.InnerException);
                throw new DataAccessException() {BaseException = ex.InnerException.InnerException};
            }
            _logger.LogError(ex, "There was a problem with the database.");
        }

        [ConditionalAttribute("DEBUG")]
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

        public FundingDeal UpdateFundingDeal(FundingDeal deal, int dealScopeId)
        {
            //TODO: Benny: This needs to be revisited as it overwrites all the columns in the tblDeal 
            try
            {
                tblDeal entity = _dealMapper.MapToEntity(deal);
                entity.DealScopeID = dealScopeId;
                var dbDeal = _context.tblDeals.AsNoTracking().FirstOrDefault(d => d.DealID == entity.DealID);
                if (dbDeal != null)
                {
                    entity.FCTRefNum = dbDeal.FCTRefNum;
                    entity.RFIReceiveDate = dbDeal.RFIReceiveDate;
                    entity.StatusDate = dbDeal.StatusDate;
                    string dealStatus = dbDeal.Status;
                    if (dealStatus != entity.Status && entity.Status == DealStatus.Active)
                    {
                        entity.LawyerDeclinedFlag = false;
                        entity.LawyerAcceptDeclinedDate = DateTime.Now;
                    }
                    else if (dealStatus == DealStatus.Active)
                    {
                        entity.LawyerDeclinedFlag = dbDeal.LawyerDeclinedFlag;
                        entity.LawyerAcceptDeclinedDate = dbDeal.LawyerAcceptDeclinedDate;
                    }
                }
                Update(entity);
                _context.SaveChanges();
                var data = _dealMapper.MapToData(entity);
                return data;
            }

            catch (DbEntityValidationException dbEx)
            {
                TraceExceptionInDebugMode(dbEx);
            }
            catch (DbUpdateException ex)
            {
                LogException(ex);

            }
            return null;
        }

        public void UpdateDealStatus(string newStatus, int dealID)
        {
            var deal = _context.tblDeals.SingleOrDefault(d => d.DealID == dealID);
            if (deal != null)
            {
                _context.Entry(deal).Property(d => d.Status).CurrentValue = newStatus;
                _context.Entry(deal).Property(d => d.StatusDate).CurrentValue = DateTime.Now;
            }
            _context.SaveChanges();
        }

        public string GetStatus(int dealId)
        {
            var connection = _context.Database.Connection;
            var deal = GetAll().SingleOrDefault(d => d.DealID == dealId);
            if (deal != null)
            {
                return deal.Status;
            }
            return null;
        }

        public void DeleteFundingDeal(int dealId, bool autoTriggered)
        {
            if (autoTriggered)
            {
                var deal = _context.tblDeals.SingleOrDefault(d => d.DealID == dealId);
                Delete(deal);
                _context.SaveChanges();
            }
            else
            {
                ExecuteQuery("dbo.sp_EasyFund_DeleteDraftDealsById @p0", dealId); 
            }
            
        }

        public FundingDeal GetFundingDealForDisbursement(int dealId)
        {
            var deal = GetAll().Include(d => d.tblProperties).SingleOrDefault(d => d.DealID == dealId);
            if (deal != null)
            {
                return _dealMapper.MapToData(deal);
            }
            return null;
        }

        public void UpdateLLCDeal(FundingDeal llcDeal, int dealScopeID)
        {
            tblDeal deal = GetAll().Single(x => x.DealID == llcDeal.DealID);
            _context.Entry(deal).Property(d => d.DealScopeID).CurrentValue = dealScopeID;
            _context.Entry(deal).Property(d => d.BusinessModel).CurrentValue = llcDeal.BusinessModel;
            _context.Entry(deal).Property(d => d.LawyerActingFor).CurrentValue = llcDeal.ActingFor;
            _context.Entry(deal).Property(d => d.DealType).CurrentValue = llcDeal.DealType;
            Update(deal);
            _context.SaveChanges();
        }

        public int GetExistingDeal(FundingDeal deal, int dealScopeID)
        {
            var existingdeal=GetAll()
                .SingleOrDefault(
                    d =>
                        d.LawyerActingFor == deal.ActingFor && d.DealScopeID == dealScopeID &&
                        d.Status == deal.DealStatus && d.BusinessModel==deal.BusinessModel);
            if (existingdeal != null)
            {
                return existingdeal.DealID;
            }
            return 0;
        }

        public void UpdateOtherLawyer(Lawyer updatedLawyer, int otherDealId)
        {
            var dbDeal = _context.tblDeals.SingleOrDefault(d => d.DealID == otherDealId);
            if (dbDeal != null && dbDeal.LawyerID != updatedLawyer.LawyerID)
            {
                dbDeal.LawyerID = updatedLawyer.LawyerID;
            }
            _context.SaveChanges();
        }

        public string GetLawyerActingFor(int dealID)
        {
            var tblDeal=_context.tblDeals.AsNoTracking().SingleOrDefault(d => d.DealID == dealID);
            if (tblDeal != null)
            {
                return tblDeal.LawyerActingFor;
            }
            return string.Empty;
        }

        public void RemoveDealFromScope(int dealId)
        {
            tblDeal deal = GetAll().Single(x => x.DealID == dealId);
            _context.Entry(deal).Property(d => d.DealScopeID).CurrentValue = null;
            Update(deal);
            _context.SaveChanges();
        }

        public void UpdateOtherLawyerInfoFromDoProcess(FundingDeal fundingDeal, int dealScopeId)
        {
            tblFundingDeal curFundingDeal = _context.tblFundingDeals.SingleOrDefault(d => d.DealScopeID == dealScopeId);

            if ((curFundingDeal != null) && (fundingDeal != null) && (fundingDeal.OtherLawyer != null))
            {
                curFundingDeal.OtherLawyerFirmName = fundingDeal.OtherLawyer.LawFirm;
                curFundingDeal.OtherLawyerFirstName = fundingDeal.OtherLawyer.FirstName;
                curFundingDeal.OtherLawyerLastName = fundingDeal.OtherLawyer.LastName;
                
                _context.SaveChanges();
            }

            return;
        }

        public void UpdateDealContact(int dealId, int contactId)
        {
            tblDeal entity = GetAll().Single(d => d.DealID == dealId);

            if (entity.PrimaryDealContactID != contactId)
            {
                _context.Entry(entity).Property(d => d.PrimaryDealContactID).CurrentValue = contactId;
                Update(entity);
                _context.SaveChanges();
            }
        }

        public void UpdateConfirmClosing(int dealID, bool isConfirmed)
        {
            int? dealScopeId = _context.tblDeals.First(deal => deal.DealID == dealID).DealScopeID;
            var deals = _context.tblDeals
                                .Where(d => d.DealScopeID == dealScopeId.Value && d.Status == DealStatus.Active)
                                .ToList();
            
            if (deals.Count > 0)
            {
                deals.ForEach(deal => deal.IsLawyerConfirmedClosing = isConfirmed);
                _context.SaveChanges();
            }

            return;
        }

        public tblDealScope GetFundingDealByFCTURN(string fctUrn)
        {
            return _context.tblDealScopes.AsNoTracking()
                    .Where(d => d.FCTRefNumber == fctUrn)
                    .Include(d => d.tblDeals.Select(dl => dl.tblLawyer))
                    .SingleOrDefault();
        }


    }
}
