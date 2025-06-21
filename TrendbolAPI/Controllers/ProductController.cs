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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IJwtService _jwtService;

        public ProductController(IProductService productService, IJwtService jwtService)
        {
            _productService = productService;
            _jwtService = jwtService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsDtoAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductDtoByIdAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

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

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> SearchProducts([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Arama terimi boş olamaz.");

            var products = await _productService.SearchProductsDtoAsync(searchTerm);
            return Ok(products);
        }

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

        // Eski metodlar 
        [HttpGet("legacy")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsLegacy()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("legacy/{id}")]
        public async Task<ActionResult<Product>> GetProductByIdLegacy(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("legacy")]
        public async Task<ActionResult<Product>> CreateProductLegacy([FromBody] Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                return BadRequest("Ürün adı boş olamaz.");

            if (string.IsNullOrWhiteSpace(product.Description))
                return BadRequest("Ürün açıklaması boş olamaz.");

            if (product.Price <= 0)
                return BadRequest("Ürün fiyatı 0'dan büyük olmalıdır.");

            if (product.StockQuantity < 0)
                return BadRequest("Stok miktarı negatif olamaz.");

            product.CreatedAt = DateTime.UtcNow;
            var createdProduct = await _productService.CreateProductAsync(product);
            
            return CreatedAtAction(nameof(GetProductByIdLegacy), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("legacy/{id}")]
        public async Task<ActionResult<Product>> UpdateProductLegacy(int id, [FromBody] Product product)
        {
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
                return NotFound($"ID'si {id} olan ürün bulunamadı.");

            if (!string.IsNullOrWhiteSpace(product.Name))
                existingProduct.Name = product.Name;

            if (!string.IsNullOrWhiteSpace(product.Description))
                existingProduct.Description = product.Description;

            if (product.Price > 0)
                existingProduct.Price = product.Price;

            if (product.StockQuantity >= 0)
                existingProduct.StockQuantity = product.StockQuantity;

            existingProduct.UpdatedAt = DateTime.UtcNow;
            var updatedProduct = await _productService.UpdateProductAsync(id, existingProduct);
            
            return Ok(updatedProduct);
        }

        [HttpDelete("legacy/{id}")]
        public async Task<IActionResult> DeleteProductLegacy(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan ürün bulunamadı.");

            return NoContent();
        }

        [HttpGet("legacy/search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProductsLegacy([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Arama terimi boş olamaz.");

            var products = await _productService.SearchProductsAsync(searchTerm);
            return Ok(products);
        }

        [HttpPut("legacy/{id}/stock")]
        public async Task<IActionResult> UpdateStockLegacy(int id, [FromBody] int quantity)
        {
            if (quantity < 0)
                return BadRequest("Stok miktarı negatif olamaz.");

            var result = await _productService.UpdateProductStockAsync(id, quantity);
            if (!result)
                return NotFound($"ID'si {id} olan ürün bulunamadı.");

            return NoContent();
        }
    }
}
