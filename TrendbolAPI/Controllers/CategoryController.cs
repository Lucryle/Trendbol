using Microsoft.AspNetCore.Mvc;
using TrendbolAPI.Models;
using TrendbolAPI.Models.DTOs;
using TrendbolAPI.Services.Interfaces;

namespace TrendbolAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDTO>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var categoryDtos = categories.Select(c => new CategoryResponseDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt
            });
            return Ok(categoryDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDTO>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound($"ID'si {id} olan kategori bulunamadı.");

            var categoryDto = new CategoryResponseDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt
            };
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDTO>> CreateCategory(CreateCategoryDTO createCategoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            var createdCategory = await _categoryService.CreateCategoryAsync(category);
            if (createdCategory == null)
                return BadRequest("Kategori oluşturulamadı.");

            var categoryDto = new CategoryResponseDTO
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Description = createdCategory.Description,
                CreatedAt = createdCategory.CreatedAt
            };

            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, categoryDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryResponseDTO>> UpdateCategory(int id, UpdateCategoryDTO updateCategoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
                return NotFound($"ID'si {id} olan kategori bulunamadı.");

            // Sadece değişen alanları güncelle
            if (updateCategoryDto.Name != null)
                existingCategory.Name = updateCategoryDto.Name;
            if (updateCategoryDto.Description != null)
                existingCategory.Description = updateCategoryDto.Description;

            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, existingCategory);
            if (updatedCategory == null)
                return BadRequest("Kategori güncellenemedi.");

            var categoryDto = new CategoryResponseDTO
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name,
                Description = updatedCategory.Description,
                CreatedAt = updatedCategory.CreatedAt
            };

            return Ok(categoryDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result)
                return NotFound($"ID'si {id} olan kategori bulunamadı.");

            return NoContent();
        }

        [HttpGet("{id}/products")]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetCategoryProducts(int id)
        {
            var products = await _categoryService.GetCategoryProductsAsync(id);
            if (products == null)
                return NotFound($"ID'si {id} olan kategori bulunamadı.");

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
    }
}
