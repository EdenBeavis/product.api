using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace product.api.test
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void SetupTestDependencies(IServiceCollection services)
        {
            //prepare for entity framework
            //services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));
            //services.TryAddTransient<RefDataProvider>();
        }
    }
}