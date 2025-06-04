using Xunit;
using TrendbolAPI.Services.Implementations;
using TrendbolAPI.Models;
using TrendbolAPI.Repositories.Interfaces;
using Moq;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockProductRepository = new Mock<IProductRepository>();
        _orderService = new OrderService(_mockOrderRepository.Object, _mockProductRepository.Object);
    }

    [Fact]
    public async Task GetOrderById_ExistingOrder_ReturnsOrder()
    {
        // Arrange
        var expectedOrder = new Order 
        { 
            Id = 1, 
            UserId = 1,
            CreatedAt = DateTime.UtcNow,
            TotalAmount = 100
        };
        _mockOrderRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(expectedOrder);

        // Act
        var result = await _orderService.GetOrderByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedOrder.Id, result.Id);
        Assert.Equal(expectedOrder.UserId, result.UserId);
        Assert.Equal(expectedOrder.TotalAmount, result.TotalAmount);
    }

    [Fact]
    public async Task GetUserOrders_ReturnsUserOrders()
    {
        // Arrange
        var expectedOrders = new List<Order>
        {
            new Order { Id = 1, UserId = 1, CreatedAt = DateTime.UtcNow, TotalAmount = 100 },
            new Order { Id = 2, UserId = 1, CreatedAt = DateTime.UtcNow, TotalAmount = 200 }
        };
        _mockOrderRepository.Setup(repo => repo.GetByUserIdAsync(1))
            .ReturnsAsync(expectedOrders);

        // Act
        var result = await _orderService.GetUserOrdersAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
} 