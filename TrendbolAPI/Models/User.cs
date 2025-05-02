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

        public User(int userID, string name, string email, string password, string role, DateTime createdAt, bool isVerified)
        {
            UserID = userID;
            Name = name;
            Email = email;
            Password = password;
            Role = role;
            CreatedAt = createdAt;
            IsVerified = isVerified;
        }

        // TODO: Fonksiyonlar buraya eklenecek
    }
}
