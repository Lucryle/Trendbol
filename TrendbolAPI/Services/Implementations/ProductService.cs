using TrendbolAPI.Models;
using TrendbolAPI.Repositories.Interfaces;
using TrendbolAPI.Services.Interfaces;
using TrendbolAPI.Factories;

namespace TrendbolAPI.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductFactory _productFactory;

        public ProductService(IProductRepository productRepository, IProductFactory productFactory)
        {
            _productRepository = productRepository;
            _productFactory = productFactory;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            return product;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            return await _productRepository.AddAsync(product);
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            product.Id = id; 
            return await _productRepository.UpdateAsync(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return false;

            await _productRepository.DeleteAsync(product);
            return true;
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _productRepository.SearchProductsAsync(searchTerm);
        }

        public async Task<bool> UpdateProductStockAsync(int id, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return false;

            product.StockQuantity = quantity;
            await _productRepository.UpdateAsync(product);
            return true;
        }
    }
} 