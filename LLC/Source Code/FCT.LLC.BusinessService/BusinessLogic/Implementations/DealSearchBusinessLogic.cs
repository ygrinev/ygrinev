using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.BusinessService.DataAccess.Mappers;

namespace FCT.LLC.BusinessService.BusinessLogic
{
    public class DealSearchBusinessLogic : IDealSearchBusinessLogic
    {
        private readonly IDealSearchRepository _dealSearchRepository;

        public DealSearchBusinessLogic(IDealSearchRepository dealSearchRepository)
        {
            _dealSearchRepository = dealSearchRepository;
        }

        public SearchDealResponse SearchDeal(SearchDealRequest request)
        {

            int TotalRowCount = 0;
            List<vw_Deal> _deals = _dealSearchRepository.SearchDeal(request.SearchDealCriteria, request.OrderBySpecifications
                , request.PageIndex, request.PageSize, request.UserContext, out TotalRowCount);

            SearchDealResponse searchResult = new SearchDealResponse();
            searchResult.SearchResults = MapDealRelatedEntities.MapForSearchDeal(_deals, request.SearchDealCriteria);
            searchResult.TotalRowsCount = TotalRowCount;
            searchResult.PageIndex = request.PageIndex;

            return searchResult;
        }
    }
}
