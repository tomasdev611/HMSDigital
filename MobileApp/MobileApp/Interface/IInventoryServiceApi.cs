using MobileApp.Models;
using Refit;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MobileApp.Interface
{
    public interface IInventoryServiceApi
    {
        [Get("/api/transfer-orders")]
        Task<ApiResponse<IEnumerable<TransferOrder>>> GetPendingTransferOrders(int siteId, bool truckTransferOrders);

        [Post("/api/transfer-orders")]
        Task<ApiResponse<TransferOrder>> CreateTransferOrder(TransferOrderCreateRequest request);

        [Post("/api/transfer-orders/{netSuiteTransferOrderId}/fulfill-receive")]
        Task<ApiResponse<FulfillReceiveOrderResponse>> FulfillReceiveTransferOrder(int netSuiteTransferOrderId, FulfillReceiveOrderRequest fulfillReceiveRequest);

        [Post("/api/purchase-orders/{purchaseOrderId}/receive")]
        Task<ApiResponse<ReceivePurchaseOrderResponse>> ReceivePurchaseOrder(int purchaseOrderId, ReceivePurchaseOrderRequest request);
    }
}