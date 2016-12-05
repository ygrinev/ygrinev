using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebAPIODataWithBreezeConsumer.Models
{
    public class NorthwindDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
    }
}