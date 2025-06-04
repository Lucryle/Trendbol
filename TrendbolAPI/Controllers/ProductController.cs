using Microsoft.AspNetCore.Mvc;
using TrendbolAPI.Models;
using TrendbolAPI.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace TrendbolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"ID'si {id} olan ürün bulunamadı.");

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
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
            
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, [FromBody] Product product)
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan ürün bulunamadı.");

            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Arama terimi boş olamaz.");

            var products = await _productService.SearchProductsAsync(searchTerm);
            return Ok(products);
        }

        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int quantity)
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
