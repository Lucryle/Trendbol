using System;

namespace TrendbolAPI.Models
{
    public class User
    {
        public int UserID{get; set;}
        public string Name{get; set;}
        public string Email{get; set;}
        public string Password{get; set;}
        public string Role{get; set;}
        public DateTime CreatedAt{get; set;}
        public bool IsVerified{get; set;}

        public ICollection<Product> Products { get; set; } = new List<Product>(); // seller için sattığı ürünler

    }
}
