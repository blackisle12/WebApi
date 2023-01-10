using Microsoft.EntityFrameworkCore;
using WebApi.Repository.Entity;

namespace WebApi.Repository
{
    public class DbInitializer
    {
        private readonly ModelBuilder builder;

        public DbInitializer(ModelBuilder builder)
        {
            this.builder = builder;
        }

        public void Seed()
        {
            builder.Entity<Product>(p =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    p.HasData(new Product { Id = i, Name = $"Product {i}", Price = 10 * i });
                }
            });
        }
    }
}
