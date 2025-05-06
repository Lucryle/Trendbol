using System.ComponentModel.DataAnnotations;

namespace TrendbolAPI.Models.DTOs
{
    public class CreateOrderDTO
    {
        [Required(ErrorMessage = "Kullanıcı ID'si zorunludur.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Ürün ID'si zorunludur.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Miktar zorunludur.")]
        [Range(1, int.MaxValue, ErrorMessage = "Miktar 1'den büyük olmalıdır.")]
        public int Quantity { get; set; }
    }

    public class UpdateOrderStatusDTO
    {
        [Required(ErrorMessage = "Sipariş durumu zorunludur.")]
        [RegularExpression("^(Pending|Processing|Shipped|Delivered|Cancelled)$", 
            ErrorMessage = "Geçersiz sipariş durumu. Geçerli durumlar: Pending, Processing, Shipped, Delivered, Cancelled")]
        public string Status { get; set; }
    }

    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 