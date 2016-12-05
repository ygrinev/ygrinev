using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using WebAPIODataWithBreezeConsumer.Models;

namespace WebAPIODataWithBreezeConsumer.ODataControllers
{
    public class CustomersController : EntitySetController<Customer,string>
    {
        NorthwindDbContext _Context = new NorthwindDbContext();

        [Queryable]
        public override IQueryable<Customer> Get()
        {
            return _Context.Customers;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _Context.Dispose();
        }
    }
}