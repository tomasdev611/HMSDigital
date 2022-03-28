using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Models = HMSDigital.Core.Data.Models;

namespace Core.Test.Services
{
    public class DispatchServiceUnitTest
    {
        private readonly IDispatchService _dispatchService;

        private readonly IOrderHeadersService _orderHeadersService;

        private readonly IOrderHeadersRepository _orderHeadersRepository;

        private readonly IOrderFulfillmentLineItemsRepository _orderFulfillmentLineItemsRepository;

        private readonly IDispatchInstructionsRepository _dispatchInstructionsRepository;

        private readonly IItemTransferRequestRepository _itemTransferRequestRepository;

        private readonly IDriverRepository _driverRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly MockViewModels _mockViewModels;

        private readonly MockData _mockData;

        private readonly MockModels _mockModels;

        public DispatchServiceUnitTest()
        {
            _mockViewModels = new MockViewModels();
            var mockService = new MockServices();
            _dispatchService = mockService.GetService<IDispatchService>();
            _orderHeadersService = mockService.GetService<IOrderHeadersService>();
            _orderHeadersRepository = mockService.GetService<IOrderHeadersRepository>();
            _orderFulfillmentLineItemsRepository = mockService.GetService<IOrderFulfillmentLineItemsRepository>();
            _dispatchInstructionsRepository = mockService.GetService<IDispatchInstructionsRepository>();
            _itemTransferRequestRepository = mockService.GetService<IItemTransferRequestRepository>();
            _driverRepository = mockService.GetService<IDriverRepository>();
            _sitesRepository = mockService.GetService<ISitesRepository>();

            _mockModels = mockService.GetService<MockModels>();
            _mockData = new MockData(mockService, _mockModels);
        }

        #region Assign Order

