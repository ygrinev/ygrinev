using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCT.LLC.BusinessService.BusinessLogic;
using FCT.LLC.BusinessService.DataAccess;
using FCT.LLC.Common.DataContracts;
using Moq;
using NUnit.Framework;
using FCT.LLC.BusinessService;
using FCT.LLC.BusinessService.Entities;

namespace FCT.LLC.BusinessService.UnitTests
{
    [TestClass]
    public class DealSearchTest
    {
        [TestMethod]
        public void SearchDealTest()
        {            
            var mockDealSearch = new Mock<IDealSearchRepository>();
                        
            SearchDealRequest request = new SearchDealRequest();
            SearchDealCriteria _searchDealCriteria = new SearchDealCriteria();
            _searchDealCriteria.LenderReferenceNumber = "1";//jenrqa1

            request.SearchDealCriteria = _searchDealCriteria;
            request.PageSize = 10;
            request.PageIndex = 1;
            request.UserContext = new UserContext() { UserID = "test" };

            OrderBySpecification orderbySpecification = new OrderBySpecification();
            orderbySpecification.OrderByColumn = OrderByColumn.FCTURN;
            orderbySpecification.OrderByDirection = OrderByDirection.ASC;

            request.OrderBySpecifications = new OrderBySpecificationCollection();
            request.OrderBySpecifications.Add(orderbySpecification);
            int totalRowCount = 0;

            List<vw_Deal> _deals  = new List<vw_Deal>();
            mockDealSearch.Setup(f => f.SearchDeal(_searchDealCriteria, request.OrderBySpecifications, 1, 10, new UserContext() { UserID = "test" }, out totalRowCount)).Returns(_deals);
            
            var s = new DealSearchBusinessLogic(mockDealSearch.Object);
            //var searchDeal = new LLCBusinessService();
            var r = s.SearchDeal(request);


            ////var result = searchDeal.SearchDeal(request);
            //var r = s.SearchDeal(request);
            
            
            //Assert.IsTrue(result.TotalRowsCount >= 1);
        }
    }
}
