using System.ComponentModel.DataAnnotations;

namespace TrendbolAPI.Models.DTOs
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ürün adı 3-100 karakter arasında olmalıdır.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ürün açıklaması zorunludur.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Ürün açıklaması 10-500 karakter arasında olmalıdır.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ürün fiyatı zorunludur.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Ürün fiyatı 0'dan büyük olmalıdır.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stok miktarı zorunludur.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0'dan büyük veya eşit olmalıdır.")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Kategori ID'si zorunludur.")]
        public int CategoryId { get; set; }
    }

    public class UpdateProductDTO
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ürün adı 3-100 karakter arasında olmalıdır.")]
        public string? Name { get; set; }

        [StringLength(500, MinimumLength = 10, ErrorMessage = "Ürün açıklaması 10-500 karakter arasında olmalıdır.")]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Ürün fiyatı 0'dan büyük olmalıdır.")]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı 0'dan büyük veya eşit olmalıdır.")]
        public int? StockQuantity { get; set; }

        public int? CategoryId { get; set; }
    }

    public class ProductResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 