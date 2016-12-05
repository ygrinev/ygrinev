using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IReconciliationItemsRepository : IRepository<vw_ReconciliationItem>
    {
        GetReconciliationWorklistResponse GetReconciliationItems(GetReconciliationWorklistRequest request);
    }
}
