using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagement.Data
{
    public class CustomerDbContext:DbContext
    {

        public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
        {
            
        }
        

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<State> States { get; set; }

    }
}
