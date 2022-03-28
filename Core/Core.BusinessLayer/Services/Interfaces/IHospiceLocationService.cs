using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IHospiceLocationService
    {
        Task<PaginatedList<HospiceLocation>> GetAllHospiceLocations(int hospiceId, SieveModel sieveModel);

        Task<HospiceLocation> GetHospiceLocationById(int locationId);

        Task<IEnumerable<HospiceLocation>> SearchHospiceLocations(int hospiceId, string searchQuery);

        Task<PaginatedList<CatalogItem>> GetProductsCatalog(int hospiceId, int locationId, SieveModel sieveModel);
    }
}
