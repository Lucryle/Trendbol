using TrendbolAPI.Models;

namespace TrendbolAPI.Factories
{
    public interface IProductFactory
    {
        Product CreateProduct(Product product);
    }

    public class ProductFactory : IProductFactory
    {
        public Product CreateProduct(Product product)
        {
            return new Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                SellerId = product.SellerId,
                ImageUrl = product.ImageUrl,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
} 