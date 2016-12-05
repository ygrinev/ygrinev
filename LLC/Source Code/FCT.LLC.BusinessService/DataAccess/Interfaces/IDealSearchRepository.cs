using FCT.LLC.BusinessService.Entities;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.GenericRepository.Contracts;
using System.Collections.Generic;

namespace FCT.LLC.BusinessService.DataAccess
{
    public interface IDealSearchRepository: IRepository<vw_Deal>
    {
        List<vw_Deal> SearchDeal(SearchDealCriteria searchDealCriteria, OrderBySpecificationCollection orderBySpecifications, int pageIndex, int pageSize, UserContext usercontext, out int totalRowCount);
    }
}
