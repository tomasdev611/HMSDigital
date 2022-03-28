using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.BusinessLayer.Constants;
using NotificationSDK.Interfaces;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using System.Linq;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Patient.FHIR.BusinessLayer.Interfaces;
using HMSDigital.Core.ViewModels.NetSuite;
using HMSDigital.Patient.FHIR.Models;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class HospiceV2Service: IHospiceV2Service
    {
        private readonly IHospiceRepository _hospiceRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IHospiceLocationRepository _hospiceLocationRepository;
        private readonly ISitesRepository _sitesRepository;

        private readonly IHospiceMemberService _hospiceMemberService;
        private readonly INotificationService _notificationService;
        private readonly IAddressStandardizerService _addressStandardizerService;
        private readonly IFHIRQueueService<FHIRHospice> _fhirQueueService;

        private readonly IMapper _mapper;
        private readonly ILogger<HospiceV2Service> _logger;

        public HospiceV2Service(
            IHospiceRepository hospiceRepository, 
            IRolesRepository rolesRepository,
            IHospiceLocationRepository hospiceLocationRepository,
            ISitesRepository sitesRepository,
            IHospiceMemberService hospiceMemberService,
            INotificationService notificationService,
            IAddressStandardizerService addressStandardizerService,
            IMapper mapper,
            ILogger<HospiceV2Service> logger,
            IFHIRQueueService<FHIRHospice> fhirQueueService) 
        {
            _hospiceRepository = hospiceRepository;
            _rolesRepository = rolesRepository;
            _hospiceLocationRepository = hospiceLocationRepository;
            _sitesRepository = sitesRepository;

            _hospiceMemberService = hospiceMemberService;
            _notificationService = notificationService;
            _addressStandardizerService = addressStandardizerService;
            _fhirQueueService = fhirQueueService;

            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HospiceResponse> UpsertHospiceWithLocation(NSHospiceRequest hospiceRequest)
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

                return _mapper.Map<HospiceResponse>(hospice);
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

        public async Task<Hospice> UpdateHospiceFhirOrganizationId(int hospiceId, Guid fhirOrganizationId)
        {
            try
            {
                var hospiceModel = await _hospiceRepository.GetByIdAsync(hospiceId);

                if (hospiceModel == null)
                {
                    throw new ValidationException($"Hospice with Id {hospiceId} not found");
                }

                hospiceModel.FhirOrganizationId = fhirOrganizationId;

                await _hospiceRepository.UpdateAsync(hospiceModel);
                return _mapper.Map<Hospice>(hospiceModel);
            }
            catch (ValidationException vx)
            {
                _logger.LogInformation($"Exception Occurred while updating hospice FHIR organization id with Id({hospiceId}): {vx.Message}");
                throw vx;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception Occurred while updating hospice FHIR organization id with Id({hospiceId}): {ex.Message}");
                throw ex;
            }
        }

        private async Task<Hospices> UpdateHospiceWithLocation(Hospices hospiceModel, NSHospiceRequest hospiceRequest)
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
            _mapper.Map(hospiceRequest, hospiceModel);
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

            var hospice = _mapper.Map<FHIRHospice>(hospiceModel);
            try
            {
                await _fhirQueueService.QueueUpdateRequest(hospice);
            }
            catch
            {
                _logger.LogInformation($"Exception occurred while sending FHIR hospice request to service bus");
                throw;
            }

            return hospiceModel;
        }

        private async Task<Hospices> InsertHospiceWithLocation(NSHospiceRequest hospiceRequest)
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

            var hospice = _mapper.Map<FHIRHospice>(hospiceModel);
            try
            {
                await _fhirQueueService.QueueCreateRequest(hospice);
            }
            catch
            {
                _logger.LogInformation($"Exception occurred while sending FHIR hospice request to service bus");
                await DeleteHospice(hospice.Id);
                throw;
            }

            return hospiceModel;
        }

        private async Task DeleteHospice(int hospiceId)
        {
            var hospiceModel = await _hospiceRepository.GetByIdAsync(hospiceId);
            if (hospiceModel == null)
            {
                return;
            }
            await _hospiceRepository.DeleteAsync(hospiceModel);
        }

        private async Task<IEnumerable<Sites>> GetSites(NSHospiceRequest hospiceRequest)
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
    }
}
