using TrendbolAPI.Models;
using TrendbolAPI.Models.DTOs;

namespace TrendbolAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<OrderResponseDto>> GetAllOrdersDtoAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<OrderResponseDto?> GetOrderDtoByIdAsync(int id);
        Task<IEnumerable<Order>> GetUserOrdersAsync(int userId);
        Task<IEnumerable<UserOrderResponseDto>> GetUserOrdersDtoAsync(int userId);
        Task<Order> CreateOrderAsync(Order order);
        Task<OrderResponseDto> CreateOrderFromDtoAsync(CreateOrderDto createOrderDto, int userId);
        Task<Order?> UpdateOrderStatusAsync(int id, string status);
        Task<OrderResponseDto?> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateOrderStatusDto, int userId);
        Task<bool> DeleteOrderAsync(int id);
        Task<bool> CancelOrderAsync(int id);
        Task<bool> ProcessOrderAsync(int id);
    }
}