using product.api.test.Infrastructure;
using System.Net.Http;
using System.Threading.Tasks;

namespace product.api.test.Fixtures
{
    public class ProductsTestFixture : TestWebApplicationFactory
    {
        private const string ProductApi = "api/products";

        public async Task<(HttpResponseMessage message, string stringContent)> GetProduct(string query)
        {
            var response = await Client.GetAsync($"{ProductApi}/{query}");
            var responseString = await response.Content.ReadAsStringAsync();

            return (response, responseString);
        }

        //public async Task<(HttpResponseMessage message, string stringContent)> PostPoduct(string query, Product productDto)
        //{
        //    var json = JsonConvert.SerializeObject(productDto);
        //    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await Client.PostAsync($"{ProductApi}/{query}", stringContent);
        //    var responseString = await response.Content.ReadAsStringAsync();

        //    return (response, responseString);
        //}

        //public async Task<(HttpResponseMessage message, string stringContent)> UpdateProduct(string query, Product productDto)
        //{
        //    var json = JsonConvert.SerializeObject(productDto);
        //    var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await Client.PatchAsync($"{ProductApi}/{query}", stringContent);
        //    var responseString = await response.Content.ReadAsStringAsync();

        //    return (response, responseString);
        //}
    }
}