using Core.Test.MockProvider;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models = HMSDigital.Core.Data.Models;
namespace Core.Test.Services
{
    public class MockData
    {
        private readonly IOrderHeadersRepository _orderHeadersRepository;

        private readonly IInventoryRepository _inventoryRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly IPatientInventoryRepository _patientInventoryRepository;

        private readonly MockModels _mockModels;

        private const int PATIENT_NETSUITE_WAREHOUSE_ID = 999;

        public MockData(MockServices mockService, MockModels mockModels)
        {
            _mockModels = mockModels;
            _orderHeadersRepository = mockService.GetService<IOrderHeadersRepository>();
            _inventoryRepository = mockService.GetService<IInventoryRepository>();
            _sitesRepository = mockService.GetService<ISitesRepository>();
            _patientInventoryRepository = mockService.GetService<IPatientInventoryRepository>();
        }

        public async Task<Models.Inventory> CreateInventory(int itemId, string serialNumber = "", string assetTagNumber = "", string lotNumber = "", int count = 1, int siteId = 1, int statusId = 0)
        {
            var itemModel = _mockModels.Items.FirstOrDefault(i => i.Id == itemId);
            var inventoryModel = new Models.Inventory()
            {
                Item = itemModel,
                ItemId = itemId,
                SerialNumber = itemModel.IsSerialized ? serialNumber : string.Empty,
                AssetTagNumber = itemModel.IsAssetTagged ? assetTagNumber : string.Empty,
                LotNumber = itemModel.IsLotNumbered ? lotNumber : string.Empty,
                CurrentLocationId = siteId,
                QuantityAvailable = count,
                StatusId = statusId
            };
            await _inventoryRepository.AddAsync(inventoryModel);
            return inventoryModel;
        }

        public async Task<Models.OrderHeaders> CreateOrder(OrderTypes orderType, Guid patientUUID, IEnumerable<Models.Inventory> pickupInventory, IEnumerable<Models.Inventory> dropInventory, bool assignSite = true, int statusId = (int)OrderHeaderStatusTypes.Planned)
        {
            var orderLineItems = new List<Models.OrderLineItems>();
            if (pickupInventory != null)
            {
                foreach (var inventory in pickupInventory)
                {
                    orderLineItems.Add(GetOrderLineItem(inventory.ItemId, OrderTypes.Pickup, inventory.SerialNumber, inventory.AssetTagNumber, inventory.LotNumber));
                }
            }
            if (dropInventory != null)
            {
                foreach (var inventory in dropInventory)
                {
                    orderLineItems.Add(GetOrderLineItem(inventory.ItemId, OrderTypes.Delivery, inventory.SerialNumber, inventory.AssetTagNumber, inventory.LotNumber));
                }
            }
            var orderModel = new Models.OrderHeaders()
            {
                OrderTypeId = (int)orderType,
                Id = 1,
                OrderLineItems = orderLineItems,
                HospiceId = 1,
                HospiceLocationId = 1,
                NetSuiteCustomerId = 1,
                StatusId = statusId,
                DispatchStatusId = statusId,
                PatientUuid = patientUUID,
                DeliveryAddress = new Models.Addresses(),
                PickupAddress = new Models.Addresses()
            };
            if (assignSite)
            {
                orderModel.SiteId = 1;
                orderModel.Site = new Models.Sites()
                {
                    Id = 10,
                    NetSuiteLocationId = 10010
                };
            }
            await _orderHeadersRepository.AddAsync(orderModel);
            return orderModel;
        }

        public async Task<Models.Sites> GetPatientSite()
        {
            return await _sitesRepository.GetAsync(s => s.NetSuiteLocationId == PATIENT_NETSUITE_WAREHOUSE_ID);
        }

        public async Task<IEnumerable<Models.PatientInventory>> AddPatientInventory(Guid patientUUID, IEnumerable<Models.Inventory> inventories)
        {
            var patientInventories = inventories.Select(i => new Models.PatientInventory()
            {
                InventoryId = i.Id,
                Inventory = i,
                Item = i.Item,
                ItemId = i.ItemId,
                PatientUuid = patientUUID,
                StatusId = (int)InventoryStatusTypes.NotReady,
                ItemCount = 1
            });
            await _patientInventoryRepository.AddManyAsync(patientInventories);
            return patientInventories;
        }

        private Models.OrderLineItems GetOrderLineItem(int itemId, OrderTypes action, string serialNumber = "", string assetTagNumber = "", string lotNumber = "")
        {
            return new Models.OrderLineItems()
            {
                ItemId = itemId,
                Item = _mockModels.Items.FirstOrDefault(i => i.Id == itemId),
                ItemCount = 1,
                ActionId = (int)action,
                Action = new Models.OrderTypes()
                {
                    Name = action.ToString()
                },
                AssetTagNumber = assetTagNumber,
                SerialNumber = serialNumber,
                LotNumber = lotNumber,
                StatusId = (int)OrderLineItemStatusTypes.Planned

            };
        }

    }
}
