using Bolt.Common.Extensions;
using FluentAssertions;
using FluentAssertions.Equivalency;
using product.api.Models.Products;
using product.api.test.Fixtures;
using product.api.test.Infrastructure;
using product.api.test.Tests.TestData;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace product.api.test
{
    public class ProductsUnitTest : IClassFixture<ProductsTestFixture>
    {
        private readonly ProductsTestFixture _fixture;

        public ProductsUnitTest(ProductsTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.RefDataProvider.SetupRefData();
        }

        #region Get By Id

        [Theory]
        [ClassData(typeof(ProductGetTest))]
        public async Task WhenASingleProductsIsBeingRetrievedThenTryAndReturnTheProduct(ProductGetTestData testData)
        {
            // Arrange
            if (testData.Setup.HasItem()) await _fixture.CreateProducts(testData.Setup);

            // Act
            var (message, stringContent) = await _fixture.GetProducts($"/{testData.Input}/");

            //Assert
            var response = stringContent;

            message.StatusCode.Should().Be(testData.ExpectedStatusCode);
            response.Should().NotBeNullOrEmpty();

            if (testData.ExpectedObject != null && response.TryParseJson<ProductDto>(out var product))
                product.Should().BeEquivalentTo(testData.ExpectedObject, (options) => options.Excluding((IMemberInfo Mi) => Mi.SelectedMemberPath.EndsWith("Id")));
            else
                response.Should().Be(testData.ExpectedString);
        }

        #endregion Get By Id

        #region Get List

        [Theory]
        [ClassData(typeof(ProductGetListTest))]
        public async Task WhenAListOfProductsIsBeingRetrievedThenTryAndReturnTheProducts(ProductGetListTestData testData)
        {
            // Arrange
            if (testData.Setup.HasItem()) await _fixture.CreateProducts(testData.Setup);

            // Act
            var (message, stringContent) = await _fixture.GetProducts($"?name={testData.Input}");

            //Assert
            var response = stringContent;

            message.StatusCode.Should().Be(testData.ExpectedStatusCode);
            response.Should().NotBeNullOrEmpty();

            if (response.TryParseJson<ProductsDto>(out var products))
                products.Should().BeEquivalentTo(testData.Expected, (options) => options.Excluding((IMemberInfo Mi) => Mi.SelectedMemberPath.EndsWith("Id")));
            else
                response.Should().Be("No products found.");
        }

        #endregion Get List

        #region Create

        [Theory]
        [ClassData(typeof(ProductCreateTest))]
        public async Task WhenANewProductIsPostedThenTryAndProcessTheProduct(ProductCreateTestData testData)
        {
            // Arrange
            if (testData.Setup.HasItem()) await _fixture.CreateProducts(testData.Setup);

            // Act
            var (message, stringContent) = await _fixture.PostProduct(testData.Input);

            //Assert
            var response = stringContent;

            message.StatusCode.Should().Be(testData.ExpectedStatusCode);
            response.Should().NotBeNullOrEmpty();

            if (response.TryParseJson<Guid>(out var responseId))
                responseId.Should().NotBe(Guid.Empty);
            else
                response.Should().Be(testData.Expected);
        }

        #endregion Create

        #region Update

        [Theory]
        [ClassData(typeof(ProductUpdateTest))]
        public async Task WhenAProductIsBeingUpdatedThenTryAndProcessTheUpdate(ProductUpdateTestData testData)
        {
            // Arrange
            if (testData.Setup.HasItem()) await _fixture.CreateProducts(testData.Setup);

            // Act
            var (message, stringContent) = await _fixture.UpdateProduct(testData.InputId, testData.InputObject);

            //Assert
            var response = stringContent;

            message.StatusCode.Should().Be(testData.ExpectedStatusCode);
            response.Should().NotBeNullOrEmpty();

            if (response.TryParseJson<Guid>(out var responseId))
                responseId.Should().Be(testData.Expected);
            else
                response.Should().Be(testData.Expected);
        }

        #endregion Update

        #region Delete

        [Theory]
        [ClassData(typeof(ProductDeleteTest))]
        public async Task WhenAProductIsBeingDeletedThenTryAndProcessTheDelete(ProductDeleteTestData testData)
        {
            // Arrange
            if (testData.Setup.HasItem()) await _fixture.CreateProducts(testData.Setup);

            // Act
            var (message, stringContent) = await _fixture.DeleteProduct(testData.Input);

            //Assert
            var response = stringContent;

            message.StatusCode.Should().Be(testData.ExpectedStatusCode);
            if (!testData.ExpectedStatusCode.Equals(HttpStatusCode.OK)) response.Should().NotBeNullOrEmpty();
            response.Should().Be(testData.Expected);
        }

        #endregion Delete
    }
}