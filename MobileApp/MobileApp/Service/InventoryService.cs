using System.Collections.Generic;
using System.Threading.Tasks;
using JsonDiffPatchDotNet.Formatters.JsonPatch;
using MobileApp.Interface;
using MobileApp.Models;
using MobileApp.Utils;
using Refit;

namespace MobileApp.Service
{
    public class InventoryService
    {
        private readonly IInventory _inventory;

        public InventoryService()
        {
            _inventory = RestService.For<IInventory>(HMSHttpClientFactory.GetCoreHttpClient());
        }

        internal async Task<bool> AdjustPhysicalInventoryAtLocation(Inventory inventory, int currentLocationId)
        {
            try
            {
                var inventoryReq = new InventoryRequest()
                {
                    SerialNumber = inventory.SerialNumber,
                    AssetTagNumber = inventory.AssetTagNumber,
                    LotNumber = inventory.LotNumber,
                    ItemId = inventory.Item.Id,
                    Count = (
                            inventory.Item.IsSerialized
                            || inventory.Item.IsAssetTagged
                            || inventory.Item.IsLotNumbered
                            ) ? 1 : inventory.Count,
                    CurrentLocationId = currentLocationId
                };
                var response = await _inventory.AdjustPhysicalInventory(inventoryReq);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                throw;
            }
        }

        internal async Task<IEnumerable<Inventory>> GetInventory(string filterString)
        {
            try
            {
                var response = await _inventory.GetInventoryListAsync(filterString);
                if (response != null)
                {
                    return response.Content.Records;
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        internal async Task<IEnumerable<Inventory>> GetPatientInventory(string patientUuid, string filterString)
        {
            try
            {
                var response = await _inventory.GetPatientInventory(patientUuid, filterString);
                if (response != null)
                {
                    return response.Content.Records;
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        internal async Task<bool> MoveInventory(MoveInventoryRequest moveInventoryRequest)
        {
            try
            {
                var moveResponse = await _inventory.MoveInventoryAsync(moveInventoryRequest);
                return moveResponse.IsSuccessStatusCode;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Inventory>> GetCurrentInventoryAsync()
        {
            var filterString = "";

            try
            {
                var currentVehicleId = await UserDetailsUtils.GetUsersCurrentVehicleId();
                if (currentVehicleId == null)
                {
                    var currentSiteId = await UserDetailsUtils.GetUsersCurrentSiteId();
                    filterString = "currentLocationId==" + currentSiteId;
                }
                else
                {
                    filterString = "currentLocationId==" + currentVehicleId;
                }

                var response = await _inventory.GetInventoryListAsync(filterString);
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.Records;
                }
                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> UpdatePhysicalInventory(IList<Operation> operations, int inventoryId)
        {
            try
            {
                var updateInventoryResponse = await _inventory.UpdatePhysicalInventory(inventoryId, operations);
                if (updateInventoryResponse.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
