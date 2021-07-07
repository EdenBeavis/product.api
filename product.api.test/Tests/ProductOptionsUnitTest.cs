using Bolt.Common.Extensions;
using FluentAssertions;
using FluentAssertions.Equivalency;
using product.api.Models.ProductOptions;
using product.api.test.Fixtures;
using product.api.test.Infrastructure;
using product.api.test.Tests.TestData;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace product.api.test
{
    public class ProductOptionsUnitTest : IClassFixture<ProductOptionsTestFixture>
    {
        private readonly ProductOptionsTestFixture _fixture;

        public ProductOptionsUnitTest(ProductOptionsTestFixture fixture)
        {
            _fixture = fixture;
            _fixture.RefDataProvider.SetupRefData();
        }

        #region Get By Id

        [Theory]
        [ClassData(typeof(ProductOptionGetTest))]
        public async Task WhenASingleProductOptionIsBeingRetrievedThenTryAndReturnTheProductOption(ProductOptionGetTestData testData)
        {
            // Arrange
            if (testData.Setup.HasItem()) await _fixture.CreateProducts(testData.Setup);

            // Act
            var (message, stringContent) = await _fixture.GetProductOptions(testData.ProductId, testData.Input.ToString());

            //Assert
            var response = stringContent;

            message.StatusCode.Should().Be(testData.ExpectedStatusCode);
            response.Should().NotBeNullOrEmpty();

            if (testData.ExpectedObject != null && response.TryParseJson<ProductOptionDto>(out var productOption))
                productOption.Should().BeEquivalentTo(testData.ExpectedObject, (options) => options.Excluding((IMemberInfo Mi) => Mi.SelectedMemberPath.EndsWith("Id")));
            else
                response.Should().Be(testData.ExpectedString);
        }

        #endregion Get By Id

        #region Get List

        [Theory]
        [ClassData(typeof(ProductOptionGetListTest))]
        public async Task WhenAListOfProductOptionsIsBeingRetrievedThenTryAndReturnTheProductOptions(ProductOptionGetListTestData testData)
        {
            // Arrange
            if (testData.Setup.HasItem()) await _fixture.CreateProducts(testData.Setup);

            // Act
            var (message, stringContent) = await _fixture.GetProductOptions(testData.Input);

            //Assert
            var response = stringContent;

            message.StatusCode.Should().Be(testData.ExpectedStatusCode);
            response.Should().NotBeNullOrEmpty();

            if (testData.ExpectedObject != null && response.TryParseJson<ProductOptionsDto>(out var productOptions))
                productOptions.Should().BeEquivalentTo(testData.ExpectedObject, (options) => options.Excluding((IMemberInfo Mi) => Mi.SelectedMemberPath.EndsWith("Id")));
            else
                response.Should().Be(testData.ExpectedString);
        }

        #endregion Get List

        #region Create

        [Theory]
        [ClassData(typeof(ProductOptionCreateTest))]
        public async Task WhenANewProductOptionIsPostedThenTryAndProcessTheProductOption(ProductOptionCreateTestData testData)
        {
            // Arrange
            if (testData.Setup.HasItem()) await _fixture.CreateProducts(testData.Setup);

            // Act
            var (message, stringContent) = await _fixture.PostProductOption(testData.ProductId, testData.Input);

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
        [ClassData(typeof(ProductOptionUpdateTest))]
        public async Task WhenAProductOptionIsBeingUpdatedThenTryAndProcessTheUpdate(ProductOptionUpdateTestData testData)
        {
            // Arrange
            if (testData.Setup.HasItem()) await _fixture.CreateProducts(testData.Setup);

            // Act
            var (message, stringContent) = await _fixture.UpdateProductOption(testData.ProductId, testData.InputId, testData.InputObject);

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
        [ClassData(typeof(ProductOptionDeleteTest))]
        public async Task WhenAProductOptionsIsBeingDeletedThenTryAndProcessTheDelete(ProductOptionDeleteTestData testData)
        {
            // Arrange
            if (testData.Setup.HasItem()) await _fixture.CreateProducts(testData.Setup);

            // Act
            var (message, stringContent) = await _fixture.DeleteProductOption(testData.ProductId, testData.Input);

            //Assert
            var response = stringContent;

            message.StatusCode.Should().Be(testData.ExpectedStatusCode);
            if (!testData.ExpectedStatusCode.Equals(HttpStatusCode.OK)) response.Should().NotBeNullOrEmpty();
            response.Should().Be(testData.Expected);
        }

        #endregion Delete
    }
}