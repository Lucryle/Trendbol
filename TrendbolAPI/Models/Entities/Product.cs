using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrendbolAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        // Foreign key for seller
        public int SellerId { get; set; }
        public User? Seller { get; set; }

        // siparişler için navigation property
        public ICollection<Order>? Orders { get; set; } = new List<Order>();
    }
}
