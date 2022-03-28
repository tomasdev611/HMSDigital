using AutoMapper;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using HMSDigital.Patient.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HospiceSource.Digital.NetSuite.SDK.Config;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using HospiceSource.Digital.NetSuite.SDK.ViewModels;
using NotificationSDK.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using PatientSDK = HospiceSource.Digital.Patient.SDK.Interfaces;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class FulfillmentService : IFulfillmentService
    {
        private readonly IDriverRepository _driverRepository;

        private readonly IOrderHeadersRepository _orderHeadersRepository;

        private readonly IInventoryRepository _inventoryRepository;

        private readonly IPatientInventoryRepository _patientInventoryRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly IOrderFulfillmentLineItemsRepository _orderFulfillmentLineItemsRepository;

        private readonly IItemRepository _itemRepository;

        private readonly IHospiceRepository _hospiceRepository;

        private readonly IHospiceLocationRepository _hospiceLocationRepository;

        private readonly HttpContext _httpContext;

        private readonly IMapper _mapper;

        private readonly INetSuiteService _netSuiteService;

        private readonly IInventoryService _inventoryService;

        private readonly INotificationService _notificationService;

        private readonly PatientSDK.IPatientService _patientService;

        private readonly NetSuiteConfig _netSuiteConfig;

        private readonly ILogger<FulfillmentService> _logger;

        private readonly IDbContextFactoryService _dbContextFactoryService;

        public FulfillmentService(IMapper mapper,
            IDriverRepository driverRepository,
            IOrderHeadersRepository orderHeadersRepository,
            IInventoryRepository inventoryRepository,
            IPatientInventoryRepository patientInventoryRepository,
            ISitesRepository sitesRepository,
            IOrderFulfillmentLineItemsRepository orderFulfillmentLineItemsRepository,
            IItemRepository itemRepository,
            IHospiceRepository hospiceRepository,
            IHospiceLocationRepository hospiceLocationRepository,
            INetSuiteService netSuiteService,
            IInventoryService inventoryService,
            INotificationService notificationService,
            PatientSDK.IPatientService patientService,
            IOptions<NetSuiteConfig> netSuiteOptions,
            IHttpContextAccessor httpContextAccessor,
            IDbContextFactoryService dbContextFactoryService,
            ILogger<FulfillmentService> logger)
        {
            _mapper = mapper;
            _driverRepository = driverRepository;
            _orderHeadersRepository = orderHeadersRepository;
            _inventoryRepository = inventoryRepository;
            _patientInventoryRepository = patientInventoryRepository;
            _sitesRepository = sitesRepository;
            _orderFulfillmentLineItemsRepository = orderFulfillmentLineItemsRepository;
            _itemRepository = itemRepository;
            _hospiceRepository = hospiceRepository;
            _hospiceLocationRepository = hospiceLocationRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _netSuiteService = netSuiteService;
            _inventoryService = inventoryService;
            _notificationService = notificationService;
            _patientService = patientService;
            _netSuiteConfig = netSuiteOptions.Value;
            _logger = logger;
            _dbContextFactoryService = dbContextFactoryService;
        }

        public async Task FulfillOrder(OrderFulfillmentRequest fulfillmentRequest)
        {
            Data.Models.Drivers driverModel;

            var orderModel = await _orderHeadersRepository.GetByIdAsync(fulfillmentRequest.OrderId);
            if (orderModel == null)
            {
                throw new ValidationException($"Order with Id ({fulfillmentRequest.OrderId}) is not valid");
            }

            var originalOrderStatusId = orderModel.StatusId;
            if (orderModel.StatusId == (int)OrderHeaderStatusTypes.Pending_Approval
                || orderModel.StatusId == (int)OrderHeaderStatusTypes.Completed
                || orderModel.StatusId == (int)OrderHeaderStatusTypes.Cancelled)
            {
                throw new ValidationException($"order with Id ({orderModel.Id}) have invalid status for fulfillment");
            }

            if (fulfillmentRequest.DriverId == null)
            {
                driverModel = await _driverRepository.GetAsync(d => d.UserId == GetLoggedInUserId());
                if (driverModel == null)
                {
                    throw new ValidationException($"Driver which fulfilled the order should be specified or logged in user should be driver");
                }
                fulfillmentRequest.DriverId = driverModel.Id;
                fulfillmentRequest.VehicleId = driverModel.CurrentVehicle?.Id;
            }
            else
            {
                driverModel = await _driverRepository.GetByIdAsync(fulfillmentRequest.DriverId.Value);
                if (driverModel == null)
                {
                    throw new ValidationException($"DriverId ({fulfillmentRequest.DriverId}) is not valid");
                }
                if (fulfillmentRequest.VehicleId == null)
                {
                    fulfillmentRequest.VehicleId = driverModel.CurrentVehicle?.Id;
                }
            }
            await ValidateOrderSite(orderModel);

            if (fulfillmentRequest.FulfillmentItems != null && fulfillmentRequest.FulfillmentItems.Any())
            {
                await FulfillOrderWithFulfillmentItems(fulfillmentRequest, orderModel, driverModel);
            }
            else if (!(fulfillmentRequest.IsExceptionFulfillment && orderModel.OrderTypeId == (int)OrderTypes.Pickup))
            {
                throw new ValidationException("Fulfillment Items are required");
            }

            await UpdateExceptionFulfillmentStatus(fulfillmentRequest, orderModel);

            await _orderHeadersRepository.UpdateAsync(orderModel);

            if (orderModel.StatusId != originalOrderStatusId)
            {
                var isDMEEquipmentLeft = await _patientInventoryRepository.ExistsAsync(pi => pi.PatientUuid == orderModel.PatientUuid &&
                                                                                             pi.Item.IsDme && !pi.IsExceptionFulfillment);

                var hasOpenOrders = await _orderHeadersRepository.ExistsAsync(x => x.PatientUuid == orderModel.PatientUuid &&
                                                                                    x.StatusId != (int)OrderHeaderStatusTypes.Completed
                                                                                    && x.StatusId != (int)OrderHeaderStatusTypes.Cancelled
                                                                                    && !(x.IsExceptionFulfillment && x.StatusId == (int)OrderHeaderStatusTypes.Partial_Fulfillment));

                _patientService.RecordOrderFulfillment(orderModel.PatientUuid, orderModel.Status?.Name, orderModel.PickupReason, isDMEEquipmentLeft, hasOpenOrders);
            }
        }

        public async Task FulfillOrderWithFulfillmentItems(OrderFulfillmentRequest fulfillmentRequest, Data.Models.OrderHeaders orderModel, Data.Models.Drivers driverModel)
        {
            if (fulfillmentRequest.FulfillmentItems.Any(fi => fi == null))
            {
                throw new ValidationException($"Fulfillment Item can not be null");
            }

            ValidateOrderMovement(fulfillmentRequest);

            var fulfilledItemsToPickup = fulfillmentRequest.FulfillmentItems.Where(d => d.FulfillmentType.ToLower() == "pickup");
            var fulfilledItemsToDrop = fulfillmentRequest.FulfillmentItems.Where(d => d.FulfillmentType.ToLower() == "delivery");
            var patientSiteId = await GetPatientSiteId();
            var pickupFulfilledInventories = await GetFulfilledInventory(orderModel.OrderLineItems, fulfilledItemsToPickup, patientSiteId, patientSiteId);
            var dropFulfilledInventories = await GetFulfilledInventory(orderModel.OrderLineItems, fulfilledItemsToDrop, fulfillmentRequest.VehicleId, orderModel.SiteId);

            var notReadyInventories = dropFulfilledInventories.Where(i => i.Inventory.StatusId == (int)InventoryStatusTypes.NotReady);
            if (notReadyInventories.Count() > 0)
            {
                var notReadyItemNames = string.Join(", ", notReadyInventories.Select(i => i.Inventory.Item.Name));
                var notReadySerialNumbers = string.Join(", ", notReadyInventories.Select(i => i.Inventory.SerialNumber));
                throw new ValidationException($"inventories for items ({notReadyItemNames}) with serial Numbers ({notReadySerialNumbers}) are not ready for delivery");
            }

            var fulfilledLineItems = await _orderFulfillmentLineItemsRepository.GetManyAsync(l => l.OrderHeaderId == orderModel.Id);
            var pendingLineItems = orderModel.OrderLineItems.Where(l => l.StatusId != (int)OrderLineItemStatusTypes.Completed);
            var pendingLineItemIds = pendingLineItems.Select(l => l.Id).ToList();

            ValidateOrderFulfilledItems(orderModel, pickupFulfilledInventories, dropFulfilledInventories, fulfilledLineItems);

            var patientInventoryModels = await _patientInventoryRepository.GetManyAsync(pi => pi.PatientUuid == orderModel.PatientUuid);

            if (pickupFulfilledInventories.Any())
            {
                ValidatePickupPatientInventory(pickupFulfilledInventories, patientInventoryModels);

                await UpdatePickupForFulfilledInventory(pickupFulfilledInventories, fulfillmentRequest.VehicleId.Value, orderModel.NetSuiteOrderId.HasValue, orderModel.OrderTypeId);
                await PickupPatientInventory(pickupFulfilledInventories, patientInventoryModels);
            }
            if (dropFulfilledInventories.Any())
            {
                await UpdateDropForFulfilledInventory(dropFulfilledInventories, patientSiteId, fulfillmentRequest.VehicleId.Value, orderModel.NetSuiteOrderId.HasValue);
                await DropPatientInventory(dropFulfilledInventories, orderModel);
            }

            foreach (var lineItem in pendingLineItems)
            {
                var fulfilledItemCount = 0;
                if (lineItem.ActionId == (int)OrderTypes.Pickup)
                {
                    fulfilledItemCount = pickupFulfilledInventories.Where(di => di.OrderLineItemId == lineItem.Id).Sum(di => di.Count);

                }
                else if (lineItem.ActionId == (int)OrderTypes.Delivery)
                {
                    fulfilledItemCount = dropFulfilledInventories.Where(di => di.OrderLineItemId == lineItem.Id).Sum(di => di.Count);
                }
                if (fulfilledItemCount == 0)
                {
                    continue;
                }
                var fulfilledQuantity = fulfilledLineItems.Where(l => l.OrderLineItemId == lineItem.Id).Sum(l => l.Quantity) ?? 0;

                if ((fulfilledQuantity + fulfilledItemCount) >= lineItem.ItemCount)
                {
                    lineItem.StatusId = (int)OrderLineItemStatusTypes.Completed;
                    lineItem.DispatchStatusId = (int)OrderLineItemStatusTypes.Completed;
                    fulfilledItemCount -= fulfilledQuantity + lineItem.ItemCount;
                }
                else if (fulfilledItemCount > 0 && (fulfilledQuantity + fulfilledItemCount) < lineItem.ItemCount)
                {
                    lineItem.StatusId = (int)OrderLineItemStatusTypes.Partial_Fulfillment;
                }
            }

            if (orderModel.OrderLineItems.All(l => l.StatusId == (int)OrderLineItemStatusTypes.Completed))
            {
                orderModel.StatusId = (int)OrderHeaderStatusTypes.Completed;
                orderModel.DispatchStatusId = (int)OrderHeaderStatusTypes.Completed;
                orderModel.FulfillmentStartDateTime = fulfillmentRequest.FulfillmentStartDateTime;
                orderModel.FulfillmentStartAtLatitude = fulfillmentRequest.FulfillmentStartAtLatitude;
                orderModel.FulfillmentStartAtLongitude = fulfillmentRequest.FulfillmentStartAtLongitude;
                orderModel.FulfillmentEndDateTime = fulfillmentRequest.FulfillmentEndDateTime;
                orderModel.FulfillmentEndAtLatitude = fulfillmentRequest.FulfillmentEndAtLatitude;
                orderModel.FulfillmentEndAtLongitude = fulfillmentRequest.FulfillmentEndAtLongitude;
            }
            else if (orderModel.OrderLineItems.Any(l => l.StatusId == (int)OrderLineItemStatusTypes.Completed || l.StatusId == (int)OrderLineItemStatusTypes.Partial_Fulfillment))
            {
                orderModel.StatusId = (int)OrderHeaderStatusTypes.Partial_Fulfillment;
                orderModel.PartialFulfillmentReason = fulfillmentRequest.PartialFulfillmentReason;

                var driverName = $"{driverModel.User.FirstName} {driverModel.User.LastName}";
                _notificationService.SendPartialOrderFulfillmentNotification(orderModel.OrderNumber, fulfillmentRequest.PartialFulfillmentReason, driverName, driverModel.CurrentVehicle?.Cvn);
            }

            var lineItemFulfilments = await SaveLineItemFulfillment(pickupFulfilledInventories, dropFulfilledInventories, orderModel, fulfillmentRequest);

            // calling asynchronously to avoid effect of netsuite failure on api response
            ConfirmOrderFulfillment(fulfillmentRequest.OrderId, lineItemFulfilments, !orderModel.NetSuiteOrderId.HasValue);
        }

        public async Task UpdateOrderStatus(OrderStatusUpdateRequest statusUpdateRequest)
        {
            if (!statusUpdateRequest.StatusId.HasValue && !statusUpdateRequest.DispatchStatusId.HasValue)
            {
                throw new ValidationException($"StatusId/DispatchStatusId is required");
            }

            if (statusUpdateRequest.OrderIds == null || statusUpdateRequest.OrderIds.Count() == 0)
            {
                throw new ValidationException("Order Ids can not be empty");
            }

            var validOrderStatusMapping = new Dictionary<int, List<int>>
            {
                { (int)OrderHeaderStatusTypes.Scheduled, new List<int>() { (int)OrderHeaderStatusTypes.OutForFulfillment, (int)OrderHeaderStatusTypes.Enroute, (int)OrderHeaderStatusTypes.OnSite}},
                { (int)OrderHeaderStatusTypes.OnTruck, new List<int>() { (int)OrderHeaderStatusTypes.OutForFulfillment} },
                { (int)OrderHeaderStatusTypes.Partial_Fulfillment, new List<int>() { (int)OrderHeaderStatusTypes.Enroute} },
                { (int)OrderHeaderStatusTypes.OutForFulfillment, new List<int>() { (int)OrderHeaderStatusTypes.Enroute} },
                { (int)OrderHeaderStatusTypes.Enroute, new List<int>() { (int)OrderHeaderStatusTypes.OnSite} }
            };

            var validDispatchStatusMapping = new Dictionary<int, List<int>>
            {
                { (int)OrderHeaderStatusTypes.Scheduled, new List<int>() { (int)OrderHeaderStatusTypes.PreLoad, (int)OrderHeaderStatusTypes.OutForFulfillment, (int)OrderHeaderStatusTypes.Enroute, (int)OrderHeaderStatusTypes.OnSite, (int)OrderHeaderStatusTypes.Loading_Truck } },
                { (int)OrderHeaderStatusTypes.PreLoad, new List<int>() { (int)OrderHeaderStatusTypes.Loading_Truck} },
                { (int)OrderHeaderStatusTypes.OnTruck, new List<int>() { (int)OrderHeaderStatusTypes.OutForFulfillment} },
                { (int)OrderHeaderStatusTypes.OutForFulfillment, new List<int>() { (int)OrderHeaderStatusTypes.Enroute} },
                { (int)OrderHeaderStatusTypes.Enroute, new List<int>() { (int)OrderHeaderStatusTypes.OnSite} },
                { (int)OrderHeaderStatusTypes.OnSite, new List<int>() { (int)OrderHeaderStatusTypes.Enroute} },
            };

            var orderModels = await _orderHeadersRepository.GetManyAsync(o => statusUpdateRequest.OrderIds.Contains(o.Id));
            var invalidOrderIds = statusUpdateRequest.OrderIds.Except(orderModels.Select(o => o.Id));
            if (invalidOrderIds.Count() > 0)
            {
                throw new ValidationException($"Order with Ids ({string.Join(", ", invalidOrderIds)}) is not valid");
            }

            foreach (var orderModel in orderModels)
            {
                if (statusUpdateRequest.StatusId.HasValue && statusUpdateRequest.StatusId.Value != orderModel.StatusId)
                {
                    ValidateOrderStatusUpdate(validOrderStatusMapping, orderModel.StatusId, statusUpdateRequest.StatusId.Value, "Order");
                    orderModel.StatusId = statusUpdateRequest.StatusId.Value;
                    foreach (var lineItem in orderModel.OrderLineItems)
                    {
                        if (lineItem.StatusId != (int)OrderLineItemStatusTypes.Completed)
                        {
                            lineItem.StatusId = statusUpdateRequest.StatusId.Value;
                        }
                    }
                }

                if (statusUpdateRequest.DispatchStatusId.HasValue && statusUpdateRequest.DispatchStatusId != orderModel.DispatchStatusId)
                {
                    ValidateOrderStatusUpdate(validDispatchStatusMapping, orderModel.DispatchStatusId ?? 0, statusUpdateRequest.DispatchStatusId.Value, "Dispatch");
                    orderModel.DispatchStatusId = statusUpdateRequest.DispatchStatusId.Value;
                    foreach (var lineItem in orderModel.OrderLineItems)
                    {
                        if (lineItem.DispatchStatusId != (int)OrderLineItemStatusTypes.Completed)
                        {
                            lineItem.DispatchStatusId = statusUpdateRequest.DispatchStatusId.Value;
                        }
                    }
                }
            }

            await _orderHeadersRepository.UpdateManyAsync(orderModels);
        }

        private void ValidateOrderStatusUpdate(Dictionary<int, List<int>> validStatusMapping, int currentStatusId, int newStatusId, string statusType)
        {
            if (!Enum.IsDefined(typeof(OrderHeaderStatusTypes), newStatusId))
            {
                throw new ValidationException($"{statusType} StatusId ({newStatusId}) is not valid");
            }

            if (!validStatusMapping.TryGetValue(currentStatusId, out List<int> allowedUpdatedStatusIds))
            {
                throw new ValidationException($"Current {statusType} Status ({(OrderHeaderStatusTypes)currentStatusId}) cannot be modified");
            }
            if (!allowedUpdatedStatusIds.Contains(newStatusId))
            {
                throw new ValidationException($"Current {statusType} Status ({(OrderHeaderStatusTypes)currentStatusId}) cannot be modified to ({(OrderHeaderStatusTypes)newStatusId})");
            }
        }

        public async Task<bool> ConfirmOrderFulfillment(int orderHeaderId, IEnumerable<Data.Models.OrderFulfillmentLineItems> lineItemFullfilments, bool dispatchOnly = false)
        {
            var fulfilmentRequest = new ConfirmFulfilmentRequest()
            {
                NetSuiteCustomerId = lineItemFullfilments.FirstOrDefault().NetSuiteCustomerId.Value,
                PatientUuid = lineItemFullfilments.FirstOrDefault().PatientUuid.ToString(),
                DispatchOnly = dispatchOnly,
                Items = _mapper.Map<IEnumerable<FulfilmentItem>>(lineItemFullfilments)
            };
            if (lineItemFullfilments.FirstOrDefault().NetSuiteOrderId.HasValue)
            {
                fulfilmentRequest.NetSuiteTransactionId = lineItemFullfilments.FirstOrDefault().NetSuiteOrderId.Value;
            }
            var confirmationErrorMessage = $@"Failed to  confirm fulfilment for NetSuiteOrderId ({fulfilmentRequest.NetSuiteTransactionId}) 
                                             and NetSuiteCustomerId({fulfilmentRequest.NetSuiteCustomerId}) and PatientUUID ({fulfilmentRequest.PatientUuid})";

            var invalidLineItemFulfillments = lineItemFullfilments.Where(l => l.OrderHeaderId != orderHeaderId);
            if (invalidLineItemFulfillments.Count() > 0)
            {
                var invalidOrderHeaderIds = string.Join(", ", invalidLineItemFulfillments.Select(l => l.OrderHeaderId));
                var invalidOrderLineItemIds = string.Join(", ", invalidLineItemFulfillments.Select(l => l.OrderLineItemId));
                _logger.LogError($@"Order Header Id should be ({orderHeaderId}) for all lineItems. Order Header Ids ({invalidOrderHeaderIds}) 
                                    with Order Line Items ({invalidOrderLineItemIds}) are not valid for this request");
                _logger.LogError(confirmationErrorMessage);
            }

            try
            {
                var confirmFulfilmentResponse = await _netSuiteService.ConfirmOrderFulfilment(fulfilmentRequest);

                if (confirmFulfilmentResponse == null || !confirmFulfilmentResponse.Success)
                {
                    _logger.LogError(confirmationErrorMessage);
                    return false;
                }
                var dbContext = await _dbContextFactoryService.GetDBContext();
                lineItemFullfilments = dbContext.OrderFulfillmentLineItems.Where(o => o.OrderHeaderId == orderHeaderId);
                foreach (var dispatchRecord in confirmFulfilmentResponse.DispatchRecords)
                {
                    var fulfilmentsForItem = lineItemFullfilments.Where(l => l.NetSuiteItemId == dispatchRecord.NetSuiteItemId);
                    foreach (var lineItemFulfilment in fulfilmentsForItem)
                    {
                        lineItemFulfilment.NetSuiteDispatchRecordId = dispatchRecord.DispatchRecordId;
                        lineItemFulfilment.IsFulfilmentConfirmed = true;
                    }
                }
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(confirmationErrorMessage);
                return false;
            }
            return true;
        }

        private async Task<int> GetPatientSiteId()
        {
            var patientSite = await _sitesRepository.GetAsync(s => s.NetSuiteLocationId == _netSuiteConfig.PatientWarehouseId);
            if (patientSite == null)
            {
                throw new ValidationException("Patient Warehouse is not configured");
            }
            return patientSite.Id;
        }

        private void ValidateOrderMovement(OrderFulfillmentRequest fulfillmentRequest)
        {
            var fulfillmentValidator = new FulfillmentValidator();
            var validationResult = fulfillmentValidator.Validate(fulfillmentRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            if (fulfillmentRequest.FulfillmentItems.Any(di => string.IsNullOrEmpty(di.FulfillmentType)
                                                            || (di.FulfillmentType.ToLower() != "pickup" && di.FulfillmentType.ToLower() != "delivery")))
            {
                throw new ValidationException($"Fulfillment type should be specified(pickup or delivery) for each fulfilled Item");
            }
        }

        private void ValidateOrderFulfilledItems(Data.Models.OrderHeaders orderModel, IEnumerable<FulfilledInventory> pickupFulfilledInventoryModels, IEnumerable<FulfilledInventory> dropFulfilledInventoryModels, IEnumerable<Data.Models.OrderFulfillmentLineItems> fulfilledLineItems)
        {
            var lineItems = orderModel.OrderLineItems.Where(l => l.StatusId != (int)OrderLineItemStatusTypes.Completed);

            var pickupLineItems = lineItems.Where(l => l.ActionId == (int)OrderTypes.Pickup);
            var pickupItemCountMap = GetItemCountMap(pickupLineItems, fulfilledLineItems);
            ValidateFulfilledItems(pickupFulfilledInventoryModels, pickupItemCountMap, $"pickup items for Order #{orderModel.OrderNumber}");

            var dropLineItems = lineItems.Where(l => l.ActionId == (int)OrderTypes.Delivery);
            var dropItemCountMap = GetItemCountMap(dropLineItems, fulfilledLineItems);
            ValidateFulfilledItems(dropFulfilledInventoryModels, dropItemCountMap, $"delivery items for Order #{orderModel.OrderNumber}");
        }

        private Dictionary<int, int> GetItemCountMap(IEnumerable<Data.Models.OrderLineItems> lineItems, IEnumerable<Data.Models.OrderFulfillmentLineItems> fulfilledLineItems)
        {
            var itemCountMap = new Dictionary<int, int>();
            foreach (var lineItem in lineItems)
            {
                var fulfilledItemCount = fulfilledLineItems.Where(l => l.OrderLineItemId == lineItem.Id).Sum(l => l.Quantity.Value);
                itemCountMap.TryGetValue(lineItem.ItemId.Value, out int existingItemCount);
                itemCountMap[lineItem.ItemId.Value] = existingItemCount + (lineItem.ItemCount - fulfilledItemCount);
            }
            return itemCountMap;
        }

        private void ValidateFulfilledItems(IEnumerable<FulfilledInventory> fulfilledInventories, Dictionary<int, int> itemCountMap, string errorMessageLabel)
        {
            var itemIds = itemCountMap.Select(i => i.Key);

            // validate if any additional item is being processed

            var additionalFulfilledInventory = fulfilledInventories.Where(d => !itemIds.Contains(d.Inventory.ItemId))
                                                                        .Select(d => d.Inventory);
            if (additionalFulfilledInventory.Count() > 0)
            {
                var itemNames = string.Join(", ", additionalFulfilledInventory.Select(d => d.Item.Name));
                var serialNumbers = string.Join(", ", additionalFulfilledInventory.Where(d => !string.IsNullOrEmpty(d.SerialNumber)).Select(d => d.SerialNumber));
                throw new ValidationException($"Items {itemNames} with Serial Numbers {serialNumbers} is not part of {errorMessageLabel}");

            }

            //validate if count of fulfilled items is same as ordered
            var fulfilledInventoryByItemIds = fulfilledInventories.GroupBy(dk => dk.Inventory.ItemId);

            foreach (var fulfilledInventory in fulfilledInventoryByItemIds)
            {
                if (itemCountMap.TryGetValue(fulfilledInventory.Key, out var itemCount) && itemCount < fulfilledInventory.Sum(di => di.Count))
                {
                    var itemWithInvalidCount = fulfilledInventories.FirstOrDefault(di => di.Inventory.ItemId == fulfilledInventory.Key).Inventory.Item;
                    throw new ValidationException($"only {itemCount} quantity of {itemWithInvalidCount.Name} is allowed for {errorMessageLabel}, but found {fulfilledInventory.Sum(di => di.Count)}");
                }
            }

        }

        private async Task UpdatePickupForFulfilledInventory(IEnumerable<FulfilledInventory> fulfilledInventories, int vehicleId, bool isNetSuiteOrder, int orderTypeId)
        {
            var fulfilledInventoryIds = fulfilledInventories.Select(di => di.Inventory.Id);
            var fulfilledInventoryModels = await _inventoryRepository.GetManyAsync(i => fulfilledInventoryIds.Contains(i.Id));
            foreach (var inventoryModel in fulfilledInventoryModels)
            {
                var isStandAloneInventory = !inventoryModel.Item.IsSerialized && !inventoryModel.Item.IsAssetTagged && !inventoryModel.Item.IsLotNumbered;
                if (isStandAloneInventory)
                {
                    var fulfilledInventory = fulfilledInventories.FirstOrDefault(di => di.Inventory.Id == inventoryModel.Id);
                    var vehicleInventoryModel = await GetStandaloneInventory(inventoryModel.ItemId, vehicleId);

                    inventoryModel.QuantityAvailable -= fulfilledInventory.Count;
                    vehicleInventoryModel.QuantityAvailable += fulfilledInventory.Count;
                    await _inventoryRepository.UpdateAsync(vehicleInventoryModel);

                }
                else if (inventoryModel.Item.IsLotNumbered)
                {
                    var fulfilledInventory = fulfilledInventories.FirstOrDefault(di => di.Inventory.Id == inventoryModel.Id);
                    var vehicleInventoryModel = await GetLotNumberedInventory(inventoryModel.ItemId, vehicleId, inventoryModel.LotNumber);

                    inventoryModel.QuantityAvailable -= fulfilledInventory.Count;
                    vehicleInventoryModel.QuantityAvailable += fulfilledInventory.Count;
                    await _inventoryRepository.UpdateAsync(vehicleInventoryModel);

                }
                else
                {
                    if (orderTypeId == (int)OrderTypes.Patient_Move)
                    {
                        inventoryModel.StatusId = (int)InventoryStatusTypes.Available;
                    }
                    inventoryModel.CurrentLocationId = vehicleId;
                }
            }
            await _inventoryRepository.UpdateManyAsync(fulfilledInventoryModels);

            if (!isNetSuiteOrder)
            {
                var vehicleModel = await _sitesRepository.GetByIdAsync(vehicleId);
                MoveInventoryInNetSuite(fulfilledInventories, _netSuiteConfig.PatientWarehouseId, vehicleModel.NetSuiteLocationId.Value);
            }
        }

        private async Task UpdateDropForFulfilledInventory(IEnumerable<FulfilledInventory> fulfilledInventories, int patientSiteId, int vehicleId, bool isNetSuiteOrder)
        {
            var fulfilledInventoryIds = fulfilledInventories.Select(di => di.Inventory.Id);
            var fulfilledInventoryModels = await _inventoryRepository.GetManyAsync(i => fulfilledInventoryIds.Contains(i.Id));
            foreach (var inventoryModel in fulfilledInventoryModels)
            {
                var isStandAloneInventory = !inventoryModel.Item.IsSerialized && !inventoryModel.Item.IsAssetTagged && !inventoryModel.Item.IsLotNumbered;
                if (isStandAloneInventory)
                {
                    var fulfilledInventory = fulfilledInventories.FirstOrDefault(di => di.Inventory.Id == inventoryModel.Id);
                    inventoryModel.QuantityAvailable -= fulfilledInventory.Count;

                    var locationInventoryModel = await GetStandaloneInventory(inventoryModel.ItemId, patientSiteId);
                    locationInventoryModel.QuantityAvailable += fulfilledInventory.Count;
                    await _inventoryRepository.UpdateAsync(locationInventoryModel);
                }
                else if (inventoryModel.Item.IsLotNumbered)
                {
                    var fulfilledInventory = fulfilledInventories.FirstOrDefault(di => di.Inventory.Id == inventoryModel.Id);
                    inventoryModel.QuantityAvailable -= fulfilledInventory.Count;

                    var locationInventoryModel = await GetLotNumberedInventory(inventoryModel.ItemId, patientSiteId, inventoryModel.LotNumber);
                    locationInventoryModel.QuantityAvailable += fulfilledInventory.Count;
                    await _inventoryRepository.UpdateAsync(locationInventoryModel);
                }
                else
                {
                    inventoryModel.StatusId = (int)InventoryStatusTypes.NotReady;

                    inventoryModel.CurrentLocationId = patientSiteId;
                }
            }
            await _inventoryRepository.UpdateManyAsync(fulfilledInventoryModels);
            if (!isNetSuiteOrder)
            {
                var vehicleModel = await _sitesRepository.GetByIdAsync(vehicleId);
                MoveInventoryInNetSuite(fulfilledInventories, vehicleModel.NetSuiteLocationId.Value, _netSuiteConfig.PatientWarehouseId);
            }
        }

        private async Task<Data.Models.Inventory> GetStandaloneInventory(int itemId, int locationId)
        {
            var inventoryModel = await _inventoryRepository.GetAsync(i => i.ItemId == itemId && i.CurrentLocationId == locationId);
            if (inventoryModel == null)
            {
                inventoryModel = new Data.Models.Inventory()
                {
                    ItemId = itemId,
                    CurrentLocationId = locationId,
                    StatusId = (int)InventoryStatusTypes.Available,
                };
                await _inventoryRepository.AddAsync(inventoryModel);
            }
            return inventoryModel;
        }

        private async Task<Data.Models.Inventory> GetLotNumberedInventory(int itemId, int locationId, string lotNumber)
        {
            var inventoryModel = await _inventoryRepository.GetAsync(i => i.ItemId == itemId && i.CurrentLocationId == locationId
                                                                    && i.LotNumber.ToLower() == lotNumber.ToLower());
            if (inventoryModel == null)
            {
                inventoryModel = new Data.Models.Inventory()
                {
                    ItemId = itemId,
                    CurrentLocationId = locationId,
                    StatusId = (int)InventoryStatusTypes.Available,
                    LotNumber = lotNumber
                };
                await _inventoryRepository.AddAsync(inventoryModel);
            }
            return inventoryModel;
        }

        private async Task<IEnumerable<FulfilledInventory>> GetFulfilledInventory(IEnumerable<Data.Models.OrderLineItems> orderLineItems, IEnumerable<FulfillmentItem> fulfilledItems, int? sourceLocationId, int? fallbackLocationId)
        {
            var fulfilledSerialNumbers = fulfilledItems.Where(d => !string.IsNullOrEmpty(d.SerialNumber)).Select(d => d.SerialNumber);
            var fulfilledAssetTagNumbers = fulfilledItems.Where(d => !string.IsNullOrEmpty(d.AssetTagNumber)).Select(d => d.AssetTagNumber);
            var fulfilledLotNumbers = fulfilledItems.Where(d => !string.IsNullOrEmpty(d.LotNumber)).Select(d => d.LotNumber);
            var fulfilledItemIds = fulfilledItems.Where(d => d.ItemId.HasValue).Select(d => d.ItemId);

            var duplicateSerialNumbers = fulfilledSerialNumbers.GroupBy(ds => ds).Where(ds => ds.Count() > 1);
            if (duplicateSerialNumbers.Count() > 0)
            {
                throw new ValidationException($"Duplicate Serial numbers({string.Join(", ", duplicateSerialNumbers.Select(di => di.Key))}) are not allowed");
            }

            var duplicateAssetTagNumbers = fulfilledAssetTagNumbers.GroupBy(dt => dt).Where(dt => dt.Count() > 1);
            if (duplicateAssetTagNumbers.Count() > 0)
            {
                throw new ValidationException($"Duplicate Asset tag numbers({string.Join(", ", duplicateAssetTagNumbers.Select(di => di.Key))}) are not allowed");
            }

            var fulfilledItemsWithoutLineItemIds = fulfilledItems.Where(di => di.OrderLineItemId == 0);
            if (fulfilledItemsWithoutLineItemIds.Count() > 0)
            {
                throw new ValidationException($"Order LineItem Id is required for fulfillment");
            }

            var trackedInventoryModels = await _inventoryRepository.GetManyAsync(i => fulfilledSerialNumbers.Contains(i.SerialNumber)
                                                                                    || fulfilledAssetTagNumbers.Contains(i.AssetTagNumber));

            var standaloneInventoryModels = await _inventoryRepository.GetManyAsync(i => fulfilledItemIds.Contains(i.ItemId)
                                                                                        && (i.CurrentLocationId == sourceLocationId || i.CurrentLocationId == fallbackLocationId));

            var lotNumberedInventoryModels = await _inventoryRepository.GetManyAsync(i => fulfilledItemIds.Contains(i.ItemId)
                                                                                       && fulfilledLotNumbers.Contains(i.LotNumber)
                                                                                       && (i.CurrentLocationId == sourceLocationId || i.CurrentLocationId == fallbackLocationId));

            var standaloneItemModels = await _itemRepository.GetManyAsync(i => fulfilledItemIds.Contains(i.Id));

            var sourceLocationName = await _inventoryService.GetLocationName(sourceLocationId, "Source location");
            var fallbackLocationName = await _inventoryService.GetLocationName(fallbackLocationId, "Fallback location");

            var inventoryModels = trackedInventoryModels.Concat(standaloneInventoryModels).Concat(lotNumberedInventoryModels);

            return fulfilledItems.Select(di =>
            {
                Data.Models.Inventory inventoryModel;
                if (!string.IsNullOrEmpty(di.SerialNumber))
                {
                    inventoryModel = inventoryModels.FirstOrDefault(i => i.SerialNumber.ToLower() == di.SerialNumber.ToLower());
                    if (inventoryModel == null)
                    {
                        throw new ValidationException($"inventory with serial Number ({di.SerialNumber}) is not valid");
                    }
                }
                else if (!string.IsNullOrEmpty(di.AssetTagNumber))
                {
                    inventoryModel = inventoryModels.FirstOrDefault(i => i.AssetTagNumber.ToLower() == di.AssetTagNumber.ToLower());
                    if (inventoryModel == null)
                    {
                        throw new ValidationException($"inventory with Asset Tag Number ({di.AssetTagNumber}) is not valid");
                    }
                }
                else if (!string.IsNullOrEmpty(di.LotNumber))
                {
                    inventoryModel = inventoryModels.FirstOrDefault(i => i.LotNumber.ToLower() == di.LotNumber.ToLower());
                    if (inventoryModel == null)
                    {
                        throw new ValidationException($"inventory with Lot Number ({di.LotNumber}) is not valid");
                    }
                }
                else
                {
                    inventoryModel = inventoryModels.FirstOrDefault(i => i.ItemId == di.ItemId && i.CurrentLocationId == sourceLocationId);
                    if (inventoryModel == null)
                    {
                        inventoryModel = inventoryModels.FirstOrDefault(i => i.ItemId == di.ItemId && i.CurrentLocationId == fallbackLocationId);
                        if (inventoryModel == null)
                        {
                            var itemModel = standaloneItemModels.FirstOrDefault(i => i.Id == di.ItemId);
                            if (itemModel == null)
                            {
                                throw new ValidationException($"ItemId ({di.ItemId}) is not valid");
                            }

                            var errorMessage = $"No inventory available for {itemModel.Name}({itemModel.ItemNumber})";
                            if (string.Equals(sourceLocationName, fallbackLocationName))
                            {
                                errorMessage += $" at {sourceLocationName}";
                            }
                            else
                            {
                                errorMessage += $" on {sourceLocationName} or at {fallbackLocationName}";
                            }
                            throw new ValidationException(errorMessage);
                        }
                    }
                    var isStandAloneInventory = !inventoryModel.Item.IsSerialized && !inventoryModel.Item.IsAssetTagged && !inventoryModel.Item.IsLotNumbered;
                    if (!isStandAloneInventory)
                    {
                        string assetTagNumber = !string.IsNullOrWhiteSpace(inventoryModel.AssetTagNumber) ?
                                                    $" AssetTagNumber ({inventoryModel.AssetTagNumber})" : string.Empty;

                        string serialNumber = !string.IsNullOrWhiteSpace(inventoryModel.SerialNumber) ?
                                                    $" SerialNumber ({inventoryModel.SerialNumber})" : string.Empty;

                        string lotNumber = !string.IsNullOrWhiteSpace(inventoryModel.LotNumber) ?
                                                    $" LotNumber ({inventoryModel.LotNumber})" : string.Empty;

                        _logger.LogError($@"serialNumber/AssetTagNumber/LotNumber is not provided for not stand alone 
                                                tracked inventory ({inventoryModel.Id}){assetTagNumber}{serialNumber}{lotNumber}");

                        throw new ValidationException($"serialNumber/AssetTagNumber/LotNumber should be provided for tracked inventory");
                    }
                }

                if (inventoryModel.Item.IsSerialized && di.Count != 1)
                {
                    throw new ValidationException($"Serialized item ({inventoryModel.Item.Name}) can not have count more than 1");
                }

                var lineItem = orderLineItems.FirstOrDefault(l => l.Id == di.OrderLineItemId);
                if (lineItem.ItemId != inventoryModel.ItemId)
                {
                    throw new ValidationException($"Line item with item {lineItem.Item?.Name} can not be fulfilled with {inventoryModel.Item?.Name}");
                }

                if (!string.IsNullOrEmpty(lineItem.AssetTagNumber) && !string.Equals(lineItem.AssetTagNumber, inventoryModel.AssetTagNumber))
                {
                    throw new ValidationException($"Line item with expected Asset Tag {lineItem.AssetTagNumber} can not be fulfilled with {inventoryModel.AssetTagNumber}");
                }

                if (!string.IsNullOrEmpty(lineItem.SerialNumber) && !string.Equals(lineItem.SerialNumber, inventoryModel.SerialNumber))
                {
                    throw new ValidationException($"Line item with expected Serial Number {lineItem.SerialNumber} can not be fulfilled with {inventoryModel.SerialNumber}");
                }

                if (!string.IsNullOrEmpty(lineItem.LotNumber) && !string.Equals(lineItem.LotNumber, inventoryModel.LotNumber))
                {
                    throw new ValidationException($"Line item with expected Lot Number {lineItem.LotNumber} can not be fulfilled with {inventoryModel.LotNumber}");
                }

                return new FulfilledInventory()
                {
                    Inventory = _mapper.Map<Inventory>(inventoryModel),
                    Count = di.Count,
                    IsSerialized = inventoryModel.Item.IsSerialized,
                    OrderLineItemId = di.OrderLineItemId
                };

            });
        }

        private async Task<IEnumerable<Inventory>> GetUnFulfilledPickupInventory(IEnumerable<Data.Models.OrderLineItems> orderLineItems)
        {
            var unfulfilledLineItems = orderLineItems.Where(l => l.StatusId != (int)OrderLineItemStatusTypes.Completed && (l.Item.IsSerialized || l.Item.IsAssetTagged));
            var unfulfilledSerialNumbers = unfulfilledLineItems.Where(l => !string.IsNullOrEmpty(l.SerialNumber)).Select(l => l.SerialNumber);
            var unfulfilledAssetTagNumbers = unfulfilledLineItems.Where(l => !string.IsNullOrEmpty(l.AssetTagNumber)).Select(l => l.AssetTagNumber);

            var inventoryModels = await _inventoryRepository.GetManyAsync(i => unfulfilledSerialNumbers.Contains(i.SerialNumber)
                                                                                    || unfulfilledAssetTagNumbers.Contains(i.AssetTagNumber));

            return unfulfilledLineItems.Select(di =>
            {
                Data.Models.Inventory inventoryModel;
                if (!string.IsNullOrEmpty(di.AssetTagNumber))
                {
                    inventoryModel = inventoryModels.FirstOrDefault(i => i.AssetTagNumber.ToLower() == di.AssetTagNumber.ToLower());
                }
                else
                {
                    inventoryModel = inventoryModels.FirstOrDefault(i => i.SerialNumber.ToLower() == di.SerialNumber.ToLower());
                }
                return _mapper.Map<Inventory>(inventoryModel);
            });
        }

        private void ValidatePickupPatientInventory(IEnumerable<FulfilledInventory> fulfilledInventories, IEnumerable<Data.Models.PatientInventory> patientInventoryModels)
        {
            foreach (var fulfilledInventory in fulfilledInventories)
            {
                var item = fulfilledInventory.Inventory.Item;
                var isStandAloneInventory = !item.IsSerialized && !item.IsAssetTagged;
                if (!isStandAloneInventory)
                {
                    var patientInventories = patientInventoryModels.Where(pi => pi.InventoryId == fulfilledInventory.Inventory.Id);
                    if (patientInventories.Count() == 0)
                    {
                        throw new ValidationException($"picked up inventory ({item.Name}) with Id({fulfilledInventory.Inventory.Id}) is not available with patient to pickup");
                    }
                }

            }
        }

        private async Task PickupPatientInventory(IEnumerable<FulfilledInventory> fulfilledInventories, IEnumerable<Data.Models.PatientInventory> patientInventoryModels)
        {
            foreach (var fulfilledInventory in fulfilledInventories)
            {
                if (fulfilledInventory.IsSerialized)
                {
                    var patientInventories = patientInventoryModels.Where(pi => pi.InventoryId == fulfilledInventory.Inventory.Id);
                    var serializedPatientInventory = patientInventories.FirstOrDefault();
                    serializedPatientInventory.ItemCount = 0;
                }
                else
                {
                    var patientInventories = patientInventoryModels.Where(pi => pi.ItemId == fulfilledInventory.Inventory.ItemId);
                    var count = fulfilledInventory.Count;
                    foreach (var patientInventoryModel in patientInventories)
                    {
                        if (count > patientInventoryModel.ItemCount)
                        {
                            count -= patientInventoryModel.ItemCount;
                            patientInventoryModel.ItemCount = 0;
                        }
                        else
                        {
                            patientInventoryModel.ItemCount -= count;
                            count = 0;
                            break;
                        }
                    }
                }
            }

            var pickedUpPatientInventoryIds = patientInventoryModels.Where(pi => pi.ItemCount == 0).Select(pi => pi.Id);
            await _patientInventoryRepository.DeleteAsync(pi => pickedUpPatientInventoryIds.Contains(pi.Id));

            var updatedPatientInventory = patientInventoryModels.Where(pi => !pickedUpPatientInventoryIds.Contains(pi.Id));

            await _patientInventoryRepository.UpdateManyAsync(updatedPatientInventory);
            var patientUuid = patientInventoryModels.Select(p => p.PatientUuid).FirstOrDefault();
            var isDMEItemAvailable = await _patientInventoryRepository.ExistsAsync(pi => pi.PatientUuid == patientUuid && pi.Item.IsDme && !pi.IsExceptionFulfillment);
            if (!isDMEItemAvailable)
            {
                await _patientInventoryRepository.DeleteAsync(pi => pi.PatientUuid == patientUuid && pi.Item != null && pi.Item.IsConsumable);
            }
        }

        private async Task DropPatientInventory(IEnumerable<FulfilledInventory> fulfilledInventories, Data.Models.OrderHeaders orderModel)
        {
            var newPatientInventories = new List<Data.Models.PatientInventory>();

            newPatientInventories.AddRange(
                fulfilledInventories.Select(di =>
                new Data.Models.PatientInventory()
                {
                    ItemCount = di.Count,
                    ItemId = di.Inventory.ItemId,
                    OrderHeaderId = orderModel.Id,
                    HospiceId = orderModel.HospiceId,
                    HospiceLocationId = orderModel.HospiceLocationId,
                    PatientUuid = (Guid)orderModel.PatientUuid,
                    StatusId = (int)InventoryStatusTypes.NotReady,
                    OrderLineItem = orderModel.OrderLineItems.FirstOrDefault(l => l.Id == di.OrderLineItemId),
                    DeliveryAddressUuid = orderModel.DeliveryAddress.AddressUuid,
                    InventoryId = di.Inventory.Id
                })
            );

            await _patientInventoryRepository.AddManyAsync(newPatientInventories);
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

        private async Task<IEnumerable<Data.Models.OrderFulfillmentLineItems>> SaveLineItemFulfillment(IEnumerable<FulfilledInventory> pickupInventories,
                                                                          IEnumerable<FulfilledInventory> dropInventories,
                                                                          Data.Models.OrderHeaders orderModel,
                                                                          OrderFulfillmentRequest orderFulfillmentRequest)
        {
            var lineItemFulfillments = new List<Data.Models.OrderFulfillmentLineItems>();

            foreach (var pickupInventory in pickupInventories)
            {
                var orderLineItems = orderModel.OrderLineItems.Where(l => l.Id == pickupInventory.OrderLineItemId);
                foreach (var orderLineItem in orderLineItems)
                {
                    var lineItemFulfillment = _mapper.Map<Data.Models.OrderFulfillmentLineItems>(pickupInventory);

                    lineItemFulfillment.OrderType = "pickup";
                    lineItemFulfillment.DeliveredStatus = "Delivered";
                    lineItemFulfillment.OrderLineItem = orderLineItem;
                    lineItemFulfillment.ItemId = pickupInventory.Inventory.ItemId;

                    lineItemFulfillments.Add(lineItemFulfillment);
                }
            }

            foreach (var dropInventory in dropInventories)
            {
                var orderLineItems = orderModel.OrderLineItems.Where(l => l.Id == dropInventory.OrderLineItemId);

                var lineItemFulfillment = _mapper.Map<Data.Models.OrderFulfillmentLineItems>(dropInventory);
                foreach (var orderLineItem in orderLineItems)
                {
                    lineItemFulfillment.OrderType = "delivery";
                    lineItemFulfillment.DeliveredStatus = "Delivered";
                    lineItemFulfillment.OrderLineItem = orderLineItem;
                    lineItemFulfillment.ItemId = dropInventory.Inventory.ItemId;

                    lineItemFulfillments.Add(lineItemFulfillment);
                }
            }

            var fulfilledByVehicle = await _sitesRepository.GetAsync(v => v.Id == orderFulfillmentRequest.VehicleId && v.LocationType.ToLower() == "vehicle");

            foreach (var lineItemFulfillment in lineItemFulfillments)
            {
                lineItemFulfillment.FulfillmentStartAtLatitude = orderFulfillmentRequest.FulfillmentStartAtLatitude;
                lineItemFulfillment.FulfillmentStartAtLongitude = orderFulfillmentRequest.FulfillmentStartAtLongitude;
                lineItemFulfillment.FulfillmentStartDateTime = orderFulfillmentRequest.FulfillmentStartDateTime;
                lineItemFulfillment.FulfillmentEndAtLatitude = orderFulfillmentRequest.FulfillmentEndAtLatitude;
                lineItemFulfillment.FulfillmentEndAtLongitude = orderFulfillmentRequest.FulfillmentEndAtLongitude;
                lineItemFulfillment.FulfillmentEndDateTime = orderFulfillmentRequest.FulfillmentEndDateTime;

                lineItemFulfillment.NetSuiteWarehouseId = fulfilledByVehicle?.NetSuiteLocationId;
                lineItemFulfillment.NetSuiteCustomerId = orderModel.NetSuiteCustomerId;
                lineItemFulfillment.PatientUuid = orderModel.PatientUuid;
                lineItemFulfillment.NetSuiteOrderId = orderModel.NetSuiteOrderId;
                lineItemFulfillment.OrderHeaderId = orderModel.Id;
                lineItemFulfillment.FulfilledByDriverId = orderFulfillmentRequest.DriverId;
                lineItemFulfillment.FulfilledByVehicleId = orderFulfillmentRequest.VehicleId;
                lineItemFulfillment.IsWebportalFulfillment = orderFulfillmentRequest.IsWebportalFulfillment;
            }

            await _orderFulfillmentLineItemsRepository.AddManyAsync(lineItemFulfillments);

            return lineItemFulfillments;
        }

        private async Task MoveInventoryInNetSuite(IEnumerable<FulfilledInventory> fulfilledInventories, int netSuiteSourceLocationId, int netSuiteDestinationLocationId)
        {
            var movementRequest = new InventoryMovementRequest()
            {
                NetSuiteSourceWarehouseId = netSuiteSourceLocationId,
                NetSuiteDestinationWarehouseId = netSuiteDestinationLocationId,
                Items = fulfilledInventories.Select(i => new MovementItem()
                {
                    NetSuiteItemId = i.Inventory.Item.NetSuiteItemId,
                    AssetTag = i.Inventory.AssetTagNumber,
                    SerialNumber = i.Inventory.SerialNumber,
                    LotNumber = i.Inventory.LotNumber,
                    Quantity = i.Count
                })
            };
            await _netSuiteService.ConfirmInventoryMovement(movementRequest);
        }

        private async Task ValidateOrderSite(Core.Data.Models.OrderHeaders orderModel)
        {
            if (orderModel.SiteId.HasValue)
            {
                return;
            }
            int? serviceZipCode;
            if (orderModel.OrderTypeId == (int)OrderTypes.Delivery
                || orderModel.OrderTypeId == (int)OrderTypes.Exchange
                || orderModel.OrderTypeId == (int)OrderTypes.Respite)
            {
                serviceZipCode = orderModel.DeliveryAddress.ZipCode;
            }
            else
            {
                serviceZipCode = orderModel.PickupAddress.ZipCode;
            }
            var errorMessage = $"Zip Code {serviceZipCode} is not within service area of any site.";
            if (orderModel.HospiceLocationId.HasValue)
            {
                var hospiceLocation = await _hospiceLocationRepository.GetByIdAsync(orderModel.HospiceLocationId.Value);
                errorMessage += $" Also, \"{hospiceLocation?.Name}\" is not assigned to any site";
            }
            else if (orderModel.HospiceId.HasValue)
            {
                var hospice = await _hospiceRepository.GetByIdAsync(orderModel.HospiceId.Value);
                errorMessage += $" Also, \"{hospice?.Name}\" is not assigned to any site";
            }
            throw new ValidationException(errorMessage);
        }

        private async Task UpdateExceptionFulfillmentStatus(Core.ViewModels.OrderFulfillmentRequest fulfillmentRequest, Core.Data.Models.OrderHeaders orderModel)
        {
            if (orderModel.StatusId == (int)OrderHeaderStatusTypes.Completed)
            {
                orderModel.IsExceptionFulfillment = false;
                return;
            }
            else if (orderModel.IsExceptionFulfillment)
            {
                return;
            }
            if (!fulfillmentRequest.IsExceptionFulfillment)
            {
                return;
            }

            orderModel.IsExceptionFulfillment = true;
            var unfulfilledPickupInventory = await GetUnFulfilledPickupInventory(orderModel.OrderLineItems);

            var updatedPatientInventoryModels = await _patientInventoryRepository.GetManyAsync(p => p.PatientUuid == orderModel.PatientUuid);
            foreach (var inventory in unfulfilledPickupInventory)
            {
                if (inventory.Item.IsAssetTagged || inventory.Item.IsSerialized)
                {
                    var patientInventoryModel = updatedPatientInventoryModels.FirstOrDefault(pi => inventory.Id == pi.InventoryId);
                    patientInventoryModel.IsExceptionFulfillment = true;
                }
            }
            await _patientInventoryRepository.UpdateManyAsync(updatedPatientInventoryModels);
        }
    }
}

