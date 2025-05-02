using Microsoft.EntityFrameworkCore;
using TrendbolAPI.Models;

namespace TrendbolAPI.Data
{
    public class TrendbolContext : DbContext
    {
        public TrendbolContext(DbContextOptions<TrendbolContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
