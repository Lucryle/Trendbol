using Microsoft.AspNetCore.Mvc;
using TrendbolAPI.Models;
using TrendbolAPI.Models.DTOs;
using TrendbolAPI.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace TrendbolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IJwtService _jwtService;

        public OrderController(IOrderService orderService, IJwtService jwtService)
        {
            _orderService = orderService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersDtoAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderDtoByIdAsync(id);
            if (order == null)
                return NotFound($"ID'si {id} olan sipariş bulunamadı.");
            return Ok(order);
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult<IEnumerable<UserOrderResponseDto>>> GetMyOrders()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                int? userId = _jwtService.GetUserIdFromAuthorizationHeader(authHeader);
                if (userId == null)
                    return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
                var orders = await _orderService.GetUserOrdersDtoAsync(userId.Value);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<UserOrderResponseDto>>> GetUserOrders(int userId)
        {
            var orders = await _orderService.GetUserOrdersDtoAsync(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                int? userId = _jwtService.GetUserIdFromAuthorizationHeader(authHeader);
                if (userId == null)
                    return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
                var createdOrder = await _orderService.CreateOrderFromDtoAsync(createOrderDto, userId.Value);
                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        public async Task<ActionResult<OrderResponseDto>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateOrderStatusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                int? userId = _jwtService.GetUserIdFromAuthorizationHeader(authHeader);
                if (userId == null)
                    return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
                var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, updateOrderStatusDto, userId.Value);
                if (updatedOrder == null)
                    return NotFound($"ID'si {id} olan sipariş bulunamadı.");
                return Ok(updatedOrder);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

        // Eski metodlar
        [HttpGet("legacy")]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrdersLegacy()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("legacy/{id}")]
        public async Task<ActionResult<Order>> GetOrderByIdLegacy(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound($"ID'si {id} olan sipariş bulunamadı.");
            return Ok(order);
        }

        [HttpGet("legacy/user/{userId}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrdersLegacy(int userId)
        {
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpPost("legacy")]
        public async Task<ActionResult<Order>> CreateOrderLegacy([FromBody] Order order)
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
            return CreatedAtAction(nameof(GetOrderByIdLegacy), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("legacy/{id}/status")]
        public async Task<ActionResult<Order>> UpdateOrderStatusLegacy(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Sipariş durumu boş olamaz.");
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(id, status);
            if (updatedOrder == null)
                return NotFound($"ID'si {id} olan sipariş bulunamadı.");
            return Ok(updatedOrder);
        }

        [HttpDelete("legacy/{id}")]
        public async Task<IActionResult> DeleteOrderLegacy(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan sipariş bulunamadı.");
            return NoContent();
        }

        [HttpPost("legacy/{id}/cancel")]
        public async Task<IActionResult> CancelOrderLegacy(int id)
        {
            var result = await _orderService.CancelOrderAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan sipariş bulunamadı veya iptal edilemedi.");
            return Ok("Sipariş iptal edildi.");
        }

        [HttpPost("legacy/{id}/process")]
        public async Task<IActionResult> ProcessOrderLegacy(int id)
        {
            var result = await _orderService.ProcessOrderAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan sipariş bulunamadı veya işleme alınamadı.");
            return Ok("Sipariş işleme alındı.");
        }
    }
}
