using Xunit;
using TrendbolAPI.Services.Implementations;
using TrendbolAPI.Models;
using TrendbolAPI.Repositories.Interfaces;
using TrendbolAPI.Factories;
using Moq;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly Mock<IProductFactory> _mockProductFactory;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _mockProductFactory = new Mock<IProductFactory>();
        _productService = new ProductService(_mockProductRepository.Object, _mockProductFactory.Object);
    }

    [Fact]
    public async Task GetProductById_ExistingProduct_ReturnsProduct()
    {
        // Arrange
        var expectedProduct = new Product { Id = 1, Name = "Test Product", Price = 100 };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(expectedProduct);

        // Act
        var result = await _productService.GetProductByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Id, result.Id);
        Assert.Equal(expectedProduct.Name, result.Name);
        Assert.Equal(expectedProduct.Price, result.Price);
    }

    [Fact]
    public async Task GetAllProducts_ReturnsAllProducts()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Price = 100 },
            new Product { Id = 2, Name = "Product 2", Price = 200 }
        };
        _mockProductRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(expectedProducts);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
} 