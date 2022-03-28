using AutoMapper;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HMSDigital.Common.BusinessLayer.Constants;
using Microsoft.AspNetCore.Http;
using HMSDigital.Core.Data.Enums;
using System.Collections.Generic;
using HMSDigital.Core.BusinessLayer.Validations;
using System;
using Sieve.Models;
using Microsoft.Extensions.Logging;
using HMSDigital.Common.ViewModels;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using NSSDKViewModels = HospiceSource.Digital.NetSuite.SDK.ViewModels;
using NotificationSDK.Interfaces;
using Newtonsoft.Json;
using Audit.EntityFramework;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class DispatchService : IDispatchService
    {
        private readonly IDriverRepository _driverRepository;

        private readonly IItemTransferRequestRepository _itemTransferRequestRepository;

        private readonly IDispatchInstructionsRepository _dispatchInstructionsRepository;

        private readonly IOrderHeadersRepository _orderHeadersRepository;

        private readonly IInventoryRepository _inventoryRepository;

        private readonly IItemRepository _itemRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly ISiteMemberRepository _siteMemberRepository;

        private readonly IOrderFulfillmentLineItemsRepository _orderFulfillmentLineItemsRepository;

        private readonly HttpContext _httpContext;

        private readonly IMapper _mapper;

        private readonly IUserAccessService _userAccessService;

        private readonly INetSuiteService _netSuiteService;

        private readonly IInventoryService _inventoryService;

        private readonly IPaginationService _paginationService;

        private readonly IVehiclesService _vehiclesService;

        private readonly INotificationService _notificationService;

        private readonly ILogger<DispatchService> _logger;

        private readonly IDispatchAuditLogRepository _dispatchAuditLogRepository;

        public DispatchService(IMapper mapper,
            IDriverRepository driverRepository,
            IItemTransferRequestRepository itemTransferRequestRepository,
            IDispatchInstructionsRepository dispatchInstructionsRepository,
            IOrderHeadersRepository orderHeadersRepository,
            IInventoryRepository inventoryRepository,
            IItemRepository itemRepository,
            ISitesRepository sitesRepository,
            ISiteMemberRepository siteMemberRepository,
            IOrderFulfillmentLineItemsRepository orderFulfillmentLineItemsRepository,
            IUserAccessService userAccessService,
            INetSuiteService netSuiteService,
            IInventoryService inventoryService,
            IHttpContextAccessor httpContextAccessor,
            IPaginationService paginationService,
            IVehiclesService vehiclesService,
            INotificationService notificationService,
            IDispatchAuditLogRepository dispatchAuditLogRepository,
            ILogger<DispatchService> logger)
        {
            _mapper = mapper;
            _driverRepository = driverRepository;
            _itemTransferRequestRepository = itemTransferRequestRepository;
            _dispatchInstructionsRepository = dispatchInstructionsRepository;
            _orderHeadersRepository = orderHeadersRepository;
            _inventoryRepository = inventoryRepository;
            _itemRepository = itemRepository;
            _sitesRepository = sitesRepository;
            _siteMemberRepository = siteMemberRepository;
            _orderFulfillmentLineItemsRepository = orderFulfillmentLineItemsRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _userAccessService = userAccessService;
            _netSuiteService = netSuiteService;
            _inventoryService = inventoryService;
            _paginationService = paginationService;
            _vehiclesService = vehiclesService;
            _notificationService = notificationService;
            _dispatchAuditLogRepository = dispatchAuditLogRepository;
            _logger = logger;
        }

        public async Task<SiteLoadList> GetLoadList(int siteId, SieveModel sieveModel)
        {
            _dispatchInstructionsRepository.SieveModel = sieveModel;
            var canAccessSite = await _userAccessService.CanAccessSite(siteId);
            if (canAccessSite)
            {
                return await GetSiteAdminLoadList(siteId);
            }
            else
            {
                var vehicleLoadList = await GetDriverLoadList(siteId);
                return new SiteLoadList()
                {
                    SiteId = siteId,
                    Loadlists = new List<VehicleLoadlist>() { vehicleLoadList },
                    TotalInventoryCount = vehicleLoadList.TotalInventoryCount,
                    TotalItemCount = vehicleLoadList.TotalItemCount,
                    TotalOrderCount = vehicleLoadList.TotalOrderCount
                };
            }
        }

        public async Task PickupDispatchRequest(DispatchMovementRequest pickupRequest)
        {
            switch (pickupRequest.RequestType.ToLower())
            {
                case "transfer-request":
                    await PickupTransferRequest(pickupRequest);
                    break;

                case "loadlist":
                    await PickupLoadlist(pickupRequest);
                    break;

                default:
                    throw new ValidationException($"Invalid Fulfilment type: {pickupRequest.RequestType}");
            }
        }

        public async Task DropDispatchRequest(DispatchMovementRequest dropRequest)
        {
            switch (dropRequest.RequestType.ToLower())
            {
                case "transfer-request":
                    await DropTransferRequest(dropRequest);
                    break;

                default:
                    throw new ValidationException($"Invalid Fulfilment type: {dropRequest.RequestType}");

            }
        }

        public async Task<IEnumerable<NSSDKViewModels.DispatchRecordUpdateResponse>> UpdateDispatchRecords(IEnumerable<ViewModels.DispatchRecordUpdateRequest> dispatchRecordUpdateRequests)
        {
            foreach (var dispatchUpdateReq in dispatchRecordUpdateRequests)
            {
                var validator = new DispatchUpdateRequestValidator();
                var validatedItem = validator.Validate(dispatchUpdateReq);
                if (!validatedItem.IsValid)
                {
                    throw new ValidationException($"{validatedItem.Errors.First()}");
                }
            }
            var dispatchRecordRequestIds = dispatchRecordUpdateRequests.Select(d => d.DispatchRecordId);
            var netSuiteDispatchRequest = new NSSDKViewModels.NetSuiteDispatchRequest()
            {
                DispatchRecordIds = dispatchRecordRequestIds,
                StartDate = DateTime.UtcNow.AddDays(-90)
            };

            var dispatchRecordResponse = await _netSuiteService.GetNetSuiteHmsDispatchRecords(netSuiteDispatchRequest);
            var dispatchRecordResultIds = dispatchRecordResponse.Results.Where(d => d.NSDispatchId.HasValue).Select(d => d.NSDispatchId.Value);
            var invalidDispatchRecordIds = dispatchRecordRequestIds.Except(dispatchRecordResultIds);
            if (invalidDispatchRecordIds.Any())
            {
                throw new ValidationException($"Dispatch Records with Ids ({string.Join(",", invalidDispatchRecordIds)}) is not valid");
            }

            var bulkUpdateResponses = await _netSuiteService.UpdateDispatchRecords(_mapper.Map<IEnumerable<NSSDKViewModels.DispatchRecordUpdateRequest>>(dispatchRecordUpdateRequests));
            foreach (var bulkUpdateRes in bulkUpdateResponses)
            {
                if (string.Equals(bulkUpdateRes.Status, "FAILED", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogError($"{bulkUpdateRes.Error}: {bulkUpdateRes.Message}");
                }
            }
            var successDispatchRecordIds = bulkUpdateResponses.Where(br => string.IsNullOrEmpty(br.Error) && string.Equals(br.Status, "SUCCESS", StringComparison.OrdinalIgnoreCase)).Select(br => br.DispatchRecordId);
            var successDispatchUpdateReq = dispatchRecordUpdateRequests.Where(dr => successDispatchRecordIds.Contains(dr.DispatchRecordId));
            await LogDispatchRecordUpdate(dispatchRecordResponse, successDispatchUpdateReq);

            return bulkUpdateResponses;

        }

        public async Task<DispatchResponse> GetLoggedInDriverDispatch(SieveModel sieveModel)
        {
            var loggedInUserId = GetLoggedInUserId();
            var driverModel = await _driverRepository.GetAsync(d => d.UserId == loggedInUserId);
            if (driverModel == null)
            {
                throw new ValidationException($"Logged In user is not a valid driver");
            }
            if (driverModel.CurrentVehicleId == null)
            {
                throw new ValidationException("Driver should have a current vehicle");
            }
            _dispatchInstructionsRepository.SieveModel = sieveModel;
            var dispatchInstructionModels = await _dispatchInstructionsRepository.GetManyAsync(i => i.VehicleId == driverModel.CurrentVehicleId);
            var orderHeaderIds = dispatchInstructionModels.Where(d => d.OrderHeaderId != null)
                                              .Select(d => d.OrderHeaderId);

            var orders = await _orderHeadersRepository.GetDispatchOrderAsync(o => orderHeaderIds.Contains(o.Id));

            var driver = _mapper.Map<Driver>(driverModel);

            var routes = GetDispatchRoutes(orders).ToList();
            if (routes != null && routes.Count() > 0)
            {
                var startLocation = new RouteItem()
                {
                    SequenceNumber = routes.Select(r => r.SequenceNumber).Min() - 1,
                    Address = _mapper.Map<Address>(driverModel.CurrentSite.Address)
                };
                var endLocation = new RouteItem()
                {
                    SequenceNumber = routes.Select(r => r.SequenceNumber).Max() + 1,
                    Address = _mapper.Map<Address>(driverModel.CurrentSite.Address)
                };

                routes.Add(startLocation);
                routes.Add(endLocation);
                routes = routes.OrderBy(r => r.SequenceNumber).ToList();
            }

            return new DispatchResponse()
            {
                VehicleId = driverModel.CurrentVehicleId.HasValue ? driverModel.CurrentVehicleId.Value : 0,
                Vehicle = _mapper.Map<Vehicle>(driverModel.CurrentVehicle),
                DriverId = driverModel.Id,
                Driver = driver,
                Routes = routes,
                OrderHeaders = _mapper.Map<IEnumerable<OrderHeader>>(orders)
            };
        }

        public async Task<IEnumerable<OrderLocation>> GetCurrentOrderLocation(IEnumerable<int> orderIds)
        {
            var orderLocations = new List<OrderLocation>();
            var orderModels = await _orderHeadersRepository.GetManyAsync(o => orderIds.Contains(o.Id));
            var ordersWithInvalidStatus = orderModels.Where(o => o.StatusId != (int)OrderHeaderStatusTypes.Enroute);
            if (ordersWithInvalidStatus.Count() > 0)
            {
                var orderIdsWithInvalidStatus = ordersWithInvalidStatus.Select(o => o.Id);
                throw new ValidationException($"order Ids ({string.Join(",", orderIdsWithInvalidStatus)}) does not have valid status for getting current location");
            }

            var dispatchInstructions = await _dispatchInstructionsRepository.GetManyAsync(d => d.OrderHeaderId.HasValue && orderIds.Contains(d.OrderHeaderId.Value));
            foreach (var orderModel in orderModels)
            {
                var dispatchInstruction = dispatchInstructions.FirstOrDefault(d => d.OrderHeaderId == orderModel.Id);
                var assignedDriver = dispatchInstruction?.Vehicle.DriversCurrentVehicle;
                orderLocations.Add(new OrderLocation()
                {
                    OrderId = orderModel.Id,
                    Latitude = assignedDriver?.LastKnownLatitude,
                    Longitude = assignedDriver?.LastKnownLongitude
                });
            }

            return orderLocations;
        }

        public async Task<PaginatedList<DispatchInstruction>> GetAllDispatchInstructions(SieveModel sieveModel)
        {
            _dispatchInstructionsRepository.SieveModel = sieveModel;
            var totalRecords = await _dispatchInstructionsRepository.GetCountAsync(f => true);
            var dispatchInstructionsModels = await _dispatchInstructionsRepository.GetAllAsync();
            var dispatchInstructions = _mapper.Map<IEnumerable<DispatchInstruction>>(dispatchInstructionsModels);

            return _paginationService.GetPaginatedList(dispatchInstructions, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<DispatchInstruction> GetDispatchInstructionById(int id)
        {
            var dispatchInstruction = await _dispatchInstructionsRepository.GetByIdAsync(id);
            return _mapper.Map<DispatchInstruction>(dispatchInstruction);
        }

        public async Task<IEnumerable<DispatchInstruction>> CreateDispatchInstruction(DispatchAssignmentRequest dispatchAssignmentRequest)
        {
            var dispatchAssignmentValidator = new DispatchAssignmentValidator();
            var validationResult = dispatchAssignmentValidator.Validate(dispatchAssignmentRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            var vehicleModel = await _sitesRepository.GetAsync(v => v.Id == dispatchAssignmentRequest.VehicleId
                                                                && v.LocationType.ToLower() == "vehicle");
            if (vehicleModel == null)
            {
                throw new ValidationException($"Vehicle Id ({dispatchAssignmentRequest.VehicleId}) is not valid");
            }
            var transferRequestIds = dispatchAssignmentRequest.DispatchDetails.Where(d => d.TransferRequestId != null).Select(d => d.TransferRequestId ?? 0);

            var transferRequests = await _itemTransferRequestRepository.GetManyAsync(t => transferRequestIds.Contains(t.Id));
            if (transferRequests.Count() != transferRequestIds.Count())
            {
                var invalidTransferRequestIds = transferRequestIds.Except(transferRequests.Select(t => t.Id));
                throw new ValidationException($"Transfer request Ids ({string.Join(",", invalidTransferRequestIds)}) are not valid");
            }

            var orderHeaderIds = dispatchAssignmentRequest.DispatchDetails.Where(d => d.OrderHeaderId != null).Select(d => d.OrderHeaderId ?? 0);
            var orderHeaders = await _orderHeadersRepository.GetManyAsync(o => orderHeaderIds.Contains(o.Id));
            if (orderHeaders.Count() != orderHeaderIds.Count())
            {
                var invalidOrderHeaderIds = orderHeaderIds.Except(orderHeaders.Select(t => t.Id));
                throw new ValidationException($"Order Header Ids ({string.Join(",", invalidOrderHeaderIds)}) are not valid");
            }

            var ordersWithInvalidStatus = orderHeaders.Where(o => o.StatusId == (int)OrderHeaderStatusTypes.Pending_Approval
                                                                    || o.StatusId == (int)OrderHeaderStatusTypes.Cancelled);
            if (ordersWithInvalidStatus.Count() > 0)
            {
                throw new ValidationException($"Orders Header Ids ({string.Join(",", ordersWithInvalidStatus.Select(o => o.Id))}) have invalid status for assignment");
            }
            var vehicleSite = await _sitesRepository.GetAsync(s => s.NetSuiteLocationId == vehicleModel.ParentNetSuiteLocationId);
            var nonVehicleSiteOrders = orderHeaders.Where(o => o.SiteId != vehicleSite?.Id);
            if (nonVehicleSiteOrders.Count() > 0)
            {
                throw new ValidationException($"Orders ({string.Join(",", nonVehicleSiteOrders.Select(o => o.NetSuiteOrderId))}) are not assigned to ({vehicleSite?.Name}). " +
                                              $"Vehicle ({vehicleModel.Name}) is currently assigned to ({vehicleSite?.Name})");
            }
            var vehicleDispatchInstructions = await _dispatchInstructionsRepository.GetManyAsync(d => d.VehicleId == dispatchAssignmentRequest.VehicleId);
            var existingAssignedOrderIds = vehicleDispatchInstructions.Where(d => d.OrderHeaderId != null).Select(d => d.OrderHeaderId.Value).Distinct();

            var completedOrderIds = orderHeaders.Where(o => o.StatusId == (int)OrderHeaderStatusTypes.Completed).Select(o => o.Id);
            var CompletedOrderIdsByOtherVehicle = completedOrderIds.Except(existingAssignedOrderIds);

            if (CompletedOrderIdsByOtherVehicle != null && CompletedOrderIdsByOtherVehicle.Count() > 0)
            {
                var CompletedOrderByOtherVehicle = orderHeaders.Where(o => CompletedOrderIdsByOtherVehicle.Contains(o.Id));
                throw new ValidationException($"Cannot re-assign completed orders ({string.Join(",", CompletedOrderByOtherVehicle.Select(o => o.OrderNumber))}) to different vehicle.");
            }
            var newAssignedOrderIds = orderHeaderIds.Except(existingAssignedOrderIds.Select(t => t));

            var unAssignOrderIds = existingAssignedOrderIds.Except(orderHeaderIds.Select(t => t));
            if (unAssignOrderIds.Any())
            {
                foreach (var orderId in unAssignOrderIds)
                {
                    try
                    {
                        await UnassignOrder(orderId);
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
            }

            var dispatchInstructionModels = new List<Data.Models.DispatchInstructions>();
            foreach (var dispatchDetail in dispatchAssignmentRequest.DispatchDetails)
            {
                var dispatchInstructionRequest = GetBaseDispatchInstruction(dispatchAssignmentRequest.VehicleId, dispatchDetail);

                if (dispatchDetail.TransferRequestId != null)
                {
                    var transferRequest = transferRequests.FirstOrDefault(t => t.Id == dispatchDetail.TransferRequestId);
                    if (transferRequest.StatusId != (int)Data.Enums.TransferRequestStatusTypes.Created)
                    {
                        throw new ValidationException($"Transfer request with Id({transferRequest.Id}) is already processed");
                    }
                    dispatchInstructionModels.Add(dispatchInstructionRequest);

                    transferRequest.StatusId = (int)Data.Enums.TransferRequestStatusTypes.AssignedToVehicle;
                }
                else if (dispatchDetail.OrderHeaderId != null)
                {
                    var orderHeader = orderHeaders.FirstOrDefault(t => t.Id == dispatchDetail.OrderHeaderId);

                    dispatchInstructionRequest = await GetDispatchInstructionForOrderHeader(dispatchInstructionRequest, orderHeader.Id);
                    dispatchInstructionModels.Add(dispatchInstructionRequest);

                    UpdateNewAssignedOrderStatus(orderHeader, newAssignedOrderIds);
                }
            }
            await _itemTransferRequestRepository.UpdateManyAsync(transferRequests);

            await _orderHeadersRepository.UpdateManyAsync(orderHeaders);

            await _dispatchInstructionsRepository.UpdateManyAsync(dispatchInstructionModels);
            var driverUserId = vehicleModel.DriversCurrentVehicle?.UserId;
            if (driverUserId != null && newAssignedOrderIds != null && newAssignedOrderIds.Count() > 0)
            {
                var orderNumbers = orderHeaders.Where(o => newAssignedOrderIds.Contains(o.Id)).Select(o => o.OrderNumber);
                await _notificationService.SendOrderAssignNotification(orderNumbers, driverUserId.Value, vehicleModel.Cvn);
            }
            return _mapper.Map<IEnumerable<DispatchInstruction>>(dispatchInstructionModels);
        }

        public async Task UnassignOrder(int orderId)
        {
            var order = await _orderHeadersRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new ValidationException($"Order id {orderId} is invalid");
            }

            await _dispatchInstructionsRepository.DeleteAsync(d => d.OrderHeaderId == orderId);
            await FixOrderStatus(order, false);
        }

        public async Task<Data.Models.OrderHeaders> FixOrderStatus(Data.Models.OrderHeaders orderModel, bool previewChanges)
        {
            var dispatchInstruction = await _dispatchInstructionsRepository.GetAsync(d => d.OrderHeaderId == orderModel.Id);

            foreach (var lineItemModel in orderModel.OrderLineItems)
            {
                var fulfillmentLineItems = await _orderFulfillmentLineItemsRepository.GetManyAsync(l => l.OrderLineItemId == lineItemModel.Id
                                                                                                    && l.OrderHeaderId == orderModel.Id);
                if (fulfillmentLineItems != null && fulfillmentLineItems.Count() > 0)
                {
                    var fulfilledItemCount = fulfillmentLineItems.Sum(l => l.Quantity);
                    if (lineItemModel.ItemCount <= fulfilledItemCount)
                    {
                        lineItemModel.StatusId = (int)Data.Enums.OrderLineItemStatusTypes.Completed;
                        lineItemModel.DispatchStatusId = (int)Data.Enums.OrderLineItemStatusTypes.Completed;
                    }

                    else if (fulfilledItemCount > 0 && lineItemModel.ItemCount > fulfilledItemCount)
                    {
                        lineItemModel.StatusId = dispatchInstruction != null ? (int)Data.Enums.OrderLineItemStatusTypes.Scheduled
                                                                                     : (int)Data.Enums.OrderLineItemStatusTypes.Partial_Fulfillment;

                        lineItemModel.DispatchStatusId = dispatchInstruction != null ? (int)Data.Enums.OrderLineItemStatusTypes.Scheduled
                                                                                     : (int)Data.Enums.OrderLineItemStatusTypes.Planned;
                    }
                }
                else if (dispatchInstruction != null)
                {
                    lineItemModel.StatusId = (int)Data.Enums.OrderLineItemStatusTypes.Scheduled;
                    lineItemModel.DispatchStatusId = (int)Data.Enums.OrderLineItemStatusTypes.Scheduled;
                }
                else
                {
                    lineItemModel.StatusId = (int)Data.Enums.OrderLineItemStatusTypes.Planned;
                    lineItemModel.DispatchStatusId = (int)Data.Enums.OrderLineItemStatusTypes.Planned;
                }
            }
            // Update Order dispatch status
            if (orderModel.OrderLineItems == null || orderModel.OrderLineItems.Count() == 0)
            {
                orderModel.DispatchStatusId = (int)Data.Enums.OrderHeaderStatusTypes.Cancelled;
            }
            else if (orderModel.OrderLineItems.All(l => l.DispatchStatusId == (int)Data.Enums.OrderLineItemStatusTypes.Completed))
            {
                orderModel.DispatchStatusId = (int)Data.Enums.OrderHeaderStatusTypes.Completed;
            }
            else if (orderModel.OrderLineItems.Any(l => l.DispatchStatusId == (int)Data.Enums.OrderLineItemStatusTypes.Completed
                                                  || l.DispatchStatusId == (int)Data.Enums.OrderLineItemStatusTypes.Partial_Fulfillment))
            {

                if (dispatchInstruction != null)
                {
                    orderModel.DispatchStatusId = (int)Data.Enums.OrderHeaderStatusTypes.Scheduled;
                }
                else
                {
                    orderModel.DispatchStatusId = (int)Data.Enums.OrderHeaderStatusTypes.Planned;
                }
            }
            else
            {
                orderModel.DispatchStatusId = (int)Data.Enums.OrderHeaderStatusTypes.Planned;
            }

            // Update Order status
            if (orderModel.OrderLineItems == null || orderModel.OrderLineItems.Count() == 0)
            {
                orderModel.StatusId = (int)Data.Enums.OrderHeaderStatusTypes.Cancelled;
                orderModel.FulfillmentEndDateTime ??= DateTime.UtcNow;
            }
            else if (orderModel.OrderLineItems.All(l => l.StatusId == (int)Data.Enums.OrderLineItemStatusTypes.Completed))
            {
                orderModel.StatusId = (int)Data.Enums.OrderHeaderStatusTypes.Completed;
                orderModel.FulfillmentEndDateTime ??= DateTime.UtcNow;
            }
            else if (orderModel.OrderLineItems.Any(l => l.StatusId == (int)Data.Enums.OrderLineItemStatusTypes.Completed
                                                    || l.StatusId == (int)Data.Enums.OrderLineItemStatusTypes.Partial_Fulfillment))
            {
                if (dispatchInstruction != null)
                {
                    orderModel.StatusId = (int)Data.Enums.OrderHeaderStatusTypes.Scheduled;
                }
                else
                {
                    orderModel.StatusId = (int)Data.Enums.OrderHeaderStatusTypes.Partial_Fulfillment;
                }
            }
            else
            {
                orderModel.StatusId = (int)Data.Enums.OrderHeaderStatusTypes.Planned;
            }
            if (!previewChanges)
            {
                await _orderHeadersRepository.UpdateAsync(orderModel);
            }
            return orderModel;
        }

        private void UpdateNewAssignedOrderStatus(Data.Models.OrderHeaders orderHeader, IEnumerable<int> newAssignedOrderIds)
        {
            var isNewAssignedOrder = newAssignedOrderIds.Contains(orderHeader.Id);
            if (!isNewAssignedOrder)
            {
                return;
            }

            if (orderHeader.StatusId != (int)OrderHeaderStatusTypes.Completed
                && orderHeader.StatusId != (int)OrderHeaderStatusTypes.OutForFulfillment
                && orderHeader.StatusId != (int)OrderHeaderStatusTypes.Enroute
                && orderHeader.StatusId != (int)OrderHeaderStatusTypes.OnSite)
            {
                orderHeader.StatusId = (int)OrderHeaderStatusTypes.Scheduled;
                orderHeader.DispatchStatusId = (int)OrderHeaderStatusTypes.Scheduled;
            }
            foreach (var lineItem in orderHeader.OrderLineItems)
            {
                if (lineItem.StatusId != (int)OrderLineItemStatusTypes.Completed
                    && lineItem.StatusId != (int)OrderLineItemStatusTypes.OutForFulfillment
                    && lineItem.StatusId != (int)OrderLineItemStatusTypes.Enroute
                    && lineItem.StatusId != (int)OrderLineItemStatusTypes.OnSite)
                {
                    lineItem.StatusId = (int)OrderLineItemStatusTypes.Scheduled;
                    lineItem.DispatchStatusId = (int)OrderLineItemStatusTypes.Scheduled;
                }
            }
        }

        private async Task<Data.Models.DispatchInstructions> GetDispatchInstructionForOrderHeader(Data.Models.DispatchInstructions dispatchInstructionModel, int orderHeaderId)
        {
            var dispatchInstruction = await _dispatchInstructionsRepository.GetAsync(d => d.OrderHeaderId == orderHeaderId);
            if (dispatchInstruction == null)
            {
                dispatchInstruction = dispatchInstructionModel;
            }
            else
            {
                dispatchInstruction.SequenceNumber = dispatchInstructionModel.SequenceNumber;
                dispatchInstruction.DispatchStartDateTime = dispatchInstructionModel.DispatchStartDateTime;
                dispatchInstruction.DispatchEndDateTime = dispatchInstructionModel.DispatchEndDateTime;
                dispatchInstruction.VehicleId = dispatchInstructionModel.VehicleId;
            }
            return dispatchInstruction;
        }

        private Data.Models.DispatchInstructions GetBaseDispatchInstruction(int vehicleId, DispatchInstructionRequest dispatchInstructionRequest)
        {
            var dispatchInstruction = _mapper.Map<Data.Models.DispatchInstructions>(dispatchInstructionRequest);
            dispatchInstruction.VehicleId = vehicleId;
            dispatchInstruction.SequenceNumber = dispatchInstructionRequest.SequenceNumber;
            dispatchInstruction.DispatchStartDateTime = dispatchInstructionRequest.DispatchStartDateTime;
            dispatchInstruction.DispatchEndDateTime = dispatchInstructionRequest.DispatchEndDateTime;

            return dispatchInstruction;
        }

        private async Task PickupLoadlist(DispatchMovementRequest pickupRequest)
        {
            var driverLoadlist = await GetDriverLoadList(pickupRequest.RequestId, pickupRequest.VehicleId);

            if (driverLoadlist.Items == null || driverLoadlist.Items.Count() == 0)
            {
                throw new ValidationException($"loadlist for site with Id({pickupRequest.RequestId}) does not have any pending items to pickup");
            }

            await ValidateDispatchMovement(pickupRequest, driverLoadlist.VehicleId, true);

            var dispatchInventories = await GetDispatchInventory(pickupRequest.DispatchItems, pickupRequest.RequestId);

            await UpdatePickupForDispatchedInventory(dispatchInventories, driverLoadlist.VehicleId);

            var pickedUpOrderIds = driverLoadlist.Orders.Select(o => o.Id);
            var pickedUpOrders = await _orderHeadersRepository.GetManyAsync(o => pickedUpOrderIds.Contains(o.Id));
            foreach (var orderModel in pickedUpOrders)
            {
                orderModel.DispatchStatusId = (int)OrderHeaderStatusTypes.OnTruck;
                orderModel.StatusId = (int)OrderHeaderStatusTypes.OnTruck;
            }
            await _orderHeadersRepository.UpdateManyAsync(pickedUpOrders);

            var pickedUpTransferRequestIds = driverLoadlist.TransferRequests.Select(t => t.Id);
            var pickedUpTransferRequests = await _itemTransferRequestRepository.GetManyAsync(t => pickedUpTransferRequestIds.Contains(t.Id));
            foreach (var transferRequest in pickedUpTransferRequests)
            {
                transferRequest.StatusId = (int)TransferRequestStatusTypes.InTransit;
            }
            await _itemTransferRequestRepository.UpdateManyAsync(pickedUpTransferRequests);

            MoveInventoryInNetSuite(pickupRequest.RequestId, pickupRequest.VehicleId, dispatchInventories);
        }

        private async Task<Data.Models.ItemTransferRequests> PickupTransferRequest(DispatchMovementRequest pickupRequest)
        {
            var transferRequestModel = await _itemTransferRequestRepository.GetByIdAsync(pickupRequest.RequestId);

            var dispatchInstructionModel = await ValidateTransferRequestMovement(pickupRequest);

            var dispatchInventoryModels = await GetDispatchInventory(pickupRequest.DispatchItems, transferRequestModel.SourceLocationId);

            await UpdatePickupForDispatchedInventory(dispatchInventoryModels, dispatchInstructionModel.VehicleId);

            transferRequestModel.StatusId = (int)TransferRequestStatusTypes.InTransit;

            await _itemTransferRequestRepository.UpdateAsync(transferRequestModel);

            MoveInventoryInNetSuite(transferRequestModel.SourceLocationId, pickupRequest.VehicleId, dispatchInventoryModels);

            return transferRequestModel;
        }

        private async Task<Data.Models.ItemTransferRequests> DropTransferRequest(DispatchMovementRequest dropRequest)
        {
            var transferRequestModel = await _itemTransferRequestRepository.GetByIdAsync(dropRequest.RequestId);

            var dispatchInstructionModel = await ValidateTransferRequestMovement(dropRequest);

            var dispatchInventories = await GetDispatchInventory(dropRequest.DispatchItems, transferRequestModel.DestinationLocationId);

            await UpdateDropForDispatchedInventory(dispatchInventories, transferRequestModel.DestinationLocationId);

            await _dispatchInstructionsRepository.UpdateAsync(dispatchInstructionModel);

            transferRequestModel.StatusId = (int)TransferRequestStatusTypes.Completed;

            await _itemTransferRequestRepository.UpdateAsync(transferRequestModel);
            MoveInventoryInNetSuite(dropRequest.VehicleId, transferRequestModel.DestinationLocationId, dispatchInventories);
            return transferRequestModel;
        }

        private async Task MoveInventoryInNetSuite(int sourceSiteId, int destinationSiteId, IEnumerable<DispatchInventory> dispatchInventories)
        {
            var movementItems = new List<NSSDKViewModels.MovementItem>();
            foreach (var dispatchInventory in dispatchInventories)
            {
                var movementItem = new NSSDKViewModels.MovementItem()
                {
                    NetSuiteItemId = dispatchInventory.Inventory.Item.NetSuiteItemId,
                    AssetTag = dispatchInventory.Inventory.AssetTagNumber,
                    LotNumber = dispatchInventory.Inventory.LotNumber,
                    SerialNumber = dispatchInventory.Inventory.SerialNumber,
                    Quantity = dispatchInventory.Count
                };
                movementItems.Add(movementItem);
            }

            var movementRequest = new NSSDKViewModels.InventoryMovementRequest()
            {
                NetSuiteSourceWarehouseId = await GetNetSuiteWarehouseIdByLocation(sourceSiteId),
                NetSuiteDestinationWarehouseId = await GetNetSuiteWarehouseIdByLocation(destinationSiteId),
                Items = movementItems
            };
            await _netSuiteService.ConfirmInventoryMovement(movementRequest);

        }

        private async Task<Data.Models.DispatchInstructions> ValidateTransferRequestMovement(DispatchMovementRequest movementRequest)
        {
            var dispatchInstructionModel = await _dispatchInstructionsRepository.GetAsync(di => di.TransferRequestId == movementRequest.RequestId);
            if (dispatchInstructionModel == null)
            {
                throw new ValidationException($"Dispatch Instruction not found");
            }
            await ValidateDispatchMovement(movementRequest, dispatchInstructionModel.VehicleId);
            return dispatchInstructionModel;
        }

        private async Task ValidateDispatchMovement(DispatchMovementRequest movementRequest, int vehicleId, bool allowDispatchToSiteMember = false)
        {
            var dispatchValidator = new DispatchValidator();
            var validationResult = dispatchValidator.Validate(movementRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors[0].ErrorMessage);
            }

            await ValidateDriverAndSiteMember(vehicleId, allowDispatchToSiteMember);
        }

        private async Task UpdatePickupForDispatchedInventory(IEnumerable<DispatchInventory> dispatchInventories, int vehicleId)
        {
            var dispatchInventoryIds = dispatchInventories.Select(di => di.Inventory.Id);
            var dispatchInventoryModels = await _inventoryRepository.GetManyAsync(i => dispatchInventoryIds.Contains(i.Id));
            foreach (var inventoryModel in dispatchInventoryModels)
            {
                var isStandAloneInventory = !inventoryModel.Item.IsSerialized && !inventoryModel.Item.IsAssetTagged && !inventoryModel.Item.IsLotNumbered;
                if (isStandAloneInventory)
                {
                    var dispatchInventory = dispatchInventories.FirstOrDefault(di => di.Inventory.Id == inventoryModel.Id);
                    var vehicleInventoryModel = await GetStandaloneInventory(inventoryModel.ItemId, vehicleId);

                    inventoryModel.QuantityAvailable -= dispatchInventory.Count;
                    vehicleInventoryModel.QuantityAvailable += dispatchInventory.Count;
                    await _inventoryRepository.UpdateAsync(vehicleInventoryModel);

                }
                else if (inventoryModel.Item.IsLotNumbered)
                {
                    var dispatchInventory = dispatchInventories.FirstOrDefault(di => di.Inventory.Id == inventoryModel.Id);
                    var vehicleInventoryModel = await GetLotNumberedInventory(inventoryModel.ItemId, vehicleId, inventoryModel.LotNumber);

                    inventoryModel.QuantityAvailable -= dispatchInventory.Count;
                    vehicleInventoryModel.QuantityAvailable += dispatchInventory.Count;
                    await _inventoryRepository.UpdateAsync(vehicleInventoryModel);
                }
                else
                {
                    inventoryModel.CurrentLocationId = vehicleId;
                }
            }
            await _inventoryRepository.UpdateManyAsync(dispatchInventoryModels);
        }

        private async Task UpdateDropForDispatchedInventory(IEnumerable<DispatchInventory> dispatchInventories, int locationId)
        {
            var dispatchInventoryIds = dispatchInventories.Select(di => di.Inventory.Id);
            var dispatchInventoryModels = await _inventoryRepository.GetManyAsync(i => dispatchInventoryIds.Contains(i.Id));
            foreach (var inventoryModel in dispatchInventoryModels)
            {
                var isStandAloneInventory = !inventoryModel.Item.IsSerialized && !inventoryModel.Item.IsAssetTagged && !inventoryModel.Item.IsLotNumbered;
                if (isStandAloneInventory)
                {
                    var dispatchInventory = dispatchInventories.FirstOrDefault(di => di.Inventory.Id == inventoryModel.Id);
                    inventoryModel.QuantityAvailable -= dispatchInventory.Count;

                    var locationInventoryModel = await GetStandaloneInventory(inventoryModel.ItemId, locationId);
                    locationInventoryModel.QuantityAvailable += dispatchInventory.Count;
                    await _inventoryRepository.UpdateAsync(locationInventoryModel);
                }
                else if (inventoryModel.Item.IsLotNumbered)
                {
                    var dispatchInventory = dispatchInventories.FirstOrDefault(di => di.Inventory.Id == inventoryModel.Id);
                    inventoryModel.QuantityAvailable -= dispatchInventory.Count;

                    var locationInventoryModel = await GetLotNumberedInventory(inventoryModel.ItemId, locationId, inventoryModel.LotNumber);
                    locationInventoryModel.QuantityAvailable += dispatchInventory.Count;
                    await _inventoryRepository.UpdateAsync(locationInventoryModel);
                }
                else
                {
                    inventoryModel.StatusId = (int)InventoryStatusTypes.Available;

                    inventoryModel.CurrentLocationId = locationId;
                }
            }
            await _inventoryRepository.UpdateManyAsync(dispatchInventoryModels);
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
                                                                    && i.LotNumber == lotNumber);
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

        private async Task<IEnumerable<DispatchInventory>> GetDispatchInventory(IEnumerable<DispatchItem> dispatchItems, int? sourceLocationId)
        {
            var dispatchSerialNumbers = dispatchItems.Where(d => !string.IsNullOrEmpty(d.SerialNumber)).Select(d => d.SerialNumber);
            var dispatchAssetTagNumbers = dispatchItems.Where(d => !string.IsNullOrEmpty(d.AssetTagNumber)).Select(d => d.AssetTagNumber);
            var dispatchLotNumbers = dispatchItems.Where(d => !string.IsNullOrEmpty(d.LotNumber)).Select(d => d.LotNumber);
            var dispatchItemIds = dispatchItems.Where(d => d.ItemId.HasValue).Select(d => d.ItemId);

            var duplicateSerialNumbers = dispatchSerialNumbers.GroupBy(ds => ds).Where(ds => ds.Count() > 1);
            if (duplicateSerialNumbers.Count() > 0)
            {
                throw new ValidationException($"Duplicate Serial numbers({string.Join(", ", duplicateSerialNumbers.Select(di => di.Key))}) are not allowed");
            }

            var duplicateAssetTagNumbers = dispatchAssetTagNumbers.GroupBy(dt => dt).Where(dt => dt.Count() > 1);
            if (duplicateAssetTagNumbers.Count() > 0)
            {
                throw new ValidationException($"Duplicate Asset tag numbers({string.Join(", ", duplicateAssetTagNumbers.Select(di => di.Key))}) are not allowed");
            }

            var trackedInventoryModels = await _inventoryRepository.GetManyAsync(i => dispatchSerialNumbers.Contains(i.SerialNumber)
                                                                         || dispatchAssetTagNumbers.Contains(i.AssetTagNumber));

            var standaloneInventoryModels = await _inventoryRepository.GetManyAsync(i => dispatchItemIds.Contains(i.ItemId) && i.CurrentLocationId == sourceLocationId);

            var lotNumberedInventoryModels = await _inventoryRepository.GetManyAsync(i => dispatchItemIds.Contains(i.ItemId) && i.CurrentLocationId == sourceLocationId
                                                                                    && dispatchLotNumbers.Contains(i.LotNumber));

            var standaloneItemModels = await _itemRepository.GetManyAsync(i => dispatchItemIds.Contains(i.Id));

            var sourceLocationName = await _inventoryService.GetLocationName(sourceLocationId, "Source location");

            var inventoryModels = trackedInventoryModels.Concat(standaloneInventoryModels).Concat(lotNumberedInventoryModels);

            return dispatchItems.Select(di =>
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
                        var itemModel = standaloneItemModels.FirstOrDefault(i => i.Id == di.ItemId);
                        if (itemModel == null)
                        {
                            throw new ValidationException($"ItemId ({di.ItemId}) is not valid");
                        }

                        throw new ValidationException($"No inventory available for {itemModel.Name}({itemModel.ItemNumber}) at {sourceLocationName}");
                    }

                    var isStandAloneInventory = !inventoryModel.Item.IsSerialized && !inventoryModel.Item.IsAssetTagged && !inventoryModel.Item.IsLotNumbered;
                    if (!isStandAloneInventory)
                    {
                        throw new ValidationException($"serialNumber/AssetTagNumber/LotNumber should be provided for tracked inventory");
                    }
                }

                if (inventoryModel.Item.IsSerialized && di.Count != 1)
                {
                    throw new ValidationException($"Serialized item ({inventoryModel.Item.Name}) can not have count more than 1");
                }

                return new DispatchInventory()
                {
                    Inventory = _mapper.Map<Inventory>(inventoryModel),
                    Count = di.Count,
                    IsSerialized = inventoryModel.Item.IsSerialized,
                };

            });
        }

        private async Task<VehicleLoadlist> GetDriverLoadList(int siteId, int? vehicleId = null)
        {
            int? driverVehicleId;
            if (vehicleId != null)
            {
                driverVehicleId = vehicleId;
                await ValidateDriverAndSiteMember(vehicleId, true);
            }
            else
            {
                var loggedInUserId = GetLoggedInUserId();
                var driverModel = await _driverRepository.GetAsync(d => d.UserId == loggedInUserId);
                if (driverModel == null)
                {
                    throw new ValidationException($"Logged In user is not a valid driver");
                }
                if (driverModel.CurrentVehicleId == null)
                {
                    throw new ValidationException("Driver should have a current vehicle");
                }
                driverVehicleId = driverModel.CurrentVehicleId;
            }
            var orderDispatchInstructions = await _dispatchInstructionsRepository.GetManyAsync(i => i.OrderHeader != null
                                                                                               && i.VehicleId == driverVehicleId
                                                                                               && i.OrderHeader.SiteId == siteId
                                                                                               && i.OrderHeader.StatusId == (int)OrderHeaderStatusTypes.Scheduled);
            var transferRequestDispatchInstructions = await _dispatchInstructionsRepository.GetManyAsync(i => i.TransferRequest != null
                                                                                                            && i.VehicleId == driverVehicleId
                                                                                                            && i.TransferRequest.SourceLocationId == siteId
                                                                                                            && i.TransferRequest.StatusId == (int)TransferRequestStatusTypes.AssignedToVehicle);

            var loadlist = new VehicleLoadlist()
            {
                VehicleId = driverVehicleId ?? 0
            };

            if (orderDispatchInstructions.Count() == 0 && transferRequestDispatchInstructions.Count() == 0)
            {
                return loadlist;
            }
            var items = new List<LoadlistItem>();
            var transferRequestIds = transferRequestDispatchInstructions.Where(d => d.TransferRequestId != null)
                                                         .Select(d => d.TransferRequestId);

            var tranferRequestsToLoad = await _itemTransferRequestRepository.GetManyAsync(tr => transferRequestIds.Contains(tr.Id));
            if (tranferRequestsToLoad.Count() > 0)
            {
                var groupedTransferRequestItems = tranferRequestsToLoad.Select(l =>
                                                                        new Data.Models.OrderLineItems()
                                                                        {
                                                                            ItemId = l.ItemId,
                                                                            Item = l.Item,
                                                                            ItemCount = l.ItemCount
                                                                        });
                items.AddRange(_mapper.Map<IEnumerable<LoadlistItem>>(groupedTransferRequestItems).ToList());
            }
            var orderHeaderIds = orderDispatchInstructions.Where(d => d.OrderHeaderId != null)
                                                     .Select(d => d.OrderHeaderId);

            var ordersToLoad = await _orderHeadersRepository.GetDispatchOrderAsync(o => orderHeaderIds.Contains(o.Id));
            if (ordersToLoad.Count() > 0)
            {
                var groupedLineItems = ordersToLoad.SelectMany(o => o.OrderLineItems)
                                                    .Where(l => l.ActionId == (int)OrderTypes.Delivery)
                                                    .GroupBy(l => l.ItemId)
                                                    .Select(l =>
                                                        new Data.Models.OrderLineItems()
                                                        {
                                                            ItemId = l.Key,
                                                            Item = l.FirstOrDefault().Item,
                                                            ItemCount = l.Sum(lg => lg.ItemCount)
                                                        });
                items.AddRange(_mapper.Map<IEnumerable<LoadlistItem>>(groupedLineItems));
            }
            loadlist.Items = items;
            loadlist.TotalItemCount = loadlist.Items.Count();
            loadlist.TotalInventoryCount = loadlist.Items.Sum(p => p.ItemCount);
            loadlist.TotalOrderCount = (ordersToLoad.Count() + tranferRequestsToLoad.Count());
            loadlist.Orders = _mapper.Map<IEnumerable<OrderHeader>>(ordersToLoad);
            loadlist.TransferRequests = _mapper.Map<IEnumerable<ItemTransferRequest>>(tranferRequestsToLoad);
            return loadlist;
        }

        private async Task<SiteLoadList> GetSiteAdminLoadList(int siteId)
        {
            var loadListOrderTypeIds = new List<int>()
            {
                (int) OrderTypes.Delivery,(int) OrderTypes.Exchange,(int) OrderTypes.Respite
            };
            var orderDispatchInstructions = await _dispatchInstructionsRepository.GetManyAsync(i => i.OrderHeader != null
                                                                                               && i.OrderHeader.SiteId == siteId
                                                                                               && loadListOrderTypeIds.Contains(i.OrderHeader.OrderTypeId)
                                                                                               && i.OrderHeader.StatusId == (int)OrderHeaderStatusTypes.Scheduled);
            var transferRequestDispatchInstructions = await _dispatchInstructionsRepository.GetManyAsync(i => i.TransferRequest != null
                                                                                                            && i.TransferRequest.SourceLocationId == siteId
                                                                                                            && i.TransferRequest.StatusId == (int)TransferRequestStatusTypes.AssignedToVehicle);
            var a = await _dispatchInstructionsRepository.GetAllAsync();
            var siteLoadList = new SiteLoadList()
            {
                SiteId = siteId,
            };
            if (orderDispatchInstructions.Count() == 0 && transferRequestDispatchInstructions.Count() == 0)
            {
                return siteLoadList;
            }
            var loadlists = await GetTranferRequestLoadList(transferRequestDispatchInstructions);
            loadlists = await GetOrderLoadList(orderDispatchInstructions, loadlists);

            siteLoadList.Loadlists = loadlists;
            siteLoadList.TotalInventoryCount = loadlists.Sum(i => i.TotalInventoryCount);
            siteLoadList.TotalItemCount = loadlists.Sum(i => i.TotalItemCount);
            siteLoadList.TotalOrderCount = loadlists.Sum(i => i.TotalOrderCount);
            return siteLoadList;
        }

        private async Task<List<VehicleLoadlist>> GetTranferRequestLoadList(IEnumerable<Data.Models.DispatchInstructions> dispatchInstructions)
        {
            var loadlists = new List<VehicleLoadlist>();
            var transferRequestIds = dispatchInstructions.Where(d => d.TransferRequestId != null)
                                                         .Select(d => d.TransferRequestId);
            var tranferRequests = await _itemTransferRequestRepository.GetManyAsync(tr => transferRequestIds.Contains(tr.Id));
            if (tranferRequests.Count() > 0)
            {

                var transferRequestIdWithVehicle = dispatchInstructions.GroupBy(i => i.VehicleId)
                                                        .Select(d => new
                                                        {
                                                            VehicleId = d.Key,
                                                            Vehicle = d.FirstOrDefault().Vehicle,
                                                            TransferRequestIds = d.Where(dd => dd.TransferRequestId != null)
                                                                                    .Select(dd => dd.TransferRequestId)
                                                        });
                foreach (var vehicleTransferRequest in transferRequestIdWithVehicle)
                {

                    var transferRequestToLoad = tranferRequests.Where(tr => vehicleTransferRequest.TransferRequestIds.Contains(tr.Id));
                    var groupedLineItems = transferRequestToLoad.Select(l =>
                                                                            new Data.Models.OrderLineItems()
                                                                            {
                                                                                ItemId = l.ItemId,
                                                                                Item = l.Item,
                                                                                ItemCount = l.ItemCount
                                                                            });
                    var items = _mapper.Map<IEnumerable<LoadlistItem>>(groupedLineItems);
                    loadlists.Add(new VehicleLoadlist()
                    {
                        VehicleId = vehicleTransferRequest.VehicleId,
                        Vehicle = _mapper.Map<Vehicle>(vehicleTransferRequest.Vehicle),
                        Items = items,
                        TotalItemCount = items.Count(),
                        TotalInventoryCount = items.Sum(p => p.ItemCount),
                        TotalOrderCount = transferRequestToLoad.Count(),
                        TransferRequests = _mapper.Map<IEnumerable<ItemTransferRequest>>(transferRequestToLoad)
                    });
                }
            }
            return loadlists;
        }

        private async Task<List<VehicleLoadlist>> GetOrderLoadList(IEnumerable<Data.Models.DispatchInstructions> dispatchInstructions, IEnumerable<VehicleLoadlist> vehicleLoadlists)
        {
            var loadlists = vehicleLoadlists.ToList();
            var orderHeaderIds = dispatchInstructions.Where(d => d.OrderHeaderId != null)
                                                     .Select(d => d.OrderHeaderId);

            var orders = await _orderHeadersRepository.GetDispatchOrderAsync(o => orderHeaderIds.Contains(o.Id));
            if (orders.Count() > 0)
            {
                var orderHeaderIdWithVehicle = dispatchInstructions.GroupBy(i => i.VehicleId)
                                                    .Select(d => new
                                                    {
                                                        VehicleId = d.Key,
                                                        Vehicle = d.FirstOrDefault().Vehicle,
                                                        OrderIds = d.Where(dd => dd.OrderHeaderId != null)
                                                                    .Select(dd => dd.OrderHeaderId)
                                                    });

                foreach (var vehicleOrder in orderHeaderIdWithVehicle)
                {

                    var ordersToLoad = orders.Where(o => vehicleOrder.OrderIds.Contains(o.Id));
                    var groupedLineItems = ordersToLoad.SelectMany(o => o.OrderLineItems)
                                                        .Where(l => l.ActionId == (int)OrderTypes.Delivery)
                                                        .GroupBy(l => l.ItemId)
                                                        .Select(l =>
                                                            new Data.Models.OrderLineItems()
                                                            {
                                                                ItemId = l.Key,
                                                                Item = l.FirstOrDefault().Item,
                                                                ItemCount = l.Sum(lg => lg.ItemCount)
                                                            });
                    var vehicleLoadList = loadlists.FirstOrDefault(l => l.VehicleId == vehicleOrder.VehicleId);
                    if (vehicleLoadList != null)
                    {
                        var existingItems = vehicleLoadList.Items.ToList();
                        existingItems.AddRange(_mapper.Map<IEnumerable<LoadlistItem>>(groupedLineItems));
                        vehicleLoadList.TotalItemCount = existingItems.Count();
                        vehicleLoadList.TotalInventoryCount = existingItems.Sum(p => p.ItemCount);
                        vehicleLoadList.TotalOrderCount += ordersToLoad.Count();
                        vehicleLoadList.Orders = _mapper.Map<IEnumerable<OrderHeader>>(ordersToLoad);
                        vehicleLoadList.Items = existingItems;
                    }
                    else
                    {
                        var vehicle = await _vehiclesService.GetVehicleById(vehicleOrder.VehicleId);
                        var items = _mapper.Map<IEnumerable<LoadlistItem>>(groupedLineItems);
                        loadlists.Add(new VehicleLoadlist()
                        {
                            VehicleId = vehicleOrder.VehicleId,
                            Vehicle = vehicle,
                            Items = items,
                            TotalItemCount = items.Count(),
                            TotalInventoryCount = items.Sum(p => p.ItemCount),
                            TotalOrderCount = ordersToLoad.Count(),
                            Orders = _mapper.Map<IEnumerable<OrderHeader>>(ordersToLoad)
                        });
                    }
                }
            }
            return loadlists;
        }

        private async Task<int> GetNetSuiteWarehouseIdByLocation(int locationId)
        {
            var siteModel = await _sitesRepository.GetByIdAsync(locationId);

            if (siteModel.NetSuiteLocationId == null)
            {
                throw new ValidationException($"Netsuite warehouseId is not found for site with Id({siteModel.Id})");
            }
            return siteModel.NetSuiteLocationId ?? 0;
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

        private async Task ValidateDriverAndSiteMember(int? vehicleId, bool allowDispatchToSiteMember)
        {
            var loggedInUserId = GetLoggedInUserId();

            var driverModel = await _driverRepository.GetAsync(d => d.UserId == loggedInUserId);
            if (driverModel != null)
            {
                if (driverModel.CurrentVehicleId == null)
                {
                    throw new ValidationException($"Current vehicle is not assigned to driver");
                }

                if (vehicleId != driverModel.CurrentVehicleId)
                {
                    throw new ValidationException($"This dispatch is not assigned to logged in driver");
                }
            }

            else if (allowDispatchToSiteMember)
            {
                var isSiteMemberUser = await _siteMemberRepository.ExistsAsync(sm => sm.UserId == loggedInUserId);
                if (!isSiteMemberUser)
                {
                    throw new ValidationException($"Pickup/drop is not allowed for non-driver/non-site-member user");
                }
            }
            else
            {
                throw new ValidationException($"Pickup/drop is not allowed for non-driver user");
            }
        }

        private IEnumerable<RouteItem> GetDispatchRoutes(IEnumerable<Data.Models.OrderHeaders> orderHeaders)
        {
            var routeItems = new List<RouteItem>();
            foreach (var order in orderHeaders)
            {
                var locationAddress = new Data.Models.Addresses();
                if (order.OrderTypeId == (int)Data.Enums.OrderTypes.Delivery
                    || order.OrderTypeId == (int)Data.Enums.OrderTypes.Respite
                    || order.OrderTypeId == (int)Data.Enums.OrderTypes.Exchange)
                {
                    locationAddress = order.DeliveryAddress;
                }
                else if (order.OrderTypeId == (int)Data.Enums.OrderTypes.Pickup)
                {
                    locationAddress = order.PickupAddress;
                }
                else if (order.OrderTypeId == (int)Data.Enums.OrderTypes.Patient_Move)
                {
                    locationAddress = order.PickupAddress;
                }
                else
                {
                    locationAddress = null;
                }
                routeItems.Add(new RouteItem()
                {
                    SequenceNumber = order.DispatchInstructions.SequenceNumber ?? 0,
                    OrderHeaderId = order.Id,
                    Address = _mapper.Map<Common.ViewModels.Address>(locationAddress)
                });
                if (order.OrderTypeId == (int)Data.Enums.OrderTypes.Patient_Move)
                {
                    routeItems.Add(new RouteItem()
                    {
                        SequenceNumber = order.DispatchInstructions.SequenceNumber ?? 0,
                        OrderHeaderId = order.Id,
                        Address = _mapper.Map<Common.ViewModels.Address>(order.DeliveryAddress)
                    });
                }
            }
            return routeItems;
        }

        private async Task LogDispatchRecordUpdate(NSSDKViewModels.NetSuiteHMSDispatchResponse dispatchRecords, IEnumerable<ViewModels.DispatchRecordUpdateRequest> dispatchRecordUpdateRequests)
        {
            var dispatchAuditLogs = new List<Data.Models.DispatchAuditLog>();
            foreach (var dispatchRecordUpdateRequest in dispatchRecordUpdateRequests)
            {
                var dispatchRecordChangeLogs = new List<EventEntryChange>();
                var originalDispatchRecord = dispatchRecords.Results.FirstOrDefault(dr => dr.NSDispatchId.HasValue && dispatchRecordUpdateRequest.DispatchRecordId == dr.NSDispatchId.Value);
                if (originalDispatchRecord != null)
                {
                    #region Delivery Date
                    var DeliveryDateChangeLog = CompareAndReturnLogData("HmsDeliveryDate", originalDispatchRecord.HmsDeliveryDate, dispatchRecordUpdateRequest.HmsDeliveryDate);
                    if (DeliveryDateChangeLog != null)
                    {
                        dispatchRecordChangeLogs.Add(DeliveryDateChangeLog);
                    }
                    #endregion

                    #region Pickup Request Date
                    var PickupReqDateChangeLog = CompareAndReturnLogData("HmsPickupRequestDate", originalDispatchRecord.HmsPickupRequestDate, dispatchRecordUpdateRequest.HmsPickupRequestDate);
                    if (PickupReqDateChangeLog != null)
                    {
                        dispatchRecordChangeLogs.Add(PickupReqDateChangeLog);
                    }
                    #endregion

                    #region Pickup Date
                    var PickupDateChangeLog = CompareAndReturnLogData("PickupDate", originalDispatchRecord.PickupDate, dispatchRecordUpdateRequest.PickupDate);
                    if (PickupDateChangeLog != null)
                    {
                        dispatchRecordChangeLogs.Add(PickupDateChangeLog);
                    }
                    #endregion

                    var patientUuidString = dispatchRecords.Results.FirstOrDefault(dr => dr.NSDispatchId.HasValue && dispatchRecordUpdateRequest.DispatchRecordId == dr.NSDispatchId.Value)?.PatientGuid;
                    Guid? patientUuid = null;
                    if (!string.IsNullOrEmpty(patientUuidString))
                    {
                        patientUuid = new Guid(patientUuidString);
                    }

                    if (dispatchRecordChangeLogs.Any())
                    {
                        dispatchAuditLogs.Add(new Data.Models.DispatchAuditLog()
                        {
                            AuditAction = "Update",
                            AuditData = JsonConvert.SerializeObject(dispatchRecordChangeLogs),
                            AuditDate = DateTime.UtcNow,
                            EntityId = dispatchRecordUpdateRequest.DispatchRecordId,
                            PatientUuid = patientUuid,
                            UserId = GetLoggedInUserId(),
                            ClientIpaddress = _httpContext.Connection.RemoteIpAddress.ToString()
                        });
                    }
                }
            }

            if (dispatchAuditLogs.Any())
            {
                await _dispatchAuditLogRepository.AddManyAsync(dispatchAuditLogs);
            }
        }

        private EventEntryChange CompareAndReturnLogData(string columnName, DateTime? originalValue, DateTime? newValue)
        {
            if (newValue.HasValue && (!originalValue.HasValue || originalValue != newValue))
            {
                return new EventEntryChange()
                {
                    ColumnName = columnName,
                    OriginalValue = originalValue,
                    NewValue = newValue
                };
            }
            return null;
        }

    }
}
