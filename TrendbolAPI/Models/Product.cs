using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrendbolAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int SellerID { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("SellerID")]
        public User Seller { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
