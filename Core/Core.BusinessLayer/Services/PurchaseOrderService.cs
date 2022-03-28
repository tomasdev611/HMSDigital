using HMSDigital.Common.BusinessLayer.Services.Interfaces;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using HospiceSource.Digital.NetSuite.SDK.ViewModels;
using Sieve.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly INetSuiteService _netSuiteService;
        private readonly IPaginationService _paginationService;
        private readonly IFileStorageService _fileStorageService;
        private readonly ISitesService _sitesService;

        public PurchaseOrderService(
            INetSuiteService netSuiteService, 
            IPaginationService paginationService,
            IFileStorageService fileStorageService,
            ISitesService sitesService) 
        {
            _netSuiteService = netSuiteService;
            _paginationService = paginationService;
            _fileStorageService = fileStorageService;
            _sitesService = sitesService;
        }

        public async Task<PaginatedList<PurchaseOrder>> GetOpenPurchaseOrders(int siteId, SieveModel sieveModel)
        {
            var responce = await _netSuiteService.GetPurchaseOrders(new GetPurchaseOrdersRequest
            {
                PageNumber = sieveModel.Page,
                PageSize = sieveModel.PageSize,
                StatusId = 0,
                NetSuiteLocationId = await _sitesService.GetNetSuiteLocationId(siteId)
            });

            return _paginationService.GetPaginatedList(responce.Records ?? new List<PurchaseOrder>(), responce.TotalRecordCount, sieveModel.Page, sieveModel.PageSize);
        }

        public async Task<PurchaseOrder> ReceivePurchaseOrder(int netSuitePurchaseOrderId, ReceivePurchaseOrderRequest receivePurchaseOrderRequest)
        {
            if (netSuitePurchaseOrderId != receivePurchaseOrderRequest.NetSuitePurchaseOrderId) 
            {
                throw new ValidationException($"NetSuitePurchaseOrderId ({receivePurchaseOrderRequest.NetSuitePurchaseOrderId}) is not valid");
            }

            return await _netSuiteService.ReceivePurchaseOrder(receivePurchaseOrderRequest);
        }

        public async Task<IEnumerable<ReceiptImageUploadUrl>> GetReceiptImageUploadUrls(IEnumerable<ReceiptImageFileRequest> receiptImageFileRequestList)
        {
            List<ReceiptImageUploadUrl> result = null;

            if (receiptImageFileRequestList != null && receiptImageFileRequestList.Any())
            {
                result = new List<ReceiptImageUploadUrl>();

                foreach (var receiptImageFile in receiptImageFileRequestList)
                {
                    receiptImageFile.IsPublic = false;
                    var storageFilePath = _fileStorageService.GetStorageFilePath(receiptImageFile);
                    var uploadUrl = await _fileStorageService.GetUploadUrl(receiptImageFile, storageFilePath);
                    result.Add(new ReceiptImageUploadUrl 
                    { 
                        Name = receiptImageFile.Name, 
                        UploadUrl = uploadUrl.AbsoluteUri,
                        StorageFilePath = storageFilePath
                    });
                }
            }
            
            return result;
        }

        public async Task<IEnumerable<string>> GetReceiptImageDownloadUrls(IEnumerable<string> storageFilePaths)
        {
            List<string> result = null;

            if (storageFilePaths != null && storageFilePaths.Any())
            {
                result = new List<string>();

                foreach (var storageFilePath in storageFilePaths)
                {
                    var storageRoot = _fileStorageService.GetStorageRoot(new ReceiptImageFileRequest());
                    var downloadUrl = await _fileStorageService.GetDownloadUrl(storageRoot, storageFilePath);
                    result.Add(downloadUrl.AbsoluteUri);
                }
            }

            return result;
        }
    }
}
