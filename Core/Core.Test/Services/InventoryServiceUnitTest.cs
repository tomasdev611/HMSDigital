using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Enums;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Azure;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Models = HMSDigital.Core.Data.Models;
using NSViewModels = HMSDigital.Core.ViewModels.NetSuite;
using CoreViewModels = HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Core.Test.Services
{
    public class InventoryServiceUnitTest
    {
        private readonly IInventoryService _inventoryService;

        private readonly IHospiceService _hospiceService;

        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        private readonly MockData _mockData;

        private readonly MockModels _mockModels;

        public InventoryServiceUnitTest()
        {
            _mockViewModels = new MockViewModels();
            var mockService = new MockServices();
            _inventoryService = mockService.GetService<IInventoryService>();
            _hospiceService = mockService.GetService<IHospiceService>();

            _mockModels = mockService.GetService<MockModels>();
            _mockData = new MockData(mockService, _mockModels);
            _sieveModel = new SieveModel();
        }

        [Fact]
        public async Task SearchInventoryByItemNameShouldReturnValidList()
        {
            string itemName = "ItemName";
            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, itemName);
            Assert.NotEmpty(inventoryResult.Records);
            Assert.Contains(itemName, inventoryResult.Records.First().Item.Name);
        }

        [Fact]
        public async Task SearchInventoryByItemNameShouldNotBeCaseSensitive()
        {

            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, "iTeMnAme");
            Assert.NotEmpty(inventoryResult.Records);
            Assert.Contains("ItemName", inventoryResult.Records.First().Item.Name);
        }

        [Fact]
        public async Task SearchInventoryByItemNamePartialShouldReturnValidList()
        {
            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, "ItemN");
            Assert.NotEmpty(inventoryResult.Records);
            Assert.Contains("ItemName", inventoryResult.Records.First().Item.Name);
        }

        [Fact]
        public async Task SearchInventoryByItemDescriptionShouldReturnValidList()
        {
            string itemDescription = "ItemDescription";
            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, itemDescription);
            Assert.NotEmpty(inventoryResult.Records);
            Assert.Contains(itemDescription, inventoryResult.Records.First().Item.Description);
        }

        [Fact]
        public async Task SearchInventoryByItemNumberShouldReturnValidList()
        {
            string itemNumber = "ItemNumber";
            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, itemNumber);
            Assert.NotEmpty(inventoryResult.Records);
            Assert.Contains(itemNumber, inventoryResult.Records.First().Item.ItemNumber);
        }

        [Fact]
        public async Task SearchInventoryByItemCogsAccountNameShouldReturnValidList()
        {
            string itemCogsAccountName = "CogsAccountName";
            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, itemCogsAccountName);
            Assert.NotEmpty(inventoryResult.Records);
            Assert.Contains(itemCogsAccountName, inventoryResult.Records.First().Item.CogsAccountName);
        }

        [Fact]
        public async Task SearchInventoryBySerialNumberShouldReturnValidList()
        {
            string serialNamber = "123456";
            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, serialNamber);
            Assert.NotEmpty(inventoryResult.Records);
            Assert.Equal(serialNamber, inventoryResult.Records.First().SerialNumber);
        }

        [Fact]
        public async Task SearchInventoryByAssetTagNumberShouldReturnValidList()
        {
            string assetTagNumber = "654321";
            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, assetTagNumber);
            Assert.NotEmpty(inventoryResult.Records);
            Assert.Equal(assetTagNumber, inventoryResult.Records.First().AssetTagNumber);
        }

        [Fact]
        public async Task SearchInventoryByLotNumberShouldReturnValidList()
        {
            string lotNumber = "212121212121";
            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, lotNumber);
            Assert.NotEmpty(inventoryResult.Records);
            Assert.Equal(lotNumber, inventoryResult.Records.First().LotNumber);
        }

        [Fact]
        public async Task SearchInventoryByEmptySearchQueryShouldReturnNull()
        {
            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, "");
            Assert.Null(inventoryResult);
        }

        [Fact]
        public async Task SearchInventoryByNullSearchQueryShouldReturnNull()
        {
            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, null);
            Assert.Null(inventoryResult);
        }

        [Fact]
        public async Task SearchInventoryByWhiteSpacesSearchQueryShouldReturnNull()
        {
            var inventoryResult = await _inventoryService.SearchInventoryBySearchQuery(_sieveModel, "  ");
            Assert.Null(inventoryResult);
        }

        [Fact]
        public async Task GetInventoryWithoutAccessShouldNotReturnResult()
        {
            var inventoryResult = await _inventoryService.GetAllInventory(_sieveModel);
            Assert.NotEmpty(inventoryResult.Records);
            _mockModels.LoggedInUser.UserRoles = new List<Models.UserRoles>();
            var inventoryResultWithoutUserRoles = await _inventoryService.GetAllInventory(_sieveModel);
            Assert.Empty(inventoryResultWithoutUserRoles.Records);
        }

        [Fact]
        public async Task DeleteInventoryShouldSucceed()
        {
            var inventoryResult = await _inventoryService.GetInventoryById(1);
            Assert.NotNull(inventoryResult);

            await _inventoryService.DeleteInventory(1);

            var inventoryResultAfterDeletion = await _inventoryService.GetInventoryById(1);
            Assert.Null(inventoryResultAfterDeletion);
        }

        [Fact]
        public async Task DeleteInventoryByNetSuiteInventoryIdShouldSucceed()
        {
            var inventoryResult = await _inventoryService.GetInventoryById(1);
            Assert.NotNull(inventoryResult);

            var inventoryDeleteRequest = new NSViewModels.InventoryDeleteRequest()
            {
                NetSuiteInventoryId = inventoryResult.NetSuiteInventoryId
            };
            await _inventoryService.DeleteInventoryByNetSuiteId(inventoryDeleteRequest);

            var inventoryResultAfterDeletion = await _inventoryService.GetInventoryById(inventoryResult.Id);
            Assert.Null(inventoryResultAfterDeletion);
        }

        [Fact]
        public async Task AddPatientInventoryShouldSucceed()
        {
            var patientUUID = Guid.NewGuid();
            var inventory = await _inventoryService.GetInventoryById(1);

            var searchQuery = !string.IsNullOrEmpty(inventory.AssetTagNumber) ? inventory.AssetTagNumber : inventory.SerialNumber;
            var existingPatientInventory = await _inventoryService.SearchPatientInventoryBySearchQuery(patientUUID.ToString(), new SieveModel(), searchQuery);
            Assert.Empty(existingPatientInventory.Records);

            var hospice = await _hospiceService.GetHospiceById(1);

            var patientInventoryRequest = new HMSDigital.Core.ViewModels.PatientInventoryRequest()
            {
                ItemNumber = inventory.Item.ItemNumber,
                Quantity = 1,
                HospiceId = hospice.Id,
                HospiceLocationId = hospice.HospiceLocations.FirstOrDefault().Id,
                AssetTagNumber = inventory.AssetTagNumber,
                SerialNumber = inventory.SerialNumber,
                LotNumber = inventory.LotNumber
            };
            await _inventoryService.AddPatientInventory(patientUUID, patientInventoryRequest);
            var updatedPatientInventory = await _inventoryService.SearchPatientInventoryBySearchQuery(patientUUID.ToString(), new SieveModel(), searchQuery);
            Assert.NotEmpty(updatedPatientInventory.Records);
            Assert.Equal(inventory.Item.ItemNumber, updatedPatientInventory.Records.FirstOrDefault().ItemNumber);
        }

        [Fact]
        public async Task GetPatientInventoryShouldReturnPickupDetails()
        {
            var patientUUID = Guid.NewGuid();
            var patientSite = await _mockData.GetPatientSite();
            var orderedInventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(1, serialNumber: "s123Pickup", statusId:2),
               await _mockData.CreateInventory(5,siteId:patientSite.Id, statusId:2)
            };
            var orderedPatientInventories = await _mockData.AddPatientInventory(patientUUID, orderedInventories);

            var orderModel = await _mockData.CreateOrder(OrderTypes.Pickup, patientUUID, orderedInventories, null);

            var unorderedInventories = new List<Models.Inventory>
            {
               await _mockData.CreateInventory(2, assetTagNumber: "a123Pickup", statusId:2),
               await _mockData.CreateInventory(6, siteId:patientSite.Id, statusId:2)
            };

            var unorderedPatientInventories = await _mockData.AddPatientInventory(patientUUID, unorderedInventories);

            var updatedPatientInventory = await _inventoryService.GetPatientInventory(patientUUID.ToString(), new SieveModel(), includePickupDetails: true, includeDeliveryAddress: false);
            foreach (var inventory in orderedInventories)
            {
                var patientInventory = updatedPatientInventory.Records.FirstOrDefault(pi => pi.ItemId == inventory.ItemId);
                Assert.NotNull(patientInventory);
                Assert.True(patientInventory.IsPartOfExistingPickup);
            }
            foreach (var inventory in unorderedInventories)
            {
                var patientInventory = updatedPatientInventory.Records.FirstOrDefault(pi => pi.ItemId == inventory.ItemId);
                Assert.NotNull(patientInventory);
                Assert.False(patientInventory.IsPartOfExistingPickup);
            }
        }

        [Fact]
        public async Task CreateInventoryShouldSucceed()
        {
            var inventoryRequest = new HMSDigital.Core.ViewModels.InventoryRequest()
            {
                ItemId = 1,
                CurrentLocationId = 1,
                SerialNumber = "sXYZ",
                QuantityAvailable = 1
            };
            await _inventoryService.CreateInventory(inventoryRequest);
        }

        [Fact]
        public async Task PatchInventoryShouldSucceed()
        {
            var inventory = await _mockData.CreateInventory(1, serialNumber: "s123Pickup", statusId: 2);
            var inventoryPatchRequest = new JsonPatchDocument<CoreViewModels.Inventory>();
            inventoryPatchRequest.Operations.Add(new Operation<CoreViewModels.Inventory>() { op = "replace", path = "/CurrentLocationId", value = "2" });
            await _inventoryService.PatchInventory(inventory.Id, inventoryPatchRequest);
            var updatedInventory = await _inventoryService.GetInventoryById(inventory.Id);
            Assert.Equal(2, updatedInventory.CurrentLocationId);
        }

        [Fact]
        public async Task MoveInventoryShouldSucceed()
        {
            var inventory = await _mockData.CreateInventory(1, serialNumber: "s123Pickup", statusId: 2);
            var moveInventoryRequest = new CoreViewModels.MoveInventoryRequest()
            {
                AssetTagNumber = inventory.AssetTagNumber,
                SerialNumber = inventory.SerialNumber,
                DestinationLocationId = 2,
                ItemNumber = inventory.Item.ItemNumber
            };
            await _inventoryService.MoveInventory(moveInventoryRequest);
            var updatedInventory = await _inventoryService.GetInventoryById(inventory.Id);
            Assert.Equal(2, updatedInventory.CurrentLocationId);
        }

        [Fact]
        public async Task UpsertNSInventoryShouldSucceed()
        {
            var initialSite = _mockModels.Sites.FirstOrDefault(s => s.Id == 1);
            var inventoryRequest = new NSViewModels.NSInventoryRequest()
            {
                NetSuiteInventoryId = new Random().Next(10000),
                NetSuiteLocationId = initialSite.NetSuiteLocationId,
                NetSuiteItemId = 1,
                SerialNumber = "sXYZ",
                QuantityOnHand = 1,
                QuantityAvailable = 1
            };
            var result = await _inventoryService.UpsertInventory(inventoryRequest);

            var createdInventories = await _inventoryService.SearchInventoryBySearchQuery(new SieveModel(), inventoryRequest.SerialNumber);
            Assert.NotEmpty(createdInventories.Records);
            var createdInventory = createdInventories.Records.FirstOrDefault(i => i.CurrentLocationId == initialSite.Id);
            Assert.NotNull(createdInventory);
            Assert.Equal("sXYZ",createdInventory.SerialNumber);

            inventoryRequest.SerialNumber = "sABC";
            var updatedResult = await _inventoryService.UpsertInventory(inventoryRequest);
            var updatedInventories = await _inventoryService.SearchInventoryBySearchQuery(new SieveModel(), inventoryRequest.SerialNumber);
            Assert.NotEmpty(updatedInventories.Records);
            var updatedInventory = updatedInventories.Records.FirstOrDefault(i => i.CurrentLocationId == initialSite.Id);
            Assert.NotNull(updatedInventory);
            Assert.Equal("sABC", updatedInventory.SerialNumber);

        }
    }
}
