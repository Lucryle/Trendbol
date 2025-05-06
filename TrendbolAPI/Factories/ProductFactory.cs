using TrendbolAPI.Models;
using TrendbolAPI.Models.DTOs;

namespace TrendbolAPI.Factories
{
    public interface IProductFactory
    {
        Product CreateProduct(CreateProductDTO createProductDto);
    }

    public class ProductFactory : IProductFactory
    {
        public Product CreateProduct(CreateProductDTO createProductDto)
        {
            return new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                CategoryId = createProductDto.CategoryId,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
} 