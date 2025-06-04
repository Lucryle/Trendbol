using Xunit;
using TrendbolAPI.Services;
using TrendbolAPI.Models;
using TrendbolAPI.Repositories.Interfaces;
using TrendbolAPI.Services.Interfaces;
using Moq;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockEmailService = new Mock<IEmailService>();
        _userService = new UserService(_mockUserRepository.Object, _mockEmailService.Object);
    }

    [Fact]
    public async Task GetUserByEmail_ExistingUser_ReturnsUser()
    {
        // Arrange
        var expectedUser = new User { Id = 1, Email = "test@example.com" };
        _mockUserRepository.Setup(repo => repo.GetByEmailAsync("test@example.com"))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _userService.GetUserByEmailAsync("test@example.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser.Id, result.Id);
        Assert.Equal(expectedUser.Email, result.Email);
    }

    [Fact]
    public async Task GetUserByEmail_NonExistingUser_ReturnsNull()
    {
        // Arrange
        _mockUserRepository.Setup(repo => repo.GetByEmailAsync("nonexistent@example.com"))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _userService.GetUserByEmailAsync("nonexistent@example.com");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ValidateUser_ValidCredentials_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 1, Email = "test@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password123") };
        _mockUserRepository.Setup(repo => repo.GetByEmailAsync("test@example.com"))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.ValidateUserAsync("test@example.com", "password123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task ValidateUser_InvalidCredentials_ReturnsNull()
    {
        // Arrange
        var user = new User { Id = 1, Email = "test@example.com", Password = BCrypt.Net.BCrypt.HashPassword("password123") };
        _mockUserRepository.Setup(repo => repo.GetByEmailAsync("test@example.com"))
            .ReturnsAsync(user);

        // Act
        var result = await _userService.ValidateUserAsync("test@example.com", "wrongpassword");

        // Assert
        Assert.Null(result);
    }
} 