using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrendbolAPI.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        // Customer (Buyer)
        public int CustomerID { get; set; }
        public User Customer { get; set; }  // navigation property

        // Product
        public int ProductID { get; set; }
        public Product Product { get; set; }  // navigation property

        public int Quantity { get; set; }
        public float TotalPrice { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