        #region Order Status
        [Fact]
        public async Task AssignOrderShouldChangePartialCompletedLineItemsStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Partial_Fulfillment, OrderHeaderStatusTypes.Planned,
                                                       OrderLineItemStatusTypes.Partial_Fulfillment, OrderLineItemStatusTypes.Planned);

            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(orderModel.Id));
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);
            foreach (var lineItem in updatedOrder.OrderLineItems)
            {
                Assert.Equal((int)OrderLineItemStatusTypes.Scheduled, lineItem.StatusId);
            }
        }

        [Fact]
        public async Task AssignOrderShouldChangePartialCompletedOrderStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Partial_Fulfillment, OrderHeaderStatusTypes.Planned,
                                                       OrderLineItemStatusTypes.Partial_Fulfillment, OrderLineItemStatusTypes.Planned);

            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(orderModel.Id));
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);

            Assert.Equal((int)OrderHeaderStatusTypes.Scheduled, updatedOrder.StatusId);
        }

        [Fact]
        public async Task AssignOrderShouldNotChangeCompletedOrderStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Completed, OrderHeaderStatusTypes.Completed,
                                                       OrderLineItemStatusTypes.Completed, OrderLineItemStatusTypes.Completed);
            await AddDispatchInstruction(orderModel.Id, 100);
            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(orderModel.Id));
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);

            Assert.Equal((int)OrderHeaderStatusTypes.Completed, updatedOrder.StatusId);
        }

        [Fact]
        public async Task AssignOrderShouldNotChangeCompletedOrderLineItemStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Completed, OrderHeaderStatusTypes.Completed,
                                                       OrderLineItemStatusTypes.Completed, OrderLineItemStatusTypes.Completed);
            await AddDispatchInstruction(orderModel.Id, 100);
            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(orderModel.Id));
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);

            foreach (var lineItem in updatedOrder.OrderLineItems)
            {
                Assert.Equal((int)OrderHeaderStatusTypes.Completed, lineItem.StatusId);
            }
        }

        #endregion

        #region Dispatch Status
        [Fact]
        public async Task AssignOrderShouldChangePlannedOrderDispatchStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Planned, OrderHeaderStatusTypes.Planned,
                                                        OrderLineItemStatusTypes.Planned, OrderLineItemStatusTypes.Planned);

            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(orderModel.Id));
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);

            Assert.Equal((int)OrderHeaderStatusTypes.Scheduled, updatedOrder.DispatchStatusId);
        }

        [Fact]
        public async Task AssignOrderShouldChangePlannedOrderLineItemDispatchStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Planned, OrderHeaderStatusTypes.Planned,
                                                        OrderLineItemStatusTypes.Planned, OrderLineItemStatusTypes.Planned);
            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(orderModel.Id));
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);

            foreach (var lineItem in updatedOrder.OrderLineItems)
            {
                Assert.Equal((int)OrderLineItemStatusTypes.Scheduled, lineItem.DispatchStatusId);
            }
        }
        #endregion

        #endregion

        #region Unassign Order
        [Fact]
        public async Task UnassignOrderShouldNotChangeCompletedLineItemsStatusAndDispachedStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Completed, OrderHeaderStatusTypes.Completed,
                                                       OrderLineItemStatusTypes.Completed, OrderLineItemStatusTypes.Completed);

            await AddOrderFulfillmentLineItems(orderModel);

            await _dispatchService.UnassignOrder(orderModel.Id);
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);

            foreach (var lineItem in updatedOrder.OrderLineItems)
            {
                Assert.Equal((int)OrderLineItemStatusTypes.Completed, lineItem.StatusId);
                Assert.Equal((int)OrderLineItemStatusTypes.Completed, lineItem.DispatchStatusId);
            }
        }

        [Fact]
        public async Task UnassignOrderShouldNotChangeCompletedOrderStatusAndDispachedStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Completed, OrderHeaderStatusTypes.Completed,
                                                       OrderLineItemStatusTypes.Completed, OrderLineItemStatusTypes.Completed);
            await AddOrderFulfillmentLineItems(orderModel);
            await _dispatchService.UnassignOrder(orderModel.Id);
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);
            Assert.Equal((int)OrderHeaderStatusTypes.Completed, updatedOrder.StatusId);
            Assert.Equal((int)OrderHeaderStatusTypes.Completed, updatedOrder.DispatchStatusId);
        }

        #region Order Status
        [Fact]
        public async Task UnassignOrderShouldNotChangePartialCompletedLineItemsStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Partial_Fulfillment, OrderHeaderStatusTypes.Scheduled,
                                                       OrderLineItemStatusTypes.Partial_Fulfillment, OrderLineItemStatusTypes.Scheduled, 2);
            await AddOrderFulfillmentLineItems(orderModel, 1);
            await _dispatchService.UnassignOrder(orderModel.Id);
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);
            foreach (var lineItem in updatedOrder.OrderLineItems)
            {
                Assert.Equal((int)OrderLineItemStatusTypes.Partial_Fulfillment, lineItem.StatusId);
            }
        }

        [Fact]
        public async Task UnassignOrderShouldNotChangePartialCompletedOrderStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Partial_Fulfillment, OrderHeaderStatusTypes.Scheduled,
                                                        OrderLineItemStatusTypes.Partial_Fulfillment, OrderLineItemStatusTypes.Scheduled, 2);
            await AddOrderFulfillmentLineItems(orderModel, 1);
            await _dispatchService.UnassignOrder(orderModel.Id);
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);

            Assert.Equal((int)OrderHeaderStatusTypes.Partial_Fulfillment, updatedOrder.StatusId);
        }

        [Fact]
        public async Task UnassignOrderShouldNotChangeCompletedOrderStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Completed, OrderHeaderStatusTypes.Completed,
                                                       OrderLineItemStatusTypes.Completed, OrderLineItemStatusTypes.Completed);
            await AddOrderFulfillmentLineItems(orderModel);
            await _dispatchService.UnassignOrder(orderModel.Id);
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);

            Assert.Equal((int)OrderHeaderStatusTypes.Completed, updatedOrder.StatusId);
        }

        #endregion

        #region Dispatch Status
        [Fact]
        public async Task UnassignOrderShouldChangeScheduledOrderDispatchStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Planned, OrderHeaderStatusTypes.Scheduled,
                                                        OrderLineItemStatusTypes.Planned, OrderLineItemStatusTypes.Scheduled);
            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(orderModel.Id));
            await _dispatchService.UnassignOrder(orderModel.Id);
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);

            Assert.Equal((int)OrderHeaderStatusTypes.Planned, updatedOrder.DispatchStatusId);
        }

        [Fact]
        public async Task UnassignOrderShouldChangeScheduledOrderLineItemDispatchStatus()
        {
            var orderModel = await CreateOrderByStatus(OrderHeaderStatusTypes.Planned, OrderHeaderStatusTypes.Scheduled,
                                                        OrderLineItemStatusTypes.Planned, OrderLineItemStatusTypes.Scheduled);
            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(orderModel.Id));
            await _dispatchService.UnassignOrder(orderModel.Id);
            var updatedOrder = await _orderHeadersService.GetOrderHeaderById(orderModel.Id, false);

            foreach (var lineItem in updatedOrder.OrderLineItems)
            {
                Assert.Equal((int)OrderLineItemStatusTypes.Planned, lineItem.DispatchStatusId);
            }
        }
        #endregion

        #endregion

        #region GetDriverLoadList

        [Fact]
        public async Task GetDriverLoadlistShouldSucceed()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
                await _mockData.CreateInventory(1, serialNumber: "s123"),
                await _mockData.CreateInventory(2, assetTagNumber: "a123"),
                await _mockData.CreateInventory(3, lotNumber: "l123")
            };

            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var vehicleModel = await CreateVehicle();
            var loggedInUserDriver = await CreateDriver(123, 10, vehicleModel.Id);
            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(orderModel.Id, vehicleId: vehicleModel.Id));
            var loadlist = await _dispatchService.GetLoadList(1, new SieveModel());
            var vehicleLoadlist = loadlist.Loadlists.FirstOrDefault(l => l.VehicleId == vehicleModel.Id);
            Assert.NotNull(vehicleLoadlist.Orders.FirstOrDefault(o => o.Id == orderModel.Id));
        }
        #endregion

        #region PickupLoadlist

        [Fact]
        public async Task PickupDriverLoadlistShouldSucceed()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
                await _mockData.CreateInventory(1, serialNumber: "s123"),
                await _mockData.CreateInventory(2, assetTagNumber: "a123"),
                await _mockData.CreateInventory(3, lotNumber: "l123")
            };

            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);

            var vehicleModel = await CreateVehicle();
            var loggedInUserDriver = await CreateDriver(123, 10, vehicleModel.Id);
            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(orderModel.Id, null, vehicleModel.Id));
            var loadlist = await _dispatchService.GetLoadList(1, new SieveModel());
            var vehicleLoadlist = loadlist.Loadlists.FirstOrDefault(l => l.VehicleId == vehicleModel.Id);
            Assert.NotNull(vehicleLoadlist.Orders.FirstOrDefault(o => o.Id == orderModel.Id));

            var dispatchItems = new List<DispatchItem>();
            foreach (var inventory in inventories)
            {
                dispatchItems.Add(new DispatchItem()
                {
                    ItemId = inventory.ItemId,
                    AssetTagNumber = inventory.AssetTagNumber,
                    SerialNumber = inventory.SerialNumber,
                    LotNumber = inventory.LotNumber,
                    Count = 1,
                    OrderLineItemId = orderModel.OrderLineItems.FirstOrDefault(li => li.ItemId == inventory.ItemId).Id
                });
            }
            var pickupDispatchRequest = new DispatchMovementRequest()
            {
                RequestId = 1, //siteId
                RequestType = "loadlist",
                VehicleId = vehicleModel.Id,
                DispatchItems = dispatchItems
            };
            await _dispatchService.PickupDispatchRequest(pickupDispatchRequest);
            var updatedLoadlist = await _dispatchService.GetLoadList(1, new SieveModel());
            Assert.Null(updatedLoadlist.Loadlists);
        }

        #endregion

        #region pickupTransferRequest

        [Fact]
        public async Task PickupTransferRequestShouldSucceed()
        {
            var patientUUID = Guid.NewGuid();

            var transferRequestInventory = await _mockData.CreateInventory(2, assetTagNumber: "aXYZ");
            var transferRequestModel = await CreateTransferRequest(1, 2, transferRequestInventory.ItemId);

            var vehicleModel = await CreateVehicle();
            var loggedInUserDriver = await CreateDriver(123, 10, vehicleModel.Id);
            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(null, transferRequestModel.Id, vehicleModel.Id));
            var loadlist = await _dispatchService.GetLoadList(1, new SieveModel());
            var vehicleLoadlist = loadlist.Loadlists.FirstOrDefault(l => l.VehicleId == vehicleModel.Id);
            Assert.NotNull(vehicleLoadlist.TransferRequests.FirstOrDefault(o => o.Id == transferRequestModel.Id));

            var dispatchItems = new List<DispatchItem>(){
                new DispatchItem()
                {
                    ItemId = transferRequestInventory.ItemId,
                    AssetTagNumber = transferRequestInventory.AssetTagNumber,
                    SerialNumber = transferRequestInventory.SerialNumber,
                    LotNumber = transferRequestInventory.LotNumber,
                    Count = 1
                }
            };
            var pickupDispatchRequest = new DispatchMovementRequest()
            {
                RequestId = transferRequestModel.Id,
                RequestType = "transfer-request",
                VehicleId = vehicleModel.Id,
                DispatchItems = dispatchItems
            };
            await _dispatchService.PickupDispatchRequest(pickupDispatchRequest);
            var updatedLoadlist = await _dispatchService.GetLoadList(1, new SieveModel());
            Assert.Null(updatedLoadlist.Loadlists);
        }

        #endregion

        #region pickupTransferRequest

        [Fact]
        public async Task DropTransferRequestShouldSucceed()
        {
            var patientUUID = Guid.NewGuid();

            var transferRequestInventory = await _mockData.CreateInventory(2, assetTagNumber: "aXYZ");
            var transferRequestModel = await CreateTransferRequest(1, 2, transferRequestInventory.ItemId);

            var vehicleModel = await CreateVehicle();
            var loggedInUserDriver = await CreateDriver(123, 10, vehicleModel.Id);
            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(null, transferRequestModel.Id, vehicleModel.Id));
            var loadlist = await _dispatchService.GetLoadList(1, new SieveModel());
            var vehicleLoadlist = loadlist.Loadlists.FirstOrDefault(l => l.VehicleId == vehicleModel.Id);
            Assert.NotNull(vehicleLoadlist.TransferRequests.FirstOrDefault(o => o.Id == transferRequestModel.Id));

            var dispatchItems = new List<DispatchItem>(){
                new DispatchItem()
                {
                    ItemId = transferRequestInventory.ItemId,
                    AssetTagNumber = transferRequestInventory.AssetTagNumber,
                    SerialNumber = transferRequestInventory.SerialNumber,
                    LotNumber = transferRequestInventory.LotNumber,
                    Count = 1
                }
            };
            var pickupDispatchRequest = new DispatchMovementRequest()
            {
                RequestId = transferRequestModel.Id,
                RequestType = "transfer-request",
                VehicleId = vehicleModel.Id,
                DispatchItems = dispatchItems
            };
            await _dispatchService.PickupDispatchRequest(pickupDispatchRequest);
            var updatedLoadlist = await _dispatchService.GetLoadList(1, new SieveModel());
            Assert.Null(updatedLoadlist.Loadlists);

            await _dispatchService.DropDispatchRequest(pickupDispatchRequest);
        }

        #endregion

        #region UpdateDispatchRecords

        [Fact]
        public async Task UpdateDispatchRecordShouldSucceed()
        {
            var existingDispatchRecord = _mockModels.DispatchRecords.FirstOrDefault();
            var deliveryDate = DateTime.UtcNow;
            var pickupRequestDate = deliveryDate.AddDays(1);
            var pickupDate = pickupRequestDate.AddDays(1);
            var dispatchRecordUpdateRequests = new List<DispatchRecordUpdateRequest>()
            {
                new DispatchRecordUpdateRequest()
                {
                    DispatchRecordId=existingDispatchRecord.NSDispatchId.Value,
                    HmsDeliveryDate=deliveryDate,
                    HmsPickupRequestDate = pickupRequestDate,
                    PickupDate = pickupDate
                }
            };
            await _dispatchService.UpdateDispatchRecords(dispatchRecordUpdateRequests);
            var updatedDispatchRecord = _mockModels.DispatchRecords.FirstOrDefault(d => d.NSDispatchId == existingDispatchRecord.NSDispatchId);
            Assert.Equal(deliveryDate, updatedDispatchRecord.HmsDeliveryDate);
            Assert.Equal(pickupRequestDate, updatedDispatchRecord.HmsPickupRequestDate);
            Assert.Equal(pickupDate, updatedDispatchRecord.PickupDate);
        }

        #endregion

        #region GetLoggedInDriverDispatch
        [Fact]
        public async Task GetLoggedInDriverDispatchShouldSucceed()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
                await _mockData.CreateInventory(1, serialNumber: "s123"),
                await _mockData.CreateInventory(2, assetTagNumber: "a123"),
                await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var vehicleModel = await CreateVehicle();
            var loggedInUserDriver = await CreateDriver(123, 10, vehicleModel.Id);
            await _dispatchService.CreateDispatchInstruction(_mockViewModels.GetDispatchAssignmentRequest(orderModel.Id, null, vehicleModel.Id));

            var driverDispatch = await _dispatchService.GetLoggedInDriverDispatch(new SieveModel());
            Assert.Equal(vehicleModel.Id, driverDispatch.VehicleId);
            Assert.Equal(loggedInUserDriver.Id, driverDispatch.DriverId);
            Assert.Equal(orderModel.Id, driverDispatch.OrderHeaders.FirstOrDefault().Id);
        }

        #endregion



        private async Task<Models.OrderHeaders> CreateOrderByStatus(OrderHeaderStatusTypes orderStatus, OrderHeaderStatusTypes orderDispatchStatus,
                                                                OrderLineItemStatusTypes lineItemStatus, OrderLineItemStatusTypes lineItemDispatchStatus,
                                                                int itemCount = 1)
        {
            var orderModel = new Models.OrderHeaders()
            {
                OrderTypeId = (int)OrderTypes.Delivery,
                OrderType = new Models.OrderTypes()
                {
                    Id = (int)OrderTypes.Delivery,
                    Name = OrderTypes.Delivery.ToString()
                },
                SiteId = 1,
                PatientUuid = Guid.NewGuid(),
                RequestedStartDateTime = DateTime.Now,
                RequestedEndDateTime = DateTime.Now.AddHours(2),
                StatusId = (int)orderStatus,
                Status = new Models.OrderHeaderStatusTypes()
                {
                    Id = (int)orderStatus,
                    Name = orderStatus.ToString()
                },
                DispatchStatusId = (int)orderDispatchStatus,
                DispatchStatus = new Models.OrderHeaderStatusTypes()
                {
                    Id = (int)orderDispatchStatus,
                    Name = orderDispatchStatus.ToString()
                },
                OrderLineItems = new List<Models.OrderLineItems>()
                {
                       new Models.OrderLineItems()
                       {
                            ItemId = 2,
                            ItemCount = itemCount,
                            ActionId = (int)OrderTypes.Delivery,
                            Action = new Models.OrderTypes()
                            {
                                Name = OrderTypes.Delivery.ToString()
                            },
                            StatusId = (int)lineItemStatus,
                            Status = new Models.OrderLineItemStatusTypes()
                            {
                                Id = (int)lineItemStatus,
                                Name = lineItemStatus.ToString()
                            },
                            DispatchStatusId = (int)lineItemDispatchStatus,
                            DispatchStatus = new Models.OrderLineItemStatusTypes()
                            {
                                Id = (int)lineItemDispatchStatus,
                                Name = lineItemDispatchStatus.ToString()
                            }
                       }
                }
            };
            await _orderHeadersRepository.AddAsync(orderModel);
            return orderModel;
        }

        private async Task AddOrderFulfillmentLineItems(Models.OrderHeaders orderHeaders, int? itemCount = null)
        {
            foreach (var orderLineItem in orderHeaders.OrderLineItems)
            {
                var fulfillment = new Models.OrderFulfillmentLineItems()
                {
                    OrderHeaderId = orderHeaders.Id,
                    OrderLineItemId = orderLineItem.Id,
                    ItemId = orderLineItem.ItemId.Value,
                    Quantity = itemCount != null ? itemCount : orderLineItem.ItemCount
                };

                await _orderFulfillmentLineItemsRepository.AddAsync(fulfillment);
            }
        }

        private async Task<Models.Drivers> CreateDriver(int driverId, int userId, int vehicleId)
        {
            var driverModel = new Models.Drivers()
            {
                Id = driverId,
                UserId = userId,
                CurrentVehicleId = vehicleId,
                CurrentSiteId = 1,
                CurrentSite = _mockModels.Sites.FirstOrDefault(s => s.Id == 1)
            };
            await _driverRepository.AddAsync(driverModel);
            return driverModel;
        }

        private async Task<Models.ItemTransferRequests> CreateTransferRequest(int sourceLocationId, int destinationLocationId, int itemId)
        {
            var itemTransferRequest = new Models.ItemTransferRequests()
            {
                SourceLocationId = sourceLocationId,
                DestinationLocationId = destinationLocationId,
                ItemId = itemId,
                ItemCount = 1,
                StatusId = (int)TransferRequestStatusTypes.Created
            };
            await _itemTransferRequestRepository.AddAsync(itemTransferRequest);
            return itemTransferRequest;
        }

        private async Task<Models.Sites> CreateVehicle()
        {
            var parentSite = await _sitesRepository.GetByIdAsync(1);
            var vehicleModel = new Models.Sites()
            {
                Id = new Random().Next(1000),
                Cvn = "testCvn",
                LocationType = "vehicle",
                Name = "testName",
                ParentNetSuiteLocationId = parentSite.NetSuiteLocationId
            };
            await _sitesRepository.AddAsync(vehicleModel);
            return vehicleModel;
        }

        private async Task AddDispatchInstruction(int orderId, int vehicleId)
        {
            var dispatchInstruction = new Models.DispatchInstructions()
            {
                OrderHeaderId = orderId,
                VehicleId = vehicleId,
                CreatedByUserId = 1,
                CreatedDateTime = DateTime.UtcNow
            };

            await _dispatchInstructionsRepository.AddAsync(dispatchInstruction);
        }
    }
}
