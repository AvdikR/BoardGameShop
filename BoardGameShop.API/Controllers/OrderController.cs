using BoardGameShop.Application.DTOs;
using BoardGameShop.Application.Interfaces;
using BoardGameShop.Application.UseCases.Orders;
using Microsoft.AspNetCore.Mvc;

namespace BoardGameShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly CreateOrderCommandHandler _createOrderHandler;

        public OrderController(IOrderService service, CreateOrderCommandHandler createOrderHandler)
        {
            _service = service;
            _createOrderHandler = createOrderHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _service.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderDto dto)
        {
            // Тепер процес створення керується через Command Handler
            var orderId = await _createOrderHandler.Handle(dto);
            return Ok(new { OrderId = orderId });
        }
    }
}
