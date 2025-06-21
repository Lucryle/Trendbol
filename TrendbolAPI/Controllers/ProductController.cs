using Microsoft.AspNetCore.Authorization;
// ... existing code ...
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IJwtService _jwtService;

    public ProductController(IProductService productService, IJwtService jwtService)
    {
        _productService = productService;
        _jwtService = jwtService;
    }
// ... existing code ...
[HttpPost]
public async Task<ActionResult<ProductResponseDto>> CreateProduct([FromBody] CreateProductDto createProductDto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    try
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        int? sellerId = _jwtService.GetUserIdFromAuthorizationHeader(authHeader);
        if (sellerId == null)
            return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
        var createdProduct = await _productService.CreateProductFromDtoAsync(createProductDto, sellerId.Value);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
// ... existing code ...
[HttpPut("{id}")]
public async Task<ActionResult<ProductResponseDto>> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    try
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        int? sellerId = _jwtService.GetUserIdFromAuthorizationHeader(authHeader);
        if (sellerId == null)
            return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
        var updatedProduct = await _productService.UpdateProductFromDtoAsync(id, updateProductDto, sellerId.Value);
        return Ok(updatedProduct);
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(ex.Message);
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
// ... existing code ...
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteProduct(int id)
{
    try
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        int? sellerId = _jwtService.GetUserIdFromAuthorizationHeader(authHeader);
        if (sellerId == null)
            return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
        var result = await _productService.DeleteProductAsync(id, sellerId.Value);
        if (!result)
            return NotFound($"ID'si {id} olan ürün bulunamadı veya silme yetkiniz yok.");
        return NoContent();
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
// ... existing code ...
[HttpPut("{id}/stock")]
public async Task<IActionResult> UpdateStock(int id, [FromBody] int quantity)
{
    if (quantity < 0)
        return BadRequest("Stok miktarı negatif olamaz.");

    try
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        int? sellerId = _jwtService.GetUserIdFromAuthorizationHeader(authHeader);
        if (sellerId == null)
            return Unauthorized("Kullanıcı kimliği doğrulanamadı.");
        var result = await _productService.UpdateProductStockAsync(id, quantity, sellerId.Value);
        if (!result)
            return NotFound($"ID'si {id} olan ürün bulunamadı veya güncelleme yetkiniz yok.");
        return NoContent();
    }
    catch (Exception ex)
    {
        return BadRequest(ex.Message);
    }
}
