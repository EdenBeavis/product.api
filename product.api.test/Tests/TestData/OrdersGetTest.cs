using ketobros.api.Features.Shared.Models;
using ketobros.api.Infrastructure;
using ketobros.api.test.Fakes.Buillders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ketobros.api.test.Tests.TestData.Orders
{
    public class OrdersGetTestData
    {
        public OrderDto[] Setup { get; set; }
        public string Query { get; set; }
        public IEnumerable<OrderDto> Expected { get; set; }
    }

    public class OrdersGetTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            #region status tests

            yield return new object[]
            {
                new OrdersGetTestData
                {
                    Setup = ThreeOfAllStatuses(),
                    Query = GetUrl(Array.Empty<int>(), OrderStatusValue.Processing),
                    Expected = ThreeStatusOrders(4, OrderStatusValue.Processing)
                }
            };
            yield return new object[]
            {
                new OrdersGetTestData
                {
                    Setup = ThreeOfAllStatuses(),
                    Query = GetUrl(Array.Empty<int>(), OrderStatusValue.Completed),
                    Expected = ThreeStatusOrders(10, OrderStatusValue.Completed)
                }
            };
            yield return new object[]
            {
                new OrdersGetTestData
                {
                    Setup = ThreeOfAllStatuses(),
                    Query = GetUrl(Array.Empty<int>(), OrderStatusValue.Pending),
                    Expected = ThreeStatusOrders(1, OrderStatusValue.Pending)
                }
            };
            yield return new object[]
            {
                new OrdersGetTestData
                {
                    Setup = ThreeOfAllStatuses(),
                    Query = GetUrl(Array.Empty<int>(), OrderStatusValue.Delivering),
                    Expected = ThreeStatusOrders(7, OrderStatusValue.Delivering)
                }
            };
            yield return new object[]
            {
                new OrdersGetTestData
                {
                    Setup = ThreeOfAllStatuses(),
                    Query = GetUrl(Array.Empty<int>(), OrderStatusValue.Cancelled),
                    Expected = ThreeStatusOrders(13, OrderStatusValue.Cancelled)
                }
            };
            yield return new object[]
            {
                new OrdersGetTestData
                {
                    Setup = ThreeOfAllStatuses(),
                    Query = GetUrl(Array.Empty<int>(), OrderStatusValue.Refunded),
                    Expected = ThreeStatusOrders(16, OrderStatusValue.Refunded)
                }
            };
            yield return new object[]
            {
                new OrdersGetTestData
                {
                    Setup = ThreeOfAllStatuses(),
                    Query = GetUrl(Array.Empty<int>(), OrderStatusValue.Any),
                    Expected = ThreeOfAllStatuses()
                }
            };

            #endregion status tests

            #region id tests

            yield return new object[]
            {
                new OrdersGetTestData
                {
                    Setup = ThreeOfAllStatuses(),
                    Query = GetUrl(new int[]{4,5,8}),
                    Expected = OrdersWithId(new (int Id, OrderStatusValue Status) []{
                        (4, OrderStatusValue.Processing),
                        (5, OrderStatusValue.Processing),
                        (8, OrderStatusValue.Delivering)
                    })
                }
            };
            yield return new object[]
            {
                new OrdersGetTestData
                {
                    Setup = ThreeOfAllStatuses(),
                    Query = GetUrl(new int[]{11,2,17}),
                    Expected = OrdersWithId(new (int Id, OrderStatusValue Status) []{
                        (2, OrderStatusValue.Pending),
                        (11, OrderStatusValue.Completed),
                        (17, OrderStatusValue.Refunded)
                    })
                }
            };
            yield return new object[]
            {
                new OrdersGetTestData
                {
                    Setup = Array.Empty<OrderDto>(),
                    Query = GetUrl(Array.Empty<int>()),
                    Expected = new List<OrderDto>()
                }
            };

            #endregion id tests
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private string GetUrl(int[] ids, OrderStatusValue status = OrderStatusValue.Any)
        {
            return $"?ids={string.Join("&ids=", ids)}&status={status}";
        }

        private static IEnumerable<OrderDto> OrdersWithId((int Id, OrderStatusValue Status)[] input)
        {
            return input.Select(i => new OrderDto().WithDefault().WithId(i.Id).WithStatus(i.Status));
        }

        private IEnumerable<OrderDto> ThreeStatusOrders(int id, OrderStatusValue status) => new OrderDto[]
            {
                new OrderDto().WithDefault().WithId(id).WithStatus(status),
                new OrderDto().WithDefault().WithId(++id).WithStatus(status),
                new OrderDto().WithDefault().WithId(++id).WithStatus(status)
            };

        private static OrderDto[] ThreeOfAllStatuses()
        {
            var orders = new List<OrderDto>();
            var orderId = 1;

            foreach (var status in ((OrderStatusValue[])Enum.GetValues(typeof(OrderStatusValue))).Where(s => s != OrderStatusValue.Any))
            {
                orders.AddRange(
                    new OrderDto[]
                    {
                        new OrderDto().WithDefault().WithId(orderId++).WithStatus(status),
                        new OrderDto().WithDefault().WithId(orderId++).WithStatus(status),
                        new OrderDto().WithDefault().WithId(orderId++).WithStatus(status)
                    });
            }

            return orders.ToArray();
        }
    }
}