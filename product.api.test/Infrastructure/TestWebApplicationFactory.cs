using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;

namespace product.api.test.Infrastructure
{
    public class TestWebApplicationFactory : WebApplicationFactory<TestStartup>
    {
        public HttpClient Client => CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        public RefDataProvider RefDataProvider => Services.GetService<RefDataProvider>();

        protected override IHostBuilder CreateHostBuilder()
        {
            return TestHostBuilder.CreateHostBuilder();
        }
    }
}