using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Models = HMSDigital.Core.Data.Models;
using Enums = HMSDigital.Core.Data.Enums;
using System.ComponentModel.DataAnnotations;
using NSSDKViewModels = HospiceSource.Digital.NetSuite.SDK.ViewModels;

namespace Core.Test.Services
{
    public class FulfillmentServiceUnitTest
    {
        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        private readonly MockModels _mockModels;

        private readonly MockData _mockData;

        private readonly IFulfillmentService _fulfillmentService;

        private readonly IDriversService _driversService;

        private readonly IInventoryRepository _inventoryRepository;

        private readonly IPatientInventoryRepository _patientInventoryRepository;

        private readonly IOrderHeadersRepository _orderHeadersRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly int _patientSiteId;

        public FulfillmentServiceUnitTest()
        {
            _mockViewModels = new MockViewModels();
            var mockService = new MockServices();
            _fulfillmentService = mockService.GetService<IFulfillmentService>();
            _driversService = mockService.GetService<IDriversService>();
            _inventoryRepository = mockService.GetService<IInventoryRepository>();
            _orderHeadersRepository = mockService.GetService<IOrderHeadersRepository>();
            _patientInventoryRepository = mockService.GetService<IPatientInventoryRepository>();
            _sitesRepository = mockService.GetService<ISitesRepository>();
            _mockModels = mockService.GetService<MockModels>();
            _sieveModel = new SieveModel();
            _mockData = new MockData(mockService, _mockModels);
            _patientSiteId = _mockData.GetPatientSite().Result.Id;
        }


        #region delivery orders

