using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Models = HMSDigital.Core.Data.Models;
using ViewModels = HMSDigital.Core.ViewModels;

namespace Core.Test.Services
{
    public class OrderHeadersServiceUnitTest
    {
        private readonly IOrderHeadersService _orderHeadersService;

        private readonly IOrderFulfillmentLineItemsRepository _orderFulfillmentLineItemsRepository;

        private readonly SieveModel _sieveModel;

        private readonly MockModels _mockModels;

        private readonly MockViewModels _mockViewModels;

        public OrderHeadersServiceUnitTest()
        {
            var mockService = new MockServices();
            _orderHeadersService = mockService.GetService<IOrderHeadersService>();
            _orderFulfillmentLineItemsRepository = mockService.GetService<IOrderFulfillmentLineItemsRepository>();
            _sieveModel = new SieveModel();
            _mockModels = mockService.GetService<MockModels>();
            _mockViewModels = new MockViewModels();
        }

        [Fact]
        public async Task GetOrderHeadersShouldReturnValidList()
        {
            var orderHeadersResult = await _orderHeadersService.GetAllOrderHeaders(_sieveModel, true);
            Assert.NotEmpty(orderHeadersResult.Records);
        }

        [Fact]
        public async Task ShouldGetOrderHeaderForValidOrderHeaderId()
        {
            var orderHeaderResult = await _orderHeadersService.GetOrderHeaderById(1, true);
            Assert.NotNull(orderHeaderResult);
        }

        [Fact]
        public async Task ShouldNotGetOrderHeaderForInValidOrderHeaderId()
        {
            await Assert.ThrowsAsync<ValidationException>(() => _orderHeadersService.GetOrderHeaderById(int.MaxValue, false));
        }

        [Fact]
        public async Task GetOrderFulfillmentsShouldReturnValidList()
        {
            var orderFulfillments = await _orderHeadersService.GetOrderFulfillments(1, _sieveModel);
            Assert.NotEmpty(orderFulfillments.Records);
        }

        [Fact]
        public async Task SiteShouldAssignedToOrderForValidData()
        {
            var orderModel = _mockModels.OrderHeaders.FirstOrDefault(o => o.Id == 2);
            var isSiteAssign = await _orderHeadersService.AssignSiteToOrder(orderModel);
            Assert.True(isSiteAssign);
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(2, false);
            Assert.NotNull(updatedOrder);
            Assert.NotEqual(0, updatedOrder.SiteId);
        }

        [Fact]
        public async Task OrderNotesShouldBeCreatedForValidData()
        {
            var orderNotesRequest = _mockViewModels.GetOrderNotesRequests();
            var order = await _orderHeadersService.UpsertOrderNotes(2, orderNotesRequest);
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(2, false);
            Assert.NotNull(updatedOrder.OrderNotes);
            foreach(var noteRequest in orderNotesRequest)
            {
                var orderNote = updatedOrder.OrderNotes.FirstOrDefault(n => n.Note == noteRequest.Note);
                Assert.NotNull(orderNote);
            }
        }

        [Fact]
        public async Task OrderNotesShouldBeFailedForInvalidOrder()
        {
            var orderNotesRequest = _mockViewModels.GetOrderNotesRequests();
            await Assert.ThrowsAsync<ValidationException>(() => _orderHeadersService.UpsertOrderNotes(222, orderNotesRequest));
        }

        [Fact]
        public async Task OrderNotesShouldBeFailedForInvalidMember()
        {
            var orderNotesRequest = _mockViewModels.GetOrderNotesRequests();
            foreach(var orderNote in orderNotesRequest)
            {
                orderNote.HospiceMemberId = 222;
            }
            await Assert.ThrowsAsync<ValidationException>(() => _orderHeadersService.UpsertOrderNotes(222, orderNotesRequest));
        }

        #region PickupOrderFromWebPrortal

        [Fact]
        public async Task PickupOrderShouldBeCreatedFromWebPortal()
        {
            var orderLineItems = new List<CoreOrderLineItemRequest>()
            {
                GetWebPortalOrderLineItem(1, 1, "s100", "a100", OrderTypes.Pickup),
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Pickup)
            };
            var orderResponse = await CreateInitialOrderFromWebPortal(orderLineItems, OrderTypes.Pickup);
            Assert.Equal(2, orderResponse.OrderLineItems.Count());
            Assert.NotEmpty(orderResponse.OrderNumber);
            Assert.Equal(OrderTypes.Pickup.ToString(), orderResponse.OrderType);
        }

