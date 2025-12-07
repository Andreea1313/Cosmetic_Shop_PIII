using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CosmeticShop.Models
{
    public class CosmeticShopContext : IdentityDbContext<User>
    {
        public CosmeticShopContext(DbContextOptions<CosmeticShopContext> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Favorite> Favorites { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .HasPrincipalKey(u => u.UserId); 

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey("IdentityUserId");
            modelBuilder.Entity<Favorite>()
                .Ignore(f => f.User);

            modelBuilder.Entity<Cart>().Ignore(c => c.User);
            modelBuilder.Entity<Order>().Ignore(o => o.User);
        }

    }

}
