using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess.Mappers;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class MortgagorRepository : Repository<tblMortgagor>, IMortgagorRepository
    {
        private readonly EFBusinessContext _context;
        private readonly IEntityMapper<tblMortgagor, Mortgagor> _mortgagorMapper;

        public MortgagorRepository(EFBusinessContext context, IEntityMapper<tblMortgagor, Mortgagor> mortgagormapper)
            : base(context)
        {
            _context = context;
            _mortgagorMapper = mortgagormapper;
        }
       
        public MortgagorCollection InsertMortgagorRange(IEnumerable<Mortgagor> mortgagors, int dealId) 
        {
            if (mortgagors != null) 
            {
                IEnumerable<tblMortgagor> entities = mortgagors.Select(m => _mortgagorMapper.MapToEntity(m));
                var results = new List<tblMortgagor>();

                foreach (tblMortgagor entity in entities) 
                {
                    entity.DealID = dealId;
                    results.Add(entity);
                }
                InsertRange(results);
                _context.SaveChanges();

                var insertedMortgagors = results.Select(m => _mortgagorMapper.MapToData(m));
                var coll = new MortgagorCollection();
                coll.AddRange(insertedMortgagors);
                return coll;
            }

            return null;
        }
       
        public void UpdateMorgagorRange(IEnumerable<Mortgagor> mortgagors, int dealId)
        {
            if (mortgagors != null)
            {
                var existingMortgagors = GetMortgagors(dealId);
                IEnumerable<Mortgagor> deletedMortgagors = existingMortgagors.Except(mortgagors,
                    new MortgagorEqualityComparer());

                if (deletedMortgagors.Any())
                {
                    IEnumerable<tblMortgagor> entities = deletedMortgagors.Select(d => _mortgagorMapper.MapToEntity(d));
                    DeleteRange(entities);
                    _context.SaveChanges();
                }

                _context.Configuration.AutoDetectChangesEnabled = false;

                foreach (Mortgagor mortgagor in mortgagors)
                {
                    if (mortgagor.MortgagorID.HasValue)
                    {
                        var entity = _mortgagorMapper.MapToEntity(mortgagor);
                        entity.DealID = dealId;
                        Update(entity);
                    }
                    else
                    {
                        var entity = _mortgagorMapper.MapToEntity(mortgagor);
                        entity.DealID = dealId;
                        Insert(entity);
                    }
                }
                _context.Configuration.AutoDetectChangesEnabled = true;
                _context.SaveChanges();
            }
        }

        public void UpdateMortgagorRangeForOtherDeal(int targetDealId, int srcDealId)
        {
            if (targetDealId < 1 || srcDealId < 1)
                return;
            var srcMortgagors = _context.tblMortgagors.Where(m => m.DealID == srcDealId).ToList();
            var targetMortgagors = _context.tblMortgagors.Where(m => m.DealID == targetDealId).ToList();
            // delete extra mortgagors in target
            DeleteRange(targetMortgagors.Where(tm=>!srcMortgagors.Any(sm=>sm.MortgagorID == tm.SourceID)));
            if(srcMortgagors.Count > 0)
            {
                // Insert not existing mortgagors
                InsertMortgagorRange(srcMortgagors.Where(m => !targetMortgagors.Any(tm=>tm.SourceID == m.MortgagorID)).Select(m =>_mortgagorMapper.MapToData(m)), targetDealId);
                // update values of existing mortgagors for the target deal
                UpdateMortgagorRange(targetMortgagors, srcMortgagors);
                _context.SaveChanges();
            }

            /*Naga efcode
                    var addedMortgagorList = (from mortgagor in requestedMortgagors
                        where existingMortgagorsWithSource.All(e => e.SourceID != mortgagor.MortgagorID)
                        select mortgagor);

                    if (addedMortgagorList.Any())
                    {
                        var otherDealMortgagors =
                            _context.tblMortgagors.Where(m => m.DealID == otherDealId).AsEnumerable();
                        InsertMortgagorRange(addedMortgagorList, dealId, otherDealMortgagors);
                    }
             * */
        }

        /// <summary>
        /// Copy all the Mortgagors from Source deal to Destination Deal
        /// </summary>
        /// <param name="srcMortgagors"></param>
        /// <param name="targetDealId"></param>
        /// <param name="srcDealId"></param>
        public void InsertMortgagorRangeForOtherDeal(IEnumerable<Mortgagor> srcMortgagors, int targetDealId, int srcDealId)
        {

            if (srcMortgagors != null)
            {
                IEnumerable<tblMortgagor> mortgagorEntities = _context.tblMortgagors.Where(m => m.DealID == srcDealId);

                if (mortgagorEntities.Any())
                {
                    //mortgagorEntities = srcMortgagors.Select(m => _mortgagorMapper.MapToEntity(m));
                    InsertMortgagorRange(mortgagorEntities, targetDealId);

                    _context.SaveChanges();
                }
            }

        }


        /*
         * Naga efcode

        public MortgagorCollection InsertMortgagorRange(IEnumerable<Mortgagor> mortgagors, int dealId) {
            if (mortgagors != null) {
                IEnumerable<tblMortgagor> entities = mortgagors.Select(m => _mortgagorMapper.MapToEntity(m));
                var results = new List<tblMortgagor>();

                foreach (tblMortgagor entity in entities) {
                    entity.DealID = dealId;
                    results.Add(entity);
                }
                InsertRange(results);
                _context.SaveChanges();

                var insertedMortgagors = results.Select(m => _mortgagorMapper.MapToData(m));
                var coll = new MortgagorCollection();
                coll.AddRange(insertedMortgagors);
                return coll;
            }

            return null;
        }

       
         * 
         * private void InsertMortgagorRange(IEnumerable<tblMortgagor> mortgagors, int dealId, IEnumerable<tblMortgagor> otherDealMortgagors)
        
         {
         *  {
                          mortgagorEntities =
                              (from m in _context.tblMortgagors where mortgagorIds.Contains(m.MortgagorID) select m)
                                  .AsEnumerable();
               

                      if (otherDealMortgagors.Any())
                      {
                          InsertMortgagorRange(mortgagorEntities, dealId, otherDealMortgagors);

                          _context.SaveChanges();
                      }
             
              }
         * 
         */
        private void InsertMortgagorRange(IEnumerable<tblMortgagor> targetMortgagors, int targetDealId, IEnumerable<tblMortgagor> srcDealMortgagors)
        {       
                  var requestedMorgagors = new List<tblMortgagor>();
                  foreach (var srcDealMortgagor in srcDealMortgagors.ToList())
                  {
                      foreach (var targetMortgagor in targetMortgagors.ToList())
                      {
                          //Add all mortgagors existing in BOTH:  sent list and source deal
                          if (MortgagorEquals(targetMortgagor, srcDealMortgagor))
                          {
                              var entity = MortgagorMapper.SyncEntities(targetMortgagor);
                              entity.MortgagorID = 0;
                              entity.DealID = targetDealId;
                              entity.SourceID = srcDealMortgagor.MortgagorID;
                              requestedMorgagors.Add(entity);
                          }
                      }
                  }
                  InsertRange(requestedMorgagors);
              }

         
        private void InsertMortgagorRange(IEnumerable<tblMortgagor> mortgagors, int targetDealId)
        {
            var requestedMorgagors = new List<tblMortgagor>();
            foreach (var mortgagor in mortgagors.ToList())
            {
                //Add mortgagor when a matching record does not exist
                if (mortgagor != null)
                {
                    var entity = MortgagorMapper.SyncEntities(mortgagor);
                    entity.MortgagorID = 0;
                    entity.DealID = targetDealId;
                    entity.SourceID = mortgagor.MortgagorID;
                    requestedMorgagors.Add(entity);
                }
            }
            InsertRange(requestedMorgagors);
        }
        private void UpdateMortgagorRange(IEnumerable<tblMortgagor> targetMortgagors, IEnumerable<tblMortgagor> srcMortgagors)
        {
            targetMortgagors.Where(tm => srcMortgagors.Any(sm => sm.MortgagorID == tm.SourceID)).ToList().
            ForEach(m=>
            {
                tblMortgagor smUpd = srcMortgagors.First(sm => m.SourceID == sm.MortgagorID);
                ShallowCopyData<tblMortgagor>(m, smUpd, new string[] { "MortgagorID", "SourceID", "DealID" });
                Update(smUpd);
            });
        }
        public static void ShallowCopyData<T>(T to, T from, string[] exclude) where T : class
        {
            typeof(T).GetProperties().Where(p => !exclude.Any(n => string.Compare(n, p.Name, true) == 0))
                .ToList().ForEach(pr =>
                {
                    typeof(T).GetProperty(pr.Name).SetValue(to, typeof(T).GetProperty(pr.Name).GetValue(from));
                });
        }
        public IEnumerable<Mortgagor> GetMortgagors(int dealId)
        {
            var results = GetAll().Where(m => m.DealID == dealId).AsEnumerable();
            return results.Select(r => _mortgagorMapper.MapToData(r));
        }

        private class MortgagorEqualityComparer : IEqualityComparer<Mortgagor>
        {
            public bool Equals(Mortgagor x, Mortgagor y)
            {
                return x.MortgagorID == y.MortgagorID;
            }

            public int GetHashCode(Mortgagor obj)
            {
                return obj.MortgagorID.GetValueOrDefault();
            }
        }


        private bool MortgagorEquals(tblMortgagor x, tblMortgagor y)
        {
/* Easyfund logic return x.MortgagorType == y.MortgagorType && x.FirstName == y.FirstName && x.LastName == y.LastName &&
                   x.CompanyName == y.CompanyName && x.StreetNumber == y.StreetNumber &&
                   x.UnitNumber == y.UnitNumber && x.PostalCode == y.PostalCode && x.Country == y.Country && x.LenderMortgagorID==y.LenderMortgagorID;
*/
            return x.MortgagorID > 0 && y.MortgagorID > 0 &&  x.MortgagorID.Equals(y.MortgagorID);
        }

    }
}
