using System.Collections.Generic;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface ISiteServiceAreaService
    {
        Task<PaginatedList<ServiceArea>> GetServiceAreasBySiteId(SieveModel sieveModel, int siteId);
        Task<IEnumerable<ServiceArea>> CreateServiceAreasForSite(int siteId, IEnumerable<int> siteServiceAreas);
        Task DeleteServiceAreasForSite(int siteId, IEnumerable<int> siteServiceAreasRequest);
    }
}
