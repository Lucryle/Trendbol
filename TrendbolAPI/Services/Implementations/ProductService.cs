using TrendbolAPI.Models;
using TrendbolAPI.Models.DTOs;
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

        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsDtoAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                SellerId = p.SellerId,
                SellerName = p.Seller != null ? $"{p.Seller.FirstName} {p.Seller.LastName}" : "Bilinmeyen Satıcı"
            });
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            return product;
        }

        public async Task<ProductResponseDto> GetProductDtoByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name ?? string.Empty,
                Description = product.Description ?? string.Empty,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                SellerId = product.SellerId,
                SellerName = product.Seller != null ? $"{product.Seller.FirstName} {product.Seller.LastName}" : "Bilinmeyen Satıcı"
            };
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            return await _productRepository.AddAsync(product);
        }

        public async Task<ProductResponseDto> CreateProductFromDtoAsync(CreateProductDto createProductDto, int sellerId)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                SellerId = sellerId
            };

            var createdProduct = await _productRepository.AddAsync(product);
            
            return new ProductResponseDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name ?? string.Empty,
                Description = createdProduct.Description ?? string.Empty,
                Price = createdProduct.Price,
                StockQuantity = createdProduct.StockQuantity,
                SellerId = createdProduct.SellerId,
                SellerName = createdProduct.Seller != null ? $"{createdProduct.Seller.FirstName} {createdProduct.Seller.LastName}" : "Bilinmeyen Satıcı"
            };
        }

        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            product.Id = id; 
            return await _productRepository.UpdateAsync(product);
        }

        public async Task<ProductResponseDto> UpdateProductFromDtoAsync(int id, UpdateProductDto updateProductDto, int sellerId)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            if (existingProduct.SellerId != sellerId)
                throw new UnauthorizedAccessException("Bu ürünü güncelleme yetkiniz yok.");

            if (!string.IsNullOrWhiteSpace(updateProductDto.Name))
                existingProduct.Name = updateProductDto.Name;

            if (!string.IsNullOrWhiteSpace(updateProductDto.Description))
                existingProduct.Description = updateProductDto.Description;

            if (updateProductDto.Price.HasValue && updateProductDto.Price.Value > 0)
                existingProduct.Price = updateProductDto.Price.Value;

            if (updateProductDto.StockQuantity.HasValue && updateProductDto.StockQuantity.Value >= 0)
                existingProduct.StockQuantity = updateProductDto.StockQuantity.Value;

            var updatedProduct = await _productRepository.UpdateAsync(existingProduct);

            return new ProductResponseDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name ?? string.Empty,
                Description = updatedProduct.Description ?? string.Empty,
                Price = updatedProduct.Price,
                StockQuantity = updatedProduct.StockQuantity,
                SellerId = updatedProduct.SellerId,
                SellerName = updatedProduct.Seller != null ? $"{updatedProduct.Seller.FirstName} {updatedProduct.Seller.LastName}" : "Bilinmeyen Satıcı"
            };
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return false;

            await _productRepository.DeleteAsync(product);
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id, int sellerId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return false;

            if (product.SellerId != sellerId)
                return false;

            await _productRepository.DeleteAsync(product);
            return true;
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _productRepository.SearchProductsAsync(searchTerm);
        }

        public async Task<IEnumerable<ProductResponseDto>> SearchProductsDtoAsync(string searchTerm)
        {
            var products = await _productRepository.SearchProductsAsync(searchTerm);
            return products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name ?? string.Empty,
                Description = p.Description ?? string.Empty,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                SellerId = p.SellerId,
                SellerName = p.Seller != null ? $"{p.Seller.FirstName} {p.Seller.LastName}" : "Bilinmeyen Satıcı"
            });
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

        public async Task<bool> UpdateProductStockAsync(int id, int quantity, int sellerId)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return false;

            if (product.SellerId != sellerId)
                return false;

            product.StockQuantity = quantity;
            await _productRepository.UpdateAsync(product);
            return true;
        }
    }
} 