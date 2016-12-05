using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public sealed class VendorRepository:Repository<tblVendor>, IVendorRepository
    {
        private readonly EFBusinessContext _context;
        private readonly IEntityMapper<tblVendor, Vendor> _vendormapper;
        public VendorRepository(EFBusinessContext context, IEntityMapper<tblVendor, Vendor> vendormapper) : base(context)
        {
            _context = context;
            _vendormapper = vendormapper;
        }

        public VendorCollection InsertVendorRange(IEnumerable<Vendor> vendors, int dealScopeId)
        {
            if (vendors != null)
            {
                var entities = vendors.Select(v => _vendormapper.MapToEntity(v));
                var results = new List<tblVendor>();
                foreach (var tblVendor in entities)
                {
                    tblVendor.DealScopeID = dealScopeId;
                    results.Add(tblVendor);
                }
                InsertRange(results);
                _context.SaveChanges();

                var insertedVendors = results.Select(r => _vendormapper.MapToData(r));
                var collection = new VendorCollection();
                collection.AddRange(insertedVendors);
                return collection;
            }
            return null;
        }

        public void UpdateVendorRange(IEnumerable<Vendor> vendors, int dealScopeId)
        {
            if (vendors != null)
            {
                IEnumerable<Vendor> deletedVendors = GetVendors(dealScopeId).Except(vendors, new VendorEqualityComparer());
                
                if (deletedVendors.Any())
                {
                    IEnumerable<tblVendor> entities = deletedVendors.Select(d => _vendormapper.MapToEntity(d));
                    DeleteRange(entities);
                    _context.SaveChanges();
                }

                _context.Configuration.AutoDetectChangesEnabled = false;

                foreach (var vendor in vendors)
                {
                    if (vendor.VendorID.HasValue)
                    {
                        var entity = _vendormapper.MapToEntity(vendor);
                        entity.DealScopeID = dealScopeId;
                        Update(entity);
                    }
                    else
                    {
                        var entity = _vendormapper.MapToEntity(vendor);
                        entity.DealScopeID = dealScopeId;
                        Insert(entity);
                    }
                }
                _context.Configuration.AutoDetectChangesEnabled = true;
                _context.SaveChanges(); 
            }

        }

        public IEnumerable<Vendor> GetVendors(int dealScopeId)
        {
            var results = GetAll().Where(v => v.DealScopeID == dealScopeId).AsEnumerable();
            return results.Select(r => _vendormapper.MapToData(r));
        }

        public IEnumerable<Vendor> GetVendorsByDeal(int dealId)
        {
           var entities = (from v in _context.tblVendors
                      join d in _context.tblDeals on v.DealScopeID equals d.DealScopeID
                      where d.DealID == dealId
                      select v).ToList();
            return entities.Select(v => _vendormapper.MapToData(v));
        }

        public void DeleteRange(IEnumerable<Vendor> vendors)
        {
            var entities = vendors.Select(v => _vendormapper.MapToEntity(v));

            if (entities.Any())
            {
                DeleteRange(entities);
                _context.SaveChanges();
            }
        }

        private class VendorEqualityComparer : IEqualityComparer<Vendor>
        {
            public bool Equals(Vendor x, Vendor y)
            {
                return x.VendorID == y.VendorID;
            }

            public int GetHashCode(Vendor obj)
            {
                return obj.VendorID.GetValueOrDefault();
            }
        }
  
    }
}
