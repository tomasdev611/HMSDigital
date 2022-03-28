using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface ISitesService
    {
        Task<PaginatedList<Site>> GetAllSites(SieveModel sieveModel);

        Task<Site> GetSiteById(int siteId);

        Task<WarehouseResponse> UpsertWarehouse(WarehouseRequest warehouseRequest);

        Task<PaginatedList<Site>> SearchSites(SieveModel sieveModel, string searchQuery);

        Task<IEnumerable<Role>> GetInternalRoles(int siteId);

        Task DeleteWarehouse(int netSuiteInternalId);

        Task<IEnumerable<int>> GetSiteIdsByParentLocationId(int siteId);

        Task<int> GetNetSuiteLocationId(int siteId);

        Task<IEnumerable<Site>> GetSitesByNetsuiteLocationIds(List<int> netsuiteLocationIds);
    }
}
