using product.api.Models.Products;
using product.api.test.Fakes.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace product.api.test.Tests.TestData
{
    public class ProductCreateTestData
    {
        public ProductDtoSetup[] Setup { get; set; }
        public ProductDto Input { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; }
        public string Expected { get; set; }
    }

    public class ProductCreateTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            #region Guid id is not empty

            yield return new object[]
            {
                new ProductCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = new ProductDtoSetup().WithDefault().WithId(Guid.NewGuid()),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "New products must have an empty Id.",
                }
            };

            #endregion Guid id is not empty

            #region Name Longer than 250

            yield return new object[]
            {
                new ProductCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = new ProductDtoSetup().WithDefault().WithName(SuperLongString()),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = $"Name must be less than 250 characters.",
                }
            };

            #endregion Name Longer than 250

            #region Description Longer than 500

            yield return new object[]
            {
                new ProductCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = new ProductDtoSetup().WithDefault().WithDescription(SuperLongString()),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Description must be less than 500 characters.",
                }
            };

            #endregion Description Longer than 500

            #region Price less than 0.0

            yield return new object[]
            {
                new ProductCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = new ProductDtoSetup().WithDefault().WithPrice(-1m),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Price must be a greater or equal to 0.0.",
                }
            };

            #endregion Price less than 0.0

            #region Delievery price less than 0.0

            yield return new object[]
            {
                new ProductCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = new ProductDtoSetup().WithDefault().WithDelieveryPrice(-1m),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Delivery price must be a greater or equal to 0.0.",
                }
            };

            #endregion Delievery price less than 0.0

            #region Price equal to 0.0

            yield return new object[]
            {
                new ProductCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = new ProductDtoSetup().WithDefault().WithPrice(0.0m),
                    ExpectedStatusCode = HttpStatusCode.Created,
                    Expected = string.Empty,
                }
            };

            #endregion Price equal to 0.0

            #region Delievery price equal to 0.0

            yield return new object[]
            {
                new ProductCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = new ProductDtoSetup().WithDefault().WithDelieveryPrice(0.0m),
                    ExpectedStatusCode = HttpStatusCode.Created,
                    Expected = string.Empty,
                }
            };

            #endregion Delievery price equal to 0.0

            #region Add A product when a product is already created

            yield return new object[]
            {
                new ProductCreateTestData
                {
                    Setup = OneGenericProduct(),
                    Input = new ProductDtoSetup().WithDefault(),
                    ExpectedStatusCode = HttpStatusCode.Created,
                    Expected = string.Empty,
                }
            };

            #endregion Add A product when a product is already created

            #region Add a product when no products are created

            yield return new object[]
            {
                new ProductCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = new ProductDtoSetup().WithDefault(),
                    ExpectedStatusCode = HttpStatusCode.Created,
                    Expected = string.Empty,
                }
            };

            #endregion Add a product when no products are created
        }

        private static ProductDtoSetup[] OneGenericProduct() =>
            new[]
            {
                new ProductDtoSetup().WithDefault()
            };

        private static string SuperLongString()
        {
            var longString = string.Empty;

            for (var i = 0; i < 600; i++)
                longString += "a";

            return longString;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}