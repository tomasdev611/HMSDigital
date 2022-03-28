using Core.Test.MockProvider;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels.NetSuite;
using Sieve.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Core.Test.Services
{
    public class ItemServiceUnitTest
    {
        private readonly SieveModel _sieveModel;

        private readonly MockViewModels _mockViewModels;

        private readonly IItemsService _itemsService;

        private readonly IInventoryService _inventoryService;

        private readonly MockModels _mockModels;

        public ItemServiceUnitTest()
        {
            _mockViewModels = new MockViewModels();
            var mockService = new MockServices();
            _mockModels = mockService.GetService<MockModels>();
            _itemsService = mockService.GetService<IItemsService>();
            _inventoryService = mockService.GetService<IInventoryService>();
            _sieveModel = new SieveModel();
        }

        #region Item

        [Fact]
        public async Task GetAllItemShouldReturnValidList()
        {
            var items = await _itemsService.GetAllItems(_sieveModel);
            Assert.NotEmpty(items.Records);
        }

        [Fact]
        public async Task SearchItemShouldReturnValidList()
        {
            var items = await _itemsService.SearchItems(_sieveModel, "Item");
            Assert.NotEmpty(items.Records);
        }

        [Fact]
        public async Task ItemShouldBeCreatedForValidData()
        {
            var itemRequest = _mockViewModels.GetItemRequest();
            var createdItem = await _itemsService.CreateItem(itemRequest);
            var item = await _itemsService.GetItemById(createdItem.Id);
            Assert.NotNull(item);
            Assert.Equal(itemRequest.Name, item.Name);
            Assert.Equal(itemRequest.ItemNumber, item.ItemNumber);
            Assert.Equal(itemRequest.Description, item.Description);
        }

        [Fact]
        public async Task ItemCreationShouldBeFailedForInvalidData()
        {
            var itemRequest = _mockViewModels.GetItemRequest();
            itemRequest.Name = "";
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.CreateItem(itemRequest));
        }

        [Fact]
        public async Task ItemCreationShouldBeFailedForAlreadyExistItemNumber()
        {
            var itemRequest = _mockViewModels.GetItemRequest();
            itemRequest.ItemNumber = "ItemNumber_5";
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.CreateItem(itemRequest));
        }

        [Fact]
        public async Task ItemCreationShouldBeFailedForInvalidCategory()
        {
            var itemRequest = _mockViewModels.GetItemRequest();
            itemRequest.CategoryId = 5;
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.CreateItem(itemRequest));
        }

        [Fact]
        public async Task PatchItemShouldUpdatItemForValidData()
        {
            var itemRequest = _mockViewModels.GetItemRequest();
            var createdItem = await _itemsService.CreateItem(itemRequest);
            var item = await _itemsService.GetItemById(createdItem.Id);
            Assert.NotNull(item);
            Assert.Equal(itemRequest.Name, item.Name);
            Assert.Equal(itemRequest.ItemNumber, item.ItemNumber);
            Assert.Equal(itemRequest.Description, item.Description);
            var updateRequest = _mockViewModels.GetItemJsonPatchDocument();
            var updatedItem = await _itemsService.PatchItem(createdItem.Id, updateRequest);
            var itemAfterUpdate = await _itemsService.GetItemById(updatedItem.Id);
            Assert.Equal("UpdatedItem", itemAfterUpdate.Name);
        }

        [Fact]
        public async Task PatchItemShouldFailedForNotAllowedPath()
        {
            var updateRequest = _mockViewModels.GetItemJsonPatchDocument();
            updateRequest.Replace(i => i.ItemNumber, "CT6600");
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.PatchItem(5, updateRequest));
        }

        [Fact]
        public async Task PatchItemShouldFailedForInvalidData()
        {
            var updateRequest = _mockViewModels.GetItemJsonPatchDocument();
            updateRequest.Replace(i => i.Name, "");
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.PatchItem(5, updateRequest));
        }

        [Fact]
        public async Task PatchItemShouldFailedForInvalidItem()
        {
            var updateRequest = _mockViewModels.GetItemJsonPatchDocument();
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.PatchItem(5500, updateRequest));
        }

        [Fact]
        public async Task ItemShouldBeDeletedForValidItem()
        {
            var itemBeforeDelete = await _itemsService.GetItemById(7);
            Assert.NotNull(itemBeforeDelete);
            await _itemsService.DeleteItem(7);
            var itemAfterDelete = await _itemsService.GetItemById(7);
            Assert.Null(itemAfterDelete);
        }

        [Fact]
        public async Task ItemShouldBeDeletedForNotAvailableItem()
        {
            await _itemsService.DeleteItem(6600);
            var itemAfterDelete = await _itemsService.GetItemById(6600);
            Assert.Null(itemAfterDelete);
        }

        [Fact]
        public async Task ItemDeletionShouldBeFailedForInventoryAvailableItem()
        {
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.DeleteItem(1));
        }

        [Fact]
        public async Task ItemShouldBeDeletedForValidNetsuiteId()
        {
            var deleteRequest = new ItemDeleteRequest()
            {
                Id = 7,
                NetSuiteItemId = 7,
                DeletedByUserEmail = "test@zbc.com"
            };
            var itemBeforeDelete = await _itemsService.GetItemById(deleteRequest.Id);
            Assert.NotNull(itemBeforeDelete);
            await _itemsService.DeleteItemByNetSuiteId(deleteRequest);
            var itemAfterDelete = await _itemsService.GetItemById(deleteRequest.Id);
            Assert.Null(itemAfterDelete);
        }

        #endregion

        #region Item Integration

        [Fact]
        public async Task UpsertItemShouldCreateItemForValidData()
        {
            var itemRequest = _mockViewModels.GetNSItemRequest(2020, true, true, false);
            var createdItem = await _itemsService.UpsertItem(itemRequest);
            await ValidateItemAssertion(createdItem.Id, itemRequest);
        }

        [Fact]
        public async Task UpsertItemShouldUpdateItemForValidData()
        {
            var itemRequest = _mockViewModels.GetNSItemRequest(2020, true, true, false);
            var createdItem = await _itemsService.UpsertItem(itemRequest);
            await ValidateItemAssertion(createdItem.Id, itemRequest);
            var newInventory = _mockViewModels.GetNSInventory(2021, true, true, false);
            itemRequest.Inventory = itemRequest.Inventory.Append(newInventory);
            var updatedItem = await _itemsService.UpsertItem(itemRequest);
            await ValidateItemAssertion(updatedItem.Id, itemRequest);
        }

        [Fact]
        public async Task UpsertItemShouldFailedWhileItemCreateForAlreadyExistItemNumber()
        {
            var itemRequest = _mockViewModels.GetNSItemRequest(2020, true, true, false);
            itemRequest.ItemNumber = "ItemNumber_5";
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.UpsertItem(itemRequest));
        }

        [Fact]
        public async Task UpsertItemShouldFailedWhileItemUpdateForAlreadyExistItemNumber()
        {
            var itemRequest = _mockViewModels.GetNSItemRequest(2020, true, true, false);
            var createdItem = await _itemsService.UpsertItem(itemRequest);
            await ValidateItemAssertion(createdItem.Id, itemRequest);
            itemRequest.ItemNumber = "ItemNumber_5";
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.UpsertItem(itemRequest));
        }

        [Fact]
        public async Task UpsertItemShouldCreatedStanaloneItemForValidData()
        {
            var itemRequest = _mockViewModels.GetNSItemRequest(2020, false, false, false);
            var createdItem = await _itemsService.UpsertItem(itemRequest);
            await ValidateItemAssertion(createdItem.Id, itemRequest);
        }

        [Fact]
        public async Task UpsertItemShouldFailedToCreateStanaloneItemMoreThanOneInventoryLineForOneLocation()
        {
            var itemRequest = _mockViewModels.GetNSItemRequest(2020, false, false, false);
            var newInventory = _mockViewModels.GetNSInventory(2021, false, false, false);
            itemRequest.Inventory = itemRequest.Inventory.Append(newInventory);
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.UpsertItem(itemRequest));
        }

        [Fact]
        public async Task UpsertItemShouldFailedToCreateItemForDuplicateNetsuiteId()
        {
            var itemRequest = _mockViewModels.GetNSItemRequest(2020, true, true, false);
            var newInventory = _mockViewModels.GetNSInventory(2020, true, true, false);
            itemRequest.Inventory = itemRequest.Inventory.Append(newInventory);
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.UpsertItem(itemRequest));
        }

        #endregion

        #region Transfer Request

        [Fact]
        public async Task TransferRequestShouldBeCreatedForValidData()
        {
            var itemId = 6;
            var itemTransferRequest = _mockViewModels.GetItemTransferCreateRequest(1, 2, 1, 2);
            await _itemsService.TransferItem(itemId, itemTransferRequest);
            var transferRequests = await _itemsService.GetTransferRequests(itemId);
            Assert.NotEmpty(transferRequests);
            foreach (var transferRequest in transferRequests)
            {
                Assert.Equal(itemTransferRequest.ItemCount, transferRequest.ItemCount);
                Assert.Equal(itemTransferRequest.SourceLocationId, transferRequest.SourceLocationId);
                Assert.Equal(itemTransferRequest.DestinationLocationId, transferRequest.DestinationLocationId);
                Assert.Equal(itemTransferRequest.DestinationSiteMemberId, transferRequest.DestinationSiteMemberId);
                Assert.Equal(itemId, transferRequest.ItemId);
            }
        }

        [Theory]
        [InlineData(6, 1, 2, 0, 2)]
        [InlineData(6, 1, 2, 1, 2222)]
        [InlineData(6, 2, 1, 1, 1)]
        [InlineData(6, 2222, 1, 1, 1)]
        [InlineData(666, 1, 2, 1, 2)]
        public async Task TransferRequestShouldBeFailedForInvalidData(int itemId, int sourceLocationId, int destinationLocationId, int count, int siteMemberId)
        {
            var itemTransferRequest = _mockViewModels.GetItemTransferCreateRequest(sourceLocationId, destinationLocationId, count, siteMemberId);
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.TransferItem(itemId, itemTransferRequest));
        }
        #endregion

        #region Item Image

        [Fact]
        public async Task AddItemImageShouldBeAddedForValidData()
        {
            var itemId = 6;
            var itemImageRequest = _mockViewModels.GetItemImageFileRequest(itemId);
            var createdItemImageFile = await _itemsService.AddItemImage(itemId, itemImageRequest);
            var itemImages = _mockModels.ItemImageFiles.Where(i => i.ItemId == itemId);
            Assert.NotEmpty(itemImages);
        }

        [Fact]
        public async Task ItemImageShouldBeConfirmUploadedForValidData()
        {
            var itemId = 6;
            var itemImageRequest = _mockViewModels.GetItemImageFileRequest(itemId);
            var createdItemImageFile = await _itemsService.AddItemImage(itemId, itemImageRequest);
            var itemImages = _mockModels.ItemImageFiles.Where(i => i.ItemId == itemId);
            Assert.NotEmpty(itemImages);
            foreach(var itemImage in itemImages)
            {
                var confirmedImageUpload = await _itemsService.ConfirmImageUpload(itemId, itemImage.Id);
                var image = _mockModels.ItemImageFiles.FirstOrDefault(i => i.Id == itemImage.Id);
                Assert.NotNull(image);
                Assert.True(image.IsUploaded);
            }
        }

        [Fact]
        public async Task GetAllItemImagesShouldReturnValidList()
        {
            var itemImages = await _itemsService.GetAllItemImages(_sieveModel);
            Assert.NotEmpty(itemImages.Records);
        }

        [Fact]
        public async Task GetItemImagesShouldReturnValidList()
        {
            var itemImages = await _itemsService.GetItemImages(6);
            Assert.NotEmpty(itemImages);
        }

        [Theory]
        [InlineData(6, 1)]
        [InlineData(666, 1)]
        public async Task ItemImageConfirmUploadShouldFailForInvalidData(int itemId, int imageId)
        {
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.ConfirmImageUpload(itemId, imageId));
        }

        [Fact]
        public async Task GetItemImagesShouldFailForInvalidItem()
        {
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.GetItemImages(66));
        }

        [Fact]
        public async Task AddItemImageShouldBeFailedForInvalidItem()
        {
            var itemId = 66;
            var itemImageRequest = _mockViewModels.GetItemImageFileRequest(itemId);
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.AddItemImage(itemId, itemImageRequest));
        }

        [Fact]
        public async Task AddItemImageShouldBeFailedForInvalidData()
        {
            var itemId = 66;
            var itemImageRequest = _mockViewModels.GetItemImageFileRequest(itemId);
            itemImageRequest.ContentType = "json";
            await Assert.ThrowsAsync<ValidationException>(() => _itemsService.AddItemImage(itemId, itemImageRequest));
        }
        #endregion

        private async Task ValidateItemAssertion(int id, NSItemRequest itemRequest)
        {
            var item = await _itemsService.GetItemById(id);
            Assert.NotNull(item);
            var inventories = await _inventoryService.GetAllInventory(_sieveModel);
            Assert.NotEmpty(inventories.Records);
            Assert.Equal(itemRequest.Name, item.Name);
            Assert.Equal(itemRequest.ItemNumber, item.ItemNumber);
            Assert.Equal(itemRequest.NetSuiteItemId, item.NetSuiteItemId);
            foreach (var inventory in itemRequest.Inventory)
            {
                var createdInventory = inventories.Records.FirstOrDefault(i => i.NetSuiteInventoryId == inventory.NetSuiteInventoryId && i.ItemId == id);
                Assert.NotNull(createdInventory);
                Assert.Equal(inventory.AssetTagNumber, createdInventory.AssetTagNumber);
                Assert.Equal(inventory.SerialNumber, createdInventory.SerialNumber);
                Assert.Equal(inventory.LotNumber, createdInventory.LotNumber);
                Assert.Equal(inventory.QuantityAvailable, createdInventory.QuantityAvailable);
            }
        }
    }
}
