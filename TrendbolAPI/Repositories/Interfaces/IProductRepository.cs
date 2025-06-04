using TrendbolAPI.Models;

namespace TrendbolAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product> UpdateAsync(Product product);
        Task<bool> DeleteAsync(Product product);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    }
} 