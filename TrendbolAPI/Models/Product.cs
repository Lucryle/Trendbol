using System.ComponentModel.DataAnnotations.Schema;

namespace TrendbolAPI.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public int Stock { get; set; }
        public int SellerID { get; set; } // Foreign Key
        public int CategoryID { get; set; }
        public Category Category { get; set; }  // Navigasyon property

        // Navigation property
        [ForeignKey("SellerID")]
        public User Seller { get; set; }
    }
}
