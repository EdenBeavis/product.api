using product.api.test.Fakes.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace product.api.test.Tests.TestData
{
    public class ProductDeleteTestData
    {
        public ProductDtoSetup[] Setup { get; set; }
        public Guid Input { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; }
        public string Expected { get; set; }
    }

    public class ProductDeleteTest : IEnumerable<object[]>
    {
        private static Guid _idToDelete;

        public ProductDeleteTest()
        {
            _idToDelete = Guid.NewGuid();
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            #region Guid id is empty

            yield return new object[]
            {
                new ProductDeleteTestData
                {
                    Setup = TwoGenericProducts(),
                    Input = Guid.Empty,
                    ExpectedStatusCode = HttpStatusCode.BadRequest,
                    Expected = "Invalid Id.",
                }
            };

            #endregion Guid id is empty

            #region Delete a Product that is created

            yield return new object[]
            {
                new ProductDeleteTestData
                {
                    Setup = TwoGenericProducts(),
                    Input = _idToDelete,
                    ExpectedStatusCode = HttpStatusCode.OK,
                    Expected = string.Empty,
                }
            };

            #endregion Delete a Product that is created

            #region Delete a product that is not creaded

            yield return new object[]
            {
                new ProductDeleteTestData
                {
                    Setup = OneGenericProduct(),
                    Input = _idToDelete,
                    ExpectedStatusCode = HttpStatusCode.NotFound,
                    Expected = "Product does not exist.",
                }
            };

            #endregion Delete a product that is not creaded
        }

        private static ProductDtoSetup[] OneGenericProduct() =>
            new[]
            {
                new ProductDtoSetup().WithDefault()
            };

        private static ProductDtoSetup[] TwoGenericProducts() =>
            new[]
            {
                new ProductDtoSetup().WithDefault().WithId(_idToDelete),
                new ProductDtoSetup().WithDefault()
            };

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}