using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.WebAPI.Entities;

namespace Shop.WebAPI.Data
{
    public class ShopApplicationContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public ShopApplicationContext(DbContextOptions<ShopApplicationContext> options)
            : base(options)
        {
        }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // modelBuilder.Entity<Order>()
            //     .HasOne(o => o.User)
            //     .WithMany(u => u.Orders)
            //     .HasForeignKey(o => o.UserId);
            //
            // modelBuilder.Entity<Product>()
            //     .HasOne(p => p.Category)
            //     .WithMany(c => c.Products)
            //     .HasForeignKey(p => p.CategoryId);
            //
            // modelBuilder.Entity<Comment>()
            //     .HasOne(c => c.Product)
            //     .WithMany(p => p.Comments)  
            //     .HasForeignKey(c => c.ProductId);
            //
            // modelBuilder.Entity<Comment>()
            //     .HasOne(c => c.User)
            //     .WithMany(u => u.Comments)
            //     .HasForeignKey(c => c.UserId);
        }
    }
}
