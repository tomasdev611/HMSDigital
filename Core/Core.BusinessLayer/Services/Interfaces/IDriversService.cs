using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IDriversService
    {
        Task<PaginatedList<ViewModels.Driver>> GetAllDrivers(SieveModel SieveModel);

        Task<Driver> GetMyDriver();

        Task<ViewModels.Driver> GetDriverById(int driverId);

        Task<ViewModels.Driver> CreateDriver(DriverBase driverCreateRequest);

        Task<ViewModels.Driver> UpdateDriver(int driverId, DriverBase driverUpdateRequest);

        Task DeleteDriver(int driverId);

         Task<IEnumerable<ViewModels.Driver>> SearchDrivers(ViewModels.DriversRequest driversRequest);

        Task<PaginatedList<ViewModels.Driver>> SearchDriversBySearchQuery(SieveModel sieveModel, string searchQuery);

        Task<Driver> UpdateMyVehicle(int vehicleId);

        Task<ViewModels.Driver> UpdateMyLocation(GeoLocation location);
    }
}
