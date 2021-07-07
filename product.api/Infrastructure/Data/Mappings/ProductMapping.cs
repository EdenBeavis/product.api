using Microsoft.EntityFrameworkCore;
using product.api.Infrastructure.Data.Entities;

namespace product.api.Infrastructure.Data.Mappings
{
    public class ProductMapping : IEntityConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<Product>();

            entity.ToTable("Products");

            entity.HasMany(p => p.Options)
                .WithOne(o => o.Product)
                .HasForeignKey(o => o.ProductId);

            entity.Property(p => p.Id).HasColumnType("TEXT COLLATE NOCASE");
        }
    }
}