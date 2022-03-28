using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IItemsService
    {
        Task<PaginatedList<Item>> GetAllItems(SieveModel sieveModel);

        Task<Item> GetItemById(int itemId);

        Task<Item> CreateItem(ItemRequest itemRequest);

        Task<ViewModels.NetSuite.ItemResponse> UpsertItem(ViewModels.NetSuite.NSItemRequest itemRequest);

        Task<Item> PatchItem(int itemId, JsonPatchDocument<Item> itemPatchDocument);

        Task<ItemImageFile> AddItemImage(int itemId, ItemImageFileRequest imageFile);

        Task<IEnumerable<ItemImage>> GetItemImages(int itemId);

        Task<PaginatedList<ItemImage>> GetAllItemImages(SieveModel sieveModel);

        Task<ItemImageFile> ConfirmImageUpload(int itemId, int imageId);

        Task<IEnumerable<ItemTransferRequest>> GetTransferRequests(int itemId);

        Task TransferItem(int itemId, ItemTransferCreateRequest itemTransferRequest);

        Task DeleteItem(int itemId);

        Task<ViewModels.NetSuite.ItemResponse> DeleteItemByNetSuiteId(ViewModels.NetSuite.ItemDeleteRequest itemDeleteRequest);

        Task<PaginatedList<Item>> SearchItems(SieveModel sieveModel, string searchQuery);

        Task<IEnumerable<Item>> GetItemsByIds(IEnumerable<int> ids);

        Task<IEnumerable<Item>> GetItemsByNetSuiteItemIds(IEnumerable<int> netSuiteItemIds);

        Task<Item> UpdateEquipmentSettingConfig(int itemId, IEnumerable<EquipmentSettingConfig> equipmentSettingsRequest);

        Task<IEnumerable<EquipmentSettingType>> GetEquipmentSettingTypes(SieveModel sieveModel);

        Task<Item> UpdateAddOnsConfig(int itemId, IEnumerable<AddOnsConfigRequest> addOnsConfigRequests);
    }
}
