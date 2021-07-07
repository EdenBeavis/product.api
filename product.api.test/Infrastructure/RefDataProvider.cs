using Microsoft.EntityFrameworkCore;
using product.api.Infrastructure.Data;
using System.Linq;

namespace product.api.test.Infrastructure
{
    public class RefDataProvider
    {
        private readonly ProductsDbContext _productsDbContext;

        public RefDataProvider(ProductsDbContext productsDbContext)
        {
            _productsDbContext = productsDbContext;
        }

        public void SetupRefData()
        {
            DeleteRefData();

            _productsDbContext.Database.GetDbConnection().Open();
            _productsDbContext.Database.EnsureCreated();
            _productsDbContext.SaveChanges();
        }

        private void DeleteRefData()
        {
            _productsDbContext.ChangeTracker.Entries().ToList().All(x =>
            {
                x.State = EntityState.Detached;
                return true;
            });

            _productsDbContext.Database.GetDbConnection().Close();
            _productsDbContext.Database.EnsureDeleted();
        }
    }
}