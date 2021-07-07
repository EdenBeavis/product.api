using product.api.Models.Products;
using product.api.test.Fakes.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace product.api.test.Tests.TestData
{
    public class ProductGetListTestData
    {
        public ProductDtoSetup[] Setup { get; set; }
        public string Input { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; }
        public ProductsDto Expected { get; set; }
    }

    public class ProductGetListTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            #region Name is empty and there is no products

            yield return new object[]
            {
                new ProductGetListTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = string.Empty,
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    Expected =  null,
                }
            };

            #endregion Name is empty and there is no products

            #region Name is empty and there are products

            yield return new object[]
            {
                new ProductGetListTestData
                {
                    Setup = FiveGenericProduct(),
                    Input = string.Empty,
                    ExpectedStatusCode = HttpStatusCode.OK,
                    Expected = new ProductsDto { Items = CorrectFiveProduct().ToList() },
                }
            };

            #endregion Name is empty and there are products

            #region Name has value and there is no products

            yield return new object[]
            {
                new ProductGetListTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = "Potato",
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    Expected =  null,
                }
            };

            #endregion Name has value and there is no products

            #region Name has value and there are products

            yield return new object[]
            {
                new ProductGetListTestData
                {
                    Setup = FiveGenericProduct(),
                    Input = "Potato",
                    ExpectedStatusCode = HttpStatusCode.OK,
                    Expected = new ProductsDto { Items = CorrectNameProducts().ToList() },
                }
            };

            #endregion Name has value and there are products
        }

        private static ProductDto[] CorrectNameProducts() =>
            new[]
            {
                new ProductDtoSetup().WithDefault().WithName("Potato cakes"),
                new ProductDtoSetup().WithDefault().WithName("Potato scallops"),
                new ProductDtoSetup().WithDefault().WithName("Sauce and potato"),
            };

        private static ProductDto[] CorrectFiveProduct() =>
            new[]
            {
                new ProductDtoSetup().WithDefault().WithName("Potato cakes"),
                new ProductDtoSetup().WithDefault().WithName("Potato scallops"),
                new ProductDtoSetup().WithDefault().WithName("Baked Beans"),
                new ProductDtoSetup().WithDefault().WithName("Sauce and potato"),
                new ProductDtoSetup().WithDefault().WithName("Jerky"),
            };

        private static ProductDtoSetup[] FiveGenericProduct() =>
            new[]
            {
                new ProductDtoSetup().WithDefault().WithName("Potato cakes"),
                new ProductDtoSetup().WithDefault().WithName("Potato scallops"),
                new ProductDtoSetup().WithDefault().WithName("Baked Beans"),
                new ProductDtoSetup().WithDefault().WithName("Sauce and potato"),
                new ProductDtoSetup().WithDefault().WithName("Jerky"),
            };

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}