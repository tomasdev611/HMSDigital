using AutoMapper;
using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.Data.Repositories.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.Extensions.Logging;
using Sieve.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HMSDigital.Core.Data.Models;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class SiteServiceAreaService : ISiteServiceAreaService
    {
        private readonly IMapper _mapper;

        private readonly ILogger<SiteServiceAreaService> _logger;

        private readonly ISiteServiceAreaRepository _siteServiceAreaRepository;

        private readonly ISitesRepository _sitesRepository;

        private readonly IPaginationService _paginationService;

        public SiteServiceAreaService(IMapper mapper,
                                      ISiteServiceAreaRepository siteServiceAreaRepository,
                                      IPaginationService paginationService,
                                      ISitesRepository sitesRepository,
                                      ILogger<SiteServiceAreaService> logger)
        {
            _mapper = mapper;
            _siteServiceAreaRepository = siteServiceAreaRepository;
            _sitesRepository = sitesRepository;
            _paginationService = paginationService;
            _logger = logger;
        }

        public async Task<IEnumerable<ServiceArea>> CreateServiceAreasForSite(int siteId, IEnumerable<int> siteServiceAreasRequest)
        {
            var site = await _sitesRepository.GetByIdAsync(siteId);
            if (site == null)
            {
                throw new ValidationException($"Site with Id ({siteId}) is invalid");
            }

            var zipCodeExists = await _siteServiceAreaRepository.ExistsAsync(c => siteServiceAreasRequest.Contains(c.ZipCode.Value));
            if (zipCodeExists)
            {
                throw new ValidationException($"Some of the zip codes you are trying to add already exists");
            }

            var siteServiceAreas = siteServiceAreasRequest.Select(zipCode => new SiteServiceAreas
            {
                SiteId = siteId,
                ZipCode = zipCode
            }).ToList();

            var addedServiceAreas = await _siteServiceAreaRepository.AddManyAsync(siteServiceAreas);

            return _mapper.Map<IEnumerable<ServiceArea>>(addedServiceAreas);
        }

        public async Task DeleteServiceAreasForSite(int siteId, IEnumerable<int> siteServiceAreasRequest)
        {
            var site = await _sitesRepository.GetByIdAsync(siteId);
            if (site == null)
            {
                throw new ValidationException($"Site with Id ({siteId}) is invalid");
            }

            await _siteServiceAreaRepository.DeleteAsync(c => siteServiceAreasRequest.Contains(c.ZipCode.Value)
                                                              && c.SiteId.HasValue
                                                              && c.SiteId.Value.Equals(siteId));
        }


        public async Task<PaginatedList<ViewModels.ServiceArea>> GetServiceAreasBySiteId(SieveModel sieveModel, int siteId)
        {
            _siteServiceAreaRepository.SieveModel = sieveModel;
            var totalCount = await _siteServiceAreaRepository.GetCountAsync(s => s.SiteId == siteId);
            var serviceAreaModel = await _siteServiceAreaRepository.GetManyAsync(s => s.SiteId == siteId);
            var serviceAreas = _mapper.Map<IEnumerable<ViewModels.ServiceArea>>(serviceAreaModel);
            return _paginationService.GetPaginatedList(serviceAreas, totalCount, sieveModel.Page, sieveModel.PageSize);
        }
    }
}
