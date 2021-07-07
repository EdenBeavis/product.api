using product.api.Models.Products;
using product.api.test.Fakes.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace product.api.test.Tests.TestData
{
    public class ProductGetTestData
    {
        public ProductDtoSetup[] Setup { get; set; }
        public Guid Input { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; }
        public ProductDto ExpectedObject { get; set; }
        public string ExpectedString { get; set; }
    }

    public class ProductGetTest : IEnumerable<object[]>
    {
        private static Guid _idToGet;

        public ProductGetTest()
        {
            _idToGet = Guid.NewGuid();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            #region Guid id is empty

            yield return new object[]
            {
                new ProductGetTestData
                {
                    Setup = FiveGenericProduct(),
                    Input = Guid.Empty,
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedObject = null,
                    ExpectedString = "Invalid Id."
                }
            };

            #endregion Guid id is empty

            #region Get by id and there is no products

            yield return new object[]
            {
                new ProductGetTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = _idToGet,
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    ExpectedObject = null,
                    ExpectedString = $"No product found with Id: {_idToGet}."
                }
            };

            #endregion Get by id and there is no products

            #region Get by id and the product is not found

            yield return new object[]
            {
                new ProductGetTestData
                {
                    Setup = FiveGenericProduct().Where(p => !p.Id.Equals(_idToGet)).ToArray(),
                    Input = _idToGet,
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    ExpectedObject = null,
                    ExpectedString = $"No product found with Id: {_idToGet}."
                }
            };

            #endregion Get by id and the product is not found

            #region Get by id and the product is found

            yield return new object[]
            {
                new ProductGetTestData
                {
                    Setup = FiveGenericProduct(),
                    Input = _idToGet,
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedObject = CorrectProduct(),
                    ExpectedString = string.Empty
                }
            };

            #endregion Get by id and the product is found
        }

        private static ProductDto CorrectProduct() =>
                new ProductDtoSetup().WithDefault().WithId(_idToGet).WithName("Potato cakes");

        private static ProductDtoSetup[] FiveGenericProduct() =>
            new[]
            {
                new ProductDtoSetup().WithDefault().WithId(_idToGet).WithName("Potato cakes"),
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