using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using product.api.Infrastructure.Data;
using product.api.test.Infrastructure;

namespace product.api.test
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void SetupDatabases(IServiceCollection services)
        {
            var connection = new SqliteConnection("DataSource=:memory:");

            connection.Open();
            services.AddEntityFrameworkSqlite();
            services.AddDbContext<ProductsDbContext>(options => options.UseSqlite(connection));
        }

        protected override void SetupTestDependencies(IServiceCollection services)
        {
            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));
            services.TryAddTransient<RefDataProvider>();
        }
    }
}