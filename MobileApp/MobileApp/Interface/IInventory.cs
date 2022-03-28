using System.Collections.Generic;
using System.Threading.Tasks;
using JsonDiffPatchDotNet.Formatters.JsonPatch;
using MobileApp.Models;

using Refit;

namespace MobileApp.Interface
{
    public interface IInventory
    {
        [Get("/api/inventory")]
        Task<ApiResponse<PaginatedList<Inventory>>> GetInventoryListAsync(string filters);

        [Post("/api/inventory")]
        Task<ApiResponse<Inventory>> AdjustPhysicalInventory([Body] InventoryRequest inventory);

        [Patch("/api/inventory/{inventoryId}")]
        Task<ApiResponse<Inventory>> UpdatePhysicalInventory(int inventoryId, [Body] IList<Operation> operations);

        [Post("/api/inventory/move")]
        Task<ApiResponse<Inventory>> MoveInventoryAsync([Body] MoveInventoryRequest moveInventoryRequest);

        [Get("/api/inventory/patient/{patientUuid}")]
        Task<ApiResponse<PaginatedList<Inventory>>> GetPatientInventory(string patientUuid, string filters);
    }
}