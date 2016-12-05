using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class PINRepository : Repository<tblPIN>, IPINRepository
    {
        private readonly EFBusinessContext _context;
        private readonly IEntityMapper<tblPIN, Pin> _pinMapper; 
        public PINRepository(EFBusinessContext context, IEntityMapper<tblPIN, Pin> pinMapper ) : base(context)
        {
            _context = context;
            _pinMapper = pinMapper;
        }

        public PinCollection InsertPINRange(IEnumerable<Pin> pins, int propertyID)
        {
            if(pins!=null)
            {
                var entities = pins.Select(p => _pinMapper.MapToEntity(p));
                var results = new List<tblPIN>();

                foreach (var entity in entities)
                {
                    entity.PropertyID = propertyID;
                    if (entity.PINID > 0)
                    {
                        entity.SourceID = entity.PINID;
                    }
                    results.Add(entity);
                }
                InsertRange(results);
                _context.SaveChanges();

                var insertedPins = results.Select(r => _pinMapper.MapToData(r));
                var collection = new PinCollection();
                collection.AddRange(insertedPins);
                return collection;
            }
            return null;
        }

        public void UpdatePINRange(IEnumerable<Pin> pins, int propertyID)
        {
            if (pins != null)
            {
                var requestedpins = pins.Select(p => _pinMapper.MapToEntity(p));
                var deletedPins = GetPINs(propertyID).Except(requestedpins, new PinEqualityComparer());
                DeleteRange(deletedPins);

                _context.SaveChanges();

                _context.Configuration.AutoDetectChangesEnabled = false;

                foreach (var pin in pins)
                {

                    if (pin.PinID.HasValue)
                    {
                        var entity = _pinMapper.MapToEntity(pin);
                        entity.PropertyID = propertyID;
                        Update(entity);
                    }
                    else
                    {
                        var entity = _pinMapper.MapToEntity(pin);
                        entity.PropertyID = propertyID;
                        Insert(entity);
                    }
                }
                _context.SaveChanges();
                _context.Configuration.AutoDetectChangesEnabled = true;  
            }
           
        }

        public void UpdatePINRangeForOtherDeal(IEnumerable<Pin> pins, int propertyID)

        {
            if (pins != null)
            {
                IEnumerable<tblPIN> requestedpins = pins.Select(p => _pinMapper.MapToEntity(p));
                IEnumerable<tblPIN> existingpins = GetPINs(propertyID);
                IEnumerable<tblPIN> deletedPins = existingpins.Where(e => requestedpins.All(r => r.PINID != e.SourceID));
                if (deletedPins.Any())
                {
                    DeleteRange(deletedPins);

                }

                IEnumerable<tblPIN> addedPins = requestedpins.Where(r => existingpins.All(e => e.SourceID != r.PINID));
                if (addedPins.Any())
                {
                    foreach (var addedPin in addedPins)
                    {
                        tblPIN pin = new tblPIN();
                        pin.PropertyID = propertyID;
                        pin.PINNumber = addedPin.PINNumber;
                        pin.SourceID = addedPin.PINID;
                        Insert(pin);
                    }
                }

                _context.Configuration.AutoDetectChangesEnabled = false;

                IEnumerable<tblPIN> updatedPins = requestedpins.Where(r => existingpins.Any(e => e.SourceID == r.PINID));

                foreach (var updatedPin in updatedPins)
                {
                    tblPIN existingPin = existingpins.SingleOrDefault(e => e.SourceID == updatedPin.PINID);
                    if (existingPin.PINNumber != updatedPin.PINNumber)
                    {
                        existingPin.PINNumber = updatedPin.PINNumber;
                        Update(existingPin);
                    }
                }
                _context.SaveChanges();
                _context.Configuration.AutoDetectChangesEnabled = true;    
            }
           
        }

        public IEnumerable<tblPIN> GetPINs(int propertyID)
        {
            var results = GetAll().Where(v => v.PropertyID == propertyID).AsEnumerable();
            return results;
        }

        private class PinEqualityComparer: IEqualityComparer<tblPIN>
        {
            public bool Equals(tblPIN x, tblPIN y)
            {
                return x.PINID == y.PINID;
            }

            public int GetHashCode(tblPIN obj)
            {
                return obj.PINID.GetHashCode();
            }
        }

        public void DeleteAndInsertPins(int PropertyID, int FromPropertyID)
        {
            var PinsTobeDeleted = GetPINs(PropertyID);

            var PinsTobeInserted = GetPINs(FromPropertyID);

            if (PinsTobeDeleted.Any())
            {
                DeleteRange(PinsTobeDeleted);
                _context.SaveChanges();
            }
                        
            _context.Configuration.AutoDetectChangesEnabled = false;

            foreach (var pin in PinsTobeInserted)
            {
                pin.PropertyID = PropertyID;
                Insert(pin);
            }
            _context.SaveChanges();
            _context.Configuration.AutoDetectChangesEnabled = true;    
        }

    }
}
