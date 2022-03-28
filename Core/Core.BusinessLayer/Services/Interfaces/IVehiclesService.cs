using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.JsonPatch;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IVehiclesService
    {
        Task<PaginatedList<Vehicle>> GetAllVehicles(SieveModel sieveModel);

        Task<Vehicle> GetVehicleById(int vehicleId);

        Task<PaginatedList<Vehicle>> SearchVehicles(SieveModel sieveModel, string searchQuery);

        Task<List<Site>> GetVehiclesByLocationId(int locationId);
    }
}
