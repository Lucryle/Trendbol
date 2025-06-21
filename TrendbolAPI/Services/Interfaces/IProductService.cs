using TrendbolAPI.Models;
using TrendbolAPI.Models.DTOs;

namespace TrendbolAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<ProductResponseDto>> GetAllProductsDtoAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<ProductResponseDto> GetProductDtoByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<ProductResponseDto> CreateProductFromDtoAsync(CreateProductDto createProductDto, int sellerId);
        Task<Product> UpdateProductAsync(int id, Product product);
        Task<ProductResponseDto> UpdateProductFromDtoAsync(int id, UpdateProductDto updateProductDto, int sellerId);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> DeleteProductAsync(int id, int sellerId);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<ProductResponseDto>> SearchProductsDtoAsync(string searchTerm);
        Task<bool> UpdateProductStockAsync(int id, int quantity);
        Task<bool> UpdateProductStockAsync(int id, int quantity, int sellerId);
    }
}