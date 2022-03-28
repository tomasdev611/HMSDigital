using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Config;
using HMSDigital.Core.BusinessLayer.DTOs;
using HMSDigital.Core.BusinessLayer.Enums;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using HospiceSource.Digital.Patient.SDK.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using HMSDigital.Patient.FHIR.BusinessLayer.Interfaces;
using HMSDigital.Patient.FHIR.Models;
using HospiceSource.Digital.NetSuite.SDK.Config;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using NSSDKViewModels = HospiceSource.Digital.NetSuite.SDK.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HMSDigital.Patient.ViewModels;
using HospiceSource.Digital.Patient.SDK.ViewModels;
using PatientInventory = HMSDigital.Core.ViewModels.PatientInventory;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public class SystemService : ISystemService
    {
        private readonly IUsersRepository _usersRepository;

        private readonly IRolesRepository _rolesRepository;

        private readonly IAddressRepository _addressRepository;

        private readonly IHospiceMemberRepository _hospiceMemberRepository;

        private readonly IOrderFulfillmentLineItemsRepository _orderFulfillmentLineItemsRepository;

        private readonly IOrderHeadersRepository _orderHeadersRepository;

        private readonly IPatientInventoryRepository _patientInventoryRepository;

        private readonly IInventoryRepository _inventoryRepository;

        private readonly IOrderLineItemsRepository _orderLineItemsRepository;

        private readonly IHospiceRepository _hospiceRepository;

        private readonly IHospiceLocationRepository _hospiceLocationRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly IItemRepository _itemRepository;

        private readonly IDispatchInstructionsRepository _dispatchInstructionsRepository;

        private readonly IIdentityService _identityService;

        private readonly IPaginationService _paginationService;

        private readonly IAddressStandardizerService _addressStandardizerService;

        private readonly INetSuiteService _netSuiteService;

        private readonly IHospiceMemberService _hospiceMemberService;

        private readonly IFulfillmentService _fulfillmentService;

        private readonly IOrderHeadersService _orderHeadersService;

        private readonly IPatientService _patientService;

        private readonly IDispatchService _dispatchService;

        private readonly IFHIRQueueService<FHIRHospice> _fhirQueueService;

        private readonly NetSuiteConfig _netSuiteConfig;

        private readonly IMapper _mapper;

        private readonly ILogger<SystemService> _logger;

        private readonly SystemLogConfig _systemLogConfig;

        public SystemService(IUsersRepository usersRepository,
            IRolesRepository rolesRepository,
            IAddressRepository addressRepository,
            IHospiceMemberRepository hospiceMemberRepository,
            IOrderFulfillmentLineItemsRepository orderFulfillmentLineItemsRepository,
            IOrderHeadersRepository orderHeadersRepository,
            IPatientInventoryRepository patientInventoryRepository,
            IInventoryRepository inventoryRepository,
            IOrderLineItemsRepository orderLineItemsRepository,
            IIdentityService identityService,
            IPaginationService paginationService,
            INetSuiteService netSuiteService,
            IAddressStandardizerService addressStandardizerService,
            IHospiceMemberService hospiceMemberService,
            IFulfillmentService fulfillmentService,
            IOrderHeadersService orderHeadersService,
            IFHIRQueueService<FHIRHospice> fhirQueueService,
            IDispatchInstructionsRepository dispatchInstructionsRepository,
            IOptions<NetSuiteConfig> netSuiteOptions,
            IOptions<SystemLogConfig> systemLogConfigOptions,
            IHospiceRepository hospiceRepository,
            IHospiceLocationRepository hospiceLocationRepository,
            ISitesRepository sitesRepository,
            IItemRepository itemRepository,
            IDispatchService dispatchService,
            IPatientService patientService,
            ILogger<SystemService> logger,
            IMapper mapper)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _addressRepository = addressRepository;
            _hospiceMemberRepository = hospiceMemberRepository;
            _orderFulfillmentLineItemsRepository = orderFulfillmentLineItemsRepository;
            _orderHeadersRepository = orderHeadersRepository;
            _patientInventoryRepository = patientInventoryRepository;
            _inventoryRepository = inventoryRepository;
            _orderLineItemsRepository = orderLineItemsRepository;
            _hospiceRepository = hospiceRepository;
            _dispatchInstructionsRepository = dispatchInstructionsRepository;
            _hospiceLocationRepository = hospiceLocationRepository;
            _sitesRepository = sitesRepository;
            _itemRepository = itemRepository;

            _identityService = identityService;
            _paginationService = paginationService;
            _addressStandardizerService = addressStandardizerService;
            _netSuiteService = netSuiteService;
            _hospiceMemberService = hospiceMemberService;
            _fulfillmentService = fulfillmentService;
            _orderHeadersService = orderHeadersService;
            _dispatchService = dispatchService;
            _patientService = patientService;
            _fhirQueueService = fhirQueueService;

            _netSuiteConfig = netSuiteOptions.Value;
            _systemLogConfig = systemLogConfigOptions.Value;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<PaginatedList<User>> GetUsersWithoutIdentity(SieveModel sieveModel)
        {
            _usersRepository.SieveModel = sieveModel;
            var totalRecords = await _usersRepository.GetCountAsync(u => string.IsNullOrEmpty(u.CognitoUserId) && !string.IsNullOrEmpty(u.Email));
            var userModels = await _usersRepository.GetManyAsync(u => string.IsNullOrEmpty(u.CognitoUserId) && !string.IsNullOrEmpty(u.Email));
            var users = _mapper.Map<IEnumerable<User>>(userModels);
            return _paginationService.GetPaginatedList(users, totalRecords, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<PaginatedList<HospiceMember>> GetMemberWithoutNetSuiteContact(SieveModel sieveModel)
        {
            _hospiceMemberRepository.SieveModel = sieveModel;
            var totalRecords = await _hospiceMemberRepository.GetCountAsync(m => (
                                                                   m.HospiceLocationMembers.Any(l => l.NetSuiteContactId == null)
                                                                || m.NetSuiteContactId == null
                                                                ) && !string.IsNullOrEmpty(m.User.Email));
            var memberModels = await _hospiceMemberRepository.GetManyAsync(m => (
                                                                   m.HospiceLocationMembers.Any(l => l.NetSuiteContactId == null)
                                                                || m.NetSuiteContactId == null
                                                                ) && !string.IsNullOrEmpty(m.User.Email));
            var members = _mapper.Map<IEnumerable<HospiceMember>>(memberModels);
            return _paginationService.GetPaginatedList(members, totalRecords, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<PaginatedList<User>> GetUsersWithoutEmail(SieveModel sieveModel)
        {
            _usersRepository.SieveModel = sieveModel;
            var totalRecords = await _usersRepository.GetCountAsync(u => string.IsNullOrEmpty(u.Email));
            var userModels = await _usersRepository.GetManyAsync(u => string.IsNullOrEmpty(u.Email));
            var users = _mapper.Map<IEnumerable<User>>(userModels);
            return _paginationService.GetPaginatedList(users, totalRecords, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task FixAllUsersWithoutIdentity()
        {
            var nonCognitoUserModels = await _usersRepository.GetManyAsync(u => string.IsNullOrEmpty(u.CognitoUserId) && !string.IsNullOrEmpty(u.Email));
            foreach (var nonCognitoUserModel in nonCognitoUserModels)
            {
                var nonCognitoUser = _mapper.Map<UserCreateRequest>(nonCognitoUserModel);
                var identityUser = await _identityService.CreateUser(nonCognitoUser);
                nonCognitoUserModel.CognitoUserId = identityUser.UserId;
                await _usersRepository.UpdateAsync(nonCognitoUserModel);
            }
        }

        public async Task<long> FixNonVerifiedAddresses()
        {
            var addresses = await _addressRepository.GetManyAsync(a => !a.IsVerified && string.IsNullOrEmpty(a.VerifiedBy));
            var updatedAddressCount = 0;
            foreach (var address in addresses)
            {
                try
                {
                    var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(address));
                    if (standardizedAddress != null && standardizedAddress.IsVerified)
                    {
                        await _addressRepository.UpdateAsync(_mapper.Map(standardizedAddress, address));
                        updatedAddressCount++;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return updatedAddressCount;
        }

        public async Task<long> FixNonVerifiedHomeAddresses()
        {
            return await _patientService.FixAddressesWithIssue();
        }

        public async Task<long> FixMemberWithoutNetSuiteContact()
        {
            var updatedMemberCount = 0;
            var members = await _hospiceMemberRepository.GetManyAsync(m => m.HospiceLocationMembers.Any(l => l.NetSuiteContactId == null)
                                                                || m.NetSuiteContactId == null);

            var hospiceIds = members.Select(m => m.HospiceId);
            var hospices = await _hospiceRepository.GetManyAsync(h => hospiceIds.Contains(h.Id));

            foreach (var member in members)
            {
                var hospice = hospices.FirstOrDefault(h => h.Id == member.HospiceId);
                if (member.NetSuiteContactId == null && hospice.NetSuiteCustomerId.HasValue)
                {
                    var hospiceContactRequest = _mapper.Map<NSSDKViewModels.CustomerContactBase>(member.User);
                    hospiceContactRequest.NetSuiteCustomerId = hospice.NetSuiteCustomerId.Value;
                    hospiceContactRequest.CanAccessWebStore = member.CanAccessWebStore ?? false;
                    var hospiceContact = await _netSuiteService.CreateCustomerContact(hospiceContactRequest);
                    member.NetSuiteContactId = hospiceContact?.NetSuiteContactId;
                }

                foreach (var memberLocation in member.HospiceLocationMembers)
                {
                    if (memberLocation.NetSuiteContactId == null && memberLocation.HospiceLocation != null && memberLocation.HospiceLocation.NetSuiteCustomerId.HasValue)
                    {
                        var locationContactRequest = _mapper.Map<NSSDKViewModels.CustomerContactBase>(member.User);
                        locationContactRequest.NetSuiteCustomerId = memberLocation.HospiceLocation.NetSuiteCustomerId.Value;
                        locationContactRequest.CanAccessWebStore = member.CanAccessWebStore ?? false;
                        var locationContact = await _netSuiteService.CreateCustomerContact(locationContactRequest);
                        memberLocation.NetSuiteContactId = locationContact?.NetSuiteContactId;
                    }
                }

                if (member.NetSuiteContactId != null || member.HospiceLocationMembers.Any(l => l.NetSuiteContactId != null))
                {
                    await _hospiceMemberRepository.UpdateAsync(member);
                    updatedMemberCount++;
                }
            }
            return updatedMemberCount;
        }

        public async Task<PaginatedList<User>> GetInternalUsersWithoutNetSuiteContact(SieveModel sieveModel)
        {
            var userModels = await GetInternalUsersWithoutNetSuiteContact();
            var users = _mapper.Map<IEnumerable<User>>(userModels);
            return _paginationService.GetPaginatedList(users, userModels.Count(), sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<long> FixInternalUsersWithoutNetSuiteContact()
        {
            var usersToCreateInternalHospiceMember = _mapper.Map<IEnumerable<User>>(await GetInternalUsersWithoutNetSuiteContact());

            var updatedCount = 0;
            foreach (var user in usersToCreateInternalHospiceMember)
            {
                try
                {
                    await _hospiceMemberService.CreateInternalHospiceMember(user);
                    updatedCount++;
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return updatedCount;
        }

        public async Task<PaginatedList<OrderHeader>> GetUnconfirmedFulfillmentOrders(SieveModel sieveModel)
        {
            _orderHeadersRepository.SieveModel = sieveModel;
            var lineItemFulfillmentModels = await _orderFulfillmentLineItemsRepository.GetManyAsync(o => !o.IsFulfilmentConfirmed);
            var orderByLineItemFulfilmentModels = lineItemFulfillmentModels.GroupBy(l => l.OrderHeaderId);
            var orderIds = orderByLineItemFulfilmentModels.Select(g => g.Key);
            var totalRecords = orderByLineItemFulfilmentModels.Count();
            var orderModels = await _orderHeadersRepository.GetManyAsync(o => orderIds.Contains(o.Id));
            var orders = _mapper.Map<IEnumerable<OrderHeader>>(orderModels);
            return _paginationService.GetPaginatedList(orders, totalRecords, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<long> FixUnconfirmedFulfillmentOrders(bool dispatchOnly, int batchSize, bool stopOnFirstError)
        {
            var lineItemFulfillmentModels = await _orderFulfillmentLineItemsRepository.GetManyAsync(o => !o.IsFulfilmentConfirmed);

            var updatedCount = 0;
            var fulfillmentsByOrderId = lineItemFulfillmentModels.GroupBy(o => o.OrderHeaderId).Take(batchSize);
            foreach (var lineItemFulfillments in fulfillmentsByOrderId)
            {
                try
                {
                    var isOrderConfirmed = await _fulfillmentService.ConfirmOrderFulfillment(lineItemFulfillments.Key, lineItemFulfillments, dispatchOnly);
                    if (isOrderConfirmed)
                    {
                        updatedCount++;
                    }
                    else if (stopOnFirstError)
                    {
                        throw new ValidationException($"Failed to Confirm Fulfillment for order with Id ({lineItemFulfillments.Key})"); ;
                    }
                }
                catch (Exception ex)
                {
                    if (stopOnFirstError)
                    {
                        throw ex;
                    }
                    continue;
                }
            }
            return updatedCount;
        }

        public async Task<bool> FixUnconfirmedFulfillmentOrder(int orderId, bool dispatchOnly)
        {
            var orderModel = await _orderHeadersRepository.GetByIdAsync(orderId);
            if (orderModel == null)
            {
                throw new ValidationException($"Order with id ({orderId}) not found");
            }
            var lineItemFulfillmentModels = await _orderFulfillmentLineItemsRepository.GetManyAsync(o => !o.IsFulfilmentConfirmed
                                                                                && o.OrderHeaderId == orderId);

            if (lineItemFulfillmentModels == null || lineItemFulfillmentModels.Count() == 0)
            {
                throw new ValidationException($"Fulfillment for order with order id ({orderId}) is already confirmed.");
            }

            return await _fulfillmentService.ConfirmOrderFulfillment(orderId, lineItemFulfillmentModels, dispatchOnly);
        }

        public async Task<long> FixOrdersWithoutSite()
        {
            var orderHeaderModels = await _orderHeadersRepository.GetManyAsync(o => !o.SiteId.HasValue);

            var updatedCount = 0;
            foreach (var orderHeader in orderHeaderModels)
            {
                try
                {
                    var isOrderConfirmed = await _orderHeadersService.AssignSiteToOrder(orderHeader);
                    if (isOrderConfirmed)
                    {
                        updatedCount++;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return updatedCount;
        }

        public async Task<AzureLogResponse<APILog>> GetCoreApiLogs(APILogRequest apiLogRequest)
        {
            if (!Enum.TryParse(apiLogRequest.APILogType, true, out APILogTypes type))
            {
                throw new ValidationException($"Invalid API Log type: ({apiLogRequest.APILogType})");
            }
            var storageAccount = CloudStorageAccount.Parse(_systemLogConfig.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = null;
            if (type == APILogTypes.Core)
            {
                table = tableClient.GetTableReference("coreapilogs");
            }
            else if (type == APILogTypes.Patient)
            {
                table = tableClient.GetTableReference("patientsapisink");
            }
            else if (type == APILogTypes.Notification)
            {
                table = tableClient.GetTableReference("notificationapisink");
            }
            else if (type == APILogTypes.Fulfillment)
            {
                table = tableClient.GetTableReference("routingapilogs");
            }

            var filter = PrepareApiLogFilterQuery(apiLogRequest);
            var query = new TableQuery<APILogs>().Where(filter).Take(apiLogRequest.PageSize);
            var apiLogs = await table.ExecuteQuerySegmentedAsync(query, apiLogRequest.ContinuationToken);

            return new AzureLogResponse<APILog>()
            {
                APILogs = _mapper.Map<IEnumerable<APILog>>(apiLogs.Results),
                ContinuationToken = apiLogs.ContinuationToken
            };
        }

        public async Task<PaginatedList<OrderHeader>> GetOrdersWithoutSite(SieveModel sieveModel)
        {
            _orderHeadersRepository.SieveModel = sieveModel;
            var totalRecords = await _orderHeadersRepository.GetCountAsync(o => o.SiteId == null);
            var orderModels = await _orderHeadersRepository.GetManyAsync(o => o.SiteId == null);
            var orders = _mapper.Map<IEnumerable<OrderHeader>>(orderModels);
            return _paginationService.GetPaginatedList(orders, totalRecords, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<PaginatedList<Address>> GetNonVerifiedAddresses(SieveModel sieveModel)
        {
            _addressRepository.SieveModel = sieveModel;
            var totalRecords = await _addressRepository.GetCountAsync(a => !a.IsVerified);
            var addressModels = await _addressRepository.GetManyAsync(a => !a.IsVerified);
            var addresses = _mapper.Map<IEnumerable<Address>>(addressModels);
            return _paginationService.GetPaginatedList(addresses, totalRecords, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<Address> FixNonVerifiedAddress(int addressId)
        {
            var address = await _addressRepository.GetByIdAsync(addressId);
            if (address == null)
            {
                throw new ValidationException($"Address with Id({addressId}) does not exist");
            }
            try
            {
                var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(address));
                if (standardizedAddress != null && standardizedAddress.IsVerified)
                {
                    await _addressRepository.UpdateAsync(_mapper.Map(standardizedAddress, address));
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Address verification failed : ({ex.Message})");
                throw new ValidationException($"Address verification failed.");
            }
            return _mapper.Map<Address>(address);
        }

        public async Task<Address> FixNonVerifiedHomeAddress(int addressId)
        {
            return await _patientService.FixAddressWithIssue(addressId);
        }

        public async Task<PaginatedList<OrderHeader>> GetOrdersWithIncorrectStatus(SieveModel sieveModel)
        {
            var ignoredOrderStatuses = new List<int>()
            {
                (int)Data.Enums.OrderHeaderStatusTypes.Completed,
                (int)Data.Enums.OrderHeaderStatusTypes.Pending_Approval
            };
            var orderModels = await _orderHeadersRepository.GetManyAsync(o => !ignoredOrderStatuses.Contains(o.StatusId)
                                                                            || !o.DispatchStatusId.HasValue
                                                                            || !ignoredOrderStatuses.Contains(o.DispatchStatusId.Value));

            var orderIds = orderModels.Select(o => o.Id);
            var incorrectOrderIds = new List<int>();
            var fulfillmentOrderLineItems = await _orderFulfillmentLineItemsRepository.GetManyAsync(ol => orderIds.Contains(ol.OrderHeaderId));
            var dispatchInstructions = await _dispatchInstructionsRepository.GetManyAsync(d => d.OrderHeaderId.HasValue
                                                                                               && orderIds.Contains(d.OrderHeaderId.Value));
            foreach (var orderModel in orderModels)
            {
                var orderStatusValidator = new OrderStatusValidator(fulfillmentOrderLineItems, dispatchInstructions);
                var validationResult = orderStatusValidator.Validate(orderModel);
                if (!validationResult.IsValid)
                {
                    incorrectOrderIds.Add(orderModel.Id);
                }
            }

            _orderHeadersRepository.SieveModel = sieveModel;
            var totalRecords = await _orderHeadersRepository.GetCountAsync(o => incorrectOrderIds.Contains(o.Id));
            var ordersWithIncorrectStatus = await _orderHeadersRepository.GetManyAsync(o => incorrectOrderIds.Contains(o.Id));
            var orders = _mapper.Map<IEnumerable<OrderHeader>>(ordersWithIncorrectStatus);
            return _paginationService.GetPaginatedList(orders, totalRecords, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<OrderHeader> FixOrderStatus(int orderId, bool previewChanges)
        {
            var orderModel = await _orderHeadersRepository.GetByIdAsync(orderId);
            if (orderModel == null)
            {
                throw new ValidationException($"Order with Id({orderId}) does not exist");
            }

            orderModel = await _dispatchService.FixOrderStatus(orderModel, previewChanges);
            return _mapper.Map<OrderHeader>(orderModel);
        }

        public async Task<NSSDKViewModels.NetSuiteHmsLogResponse> GetNetSuiteLogs(ViewModels.NetSuiteLogRequest netsuiteLogRequest)
        {
            var nsNetsuiteLogRequest = _mapper.Map<NSSDKViewModels.NetSuiteLogRequest>(netsuiteLogRequest);
            return await _netSuiteService.GetNetSuiteHmsLogs(nsNetsuiteLogRequest);
        }

        public async Task<PaginatedList<NetSuiteHmsDispatchRecord>> GetNetSuiteDispatchRecords(ViewModels.NetSuiteDispatchRequest netSuiteDispatchRequest)
        {
            var nsNetsuiteDispatchRequest = _mapper.Map<NSSDKViewModels.NetSuiteDispatchRequest>(netSuiteDispatchRequest);

            if (netSuiteDispatchRequest.HospiceId != null)
            {
                var hospice = await _hospiceRepository.GetByIdAsync(netSuiteDispatchRequest.HospiceId.Value);
                if (hospice == null)
                {
                    throw new ValidationException($"Hospice with Id({netSuiteDispatchRequest.HospiceId}) does not exist");
                }
                nsNetsuiteDispatchRequest.CustomerId = hospice.NetSuiteCustomerId;
            }
            if (netSuiteDispatchRequest.HospiceLocationId != null)
            {
                var hospiceLocation = await _hospiceLocationRepository.GetByIdAsync(netSuiteDispatchRequest.HospiceLocationId.Value);
                if (hospiceLocation == null)
                {
                    throw new ValidationException($"Hospice Location with Id({netSuiteDispatchRequest.HospiceLocationId}) does not exist");
                }
                nsNetsuiteDispatchRequest.CustomerLocationId = hospiceLocation.NetSuiteCustomerId;
            }
            if (netSuiteDispatchRequest.SiteId != null)
            {
                var site = await _sitesRepository.GetByIdAsync(netSuiteDispatchRequest.SiteId.Value);
                if (site == null)
                {
                    throw new ValidationException($"Site with Id({netSuiteDispatchRequest.SiteId}) does not exist");
                }
                nsNetsuiteDispatchRequest.NetSuiteWareHouseId = site.NetSuiteLocationId;
            }
            if (netSuiteDispatchRequest.ItemId != null)
            {
                var item = await _itemRepository.GetByIdAsync(netSuiteDispatchRequest.ItemId.Value);
                if (item == null)
                {
                    throw new ValidationException($"Item with Id({netSuiteDispatchRequest.ItemId}) does not exist");
                }
                nsNetsuiteDispatchRequest.NetSuiteItemId = item.NetSuiteItemId;
            }

            var dispatchRecords = await _netSuiteService.GetNetSuiteHmsDispatchRecords(nsNetsuiteDispatchRequest);
            var dispatchRecordsWithItem = _paginationService.GetPaginatedList<NetSuiteHmsDispatchRecord>(
                _mapper.Map<IEnumerable<NetSuiteHmsDispatchRecord>>(dispatchRecords.Results),
                dispatchRecords.Count,
                dispatchRecords.CurrentPage,
                dispatchRecords.PageSize
            );
            var netsuiteItemIds = dispatchRecordsWithItem.Records.Where(dr => dr.NetsuiteItemId.HasValue).Select(dr => dr.NetsuiteItemId.Value).ToList();
            var items = await _itemRepository.GetManyAsync(i => i.NetSuiteItemId.HasValue && netsuiteItemIds.Contains(i.NetSuiteItemId.Value));
            foreach (var dispatchRecord in dispatchRecordsWithItem.Records)
            {
                if (dispatchRecord.NetsuiteItemId.HasValue)
                {
                    dispatchRecord.NetSuiteItem = _mapper.Map<Item>(items.FirstOrDefault(i => i.NetSuiteItemId.HasValue && i.NetSuiteItemId.Value == dispatchRecord.NetsuiteItemId.Value));
                }
            }
            return dispatchRecordsWithItem;
        }

        public async Task<GetPatientInventoryWithIssuesResponce> GetPatientInventoryWithInvalidInventory(string orderNumber, string assetTagNumber)
        {
            GetPatientInventoryWithIssuesResponce response = null;

            var patientInventoryModel = await GetPatientInventory(orderNumber, assetTagNumber);

            if (patientInventoryModel != null)
            {
                response = new GetPatientInventoryWithIssuesResponce();

                var inventory = await _inventoryRepository.GetManyAsync(i => (i.ItemId.Equals(patientInventoryModel.ItemId) && i.AssetTagNumber.Equals(assetTagNumber))
                                                                             || i.Id.Equals(patientInventoryModel.InventoryId));

                var currentInventory = inventory.FirstOrDefault(x => x.Id.Equals(patientInventoryModel.InventoryId));
                var newInventory = inventory.Where(x => !x.Id.Equals(patientInventoryModel.InventoryId) && !x.IsDeleted);

                response.PatientInventory = _mapper.Map<PatientInventory>(patientInventoryModel);
                response.CurrentInventory = _mapper.Map<Inventory>(currentInventory);
                response.CurrentInventoryIsDeleted = currentInventory.IsDeleted;
                response.NewInventory = _mapper.Map<IEnumerable<Inventory>>(newInventory);
            }

            return response;
        }

        public async Task<PatientInventoryWithInvalidItemResponse> GetPatientInventoryWithInvalidItem(string orderNumber, string assetTagNumber) 
        {
            PatientInventoryWithInvalidItemResponse response = null;

            var patientInventoryModel = await GetPatientInventory(orderNumber, assetTagNumber, true);

            if (patientInventoryModel != null)
            {
                response = new PatientInventoryWithInvalidItemResponse();
                response.PatientInventory = _mapper.Map<PatientInventory>(patientInventoryModel);

                var currentItem = await _itemRepository.GetAsync(x => x.Id.Equals(patientInventoryModel.ItemId));
                response.CurrentItem = _mapper.Map<Item>(currentItem);

                if (patientInventoryModel.ItemId != patientInventoryModel.Inventory.ItemId) {
                    response.NewItem = _mapper.Map<Item>(patientInventoryModel.Inventory.Item);
                }
            }

            return response;
        }

        private async Task<Data.Models.PatientInventory> GetPatientInventory(string orderNumber, string assetTagNumber, bool getByInventory = false) 
        {
            if (string.IsNullOrWhiteSpace(orderNumber) || string.IsNullOrWhiteSpace(assetTagNumber))
            {
                return null;
            }

            var orderLineItemModel = await _orderLineItemsRepository.GetAsync(oli => oli.OrderHeader.OrderNumber.Equals(orderNumber) &&
                                                                                     oli.AssetTagNumber.Equals(assetTagNumber));

            if (orderLineItemModel == null)
            {
                return null;
            }

            if (getByInventory) 
            {
                var inventoryModel = await _inventoryRepository.GetAsync(x => x.AssetTagNumber == assetTagNumber);

                if (inventoryModel == null) 
                {
                    return null;
                }

                return await _patientInventoryRepository.GetAsync(pi => pi.PatientUuid == orderLineItemModel.OrderHeader.PatientUuid &&
                                                                                              pi.InventoryId == inventoryModel.Id);
            }

            return await _patientInventoryRepository.GetAsync(pi => pi.PatientUuid == orderLineItemModel.OrderHeader.PatientUuid &&
                                                                                              pi.ItemId == orderLineItemModel.ItemId);
        }

        public async Task FixPatientInventoryWithInvalidInventory(FixPatientInventoryWithIssuesRequest request)
        {
            if (!request.NewInventoryId.HasValue) 
            {
                throw new ValidationException($"Inventory Id is required.");
            }

            var patientInventoryModel = await _patientInventoryRepository.GetByIdAsync(request.PatientInventoryId);
            if (patientInventoryModel == null)
            {
                throw new ValidationException($"Patient Inventory with Id({request.PatientInventoryId}) does not exist");
            }

            var inventoryModel = await _inventoryRepository.GetByIdAsync(request.NewInventoryId.Value);
            if (inventoryModel == null)
            {
                throw new ValidationException($"Inventory with Id({request.NewInventoryId}) does not exist");
            }

            if (patientInventoryModel.ItemId != inventoryModel.ItemId)
            {
                throw new ValidationException($"The item Id({patientInventoryModel.ItemId}) on the Patient Inventory is different from the item Id({inventoryModel.ItemId}) on the new Inventory");
            }

            var originalInventoryId = patientInventoryModel.InventoryId;
            patientInventoryModel.InventoryId = inventoryModel.Id;

            await _patientInventoryRepository.UpdateAsync(patientInventoryModel);

            var orderLineItemModels = await _orderLineItemsRepository.GetManyAsync(x => x.OrderHeader.PatientUuid.Equals(patientInventoryModel.PatientUuid) &&
                                                                                        x.ItemId == patientInventoryModel.ItemId &&
                                                                                       (x.AssetTagNumber != inventoryModel.AssetTagNumber || x.SerialNumber != inventoryModel.SerialNumber));
            if (orderLineItemModels != null && orderLineItemModels.Any())
            {
                foreach (var orderLineItemModel in orderLineItemModels)
                {
                    orderLineItemModel.AssetTagNumber = inventoryModel.AssetTagNumber;
                    orderLineItemModel.SerialNumber = inventoryModel.SerialNumber;
                }

                await _orderLineItemsRepository.UpdateManyAsync(orderLineItemModels);
            }

            _logger.LogInformation($"Patient inventory with id({request.PatientInventoryId}) was updated. Original inventory id: {originalInventoryId}. New inventory id: {inventoryModel.Id}");
        }

        public async Task FixPatientInventoryWithInvalidItem(FixPatientInventoryWithIssuesRequest request)
        {
            if (!request.NewItemId.HasValue)
            {
                throw new ValidationException($"Item Id is required.");
            }

            var patientInventoryModel = await _patientInventoryRepository.GetByIdAsync(request.PatientInventoryId);
            if (patientInventoryModel == null)
            {
                throw new ValidationException($"Patient Inventory with Id({request.PatientInventoryId}) does not exist");
            }

            var itemModel = await _itemRepository.GetByIdAsync(request.NewItemId.Value);
            if (itemModel == null)
            {
                throw new ValidationException($"Item with Id({request.NewItemId}) does not exist");
            }

            if (patientInventoryModel.Inventory.ItemId != itemModel.Id)
            {
                throw new ValidationException($"The item Id({patientInventoryModel.Inventory.ItemId}) on the Patient Inventory is different from the new item Id({itemModel.Id})");
            }

            var originalItemId = patientInventoryModel.ItemId;
            patientInventoryModel.ItemId = itemModel.Id;

            await _patientInventoryRepository.UpdateAsync(patientInventoryModel);

            _logger.LogInformation($"Patient inventory with id({request.PatientInventoryId}) was updated. Original item id: {originalItemId}. New item id: {itemModel.Id}");
        }

        public async Task<IEnumerable<Guid>> GetPatientsWithOnlyConsumableInventory()
        {
            var consumablePatientInventory = await _patientInventoryRepository.GetManyAsync(pi => pi.Item.IsConsumable);
            var patientUUIDsWithConsumableInventory = consumablePatientInventory.Select(pi => pi.PatientUuid).Distinct();
            var dmePatientInventoryForPatientsWithConsumableInventory = await _patientInventoryRepository.GetManyAsync(pi => patientUUIDsWithConsumableInventory.Contains(pi.PatientUuid) && pi.Item.IsDme);
            var patientUUIDsWithDmeAndConsumableInventory = dmePatientInventoryForPatientsWithConsumableInventory.Select(pi => pi.PatientUuid).Distinct();
            var patientUUIDsWithConsumableInventoryOnly = patientUUIDsWithConsumableInventory.Except(patientUUIDsWithDmeAndConsumableInventory);

            var activeOrders = await _orderHeadersRepository.GetManyAsync(o => o.PatientUuid != Guid.Empty && patientUUIDsWithConsumableInventoryOnly.Contains(o.PatientUuid)
                                                                                       && o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Completed
                                                                                       && o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Cancelled);
            var patientWithActiveOrders = activeOrders.Where(o => o.PatientUuid != Guid.Empty).Select(o => o.PatientUuid).Distinct();
            return patientUUIDsWithConsumableInventoryOnly.Except(patientWithActiveOrders);
        }

        public async Task<IEnumerable<ViewModels.PatientInventory>> FixPatientsWithOnlyConsumableInventory(Guid patientUUID, bool previewChanges)
        {
            var patientInventory = await _patientInventoryRepository.GetManyAsync(pi => pi.PatientUuid == patientUUID);
            if (patientInventory.Count() == 0)
            {
                return _mapper.Map<IEnumerable<ViewModels.PatientInventory>>(patientInventory);
            }
            if (!patientInventory.All(pi => pi.Item.IsConsumable))
            {
                throw new ValidationException("Cannot delete patient inventory for patient having non-consumable inventory");
            }
            var activeOrders = await _orderHeadersRepository.GetManyAsync(o => o.PatientUuid == patientUUID
                                                                            && o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Completed
                                                                            && o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Cancelled);
            if (activeOrders != null && activeOrders.Count() > 0)
            {
                throw new ValidationException("Patient have open orders.");
            }
            if (!previewChanges)
            {
                await _patientInventoryRepository.DeleteAsync(pi => pi.PatientUuid == patientUUID);
            }
            return _mapper.Map<IEnumerable<ViewModels.PatientInventory>>(patientInventory);
        }

        public async Task<PaginatedList<PatientInventory>> GetDeletedPatientInventory(SieveModel sieveModel)
        {
            _patientInventoryRepository.SieveModel = sieveModel;

            var totalRecords = await _patientInventoryRepository.GetCountAsync(pi => pi.Inventory.IsDeleted);
            var deletedPatientInventory = await _patientInventoryRepository.GetManyAsync(pi => pi.Inventory.IsDeleted);

            var patientInventories = _mapper.Map<IEnumerable<ViewModels.PatientInventory>>(deletedPatientInventory);

            var deletedByUserIds = patientInventories.Select(p => p.DeletedByUserId);
            var deletedByUsers = await _usersRepository.GetManyAsync(u => deletedByUserIds.Contains(u.Id));

            foreach (var patientInventory in patientInventories)
            {
                var deletedByUser = deletedByUsers.FirstOrDefault(u => u.Id == patientInventory.DeletedByUserId);
                patientInventory.DeletedByUserName = $"{deletedByUser.FirstName} {deletedByUser.LastName}";
            }

            return _paginationService.GetPaginatedList(patientInventories, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<IEnumerable<string>> GetPatientsWithInvalidStatus()
        {
            var sieveModel = new SieveModel()
            {
                PageSize = 25,
                Sorts = "createdDateTime",
                Page = 1
            };

            var invalidStatusPatients = new List<string>();
            while (!invalidStatusPatients.Any())
            {
                var patientsRequest = new List<PatientStatusValidationRequest>();
                var activePatients = await _patientService.GetAllPatients(sieveModel);

                patientsRequest.AddRange(await GetPatientStatusRequest(activePatients));

                var patientValidationResults = await _patientService.ValidatePatientStatus(patientsRequest, false);
                invalidStatusPatients.AddRange(patientValidationResults.Where(x => !x.HasValidStatus).Select(x => x.PatientUuid.ToString()).ToList());

                if (activePatients.TotalPageCount <= sieveModel.Page)
                {
                    break;
                }

                sieveModel.Page++;
            }

            return invalidStatusPatients;
        }
        public async Task<IEnumerable<string>> FixAllPatientWithStatusIssues()
        {
            var sieveModel = new SieveModel()
            {
                PageSize = 100,
                Sorts = "createdDateTime",
                Page = 1
            };
            var activePatients = await _patientService.GetAllPatients(sieveModel);

            while (sieveModel.Page <= activePatients.TotalPageCount)
            {
                var patientsRequest = new List<PatientStatusValidationRequest>();
                patientsRequest.AddRange(await GetPatientStatusRequest(activePatients));

                _patientService.ValidatePatientStatus(patientsRequest, true);

                sieveModel.Page++;
                activePatients = await _patientService.GetAllPatients(sieveModel);
            }
            return null;
        }

        private async Task<IEnumerable<PatientStatusValidationRequest>> GetPatientStatusRequest(PaginatedList<PatientDetail> activePatients)
        {
            var patientsRequest = new List<PatientStatusValidationRequest>();
            var activePatientUuids = activePatients.Records.Select(p => Guid.Parse(p.UniqueId));

            foreach (var patientUuid in activePatientUuids)
            {
                var activeOrders = await _orderHeadersRepository.ExistsAsync(o => o.PatientUuid.Equals(patientUuid) && o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Completed
                                                                                                                    && o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Cancelled
                                                                                                                    && !(o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Partial_Fulfillment && o.IsExceptionFulfillment));

                var patientsInventory =
                    await _patientInventoryRepository.ExistsAsync(p => p.PatientUuid.Equals(patientUuid) && p.Item.IsDme);

                var hasOrders = await _orderHeadersRepository.ExistsAsync(o => o.PatientUuid.Equals(patientUuid));

                patientsRequest.Add(new PatientStatusValidationRequest
                {
                    PatientUuid = patientUuid,
                    StatusId = activePatients.Records.SingleOrDefault(x => x.UniqueId.Equals(patientUuid.ToString()))?.StatusId,
                    HasOpenOrders = activeOrders,
                    IsDMEEquipmentLeft = patientsInventory,
                    HasOrders = hasOrders
                });
            }

            return patientsRequest;
        }

        public async Task<PatientStatus> FixPatientWithInvalidStatus(Guid patientUUID, bool previewChanges)
        {
            var activeOrders = await _orderHeadersRepository.ExistsAsync(o => o.PatientUuid == patientUUID
                                                                               && o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Completed
                                                                               && o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Cancelled
                                                                               && !(o.StatusId != (int)Data.Enums.OrderHeaderStatusTypes.Partial_Fulfillment && o.IsExceptionFulfillment));

            var patientsInventory = await _patientInventoryRepository.ExistsAsync(p => p.PatientUuid == patientUUID && p.Item.IsDme);

            var patientStatusRequest = new PatientStatus
            {
                StatusChangedDate = DateTime.UtcNow,
                HasOpenOrders = activeOrders,
                IsDMEEquipmentLeft = patientsInventory
            };
            if (!previewChanges)
            {
                _patientService.UpdatePatientStatus(patientUUID, null, patientsInventory, activeOrders);
            }
            return patientStatusRequest;
        }

        public async Task<PaginatedList<Address>> GetNonVerifiedHomeAddresses(SieveModel sieveModel)
        {
            return await _patientService.GetNonVerifiedAddresses(sieveModel);
        }

        public async Task<IEnumerable<string>> GetHospicesWithoutFhirOrganization()
        {
            var hospicesWithoutFhirOrganization = await _hospiceRepository.GetManyAsync(x => !x.FhirOrganizationId.HasValue);
            return hospicesWithoutFhirOrganization.Select(x => x.Name);
        }

        public async Task<long> FixHospicesWithoutFhirOrganization()
        {
            var hospiceModels = await _hospiceRepository.GetManyAsync(x => !x.FhirOrganizationId.HasValue);
            hospiceModels.Select(x => { x.FhirOrganizationId = Guid.Empty; return x; }).ToList();
            await _hospiceRepository.UpdateManyAsync(hospiceModels);

            var fhirHospices = _mapper.Map<IEnumerable<FHIRHospice>>(hospiceModels);
            _fhirQueueService.QueueCreateRequestList(fhirHospices);
            return hospiceModels.Count();
        }

        private async Task<IEnumerable<Data.Models.Users>> GetInternalUsersWithoutNetSuiteContact()
        {
            var eligibleRoles = await _rolesRepository.GetManyAsync(r => r.RoleType == Enums.RoleTypes.Internal.ToString()
                                                                         && r.RolePermissions.Any(rp => (rp.PermissionNoun.Name.ToLower() == "orders"
                                                                                                        && (rp.PermissionVerb.Name.ToLower() == "create" || rp.PermissionVerb.Name.ToLower() == "approve"))));
            var eligibleRoleIds = eligibleRoles.Select(r => r.Id);
            var internalUsers = await _usersRepository.GetManyAsync(u => u.UserRoles.Any(ur => ur.RoleId.HasValue && eligibleRoleIds.Contains(ur.RoleId.Value)));
            var internalUserIds = internalUsers.Select(u => u.Id);
            var validHospiceMembersForInternalUsers = await _hospiceMemberRepository.GetManyAsync(hm => internalUserIds.Contains(hm.UserId)
                                                                                                    && hm.Hospice.NetSuiteCustomerId == _netSuiteConfig.InternalUsersHostCustomerId);
            var internalUserIdsWithoutNetSuiteContact = internalUserIds.Except(validHospiceMembersForInternalUsers.Select(hm => hm.UserId));
            return internalUsers.Where(u => internalUserIdsWithoutNetSuiteContact.Contains(u.Id));
        }

        public async Task<IEnumerable<string>> GetPatientsWithoutFhirRecord()
        {
            var sieveModel = new SieveModel()
            {
                Filters = "fhirPatientId==null",
                Sorts = "createdDateTime"
            };

            var missingFhirPatients = await _patientService.GetAllPatients(sieveModel);
            return missingFhirPatients.Records.Select(p => p.UniqueId);
        }

        public async Task<long> FixPatientsWithoutFhirRecord()
        {
            return await _patientService.FixMissingFhirPatients();
        }

        private string PrepareApiLogFilterQuery(APILogRequest apiLogRequest)
        {
            var filter = "";

            if (apiLogRequest.FromDate != null && apiLogRequest.ToDate != null
                && apiLogRequest.FromDate != DateTime.MinValue && apiLogRequest.ToDate != DateTime.MinValue)
            {
                if (apiLogRequest.FromDate == apiLogRequest.ToDate)
                {
                    apiLogRequest.ToDate = apiLogRequest.ToDate.AddDays(1);
                }
                var fromDate = TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.GreaterThanOrEqual, apiLogRequest.FromDate);
                var toDate = TableQuery.GenerateFilterConditionForDate("Timestamp", QueryComparisons.LessThanOrEqual, apiLogRequest.ToDate);
                filter = CombineFilters(fromDate, toDate);
            }

            return filter;
        }

        private string CombineFilters(string baseFilter, string newFilter, string tableOperator = TableOperators.And)
        {
            if (string.IsNullOrEmpty(baseFilter))
            {
                return newFilter;
            }
            return TableQuery.CombineFilters(baseFilter, tableOperator, newFilter);
        }
    }
}
