using product.api.Models.Products;
using product.api.test.Fakes.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace product.api.test.Tests.TestData
{
    public class ProductUpdateTestData
    {
        public ProductDtoSetup[] Setup { get; set; }
        public Guid InputId { get; set; }
        public ProductDto InputObject { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; }
        public string Expected { get; set; }
    }

    public class ProductUpdateTest : IEnumerable<object[]>
    {
        private static Guid _idToUpdate;

        public ProductUpdateTest()
        {
            _idToUpdate = Guid.NewGuid();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            #region Guid id is empty

            yield return new object[]
            {
                new ProductUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    InputId = Guid.Empty,
                    InputObject = new ProductDtoSetup().WithDefault().WithName("Updated").WithId(Guid.NewGuid()),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Invalid Id.",
                }
            };

            #endregion Guid id is empty

            #region Name Longer than 250

            yield return new object[]
            {
                new ProductUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    InputId = _idToUpdate,
                    InputObject = new ProductDtoSetup().WithDefault().WithName(SuperLongString()),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = $"Name must be less than 250 characters.",
                }
            };

            #endregion Name Longer than 250

            #region Description Longer than 500

            yield return new object[]
            {
                new ProductUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    InputId = _idToUpdate,
                    InputObject = new ProductDtoSetup().WithDefault().WithDescription(SuperLongString()),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Description must be less than 500 characters.",
                }
            };

            #endregion Description Longer than 500

            #region Price less than 0.0

            yield return new object[]
            {
                new ProductUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    InputId = _idToUpdate,
                    InputObject = new ProductDtoSetup().WithDefault().WithPrice(-1m),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Price must be a greater or equal to 0.0.",
                }
            };

            #endregion Price less than 0.0

            #region Delievery price less than 0.0

            yield return new object[]
            {
                new ProductUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    InputId = _idToUpdate,
                    InputObject = new ProductDtoSetup().WithDefault().WithDelieveryPrice(-1m),
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Delivery price must be a greater or equal to 0.0.",
                }
            };

            #endregion Delievery price less than 0.0

            #region Price equal to 0.0

            yield return new object[]
            {
                new ProductUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    InputId = _idToUpdate,
                    InputObject = new ProductDtoSetup().WithDefault().WithPrice(0.0m),
                    ExpectedStatusCode = HttpStatusCode.OK,
                    Expected = _idToUpdate.ToString(),
                }
            };

            #endregion Price equal to 0.0

            #region Delievery price equal to 0.0

            yield return new object[]
            {
                new ProductUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    InputId = _idToUpdate,
                    InputObject = new ProductDtoSetup().WithDefault().WithDelieveryPrice(0.0m),
                    ExpectedStatusCode = HttpStatusCode.OK,
                    Expected = _idToUpdate.ToString(),
                }
            };

            #endregion Delievery price equal to 0.0

            #region Update A product that is created

            yield return new object[]
            {
                new ProductUpdateTestData
                {
                    Setup = TwoGenericProducts(),
                    InputId = _idToUpdate,
                    InputObject = new ProductDtoSetup().WithDefault().WithName("Updated"),
                    ExpectedStatusCode = HttpStatusCode.OK,
                    Expected = _idToUpdate.ToString(),
                }
            };

            #endregion Update A product that is created

            #region Update a product that is not in the database

            yield return new object[]
            {
                new ProductUpdateTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    InputId = Guid.NewGuid(),
                    InputObject = new ProductDtoSetup().WithDefault().WithName("Updated"),
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    Expected = "Product does not exist.",
                }
            };

            #endregion Update a product that is not in the database
        }

        private static ProductDtoSetup[] TwoGenericProducts() =>
            new[]
            {
                new ProductDtoSetup().WithDefault().WithId(_idToUpdate),
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