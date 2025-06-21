using Microsoft.AspNetCore.Authorization;
// ... existing code ...
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
// ... existing code ...
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
// ... existing code ...
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
// ... existing code ...
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
