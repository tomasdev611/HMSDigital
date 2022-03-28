using MobileApp.Models;
using Refit;
using System.Threading.Tasks;

namespace MobileApp.Interface
{
    public interface IPurchaseOrderApi
    {
        [Get("/api/purchase-orders")]
        Task<ApiResponse<PaginatedList<PurchaseOrderModel>>> GetOpenPurchaseOrders(string filters, int siteId);
    }
}