using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using Microsoft.AspNetCore.JsonPatch;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<PaginatedList<Inventory>> GetAllInventory(SieveModel sieveModel);

        Task<Inventory> GetInventoryById(int inventoryId);

        Task<PaginatedList<PatientInventory>> GetPatientInventory(string patientUuid, SieveModel sieveModel, bool includePickupDetails, bool includeDeliveryAddress);

        Task<PatientInventory> AddPatientInventory(Guid patientUuid, PatientInventoryRequest patientInventoryRequest);

        Task CreateInventory(InventoryRequest inventoryRequest);

        Task<ViewModels.NetSuite.NSInventoryResponse> UpsertInventory(ViewModels.NetSuite.NSInventoryRequest inventoryRequest);

        Task<Inventory> PatchInventory(int inventoryId, JsonPatchDocument<Inventory> inventoryPatchDocument);

        Task DeleteInventory(int inventoryId);

        Task<ViewModels.NetSuite.InventoryResponse> DeleteInventoryByNetSuiteId(ViewModels.NetSuite.InventoryDeleteRequest inventoryDeleteRequest);

        Task<Data.Models.Sites> GetSiteByNetSuiteLocationId(int locationId);

        Task<PaginatedList<Inventory>> SearchInventoryBySearchQuery(SieveModel sieveModel, string searchQuery);

        Task<Inventory> MoveInventory(MoveInventoryRequest moveInventoryRequest);

        Task<string> GetLocationName(int? locationId, string locationLabel);

        Task<PaginatedList<PatientInventory>> SearchPatientInventoryBySearchQuery(string patientUuid, SieveModel sieveModel, string searchQuery);

        Task<NSUpdateInventoryResponse> UpdateNetSuiteInventory(int inventoryId, NSUpdateInventoryRequest updateNetsuiteInventoryRequest);

        Task<IEnumerable<NSAddInventoryResponse>> AddNetSuiteInventory(NSAddInventoryRequest addInventoryRequest);
    }
}
