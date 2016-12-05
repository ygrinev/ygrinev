using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IGlobalizationRepository:IRepository<tblGlobalization>
    {
        DealHistoryEntry GetEntry(string resourcekey);
        DealHistoryEntry GetEntry(string resourceSet, string resourceKey);
        IDictionary<string, DealHistoryEntry> GetEntries(string resourceSet, string resourceType=null);
        DealHistoryEntry GetEntry(int statusReasonId, string resourceSet);
        IDictionary<string, DealHistoryEntry> GetEntries(IEnumerable<string> resourceKeys);
    }
}
