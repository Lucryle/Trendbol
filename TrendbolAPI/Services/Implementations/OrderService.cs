using TrendbolAPI.Models;
using TrendbolAPI.Models.DTOs;
using TrendbolAPI.Repositories.Interfaces;
using TrendbolAPI.Services.Interfaces;

namespace TrendbolAPI.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllOrdersDtoAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(o => new OrderResponseDto
            {
                Id = o.Id,
                UserId = o.UserId,
                UserName = o.User != null ? $"{o.User.FirstName} {o.User.LastName}" : "Bilinmeyen Kullanıcı",
                ProductId = o.ProductId,
                ProductName = o.Product != null ? o.Product.Name ?? "Bilinmeyen Ürün" : "Bilinmeyen Ürün",
                Quantity = o.Quantity,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt,
                ProductSellerId = o.Product?.SellerId ?? 0,
                ProductSellerName = o.Product?.Seller != null ? $"{o.Product.Seller.FirstName} {o.Product.Seller.LastName}" : "Bilinmeyen Satıcı"
            });
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<OrderResponseDto?> GetOrderDtoByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return null;

            return new OrderResponseDto
            {
                Id = order.Id,
                UserId = order.UserId,
                UserName = order.User != null ? $"{order.User.FirstName} {order.User.LastName}" : "Bilinmeyen Kullanıcı",
                ProductId = order.ProductId,
                ProductName = order.Product != null ? order.Product.Name ?? "Bilinmeyen Ürün" : "Bilinmeyen Ürün",
                Quantity = order.Quantity,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                ProductSellerId = order.Product?.SellerId ?? 0,
                ProductSellerName = order.Product?.Seller != null ? $"{order.Product.Seller.FirstName} {order.Product.Seller.LastName}" : "Bilinmeyen Satıcı"
            };
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(int userId)
        {
            return await _orderRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<UserOrderResponseDto>> GetUserOrdersDtoAsync(int userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return orders.Select(o => new UserOrderResponseDto
            {
                Id = o.Id,
                ProductId = o.ProductId,
                ProductName = o.Product != null ? o.Product.Name ?? "Bilinmeyen Ürün" : "Bilinmeyen Ürün",
                Quantity = o.Quantity,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedAt = o.CreatedAt
            });
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            return await _orderRepository.AddAsync(order);
        }

        public async Task<OrderResponseDto> CreateOrderFromDtoAsync(CreateOrderDto createOrderDto, int userId)
        {
            // Ürünü kontrol et
            var product = await _productRepository.GetByIdAsync(createOrderDto.ProductId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {createOrderDto.ProductId} not found.");

            // Stok kontrolü
            if (product.StockQuantity < createOrderDto.Quantity)
                throw new InvalidOperationException($"Yetersiz stok. Mevcut stok: {product.StockQuantity}, İstenen: {createOrderDto.Quantity}");

            var order = new Order
            {
                UserId = userId,
                ProductId = createOrderDto.ProductId,
                Quantity = createOrderDto.Quantity,
                TotalAmount = product.Price * createOrderDto.Quantity,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            var createdOrder = await _orderRepository.AddAsync(order);

            // Stok güncelle
            product.StockQuantity -= createOrderDto.Quantity;
            await _productRepository.UpdateAsync(product);

            return new OrderResponseDto
            {
                Id = createdOrder.Id,
                UserId = createdOrder.UserId,
                UserName = createdOrder.User != null ? $"{createdOrder.User.FirstName} {createdOrder.User.LastName}" : "Bilinmeyen Kullanıcı",
                ProductId = createdOrder.ProductId,
                ProductName = createdOrder.Product != null ? createdOrder.Product.Name ?? "Bilinmeyen Ürün" : "Bilinmeyen Ürün",
                Quantity = createdOrder.Quantity,
                TotalAmount = createdOrder.TotalAmount,
                Status = createdOrder.Status,
                CreatedAt = createdOrder.CreatedAt,
                ProductSellerId = createdOrder.Product?.SellerId ?? 0,
                ProductSellerName = createdOrder.Product?.Seller != null ? $"{createdOrder.Product.Seller.FirstName} {createdOrder.Product.Seller.LastName}" : "Bilinmeyen Satıcı"
            };
        }

        public async Task<Order?> UpdateOrderStatusAsync(int id, string status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return null;

            order.Status = status;
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<OrderResponseDto?> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateOrderStatusDto, int userId)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return null;

            // Sadece ürünün sahibi sipariş durumunu güncelleyebilir
            if (order.Product?.SellerId != userId)
                throw new UnauthorizedAccessException("Bu siparişin durumunu güncelleme yetkiniz yok.");

            order.Status = updateOrderStatusDto.Status;
            var updatedOrder = await _orderRepository.UpdateAsync(order);

            return new OrderResponseDto
            {
                Id = updatedOrder.Id,
                UserId = updatedOrder.UserId,
                UserName = updatedOrder.User != null ? $"{updatedOrder.User.FirstName} {updatedOrder.User.LastName}" : "Bilinmeyen Kullanıcı",
                ProductId = updatedOrder.ProductId,
                ProductName = updatedOrder.Product != null ? updatedOrder.Product.Name ?? "Bilinmeyen Ürün" : "Bilinmeyen Ürün",
                Quantity = updatedOrder.Quantity,
                TotalAmount = updatedOrder.TotalAmount,
                Status = updatedOrder.Status,
                CreatedAt = updatedOrder.CreatedAt,
                ProductSellerId = updatedOrder.Product?.SellerId ?? 0,
                ProductSellerName = updatedOrder.Product?.Seller != null ? $"{updatedOrder.Product.Seller.FirstName} {updatedOrder.Product.Seller.LastName}" : "Bilinmeyen Satıcı"
            };
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

        public async Task<bool> CancelOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return false;

            order.Status = "Cancelled";
            var updatedOrder = await _orderRepository.UpdateAsync(order);
            return updatedOrder != null;
        }

        public async Task<bool> ProcessOrderAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
                return false;

            order.Status = "Processing";
            var updatedOrder = await _orderRepository.UpdateAsync(order);
            return updatedOrder != null;
        }
    }
}