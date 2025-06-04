using Microsoft.AspNetCore.Mvc;
using TrendbolAPI.Models;
using TrendbolAPI.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

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
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound($"ID'si {id} olan sipariş bulunamadı.");
            return Ok(order);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders(int userId)
        {
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] Order order)
        {
            if (order.UserId <= 0)
                return BadRequest("Geçerli bir kullanıcı ID'si giriniz.");
            if (order.ProductId <= 0)
                return BadRequest("Geçerli bir ürün ID'si giriniz.");
            if (order.Quantity <= 0)
                return BadRequest("Adet 0'dan büyük olmalıdır.");
            if (order.TotalAmount <= 0)
                return BadRequest("Tutar 0'dan büyük olmalıdır.");
            if (string.IsNullOrWhiteSpace(order.Status))
                return BadRequest("Sipariş durumu boş olamaz.");

            order.CreatedAt = DateTime.UtcNow;
            var createdOrder = await _orderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<Order>> UpdateOrderStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Sipariş durumu boş olamaz.");
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, status);
            if (updatedOrder == null)
                return NotFound($"ID'si {id} olan sipariş bulunamadı.");
            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan sipariş bulunamadı.");
            return NoContent();
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var result = await _orderService.CancelOrderAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan sipariş bulunamadı veya iptal edilemedi.");
            return Ok("Sipariş iptal edildi.");
        }

        [HttpPost("{id}/process")]
        public async Task<IActionResult> ProcessOrder(int id)
        {
            var result = await _orderService.ProcessOrderAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan sipariş bulunamadı veya işleme alınamadı.");
            return Ok("Sipariş işleme alındı.");
        }
    }
}
