using MobileApp.Models;
using Refit;
using System.Threading.Tasks;

namespace MobileApp.Interface
{
    public interface IDispatchApi
    {
        [Get("/api/dispatch/loadlist")]
        Task<ApiResponse<SiteLoadList>> GetSiteLoadListAsync(int siteId, string filters);

        [Post("/api/dispatch/pickup")]
        Task<ApiResponse<string>> SendPickupRequestAsync([Body] DispatchMovementRequest dispatchMovementRequest);

        [Post("/api/dispatch/fulfill-order")]
        Task<ApiResponse<string>> SendFullfillOrderRequestAsync([Body] OrderFulfillmentRequest orderFulfillmentRequest);

        [Get("/api/order-headers/{orderId}/fulfillment")]
        Task<ApiResponse<PaginatedList<OrderFulfillmentLineItem>>> GetFullfillOrderItemsAsync(int orderId);

        [Post("/api/dispatch/update-status")]
        Task<ApiResponse<string>> UpdateOrderStatus([Body] OrderStatusUpdateRequest orderStatusUpdateReq);
    }
}
