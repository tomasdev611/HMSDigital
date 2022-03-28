using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using HospiceSource.Digital.NetSuite.SDK.ViewModels;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IPurchaseOrderService
    {
        Task<PaginatedList<PurchaseOrder>> GetOpenPurchaseOrders(int siteId, SieveModel sieveModel);

        Task<PurchaseOrder> ReceivePurchaseOrder(int netSuitePurchaseOrderId, ReceivePurchaseOrderRequest completeReceiptRequest);

        Task<IEnumerable<ReceiptImageUploadUrl>> GetReceiptImageUploadUrls(IEnumerable<ReceiptImageFileRequest> receiptImageFileRequest);

        Task<IEnumerable<string>> GetReceiptImageDownloadUrls(IEnumerable<string> storageFilePaths);
    }
}
