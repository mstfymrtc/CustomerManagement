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

        [HttpGet("{id}",Name="GetCustomerRoute")]
        [ProducesResponseType(typeof(List<Customer>), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> Customers(int id)
        {

            try
            {
                var customer = await _customersRepository.GetCustomerByIdAsync(id);
                return Ok(customer);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<Customer>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
            //gelen, json olarak geleceği için frombody yazdık.
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse {Status = false, ModelState = ModelState});
            }
            try
            {
                var newCustomer = await _customersRepository.InsertCustomerAsync(customer);

                if (newCustomer==null)
                {
                    return BadRequest(new ApiResponse { Status = false});

                }
                return CreatedAtRoute("GetCustomerRoute", new {id = newCustomer.Id},
                    new ApiResponse {Status =true, Customer = newCustomer});
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ApiResponse {Status = false});
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> UpdateCustomer([FromBody]  Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse { Status = false, ModelState = ModelState });
            }
            try
            {
                
                var result = await _customersRepository.UpdateCustomerAsync(customer);
                if (!result)
                {
                    return BadRequest(new ApiResponse { Status = false });

                }
                return Ok(new ApiResponse { Status = true, Customer = customer });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            
            try
            {

                var result = await _customersRepository.DeleteCustomerAsync(id);
                if (!result)
                {
                    return BadRequest(new ApiResponse { Status = false });

                }
                return Ok(new ApiResponse { Status = true});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new ApiResponse { Status = false });
            }
        }


    }
}
