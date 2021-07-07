using product.api.Models.ProductOptions;
using product.api.test.Fakes.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace product.api.test.Tests.TestData
{
    public class ProductOptionUpdateTestData
    {
        public ProductDtoSetup[] Setup { get; set; }
        public Guid ProductId { get; set; }
        public Guid InputId { get; set; }
        public ProductOptionDto InputObject { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; }
        public string Expected { get; set; }
    }

    public class ProductOptionUpdateTest : IEnumerable<object[]>
    {
        private static Guid _productId;
        private static Guid _idToUpdate;

        public ProductOptionUpdateTest()
        {
            _productId = Guid.NewGuid();
            _idToUpdate = Guid.NewGuid();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            #region Guid id is empty

            yield return new object[]
            {
                new ProductOptionUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    ProductId = _productId,
                    InputId = Guid.Empty,
                    InputObject = new ProductOptionDto().WithDefault().WithName("Updated"),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Invalid Id.",
                }
            };

            #endregion Guid id is empty

            #region Name Longer than 250

            yield return new object[]
            {
                new ProductOptionUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    ProductId = _productId,
                    InputId = _idToUpdate,
                    InputObject = new ProductOptionDto().WithDefault().WithName(SuperLongString()),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = $"Name must be less than 250 characters.",
                }
            };

            #endregion Name Longer than 250

            #region Description Longer than 500

            yield return new object[]
            {
                new ProductOptionUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    ProductId = _productId,
                    InputId = _idToUpdate,
                    InputObject = new ProductOptionDto().WithDefault().WithDescription(SuperLongString()),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Description must be less than 500 characters.",
                }
            };

            #endregion Description Longer than 500

            #region Update A product that is created

            yield return new object[]
            {
                new ProductOptionUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    ProductId = _productId,
                    InputId = _idToUpdate,
                    InputObject = new ProductOptionDto().WithDefault().WithName("Updated"),
                    ExpectedStatusCode = HttpStatusCode.OK,
                    Expected = _idToUpdate.ToString(),
                }
            };

            #endregion Update A product that is created

            #region Update a product option that is not in the database

            yield return new object[]
            {
                new ProductOptionUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    ProductId = _productId,
                    InputId = Guid.NewGuid(),
                    InputObject = new ProductOptionDto().WithDefault().WithName("Updated"),
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    Expected = "Product option does not exist.",
                }
            };

            #endregion Update a product option that is not in the database
        }

        private static ProductOptionDto[] TwoGenericOptions() =>
            new[]
            {
                new ProductOptionDto().WithDefault().WithName("Salted").WithId(_idToUpdate),
                new ProductOptionDto().WithDefault().WithName("Unsalted")
            };

        private static ProductDtoSetup[] TwoGenericProducts() =>
            new[]
            {
                new ProductDtoSetup().WithDefault().WithProductOptions(TwoGenericOptions()).WithId(_productId),
                new ProductDtoSetup().WithDefault().WithThreeDefaultProductOptions()
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