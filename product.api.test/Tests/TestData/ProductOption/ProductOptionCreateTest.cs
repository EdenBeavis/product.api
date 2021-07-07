using product.api.Models.ProductOptions;
using product.api.test.Fakes.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace product.api.test.Tests.TestData
{
    public class ProductOptionCreateTestData
    {
        public ProductDtoSetup[] Setup { get; set; }
        public Guid ProductId { get; set; }
        public ProductOptionDto Input { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; }
        public string Expected { get; set; }
    }

    public class ProductOptionCreateTest : IEnumerable<object[]>
    {
        private static Guid _productId;

        public ProductOptionCreateTest()
        {
            _productId = Guid.NewGuid();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            #region Guid id is not empty

            yield return new object[]
            {
                new ProductOptionCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    ProductId = _productId,
                    Input = new ProductOptionDto().WithDefault().WithId(Guid.NewGuid()),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "New product options must have an empty Id.",
                }
            };

            #endregion Guid id is not empty

            #region product id is empty

            yield return new object[]
            {
                new ProductOptionCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    ProductId = Guid.Empty,
                    Input = new ProductOptionDto().WithDefault(),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Product id must have a value.",
                }
            };

            #endregion product id is empty

            #region Name Longer than 250

            yield return new object[]
            {
                new ProductOptionCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    ProductId = _productId,
                    Input = new ProductOptionDto().WithDefault().WithName(SuperLongString()),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = $"Name must be less than 250 characters.",
                }
            };

            #endregion Name Longer than 250

            #region Description Longer than 500

            yield return new object[]
            {
                new ProductOptionCreateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    ProductId = _productId,
                    Input = new ProductOptionDto().WithDefault().WithDescription(SuperLongString()),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Description must be less than 500 characters.",
                }
            };

            #endregion Description Longer than 500

            #region Add A product option when a product option is already created

            yield return new object[]
            {
                new ProductOptionCreateTestData
                {
                    Setup = OneGenericProduct(),
                    ProductId = _productId,
                    Input = new ProductOptionDto().WithDefault(),
                    ExpectedStatusCode = HttpStatusCode.Created,
                    Expected = string.Empty,
                }
            };

            #endregion Add A product option when a product option is already created

            #region Add a product option when no products other product optoions are created

            yield return new object[]
            {
                new ProductOptionCreateTestData
                {
                    Setup = OneGenericProductNoOptions(),
                    ProductId = _productId,
                    Input = new ProductOptionDto().WithDefault(),
                    ExpectedStatusCode = HttpStatusCode.Created,
                    Expected = string.Empty,
                }
            };

            #endregion Add a product option when no products other product optoions are created
        }

        private static ProductDtoSetup[] OneGenericProduct() =>
            new[]
            {
                new ProductDtoSetup().WithDefault().WithThreeDefaultProductOptions().WithId(_productId)
            };

        private static ProductDtoSetup[] OneGenericProductNoOptions() =>
            new[]
            {
                new ProductDtoSetup().WithDefault().WithId(_productId)
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