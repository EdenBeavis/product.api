using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using product.api.Infrastructure.Data;
using product.api.Infrastructure.Data.Entities;
using product.api.Models.ProductOptions;
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
    public class ProductOptionsTestFixture : TestWebApplicationFactory
    {
        private const string ProductApi = "api/products";

        public async Task<(HttpResponseMessage message, string stringContent)> GetProductOptions(Guid productId, string query = "")
        {
            var optionsPath = $"{productId}/options";
            var response = await Client.GetAsync($"{ProductApi}/{optionsPath}/{query}");
            var responseString = await response.Content.ReadAsStringAsync();

            return (response, responseString);
        }

        public async Task<(HttpResponseMessage message, string stringContent)> PostProductOption(Guid productId, ProductOptionDto productOptionDto)
        {
            var optionsPath = $"{productId}/options";
            var json = JsonConvert.SerializeObject(productOptionDto);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync($"{ProductApi}/{optionsPath}", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();

            return (response, responseString);
        }

        public async Task<(HttpResponseMessage message, string stringContent)> UpdateProductOption(Guid productId, Guid id, ProductOptionDto productOptionDto)
        {
            var optionsPath = $"{productId}/options";
            var json = JsonConvert.SerializeObject(productOptionDto);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PutAsync($"{ProductApi}/{optionsPath}/{id}/", stringContent);
            var responseString = await response.Content.ReadAsStringAsync();

            return (response, responseString);
        }

        public async Task<(HttpResponseMessage message, string stringContent)> DeleteProductOption(Guid productId, Guid id)
        {
            var optionsPath = $"{productId}/options";
            var response = await Client.DeleteAsync($"{ProductApi}/{optionsPath}/{id}/");
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