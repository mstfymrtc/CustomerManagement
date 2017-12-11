using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CustomerManagement.Data
{
    public class StatesRepository : IStatesRepository
    {
        private CustomerDbContext _context;
        private ILogger _Logger; 

        public StatesRepository(CustomerDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _Logger = loggerFactory.CreateLogger("StatesRepository");
        }
        
        public async Task<List<State>> GetStatesAsync()
        {
            return await _context.States.OrderBy(c => c.Abbreviation).ToListAsync();
        }
    }
}