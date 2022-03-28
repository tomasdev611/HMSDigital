using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileApp.Interface;
using MobileApp.Interface.Services;
using MobileApp.Models;
using Refit;

namespace MobileApp.Service
{
    public class InventoryManagementService : IPurchaseOrderService, IInventoryService
    {
        #region Private Properties

        private readonly IPurchaseOrderApi _purchaseOrderApi;
        private readonly IInventoryServiceApi _inventoryServiceApi;

        #endregion

        #region Public Methods

        public InventoryManagementService()
        {
            _purchaseOrderApi = RestService.For<IPurchaseOrderApi>(HMSHttpClientFactory.GetCoreHttpClient());
            _inventoryServiceApi = RestService.For<IInventoryServiceApi>(HMSHttpClientFactory.GetCoreHttpClient());
        }

        /// <summary>
        /// Get the Purchase Orders from the service
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PurchaseOrderModel>> GetPurchaseOrders(int currentSiteId)
        {
            try
            {
                var results = await _purchaseOrderApi.GetOpenPurchaseOrders("", currentSiteId);
                if (!results.IsSuccessStatusCode)
                {
                    return null;
                }
                return results.Content.Records;
            }
            catch
            {
                return GetPurchaseOrdersMock();
                //throw;
            }
        }

        /// <summary>
        /// Receive the Purchase Order
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <param name="lineItemModels"></param>
        /// <param name="imageUrls"></param>
        /// <returns></returns>
        public async Task<ReceivePurchaseOrderResponse> ReceivePurchaseOrder(int purchaseOrderId, ReceivePurchaseOrderRequest request)
        {
            try
            {
                var results = await _inventoryServiceApi.ReceivePurchaseOrder(purchaseOrderId, request);
                if (!results.IsSuccessStatusCode)
                {
                    return null;
                }
                return results.Content;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Receive the Purchase Order
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <param name="lineItemModels"></param>
        /// <param name="imageUrls"></param>
        /// <returns></returns>
        public async Task<FulfillReceiveOrderResponse> FulfillReceiveOrder(int purchaseOrderId, FulfillReceiveOrderRequest request)
        {
            try
            {
                var results = await _inventoryServiceApi.FulfillReceiveTransferOrder(purchaseOrderId, request);
                if (!results.IsSuccessStatusCode)
                {
                    return null;
                }
                return results.Content;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get the Purchase Orders from the service
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PurchaseOrderReceiptModel>> GetPurchaseOrderReceipts(int purchaseOrderId, int currentSiteId)
        {
            try
            {
                await Task.Delay(100);

                return GetReceiptsMock();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get the Purchase Order by Id
        /// </summary>
        /// <param name="purchaseOrderId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PurchaseOrderModel>> GetPurchaseOrdersById(int purchaseOrderId, int currentSiteId)
        {
            await Task.Delay(100);
            return GetPurchaseOrdersMock().Where(x => x.PurchaseOrderNumber == purchaseOrderId);
        }

        public async Task<IEnumerable<TransferOrder>> GetPendingTransferOrders(int siteId, bool truckTransferOrders)
        {
            try
            {
                var results = await _inventoryServiceApi.GetPendingTransferOrders(siteId, truckTransferOrders);
                if (!results.IsSuccessStatusCode)
                {
                    return null;
                }
                return results.Content;
            }
            catch
            {
                return GetTransferOrdersMock();
                //throw;
            }
        }

        #endregion

        /// <summary>
        /// Get mock data for Purchase Orders
        /// </summary>
        /// <returns></returns>
        private List<PurchaseOrderModel> GetPurchaseOrdersMock()
        {
            var orders = new List<PurchaseOrderModel>();
            orders.Add(new PurchaseOrderModel()
            {
                DateCreated = DateTime.Now.AddDays(-4),
                Status = "open",
                PurchaseOrderNumber = 3220,
                VendorName = "Vendor 01",
                VendorNumber = 109,
                TotalQuantity = 12
            });
            orders.Add(new PurchaseOrderModel()
            {
                DateCreated = DateTime.Now.AddDays(-4).AddHours(16),
                Status = "partial",
                PurchaseOrderNumber = 3221,
                VendorName = "Vendor 02",
                VendorNumber = 208,
                TotalQuantity = 1,
                PurchaseOrderLineItems = new List<PurchaseOrderLineItemModel>
                {
                    new PurchaseOrderLineItemModel()
                    {
                        ItemName = "Item Name",
                        ItemDescription = "DESCRIPTION",
                        AssetTags = new List<string>()
                        {
                            "872343242",
                            "872343243",
                            "872343244",
                            "872343245"
                        },
                        ItemId = 23,
                        Quantity = 2,
                        QuantityRecieved = 1
                    },
                    new PurchaseOrderLineItemModel()
                    {
                        ItemName = "Item Name 2",
                        ItemDescription = "DESCRIPTION FOR ITEM",
                        AssetTags = new List<string>()
                        {
                            "872343242",
                            "872343243",
                            "872343244",
                            "872343245"
                        },
                        ItemId = 235,
                        Quantity = 5,
                        QuantityRecieved = 5
                    }
                }
            });
            orders.Add(new PurchaseOrderModel()
            {
                DateCreated = DateTime.Now.AddDays(-2).AddHours(6),
                Status = "competed",
                PurchaseOrderNumber = 3222,
                VendorName = "Vendor 03",
                VendorNumber = 487,
                TotalQuantity = 7
            });

            return orders;
        }

        private List<TransferOrder> GetTransferOrdersMock()
        {
            var transferOrders = new List<TransferOrder>();
            transferOrders.Add(new TransferOrder()
            {
                DateCreated = DateTime.Now.AddDays(-2),
                TransferOrderId = 23423,
                TransferOrderStatus = "stastus",
                OrderLineItems = new List<PurchaseOrderLineItemModel>
                {
                    new PurchaseOrderLineItemModel()
                    {
                        ItemName = "Item Name",
                        ItemDescription = "DESCRIPTION",
                        AssetTags = new List<string>()
                        {
                            "872343242",
                            "872343243",
                            "872343244",
                            "872343245"
                        },
                        ItemId = 23,
                        Quantity = 2,
                        QuantityRecieved = 1
                    },
                    new PurchaseOrderLineItemModel()
                    {
                        ItemName = "Item Name 2",
                        ItemDescription = "DESCRIPTION FOR ITEM",
                        AssetTags = new List<string>()
                        {
                            "872343242",
                            "872343243",
                            "872343244",
                            "872343245"
                        },
                        ItemId = 235,
                        Quantity = 5,
                        QuantityRecieved = 5
                    }
                }
            });

            transferOrders.Add(new TransferOrder()
            {
                DateCreated = DateTime.Now.AddDays(-2),
                TransferOrderId = 23423,
                TransferOrderStatus = "stastus",
                OrderLineItems = new List<PurchaseOrderLineItemModel>
                {
                    new PurchaseOrderLineItemModel()
                    {
                        ItemName = "Item Name",
                        ItemDescription = "DESCRIPTION",
                        AssetTags = new List<string>()
                        {
                            "872343242",
                            "872343243",
                            "872343244",
                            "872343245"
                        },
                        ItemId = 23,
                        Quantity = 2,
                        QuantityRecieved = 1
                    },
                    new PurchaseOrderLineItemModel()
                    {
                        ItemName = "Item Name 2",
                        ItemDescription = "DESCRIPTION FOR ITEM",
                        AssetTags = new List<string>()
                        {
                            "872343242",
                            "872343243",
                            "872343244",
                            "872343245"
                        },
                        ItemId = 235,
                        Quantity = 5,
                        QuantityRecieved = 5
                    }
                }
            });

            transferOrders.Add(new TransferOrder()
            {
                DateCreated = DateTime.Now.AddDays(-2),
                TransferOrderId = 23423,
                TransferOrderStatus = "stastus",
                OrderLineItems = new List<PurchaseOrderLineItemModel>
                {
                    new PurchaseOrderLineItemModel()
                    {
                        ItemName = "Item Name",
                        ItemDescription = "DESCRIPTION",
                        AssetTags = new List<string>()
                        {
                            "872343242",
                            "872343243",
                            "872343244",
                            "872343245"
                        },
                        ItemId = 23,
                        Quantity = 2,
                        QuantityRecieved = 1
                    },
                    new PurchaseOrderLineItemModel()
                    {
                        ItemName = "Item Name 2",
                        ItemDescription = "DESCRIPTION FOR ITEM",
                        AssetTags = new List<string>()
                        {
                            "872343242",
                            "872343243",
                            "872343244",
                            "872343245"
                        },
                        ItemId = 235,
                        Quantity = 5,
                        QuantityRecieved = 5
                    }
                }
            });

            return transferOrders;
        }

        private List<PurchaseOrderReceiptModel> GetReceiptsMock()
        {
            var receipts = new List<PurchaseOrderReceiptModel>();

            receipts.Add(new PurchaseOrderReceiptModel()
            {
                MaterialName = "FJ1001 Tubing",
                ModelNumber = "23",
                Quantity = "12",
                Status = "Receive"
            });
            receipts.Add(new PurchaseOrderReceiptModel()
            {
                MaterialName = "DC1001 Bed Full Elec HI/LOW",
                ModelNumber = "103",
                Quantity = "3",
                Status = "Receive"
            });
            receipts.Add(new PurchaseOrderReceiptModel()
            {
                MaterialName = "FJ1001a Mask",
                ModelNumber = "12",
                Quantity = "1",
                Status = "Receive"
            });

            return receipts;
        }
    }
}