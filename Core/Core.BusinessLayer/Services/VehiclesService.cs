using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.BusinessLayer.Sieve;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sieve.Services;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class VehiclesService : IVehiclesService
    {
        private readonly ISitesRepository _sitesRepository;

        private readonly IMapper _mapper;

        private readonly IPaginationService _paginationService;

        private readonly IAddressStandardizerService _addressStandardizerService;

        private readonly ISieveProcessor _sieveProcessor;

        public VehiclesService(IMapper mapper,
            IPaginationService paginationService,
            ISitesRepository sitesRepository,
            IAddressStandardizerService addressStandardizerService,
            ISieveProcessor sieveProcessor
            )
        {
            _mapper = mapper;
            _sitesRepository = sitesRepository;
            _paginationService = paginationService;
            _addressStandardizerService = addressStandardizerService;
            _sieveProcessor = sieveProcessor;
        }

        public async Task<PaginatedList<Vehicle>> GetAllVehicles(SieveModel sieveModel)
        {
            var vehicleModels = await _sitesRepository.GetManyAsync(s => s.LocationType.ToLower() == "vehicle");

            var vehicles = _mapper.Map<IEnumerable<Vehicle>>(vehicleModels);

            await AddSiteToVehicles(vehicles);

            var totalRecords = _sieveProcessor.Apply(sieveModel, vehicles.AsQueryable(), null, true, false, false);

            var vehiclesFiltered = _sieveProcessor.Apply(sieveModel, vehicles.AsQueryable());
            
            return _paginationService.GetPaginatedList(vehiclesFiltered, totalRecords.Count(), sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PaginatedList<Vehicle>> SearchVehicles(SieveModel sieveModel, string searchQuery)
        {
            _sitesRepository.SieveModel = sieveModel;
            var totalCount = await _sitesRepository.GetCountAsync(v => v.Name.Contains(searchQuery) && v.LocationType.ToLower() == "vehicle");
            var vehicleModels = await _sitesRepository.GetManyAsync(v => v.LocationType.ToLower() == "vehicle"
                                                                    && (v.Name.Contains(searchQuery) || v.Cvn.Contains(searchQuery)));
            var vehicles = _mapper.Map<IEnumerable<Vehicle>>(vehicleModels);
            await AddSiteToVehicles(vehicles);
            return _paginationService.GetPaginatedList(vehicles, totalCount, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<Vehicle> GetVehicleById(int vehicleId)
        {
            var vehicleModel = await _sitesRepository.GetAsync(v => v.Id == vehicleId && v.LocationType.ToLower() == "vehicle");

            var vehicle = _mapper.Map<Vehicle>(vehicleModel);
            await AddSiteToVehicles(new List<Vehicle>() { vehicle });
            return vehicle;
        }
        
        public async Task<List<Site>> GetVehiclesByLocationId(int locationId)
        {
            var totalRecords = await _sitesRepository.GetAllAsync();
            var sites = totalRecords.Where(s => s.ParentNetSuiteLocationId == locationId);
            var sitesForVehicle = _mapper.Map<List<Site>>(sites);
            sitesForVehicle.ForEach(siteForVehicle => siteForVehicle.Vehicles = GetVehiclesBySites(siteForVehicle, totalRecords));
            sitesForVehicle = CleanUpSitesForVehicle(sitesForVehicle);
            return sitesForVehicle;
        }


        private async Task AddSiteToVehicles(IEnumerable<Vehicle> vehicles)
        {
            var parentNetSuiteLocationIds = vehicles.Where(v => v.ParentNetSuiteLocationId.HasValue).Select(v => v.ParentNetSuiteLocationId);
            var sitesForVehicle = await _sitesRepository.GetManyAsync(s => parentNetSuiteLocationIds.Contains(s.NetSuiteLocationId));
            foreach (var vehicle in vehicles)
            {
                var assignedSite = sitesForVehicle.FirstOrDefault(s => s.NetSuiteLocationId == vehicle.ParentNetSuiteLocationId);
                if (assignedSite != null)
                {
                    vehicle.SiteId = assignedSite.Id;
                    vehicle.SiteName = assignedSite.Name;
                }
            }
        }

        private List<Site> GetVehiclesBySites(Site siteForVehicle, IEnumerable<Sites> totalRecords)
        {
            var sites = totalRecords.Where(s => s.ParentNetSuiteLocationId == siteForVehicle.NetSuiteLocationId);
            siteForVehicle.Vehicles = _mapper.Map<List<Site>>(sites);
            siteForVehicle?.Vehicles?.ForEach(siteForVehicle => siteForVehicle.Vehicles = GetVehiclesBySites(siteForVehicle, totalRecords));
            return siteForVehicle?.Vehicles;
        }

        private List<Site> CleanUpSitesForVehicle(List<Site> sitesForVehicle)
        {
            var sitesForVehicleTemp = new List<Site>();
            foreach (var siteForVehicle in sitesForVehicle)
            {
                if (siteForVehicle.Vehicles.Count > 0 || siteForVehicle.LocationType.ToLower() == "vehicle")
                {
                    sitesForVehicleTemp.Add(siteForVehicle);
                    continue;
                }
                siteForVehicle.Vehicles = CleanUpSitesForVehicle(siteForVehicle.Vehicles);
            }
            return sitesForVehicleTemp;
        }
    }
}
