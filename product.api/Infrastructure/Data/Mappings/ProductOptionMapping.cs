using Microsoft.EntityFrameworkCore;
using product.api.Infrastructure.Data.Entities;

namespace product.api.Infrastructure.Data.Mappings
{
    public class ProductOptionMapping : IEntityConfiguration
    {
        public void Configure(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<ProductOption>();

            entity.ToTable("ProductOptions");

            entity.HasOne(o => o.Product)
                .WithMany(p => p.Options)
                .HasForeignKey(o => o.ProductId);

            entity.Property(p => p.Id).HasColumnType("TEXT COLLATE NOCASE");
            entity.Property(p => p.ProductId).HasColumnType("TEXT COLLATE NOCASE");
        }
    }
}