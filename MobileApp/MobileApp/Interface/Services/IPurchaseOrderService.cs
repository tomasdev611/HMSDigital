using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MobileApp.Models;

namespace MobileApp.Interface.Services
{
    public interface IPurchaseOrderService
    {
        /// <summary>
        /// Get the Purchase Orders from the service
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PurchaseOrderModel>> GetPurchaseOrders(int siteId);

        /// <summary>
        /// Get the Purchase Orders from the service
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<PurchaseOrderReceiptModel>> GetPurchaseOrderReceipts(int purchaseOrderId, int siteId);

        /// <summary>
        /// Get the Purchase Order by Id
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        Task<IEnumerable<PurchaseOrderModel>> GetPurchaseOrdersById(int purchaseOrderId, int siteId);

        /// <summary>
        /// Receive the Purchase Order
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <param name="lineItemModels"></param>
        /// <param name="imageUrls"></param>
        /// <returns></returns>
        Task<ReceivePurchaseOrderResponse> ReceivePurchaseOrder(int purchaseOrderId, ReceivePurchaseOrderRequest request);
    }
}