        [Fact]
        public async Task DeliveryOrderShouldBeFulfilled()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, inventories);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            await ValidateCompleteFulfilledOrder(orderModel.Id, inventories, _patientSiteId, InventoryStatusTypes.NotReady);
            var inventoryIds = inventories.Select(i => i.Id);
            var patientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);
            Assert.Empty(inventoryIds.Except(patientInventories.Select(pi => pi.InventoryId.Value)));

            ValidateNetSuiteRequestForOrderFulfillment(patientUUID, inventories);
        }

        [Fact]
        public async Task DeliveryOrderForSameItemShouldBeFulfilled()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(4, serialNumber: "s123"),
               await _mockData.CreateInventory(4, assetTagNumber: "a123")
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, inventories);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            await ValidateCompleteFulfilledOrder(orderModel.Id, inventories, _patientSiteId, InventoryStatusTypes.NotReady);
            var inventoryIds = inventories.Select(i => i.Id);
            var patientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);
            Assert.Empty(inventoryIds.Except(patientInventories.Select(pi => pi.InventoryId.Value)));

            ValidateNetSuiteRequestForOrderFulfillment(patientUUID, inventories);
        }

        [Fact]
        public async Task PartialDeliveryOrderShouldBeFulfilled()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var fulfilmentInventories = inventories.Where(i => i.SerialNumber == "s123" || i.AssetTagNumber == "a123");
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, fulfilmentInventories);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            await ValidatePartiallyFulfilledOrder(orderModel.Id, fulfilmentInventories, _patientSiteId, InventoryStatusTypes.NotReady);
            var inventoryIds = fulfilmentInventories.Select(i => i.Id);
            var patientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);
            Assert.Empty(inventoryIds.Except(patientInventories.Select(pi => pi.InventoryId.Value)));

            ValidateNetSuiteRequestForOrderFulfillment(patientUUID, fulfilmentInventories.ToList());
        }

        [Fact]
        public async Task PartialDeliveryOrderForSameItemShouldBeFulfilled()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(4, serialNumber: "s123"),
               await _mockData.CreateInventory(4, assetTagNumber: "a123"),
               await _mockData.CreateInventory(4, lotNumber: "l123")
            };
            var fulfilmentInventories = inventories.Where(i => i.SerialNumber == "s123" || i.AssetTagNumber == "a123");
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, fulfilmentInventories);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            await ValidatePartiallyFulfilledOrder(orderModel.Id, fulfilmentInventories, _patientSiteId, InventoryStatusTypes.NotReady);

            var inventoryIds = fulfilmentInventories.Select(i => i.Id);
            var patientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);
            Assert.Empty(inventoryIds.Except(patientInventories.Select(pi => pi.InventoryId.Value)));

            ValidateNetSuiteRequestForOrderFulfillment(patientUUID, fulfilmentInventories.ToList());
        }

        [Fact]
        public async Task DeliveryOrderShouldBeFulfilledForStandaloneInventory()
        {
            var patientUUID = Guid.NewGuid();
            int inventoryCount = 10;
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(5, count:inventoryCount)
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, inventories);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            var updatedOrderModel = await _orderHeadersRepository.GetByIdAsync(orderModel.Id);

            Assert.Equal((int)OrderHeaderStatusTypes.Completed, updatedOrderModel.StatusId);
            foreach (var lineItem in updatedOrderModel.OrderLineItems)
            {
                Assert.Equal((int)OrderLineItemStatusTypes.Completed, lineItem.StatusId);
            }
            var fulfiledCount = updatedOrderModel.OrderLineItems.Sum(o => o.ItemCount);
            foreach (var inventory in inventories)
            {
                Assert.Equal((inventoryCount - fulfiledCount), inventory.QuantityAvailable);
            }

            ValidateNetSuiteRequestForOrderFulfillment(patientUUID, inventories);
        }

        [Fact]
        public async Task DeliveryOrderShouldBeFulfilledWithLoggedInDriver()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, null, null, inventories);
            _mockModels.Drivers.Add(new Models.Drivers()
            {
                Id = 2,
                CurrentVehicleId = 100,
                CurrentVehicle = new Models.Sites()
                {
                    Id = 100
                },
                User = _mockModels.GetUser(10),
                UserId = 10
            });

            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            await ValidateCompleteFulfilledOrder(orderModel.Id, inventories, _patientSiteId, InventoryStatusTypes.NotReady);
            var inventoryIds = inventories.Select(i => i.Id);
            var patientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);
            Assert.Empty(inventoryIds.Except(patientInventories.Select(pi => pi.InventoryId.Value)));

            ValidateNetSuiteRequestForOrderFulfillment(patientUUID, inventories);
        }

        #endregion

        #region pickup orders

        [Fact]
        public async Task PickupOrderShouldBeFulfilled()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123Pickup", statusId:2),
               await _mockData.CreateInventory(2, assetTagNumber: "a123Pickup", statusId:2),
               await _mockData.CreateInventory(3, lotNumber: "l123Pickup",siteId:_patientSiteId, statusId:2)
            };
            var patientInventories = await  _mockData.AddPatientInventory(patientUUID, inventories);

            var orderModel = await _mockData.CreateOrder(OrderTypes.Pickup, patientUUID, inventories, null);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, inventories, null);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            await ValidateCompleteFulfilledOrder(orderModel.Id, inventories, driver.CurrentVehicleId, InventoryStatusTypes.NotReady);
            var inventoryIds = inventories.Select(i => i.Id);
            var updatedPatientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);
            Assert.Empty(updatedPatientInventories.Where(pi => inventoryIds.Contains(pi.InventoryId.Value)));

            ValidateNetSuiteRequestForOrderFulfillment(patientUUID, inventories);
        }

        [Fact]
        public async Task PartialPickupOrderShouldBeFulfilled()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123Pickup", statusId:2),
               await _mockData.CreateInventory(2, assetTagNumber: "a123Pickup", statusId:2),
               await _mockData.CreateInventory(3, lotNumber: "l123Pickup",siteId:_patientSiteId, statusId:2)
            };
            var patientInventories = await  _mockData.AddPatientInventory(patientUUID, inventories);
            var fulfilmentLineItems = inventories.Where(i => i.SerialNumber == "s123Pickup" || i.AssetTagNumber == "a123Pickup");
            var orderModel = await _mockData.CreateOrder(OrderTypes.Pickup, patientUUID, inventories, null);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, fulfilmentLineItems, null);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            await ValidatePartiallyFulfilledOrder(orderModel.Id, fulfilmentLineItems, driver.CurrentVehicleId, InventoryStatusTypes.NotReady, OrderTypes.Pickup);
            var inventoryIds = fulfilmentLineItems.Select(i => i.Id);
            var updatedPatientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);
            Assert.Empty(updatedPatientInventories.Where(pi => inventoryIds.Contains(pi.InventoryId.Value)));

            ValidateNetSuiteRequestForOrderFulfillment(patientUUID, fulfilmentLineItems.ToList());
        }

        [Fact]
        public async Task PickOrderShouldBeFulfilledForStandaloneInventory()
        {
            var patientUUID = Guid.NewGuid();
            int inventoryCount = 1;
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(5, count:inventoryCount, siteId:_patientSiteId)
            };
            var patientInventories = await  _mockData.AddPatientInventory(patientUUID, inventories);
            var orderModel = await _mockData.CreateOrder(OrderTypes.Pickup, patientUUID, inventories, null);
            var driver = await _driversService.GetDriverById(1);
            var vehicleInventoryCount = await GetVehicleInventoryCount(5, driver.CurrentVehicleId.Value);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, inventories, null);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            var updatedOrderModel = await _orderHeadersRepository.GetByIdAsync(orderModel.Id);
            Assert.Equal((int)OrderHeaderStatusTypes.Completed, updatedOrderModel.StatusId);
            foreach (var lineItem in updatedOrderModel.OrderLineItems)
            {
                Assert.Equal((int)OrderLineItemStatusTypes.Completed, lineItem.StatusId);
            }

            var fulfiledCount = updatedOrderModel.OrderLineItems.Sum(o => o.ItemCount);
            foreach (var inventory in inventories)
            {
                Assert.Equal(inventory.QuantityAvailable, (inventoryCount - fulfiledCount));
            }
            var pickedupVehicleInventoryCount = await GetVehicleInventoryCount(5, driver.CurrentVehicleId.Value);
            Assert.Equal(pickedupVehicleInventoryCount, (vehicleInventoryCount + fulfiledCount));

            ValidateNetSuiteRequestForOrderFulfillment(patientUUID, inventories);
        }

        [Fact]
        public async Task ExceptionPickupOrderShouldBeFulfilledWithoutFulfillmentItems()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123Pickup", statusId:2),
               await _mockData.CreateInventory(2, assetTagNumber: "a123Pickup", statusId:2),
               await _mockData.CreateInventory(3, lotNumber: "l123Pickup",siteId:_patientSiteId, statusId:2)
            };
            var patientInventories = await  _mockData.AddPatientInventory(patientUUID, inventories);

            var orderModel = await _mockData.CreateOrder(OrderTypes.Pickup, patientUUID, inventories, null);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, null);
            orderFulfillmentRequest.IsExceptionFulfillment = true;
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            var updatedOrderModel = await _orderHeadersRepository.GetByIdAsync(orderModel.Id);

            Assert.Equal((int)OrderHeaderStatusTypes.Planned, updatedOrderModel.StatusId);
            Assert.True(updatedOrderModel.IsExceptionFulfillment);
            foreach (var lineItem in updatedOrderModel.OrderLineItems)
            {
                Assert.Equal((int)OrderLineItemStatusTypes.Planned, lineItem.StatusId);
            }
        }

        #endregion

        #region exchange orders

        [Fact]
        public async Task ExchangeOrderShouldBeFulfilled()
        {
            var patientUUID = Guid.NewGuid();
            var pickupInventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123Pickup", statusId:2),
               await _mockData.CreateInventory(2, assetTagNumber: "a123Pickup", statusId:2),
               await _mockData.CreateInventory(3, lotNumber: "l123Pickup", siteId:_patientSiteId, statusId:2)
            };
            var dropInventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123Delivery"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123Delivery"),
               await _mockData.CreateInventory(3, lotNumber: "l123Delivery")
            };
            var patientInventories = await  _mockData.AddPatientInventory(patientUUID, pickupInventories);
            var orderModel = await _mockData.CreateOrder(OrderTypes.Exchange, patientUUID, pickupInventories, dropInventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, pickupInventories, dropInventories);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            await ValidateCompleteFulfilledOrder(orderModel.Id, pickupInventories, driver.CurrentVehicleId, InventoryStatusTypes.NotReady);
            await ValidateCompleteFulfilledOrder(orderModel.Id, dropInventories, _patientSiteId, InventoryStatusTypes.NotReady);

            var pickupInventoryIds = pickupInventories.Select(i => i.Id);
            var dropInventoryIds = dropInventories.Select(i => i.Id);
            var updatedPatientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);

            Assert.Empty(updatedPatientInventories.Where(pi => pickupInventoryIds.Contains(pi.InventoryId.Value)));
            Assert.Empty(dropInventoryIds.Except(updatedPatientInventories.Select(pi => pi.InventoryId.Value)));
        }

        [Fact]
        public async Task PartialExchangeOrderShouldBeFulfilled()
        {
            var patientUUID = Guid.NewGuid();
            var pickupInventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123Pickup", statusId:2),
               await _mockData.CreateInventory(2, assetTagNumber: "a123Pickup", statusId:2),
               await _mockData.CreateInventory(3, lotNumber: "l123Pickup",siteId:_patientSiteId, statusId:2)
            };
            var dropInventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123Delivery"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123Delivery"),
               await _mockData.CreateInventory(3, lotNumber: "l123Delivery")
            };
            var pickupFulfilmentLineItems = pickupInventories.Where(i => i.SerialNumber == "s123Pickup" || i.AssetTagNumber == "a123Pickup");
            var dropFulfilmentLineItems = dropInventories.Where(i => i.SerialNumber == "s123Delivery" || i.AssetTagNumber == "a123Delivery");
            var patientInventories = await  _mockData.AddPatientInventory(patientUUID, pickupInventories);
            var orderModel = await _mockData.CreateOrder(OrderTypes.Exchange, patientUUID, pickupInventories, dropInventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, pickupFulfilmentLineItems, dropFulfilmentLineItems);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            await ValidatePartiallyFulfilledOrder(orderModel.Id, pickupFulfilmentLineItems, driver.CurrentVehicleId, InventoryStatusTypes.NotReady, OrderTypes.Pickup);
            await ValidatePartiallyFulfilledOrder(orderModel.Id, dropFulfilmentLineItems, _patientSiteId, InventoryStatusTypes.NotReady);

            var pickupInventoryIds = pickupFulfilmentLineItems.Select(i => i.Id);
            var dropInventoryIds = dropFulfilmentLineItems.Select(i => i.Id);
            var updatedPatientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);

            Assert.Empty(updatedPatientInventories.Where(pi => pickupInventoryIds.Contains(pi.InventoryId.Value)));
            Assert.Empty(dropInventoryIds.Except(updatedPatientInventories.Select(pi => pi.InventoryId.Value)));
        }

        #endregion

        #region respite orders

        [Fact]
        public async Task RespiteOrderShouldBeFulfilled()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Respite, patientUUID, null, inventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, inventories);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            await ValidateCompleteFulfilledOrder(orderModel.Id, inventories, _patientSiteId, InventoryStatusTypes.NotReady);
            var inventoryIds = inventories.Select(i => i.Id);
            var patientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);
            Assert.Empty(inventoryIds.Except(patientInventories.Select(pi => pi.InventoryId.Value)));
        }

        [Fact]
        public async Task PartialRespiteOrderShouldBeFulfilled()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var fulfilmentLineItems = inventories.Where(i => i.SerialNumber == "s123" || i.AssetTagNumber == "a123");
            var orderModel = await _mockData.CreateOrder(OrderTypes.Respite, patientUUID, null, inventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, fulfilmentLineItems);
            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);

            await ValidatePartiallyFulfilledOrder(orderModel.Id, fulfilmentLineItems, _patientSiteId, InventoryStatusTypes.NotReady);
            var inventoryIds = fulfilmentLineItems.Select(i => i.Id);
            var patientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);
            Assert.Empty(inventoryIds.Except(patientInventories.Select(pi => pi.InventoryId.Value)));
        }

        #endregion

        #region move orders

        [Fact]
        public async Task MoveOrderShouldBeFulfilled()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Patient_Move, patientUUID, inventories, inventories);
            var driver = await _driversService.GetDriverById(1);

            var deliveryOrderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, inventories);
            await _fulfillmentService.FulfillOrder(deliveryOrderFulfillmentRequest);

            await ValidateInventory(inventories, _patientSiteId, InventoryStatusTypes.NotReady);
            var inventoryIds = inventories.Select(i => i.Id);
            var patientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);
            Assert.Empty(inventoryIds.Except(patientInventories.Select(pi => pi.InventoryId.Value)));

            var updatedOrderModel = await _orderHeadersRepository.GetByIdAsync(orderModel.Id);

            Assert.Equal((int)OrderHeaderStatusTypes.Partial_Fulfillment, updatedOrderModel.StatusId);
            foreach (var lineItem in updatedOrderModel.OrderLineItems)
            {
                if (lineItem.ActionId == (int)OrderTypes.Delivery)
                {
                    Assert.Equal((int)OrderLineItemStatusTypes.Completed, lineItem.StatusId);
                }
            }

            // pick up same item which deliver in the order
            var pickupOrderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, inventories, null);
            await _fulfillmentService.FulfillOrder(pickupOrderFulfillmentRequest);

            await ValidateInventory(inventories, driver.CurrentVehicleId, InventoryStatusTypes.Available);
            var pickupInventoryIds = inventories.Select(i => i.Id);
            var updatedPatientInventories = await _patientInventoryRepository.GetManyAsync(pi => patientUUID == pi.PatientUuid);
            Assert.Empty(updatedPatientInventories.Where(pi => inventoryIds.Contains(pi.InventoryId.Value)));

            updatedOrderModel = await _orderHeadersRepository.GetByIdAsync(orderModel.Id);
            Assert.Equal((int)OrderHeaderStatusTypes.Completed, updatedOrderModel.StatusId);
            foreach (var lineItem in updatedOrderModel.OrderLineItems)
            {
                if (lineItem.ActionId == (int)OrderTypes.Pickup)
                {
                    Assert.Equal((int)OrderLineItemStatusTypes.Completed, lineItem.StatusId);
                }
            }

            ValidateNetSuiteRequestForOrderFulfillment(patientUUID, inventories);
        }

        #endregion

        #region negative tests

        [Fact]
        public async Task DeliveryOrderFulfillmentShouldFailForSwappedLineItems()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, inventories);

            var fulfillmentItem1 = orderFulfillmentRequest.FulfillmentItems.FirstOrDefault();
            var fulfillmentItem2 = orderFulfillmentRequest.FulfillmentItems.Skip(1).FirstOrDefault();
            var lineItemId1 = fulfillmentItem1.OrderLineItemId;
            var lineItemId2 = fulfillmentItem2.OrderLineItemId;
            fulfillmentItem1.OrderLineItemId = lineItemId2;
            fulfillmentItem2.OrderLineItemId = lineItemId1;

            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.FulfillOrder(orderFulfillmentRequest));
        }

        [Fact]
        public async Task PickupOrderFulfillmentForSameItemShouldFailForSwappedLineItems()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(4, serialNumber: "s123"),
               await _mockData.CreateInventory(4, assetTagNumber: "a123"),
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Pickup, patientUUID, null, inventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, inventories);


            var fulfillmentItem1 = orderFulfillmentRequest.FulfillmentItems.FirstOrDefault();
            var fulfillmentItem2 = orderFulfillmentRequest.FulfillmentItems.Skip(1).FirstOrDefault();
            var lineItemId1 = fulfillmentItem1.OrderLineItemId;
            var lineItemId2 = fulfillmentItem2.OrderLineItemId;
            fulfillmentItem1.OrderLineItemId = lineItemId2;
            fulfillmentItem2.OrderLineItemId = lineItemId1;

            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.FulfillOrder(orderFulfillmentRequest));

        }

        [Fact]
        public async Task DeliveryOrderShouldFailForCompletedOrder()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };

            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, inventories);

            await _fulfillmentService.FulfillOrder(orderFulfillmentRequest);
            // fulfill order after status completed
            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.FulfillOrder(orderFulfillmentRequest));
        }

        [Fact]
        public async Task DeliveryOrderShouldFailWithoutOrder()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(new Models.OrderHeaders(), driver, null, inventories);

            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.FulfillOrder(orderFulfillmentRequest));
        }

        [Fact]
        public async Task DeliveryOrderShouldFailWithoutDriver()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };

            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, null, null, inventories);

            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.FulfillOrder(orderFulfillmentRequest));
        }

        [Fact]
        public async Task DeliveryOrderShouldFailWithoutAssignedSite()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var driver = await _driversService.GetDriverById(1);
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories, assignSite: false);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, inventories);

            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.FulfillOrder(orderFulfillmentRequest));
        }

        [Fact]
        public async Task DeliveryOrderShouldFailWithoutFulfillmentItems()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var driver = await _driversService.GetDriverById(1);
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);

            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, null);

            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.FulfillOrder(orderFulfillmentRequest));
        }

        [Fact]
        public async Task DeliveryOrderShouldFailForStandaloneInventoryNotAvailableOnSite()
        {
            var patientUUID = Guid.NewGuid();
            int inventoryCount = 10;
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(5, count:inventoryCount, siteId:2)
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var driver = await _driversService.GetDriverById(1);
            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, inventories);
            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.FulfillOrder(orderFulfillmentRequest));
        }


        [Fact]
        public async Task DeliveryOrderShouldForInventoryNotValid()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };

            var driver = await _driversService.GetDriverById(1);
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);

            var orderFulfillmentRequest = GetFulfilmentRequest(orderModel, driver, null, inventories);

            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.FulfillOrder(orderFulfillmentRequest));
        }

        #endregion

        #region updateOrderStatus method

        [Theory]
        [InlineData((int)OrderHeaderStatusTypes.Scheduled, (int)OrderHeaderStatusTypes.OutForFulfillment)]
        [InlineData((int)OrderHeaderStatusTypes.Scheduled, (int)OrderHeaderStatusTypes.Enroute)]
        [InlineData((int)OrderHeaderStatusTypes.Scheduled, (int)OrderHeaderStatusTypes.OnSite)]
        [InlineData((int)OrderHeaderStatusTypes.OnTruck, (int)OrderHeaderStatusTypes.OutForFulfillment)]
        [InlineData((int)OrderHeaderStatusTypes.OutForFulfillment, (int)OrderHeaderStatusTypes.Enroute)]
        [InlineData((int)OrderHeaderStatusTypes.Enroute, (int)OrderHeaderStatusTypes.OnSite)]
        public async Task OrderStatusShouldBeUpdated(int initialStatusId, int updatedStatusId)
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories, statusId: initialStatusId);
            var updateOrderStatusRequest = new OrderStatusUpdateRequest()
            {
                StatusId = updatedStatusId,
                DispatchStatusId = updatedStatusId,
                OrderIds = new List<int>() { orderModel.Id }
            };
            await _fulfillmentService.UpdateOrderStatus(updateOrderStatusRequest);
            var updatedOrderModel = await _orderHeadersRepository.GetByIdAsync(orderModel.Id);
            Assert.Equal(updatedStatusId, updatedOrderModel.StatusId);
            Assert.Equal(updatedStatusId, updatedOrderModel.DispatchStatusId);
        }

        [Theory]
        [InlineData((int)OrderHeaderStatusTypes.Scheduled, (int)OrderHeaderStatusTypes.Completed)]
        [InlineData((int)OrderHeaderStatusTypes.Scheduled, (int)OrderHeaderStatusTypes.Partial_Fulfillment)]
        [InlineData((int)OrderHeaderStatusTypes.Scheduled, (int)OrderHeaderStatusTypes.Pending_Approval)]
        [InlineData((int)OrderHeaderStatusTypes.Scheduled, (int)OrderHeaderStatusTypes.Planned)]
        [InlineData((int)OrderHeaderStatusTypes.OnTruck, (int)OrderHeaderStatusTypes.Enroute)]
        [InlineData((int)OrderHeaderStatusTypes.OnTruck, (int)OrderHeaderStatusTypes.OnSite)]
        [InlineData((int)OrderHeaderStatusTypes.Partial_Fulfillment, (int)OrderHeaderStatusTypes.OutForFulfillment)]
        [InlineData((int)OrderHeaderStatusTypes.Partial_Fulfillment, (int)OrderHeaderStatusTypes.OnSite)]
        [InlineData((int)OrderHeaderStatusTypes.Enroute, (int)OrderHeaderStatusTypes.OutForFulfillment)]
        public async Task OrderStatusUpdateShouldFailForInvalidStatusConversion(int initialStatusId, int updatedStatusId)
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories, statusId: initialStatusId);
            var updateOrderStatusRequest = new OrderStatusUpdateRequest()
            {
                StatusId = updatedStatusId,
                DispatchStatusId = updatedStatusId,
                OrderIds = new List<int>() { orderModel.Id }
            };
            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.UpdateOrderStatus(updateOrderStatusRequest));
        }

        [Fact]
        public async Task OrderStatusUpdateShouldFailForPlannedOrders()
        {
            var patientUUID = Guid.NewGuid();
            var inventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123"),
               await _mockData.CreateInventory(2, assetTagNumber: "a123"),
               await _mockData.CreateInventory(3, lotNumber: "l123")
            };
            var orderModel = await _mockData.CreateOrder(OrderTypes.Delivery, patientUUID, null, inventories);
            var updateOrderStatusRequest = new OrderStatusUpdateRequest()
            {
                StatusId = (int)OrderHeaderStatusTypes.Scheduled,
                DispatchStatusId = (int)OrderHeaderStatusTypes.Scheduled,
                OrderIds = new List<int>() { orderModel.Id }
            };
            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.UpdateOrderStatus(updateOrderStatusRequest));
        }

        [Fact]
        public async Task OrderStatusUpdateShouldFailWithoutOrderIds()
        {
            var updateOrderStatusRequest = new OrderStatusUpdateRequest()
            {
                StatusId = (int)OrderHeaderStatusTypes.Completed,
                DispatchStatusId = (int)OrderHeaderStatusTypes.Completed
            };
            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.UpdateOrderStatus(updateOrderStatusRequest));
        }

        [Fact]
        public async Task OrderStatusUpdateShouldFailWithoutStatusId()
        {
            var updateOrderStatusRequest = new OrderStatusUpdateRequest();
            await Assert.ThrowsAsync<ValidationException>(() => _fulfillmentService.UpdateOrderStatus(updateOrderStatusRequest));
        }

        #endregion

        #region private methods

        private OrderFulfillmentRequest GetFulfilmentRequest(Models.OrderHeaders order, Driver driver, IEnumerable<Models.Inventory> pickupInventories, IEnumerable<Models.Inventory> dropInventories)
        {

            var fulfillmentItems = GetFulfillmentItems(order, pickupInventories, OrderTypes.Pickup);
            fulfillmentItems.AddRange(GetFulfillmentItems(order, dropInventories, OrderTypes.Delivery));

            return new OrderFulfillmentRequest()
            {
                OrderId = order.Id,
                DriverId = driver?.Id,
                VehicleId = driver?.CurrentVehicleId,
                FulfillmentItems = fulfillmentItems
            };
        }

        private List<FulfillmentItem> GetFulfillmentItems(Models.OrderHeaders order, IEnumerable<Models.Inventory> inventories, OrderTypes orderType)
        {
            var fulfillmentItems = new List<FulfillmentItem>();
            if (inventories == null)
            {
                return fulfillmentItems;
            }

            foreach (var inventory in inventories)
            {
                var lineItemsForItem = order.OrderLineItems.Where(l => l.ItemId == inventory.ItemId && l.ActionId == (int)orderType);
                var lineItem = lineItemsForItem.FirstOrDefault(l => !fulfillmentItems.Select(fl => fl.OrderLineItemId).Contains(l.Id));
                if (lineItem != null)
                {
                    var fulfillmentItem = new FulfillmentItem()
                    {
                        SerialNumber = inventory.Item.IsSerialized ? inventory.SerialNumber : string.Empty,
                        AssetTagNumber = inventory.Item.IsAssetTagged ? inventory.AssetTagNumber : string.Empty,
                        LotNumber = inventory.Item.IsLotNumbered ? inventory.LotNumber : string.Empty,
                        Count = lineItem.ItemCount,
                        FulfillmentType = orderType.ToString(),
                        OrderLineItemId = lineItem.Id,
                        ItemId = inventory.ItemId
                    };
                    fulfillmentItems.Add(fulfillmentItem);
                }
            }

            return fulfillmentItems;
        }





        private async Task ValidateCompleteFulfilledOrder(int orderModelId, IEnumerable<Models.Inventory> inventories, int? CurrentLocationId,
                                                             InventoryStatusTypes inventoryStatusTypes)
        {
            await ValidateInventory(inventories, CurrentLocationId, inventoryStatusTypes);

            var updatedOrderModel = await _orderHeadersRepository.GetByIdAsync(orderModelId);

            Assert.Equal((int)OrderHeaderStatusTypes.Completed, updatedOrderModel.StatusId);
            foreach (var lineItem in updatedOrderModel.OrderLineItems)
            {
                Assert.Equal((int)OrderLineItemStatusTypes.Completed, lineItem.StatusId);
            }
        }

        private async Task ValidateInventory(IEnumerable<Models.Inventory> inventories, int? CurrentLocationId,
                                            InventoryStatusTypes inventoryStatusTypes)
        {
            var inventoryIds = inventories.Select(i => i.Id);
            var updatedInventories = await _inventoryRepository.GetManyAsync(i => inventoryIds.Contains(i.Id));
            foreach (var updatedInventory in updatedInventories)
            {
                if (!updatedInventory.Item.IsLotNumbered)
                {
                    Assert.Equal(CurrentLocationId, updatedInventory.CurrentLocationId);
                    Assert.Equal((int)inventoryStatusTypes, updatedInventory.StatusId);
                }
            }
        }

        private async Task ValidatePartiallyFulfilledOrder(int orderModelId, IEnumerable<Models.Inventory> fulfilmentLineItems, int? CurrentLocationId,
                                                            InventoryStatusTypes inventoryStatusTypes,
                                                            OrderTypes lineItemAction = OrderTypes.Delivery)
        {
            await ValidateInventory(fulfilmentLineItems, CurrentLocationId, inventoryStatusTypes);
            var updatedOrderModel = await _orderHeadersRepository.GetByIdAsync(orderModelId);

            Assert.Equal((int)OrderHeaderStatusTypes.Partial_Fulfillment, updatedOrderModel.StatusId);
            foreach (var lineItem in updatedOrderModel.OrderLineItems)
            {
                if (lineItem.ActionId == (int)lineItemAction)
                {
                    var fulfilledlineItem = fulfilmentLineItems.FirstOrDefault(l => l.SerialNumber == lineItem.SerialNumber
                                                                                && l.AssetTagNumber == lineItem.AssetTagNumber
                                                                                && l.LotNumber == lineItem.LotNumber);
                    if (fulfilledlineItem != null)
                    {
                        Assert.Equal((int)OrderLineItemStatusTypes.Completed, lineItem.StatusId);
                    }
                    else
                    {
                        Assert.Equal((int)OrderLineItemStatusTypes.Planned, lineItem.StatusId);
                    }
                }
            }
        }

        private async Task<int> GetVehicleInventoryCount(int itemId, int vehicleId)
        {
            var count = 0;
            var vehicleInventory = await _inventoryRepository.GetAsync(i => i.ItemId == 5 && i.CurrentLocationId == vehicleId);
            if (vehicleInventory != null)
            {
                count = vehicleInventory.QuantityAvailable;
            }
            return count;
        }

        private void ValidateNetSuiteRequestForOrderFulfillment(Guid patientUUID, List<Models.Inventory> inventories)
        {

            Assert.NotEmpty(_mockModels.InventoryMovementRequests);
            var inventoryMovementRequest = _mockModels.InventoryMovementRequests.FirstOrDefault();
            Assert.Equal(inventoryMovementRequest.Items.Count(), inventories.Count);
            foreach (var netSuiteMovedItem in inventoryMovementRequest.Items)
            {
                var fulfilledInventoryItem = inventories.FirstOrDefault(i => i.Item.NetSuiteItemId == netSuiteMovedItem.NetSuiteItemId
                                                                                && i.AssetTagNumber == netSuiteMovedItem.AssetTag
                                                                                && i.SerialNumber == netSuiteMovedItem.SerialNumber
                                                                                && i.LotNumber == netSuiteMovedItem.LotNumber);
                Assert.NotNull(fulfilledInventoryItem);
            }

            Assert.NotEmpty(_mockModels.ConfirmFulfilmentRequests);
            var fulfillmentRequest = _mockModels.ConfirmFulfilmentRequests.FirstOrDefault();
            Assert.Equal(fulfillmentRequest.Items.Count(), inventories.Count);
            Assert.True(fulfillmentRequest.DispatchOnly);
            Assert.Equal(patientUUID.ToString(), fulfillmentRequest.PatientUuid);
            foreach (var netSuiteRequestItem in fulfillmentRequest.Items)
            {
                var fulfilledInventoryItem = inventories.FirstOrDefault(i => i.Item.NetSuiteItemId == netSuiteRequestItem.NetSuiteItemId 
                                                                                && i.AssetTagNumber == netSuiteRequestItem.AssetTag
                                                                                && i.SerialNumber == netSuiteRequestItem.SerialNumber
                                                                                && i.LotNumber == netSuiteRequestItem.LotNumber);
                Assert.NotNull(fulfilledInventoryItem);
            }
        }

        #endregion
    }
}
