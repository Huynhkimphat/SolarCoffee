using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Customer;
using SolarCoffee.Services.Order;
using SolarCoffee.Web.Serialization;
using SolarCoffee.Web.ViewModels;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<OrderController> _logger;

        private readonly IOrderService _orderService;

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
            order.Customer =
                _customerService.GetCustomerById(invoice.CustomerId);
            _orderService.GenerateOpenOrder(order);
            return Ok();
        }

        [HttpGet("/api/invoice")]
        public ActionResult GetOrders()
        {
            _logger.LogInformation("");
            var orders = _orderService.GetOrders();
            var orderModels = OrderMapper.SerializeOrderToViewModels(orders);
            return Ok(orderModels);
        }

        [HttpPatch("/api/invoice/complete/{id}")]
        public ActionResult MarkOrderComplete(int id)
        {
            _logger.LogInformation($"Making Order {id} Complete... ");
            var response = _orderService.MakeFulfilled(id);
            return Ok(response);
        }
    }
}