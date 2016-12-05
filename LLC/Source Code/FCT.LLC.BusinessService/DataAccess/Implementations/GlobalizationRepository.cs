using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;
using FCT.LLC.GenericRepository.Implementations;

namespace FCT.LLC.BusinessService.DataAccess
{
    public class GlobalizationRepository:Repository<tblGlobalization>, IGlobalizationRepository
    {
        private readonly EFBusinessContext _context;
        public GlobalizationRepository(EFBusinessContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public DealHistoryEntry GetEntry(string resourceKey)
        {
            //if resource key is not specified using FCT.LLC.BusinessService as default
            return GetEntry(ResourceSet.FCTLLCBusinessService, resourceKey);
        }

        public DealHistoryEntry GetEntry(string resourceSet, string resourceKey)
        {
            var results =
                GetAll()
                    .Where(r => r.ResourceSet == resourceSet && r.ResourceKey == resourceKey)
                    .Select(o => new { o.Value, o.LocaleID })
                    .ToDictionary(x => x.LocaleID);
            var entry = new DealHistoryEntry
            {
                EnglishVersion = results[LocaleType.EN].Value,
                FrenchVersion = results[LocaleType.FR].Value
            };

            return entry;
        }

        public IDictionary<string, DealHistoryEntry> GetEntries(string resourceSet, string resourceType = null)
        {
            var dealHistories = new Dictionary<string, DealHistoryEntry>();
            IEnumerable<tblGlobalization> results;

            if (!string.IsNullOrEmpty(resourceType))
            {
                results =
                    GetAll()
                        .Where(r => r.Type == resourceType)
                        .Select(o => new {o.Value, o.LocaleID, o.ResourceKey}).ToList().Select(
                            o =>
                                new tblGlobalization()
                                {
                                    Value = o.Value,
                                    LocaleID = o.LocaleID,
                                    ResourceKey = o.ResourceKey,
                                });
            }
            else
            {
                results =
                    GetAll()
                        .Where(r => r.ResourceSet == resourceSet)
                        .Select(o => new {o.Value, o.LocaleID, o.ResourceKey}).ToList().Select(
                            o =>
                                new tblGlobalization()
                                {
                                    Value = o.Value,
                                    LocaleID = o.LocaleID,
                                    ResourceKey = o.ResourceKey,
                                });
                
            }


            var englishversions = results.Where(r => r.LocaleID == LocaleType.EN);
            var frenchversions = results.Where(r => r.LocaleID == LocaleType.FR);

            foreach (var englishversion in englishversions)
            {
                var entry = new DealHistoryEntry()
                {
                    EnglishVersion = englishversion.Value,
                    ResourceKey = englishversion.ResourceKey
                };
                dealHistories.Add(englishversion.ResourceKey, entry);
            }
            foreach (var frenchversion in frenchversions)
            {
                dealHistories[frenchversion.ResourceKey].FrenchVersion = frenchversion.Value;
            }
            return dealHistories;
        }

        public DealHistoryEntry GetEntry(int statusReasonId, string resourceSet)
        {
            var results =
                _context.tblGlobalizations.Where(
                    g =>
                        g.ResourceKey ==
                        _context.tblStatusReasons.FirstOrDefault(s => s.StatusReasonID == statusReasonId).Reason
                        && g.ResourceSet == resourceSet && !string.IsNullOrEmpty(g.LocaleID))
                    .Select(o => new {o.Value, o.LocaleID})
                    .ToDictionary(x => x.LocaleID);
            var entry = new DealHistoryEntry
            {
                EnglishVersion = results[LocaleType.EN].Value,
                FrenchVersion = results[LocaleType.FR].Value
            };
            return entry;
        }

        public IDictionary<string, DealHistoryEntry> GetEntries(IEnumerable<string> resourceKeys)
        {
            var dealHistories = new Dictionary<string, DealHistoryEntry>();
            var results =
                (from g in _context.tblGlobalizations.AsQueryable() where resourceKeys.Contains(g.ResourceKey) select g)
                .Select(o => new {o.Value, o.LocaleID, o.ResourceKey}).ToList();

            var englishversions = results.Where(r => r.LocaleID == LocaleType.EN);
            var frenchversions = results.Where(r => r.LocaleID == LocaleType.FR);
            foreach (var englishversion in englishversions)
            {
                var entry = new DealHistoryEntry()
                {
                    EnglishVersion = englishversion.Value,
                    ResourceKey = englishversion.ResourceKey
                };
                dealHistories.Add(englishversion.ResourceKey, entry);
            }
            foreach (var frenchversion in frenchversions)
            {
                dealHistories[frenchversion.ResourceKey].FrenchVersion = frenchversion.Value;
            }
            return dealHistories;
        }
    }
}
