using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Enums;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class SitesService : ISitesService
    {
        private readonly ISitesRepository _sitesRepository;

        private readonly IMapper _mapper;

        private readonly IRolesRepository _rolesRepository;

        private readonly IPaginationService _paginationService;

        private readonly IAddressStandardizerService _addressStandardizerService;

        private readonly ILogger<SitesService> _logger;

        public SitesService(IMapper mapper,
            ISitesRepository sitesRepository,
            IPaginationService paginationService,
            IRolesRepository rolesRepository,
            IAddressStandardizerService addressStandardizerService,
            ILogger<SitesService> logger)
        {
            _mapper = mapper;
            _sitesRepository = sitesRepository;
            _rolesRepository = rolesRepository;
            _paginationService = paginationService;
            _addressStandardizerService = addressStandardizerService;
            _logger = logger;

        }

        public async Task<PaginatedList<Site>> GetAllSites(SieveModel sieveModel)
        {
            _sitesRepository.SieveModel = sieveModel;
            var totalRecords = await _sitesRepository.GetCountAsync(s => true);
            var siteModels = await _sitesRepository.GetAllAsync();
            var sites = _mapper.Map<IEnumerable<Site>>(siteModels);
            return _paginationService.GetPaginatedList(sites, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PaginatedList<Site>> SearchSites(SieveModel sieveModel, string searchQuery)
        {
            _sitesRepository.SieveModel = sieveModel;
            var totalCount = await _sitesRepository.GetCountAsync(s => s.Name.Contains(searchQuery));
            var siteModels = await _sitesRepository.GetManyAsync(s => s.Name.Contains(searchQuery));
            var sites = _mapper.Map<IEnumerable<Site>>(siteModels);
            return _paginationService.GetPaginatedList(sites, totalCount, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<Site> GetSiteById(int siteId)
        {
            var siteModel = await _sitesRepository.GetByIdAsync(siteId);
            return _mapper.Map<Site>(siteModel);
        }

        public async Task<WarehouseResponse> UpsertWarehouse(WarehouseRequest warehouseRequest)
        {
            var existingSiteModel = await _sitesRepository.GetAsync(s => s.NetSuiteLocationId == warehouseRequest.NetSuiteLocationId);
            if (existingSiteModel != null)
            {
                return await UpdateWarehouse(existingSiteModel, warehouseRequest);
            }
            return await CreateWarehouse(warehouseRequest);
        }

        public async Task<IEnumerable<Role>> GetInternalRoles(int siteId)
        {
            var internalRoles = await _rolesRepository.GetManyAsync(r => r.RoleType == RoleTypes.Internal.ToString());
            return _mapper.Map<IEnumerable<Role>>(internalRoles);
        }

        public async Task DeleteWarehouse(int netSuiteInternalId)
        {
            var siteModel = await _sitesRepository.GetAsync(s => s.NetSuiteLocationId == netSuiteInternalId);
            if (siteModel == null)
            {
                return;
            }
            await _sitesRepository.DeleteAsync(siteModel);
        }

        public async Task<IEnumerable<int>> GetSiteIdsByParentLocationId(int locationId)
        {
            var siteModel = await _sitesRepository.GetByIdAsync(locationId);

            if (siteModel == null)
            {
                throw new ValidationException($"Location Id({locationId}) is not valid");
            }

            if (siteModel.LocationType.ToLower() != "site" 
                && siteModel.LocationType.ToLower() != "area" 
                && siteModel.LocationType.ToLower() != "region") 
            {
                throw new ValidationException($"Location type({siteModel.LocationType}) for Location Id({locationId}) is not valid");
            }

            List<int> siteIds = new List<int>();

            if (siteModel.LocationType.ToLower() == "site")
            {
                siteIds.Add(siteModel.Id);
                return siteIds;
            }

            if (siteModel.LocationType.ToLower() == "area")
            {
                var siteModels = await _sitesRepository.GetManyAsync(s => s.ParentNetSuiteLocationId == siteModel.NetSuiteLocationId
                                                                           && s.LocationType.ToLower() == "site");
                if (siteModels != null && siteModels.Count() > 0) {
                    siteIds = siteModels.Select(s => s.Id).ToList();
                    return siteIds;
                }
            }

            if (siteModel.LocationType.ToLower() == "region")
            {
                var areaModels = await _sitesRepository.GetManyAsync(s => s.ParentNetSuiteLocationId == siteModel.NetSuiteLocationId
                                                                           && s.LocationType.ToLower() == "area");

                if (areaModels != null && areaModels.Count() > 0)
                {

                    var areaNetSuitLocationIds = areaModels.Select(l => l.NetSuiteLocationId);

                    var sites = await _sitesRepository.GetManyAsync(s => s.ParentNetSuiteLocationId != null
                                                                        && areaNetSuitLocationIds.Contains(s.ParentNetSuiteLocationId.Value)
                                                                        && s.LocationType.ToLower() == "site");
                    if (sites != null && sites.Count() > 0)
                    {
                        siteIds.AddRange(sites.Select(s => s.Id).ToList());
                        return siteIds;
                    }
                }
            }
            return siteIds;
        }

        public async Task<int> GetNetSuiteLocationId(int siteId)
        {
            var siteModel = await _sitesRepository.GetByIdAsync(siteId);

            if (siteModel == null)
            {
                throw new ValidationException($"SiteId ({siteId}) is not valid");
            }

            return siteModel.NetSuiteLocationId ?? 0;
        }



        public async Task<IEnumerable<Site>> GetSitesByNetsuiteLocationIds(List<int> netsuiteLocationIds)
        {
            if (netsuiteLocationIds == null || !netsuiteLocationIds.Any()) 
            {
                return null;
            }

            var siteModels = await _sitesRepository.GetManyAsync(x => x.NetSuiteLocationId.HasValue && netsuiteLocationIds.Contains(x.NetSuiteLocationId.Value));

            return _mapper.Map<IEnumerable<Site>>(siteModels);
        }

        private async Task<WarehouseResponse> CreateWarehouse(WarehouseRequest warehouseRequest)
        {
            var siteModel = _mapper.Map<Sites>(warehouseRequest);

            try
            {
                var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(warehouseRequest.Address));
                if (standardizedAddress != null)
                {
                    siteModel.Address = _mapper.Map<Addresses>(standardizedAddress);
                }
            }
            catch (ValidationException vx)
            {
                _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
            }
            if (siteModel.Address.AddressUuid == Guid.Empty)
            {
                siteModel.Address.AddressUuid = Guid.NewGuid();
            }

            await _sitesRepository.AddAsync(siteModel);
            return _mapper.Map<WarehouseResponse>(siteModel);
        }

        private async Task<WarehouseResponse> UpdateWarehouse(Data.Models.Sites existingSite, WarehouseRequest warehouseRequest)
        {
            var siteModel = _mapper.Map<WarehouseRequest, Sites>(warehouseRequest, existingSite);

            try
            {
                var standardizedAddress = await _addressStandardizerService.GetStandardizedAddress(_mapper.Map<Address>(siteModel.Address));
                if (standardizedAddress != null)
                {
                    _mapper.Map(standardizedAddress, siteModel.Address);
                }
            }
            catch (ValidationException vx)
            {
                _logger.LogWarning($"Error Occurred while standardizing address:{vx.Message}");
            }
            if (siteModel.Address.AddressUuid == Guid.Empty)
            {
                siteModel.Address.AddressUuid = Guid.NewGuid();
            }
            await _sitesRepository.UpdateAsync(siteModel);
            return _mapper.Map<WarehouseResponse>(siteModel);
        }
    }
}
