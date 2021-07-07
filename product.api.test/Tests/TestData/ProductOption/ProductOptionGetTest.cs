using product.api.Models.ProductOptions;
using product.api.test.Fakes.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace product.api.test.Tests.TestData
{
    public class ProductOptionGetTestData
    {
        public ProductDtoSetup[] Setup { get; set; }
        public Guid ProductId { get; set; }
        public Guid Input { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; }
        public ProductOptionDto ExpectedObject { get; set; }
        public string ExpectedString { get; set; }
    }

    public class ProductOptionGetTest : IEnumerable<object[]>
    {
        private static Guid _idToGet;
        private static Guid _productId;

        public ProductOptionGetTest()
        {
            _idToGet = Guid.NewGuid();
            _productId = Guid.NewGuid();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            #region Guid id is empty

            yield return new object[]
            {
                new ProductOptionGetTestData
                {
                    Setup = ThreeGenericProduct(),
                    ProductId = _productId,
                    Input = Guid.Empty,
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedObject = null,
                    ExpectedString = "Invalid Id."
                }
            };

            #endregion Guid id is empty

            #region Get by id and there is no product option

            yield return new object[]
            {
                new ProductOptionGetTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    ProductId = _productId,
                    Input = _idToGet,
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    ExpectedObject = null,
                    ExpectedString = $"No product option found with Id: {_idToGet}."
                }
            };

            #endregion Get by id and there is no product option

            #region Get by id and the product option is not found

            yield return new object[]
            {
                new ProductOptionGetTestData
                {
                    Setup = ThreeGenericProduct().Where(p => !p.Id.Equals(_productId)).ToArray(),
                    ProductId = _productId,
                    Input = _idToGet,
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    ExpectedObject = null,
                    ExpectedString = $"No product option found with Id: {_idToGet}."
                }
            };

            #endregion Get by id and the product option is not found

            #region Get by id and the product is found

            yield return new object[]
            {
                new ProductOptionGetTestData
                {
                    Setup = ThreeGenericProduct(),
                    ProductId = _productId,
                    Input = _idToGet,
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedObject = CorrectProduct(),
                    ExpectedString = string.Empty
                }
            };

            #endregion Get by id and the product is found
        }

        private static ProductOptionDto[] TwoGenericOptions() =>
            new[]
            {
                new ProductOptionDto().WithDefault().WithId(_idToGet).WithName("Salted"),
                new ProductOptionDto().WithDefault().WithName("Unsalted")
            };

        private static ProductOptionDto CorrectProduct() =>
                    new ProductOptionDto().WithDefault().WithId(_idToGet).WithName("Salted");

        private static ProductDtoSetup[] ThreeGenericProduct() =>
            new[]
            {
                new ProductDtoSetup().WithDefault().WithProductOptions(TwoGenericOptions()).WithId(_productId).WithName("Potato cakes"),
                new ProductDtoSetup().WithDefault().WithThreeDefaultProductOptions().WithName("Potato scallops"),
                new ProductDtoSetup().WithDefault().WithThreeDefaultProductOptions().WithName("Baked Beans")
            };

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}