using System;

namespace TrendbolAPI.Models
{
    public class Order
    {
        public int OrderID{get; set;}
        public int CustomerID{get; set;}
        public int ProductID{get; set;}
        public int Quantity{get; set;}
        public float TotalPrice{get; set;}
        public string Status{get; set;}
        public DateTime CreatedAt{get; set;}
        public Order(int orderID, int customerID, int productID, int quantity, float totalPrice, string status, DateTime createdAt)
        {
            OrderID = orderID;
            CustomerID = customerID;
            ProductID = productID;
            Quantity = quantity;
            TotalPrice = totalPrice;
            Status = status;
            CreatedAt = createdAt;
        }

        // TODO: fonksiyonlar buraya eklenecek
    }
}
