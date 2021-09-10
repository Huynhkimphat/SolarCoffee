using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Customer;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;
using System;
using System.Linq;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase 
    {
        private readonly ILogger<CustomerController> _logger;

        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger,ICustomerService customerService)
		{
            _logger = logger;
            _customerService = customerService;
		}

        [HttpPost("/api/customer")]
        public ActionResult CreateCustomer([FromBody] CustomerModel customer)
		{
            _logger.LogInformation("Creating a new customer");
            customer.CreateOn = DateTime.UtcNow;
            customer.UpdateOn = DateTime.UtcNow;
            var customerData = CustomerMapper.SerializeCustomer(customer);
            var newCustomer = _customerService.CreateCustomer(customerData);
            return Ok(newCustomer);
        }

        [HttpGet("/api/customer")]
        public ActionResult GetCustomers()
		{
            _logger.LogInformation("Getting customers");
            var customers = _customerService.GetAllCustomers();
            var customerModels = customers
                .Select(customer => new CustomerModel
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    PrimaryAddress = CustomerMapper.MapCustomerAddress(customer.PrimaryAddress),
                    CreateOn = customer.CreateOn,
                    UpdateOn = customer.UpdateOn
                })
                .OrderByDescending(customer => customer.CreateOn)
                .ToList();
            return Ok(customerModels);
		}

        [HttpDelete("/api/customer/{id}")]
        public ActionResult DeleteCustomer(int id)
		{
            _logger.LogInformation("Deleting a customer");
            var response = _customerService.DeleteCustomer(id);
            return Ok(response);
		}
    }
}