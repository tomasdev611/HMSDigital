using AutoMapper;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Enums;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Validations;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using NSViewModel = HMSDigital.Core.ViewModels.NetSuite;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Constants;
using NotificationSDK.Interfaces;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using HMSDigital.Common.BusinessLayer.Constants;
using Microsoft.AspNetCore.Http;
using HMSDigital.Core.ViewModels.NetSuite;
using Hms2BillingSDK.Repositories.Interfaces;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class HospiceService : IHospiceService
    {
        private readonly IMapper _mapper;

        private readonly ILogger<HospiceService> _logger;

        private readonly HttpContext _httpContext;


        private readonly IHospiceRepository _hospiceRepository;

        private readonly IRolesRepository _rolesRepository;

        private readonly ISubscriptionRepository _subscriptionRepository;

        private readonly ISubscriptionItemRepository _subscriptionItemRepository;

        private readonly IHMS2ContractRepository _hms2ContractRepository;

        private readonly IHMS2ContractItemRepository _hms2ContractItemRepository;

        private readonly ICsvMappingRepository _csvMappingRepository;

        private readonly IHospiceLocationRepository _hospiceLocationRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly IUsersRepository _usersRepository;

        private readonly ICreditHoldHistoryRepository _creditHoldHistoryRepository;

        private readonly IContractRecordsRepository _contractRecordsRepository;

        private readonly IItemRepository _itemRepository;

        private readonly IHms2BillingContractsRepository _hms2BillingContractsRepository;

        private readonly IHms2BillingContractItemsRepository _hms2BillingContractItemsRepository;

        private readonly IPaginationService _paginationService;

        private readonly IHospiceMemberService _hospiceMemberService;

        private readonly IAddressStandardizerService _addressStandardizerService;

        private readonly INotificationService _notificationService;

        private readonly INetSuiteService _netSuiteService;


        public HospiceService(IMapper mapper,
            ILogger<HospiceService> logger,
            IHttpContextAccessor httpContextAccessor,
            IHospiceRepository hospiceRepository,
            IRolesRepository rolesRepository,
            ISubscriptionRepository subscriptionRepository,
            ISubscriptionItemRepository subscriptionItemRepository,
            IHMS2ContractRepository hms2ContractRepository,
            IHMS2ContractItemRepository hms2ContractItemRepository,
            ICsvMappingRepository csvMappingRepository,
            IHospiceLocationRepository hospiceLocationRepository,
            ISitesRepository sitesRepository,
            IUsersRepository usersRepository,
            ICreditHoldHistoryRepository creditHoldHistoryRepository,
            IContractRecordsRepository contractRecordsRepository,
            IItemRepository itemRepository,
            IHms2BillingContractsRepository hms2BillingContractsRepository,
            IHms2BillingContractItemsRepository hms2BillingContractItemsRepository,
            IPaginationService paginationService,
            IHospiceMemberService hospiceMemberService,
            IAddressStandardizerService addressStandardizerService,
            INotificationService notificationService,
            INetSuiteService netSuiteService)
        {
            _mapper = mapper;
            _logger = logger;
            _httpContext = httpContextAccessor.HttpContext;

            _hospiceRepository = hospiceRepository;
            _rolesRepository = rolesRepository;
            _subscriptionRepository = subscriptionRepository;
            _subscriptionItemRepository = subscriptionItemRepository;
            _hms2ContractRepository = hms2ContractRepository;
            _hms2ContractItemRepository = hms2ContractItemRepository;
            _csvMappingRepository = csvMappingRepository;
            _hospiceLocationRepository = hospiceLocationRepository;
            _sitesRepository = sitesRepository;
            _usersRepository = usersRepository;
            _creditHoldHistoryRepository = creditHoldHistoryRepository;
            _contractRecordsRepository = contractRecordsRepository;
            _itemRepository = itemRepository;
            _hms2BillingContractsRepository = hms2BillingContractsRepository;
            _hms2BillingContractItemsRepository = hms2BillingContractItemsRepository;

            _paginationService = paginationService;
            _hospiceMemberService = hospiceMemberService;
            _addressStandardizerService = addressStandardizerService;
            _notificationService = notificationService;
            _netSuiteService = netSuiteService;
        }

        public async Task<PaginatedList<Hospice>> GetAllHospices(SieveModel sieveModel)
        {
            _hospiceRepository.SieveModel = sieveModel;
            var totalRecords = await _hospiceRepository.GetCountAsync(h => true);
            var hospiceModels = await _hospiceRepository.GetAllAsync();
            var hospices = _mapper.Map<IEnumerable<Hospice>>(hospiceModels);
            return _paginationService.GetPaginatedList(hospices, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PaginatedList<Hospice>> SearchHospices(SieveModel sieveModel, string searchQuery)
        {
            _hospiceRepository.SieveModel = sieveModel;
            var totalRecords = await _hospiceRepository.GetCountAsync(h => h.Name.Contains(searchQuery));
            var hospiceModels = await _hospiceRepository.GetManyAsync(p => p.Name.Contains(searchQuery));
            var hospices = _mapper.Map<IEnumerable<Hospice>>(hospiceModels);
            return _paginationService.GetPaginatedList(hospices, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<Hospice> GetHospiceById(int hospiceId)
        {
            var hospiceModel = await _hospiceRepository.GetByIdAsync(hospiceId);
            return _mapper.Map<Hospice>(hospiceModel);
        }

        public async Task<Hospice> ChangeCreditHoldStatus(int hospiceId, CreditHoldRequest creditHoldRequest)
        {
            var hospiceModel = await _hospiceRepository.GetByIdAsync(hospiceId);
            if (hospiceModel == null)
            {
                throw new ValidationException($"Hospice Id({hospiceId}) is not valid");
            }
            var user = await _usersRepository.GetByIdAsync(GetLoggedInUserId());
            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }
            await StoreHospiceCreditHoldHistory(hospiceModel);
            hospiceModel.IsCreditOnHold = creditHoldRequest.IsCreditOnHold;
            hospiceModel.CreditHoldByUserId = user.Id;
            hospiceModel.CreditHoldNote = creditHoldRequest.CreditHoldNote;
            hospiceModel.CreditHoldDateTime = DateTime.UtcNow;
            await _hospiceRepository.UpdateAsync(hospiceModel);
            return _mapper.Map<Hospice>(hospiceModel);
        }

        public async Task<IEnumerable<ViewModels.CreditHoldHistory>> GetCreditHoldHistory(int hospiceId, SieveModel sieveModel)
        {
            var hospiceModel = await _hospiceRepository.GetByIdAsync(hospiceId);
            if (hospiceModel == null)
            {
                throw new ValidationException($"Hospice Id({hospiceId}) is not valid");
            }
            _creditHoldHistoryRepository.SieveModel = sieveModel;
            var creditHoldHistory = await _creditHoldHistoryRepository.GetManyAsync(h => h.HospiceId == hospiceId);
            return _mapper.Map<IEnumerable<ViewModels.CreditHoldHistory>>(creditHoldHistory);
        }


        public async Task<Hospice> GetHospiceByNetSuiteCustomerId(int netSuiteCustomerId)
        {
            var hospiceModel = await _hospiceRepository.GetAsync(h => h.NetSuiteCustomerId == netSuiteCustomerId);
            return _mapper.Map<Hospice>(hospiceModel);
        }

        public async Task<NSViewModel.HospiceResponse> UpsertHospiceWithLocation(NSViewModel.NSHospiceRequest hospiceRequest)
        {
            try
            {
                var hospice = await _hospiceRepository.GetAsync(h => h.NetSuiteCustomerId == hospiceRequest.NetSuiteCustomerId);
                if (hospice != null)
                {
                    hospice = await UpdateHospiceWithLocation(hospice, hospiceRequest);
                }
                else
                {
                    hospice = await InsertHospiceWithLocation(hospiceRequest);
                    var role = await _rolesRepository.GetByIdAsync((int)Enums.Roles.HospiceAdmin);
                    var memberCreateRequest = new HospiceMemberRequest()
                    {
                        Email = hospiceRequest.Email,
                        FirstName = NetSuiteCustomer.FIRST_NAME,
                        LastName = NetSuiteCustomer.LAST_NAME,
                        Designation = NetSuiteCustomer.DESIGNATION,
                        UserRoles = new List<HospiceMemberRoleRequest>()
                        {
                            new HospiceMemberRoleRequest()
                            {
                                RoleId = role.Id,
                                ResourceType = Data.Enums.ResourceTypes.Hospice.ToString(),
                                ResourceId = hospice.Id
                            }
                        }
                    };
                    await _hospiceMemberService.CreateHospiceMember(hospice.Id, memberCreateRequest);
                    _notificationService.SendHopiceCreatedNotification(memberCreateRequest.Email);
                }

                return _mapper.Map<NSViewModel.HospiceResponse>(hospice);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while Creating hospice with location and facility: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while Creating hospice with location and facility: {ex.Message}");
                throw ex;
            }
        }

        public async Task<IEnumerable<Role>> GetHospiceRoles(int hospiceId)
        {
            var hospiceRoles = await _rolesRepository.GetManyAsync(r => r.RoleType == RoleTypes.Hospice.ToString());
            return _mapper.Map<IEnumerable<Role>>(hospiceRoles);
        }

        public async Task<ViewModels.NetSuite.HospiceResponse> DeleteCustomer(NSViewModel.HospiceDeleteRequest hospiceDeleteRequest)
        {

            var hospiceModel = await _hospiceRepository.GetAsync(c => c.NetSuiteCustomerId == hospiceDeleteRequest.NetSuiteCustomerId);
            if (hospiceModel != null)
            {
                await _hospiceRepository.DeleteAsync(hospiceModel);
            }
            else
            {
                var hospiceLocationModel = await _hospiceLocationRepository.GetAsync(c => c.NetSuiteCustomerId == hospiceDeleteRequest.NetSuiteCustomerId);
                if (hospiceLocationModel != null)
                {
                    await _hospiceLocationRepository.DeleteAsync(hospiceLocationModel);
                }
            }
            return _mapper.Map<ViewModels.NetSuite.HospiceResponse>(hospiceDeleteRequest);
        }

        public async Task<CsvMapping<InputMappedItem>> GetInputCsvMapping(int hospiceId, string mappedItem)
        {
            if (!Enum.TryParse(mappedItem, true, out MappedItemTypes mappedTable))
            {
                throw new ValidationException($"Mapped Item Type ({mappedItem}) is invalid");
            }
            var mapping = await _csvMappingRepository.GetAsync(cm => cm.MappedTable == mappedTable.ToString() && cm.HospiceId == hospiceId &&
                                                                     cm.MappingType == CsvMappingTypes.Input.ToString());
            if (mapping == null)
            {
                return CsvMapping.GetInputCsvMapping(mappedTable);
            }
            return JsonConvert.DeserializeObject<CsvMapping<InputMappedItem>>(mapping.MappingInJson);
        }

        public async Task<CsvMapping<OutputMappedItem>> GetOutputCsvMapping(int hospiceId, string mappedItem)
        {
            if (!Enum.TryParse(mappedItem, true, out MappedItemTypes mappedTable))
            {
                throw new ValidationException($"Mapped Item Type ({mappedItem}) is invalid");
            }
            var mapping = await _csvMappingRepository.GetAsync(cm => cm.MappedTable == mappedTable.ToString() && cm.HospiceId == hospiceId &&
                                                                     cm.MappingType == CsvMappingTypes.Output.ToString());
            if (mapping == null)
            {
                return CsvMapping.GetOutputCsvMapping(mappedTable);
            }
            return JsonConvert.DeserializeObject<CsvMapping<OutputMappedItem>>(mapping.MappingInJson);
        }

        public async Task<CsvMapping<InputMappedItem>> PutInputCsvMapping(int hospiceId, string mappedItem, CsvMapping<InputMappedItem> inputMapping)
        {
            if (!Enum.TryParse(mappedItem, true, out MappedItemTypes mappedTable))
            {
                throw new ValidationException($"Mapped Item Type ({mappedItem}) is invalid");
            }
            try
            {
                var mappedItemValidator = new MappedItemValidator();
                foreach (var colMapping in inputMapping.ColumnNameMappings.Values)
                {
                    var validationResult = mappedItemValidator.Validate(colMapping);
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors[0].ErrorMessage);
                    }
                }

                return await UpsertCsvMapping<CsvMapping<InputMappedItem>>(hospiceId, mappedTable.ToString(), inputMapping, CsvMappingTypes.Input.ToString());
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while put input Csv Mapping: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while put input Csv Mapping: {ex.Message}");
                throw ex;
            }
        }

        public async Task<CsvMapping<OutputMappedItem>> PutOutputCsvMapping(int hospiceId, string mappedItem, CsvMapping<OutputMappedItem> outputMapping)
        {
            if (!Enum.TryParse(mappedItem, true, out MappedItemTypes mappedTable))
            {
                throw new ValidationException($"Mapped Item Type ({mappedItem}) is invalid");
            }
            try
            {
                var mappedItemValidator = new MappedItemValidator();
                foreach (var colMapping in outputMapping.ColumnNameMappings.Values)
                {
                    var validationResult = mappedItemValidator.Validate(colMapping);
                    if (!validationResult.IsValid)
                    {
                        throw new ValidationException(validationResult.Errors[0].ErrorMessage);
                    }
                }

                return await UpsertCsvMapping<CsvMapping<OutputMappedItem>>(hospiceId, mappedTable.ToString(), outputMapping, CsvMappingTypes.Output.ToString());
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while create output Csv Mapping: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while create output Csv Mapping: {ex.Message}");
                throw ex;
            }
        }

        public async Task<PaginatedList<Subscription>> GetHospiceSubscriptions(int hospiceId, SieveModel sieveModel)
        {
            var hospiceModel = await _hospiceRepository.GetByIdAsync(hospiceId);
            if (hospiceModel == null)
            {
                throw new ValidationException($"Hospice with Id ({hospiceId}) is not valid");
            }

            _subscriptionRepository.SieveModel = sieveModel;
            var totalSubscriptionCount = await _subscriptionRepository.GetCountAsync(s => s.HospiceId == hospiceId);
            var subscriptions = await _subscriptionRepository.GetManyAsync(s => s.HospiceId == hospiceId);
            var subscriptionViewModels = _mapper.Map<IEnumerable<Subscription>>(subscriptions);

            return _paginationService.GetPaginatedList(subscriptionViewModels, totalSubscriptionCount, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<PaginatedList<Hms2Contract>> GetHMS2Contracts(int hospiceId, SieveModel sieveModel)
        {
            var hospiceModel = await _hospiceRepository.GetByIdAsync(hospiceId);
            if (hospiceModel == null)
            {
                throw new ValidationException($"Hospice with Id ({hospiceId}) is not valid");
            }

            _hms2ContractRepository.SieveModel = sieveModel;
            var totalContractCount = await _hms2ContractRepository.GetCountAsync(s => s.HospiceId == hospiceId);
            var contractModels = await _hms2ContractRepository.GetManyAsync(s => s.HospiceId == hospiceId);
            var contracts = _mapper.Map<IEnumerable<Hms2Contract>>(contractModels);

            return _paginationService.GetPaginatedList(contracts, totalContractCount, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task UpsertHospiceSubscriptions(int hospiceId)
        {
            var hospiceModel = await _hospiceRepository.GetByIdAsync(hospiceId);
            if (hospiceModel == null)
            {
                throw new ValidationException($"Hospice with Id ({hospiceId}) is not valid");
            }

            var netSuiteCustomerIds = hospiceModel.HospiceLocations.Where(l => l.NetSuiteCustomerId.HasValue).Select(l => l.NetSuiteCustomerId.Value).ToList();
            netSuiteCustomerIds.Add(hospiceModel.NetSuiteCustomerId.Value);
            await UpdateHospiceSubscriptionsFromNetSuite(hospiceId, netSuiteCustomerIds);

            var subscriptions = await _subscriptionRepository.GetManyAsync(s => s.HospiceId == hospiceId);

            foreach (var subscription in subscriptions)
            {
                await UpdateHospiceSubscriptionItemsFromNetSuite(subscription.Id, subscription);
            }

        }

        public async Task UpsertHms2HospiceContracts(int hospiceId)
        {
            var hospiceModel = await _hospiceRepository.GetByIdAsync(hospiceId);
            if (hospiceModel == null)
            {
                throw new ValidationException($"Hospice with Id ({hospiceId}) is not valid");
            }
            var hms2Ids = hospiceModel.Hms2HmsDigitalHospiceMappings.Select(h => h.Hms2id);
            if (hms2Ids.Any())
            {
                await UpdateHospiceContractsFromHms2Billing(hospiceId, hms2Ids, hospiceModel.Hms2HmsDigitalHospiceMappings);
            }

            var contracts = await _hms2ContractRepository.GetManyAsync(c => c.HospiceId == hospiceId);

            foreach (var contract in contracts)
            {
                await UpdateHospiceContractItemsFromHms2Billing(contract.Id, contract.Hms2ContractId);
            }
        }

        public async Task<PaginatedList<ViewModels.SubscriptionItem>> GetHospiceSubscriptionItemsBySubscription(int subscriptionId, SieveModel sieveModel)
        {
            var subscriptionModel = await _subscriptionRepository.GetByIdAsync(subscriptionId);
            if (subscriptionModel == null)
            {
                throw new ValidationException($"Subscription with Id ({subscriptionId}) is not valid");
            }

            _subscriptionItemRepository.SieveModel = sieveModel;

            var totalSubscriptionItemCount = await _subscriptionItemRepository.GetCountAsync(s => s.SubscriptionId == subscriptionId);
            var subscriptionItem = await _subscriptionItemRepository.GetManyAsync(s => s.SubscriptionId == subscriptionId);
            var subscriptionItemViewModels = _mapper.Map<IEnumerable<ViewModels.SubscriptionItem>>(subscriptionItem);

            return _paginationService.GetPaginatedList(subscriptionItemViewModels, totalSubscriptionItemCount, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<PaginatedList<ViewModels.Hms2ContractItem>> GetHospiceHMS2ContractItemsByContract(int contractId, SieveModel sieveModel)
        {
            var contractModel = await _hms2ContractRepository.GetByIdAsync(contractId);
            if (contractModel == null)
            {
                throw new ValidationException($"Subscription with Id ({contractId}) is not valid");
            }

            _hms2ContractItemRepository.SieveModel = sieveModel;

            var totalContractItemCount = await _hms2ContractItemRepository.GetCountAsync(s => s.ContractId == contractId);
            var contractItemModels = await _hms2ContractItemRepository.GetManyAsync(s => s.ContractId == contractId);
            var contractItems = _mapper.Map<IEnumerable<ViewModels.Hms2ContractItem>>(contractItemModels);

            return _paginationService.GetPaginatedList(contractItems, totalContractItemCount, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<ContractRecord> UpsertContractRecord(NSContractRecordRequest contractRecordRequest)
        {
            var hospiceModel = await _hospiceRepository.GetAsync(h => h.NetSuiteCustomerId == contractRecordRequest.NetSuiteCustomerId);

            var hospiceLocationModel = await _hospiceLocationRepository.GetAsync(l => l.NetSuiteCustomerId == contractRecordRequest.NetSuiteCustomerId);

            if (contractRecordRequest.NetSuiteCustomerId.HasValue && hospiceModel == null && hospiceLocationModel == null)
            {
                throw new ValidationException($"NetSuiteCustomerId ({contractRecordRequest.NetSuiteCustomerId}) is not valid");
            }

            var itemModel = await _itemRepository.GetAsync(i => i.NetSuiteItemId == contractRecordRequest.NetSuiteRelatedItemId);

            if (contractRecordRequest.NetSuiteRelatedItemId.HasValue && itemModel == null)
            {
                throw new ValidationException($"NetSuiteRelatedItemId ({contractRecordRequest.NetSuiteRelatedItemId}) is not valid");
            }

            var contractRecordModel = await _contractRecordsRepository.GetAsync(cr => cr.NetSuiteContractRecordId == contractRecordRequest.NetSuiteContractRecordId);

            if (contractRecordModel == null)
            {
                contractRecordModel = _mapper.Map<ContractRecords>(contractRecordRequest);
                contractRecordModel.HospiceId = hospiceModel?.Id;
                contractRecordModel.HospiceLocationId = hospiceLocationModel?.Id;
                contractRecordModel.ItemId = itemModel?.Id;
                await _contractRecordsRepository.AddAsync(contractRecordModel);
            }
            else
            {
                contractRecordModel = _mapper.Map(contractRecordRequest, contractRecordModel);
                contractRecordModel.HospiceId = hospiceModel?.Id;
                contractRecordModel.HospiceLocationId = hospiceLocationModel?.Id;
                contractRecordModel.ItemId = itemModel?.Id;
                await _contractRecordsRepository.UpdateAsync(contractRecordModel);
            }
            return _mapper.Map<ViewModels.ContractRecord>(contractRecordModel);
        }

        public async Task<PaginatedList<ContractRecord>> GetHospiceContractRecords(SieveModel sieveModel)
        {
            _contractRecordsRepository.SieveModel = sieveModel;
            var totalContractRecordsCount = await _contractRecordsRepository.GetCountAsync(s => true);
            var contractRecordModels = await _contractRecordsRepository.GetAllAsync();
            var contractRecords = _mapper.Map<IEnumerable<ContractRecord>>(contractRecordModels);

            return _paginationService.GetPaginatedList(contractRecords, totalContractRecordsCount, sieveModel?.Page, sieveModel?.PageSize);
        }

        public async Task<NSViewModel.NSContractRecordResponse> DeleteContractRecord(int netSuiteContractRecordId)
        {
            var contractRecordModel = await _contractRecordsRepository.GetAsync(c => c.NetSuiteContractRecordId == netSuiteContractRecordId);
            if (contractRecordModel != null)
            {
                await _contractRecordsRepository.DeleteAsync(contractRecordModel);
            }
            return new NSViewModel.NSContractRecordResponse()
            {
                Success = true,
                HmsContractRecordId = contractRecordModel?.Id,
                NetSuiteContractRecordId = netSuiteContractRecordId
            };
        }

        private async Task<T> UpsertCsvMapping<T>(int hospiceId, string mappedItem, T mapping, string type) where T : class
        {
            try
            {
                var csvMapping = await _csvMappingRepository.GetAsync(cm => cm.MappedTable == mappedItem && cm.HospiceId == hospiceId && cm.MappingType == type);
                var mappingInJson = JsonConvert.SerializeObject(mapping);
                if (csvMapping == null)
                {
                    csvMapping = new CsvMappings()
                    {
                        HospiceId = hospiceId,
                        MappedTable = mappedItem,
                        MappingInJson = mappingInJson,
                        MappingType = type
                    };
                    await _csvMappingRepository.AddAsync(csvMapping);
                }
                else
                {
                    csvMapping.MappingInJson = mappingInJson;
                    await _csvMappingRepository.UpdateAsync(csvMapping);
                }
                return JsonConvert.DeserializeObject<T>(csvMapping.MappingInJson);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while put Csv Mapping: {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while put Csv Mapping: {ex.Message}");
                throw ex;
            }
        }

        private async Task<Hospices> InsertHospiceWithLocation(NSViewModel.NSHospiceRequest hospiceRequest)
        {
            var hospiceModel = _mapper.Map<Hospices>(hospiceRequest);
            var sites = await GetSites(hospiceRequest);
            hospiceModel.CustomerTypeId = GetCustomerTypeId(hospiceRequest.CustomerType);

            if (hospiceModel.Address != null)
            {
                try
                {
                    var standardizedHospiceAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(hospiceModel.Address));
                    if (standardizedHospiceAddress != null)
                    {
                        hospiceModel.Address = _mapper.Map<Addresses>(standardizedHospiceAddress);
                    }
                }
                catch (ValidationException vx)
                {
                    _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
                }
                if (hospiceModel.Address.AddressUuid == Guid.Empty)
                {
                    hospiceModel.Address.AddressUuid = Guid.NewGuid();
                }
            }

            foreach (var location in hospiceModel.HospiceLocations)
            {
                var locationRequest = hospiceRequest.Locations.FirstOrDefault(l => l.NetSuiteCustomerId == location.NetSuiteCustomerId);
                location.SiteId = sites.FirstOrDefault(s => s.NetSuiteLocationId == locationRequest?.InternalWarehouseId)?.Id;
                location.CustomerTypeId = GetCustomerTypeId(locationRequest.CustomerType);
                if (location.Address != null)
                {
                    try
                    {
                        var standardizedLocationAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(location.Address));
                        if (standardizedLocationAddress != null)
                        {
                            location.Address = _mapper.Map<Addresses>(standardizedLocationAddress);
                        }
                    }
                    catch (ValidationException vx)
                    {
                        _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
                    }
                    if (location.Address.AddressUuid == Guid.Empty)
                    {
                        location.Address.AddressUuid = Guid.NewGuid();
                    }
                }
            }
            hospiceModel.AssignedSiteId = sites.FirstOrDefault(s => s.NetSuiteLocationId == hospiceRequest?.InternalWarehouseId)?.Id;
            await _hospiceRepository.AddAsync(hospiceModel);
            return hospiceModel;
        }

        private int? GetCustomerTypeId(string customerTypeString)
        {
            if (string.IsNullOrEmpty(customerTypeString))
            {
                return null;
            }

            if (!Enum.TryParse(customerTypeString, true, out Data.Enums.CustomerTypes customerType))
            {
                throw new ValidationException($"customer type {customerTypeString} is not valid");
            }
            return (int)customerType;
        }

        private async Task<Hospices> UpdateHospiceWithLocation(Hospices hospiceModel, NSViewModel.NSHospiceRequest hospiceRequest)
        {
            var updatedLocations = new List<HospiceLocations>();
            var sites = await GetSites(hospiceRequest);
            hospiceModel.CustomerTypeId = GetCustomerTypeId(hospiceRequest.CustomerType);

            foreach (var location in hospiceRequest.Locations)
            {
                var existingLocation = await _hospiceLocationRepository.GetAsync(l => l.NetSuiteCustomerId == location.NetSuiteCustomerId);
                var updatedLocation = _mapper.Map(location, existingLocation);
                updatedLocation.CustomerTypeId = GetCustomerTypeId(location.CustomerType);
                updatedLocation.SiteId = sites.FirstOrDefault(s => s.NetSuiteLocationId == location?.InternalWarehouseId)?.Id;
                if (updatedLocation.Address != null)
                {
                    try
                    {
                        var standardizedLocationAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(updatedLocation.Address));
                        if (standardizedLocationAddress != null)
                        {
                            _mapper.Map(standardizedLocationAddress, updatedLocation.Address);
                        }
                    }
                    catch (ValidationException vx)
                    {
                        _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
                    }
                    if (updatedLocation.Address.AddressUuid == Guid.Empty)
                    {
                        updatedLocation.Address.AddressUuid = Guid.NewGuid();
                    }
                }
                updatedLocations.Add(updatedLocation);
            }
            _mapper.Map<NSViewModel.NSHospiceRequest, Hospices>(hospiceRequest, hospiceModel);
            hospiceModel.HospiceLocations = updatedLocations;
            hospiceModel.AssignedSiteId = sites.FirstOrDefault(s => s.NetSuiteLocationId == hospiceRequest?.InternalWarehouseId)?.Id;
            if (hospiceModel.Address != null)
            {
                try
                {
                    var standardizedHospiceAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(hospiceModel.Address));
                    if (standardizedHospiceAddress != null)
                    {
                        _mapper.Map(standardizedHospiceAddress, hospiceModel.Address);
                    }
                }
                catch (ValidationException vx)
                {
                    _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
                }
                if (hospiceModel.Address.AddressUuid == Guid.Empty)
                {
                    hospiceModel.Address.AddressUuid = Guid.NewGuid();
                }
            }
            await _hospiceRepository.UpdateAsync(hospiceModel);
            return hospiceModel;
        }

        private async Task UpdateHospiceSubscriptionsFromNetSuite(int hospiceId, IEnumerable<int> netSuiteCustomerIds)
        {
            var existingSubscriptionForHospice = await _subscriptionRepository.GetManyAsync(s => s.HospiceId == hospiceId);

            var subscriptions = await _netSuiteService.GetSubscriptions(netSuiteCustomerIds);
            var newSubscriptionModels = new List<Subscriptions>();
            var netSuiteSubscriptionIds = subscriptions.Records.Select(s => int.Parse(s.NetSuiteSubscriptionId.Value));
            var existingSubscriptionModels = await _subscriptionRepository.GetManyAsync(s => s.NetSuiteSubscriptionId.HasValue && netSuiteSubscriptionIds.Contains(s.NetSuiteSubscriptionId.Value));
            foreach (var subscription in subscriptions.Records)
            {
                var existingSubscriptionModel = existingSubscriptionModels.FirstOrDefault(s => s.NetSuiteSubscriptionId == int.Parse(subscription.NetSuiteSubscriptionId.Value));
                if (existingSubscriptionModel != null)
                {
                    existingSubscriptionModel.NetSuiteLastFetchedDateTime = DateTime.UtcNow;
                    _mapper.Map(subscription, existingSubscriptionModel);
                }
                else
                {
                    var subscriptionModel = _mapper.Map<Data.Models.Subscriptions>(subscription);
                    subscriptionModel.HospiceId = hospiceId;
                    subscriptionModel.NetSuiteLastFetchedDateTime = DateTime.UtcNow;
                    newSubscriptionModels.Add(subscriptionModel);
                }
            }

            await _subscriptionRepository.AddManyAsync(newSubscriptionModels);
            await _subscriptionRepository.UpdateManyAsync(existingSubscriptionModels);
        }

        private async Task UpdateHospiceSubscriptionItemsFromNetSuite(int subscriptionId, Subscriptions subscriptionModel)
        {
            var existingSubscriptionItem = await _subscriptionItemRepository.GetManyAsync(s => s.SubscriptionId == subscriptionId);

            var subscriptionItems = await _netSuiteService.GetSubscriptionItemsBySubscription(subscriptionModel.NetSuiteSubscriptionId.Value);

            var newSubscriptionItemModels = new List<SubscriptionItems>();
            var netSuiteSubscriptionItemIds = subscriptionItems.Records.Select(s => int.Parse(s.NetSuiteSubscriptionItemId.Value));
            var existingSubscriptionItemModels = await _subscriptionItemRepository.GetManyAsync(s => s.NetSuiteSubscriptionItemId.HasValue && netSuiteSubscriptionItemIds.Contains(s.NetSuiteSubscriptionItemId.Value));

            foreach (var subscriptionItem in subscriptionItems.Records)
            {
                var netSuiteSubscriptionItemId = int.Parse(subscriptionItem.NetSuiteSubscriptionItemId.Value);

                var existingSubscriptionItemModel = existingSubscriptionItemModels.FirstOrDefault(s => s.NetSuiteSubscriptionItemId == netSuiteSubscriptionItemId);
                if (existingSubscriptionItemModel != null)
                {
                    existingSubscriptionItemModel.NetSuiteLastFetchedDateTime = DateTime.UtcNow;
                    _mapper.Map(subscriptionItem, existingSubscriptionItemModel);
                }
                else
                {
                    var subscriptionItemModel = _mapper.Map<SubscriptionItems>(subscriptionItem);

                    subscriptionItemModel.SubscriptionId = subscriptionModel.Id;
                    subscriptionItemModel.HospiceId = subscriptionModel.HospiceId;
                    subscriptionItemModel.NetSuiteLastFetchedDateTime = DateTime.UtcNow;
                    newSubscriptionItemModels.Add(subscriptionItemModel);
                }
            }

            await _subscriptionItemRepository.AddManyAsync(newSubscriptionItemModels);
            await _subscriptionItemRepository.UpdateManyAsync(existingSubscriptionItemModels);
        }

        private async Task UpdateHospiceContractsFromHms2Billing(int hospiceId, IEnumerable<int> hms2Ids, IEnumerable<Hms2HmsDigitalHospiceMappings> mappings)
        {
            var contracts = await _hms2BillingContractsRepository.GetManyAsync(c => c.HospiceId.HasValue && hms2Ids.Contains(c.HospiceId.Value));
            var newContractModels = new List<Hms2Contracts>();
            
            var existingContractModels = await _hms2ContractRepository.GetManyAsync(c => c.HospiceId == hospiceId);
            var existingContractIds = existingContractModels.Select(c => c.Hms2ContractId);
            foreach (var contract in contracts)
            {
                var existingContractModel = existingContractModels.FirstOrDefault(c => c.Hms2ContractId == contract.ContractId);
                if (existingContractModel != null)
                {
                    _mapper.Map(contract, existingContractModel);
                }
                else
                {
                    var contractModel = _mapper.Map<Hms2Contracts>(contract);
                    contractModel.HospiceId = hospiceId;
                    contractModel.HospiceLocationId = mappings.FirstOrDefault(m => m.Hms2id == contract.HospiceId)?.HospiceLocationId;
                    newContractModels.Add(contractModel);
                }
            }

            var removedContractIds = existingContractIds.Except(contracts.Select(c => c.ContractId));
            await _hms2ContractRepository.DeleteAsync(c => removedContractIds.Contains(c.Hms2ContractId));

            await _hms2ContractRepository.AddManyAsync(newContractModels);
            await _hms2ContractRepository.UpdateManyAsync(existingContractModels);
        }

        private async Task UpdateHospiceContractItemsFromHms2Billing(int contractId, int hms2ContractId)
        {
            var contractItems = await _hms2BillingContractItemsRepository.GetContractItemsByContract(ci => ci.ContractId == hms2ContractId);

            var newContractItemModels = new List<Hms2ContractItems>();
            
            var existingContractItemModels = await _hms2ContractItemRepository.GetManyAsync(ci => ci.ContractId == contractId);
            var existingContractItemIds = existingContractItemModels.Select(ci => ci.Hms2ContractItemId);
            var itemNumbers = contractItems.Select(ci => ci.ItemNumber);
            var items = await _itemRepository.GetManyAsync(i => itemNumbers.Contains(i.ItemNumber));
            foreach (var contractItem in contractItems)
            {
                var existingContractItemModel = existingContractItemModels.FirstOrDefault(ci => ci.Hms2ContractItemId == contractItem.InvctrId);
                if (existingContractItemModel != null)
                {
                    _mapper.Map(contractItem, existingContractItemModel);
                }
                else
                {
                    var contractItemModel = _mapper.Map<Hms2ContractItems>(contractItem);

                    contractItemModel.ContractId = contractId;
                    contractItemModel.ItemId = items.FirstOrDefault(i => i.ItemNumber == contractItem.ItemNumber)?.Id;
                    newContractItemModels.Add(contractItemModel);
                }
            }

            var removedContractItemIds = existingContractItemIds.Except(contractItems.Select(ci => ci.InvctrId));
            await _hms2ContractItemRepository.DeleteAsync(ci => removedContractItemIds.Contains(ci.Hms2ContractItemId));

            await _hms2ContractItemRepository.AddManyAsync(newContractItemModels);
            await _hms2ContractItemRepository.UpdateManyAsync(existingContractItemModels);
        }

        private async Task<IEnumerable<Sites>> GetSites(NSViewModel.NSHospiceRequest hospiceRequest)
        {
            var netSuiteSiteIds = new List<int>();

            if (hospiceRequest.InternalWarehouseId.HasValue)
            {
                netSuiteSiteIds.Add(hospiceRequest.InternalWarehouseId.Value);
            }

            if (hospiceRequest.Locations != null)
            {
                netSuiteSiteIds.AddRange(hospiceRequest.Locations?.Where(h => h.InternalWarehouseId.HasValue).Select(i => i.InternalWarehouseId.Value).ToList());
            }

            netSuiteSiteIds = netSuiteSiteIds.Distinct().ToList();

            var sites = await _sitesRepository.GetManyAsync(s => s.NetSuiteLocationId.HasValue && netSuiteSiteIds.Contains(s.NetSuiteLocationId.Value));
            if (netSuiteSiteIds.Count() != sites.Count())
            {
                var invalidSiteIds = netSuiteSiteIds.Except(sites.Select(s => s.NetSuiteLocationId.Value));
                throw new ValidationException($"Site/Warehouse with NetSuite internal ids ({string.Join(",", invalidSiteIds)}) does not exist");
            }
            return sites;
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

        private async Task StoreHospiceCreditHoldHistory(Hospices hospiceModel)
        {
            if (hospiceModel.CreditHoldByUser == null)
            {
                return;
            }
            var creditHoldHistory = _mapper.Map<Data.Models.CreditHoldHistory>(hospiceModel);
            await _creditHoldHistoryRepository.AddAsync(creditHoldHistory);
        }
    }
}
