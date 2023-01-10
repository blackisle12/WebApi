using Microsoft.EntityFrameworkCore;
using WebApi.Repository.Entity;

namespace WebApi.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            new DbInitializer(builder).Seed();
        }
    }
}
