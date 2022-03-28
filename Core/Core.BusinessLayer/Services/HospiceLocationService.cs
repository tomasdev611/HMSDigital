using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Config;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using LinqKit;
using Microsoft.Extensions.Options;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class HospiceLocationService : IHospiceLocationService
    {
        private readonly IHospiceLocationRepository _hospiceLocationRepository;

        private readonly IContractRecordsRepository _contractRecordsRepository;

        private readonly IHMS2ContractItemRepository _hms2ContractItemRepository;

        private readonly IMapper _mapper;

        private readonly IPaginationService _paginationService;

        private readonly ContractConfig _contractConfig;

        public HospiceLocationService(IHospiceLocationRepository hospiceLocationRepository,
            IContractRecordsRepository contractRecordsRepository,
            IHMS2ContractItemRepository hms2ContractItemRepository,
            IMapper mapper,
            IPaginationService paginationService,
            IOptions<ContractConfig> contractOptions)
        {
            _mapper = mapper;
            _hospiceLocationRepository = hospiceLocationRepository;
            _contractRecordsRepository = contractRecordsRepository;
            _hms2ContractItemRepository = hms2ContractItemRepository;
            _paginationService = paginationService;
            _contractConfig = contractOptions.Value;
        }

        public async Task<PaginatedList<HospiceLocation>> GetAllHospiceLocations(int hospiceId, SieveModel sieveModel)
        {
            _hospiceLocationRepository.SieveModel = sieveModel;
            var predicate = PredicateBuilder.New<HospiceLocations>(true);
            if (hospiceId != 0)
            {
                predicate.And(l => l.HospiceId == hospiceId);
            }
            var totalRecords = await _hospiceLocationRepository.GetCountAsync(predicate);
            var hospiceLocationModels = await _hospiceLocationRepository.GetManyAsync(predicate);
            var hospiceLocations = _mapper.Map<IEnumerable<HospiceLocation>>(hospiceLocationModels);
            return _paginationService.GetPaginatedList(hospiceLocations, totalRecords, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<HospiceLocation> GetHospiceLocationById(int locationId)
        {
            var hospiceLocationModel = await _hospiceLocationRepository.GetByIdAsync(locationId);
            return _mapper.Map<HospiceLocation>(hospiceLocationModel);
        }

        public async Task<PaginatedList<CatalogItem>> GetProductsCatalog(int hospiceId, int locationId, SieveModel sieveModel)
        {
            var hospiceLocationModel = await _hospiceLocationRepository.GetAsync(l => l.HospiceId == hospiceId && l.Id == locationId);

            if (hospiceLocationModel == null)
            {
                throw new ValidationException($"hospice location Id({locationId}) is not valid");
            }
            if (string.Equals(_contractConfig.ContractSource, "ZAB", StringComparison.OrdinalIgnoreCase))
            {
                return await GetNetSuiteProductCatalog(hospiceLocationModel, sieveModel);
            }
            return await GetHMS2ProductCatalog(hospiceLocationModel, sieveModel);

        }

        public async Task<IEnumerable<HospiceLocation>> SearchHospiceLocations(int hospiceId, string searchQuery)
        {
            var hospiceLocationModels = await _hospiceLocationRepository.GetManyAsync(l => l.HospiceId == hospiceId && l.Name.Contains(searchQuery));
            return _mapper.Map<IEnumerable<HospiceLocation>>(hospiceLocationModels);
        }

        private async Task<PaginatedList<CatalogItem>> GetNetSuiteProductCatalog(Data.Models.HospiceLocations hospiceLocationModel, SieveModel sieveModel)
        {
            var currentDate = DateTime.UtcNow.Date;
            var predicate = PredicateBuilder.New<ContractRecords>(true);
            if (hospiceLocationModel != null)
            {
                predicate.And(c => c.NetSuiteCustomerId == hospiceLocationModel.NetSuiteContractingCustomerId
                                && c.EffectiveStartDate <= currentDate
                                && c.EffectiveEndDate >= currentDate
                                && c.ShowOnOrderScreen
                                && c.ItemId.HasValue);
            }
            var contractRecords = await _contractRecordsRepository.GetContractRecordsAsync(predicate);
            var contractItemIds = contractRecords.Select(c => c.ItemId.Value);
            _contractRecordsRepository.SieveModel = sieveModel;
            var totalRecords = await _contractRecordsRepository.GetCountAsync(predicate);
            var productsCatalogModel = await _contractRecordsRepository.GetManyAsync(predicate);
            var productsCatalog = _mapper.Map<IEnumerable<CatalogItem>>(productsCatalogModel);
            foreach (var product in productsCatalog)
            {
                foreach (var productGroup in product.Item.AddOnGroups)
                {
                    productGroup.AddOnGroupProducts = productGroup.AddOnGroupProducts.Where(p => contractItemIds.Contains(p.ItemId)).ToList();
                    foreach (var AddOnGroupProduct in productGroup.AddOnGroupProducts)
                    {
                        AddOnGroupProduct.Rate = contractRecords.FirstOrDefault(c => c.ItemId == AddOnGroupProduct.ItemId)?.Rate;
                    }
                }
            }

            return _paginationService.GetPaginatedList(productsCatalog, totalRecords, sieveModel?.Page, sieveModel?.PageSize);
        }

        private async Task<PaginatedList<CatalogItem>> GetHMS2ProductCatalog(Data.Models.HospiceLocations hospiceLocationModel, SieveModel sieveModel)
        {

            var currentDate = DateTime.UtcNow.Date;
            var predicate = PredicateBuilder.New<Hms2ContractItems>(true);
            if (hospiceLocationModel != null)
            {
                predicate.And(c => c.Contract != null
                                && (c.Contract.HospiceLocationId == hospiceLocationModel.Id || c.Contract.HospiceId == hospiceLocationModel.HospiceId)
                                && c.Contract.StartDate <= currentDate
                                && c.Contract.EndDate >= currentDate
                                && c.ShowOnOrderScreen
                                && c.ItemId.HasValue);
            }
            var contractItems = await _hms2ContractItemRepository.GetContractItemsAsync(predicate);
            var contractItemIds = contractItems.Select(c => c.Id);
            var locationContractItemExists = await _hms2ContractItemRepository.ExistsAsync(c => contractItemIds.Contains(c.Id) && c.Contract.HospiceLocationId == hospiceLocationModel.Id);
            if (locationContractItemExists)
            {
                contractItems = contractItems.Where(c => c.Contract.HospiceLocationId == hospiceLocationModel.Id).ToList();
                predicate.And(c => c.Contract.HospiceLocationId == hospiceLocationModel.Id);
            }
            var itemIds = contractItems.Select(c => c.ItemId.Value);
            _hms2ContractItemRepository.SieveModel = sieveModel;
            var totalRecords = await _hms2ContractItemRepository.GetCountAsync(predicate);
            var productsCatalogModel = await _hms2ContractItemRepository.GetManyAsync(predicate);
            var productsCatalog = _mapper.Map<IEnumerable<CatalogItem>>(productsCatalogModel);
            foreach (var product in productsCatalog)
            {
                foreach (var productGroup in product.Item.AddOnGroups)
                {
                    productGroup.AddOnGroupProducts = productGroup.AddOnGroupProducts.Where(p => itemIds.Contains(p.ItemId)).ToList();
                    foreach (var addOnGroupProduct in productGroup.AddOnGroupProducts)
                    {
                        var contractItem = contractItems.FirstOrDefault(c => c.ItemId == addOnGroupProduct.ItemId);
                        if (contractItem != null)
                        {
                            addOnGroupProduct.Rate = (double)(contractItem.SalePrice > 0 ? contractItem.SalePrice : contractItem.RentalPrice);
                        }
                    }
                }
            }

            return _paginationService.GetPaginatedList(productsCatalog, totalRecords, sieveModel?.Page, sieveModel?.PageSize);
        }
    }
}
