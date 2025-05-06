using Microsoft.AspNetCore.Mvc;
using TrendbolAPI.Models;
using TrendbolAPI.Models.DTOs;
using TrendbolAPI.Services.Interfaces;

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
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            var productDtos = products.Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                CreatedAt = p.CreatedAt
            });
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound($"ID'si {id} olan ürün bulunamadı.");

            var productDto = new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name,
                CreatedAt = product.CreatedAt
            };
            return Ok(productDto);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            var productDtos = products.Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                CreatedAt = p.CreatedAt
            });
            return Ok(productDtos);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> CreateProduct(CreateProductDTO createProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                CategoryId = createProductDto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };

            var createdProduct = await _productService.CreateProductAsync(product);
            if (createdProduct == null)
                return BadRequest("Ürün oluşturulamadı.");

            var productDto = new ProductResponseDTO
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                Price = createdProduct.Price,
                StockQuantity = createdProduct.StockQuantity,
                CategoryId = createdProduct.CategoryId,
                CategoryName = createdProduct.Category?.Name,
                CreatedAt = createdProduct.CreatedAt
            };

            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, productDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductResponseDTO>> UpdateProduct(int id, UpdateProductDTO updateProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
                return NotFound($"ID'si {id} olan ürün bulunamadı.");

            // Sadece değişen alanları güncelle
            if (updateProductDto.Name != null)
                existingProduct.Name = updateProductDto.Name;
            if (updateProductDto.Description != null)
                existingProduct.Description = updateProductDto.Description;
            if (updateProductDto.Price.HasValue)
                existingProduct.Price = updateProductDto.Price.Value;
            if (updateProductDto.StockQuantity.HasValue)
                existingProduct.StockQuantity = updateProductDto.StockQuantity.Value;
            if (updateProductDto.CategoryId.HasValue)
                existingProduct.CategoryId = updateProductDto.CategoryId.Value;

            var updatedProduct = await _productService.UpdateProductAsync(id, existingProduct);
            if (updatedProduct == null)
                return BadRequest("Ürün güncellenemedi.");

            var productDto = new ProductResponseDTO
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price,
                StockQuantity = updatedProduct.StockQuantity,
                CategoryId = updatedProduct.CategoryId,
                CategoryName = updatedProduct.Category?.Name,
                CreatedAt = updatedProduct.CreatedAt
            };

            return Ok(productDto);
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
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> SearchProducts([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return BadRequest("Arama terimi boş olamaz.");

            var products = await _productService.SearchProductsAsync(searchTerm);
            var productDtos = products.Select(p => new ProductResponseDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                CreatedAt = p.CreatedAt
            });
            return Ok(productDtos);
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
