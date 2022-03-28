using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using HospiceSource.Digital.NetSuite.SDK.Config;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public class ItemsService : IItemsService
    {
        private readonly IFileStorageService _fileStorageService;

        private readonly IItemRepository _itemRepository;

        private readonly IItemCategoryRepository _itemCategoryRepository;

        private readonly IItemSubCategoryRepository _itemSubCategoryRepository;

        private readonly IInventoryRepository _inventoryRepository;

        private readonly IItemTransferRequestRepository _itemTransferRequestRepository;

        private readonly IItemImageFilesRepository _itemImageFilesRepository;

        private readonly IItemImageRepository _itemImageRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly ISiteMemberRepository _siteMemberRepository;

        private readonly IEquipmentSettingTypeRepository _equipmentSettingTypeRepository;

        private readonly IInventoryService _inventoryService;

        private readonly IPaginationService _paginationService;

        private readonly NetSuiteConfig _netSuiteConfig;

        private readonly IMapper _mapper;

        public ItemsService(IFileStorageService fileStorageService,
            IItemRepository itemRepository,
            IItemCategoryRepository itemCategoryRepository,
            IItemSubCategoryRepository itemSubCategoryRepository,
            IInventoryRepository inventoryRepository,
            IItemTransferRequestRepository itemTransferRequestRepository,
            IItemImageFilesRepository itemImageFilesRepository,
            ISitesRepository sitesRepository,
            ISiteMemberRepository siteMemberRepository,
            IInventoryService inventoryService,
            IPaginationService paginationService,
            IItemImageRepository itemImageRepository,
            IEquipmentSettingTypeRepository equipmentSettingTypeRepository,
            IOptions<NetSuiteConfig> netSuiteOptions,
            IMapper mapper)
        {
            _fileStorageService = fileStorageService;
            _itemRepository = itemRepository;
            _itemCategoryRepository = itemCategoryRepository;
            _itemSubCategoryRepository = itemSubCategoryRepository;
            _inventoryRepository = inventoryRepository;
            _itemTransferRequestRepository = itemTransferRequestRepository;
            _itemImageFilesRepository = itemImageFilesRepository;
            _sitesRepository = sitesRepository;
            _siteMemberRepository = siteMemberRepository;
            _mapper = mapper;
            _inventoryService = inventoryService;
            _paginationService = paginationService;
            _itemImageRepository = itemImageRepository;
            _equipmentSettingTypeRepository = equipmentSettingTypeRepository;
            _netSuiteConfig = netSuiteOptions.Value;
        }

        public async Task<PaginatedList<Item>> GetAllItems(SieveModel sieveModel)
        {
            _itemRepository.SieveModel = sieveModel;
            var itemsCount = await _itemRepository.GetCountAsync(i => true);
            var itemsModel = await _itemRepository.GetAllAsync();
            var items = _mapper.Map<IEnumerable<Item>>(itemsModel);
            return _paginationService.GetPaginatedList(items, itemsCount, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PaginatedList<Item>> SearchItems(SieveModel sieveModel, string searchQuery)
        {
            _itemRepository.SieveModel = sieveModel;
            var itemsCount = await _itemRepository.GetCountAsync(i => i.Name.Contains(searchQuery));
            var itemsModel = await _itemRepository.GetManyAsync(i => i.Name.Contains(searchQuery));
            var items = _mapper.Map<IEnumerable<Item>>(itemsModel);
            return _paginationService.GetPaginatedList(items, itemsCount, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<Item> GetItemById(int itemId)
        {
            var itemModel = await _itemRepository.GetByIdAsync(itemId);
            return _mapper.Map<Item>(itemModel);
        }

        public async Task<Item> CreateItem(ItemRequest itemRequest)
        {
            var itemValidator = new ItemValidator();

            var validationResult = itemValidator.Validate(itemRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            var itemNumberExists = await _itemRepository.ExistsAsync(p => p.ItemNumber == itemRequest.ItemNumber);
            if (itemNumberExists)
            {
                throw new ValidationException($"Item with Item Number ({itemRequest.ItemNumber}) already exists.");
            }

            await ValidateCategory(itemRequest.CategoryId);

            var itemModel = _mapper.Map<Items>(itemRequest);
            await _itemRepository.AddAsync(itemModel);
            return _mapper.Map<Item>(itemModel);
        }

        public async Task<ViewModels.NetSuite.ItemResponse> UpsertItem(ViewModels.NetSuite.NSItemRequest itemRequest)
        {
            var existingItemModel = await _itemRepository.GetAsync(s => s.NetSuiteItemId == itemRequest.NetSuiteItemId);
            if (existingItemModel != null)
            {
                return await UpdateItem(existingItemModel, itemRequest);
            }
            return await CreateItem(itemRequest);

        }

        public async Task<Item> PatchItem(int itemId, JsonPatchDocument<Item> itemPatchDocument)
        {
            var itemModel = await _itemRepository.GetByIdAsync(itemId);
            if (itemModel == null)
            {
                throw new ValidationException($"Item with Id ({itemId}) not found");
            }
            var allowedPaths = new List<string>
            {
                "/Name",
                "/Description"
            };

            foreach (var op in itemPatchDocument.Operations)
            {
                if (!allowedPaths.Any(x => x.Equals(op.path, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ValidationException($"Attempt to modify data outside of user control. Logged and reported.");
                }
            }
            var modelPatch = _mapper.Map<JsonPatchDocument<Items>>(itemPatchDocument);

            modelPatch.ApplyTo(itemModel);

            var itemViewModel = _mapper.Map<Item>(itemModel);
            var itemValidator = new ItemValidator();
            var validationResult = itemValidator.Validate(itemViewModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            await _itemRepository.UpdateAsync(itemModel);

            return _mapper.Map<Item>(itemModel);
        }

        public async Task<IEnumerable<ItemTransferRequest>> GetTransferRequests(int itemId)
        {
            var transferRequests = await _itemTransferRequestRepository.GetManyAsync(i => i.ItemId == itemId);
            return _mapper.Map<IEnumerable<ItemTransferRequest>>(transferRequests);
        }

        public async Task TransferItem(int itemId, ItemTransferCreateRequest itemTransferRequest)
        {
            if (itemTransferRequest.ItemCount <= 0)
            {
                throw new ValidationException($"item count should be greater than zero");
            }

            var ItemModel = await _itemRepository.GetByIdAsync(itemId);
            if (ItemModel == null)
            {
                throw new ValidationException($"item with Id ({itemId}) is not valid");
            }

            await ValidateLocation(itemTransferRequest.DestinationLocationId, "Destination");
            await ValidateLocation(itemTransferRequest.SourceLocationId, "Source");

            if (itemTransferRequest.DestinationSiteMemberId != null)
            {

                var siteMemberExists = await _siteMemberRepository.ExistsAsync(sm => sm.Id == itemTransferRequest.DestinationSiteMemberId
                                                                                  && sm.SiteId == itemTransferRequest.DestinationLocationId);
                if (!siteMemberExists)
                {
                    throw new ValidationException($"siteMember with Id ({itemTransferRequest.DestinationSiteMemberId}) is not a member of site with Id ({itemTransferRequest.DestinationLocationId})");
                }
            }

            var inventoryExists = await _inventoryRepository.ExistsAsync(i => i.CurrentLocationId == itemTransferRequest.SourceLocationId);
            if (!inventoryExists)
            {
                throw new ValidationException($"Item inventory is not available at source location");
            }

            var itemTransferModel = _mapper.Map<ItemTransferRequests>(itemTransferRequest);
            itemTransferModel.StatusId = (int)Data.Enums.TransferRequestStatusTypes.Created;

            ItemModel.ItemTransferRequests.Add(itemTransferModel);

            await _itemRepository.UpdateAsync(ItemModel);
        }

        public async Task DeleteItem(int itemId)
        {
            var itemModel = await _itemRepository.GetByIdAsync(itemId);
            if (itemModel == null)
            {
                return;
            }

            var itemInventories = await _inventoryRepository.GetManyAsync(i => i.ItemId == itemId);
            if (itemInventories.Count() != 0)
            {
                throw new ValidationException("Cannot delete item details having existing inventories");
            }

            await _itemRepository.DeleteAsync(itemModel);
        }

        public async Task<ViewModels.NetSuite.ItemResponse> DeleteItemByNetSuiteId(ViewModels.NetSuite.ItemDeleteRequest itemDeleteRequest)
        {
            var itemModel = await _itemRepository.GetAsync(s => s.NetSuiteItemId == itemDeleteRequest.NetSuiteItemId);
            if (itemModel != null)
            {
                await _itemRepository.DeleteAsync(itemModel);
            }
            return _mapper.Map<ViewModels.NetSuite.ItemResponse>(itemDeleteRequest);
        }

        public async Task<ItemImageFile> AddItemImage(int itemId, ItemImageFileRequest imageFile)
        {
            var itemImageValidator = new ItemImageFileValidator();
            var validationResult = itemImageValidator.Validate(imageFile);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            var itemModel = await _itemRepository.GetByIdAsync(itemId);
            if (itemModel == null)
            {
                throw new ValidationException($"Item with Id({itemId}) does not exist");
            }

            var itemImageFileModel = _mapper.Map<ItemImageFiles>(imageFile);
            itemImageFileModel.ItemId = itemId;

            itemImageFileModel.FileMetadata.StorageTypeId = _fileStorageService.GetStorageTypeId();
            itemImageFileModel.FileMetadata.StorageRoot = _fileStorageService.GetStorageRoot(imageFile);
            itemImageFileModel.FileMetadata.StorageFilePath = _fileStorageService.GetStorageFilePath(imageFile);
            itemImageFileModel.IsUploaded = false;
            await _itemImageFilesRepository.AddAsync(itemImageFileModel);

            var itemImageFile = _mapper.Map<ItemImageFile>(itemImageFileModel);
            itemImageFile.UploadUrl = await _fileStorageService.GetUploadUrl(imageFile, itemImageFileModel.FileMetadata.StorageFilePath);
            return itemImageFile;
        }

        public async Task<IEnumerable<ItemImage>> GetItemImages(int itemId)
        {
            var itemModel = await _itemRepository.GetByIdAsync(itemId);
            if (itemModel == null)
            {
                throw new ValidationException($"Item with Id({itemId}) does not exist");
            }
            var itemImageModels = await _itemImageRepository.GetManyAsync(p => p.ItemId == itemId);
            var itemImages = _mapper.Map<IEnumerable<ItemImage>>(itemImageModels);
            return itemImages;
        }

        public async Task<PaginatedList<ItemImage>> GetAllItemImages(SieveModel sieveModel)
        {
            _itemImageRepository.SieveModel = sieveModel;
            var imagesCount = await _itemImageRepository.GetCountAsync(i => true);
            var itemImageModels = await _itemImageRepository.GetAllAsync();
            var itemImages = _mapper.Map<IEnumerable<ItemImage>>(itemImageModels);
            return _paginationService.GetPaginatedList(itemImages, imagesCount, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<ItemImageFile> ConfirmImageUpload(int itemId, int imageId)
        {
            var itemModel = await _itemRepository.GetByIdAsync(itemId);
            if (itemModel == null)
            {
                throw new ValidationException($"Item with Id({itemId}) does not exist");
            }
            var itemImageModel = await _itemImageFilesRepository.GetAsync(p => p.ItemId == itemId && p.Id == imageId);
            if (itemImageModel == null)
            {
                throw new ValidationException($"Image with Id({imageId}) does not exist for Item with Id({itemId})");
            }
            itemImageModel.IsUploaded = true;
            await _itemImageFilesRepository.UpdateAsync(itemImageModel);
            return _mapper.Map<ItemImageFile>(itemImageModel);
        }

        public async Task<IEnumerable<Item>> GetItemsByIds(IEnumerable<int> ids)
        {
            var itemModels = await _itemRepository.GetManyAsync(x => ids.Contains(x.Id));
            return _mapper.Map<IEnumerable<Item>>(itemModels);
        }

        public async Task<IEnumerable<Item>> GetItemsByNetSuiteItemIds(IEnumerable<int> netSuiteItemIds)
        {
            var itemModels = await _itemRepository.GetManyAsync(x => x.NetSuiteItemId.HasValue && netSuiteItemIds.Contains(x.NetSuiteItemId.Value));
            return _mapper.Map<IEnumerable<Item>>(itemModels);
        }

        public async Task<Item> UpdateEquipmentSettingConfig(int itemId, IEnumerable<EquipmentSettingConfig> equipmentSettingsRequest)
        {
            var itemModel = await _itemRepository.GetByIdAsync(itemId);
            if (itemModel == null)
            {
                throw new ValidationException($"Item with Id({itemId}) does not exist");
            }
            itemModel.EquipmentSettingsConfig = _mapper.Map<ICollection<EquipmentSettingsConfig>>(equipmentSettingsRequest);
            foreach (var equipmentConfig in itemModel.EquipmentSettingsConfig)
            {
                if (equipmentConfig.EquipmentSettingTypeId != 0)
                {
                    equipmentConfig.EquipmentSettingType = null;
                }
            }
            await _itemRepository.UpdateAsync(itemModel);
            return _mapper.Map<Item>(itemModel);
        }

        public async Task<IEnumerable<EquipmentSettingType>> GetEquipmentSettingTypes(SieveModel sieveModel)
        {
            _equipmentSettingTypeRepository.SieveModel = sieveModel;
            var equipmentSettingTypeModels = await _equipmentSettingTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<EquipmentSettingType>>(equipmentSettingTypeModels);
        }

        public async Task<Item> UpdateAddOnsConfig(int itemId, IEnumerable<AddOnsConfigRequest> addOnsConfigRequests)
        {
            var itemModel = await _itemRepository.GetByIdAsync(itemId);
            if (itemModel == null)
            {
                throw new ValidationException($"Item with Id({itemId}) does not exist");
            }
            var addOnsGroupValidator = new AddOnsGroupValidator();
            foreach (var addOnsConfigRequest in addOnsConfigRequests)
            {
                var validationResult = addOnsGroupValidator.Validate(addOnsConfigRequest);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors[0].ErrorMessage);
                }
            }
            var itemIds = addOnsConfigRequests.SelectMany(i => i.ProductIds).Distinct();
            var validItems = await _itemRepository.GetManyAsync(i => itemIds.Contains(i.Id));
            var invalidItems = itemIds.Except(validItems.Select(i => i.Id));

            if(invalidItems != null && invalidItems.Any())
            {
                throw new ValidationException($"Item with Ids({string.Join(",", invalidItems)}) does not exist");
            }

            if(itemIds.Contains(itemId))
            {
                throw new ValidationException($"Item with Id({itemId}) cannot be added in it's own addon group.");
            }

            itemModel.AddOnGroups = _mapper.Map<ICollection<AddOnGroups>>(addOnsConfigRequests);
            await _itemRepository.UpdateAsync(itemModel);
            var item = await _itemRepository.GetByIdAsync(itemId);
            return _mapper.Map<Item>(item);
        }

        private async Task ValidateCategory(int categoryId)
        {
            var categoryExists = await _itemCategoryRepository.ExistsAsync(c => c.Id == categoryId);
            if (!categoryExists)
            {
                throw new ValidationException("Item Category is not Valid");
            }
        }

        private async Task ValidateLocation(int locationId, string locationLabel)
        {
            var siteModel = await _sitesRepository.GetByIdAsync(locationId);
            if (siteModel == null)
            {
                throw new ValidationException($"{locationLabel} site is not valid");
            }
        }

        private async Task<ViewModels.NetSuite.ItemResponse> CreateItem(ViewModels.NetSuite.NSItemRequest itemRequest)
        {
            var itemNumberExists = await _itemRepository.ExistsAsync(s => s.ItemNumber == itemRequest.ItemNumber);
            if (itemNumberExists)
            {
                throw new ValidationException($"Item with Item Number ({itemRequest.ItemNumber}) already exists");
            }

            var itemModel = _mapper.Map<Items>(itemRequest);

            await AddCategoriesToItemModel(itemModel, itemRequest);

            await ValidateInventories(itemModel, itemRequest);

            foreach (var inventoryModel in itemModel.Inventory)
            {
                inventoryModel.StatusId = (int)Data.Enums.InventoryStatusTypes.Available;
            }

            await _itemRepository.AddAsync(itemModel);

            return _mapper.Map<ViewModels.NetSuite.ItemResponse>(itemModel);
        }

        private async Task<ViewModels.NetSuite.ItemResponse> UpdateItem(Data.Models.Items existingItem, ViewModels.NetSuite.NSItemRequest itemRequest)
        {
            var itemNumberExists = await _itemRepository.ExistsAsync(s => s.ItemNumber == itemRequest.ItemNumber
                                                                      && s.NetSuiteItemId != itemRequest.NetSuiteItemId);
            if (itemNumberExists)
            {
                throw new ValidationException($"Item with Item Number ({itemRequest.ItemNumber}) already exists");
            }
            var itemModel = _mapper.Map<ViewModels.NetSuite.NSItemRequest, Items>(itemRequest, existingItem);

            await AddCategoriesToItemModel(itemModel, itemRequest);

            await ValidateInventories(itemModel, itemRequest);

            foreach (var inventoryModel in itemModel.Inventory)
            {
                if (inventoryModel.StatusId == 0)
                {
                    inventoryModel.StatusId = (int)Data.Enums.InventoryStatusTypes.Available;
                }
            }

            await _itemRepository.UpdateAsync(itemModel);

            return _mapper.Map<ViewModels.NetSuite.ItemResponse>(itemModel);
        }

        private async Task AddCategoriesToItemModel(Items itemModel, ViewModels.NetSuite.NSItemRequest itemRequest)
        {
            var itemCategoryMapping = new List<ItemCategoryMapping>();
            var itemSubCategoryMapping = new List<ItemSubCategoryMapping>();
            foreach (var category in itemRequest.Categories)
            {
                var itemCategoryModel = await _itemCategoryRepository.GetAsync(ic => ic.NetSuiteCategoryId == category.NetSuiteCategoryId);
                var itemCategory = _mapper.Map(category, itemCategoryModel);

                foreach (var subCategory in category.Categories)
                {
                    var itemSubCategoryModel = await _itemSubCategoryRepository.GetAsync(isc => isc.NetSuiteSubCategoryId == subCategory.NetSuiteSubCategoryId &&
                                                                                       isc.Category.NetSuiteCategoryId == category.NetSuiteCategoryId);
                    var itemSubCategory = _mapper.Map(subCategory, itemSubCategoryModel);
                    itemSubCategory.Category = itemCategory;
                    itemSubCategoryMapping.Add(new ItemSubCategoryMapping() { ItemSubCategory = itemSubCategory });
                }
                itemCategoryMapping.Add(new ItemCategoryMapping() { ItemCategory = itemCategory });
            }
            itemModel.ItemCategoryMapping = itemCategoryMapping;
            itemModel.ItemSubCategoryMapping = itemSubCategoryMapping;
        }

        private async Task ValidateInventories(Items itemModel, ViewModels.NetSuite.NSItemRequest itemRequest)
        {
            var inventoryModels = new List<Data.Models.Inventory>();
            if (itemRequest.Inventory != null)
            {
                var isStandAloneInventory = !itemRequest.IsAssetTagged && !itemRequest.IsSerialized && !itemRequest.IsLotNumbered;
                if (isStandAloneInventory)
                {
                    var duplicateStandAloneInventory = itemRequest.Inventory.GroupBy(i => i.NetSuiteLocationId).Where(ig => ig.Count() > 1);
                    if (duplicateStandAloneInventory.Any())
                    {
                        throw new ValidationException("standalone inventory cannot have multiple inventories at same location");
                    }
                }

                var inventoryByduplicateNSIds = itemRequest.Inventory.Where(i => i.NetSuiteInventoryId.HasValue).GroupBy(i => i.NetSuiteInventoryId).Where(it => it.Count() > 1);
                if (inventoryByduplicateNSIds.Count() > 0)
                {
                    throw new ValidationException($"Duplicate Internal Inventory Ids ({string.Join(",", inventoryByduplicateNSIds.Select(i => i.Key))}) not allowed");
                }

                var existingInventoryModels = await _inventoryRepository.GetManyAsync(i => i.Item.NetSuiteItemId == itemRequest.NetSuiteItemId);

                foreach (var inventoryRequest in itemRequest.Inventory)
                {
                    var site = new Data.Models.Sites();
                    if (inventoryRequest.NetSuiteLocationId.HasValue)
                    {
                        site = await _inventoryService.GetSiteByNetSuiteLocationId(inventoryRequest.NetSuiteLocationId.Value);
                    }
                    var existingInventoryModel = existingInventoryModels.FirstOrDefault(i => (inventoryRequest.NetSuiteInventoryId.HasValue && i.NetSuiteInventoryId == inventoryRequest.NetSuiteInventoryId)
                                                                                           || (!inventoryRequest.NetSuiteInventoryId.HasValue
                                                                                                && i.ItemId == itemModel.Id && i.CurrentLocationId == site.Id
                                                                                                && (!i.Item.IsLotNumbered || i.LotNumber == inventoryRequest.LotNumber)));
                    if (existingInventoryModel != null)
                    {
                        if (existingInventoryModel.Item.NetSuiteItemId != itemRequest.NetSuiteItemId)
                        {
                            throw new ValidationException($"internal inventoryId ({inventoryRequest.NetSuiteInventoryId}) is already assigned to an inventory of another item");
                        }
                    }
                    var inventoryModel = _mapper.Map(inventoryRequest, existingInventoryModel);
                    if (inventoryModel.IsDeleted)
                    {
                        inventoryModel.IsDeleted = false;
                    }
                    /* 02-Dec-2020 : HACK : disabling for data load from NetSuite to HMS-Digital
                     if (!string.IsNullOrWhiteSpace(inventoryRequest.SerialNumber))
                     {
                         var inventoryWithSameSerialNumber = await _inventoryRepository.GetAsync(i => i.NetSuiteInventoryId != inventoryRequest.NetSuiteInventoryId &&
                                                                                             i.SerialNumber == inventoryRequest.SerialNumber);
                         if (inventoryWithSameSerialNumber != null)
                         {
                             throw new ValidationException($"Serial Number ({inventoryRequest.SerialNumber}) is already assigned to  another inventory");
                         }
                     } */

                    inventoryModel.CurrentLocationId = site.Id;

                    inventoryModel.ItemId = itemModel.Id;

                    inventoryModels.Add(inventoryModel);
                }
                if (itemRequest.IsConsumable)
                {
                    var patientSite = await _sitesRepository.GetAsync(s => s.NetSuiteLocationId == _netSuiteConfig.PatientWarehouseId);
                    if (patientSite != null)
                    {
                        var patientInventoryExists = inventoryModels.Any(i => !i.NetSuiteInventoryId.HasValue
                                                                              && i.ItemId == itemModel.Id
                                                                              && i.CurrentLocationId == patientSite.Id);

                        if (!patientInventoryExists)
                        {
                            var patientInventory = await _inventoryRepository.GetAsync(i => !i.NetSuiteInventoryId.HasValue
                                                                                            && i.ItemId == itemModel.Id
                                                                                            && i.CurrentLocationId == patientSite.Id);
                            if (patientInventory != null)
                            {
                                inventoryModels.Add(patientInventory);
                            }
                        }
                    }

                }
                itemModel.Inventory = inventoryModels;
            }
            if (itemModel.AvgDeliveryProcessingTime == null || itemModel.AvgDeliveryProcessingTime <= 0)
            {
                itemModel.AvgDeliveryProcessingTime = 15;
            }
            if (itemModel.AvgPickUpProcessingTime == null || itemModel.AvgPickUpProcessingTime <= 0)
            {
                itemModel.AvgPickUpProcessingTime = 15;
            }
        }
    }
}