        [Fact]
        public async Task PickupOrderShouldBeUpdatedFromWebPortal()
        {
            var orderLineItems = new List<CoreOrderLineItemRequest>()
            {
                GetWebPortalOrderLineItem(1, 1, "s100", "a100", OrderTypes.Pickup),
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Pickup)
            };
            var orderResponse = await CreateInitialOrderFromWebPortal(orderLineItems, OrderTypes.Pickup);
            Assert.Equal(2, orderResponse.OrderLineItems.Count());
            Assert.NotEmpty(orderResponse.OrderNumber);
            Assert.Equal(OrderTypes.Pickup.ToString(), orderResponse.OrderType);

            var updatedLineItems = new List<CoreOrderLineItemRequest>()
            {
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Pickup)
            };
            var updatedOrderRequest = GetWebPortalOrderRequest(OrderHeaderStatusTypes.Planned, updatedLineItems, OrderTypes.Pickup);
            updatedOrderRequest.Id = orderResponse.Id;
            var updatedOrder = await _orderHeadersService.UpsertOrderFromWebPortal(updatedOrderRequest);
            Assert.Equal(orderResponse.Id, updatedOrder.Id);
            var updateOrderResponse = await _orderHeadersService.GetOrderHeaderById(updatedOrder.Id, false);
            Assert.Equal((int)OrderHeaderStatusTypes.Planned, updateOrderResponse.StatusId);
            Assert.Equal(orderResponse.OrderNumber, updatedOrder.OrderNumber);
            Assert.Equal(1, updatedOrder.OrderLineItems.Count());
        }

        [Fact]
        public async Task PickupOrderShouldUpdateStatusAsCompletedOnCancellingNonFulfillLineItemFromWebPortal()
        {
            var orderLineItems = new List<CoreOrderLineItemRequest>()
            {
                GetWebPortalOrderLineItem(1, 1, "s123", "", OrderTypes.Pickup),
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Pickup)
            };
            var orderResponse = await CreateInitialOrderFromWebPortal(orderLineItems, OrderTypes.Pickup);
            Assert.Equal(2, orderResponse.OrderLineItems.Count());
            Assert.NotEmpty(orderResponse.OrderNumber);
            Assert.Equal(OrderTypes.Pickup.ToString(), orderResponse.OrderType);

            var fulfilledItem = orderResponse.OrderLineItems.FirstOrDefault(l => l.ItemId == 1);
            await AddOrderLineFulfillment(orderResponse.Id, fulfilledItem.Id, fulfilledItem.ItemId, fulfilledItem.ItemCount);

            var updateLineItemRequest = GetWebPortalOrderLineItem(1, 1, "s123", "a123", OrderTypes.Pickup);
            updateLineItemRequest.Id = fulfilledItem.Id;
            var updatedLineItems = new List<CoreOrderLineItemRequest>()
            {
                updateLineItemRequest
            };
            var updatedOrderRequest = GetWebPortalOrderRequest(OrderHeaderStatusTypes.Planned, updatedLineItems, OrderTypes.Pickup);
            updatedOrderRequest.Id = orderResponse.Id;
            var updatedOrder = await _orderHeadersService.UpsertOrderFromWebPortal(updatedOrderRequest);
            Assert.Equal(orderResponse.Id, updatedOrder.Id);
            var updateOrderResponse = await _orderHeadersService.GetOrderHeaderById(updatedOrder.Id, false);
            Assert.Equal((int)OrderHeaderStatusTypes.Completed, updateOrderResponse.StatusId);
            Assert.Equal(orderResponse.OrderNumber, updatedOrder.OrderNumber);
            Assert.Equal(1, updatedOrder.OrderLineItems.Count());
        }

        [Fact]
        public async Task PickupOrderShouldUpdateStatusAsCancelledOnCancellingAllLineItemFromWebPortal()
        {
            var orderLineItems = new List<CoreOrderLineItemRequest>()
            {
                GetWebPortalOrderLineItem(1, 1, "s123", "", OrderTypes.Pickup),
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Pickup)
            };
            var orderResponse = await CreateInitialOrderFromWebPortal(orderLineItems, OrderTypes.Pickup);
            Assert.Equal(2, orderResponse.OrderLineItems.Count());
            Assert.NotEmpty(orderResponse.OrderNumber);
            Assert.Equal(OrderTypes.Pickup.ToString(), orderResponse.OrderType);

            var updatedLineItems = new List<CoreOrderLineItemRequest>();
            var updatedOrderRequest = GetWebPortalOrderRequest(OrderHeaderStatusTypes.Planned, updatedLineItems, OrderTypes.Pickup);
            updatedOrderRequest.Id = orderResponse.Id;
            var updatedOrder = await _orderHeadersService.UpsertOrderFromWebPortal(updatedOrderRequest);
            Assert.Equal(orderResponse.Id, updatedOrder.Id);
            var updateOrderResponse = await _orderHeadersService.GetOrderHeaderById(updatedOrder.Id, false);
            Assert.Equal((int)OrderHeaderStatusTypes.Cancelled, updateOrderResponse.StatusId);
            Assert.Equal(orderResponse.OrderNumber, updatedOrder.OrderNumber);
            Assert.Equal(0, updatedOrder.OrderLineItems.Count());
        }

        #endregion

        #region MoveOrderFromWebPortal

        [Fact]
        public async Task MoveOrderShouldBeCreatedFromWebPortal()
        {
            var orderLineItems = new List<CoreOrderLineItemRequest>()
            {
                GetWebPortalOrderLineItem(1, 1, "s100", "a100", OrderTypes.Pickup),
                GetWebPortalOrderLineItem(1, 1, "s100", "a100", OrderTypes.Delivery)
            };
            var orderResponse = await CreateInitialOrderFromWebPortal(orderLineItems, OrderTypes.Patient_Move);
            Assert.Equal(2, orderResponse.OrderLineItems.Count());
            Assert.NotEmpty(orderResponse.OrderNumber);
            Assert.Equal("Move", orderResponse.OrderType);
        }

        [Fact]
        public async Task MoveOrderShouldBeUpdatedFromWebPortal()
        {
            var orderLineItems = new List<CoreOrderLineItemRequest>()
            {
                GetWebPortalOrderLineItem(1, 1, "s100", "a100", OrderTypes.Pickup),
                GetWebPortalOrderLineItem(1, 1, "s100", "a100", OrderTypes.Delivery),
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Pickup),
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Delivery)
            };
            var orderResponse = await CreateInitialOrderFromWebPortal(orderLineItems, OrderTypes.Patient_Move);
            Assert.Equal(4, orderResponse.OrderLineItems.Count());
            Assert.NotEmpty(orderResponse.OrderNumber);
            Assert.Equal("Move", orderResponse.OrderType);

            var updatedLineItems = new List<CoreOrderLineItemRequest>()
            {
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Pickup),
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Delivery)
            };
            var updatedOrderRequest = GetWebPortalOrderRequest(OrderHeaderStatusTypes.Planned, updatedLineItems, OrderTypes.Patient_Move);
            updatedOrderRequest.Id = orderResponse.Id;
            var updatedOrder = await _orderHeadersService.UpsertOrderFromWebPortal(updatedOrderRequest);
            Assert.Equal(orderResponse.Id, updatedOrder.Id);
            var updateOrderResponse = await _orderHeadersService.GetOrderHeaderById(updatedOrder.Id, false);
            Assert.Equal((int)OrderHeaderStatusTypes.Planned, updateOrderResponse.StatusId);
            Assert.Equal(orderResponse.OrderNumber, updatedOrder.OrderNumber);
            Assert.Equal(2, updatedOrder.OrderLineItems.Count());
        }

        [Fact]
        public async Task MoveOrderShouldUpdateStatusAsCompletedOnCancellingNonFulfillLineItemFromWebPortal()
        {
            var orderLineItems = new List<CoreOrderLineItemRequest>()
            {
                GetWebPortalOrderLineItem(1, 1, "s123", "", OrderTypes.Pickup),
                GetWebPortalOrderLineItem(1, 1, "s123", "", OrderTypes.Delivery),
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Pickup),
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Delivery)
            };
            var orderResponse = await CreateInitialOrderFromWebPortal(orderLineItems, OrderTypes.Patient_Move);
            Assert.Equal(4, orderResponse.OrderLineItems.Count());
            Assert.NotEmpty(orderResponse.OrderNumber);
            Assert.Equal("Move", orderResponse.OrderType);

            var fulfilledItems = orderResponse.OrderLineItems.Where(l => l.ItemId == 2);
            var updatedLineItems = new List<CoreOrderLineItemRequest>();
            foreach (var lineItem in fulfilledItems)
            {
                await AddOrderLineFulfillment(orderResponse.Id, lineItem.Id, lineItem.ItemId, lineItem.ItemCount);
                var updateLineItemRequest = GetWebPortalOrderLineItem(lineItem.ItemId, lineItem.ItemCount, lineItem.SerialNumber, lineItem.AssetTagNumber, (OrderTypes)lineItem.ActionId);
                updateLineItemRequest.Id = lineItem.Id;
                updatedLineItems.Add(updateLineItemRequest);
            }
            var updatedOrderRequest = GetWebPortalOrderRequest(OrderHeaderStatusTypes.Planned, updatedLineItems, OrderTypes.Patient_Move);
            updatedOrderRequest.Id = orderResponse.Id;
            var updatedOrder = await _orderHeadersService.UpsertOrderFromWebPortal(updatedOrderRequest);
            Assert.Equal(orderResponse.Id, updatedOrder.Id);
            var updateOrderResponse = await _orderHeadersService.GetOrderHeaderById(updatedOrder.Id, false);
            Assert.Equal((int)OrderHeaderStatusTypes.Completed, updateOrderResponse.StatusId);
            Assert.Equal(orderResponse.OrderNumber, updatedOrder.OrderNumber);
            Assert.Equal(2, updatedOrder.OrderLineItems.Count());
        }

        [Fact]
        public async Task MoveOrderShouldUpdateStatusAsCancelledOnCancellingAllLineItemFromWebPortal()
        {
            var orderLineItems = new List<CoreOrderLineItemRequest>()
            {
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Pickup),
                GetWebPortalOrderLineItem(2, 1, "s101", "a101", OrderTypes.Delivery)
            };
            var orderResponse = await CreateInitialOrderFromWebPortal(orderLineItems, OrderTypes.Patient_Move);
            Assert.Equal(2, orderResponse.OrderLineItems.Count());
            Assert.NotEmpty(orderResponse.OrderNumber);
            Assert.Equal("Move", orderResponse.OrderType);

            var updatedLineItems = new List<CoreOrderLineItemRequest>();
            var updatedOrderRequest = GetWebPortalOrderRequest(OrderHeaderStatusTypes.Planned, updatedLineItems, OrderTypes.Pickup);
            updatedOrderRequest.Id = orderResponse.Id;
            var updatedOrder = await _orderHeadersService.UpsertOrderFromWebPortal(updatedOrderRequest);
            Assert.Equal(orderResponse.Id, updatedOrder.Id);
            var updateOrderResponse = await _orderHeadersService.GetOrderHeaderById(updatedOrder.Id, false);
            Assert.Equal((int)OrderHeaderStatusTypes.Cancelled, updateOrderResponse.StatusId);
            Assert.Equal(orderResponse.OrderNumber, updatedOrder.OrderNumber);
            Assert.Equal(0, updatedOrder.OrderLineItems.Count());
        }

        #endregion

        private async Task<ViewModels.OrderHeader> CreateInitialOrderFromWebPortal(List<CoreOrderLineItemRequest> orderLineItems, OrderTypes orderTypes)
        {
            var orderRequest = GetWebPortalOrderRequest(OrderHeaderStatusTypes.Planned, orderLineItems, orderTypes);
            var orderCreateResult = await _orderHeadersService.UpsertOrderFromWebPortal(orderRequest);
            var orderResponse = await _orderHeadersService.GetOrderHeaderById(orderCreateResult.Id, false);
            Assert.Equal((int)OrderHeaderStatusTypes.Planned, orderResponse.StatusId);
            Assert.Equal((int)OrderHeaderStatusTypes.Planned, orderResponse.DispatchStatusId);
            return orderResponse;
        }

        private OrderHeaderRequest GetWebPortalOrderRequest(OrderHeaderStatusTypes orderStatus, List<CoreOrderLineItemRequest> orderLineItems, OrderTypes orderTypes)
        {
            var orderRequest = new OrderHeaderRequest()
            {
                ConfirmationNumber = "1234",
                HospiceId = 1234,
                OrderTypeId = (int)orderTypes,
                HospiceMemberId = 1234,
                PatientUuid = Guid.NewGuid(),
                StatOrder = false,
                OrderStatusId = (int)orderStatus,
                OrderLineItems = orderLineItems,
                PickupAddress = _mockViewModels.GetCoreAddress(),
                OrderNotes = _mockViewModels.GetOrderNotesRequests()
            };
            return orderRequest;
        }

        private CoreOrderLineItemRequest GetWebPortalOrderLineItem(int itemId, int count, string serialNumber, string assetTag, OrderTypes action)
        {
            return new CoreOrderLineItemRequest()
            {
                ItemId = itemId,
                Action = action.ToString(),
                ItemCount = count,
                SerialNumber = serialNumber,
                AssetTagNumber = assetTag
            };
        }

        private async Task AddOrderLineFulfillment(int orderId, int lineItemId, int itemId, int quantity)
        {
            var fulfillmentModel = new Models.OrderFulfillmentLineItems()
            {
                ItemId = itemId,
                OrderHeaderId = orderId,
                OrderLineItemId = lineItemId,
                Quantity = quantity
            };

            await _orderFulfillmentLineItemsRepository.AddAsync(fulfillmentModel);
        }
    }
}
