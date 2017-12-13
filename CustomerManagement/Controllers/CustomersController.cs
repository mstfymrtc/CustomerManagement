using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerManagement.Data;
using CustomerManagement.Infrastructure;
using CustomerManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomerManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {

        private ICustomersRepository _customersRepository;

        private ILogger _logger;

        public CustomersController(ILoggerFactory loggerFactory, ICustomersRepository customersRepository)
        {
            _logger = loggerFactory.CreateLogger(nameof(CustomersController));
            _customersRepository = customersRepository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Customer>),200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> Customers()
        {

            try
            {
                var customers = await _customersRepository.GetCustomersAsync();
                return Ok(customers);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ApiResponse{Status = false});
            }
        }
    }
}
