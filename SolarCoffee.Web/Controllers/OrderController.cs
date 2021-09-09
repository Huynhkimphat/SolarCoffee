using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Customer;
using SolarCoffee.Services.Order;
using SolarCoffee.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SolarCoffee.Web.Serialization;

namespace SolarCoffee.Web.Controllers
{
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly ILogger<OrderController> _logger;

		private readonly IOrderService _orderService;

		private readonly ICustomerService _customerService;

		public OrderController(
			ILogger<OrderController> logger,
			IOrderService orderService,
			ICustomerService customerService
			)
		{
			_logger = logger;
			_orderService = orderService;
			_customerService = customerService;
		}

		[HttpPost("/api/invoice")]
		public ActionResult GenerateNewOrder([FromBody] InvoiceModel invoice)
		{
			_logger.LogInformation("Generating invoice");
			var order = OrderMapper.SerializeInvoiceToOrder(invoice);
			order.Customer = _customerService.GetCustomerById(invoice.CustomerId);
			return Ok();
		}
	}
}
