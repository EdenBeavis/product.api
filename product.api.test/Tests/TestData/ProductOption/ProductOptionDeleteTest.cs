using product.api.Models.ProductOptions;
using product.api.test.Fakes.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace product.api.test.Tests.TestData
{
    public class ProductOptionDeleteTestData
    {
        public ProductDtoSetup[] Setup { get; set; }
        public Guid ProductId { get; set; }
        public Guid Input { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; }
        public string Expected { get; set; }
    }

    public class ProductOptionDeleteTest : IEnumerable<object[]>
    {
        private static Guid _productId;
        private static Guid _idToDelete;

        public ProductOptionDeleteTest()
        {
            _productId = Guid.NewGuid();
            _idToDelete = Guid.NewGuid();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            #region Guid id is empty

            yield return new object[]
            {
                new ProductOptionDeleteTestData
                {
                    Setup = TwoGenericProducts(),
                    ProductId = _productId,
                    Input = Guid.Empty,
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Invalid Id.",
                }
            };

            #endregion Guid id is empty

            #region Delete a Product option that is created

            yield return new object[]
            {
                new ProductOptionDeleteTestData
                {
                    Setup = TwoGenericProducts(),
                    ProductId = _productId,
                    Input = _idToDelete,
                    ExpectedStatusCode = HttpStatusCode.OK,
                    Expected = string.Empty,
                }
            };

            #endregion Delete a Product option that is created

            #region Delete a product that is not created

            yield return new object[]
            {
                new ProductOptionDeleteTestData
                {
                    Setup = TwoGenericProducts().Where(p => !p.Id.Equals(_productId)).ToArray(),
                    Input = _idToDelete,
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    Expected = "Product option does not exist.",
                }
            };

            #endregion Delete a product that is not created
        }

        private static ProductOptionDto[] TwoGenericOptions() =>
            new[]
            {
                new ProductOptionDto().WithDefault().WithName("Salted").WithId(_idToDelete),
                new ProductOptionDto().WithDefault().WithName("Unsalted")
            };

        private static ProductDtoSetup[] TwoGenericProducts() =>
            new[]
            {
                new ProductDtoSetup().WithDefault().WithProductOptions(TwoGenericOptions()).WithId(_productId),
                new ProductDtoSetup().WithDefault().WithThreeDefaultProductOptions()
            };

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}