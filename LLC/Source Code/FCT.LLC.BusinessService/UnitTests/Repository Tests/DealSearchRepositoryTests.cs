using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.Entities;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestClass]
    public class DealSearchRepositoryTests
    {
        private EFBusinessContext _context;

        //This is an integration test due to Moq's inability to mock Include() method. set DealID to an actual deal ID with dealscope
        [TestMethod]
        public void DealSearch_ByLenderRefNumber()
        {
            _context = new EFBusinessContext();
            DealSearchRepository repository = new DealSearchRepository(_context);
            SearchDealCriteria _searchDealCriteria = new SearchDealCriteria();
            _searchDealCriteria.BatchNumber = "15";//jenrqa1

            UserContext userContext = new UserContext() { UserID = "4432432" };

            OrderBySpecification orderbySpecification = new OrderBySpecification();
            orderbySpecification.OrderByColumn = OrderByColumn.FCTURN;
            orderbySpecification.OrderByDirection = OrderByDirection.ASC;
            OrderBySpecificationCollection orderBys = new OrderBySpecificationCollection();
            orderBys.Add(orderbySpecification);

            int totalRowCount = 0;

            List<vw_Deal> _deals  = new List<vw_Deal>();
            int pageSize = 10;
            _deals = repository.SearchDeal(_searchDealCriteria, orderBys, 1, pageSize, userContext, out totalRowCount);
            if (totalRowCount > pageSize)
                Assert.AreEqual(_deals.Count, totalRowCount);
            else
                Assert.AreEqual(_deals.Count, pageSize);
        }
    }
}
