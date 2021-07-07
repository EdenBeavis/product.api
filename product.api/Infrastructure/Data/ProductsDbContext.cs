using Microsoft.EntityFrameworkCore;
using product.api.Infrastructure.Data.Entities;
using System;
using System.Linq;

namespace product.api.Infrastructure.Data
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var type = typeof(IEntityConfiguration);

            typeof(ProductsDbContext).Assembly
                .GetTypes()
                .Where(t => type.IsAssignableFrom(t) && t.IsClass)
                .Select(Activator.CreateInstance)
                .Cast<IEntityConfiguration>()
                .All(configuration =>
                {
                    configuration.Configure(modelBuilder);
                    return true;
                });
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
    }
}