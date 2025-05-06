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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Seller)
                .WithMany(u => u.Products)
                .HasForeignKey(p => p.SellerID)
                .OnDelete(DeleteBehavior.Restrict);  // satıcı (foreign key) silinirse ürün silinmesin (silinmesini zaten yapamayız çünkü foreign key)

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);  // kategori silinirse ürün silinsin

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // ürün silinirse sipariş silinsin

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade); // kullanıcı silinirse sipariş silinsin
        }
    }
}
