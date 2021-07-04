using Xunit;

namespace product.api.test
{
    public class UnitTest1 : IClassFixture<OrdersTestFixture>
    {
        private readonly OrdersTestFixture _fixture;

        public OrdersUnitTest(OrdersTestFixture fixture)
        {
            _fixture = fixture;
        }

        #region When an order is created and is being retireved by Id

        [Fact]
        public async Task ThenReturnOrderWithId()
        {
            _fixture.RefDataProvider.SetupRefDataForOrders();

            // Arrange
            var id = 5;
            await _fixture.CreateSingleDefaultOrder(id);

            // Act
            var (message, stringContent) = await _fixture.GetOrders(id.ToString());

            //Assert
            var expected = new OrderDto().WithDefault().WithId(5);
            var response = JsonConvert.DeserializeObject<OrderDto>(stringContent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

            message.EnsureSuccessStatusCode();
            response.Should().NotBeNull();
            response.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [ClassData(typeof(OrdersGetTest))]
        public async Task ThenReturnOrdersWithQueryedStatusOrId(OrdersGetTestData testData)
        {
            _fixture.RefDataProvider.SetupRefDataForOrders();

            // Arrange
            await _fixture.CreateOrders(testData.Setup);

            // Act
            var (message, stringContent) = await _fixture.GetOrders(testData.Query);

            //Assert
            var response = JsonConvert.DeserializeObject<IEnumerable<OrderDto>>(stringContent, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

            message.EnsureSuccessStatusCode();
            response.Should().BeEquivalentTo(testData.Expected);
        }
    }
}