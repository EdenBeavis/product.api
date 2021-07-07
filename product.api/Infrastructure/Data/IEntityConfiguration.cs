using Microsoft.EntityFrameworkCore;

namespace product.api.Infrastructure.Data
{
    public interface IEntityConfiguration
    {
        void Configure(ModelBuilder modelBuilder);
    }
}