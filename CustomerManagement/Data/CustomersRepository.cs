using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Data
{
    public class CustomersRepository : ICustomersRepository
    {
        private CustomerDbContext _context;
        private ILogger _Logger;

        public CustomersRepository(CustomerDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _Logger = loggerFactory.CreateLogger("CustomersRepository");
        }
        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers
                .OrderBy(o => o.LastName)
                .Include(s => s.State)
                .Include(o => o.Orders)
                .ToListAsync();
            //nagigation propertyler için include ile ekstra olarak call yapmak gerek.
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers
                .Include(s => s.State)
                .Include(o => o.Orders)
                .SingleOrDefaultAsync(c => c.CustomerID == id);
        }

        public async Task<Customer> InsertCustomerAsync(Customer customer)
        {
            _context.Add(customer);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _Logger.LogError($"Error in {nameof(InsertCustomerAsync)} :" + ex.Message);
            }
            return customer;
        }

        public async Task<bool> UpdateCustomerAsync(Customer customer)
        {

            _context.Customers.Attach(customer);
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception ex)
            {

                _Logger.LogError($"Error in {nameof(UpdateCustomerAsync)} :" + ex.Message);
            }
            return false;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.Include(o => o.Orders)
                .SingleOrDefaultAsync(c => c.CustomerID == id);

            _context.Remove(customer);

            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);

            }
            catch (Exception ex)
            {

                _Logger.LogError($"Error in {nameof(DeleteCustomerAsync)} :" + ex.Message);
            }
            return false;

        }
    }
}