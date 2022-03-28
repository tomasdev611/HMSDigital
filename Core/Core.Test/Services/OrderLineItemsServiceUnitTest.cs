using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Services
{
    public class OrderLineItemsServiceUnitTest
    {
        private readonly IOrderLineItemsService _orderLineItemsService;

        public OrderLineItemsServiceUnitTest()
        {
            var mockService = new MockServices();
            _orderLineItemsService = mockService.GetService<IOrderLineItemsService>();
        }

        [Fact]
        public async Task GetOrderLineItemsShouldReturnValidList()
        {
            var orderLineItemsResult = await _orderLineItemsService.GetAllOrderLineItems();
            Assert.NotEmpty(orderLineItemsResult);
        }

        [Fact]
        public async Task ShouldGetOrderLineItemForValidOrderLineItemId()
        {
            var orderLineItemResult = await _orderLineItemsService.GetOrderLineItemById(1);
            Assert.NotNull(orderLineItemResult);
        }

        [Fact]
        public async Task ShouldNotGetOrderLineItemForInValidOrderLineItemId()
        {
            var orderLineItemResult = await _orderLineItemsService.GetOrderLineItemById(int.MaxValue);
            Assert.Null(orderLineItemResult);
        }
    }
}
