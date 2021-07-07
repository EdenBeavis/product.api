using product.api.Models.ProductOptions;
using product.api.test.Fakes.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace product.api.test.Tests.TestData
{
    public class ProductOptionGetListTestData
    {
        public ProductDtoSetup[] Setup { get; set; }
        public Guid Input { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; }
        public ProductOptionsDto ExpectedObject { get; set; }
        public string ExpectedString { get; set; }
    }

    public class ProductOptionGetListTest : IEnumerable<object[]>
    {
        private static Guid _productId;

        public ProductOptionGetListTest()
        {
            _productId = Guid.NewGuid();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            #region Guid id is empty

            yield return new object[]
            {
                new ProductOptionGetListTestData
                {
                    Setup = ThreeGenericProduct(),
                    Input = Guid.Empty,
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    ExpectedObject = null,
                    ExpectedString = "Invalid product Id."
                }
            };

            #endregion Guid id is empty

            #region There are no product options created

            yield return new object[]
            {
                new ProductOptionGetListTestData
                {
                    Setup = Array.Empty<ProductDtoSetup>(),
                    Input = _productId,
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    ExpectedObject = null,
                    ExpectedString = $"No product options found for product Id: {_productId}"
                }
            };

            #endregion There are no product options created

            #region There are no product options created for specific id

            yield return new object[]
            {
                new ProductOptionGetListTestData
                {
                    Setup = ThreeGenericProduct().Where(p => !p.Id.Equals(_productId)).ToArray(),
                    Input = _productId,
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    ExpectedObject = null,
                    ExpectedString = $"No product options found for product Id: {_productId}"
                }
            };

            #endregion There are no product options created for specific id

            #region Product option is created and retrieved

            yield return new object[]
            {
                new ProductOptionGetListTestData
                {
                    Setup = ThreeGenericProduct(),
                    Input = _productId,
                    ExpectedStatusCode = HttpStatusCode.OK,
                    ExpectedObject = new ProductOptionsDto { Items = TwoGenericOptions().ToList() },
                    ExpectedString = string.Empty
                }
            };

            #endregion Product option is created and retrieved
        }

        private static ProductOptionDto[] TwoGenericOptions() =>
            new[]
            {
                new ProductOptionDto().WithDefault().WithName("Salted"),
                new ProductOptionDto().WithDefault().WithName("Unsalted")
            };

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