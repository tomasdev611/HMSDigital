using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobileApp.Models;

namespace MobileApp.Interface.Services
{
    public interface IInventoryService
    {
        /// <summary>
        /// Get the Purchase Orders from the service
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TransferOrder>> GetPendingTransferOrders(int siteId, bool truckTransferOrders);

        /// <summary>
        /// Receive the Purchase Order
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <param name="lineItemModels"></param>
        /// <param name="imageUrls"></param>
        /// <returns></returns>
        Task<FulfillReceiveOrderResponse> FulfillReceiveOrder(int purchaseOrderId, FulfillReceiveOrderRequest request);
    }
}