using AutoMapper;
using HMSDigital.Common.BusinessLayer;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using HospiceSource.Digital.Patient.SDK.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationSDK.Interfaces;
using NotificationSDK.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OrderHeaderStatusTypes = HMSDigital.Core.Data.Enums.OrderHeaderStatusTypes;
using OrderLineItemStatusTypes = HMSDigital.Core.Data.Enums.OrderLineItemStatusTypes;
using OrderTypes = HMSDigital.Core.Data.Enums.OrderTypes;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class OrderHeadersService : IOrderHeadersService
    {
        private readonly IOrderHeadersRepository _orderHeadersRepository;

        private readonly IOrderLineItemsRepository _lineItemsRepository;

        private readonly IHospiceRepository _hospiceRepository;

        private readonly IHospiceMemberRepository _hospiceMemberRepository;

        private readonly IHospiceLocationRepository _hospiceLocationRepository;

        private readonly IItemRepository _itemRepository;

        private readonly IMapper _mapper;

        private readonly IUsersRepository _usersRepository;

        private readonly IAddressRepository _addressRepository;

        private readonly ISiteServiceAreaRepository _siteServiceAreaRepository;

        private readonly IAddressStandardizerService _addressStandardizerService;

        private readonly IOrderFulfillmentLineItemsRepository _orderFulfillmentLineItemsRepository;

        private readonly IDispatchInstructionsRepository _dispatchInstructionsRepository;

        private readonly IPaginationService _paginationService;

        private readonly IPatientService _patientService;

        private readonly IDispatchService _dispatchService;

        private readonly ISitesService _sitesService;

        private readonly INotificationService _notificationService;

        private readonly HttpContext _httpContext;

        private readonly AWSConfig _awsConfig;

        private readonly ILogger<OrderHeadersService> _logger;

        private readonly IPatientInventoryRepository _patientInventoryRepository;

        public OrderHeadersService(IOrderHeadersRepository orderHeadersRepository,
            IOrderLineItemsRepository lineItemsRepository,
            IHospiceRepository hospiceRepository,
            IHospiceMemberRepository hospiceMemberRepository,
            IHospiceLocationRepository hospiceLocationRepository,
            IItemRepository itemRepository,
            IUsersRepository usersRepository,
            IHttpContextAccessor httpContextAccessor,
            IAddressRepository addressRepository,
            ISiteServiceAreaRepository siteServiceAreaRepository,
            IAddressStandardizerService addressStandardizerService,
            IOrderFulfillmentLineItemsRepository orderFulfillmentLineItemsRepository,
            IDispatchInstructionsRepository dispatchInstructionsRepository,
            IPaginationService paginationService,
            IPatientService patientService,
            ISitesService sitesService,
            IDispatchService dispatchService,
            INotificationService notificationService,
            IMapper mapper,
            IOptions<AWSConfig> awsConfigOptions,
            ILogger<OrderHeadersService> logger,
            IPatientInventoryRepository patientInventoryRepository)
        {
            _orderHeadersRepository = orderHeadersRepository;
            _lineItemsRepository = lineItemsRepository;
            _hospiceRepository = hospiceRepository;
            _hospiceMemberRepository = hospiceMemberRepository;
            _hospiceLocationRepository = hospiceLocationRepository;
            _itemRepository = itemRepository;
            _mapper = mapper;
            _usersRepository = usersRepository;
            _addressRepository = addressRepository;
            _siteServiceAreaRepository = siteServiceAreaRepository;
            _addressStandardizerService = addressStandardizerService;
            _orderFulfillmentLineItemsRepository = orderFulfillmentLineItemsRepository;
            _dispatchInstructionsRepository = dispatchInstructionsRepository;
            _paginationService = paginationService;
            _patientService = patientService;
            _sitesService = sitesService;
            _dispatchService = dispatchService;
            _notificationService = notificationService;
            _httpContext = httpContextAccessor.HttpContext;
            _awsConfig = awsConfigOptions.Value;
            _logger = logger;
            _patientInventoryRepository = patientInventoryRepository;
        }

        public async Task<PaginatedList<OrderHeader>> GetAllOrderHeaders(SieveModel sieveModel, bool includeFulfillmentDetails, int? locationId = null)
        {
            _orderHeadersRepository.SieveModel = sieveModel;
            var orderPredicate = await GetOrderPredicate(locationId);
            var totalRecords = await _orderHeadersRepository.GetCountAsync(orderPredicate);
            var orderHeaderModels = await _orderHeadersRepository.GetManyAsync(orderPredicate);
            if (includeFulfillmentDetails)
            {
                var orderheaderIds = orderHeaderModels.Select(o => o.Id);
                var orderFulfillments = await _orderFulfillmentLineItemsRepository.GetManyAsync(o => orderheaderIds.Contains(o.OrderHeaderId));
                foreach (var orderHeaderModel in orderHeaderModels)
                {
                    orderHeaderModel.OrderFulfillmentLineItems = orderFulfillments.Where(ofl => ofl.OrderHeaderId == orderHeaderModel.Id).ToList();
                }
            }
            var orderHeaders = _mapper.Map<IEnumerable<OrderHeader>>(orderHeaderModels);

            return _paginationService.GetPaginatedList(orderHeaders, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<OrderHeader> GetOrderHeaderById(int orderHeaderId, bool includeFulfillmentDetails)
        {
            var orderPredicate = await GetOrderPredicate();
            orderPredicate.And(o => o.Id == orderHeaderId);
            var orderHeaderModel = await _orderHeadersRepository.GetAsync(orderPredicate);
            if (orderHeaderModel == null)
            {
                throw new ValidationException($"Order header Id({orderHeaderId}) is not valid");
            }

            if (includeFulfillmentDetails)
            {
                return await IncludeFulfillmentDetails(orderHeaderModel);
            }
            return _mapper.Map<OrderHeader>(orderHeaderModel);
        }

        public async Task<OrderHeader> UpsertOrderFromWebPortal(OrderHeaderRequest orderHeaderRequest)
        {
            var existingOrderModel = await _orderHeadersRepository.GetByIdAsync(orderHeaderRequest.Id);
            if (existingOrderModel != null)
            {
                return await UpdateOrderFromWebPortal(existingOrderModel, orderHeaderRequest);
            }
            return await CreateOrderFromWebPortal(orderHeaderRequest);
        }

        public async Task<PaginatedList<OrderFulfillmentLineItem>> GetOrderFulfillments(int orderHeaderId, SieveModel sieveModel)
        {
            _orderFulfillmentLineItemsRepository.SieveModel = sieveModel;
            var fulfillmentPredicate = await GetOrderFulfillmentPredicate();
            var predicate = fulfillmentPredicate.And(o => o.OrderHeaderId == orderHeaderId);
            var totalRecords = await _orderFulfillmentLineItemsRepository.GetCountAsync(predicate);
            var fulfillmentModels = await _orderFulfillmentLineItemsRepository.GetManyAsync(predicate);
            var fulfillments = _mapper.Map<IEnumerable<OrderFulfillmentLineItem>>(fulfillmentModels);

            return _paginationService.GetPaginatedList(fulfillments, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<bool> AssignSiteToOrder(Data.Models.OrderHeaders orderModel)
        {
            int? siteIdByHospice;
            var hospiceLocationModel = await _hospiceLocationRepository.GetAsync(h => orderModel.NetSuiteCustomerId == h.NetSuiteCustomerId);
            if (hospiceLocationModel == null)
            {
                var hospiceModel = await _hospiceRepository.GetAsync(h => orderModel.NetSuiteCustomerId == h.NetSuiteCustomerId);
                siteIdByHospice = hospiceModel.AssignedSiteId;
            }
            else
            {
                siteIdByHospice = hospiceLocationModel.SiteId;
            }

            orderModel.SiteId = await GetAssignedSiteIdForOrder(orderModel, siteIdByHospice);

            if (orderModel.SiteId.HasValue)
            {
                await _orderHeadersRepository.UpdateAsync(orderModel);
                return true;
            }

            return false;
        }

        public async Task<OrderHeader> UpsertOrderNotes(int orderHeaderId, IEnumerable<UpdateOrderNotesRequest> orderNotes)
        {

            var orderModel = await _orderHeadersRepository.GetByIdAsync(orderHeaderId);

            if (orderModel == null)
            {
                throw new ValidationException($"Order header Id({orderHeaderId}) is not valid");
            }
            var existingOrderNotes = orderModel.OrderNotes.ToList();
            await UpsertWebportalOrderNotes(orderModel, existingOrderNotes, orderNotes);
            await _orderHeadersRepository.UpdateAsync(orderModel);
            return _mapper.Map<OrderHeader>(orderModel);
        }

        private async Task UpsertWebportalOrderNotes(OrderHeaders existingOrder, List<OrderNotes> existingOrderNotes, IEnumerable<UpdateOrderNotesRequest> orderNotesRequest)
        {
            var memberIds = orderNotesRequest.Where(m => m.HospiceMemberId.HasValue).Select(m => m.HospiceMemberId.Value);
            var hospiceMemberModels = await _hospiceMemberRepository.GetManyAsync(h => memberIds.Contains(h.Id));
            var invalidMemberIds = memberIds.Except(hospiceMemberModels.Select(m => m.Id));
            if (invalidMemberIds != null && invalidMemberIds.Count() > 0)
            {
                throw new ValidationException($"Hospice member with Id: ({string.Join(",", invalidMemberIds)}) are invalid.");
            }
            List<OrderNotes> orderNotes = null;
            if (orderNotesRequest != null && orderNotesRequest.Count() > 0)
            {
                orderNotes = new List<OrderNotes>();
                foreach (var orderNote in orderNotesRequest)
                {
                    var orderNoteToUpdate = existingOrderNotes.FirstOrDefault(x => orderNote.Id.HasValue && x.Id == orderNote.Id.Value);
                    orderNoteToUpdate = _mapper.Map(orderNote, orderNoteToUpdate);
                    if (!orderNoteToUpdate.NetSuiteContactId.HasValue && orderNote.HospiceMemberId.HasValue)
                    {
                        orderNoteToUpdate.NetSuiteContactId = hospiceMemberModels.FirstOrDefault(m => m.Id == orderNote.HospiceMemberId).NetSuiteContactId;
                    }
                    orderNotes.Add(orderNoteToUpdate);
                }
            }
            existingOrder.OrderNotes = orderNotes;
        }

        private async Task<OrderHeader> CreateOrderFromWebPortal(OrderHeaderRequest orderHeaderRequest)
        {
            var orderModel = _mapper.Map<Data.Models.OrderHeaders>(orderHeaderRequest);
            await ValidateAndAssignedHospiceAndSite(orderModel, orderHeaderRequest);

            if (orderModel.DeliveryAddress != null)
            {
                await ValidateAddress(orderModel.DeliveryAddress);
            }
            if (orderModel.PickupAddress != null)
            {
                await ValidateAddress(orderModel.PickupAddress);
            }
            await AssignOrderNotes(orderModel, orderHeaderRequest.OrderNotes);
            await ValidateWebPortalOrderLineItem(orderModel, orderHeaderRequest);

            orderModel.DispatchStatusId = (int)OrderHeaderStatusTypes.Planned;
            orderModel.OrderDateTime = DateTime.UtcNow;

            await _orderHeadersRepository.AddAsync(orderModel);

            if (orderModel.PatientUuid != Guid.Empty)
            {
                var hasDmeEquipment = await _patientInventoryRepository.ExistsAsync(p => p.PatientUuid.Equals(orderModel.PatientUuid) && p.Item.IsDme);
                _patientService.RecordPatientOrder(orderModel.PatientUuid, orderModel.OrderNumber, hasDmeEquipment);
            }

            var orderNotification = _mapper.Map<OrderNotification>(orderModel);
            if (orderHeaderRequest.HospiceMemberId.HasValue) 
            {
                var hospiceMember = await _hospiceMemberRepository.GetByIdAsync(orderHeaderRequest.HospiceMemberId.Value);
                orderNotification.MemberName = $"{hospiceMember?.User?.FirstName} {hospiceMember?.User?.LastName}";
            }

            var loggedInUser = await _usersRepository.GetByIdAsync(GetLoggedInUserId());
            if (loggedInUser != null)
            {
                orderNotification.OrderDashboardUrl = $"{_awsConfig.RedirectUri}/dashboard?view=open";
                _notificationService.SendOrderCreatedNotification(loggedInUser.Email, orderNotification);
            }
            return _mapper.Map<OrderHeader>(orderModel);
        }

        private async Task RecordOrderFulfillment(OrderHeaders orderModel)
        {
            if (orderModel.StatusId != (int)OrderHeaderStatusTypes.Cancelled &&
                orderModel.StatusId != (int)OrderHeaderStatusTypes.Completed &&
                orderModel.StatusId != (int)OrderHeaderStatusTypes.Planned &&
                orderModel.StatusId != (int)OrderHeaderStatusTypes.Partial_Fulfillment &&
                !(orderModel.IsExceptionFulfillment && orderModel.StatusId == (int)OrderHeaderStatusTypes.Partial_Fulfillment))
            {
                return;
            }

            var hasInventory = await _patientInventoryRepository.ExistsAsync(x => x.PatientUuid.Equals(orderModel.PatientUuid) && x.Item.IsDme && !x.IsExceptionFulfillment);

            var hasOpenOrders = await _orderHeadersRepository.ExistsAsync(x => x.PatientUuid == orderModel.PatientUuid &&
                                                                               x.StatusId != (int)OrderHeaderStatusTypes.Completed
                                                                               && x.StatusId != (int)OrderHeaderStatusTypes.Cancelled
                                                                               && !(x.IsExceptionFulfillment && x.StatusId == (int)OrderHeaderStatusTypes.Partial_Fulfillment));

            _patientService.RecordOrderFulfillment(orderModel.PatientUuid, orderModel.Status?.Name, orderModel.PickupReason, hasInventory, hasOpenOrders);
        }

        private async Task<OrderHeader> UpdateOrderFromWebPortal(Data.Models.OrderHeaders existingOrder, OrderHeaderRequest orderHeaderRequest)
        {
            var existingStatusId = existingOrder.StatusId;
            var existingOrderLineItems = existingOrder.OrderLineItems.ToList();
            var existingOrderNotes = existingOrder.OrderNotes.ToList();
            var orderModel = _mapper.Map(orderHeaderRequest, existingOrder);
            await ValidateAndAssignedHospiceAndSite(orderModel, orderHeaderRequest);

            if (orderModel.DeliveryAddress != null)
            {
                await ValidateAddress(orderModel.DeliveryAddress);
            }
            if (orderModel.PickupAddress != null)
            {
                await ValidateAddress(orderModel.PickupAddress);
            }
            await UpsertWebportalOrderNotes(orderModel, existingOrderNotes, orderHeaderRequest.OrderNotes);
            await ValidateWebPortalOrderLineItem(orderModel, orderHeaderRequest);

            var deletedOrderLineItemsIds = existingOrderLineItems.Select(l => l.Id).Except(orderModel.OrderLineItems.Select(o => o.Id));

            var fulfilledOrderLineItem = existingOrderLineItems.Where(l => deletedOrderLineItemsIds.Contains(l.Id)
                                                                        && (l.StatusId == (int)OrderLineItemStatusTypes.Completed
                                                                            || l.StatusId == (int)OrderLineItemStatusTypes.Partial_Fulfillment));

            if (fulfilledOrderLineItem != null && fulfilledOrderLineItem.Count() > 0)
            {
                var fulfilledNetSuiteLineItemIds = string.Join(",", fulfilledOrderLineItem.Select(l => l.NetSuiteOrderLineItemId));
                throw new ValidationException($"Order line items {fulfilledNetSuiteLineItemIds} cannot be deleted as those are already fulfilled");
            }
            if (orderModel.OrderLineItems == null || orderModel.OrderLineItems.Count() == 0)
            {
                await _dispatchService.UnassignOrder(orderModel.Id);
                orderModel.StatusId = (int)OrderHeaderStatusTypes.Cancelled;
                orderModel.DispatchStatusId = (int)OrderHeaderStatusTypes.Cancelled;
                orderModel.FulfillmentEndDateTime ??= DateTime.UtcNow;
            }
            else if (orderModel.OrderLineItems.All(l => l.StatusId == (int)OrderLineItemStatusTypes.Completed))
            {
                orderModel.IsExceptionFulfillment = false;
                orderModel.StatusId = (int)OrderHeaderStatusTypes.Completed;
                orderModel.DispatchStatusId = (int)OrderHeaderStatusTypes.Completed;
                orderModel.FulfillmentEndDateTime ??= DateTime.UtcNow;
            }
            else if (orderModel.OrderLineItems.Any(l => l.StatusId == (int)OrderLineItemStatusTypes.Completed || l.StatusId == (int)OrderLineItemStatusTypes.Partial_Fulfillment))
            {
                orderModel.StatusId = (int)OrderHeaderStatusTypes.Partial_Fulfillment;
            }
            else if (orderModel.StatusId == (int)OrderHeaderStatusTypes.Completed && orderModel.OrderLineItems.All(l => l.StatusId == (int)OrderLineItemStatusTypes.Planned))
            {
                orderModel.StatusId = (int)OrderHeaderStatusTypes.Planned;
                orderModel.DispatchStatusId = (int)OrderHeaderStatusTypes.Planned;
            }

            await _orderHeadersRepository.UpdateAsync(orderModel);

            if (orderModel.StatusId != existingStatusId)
            {
                await RecordOrderFulfillment(orderModel);
            }

            return _mapper.Map<OrderHeader>(orderModel);
        }

        private async Task ValidateAndAssignedHospiceAndSite(Data.Models.OrderHeaders orderModel, OrderHeaderRequest orderHeaderRequest)
        {
            var hospiceMemberModel = await _hospiceMemberRepository.GetAsync(h => h.Id == orderHeaderRequest.HospiceMemberId
                                                                                  || h.HospiceLocationMembers.Any(hl => hl.HospiceMemberId == orderHeaderRequest.HospiceMemberId));
            if (hospiceMemberModel == null && orderModel.NetSuiteOrderId == null)
            {
                throw new ValidationException($"Hospice member with Id({orderHeaderRequest.HospiceMemberId}) does not exist");
            }
            orderModel.NetSuiteCustomerContactId = hospiceMemberModel?.NetSuiteContactId;
            int? siteIdByHospice;
            var hospiceLocationModel = await _hospiceLocationRepository.GetByIdAsync(orderHeaderRequest.HospiceLocationId);
            if (hospiceLocationModel == null)
            {
                var hospiceModel = await _hospiceRepository.GetByIdAsync(orderHeaderRequest.HospiceId);
                if (hospiceModel == null)
                {
                    throw new ValidationException($"hospice with Id({orderHeaderRequest.HospiceId}) does not exist");
                }
                siteIdByHospice = hospiceModel.AssignedSiteId;
                orderModel.NetSuiteCustomerId = hospiceModel.NetSuiteCustomerId;
            }
            else
            {
                orderModel.HospiceId = hospiceLocationModel.HospiceId;
                siteIdByHospice = hospiceLocationModel.SiteId;
                orderModel.NetSuiteCustomerId = hospiceLocationModel.NetSuiteCustomerId;
            }

            orderModel.SiteId = await GetAssignedSiteIdForOrder(orderModel, siteIdByHospice);

            if (hospiceMemberModel !=  null)
            {
                orderModel.OrderRecipientUserId = hospiceMemberModel.UserId;
            }
        }

        private async Task ValidateWebPortalOrderLineItem(Data.Models.OrderHeaders orderModel, OrderHeaderRequest orderHeaderRequest)
        {
            var orderItemIds = orderModel.OrderLineItems.Where(lt => lt.ItemId != null).Select(l => l.ItemId.Value);
            var itemModels = await _itemRepository.GetManyAsync(p => orderItemIds.Contains(p.Id));
            var modelItemIds = itemModels.Select(it => it.Id);
            var invalidItemIds = orderItemIds.Except(modelItemIds);
            if (invalidItemIds != null && invalidItemIds.Count() > 0)
            {
                throw new ValidationException($"Item with Id: ({string.Join(",", invalidItemIds)}) are not present in HMS.");
            }

            var orderLineItems = new List<Data.Models.OrderLineItems>();
            foreach (var lineItem in orderHeaderRequest.OrderLineItems)
            {
                var existingLineItemModel = await _lineItemsRepository.GetAsync(l => l.Id == lineItem.Id);
                var lineItemModel = _mapper.Map(lineItem, existingLineItemModel);
                lineItemModel.NetSuiteItemId = itemModels.FirstOrDefault(p => p.Id == lineItem.ItemId).NetSuiteItemId;

                lineItemModel.DeliveryAddress = orderModel.DeliveryAddress;
                lineItemModel.PickupAddress = orderModel.PickupAddress;

                lineItemModel.SiteId = orderModel.SiteId;
                if (lineItemModel.StatusId == null)
                {
                    lineItemModel.StatusId = (int)OrderLineItemStatusTypes.Planned;
                    lineItemModel.DispatchStatusId = (int)OrderLineItemStatusTypes.Planned;
                }
                lineItemModel.RequestedStartDateTime = orderModel.RequestedStartDateTime;
                lineItemModel.RequestedEndDateTime = orderModel.RequestedEndDateTime;

                if (!Enum.TryParse(lineItem.Action, true, out OrderTypes action) || !(action == OrderTypes.Delivery || action == OrderTypes.Pickup))
                {
                    throw new ValidationException($"action ({lineItem.Action}) is not valid");
                }
                lineItemModel.ActionId = (int)action;

                if (lineItemModel.Id > 0)
                {
                    var fulfillmentLineItems = await _orderFulfillmentLineItemsRepository.GetManyAsync(l => l.OrderLineItemId == lineItemModel.Id
                                                                                                        && l.OrderHeaderId == orderModel.Id);
                    if (fulfillmentLineItems != null && fulfillmentLineItems.Count() > 0)
                    {
                        var fulfilledItemCount = fulfillmentLineItems.Sum(l => l.Quantity);
                        if (lineItemModel.ItemCount <= fulfilledItemCount)
                        {
                            lineItemModel.StatusId = (int)OrderLineItemStatusTypes.Completed;
                            lineItemModel.DispatchStatusId = (int)OrderLineItemStatusTypes.Completed;
                        }

                        else if (fulfilledItemCount > 0 && lineItemModel.ItemCount > fulfilledItemCount)
                        {
                            lineItemModel.StatusId = (int)OrderLineItemStatusTypes.Partial_Fulfillment;
                        }
                    }
                }
                orderLineItems.Add(lineItemModel);
            }
            orderModel.OrderLineItems = orderLineItems;
        }

        private async Task<Data.Models.Addresses> ValidateAddress(Data.Models.Addresses address)
        {
            if (address.AddressUuid != Guid.Empty)
            {
                var existingAddress = await _addressRepository.GetAsync(a => a.AddressUuid == address.AddressUuid);
                if (existingAddress != null)
                {
                    address = existingAddress;
                    return address;
                }

                var patientAddress = await _patientService.GetPatientAddressByUUID(address.AddressUuid);
                if (patientAddress != null)
                {
                    patientAddress.Id = address.Id;
                    address = _mapper.Map(patientAddress, address);
                    return address;
                }

            }
            try
            {
                var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(address));
                _mapper.Map(standardizedAddress, address);
            }
            catch (ValidationException vx)
            {
                _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
            }
            if (address.AddressUuid == Guid.Empty)
            {
                address.AddressUuid = Guid.NewGuid();
            }
            return address;
        }

        private async Task<int?> GetAssignedSiteIdForOrder(Data.Models.OrderHeaders orderModel, int? siteIdByHospice)
        {
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

            var siteServiceArea = await _siteServiceAreaRepository.GetAsync(s => s.ZipCode != null && serviceZipCode == s.ZipCode);

            var siteIdBySerivceArea = siteServiceArea?.SiteId;
            if (siteIdBySerivceArea == null)
            {
                return siteIdByHospice;
            }
            return siteIdBySerivceArea;
        }

        private async Task AssignOrderNotes(Data.Models.OrderHeaders orderModel, IEnumerable<OrderNotesRequest> orderNotesRequests)
        {
            List<Data.Models.OrderNotes> orderNotes = null;
            if (orderNotesRequests != null && orderNotesRequests.Count() > 0)
            {
                var memberIds = orderNotesRequests.Where(m => m.HospiceMemberId.HasValue).Select(m => m.HospiceMemberId.Value);
                var hospiceMemberModels = await _hospiceMemberRepository.GetManyAsync(h => memberIds.Contains(h.Id));
                var invalidMemberIds = memberIds.Except(hospiceMemberModels.Select(m => m.Id));
                if (invalidMemberIds != null && invalidMemberIds.Count() > 0)
                {
                    throw new ValidationException($"Hospice member with Id: ({string.Join(",", invalidMemberIds)}) are invalid.");
                }
                orderNotes = new List<Data.Models.OrderNotes>();
                foreach (var orderNotesRequest in orderNotesRequests)
                {
                    orderNotes.Add(new Data.Models.OrderNotes
                    {
                        Note = orderNotesRequest.Note,
                        NetSuiteContactId = hospiceMemberModels.FirstOrDefault(m => m.Id == orderNotesRequest.HospiceMemberId).NetSuiteContactId,
                        HospiceMemberId = orderNotesRequest.HospiceMemberId
                    });
                }
            }

            orderModel.OrderNotes = orderNotes;
        }

        private async Task<ExpressionStarter<Data.Models.OrderHeaders>> GetOrderPredicate(int? locationId = null)
        {
            var orderPredicate = PredicateBuilder.New<Data.Models.OrderHeaders>(false);
            var orderSiteIdPredicate = PredicateBuilder.New<Data.Models.OrderHeaders>(true);

            if (locationId.HasValue)
            {
                var siteIds = await _sitesService.GetSiteIdsByParentLocationId(locationId.Value);
                orderSiteIdPredicate.And(o => o.SiteId != null && siteIds.Contains(o.SiteId.Value));
            }

            var userId = GetLoggedInUserId();
            var hospiceIds = await _usersRepository.GetHospiceAccessByUserId(userId);
            if (hospiceIds.Contains("*"))
            {
                return orderPredicate.Or(o => true).And(orderSiteIdPredicate);
            }
            orderPredicate.Or(o => hospiceIds.Contains(o.HospiceId.ToString()));

            var hospiceLocationIds = await _usersRepository.GetHospiceLocationAccessByUserId(userId);
            if (hospiceLocationIds.Contains("*"))
            {
                return orderPredicate.Or(o => o.HospiceLocationId != null).And(orderSiteIdPredicate);
            }
            return orderPredicate.Or(o => hospiceLocationIds.Contains(o.HospiceLocationId.ToString()))
                                    .And(orderSiteIdPredicate);
        }

        private async Task<ExpressionStarter<Data.Models.OrderFulfillmentLineItems>> GetOrderFulfillmentPredicate(bool isSystemCall = false)
        {
            var fulfillmentPredicate = PredicateBuilder.New<Data.Models.OrderFulfillmentLineItems>(false);
            if (isSystemCall)
            {
                return fulfillmentPredicate.Or(f => true);
            }
            var userId = GetLoggedInUserId();
            var hospiceIds = await _usersRepository.GetHospiceAccessByUserId(userId);
            if (hospiceIds.Contains("*"))
            {
                return fulfillmentPredicate.Or(f => true);
            }
            fulfillmentPredicate.Or(f => hospiceIds.Contains(f.OrderLineItem.OrderHeader.HospiceId.ToString()));

            var hospiceLocationIds = await _usersRepository.GetHospiceLocationAccessByUserId(userId);
            if (hospiceLocationIds.Contains("*"))
            {
                return fulfillmentPredicate.Or(f => f.OrderLineItem.OrderHeader.HospiceLocationId != null);
            }
            fulfillmentPredicate.Or(f => hospiceLocationIds.Contains(f.OrderLineItem.OrderHeader.HospiceLocationId.ToString()));


            return fulfillmentPredicate;
        }

        private async Task<OrderHeader> IncludeFulfillmentDetails(Data.Models.OrderHeaders orderHeaderModel)
        {
            var orderHeader = _mapper.Map<OrderHeader>(orderHeaderModel);
            var orderFulfillmentLineItems = await _orderFulfillmentLineItemsRepository.GetManyAsync(o => o.OrderHeaderId == orderHeader.Id);
            orderHeader.OrderFulfillmentLineItems = _mapper.Map<IEnumerable<OrderFulfillmentLineItem>>(orderFulfillmentLineItems);

            var orderingNurse = await _usersRepository.GetByIdAsync(orderHeader.OrderRecipientUserId);
            if (orderingNurse != null)
            {
                orderHeader.OrderingNurse = $"{orderingNurse.FirstName} {orderingNurse.LastName}";
            }
            if (orderHeaderModel.CreatedByUserId.HasValue)
            {
                var originatorUser = await _usersRepository.GetByIdAsync(orderHeaderModel.CreatedByUserId.Value);
                if (originatorUser != null)
                {
                    orderHeader.CreatedByUser = $"{originatorUser.FirstName} {originatorUser.LastName}";
                }
            }
            if (orderHeaderModel.UpdatedByUserId.HasValue)
            {
                var modifiedUser = await _usersRepository.GetByIdAsync(orderHeaderModel.UpdatedByUserId.Value);
                if (modifiedUser != null)
                {
                    orderHeader.ModifiedByUser = $"{modifiedUser.FirstName} {modifiedUser.LastName}";
                }
            }
            var dispatchInstruction = await _dispatchInstructionsRepository.GetAsync(d => d.OrderHeaderId == orderHeader.Id);
            var driverUser = dispatchInstruction?.Vehicle.DriversCurrentVehicle?.User;
            if (driverUser != null)
            {
                orderHeader.AssignedDriver = $"{driverUser.FirstName} {driverUser.LastName}";
            }
            return orderHeader;
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
    }
}
