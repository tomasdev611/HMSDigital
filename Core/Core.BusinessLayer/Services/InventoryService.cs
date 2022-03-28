using AutoMapper;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Options;
using HospiceSource.Digital.NetSuite.SDK.Config;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using HospiceSource.Digital.NetSuite.SDK.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HMSDigital.Core.ViewModels.NetSuite;
using HMSDigital.Core.Data.Models;
using PatientInventory = HMSDigital.Core.ViewModels.PatientInventory;
using OrderHeaderStatusTypes = HMSDigital.Core.Data.Enums.OrderHeaderStatusTypes;
using OrderTypes = HMSDigital.Core.Data.Enums.OrderTypes;
using OrderLineItemStatusTypes = HMSDigital.Core.Data.Enums.OrderLineItemStatusTypes;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _inventoryRepository;

        private readonly IItemRepository _itemRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly IMapper _mapper;

        private readonly IPaginationService _paginationService;

        private readonly IHospiceRepository _hospiceRepository;

        private readonly IHospiceLocationRepository _hospiceLocationRepository;

        private readonly IPatientInventoryRepository _patientInventoryRepository;

        private readonly IOrderHeadersRepository _orderHeadersRepository;

        private readonly IOrderFulfillmentLineItemsRepository _orderFulfillmentLineItemsRepository;

        private readonly IUsersRepository _usersRepository;

        private readonly IAddressRepository _addressRepository;

        private readonly INetSuiteService _netSuiteService;

        private readonly NetSuiteConfig _netSuiteConfig;

        private readonly HttpContext _httpContext;

        public InventoryService(IInventoryRepository inventoryRepository,
            IItemRepository itemRepository,
            ISitesRepository sitesRepository,
            IPaginationService paginationService,
            IHospiceRepository hospiceRepository,
            IHospiceLocationRepository hospiceLocationRepository,
            IPatientInventoryRepository patientInventoryRepository,
            IOrderHeadersRepository orderHeadersRepository,
            IOrderFulfillmentLineItemsRepository orderFulfillmentLineItemsRepository,
            IUsersRepository usersRepository,
            IAddressRepository addressRepository,
            INetSuiteService netSuiteService,
            IHttpContextAccessor httpContextAccessor,
            IOptions<NetSuiteConfig> netSuiteOptions,
            IMapper mapper)
        {
            _inventoryRepository = inventoryRepository;
            _itemRepository = itemRepository;
            _sitesRepository = sitesRepository;
            _mapper = mapper;
            _paginationService = paginationService;
            _hospiceRepository = hospiceRepository;
            _hospiceLocationRepository = hospiceLocationRepository;
            _patientInventoryRepository = patientInventoryRepository;
            _orderHeadersRepository = orderHeadersRepository;
            _orderFulfillmentLineItemsRepository = orderFulfillmentLineItemsRepository;
            _usersRepository = usersRepository;
            _addressRepository = addressRepository;
            _netSuiteService = netSuiteService;
            _netSuiteConfig = netSuiteOptions.Value;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task<PaginatedList<ViewModels.Inventory>> GetAllInventory(SieveModel sieveModel)
        {
            _inventoryRepository.SieveModel = sieveModel;
            var totalRecords = await _inventoryRepository.GetCountAsync(s => true);
            var inventoryModels = await _inventoryRepository.GetAllAsync();
            var accessibleInventoryModels = await GetAccessibleInventory(inventoryModels);
            var inventory = _mapper.Map<IEnumerable<ViewModels.Inventory>>(accessibleInventoryModels);
            return _paginationService.GetPaginatedList(inventory, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PaginatedList<PatientInventory>> GetPatientInventory(string patientUuid, SieveModel sieveModel, bool includePickupDetails, bool includeDeliveryAddress)
        {
            _patientInventoryRepository.SieveModel = sieveModel;
            var totalRecords = await _patientInventoryRepository.GetCountAsync(p => p.PatientUuid.ToString() == patientUuid);
            var patientInventoryModels = await _patientInventoryRepository.GetManyAsync(p => p.PatientUuid.ToString() == patientUuid);
            var patientInventories = _mapper.Map<IEnumerable<PatientInventory>>(patientInventoryModels);

            if (includeDeliveryAddress)
            {
                var deliveryAddressUUIDs = patientInventories.Select(pi => pi.DeliveryAddressUuid);
                var deliveryAddresses = await _addressRepository.GetManyAsync(a => a.AddressUuid.HasValue && a.AddressUuid != Guid.Empty && deliveryAddressUUIDs.Contains(a.AddressUuid.Value));
                foreach (var patientInventory in patientInventories)
                {
                    var deliveryAddress = deliveryAddresses.FirstOrDefault(a => a.AddressUuid.Value == patientInventory.DeliveryAddressUuid);
                    patientInventory.DeliveryAddress = _mapper.Map<Address>(deliveryAddress);
                }
            }

            if (includePickupDetails)
            {
                var openPickupOrders = await _orderHeadersRepository.GetManyAsync(o => o.PatientUuid.ToString() == patientUuid
                                                                                    && o.StatusId != (int)OrderHeaderStatusTypes.Completed
                                                                                    && o.StatusId != (int)OrderHeaderStatusTypes.Cancelled);
                var openPickupOrderIds = openPickupOrders.Select(o => o.Id);
                var fulfillmentsForOpenPickupOrders = await _orderFulfillmentLineItemsRepository.GetManyAsync(fl => openPickupOrderIds.Contains(fl.OrderHeaderId));

                var pendingPickupLineItems = openPickupOrders.SelectMany(o => o.OrderLineItems)
                                                             .Where(l => l.ActionId == (int)OrderTypes.Pickup
                                                                        && l.StatusId != (int)OrderLineItemStatusTypes.Completed
                                                                        && l.StatusId != (int)OrderLineItemStatusTypes.Cancelled);

                var trackedPatientInventories = patientInventories.Where(pi => !string.IsNullOrEmpty(pi.AssetTagNumber) || !string.IsNullOrEmpty(pi.SerialNumber) || !string.IsNullOrEmpty(pi.LotNumber));
                foreach (var trackedPatientInventory in trackedPatientInventories)
                {
                    var pendingLineItem = pendingPickupLineItems.FirstOrDefault(l => (string.IsNullOrEmpty(trackedPatientInventory.AssetTagNumber) || string.Equals(l.AssetTagNumber, trackedPatientInventory.AssetTagNumber))
                                                                                    && (string.IsNullOrEmpty(trackedPatientInventory.SerialNumber) || string.Equals(l.SerialNumber, trackedPatientInventory.SerialNumber))
                                                                                    && (string.IsNullOrEmpty(trackedPatientInventory.LotNumber) || string.Equals(l.LotNumber, trackedPatientInventory.LotNumber)));
                    if (pendingLineItem != null)
                    {
                        trackedPatientInventory.IsPartOfExistingPickup = true;
                        trackedPatientInventory.ExistingPickupCount = 1;
                    }
                }

                var standalonePatientInventories = patientInventories.Except(trackedPatientInventories);
                var itemwisePatientInventories = standalonePatientInventories.GroupBy(pi => pi.ItemId);
                foreach (var itemwisePatientInventory in itemwisePatientInventories)
                {
                    var pendingLineItems = pendingPickupLineItems.Where(l => itemwisePatientInventory.Key == l.ItemId);
                    if (pendingLineItems.Any())
                    {
                        var pendingItemCount = pendingLineItems.Sum(l => l.ItemCount);
                        var partialFulfilledLineItems = pendingLineItems.Where(l => l.StatusId == (int)OrderLineItemStatusTypes.Partial_Fulfillment);
                        if (partialFulfilledLineItems.Any())
                        {
                            var partialFulfilledLineItemIds = partialFulfilledLineItems.Select(l => l.Id);
                            pendingItemCount -= fulfillmentsForOpenPickupOrders.Where(fl => partialFulfilledLineItemIds.Contains(fl.OrderLineItemId) && fl.Quantity.HasValue).Sum(fl => fl.Quantity.Value);
                        }
                        foreach (var patientInventory in itemwisePatientInventory)
                        {
                            if (pendingItemCount > 0)
                            {
                                patientInventory.IsPartOfExistingPickup = true;
                                patientInventory.ExistingPickupCount = pendingItemCount > patientInventory.Count ? patientInventory.Count : pendingItemCount;
                                pendingItemCount -= patientInventory.ExistingPickupCount;
                            }
                        }
                    }
                }
            }
            return _paginationService.GetPaginatedList(patientInventories, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PatientInventory> AddPatientInventory(Guid patientUuid, PatientInventoryRequest patientInventoryRequest)
        {
            var itemModel = await _itemRepository.GetAsync(i => i.ItemNumber.ToLower() == patientInventoryRequest.ItemNumber.ToLower());
            if (itemModel == null)
            {
                throw new ValidationException($"Item with Item Number({patientInventoryRequest.ItemNumber}) not found");
            }
            var validator = new PatientInventoryValidator(itemModel.IsSerialized, itemModel.IsAssetTagged, itemModel.IsLotNumbered);
            var validationResult = validator.Validate(patientInventoryRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            var hospiceModel = await _hospiceRepository.GetByIdAsync(patientInventoryRequest.HospiceId);
            if (hospiceModel == null)
            {
                throw new ValidationException("Hospice Id does not exist in database");
            }

            var hospiceLocationModel = await _hospiceLocationRepository.GetAsync(hl => hl.Id == patientInventoryRequest.HospiceLocationId
                                                                                    && hl.HospiceId == patientInventoryRequest.HospiceId);

            if (hospiceLocationModel == null)
            {
                throw new ValidationException("Hospice Location Id does not exist in database");
            }

            var patientInventoryModel = new Data.Models.PatientInventory()
            {
                ItemCount = patientInventoryRequest.Quantity,
                PatientUuid = patientUuid,
                StatusId = (int)Data.Enums.InventoryStatusTypes.NotReady,
                ItemId = itemModel.Id,
                HospiceId = patientInventoryRequest.HospiceId,
                HospiceLocationId = patientInventoryRequest.HospiceLocationId,
                DataBridgeRunUuid = patientInventoryRequest.DataBridgeRunUuid,
                DataBridgeRunDateTime = patientInventoryRequest.DataBridgeRunDateTime
            };

            var isTrackedInventory = itemModel.IsSerialized || itemModel.IsAssetTagged || itemModel.IsLotNumbered;
            if (isTrackedInventory)
            {
                Data.Models.Inventory inventoryModel;
                if (itemModel.IsAssetTagged)
                {
                    inventoryModel = await _inventoryRepository.GetAsync(i => i.AssetTagNumber == patientInventoryRequest.AssetTagNumber);
                }
                else if (itemModel.IsSerialized)
                {
                    inventoryModel = await _inventoryRepository.GetAsync(i => i.SerialNumber == patientInventoryRequest.SerialNumber);
                }
                else
                {
                    inventoryModel = await _inventoryRepository.GetAsync(i => i.LotNumber == patientInventoryRequest.LotNumber);
                }
                if (inventoryModel == null)
                {
                    throw new ValidationException($"No inventory item with Asset Tag ({patientInventoryRequest.AssetTagNumber}), Serial Number ({patientInventoryRequest.SerialNumber}) or Lot Number ({patientInventoryRequest.LotNumber}) is present in database");
                }
                patientInventoryModel.InventoryId = inventoryModel.Id;
                if (itemModel.Id != inventoryModel.ItemId)
                {
                    throw new ValidationException($"Provided inventory not associated with item with item number({patientInventoryRequest.ItemNumber}).");
                }

                var patientSite = await _sitesRepository.GetAsync(s => s.NetSuiteLocationId == _netSuiteConfig.PatientWarehouseId);
                MoveInventoryInNetSuite(inventoryModel.CurrentLocationId, patientSite.Id, inventoryModel);
            }
            var existingPatientInventory = await _patientInventoryRepository.GetAsync(p => p.PatientUuid == patientInventoryModel.PatientUuid
                                                                                        && p.ItemId == patientInventoryModel.ItemId
                                                                                        && (!patientInventoryModel.InventoryId.HasValue || p.InventoryId == patientInventoryModel.InventoryId));
            if (existingPatientInventory != null)
            {
                if (!isTrackedInventory)
                {
                    existingPatientInventory.ItemCount += patientInventoryRequest.Quantity;
                }
                await _patientInventoryRepository.UpdateAsync(existingPatientInventory);
                return _mapper.Map<PatientInventory>(existingPatientInventory);
            }

            await _patientInventoryRepository.AddAsync(patientInventoryModel);
            return _mapper.Map<PatientInventory>(patientInventoryModel);
        }

        public async Task<ViewModels.Inventory> GetInventoryById(int inventoryId)
        {
            var invetoryModel = await _inventoryRepository.GetByIdAsync(inventoryId);
            var accessibleInventoryModel = await GetAccessibleInventory(new List<Data.Models.Inventory>() { invetoryModel });
            return _mapper.Map<ViewModels.Inventory>(accessibleInventoryModel.FirstOrDefault());
        }

        public async Task CreateInventory(InventoryRequest inventoryRequest)
        {
            var inventoryValidator = new InventoryValidator();
            var validationResult = inventoryValidator.Validate(inventoryRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            var currentLocation = await GetValidatedLocation(inventoryRequest.CurrentLocationId.Value, "Current");

            var itemModel = await _itemRepository.GetByIdAsync(inventoryRequest.ItemId);
            if (itemModel == null)
            {
                throw new ValidationException($"Invalid Item Id({inventoryRequest.ItemId})");
            }

            await ValidateSerializeInventory(itemModel.IsSerialized, inventoryRequest.SerialNumber, inventoryRequest.QuantityAvailable);

            UpdateInventoryInNetSuite(inventoryRequest, currentLocation.NetSuiteLocationId, itemModel.ItemNumber);

        }

        public async Task<ViewModels.NetSuite.NSInventoryResponse> UpsertInventory(ViewModels.NetSuite.NSInventoryRequest inventoryRequest)
        {

            var itemModel = await _itemRepository.GetAsync(i => i.NetSuiteItemId == inventoryRequest.NetSuiteItemId);
            if (itemModel == null)
            {
                throw new ValidationException($"Invalid Internal Item Id({inventoryRequest.NetSuiteItemId})");
            }
            var inventoryValidator = new NSInventoryValidator(itemModel.IsSerialized, itemModel.IsAssetTagged, itemModel.IsLotNumbered);
            var validationResult = inventoryValidator.Validate(_mapper.Map<ViewModels.NetSuite.NSInventory>(inventoryRequest));
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            var site = new Data.Models.Sites();
            if (inventoryRequest.NetSuiteLocationId.HasValue)
            {
                site = await GetSiteByNetSuiteLocationId(inventoryRequest.NetSuiteLocationId.Value);
            }
            var existingInventoryModel = await _inventoryRepository.GetAsync(i => (inventoryRequest.NetSuiteInventoryId.HasValue
                                                                                 && i.NetSuiteInventoryId == inventoryRequest.NetSuiteInventoryId)
                                                                             || (!inventoryRequest.NetSuiteInventoryId.HasValue
                                                                                 && i.ItemId == itemModel.Id && i.CurrentLocationId == site.Id
                                                                                 && (!i.Item.IsLotNumbered || i.LotNumber == inventoryRequest.LotNumber)));
            if (existingInventoryModel != null)
            {
                if (existingInventoryModel.Item.NetSuiteItemId != inventoryRequest.NetSuiteItemId)
                {
                    throw new ValidationException($"internal inventoryId ({inventoryRequest.NetSuiteInventoryId}) is already assigned to an inventory of another item");
                }
            }
            var inventoryModel = _mapper.Map(inventoryRequest, existingInventoryModel);
            if (inventoryModel.IsDeleted)
            {
                inventoryModel.IsDeleted = false;
            }

            inventoryModel.CurrentLocationId = site.Id;
            inventoryModel.ItemId = itemModel.Id;
            inventoryModel.StatusId = (int)Data.Enums.InventoryStatusTypes.Available;
            if (inventoryModel.Id > 0)
            {
                await _inventoryRepository.UpdateAsync(inventoryModel);
                return _mapper.Map<ViewModels.NetSuite.NSInventoryResponse>(inventoryModel);
            }
            await _inventoryRepository.AddAsync(inventoryModel);
            return _mapper.Map<ViewModels.NetSuite.NSInventoryResponse>(inventoryModel);
        }

        public async Task<ViewModels.Inventory> PatchInventory(int inventoryId, JsonPatchDocument<ViewModels.Inventory> inventoryPatchDocument)
        {
            var inventoryModel = await _inventoryRepository.GetByIdAsync(inventoryId);
            if (inventoryModel == null)
            {
                throw new ValidationException($"Inventory Item with Id ({inventoryId}) not found");
            }
            var allowedPaths = new List<string>
            {
                "/CurrentLocationId",
                "/StatusId",
                "/QuantityAvailable"
            };

            foreach (var op in inventoryPatchDocument.Operations)
            {
                if (!allowedPaths.Any(x => x.Equals(op.path, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new ValidationException($"Attempt to modify data outside of user control. Logged and reported.");
                }
            }

            var existingSiteId = inventoryModel.CurrentLocationId;
            var modelPatch = _mapper.Map<JsonPatchDocument<Data.Models.Inventory>>(inventoryPatchDocument);

            modelPatch.ApplyTo(inventoryModel);

            var inventoryViewModel = _mapper.Map<ViewModels.Inventory>(inventoryModel);
            var inventoryValidator = new InventoryValidator();
            var validationResult = inventoryValidator.Validate(inventoryViewModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }
            if (!inventoryViewModel.CurrentLocationId.HasValue)
            {
                throw new ValidationException($"CurrentLocation can't be empty");
            }
            await ValidateSerializeInventory(inventoryModel.Item.IsSerialized, inventoryModel.SerialNumber, inventoryModel.QuantityAvailable, inventoryModel.Id);
            if (!Enum.IsDefined(typeof(Data.Enums.InventoryStatusTypes), inventoryViewModel.StatusId))
            {
                throw new ValidationException($"Status is not valid");
            }

            var itemModel = await _itemRepository.GetByIdAsync(inventoryModel.ItemId);
            var currentLocation = await GetValidatedLocation(inventoryViewModel.CurrentLocationId.Value, "Current");

            UpdateInventoryInNetSuite(inventoryViewModel, currentLocation.NetSuiteLocationId, itemModel.ItemNumber);

            await _inventoryRepository.UpdateAsync(inventoryModel);

            return _mapper.Map<ViewModels.Inventory>(inventoryModel);
        }

        public async Task DeleteInventory(int inventoryId)
        {
            var inventoryModel = await _inventoryRepository.GetByIdAsync(inventoryId);
            if (inventoryModel == null)
            {
                return;
            }
            await _inventoryRepository.DeleteAsync(inventoryModel);
        }

        public async Task<Data.Models.Sites> GetSiteByNetSuiteLocationId(int netSuiteLocationId)
        {
            var siteModel = await _sitesRepository.GetAsync(s => s.NetSuiteLocationId == netSuiteLocationId);
            if (siteModel == null)
            {
                throw new ValidationException($"warehouse id ({netSuiteLocationId}) is not valid");
            }
            return siteModel;
        }

        public async Task<ViewModels.NetSuite.InventoryResponse> DeleteInventoryByNetSuiteId(ViewModels.NetSuite.InventoryDeleteRequest inventoryDeleteRequest)
        {
            var inventoryModel = await _inventoryRepository.GetAsync(s => s.NetSuiteInventoryId == inventoryDeleteRequest.NetSuiteInventoryId);
            if (inventoryModel != null)
            {
                await _inventoryRepository.DeleteAsync(inventoryModel);
            }
            return _mapper.Map<ViewModels.NetSuite.InventoryResponse>(inventoryDeleteRequest);
        }

        public async Task<PaginatedList<ViewModels.Inventory>> SearchInventoryBySearchQuery(SieveModel sieveModel, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return null;
            }

            _inventoryRepository.SieveModel = sieveModel;

            searchQuery = searchQuery.ToLower();
            var totalRecords = await _inventoryRepository.GetCountAsync(p =>
                p.Item.Name.ToLower().Contains(searchQuery)
                || p.Item.Description.ToLower().Contains(searchQuery)
                || p.Item.ItemNumber.ToLower().Contains(searchQuery)
                || p.Item.CogsAccountName.ToLower().Contains(searchQuery)
                || p.SerialNumber.ToLower().Contains(searchQuery)
                || p.AssetTagNumber.ToLower().Contains(searchQuery)
                || p.LotNumber.ToLower().Contains(searchQuery)
            );
            var inventoryModels = await _inventoryRepository.GetManyAsync(p =>
                p.Item.Name.ToLower().Contains(searchQuery)
                || p.Item.Description.ToLower().Contains(searchQuery)
                || p.Item.ItemNumber.ToLower().Contains(searchQuery)
                || p.Item.CogsAccountName.ToLower().Contains(searchQuery)
                || p.SerialNumber.ToLower().Contains(searchQuery)
                || p.AssetTagNumber.ToLower().Contains(searchQuery)
                || p.LotNumber.ToLower().Contains(searchQuery)
            );

            var accessibleInventoryModels = await GetAccessibleInventory(inventoryModels);
            var inventory = _mapper.Map<IEnumerable<ViewModels.Inventory>>(accessibleInventoryModels);
            return _paginationService.GetPaginatedList(inventory, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<ViewModels.Inventory> MoveInventory(MoveInventoryRequest moveInventoryRequest)
        {
            await GetValidatedLocation(moveInventoryRequest.DestinationLocationId, "Destination");
            Data.Models.Inventory inventory;
            if (!string.IsNullOrEmpty(moveInventoryRequest.AssetTagNumber))
            {
                inventory = await _inventoryRepository.GetAsync(i => i.AssetTagNumber.ToLower() == moveInventoryRequest.AssetTagNumber.ToLower());
            }

            else if (!string.IsNullOrEmpty(moveInventoryRequest.SerialNumber))
            {
                inventory = await _inventoryRepository.GetAsync(i => i.SerialNumber.ToLower() == moveInventoryRequest.SerialNumber.ToLower());
            }

            else
            {
                throw new ValidationException("Asset tag/ serial number is required.");
            }

            if (inventory == null)
            {
                throw new ValidationException("Inventory with provided Asset tag/ serial number is not valid.");
            }

            if (!string.Equals(inventory.Item.ItemNumber, moveInventoryRequest.ItemNumber, StringComparison.OrdinalIgnoreCase))
            {
                throw new ValidationException($"Asset tag/ serial number does not belong to item number ({moveInventoryRequest.ItemNumber})");
            }
            var sourceSiteId = inventory.CurrentLocationId;
            inventory.CurrentLocationId = moveInventoryRequest.DestinationLocationId;
            await _inventoryRepository.UpdateAsync(inventory);
            MoveInventoryInNetSuite(sourceSiteId, moveInventoryRequest.DestinationLocationId, inventory);
            return _mapper.Map<ViewModels.Inventory>(inventory);
        }

        public async Task<PaginatedList<ViewModels.PatientInventory>> SearchPatientInventoryBySearchQuery(string patientUuid, SieveModel sieveModel, string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
            {
                return null;
            }

            _patientInventoryRepository.SieveModel = sieveModel;
            var totalRecords = await _patientInventoryRepository.GetCountAsync(
                pi => pi.PatientUuid.ToString() == patientUuid
                && (pi.Inventory.AssetTagNumber.ToLower().Contains(searchQuery)
                    || pi.Inventory.SerialNumber.ToLower().Contains(searchQuery)));

            var patientInventoryModels = await _patientInventoryRepository.GetManyAsync(
                pi => pi.PatientUuid.ToString() == patientUuid
                && (pi.Inventory.AssetTagNumber.ToLower().Contains(searchQuery)
                    || pi.Inventory.SerialNumber.ToLower().Contains(searchQuery)));

            var patientInventory = _mapper.Map<IEnumerable<ViewModels.PatientInventory>>(patientInventoryModels);
            return _paginationService.GetPaginatedList(patientInventory, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<string> GetLocationName(int? locationId, string locationLabel)
        {
            if (!locationId.HasValue)
            {
                throw new ValidationException($"{locationLabel} location Id can not be null");
            }
            var siteModel = await GetValidatedLocation(locationId.Value, locationLabel);
            return $"{siteModel.Name} warehouse";
        }

        public async Task<NSUpdateInventoryResponse> UpdateNetSuiteInventory(int inventoryId, NSUpdateInventoryRequest updateRequest)
        {
            if (inventoryId != updateRequest.InventoryId) 
            {
                throw new ValidationException($"Inventory Id ({updateRequest.InventoryId}) is not valid.");
            }

            var inventoryModel = await _inventoryRepository.GetByIdAsync(inventoryId);

            if (inventoryModel == null) 
            {
                throw new ValidationException($"Inventory with Id ({inventoryId}) not found.");
            }

            var netSuiteUpdateInventoryRequest = new UpdateInventoryRequest();
            netSuiteUpdateInventoryRequest.NetSuiteInventoryId = inventoryModel.NetSuiteInventoryId.Value;
            netSuiteUpdateInventoryRequest.AssetTag = updateRequest.AssetTag;

            Items itemModel = null;
            if (updateRequest.ItemId.HasValue) 
            {
                itemModel = await _itemRepository.GetByIdAsync(updateRequest.ItemId.Value);
                if (itemModel == null)
                {
                    throw new ValidationException($"Item with Id ({updateRequest.ItemId}) not found.");
                }
                netSuiteUpdateInventoryRequest.NetSuiteItemId = itemModel.NetSuiteItemId;
            }

            var netSuiteInventory = await _netSuiteService.UpdateInventory(netSuiteUpdateInventoryRequest);
            var inventoryResponse = _mapper.Map<NSUpdateInventoryResponse>(netSuiteInventory);
            inventoryResponse.InventoryId = inventoryModel.Id;

            if (itemModel == null) 
            {
                itemModel = await _itemRepository.GetAsync(x => x.NetSuiteItemId == inventoryResponse.NetSuiteItemId);
            }
            inventoryResponse.ItemId = itemModel.Id;

            return inventoryResponse;
        }

        public async Task<IEnumerable<NSAddInventoryResponse>> AddNetSuiteInventory(NSAddInventoryRequest addInventoryRequest)
        {
            var siteModel = await _sitesRepository.GetByIdAsync(addInventoryRequest.SiteId);
            if (siteModel == null) 
            {
                throw new ValidationException($"Site with Id ({addInventoryRequest.SiteId}) not found.");
            }

            if (addInventoryRequest.Items == null || !addInventoryRequest.Items.Any()) 
            {
                return null;
            }

            var netSuiteRequest = new AddNetSuiteInventoryRequest();
            netSuiteRequest.NetSuiteSiteId = siteModel.NetSuiteLocationId.Value;

            var itemIds = addInventoryRequest.Items.Select(x => x.ItemId);
            var itemModels = await _itemRepository.GetManyAsync(x => itemIds.Contains(x.Id));

            var netSuiteRequestLines = new List<NetSuiteInventoryLineRequest>();
            foreach (var requestItem in addInventoryRequest.Items)
            {
                var itemModel = itemModels.FirstOrDefault(x => x.Id == requestItem.ItemId);
                if (itemModel == null)
                {
                    throw new ValidationException($"Item with Id ({requestItem.ItemId}) not found.");
                }
                var netSuiteRequestItem = _mapper.Map<NetSuiteInventoryLineRequest>(requestItem);
                netSuiteRequestItem.NetSuiteItemId = itemModel.NetSuiteItemId.Value;

                netSuiteRequestLines.Add(netSuiteRequestItem);
            }

            netSuiteRequest.Items = netSuiteRequestLines;
            var netSuiteResponse = await _netSuiteService.AddInventory(netSuiteRequest);
            var response =  _mapper.Map<IEnumerable<NSAddInventoryResponse>>(netSuiteResponse);

            foreach (var responseItem in response)
            {
                var itemModel = itemModels.FirstOrDefault(x => x.NetSuiteItemId == responseItem.NetsuiteItemId);
                if (itemModel != null)
                {
                    responseItem.ItemId = itemModel.Id;
                }
            }

            return response;
        }

        private async Task MoveInventoryInNetSuite(int sourceSiteId, int destinationSiteId, Data.Models.Inventory inventory, int count = 1)
        {
            var sourceSiteModel = await GetValidatedLocation(sourceSiteId);
            var destinationSiteModel = await GetValidatedLocation(destinationSiteId);

            var movementRequest = new InventoryMovementRequest()
            {
                NetSuiteSourceWarehouseId = sourceSiteModel.NetSuiteLocationId ?? 0,
                NetSuiteDestinationWarehouseId = destinationSiteModel.NetSuiteLocationId ?? 0,
                Items = new List<MovementItem>()
                {
                    new MovementItem()
                    {
                        NetSuiteItemId = inventory.Item.NetSuiteItemId ?? 0,
                        AssetTag = inventory.AssetTagNumber,
                        LotNumber = inventory.LotNumber,
                        SerialNumber = inventory.SerialNumber,
                        Quantity = count
                    }
                }
            };
            await _netSuiteService.ConfirmInventoryMovement(movementRequest);
        }

        private async Task<Data.Models.Sites> GetValidatedLocation(int locationId, string locationLabel = "")
        {
            var siteModel = await _sitesRepository.GetByIdAsync(locationId);
            if (siteModel == null)
            {
                throw new ValidationException($"{locationLabel} site is not valid");
            }
            return siteModel;
        }

        private async Task<IEnumerable<Data.Models.Inventory>> GetAccessibleInventory(IEnumerable<Data.Models.Inventory> inventoryModels)
        {
            var accessibleSiteIds = await _usersRepository.GetSiteAccessByUserId(GetLoggedInUserId());
            if (accessibleSiteIds.Contains("*"))
            {
                return inventoryModels;
            }
            var accessibleSiteInventory = inventoryModels.Where(i => accessibleSiteIds.Contains(i.CurrentLocationId.ToString()));

            var patientSite = await _sitesRepository.GetAsync(s => s.NetSuiteLocationId == _netSuiteConfig.PatientWarehouseId);
            var inventoryAssignedToPatient = inventoryModels.Where(i => i.CurrentLocationId == patientSite?.Id);

            accessibleSiteInventory = accessibleSiteInventory.Concat(inventoryAssignedToPatient);

            var accessibleSites = await _sitesRepository.GetManyAsync(s => accessibleSiteIds.Contains(s.Id.ToString()));
            var siteNetSuiteIds = accessibleSites.Where(s => s.NetSuiteLocationId.HasValue).Select(s => s.NetSuiteLocationId.Value);
            _httpContext.Items.Add(Claims.IGNORE_GLOBAL_FILTER, GlobalFilters.Site);
            var accessibleVehicles = await _sitesRepository.GetManyAsync(v => v.ParentNetSuiteLocationId.HasValue
                                                                            && siteNetSuiteIds.Contains(v.ParentNetSuiteLocationId.Value));
            var accessibleVehicleIds = accessibleVehicles.Select(v => v.Id);

            var accessibleVehicleInventory = inventoryModels.Where(i => accessibleVehicleIds.Contains(i.CurrentLocationId));

            _httpContext.Items.Remove(Claims.IGNORE_GLOBAL_FILTER);
            return accessibleSiteInventory.Concat(accessibleVehicleInventory);
        }

        private int GetLoggedInUserId()
        {
            var userIdClaim = _httpContext.User.FindFirst(Claims.USER_ID_CLAIM);
            if (userIdClaim == null)
            {
                return -1;
            }
            return int.Parse(userIdClaim.Value);
        }

        private async Task ValidateSerializeInventory(bool isSerialized, string serialNumber, int count, int InventoryId = 0)
        {
            if (!isSerialized)
            {
                if (count <= 0)
                {
                    throw new ValidationException($"Item quantity should be greater than zero.");
                }
                return;
            }

            if (string.IsNullOrEmpty(serialNumber))
            {
                throw new ValidationException($"Item is Serialized. serial number can not be empty or null.");
            }
            var serialNumberExists = await _inventoryRepository.ExistsAsync(s => s.SerialNumber == serialNumber
                                                                  && s.Id != InventoryId);
            if (serialNumberExists)
            {
                throw new ValidationException($"Inventory with Serial Number ({serialNumber}) already exists");
            }
            if (count != 1)
            {
                throw new ValidationException($"Serialized item quantity should be equal to 1.");
            }
        }

        private async Task UpdateInventoryInNetSuite(InventoryRequest inventory, int? netSuiteLocationId, string itemNumber)
        {
            var physicalInventoryRequest = new AdjustInventoryRequest()
            {
                AssetTagNumber = inventory.AssetTagNumber,
                ItemNumber = itemNumber,
                TotalQuantityOnHand = inventory.Count,
                SerialOrLotNumber = inventory.SerialNumber ?? inventory.LotNumber,
                NetSuiteWarehouseId = netSuiteLocationId
            };

            await _netSuiteService.AdjustInventory(physicalInventoryRequest);
        }
    }
}
