using Microsoft.AspNetCore.Mvc;
using TrendbolAPI.Models;
using TrendbolAPI.Models.DTOs;
using TrendbolAPI.Services.Interfaces;

namespace TrendbolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            var orderDtos = orders.Select(o => new OrderResponseDTO
            {
                Id = o.Id,
                UserId = o.UserId,
                UserName = $"{o.User.FirstName} {o.User.LastName}",
                ProductId = o.ProductId,
                ProductName = o.Product.Name,
                Quantity = o.Quantity,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt
            });
            return Ok(orderDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound($"ID'si {id} olan sipariş bulunamadı.");

            var orderDto = new OrderResponseDTO
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = $"{order.User.FirstName} {order.User.LastName}",
                ProductId = order.ProductId,
                ProductName = order.Product.Name,
                Quantity = order.Quantity,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt
            };
            return Ok(orderDto);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetUserOrders(int userId)
        {
            var orders = await _orderService.GetUserOrdersAsync(userId);
            var orderDtos = orders.Select(o => new OrderResponseDTO
            {
                Id = o.Id,
                UserId = o.UserId,
                UserName = $"{o.User.FirstName} {o.User.LastName}",
                ProductId = o.ProductId,
                ProductName = o.Product.Name,
                Quantity = o.Quantity,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt
            });
            return Ok(orderDtos);
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDTO>> CreateOrder(CreateOrderDTO createOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = new Order
            {
                UserId = createOrderDto.UserId,
                ProductId = createOrderDto.ProductId,
                Quantity = createOrderDto.Quantity,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            var createdOrder = await _orderService.CreateOrderAsync(order);
            if (createdOrder == null)
                return BadRequest("Sipariş oluşturulamadı.");

            var orderDto = new OrderResponseDTO
            {
                Id = createdOrder.Id,
                UserId = createdOrder.UserId,
                UserName = $"{createdOrder.User.FirstName} {createdOrder.User.LastName}",
                ProductId = createdOrder.ProductId,
                ProductName = createdOrder.Product.Name,
                Quantity = createdOrder.Quantity,
                TotalAmount = createdOrder.TotalAmount,
                Status = createdOrder.Status,
                CreatedAt = createdOrder.CreatedAt
            };

            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, orderDto);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<OrderResponseDTO>> UpdateOrderStatus(int id, UpdateOrderStatusDTO updateStatusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, updateStatusDto.Status);
            if (updatedOrder == null)
                return NotFound($"ID'si {id} olan sipariş bulunamadı.");

            var orderDto = new OrderResponseDTO
            {
                Id = updatedOrder.Id,
                UserId = updatedOrder.UserId,
                UserName = $"{updatedOrder.User.FirstName} {updatedOrder.User.LastName}",
                ProductId = updatedOrder.ProductId,
                ProductName = updatedOrder.Product.Name,
                Quantity = updatedOrder.Quantity,
                TotalAmount = updatedOrder.TotalAmount,
                Status = updatedOrder.Status,
                CreatedAt = updatedOrder.CreatedAt
            };

            return Ok(orderDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan sipariş bulunamadı.");

            return NoContent();
        }
    }
}
