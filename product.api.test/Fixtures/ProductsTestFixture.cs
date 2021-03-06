using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using product.api.Infrastructure.Data;
using product.api.Infrastructure.Data.Entities;
using product.api.Models.Products;
using product.api.test.Fakes.Builders;
using product.api.test.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace product.api.test.Fixtures
{
    public class ProductsTestFixture : TestWebApplicationFactory
    {
        private const string ProductApi = "api/products";

        public async Task<(HttpResponseMessage message, string stringContent)> GetProducts(string query)
        {
            var response = await Client.GetAsync($"{ProductApi}{query}");
            var responseString = await response.Content.ReadAsStringAsync();

            return (response, responseString);
        }

        public async Task<(HttpResponseMessage message, string stringContent)> PostProduct(ProductDto productDto)
        {
            var json = JsonConvert.SerializeObject(productDto);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync($"{ProductApi}/", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();

            return (response, responseString);
        }

        public async Task<(HttpResponseMessage message, string stringContent)> UpdateProduct(Guid id, ProductDto productDto)
        {
            var json = JsonConvert.SerializeObject(productDto);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PutAsync($"{ProductApi}/{id}/", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();

            return (response, responseString);
        }

        public async Task<(HttpResponseMessage message, string stringContent)> DeleteProduct(Guid id)
        {
            var response = await Client.DeleteAsync($"{ProductApi}/{id}/");
            var responseString = await response.Content.ReadAsStringAsync();

            return (response, responseString);
        }

        public async Task CreateProducts(params ProductDtoSetup[] products)
        {
            var context = Services.GetService<ProductsDbContext>();
            var mapper = Services.GetService<IMapper>();
            var mappedProducts = mapper.Map<IEnumerable<Product>>(products);
            var mappedProductOptions = mapper.Map<IEnumerable<ProductOption>>(products.SelectMany(p => p.ProductOptions));

            await context.Products.AddRangeAsync(mappedProducts);
            await context.ProductOptions.AddRangeAsync(mappedProductOptions);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}