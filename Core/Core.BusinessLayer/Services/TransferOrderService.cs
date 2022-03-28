using AutoMapper;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using HospiceSource.Digital.NetSuite.SDK.Interfaces;
using HospiceSource.Digital.NetSuite.SDK.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class TransferOrderService : ITransferOrderService
    {
        private readonly INetSuiteService _netSuiteService;
        private readonly ISitesService _sitesService;
        private readonly IMapper _mapper;
        private readonly IItemsService _itemsService;

        public TransferOrderService(
            INetSuiteService netSuiteService, 
            ISitesService sitesService,
            IItemsService itemsService,
            IMapper mapper) 
        {
            _netSuiteService = netSuiteService;
            _sitesService = sitesService;
            _itemsService = itemsService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TransferOrder>> GetPendingTransferOrders(int siteId, bool truckTransferOrders, SieveModel sieveModel)
        {
            var transferOrderRequest = new GetTransferOrdersRequest
            {
                NetSuiteLocationId = await _sitesService.GetNetSuiteLocationId(siteId),
                TruckTransferOrders = truckTransferOrders
            };

            var netsuiteTransferOrders = await _netSuiteService.GetTransferOrders(transferOrderRequest);

            if (netsuiteTransferOrders == null || !netsuiteTransferOrders.Any()) 
            {
                return null;
            }

            return await MapTransferOrdersToHms(netsuiteTransferOrders);
        }

        public async Task<TransferOrder> CreateTransferOrder(TransferOrderCreateRequest transferOrderCreateRequest)
        {
            transferOrderCreateRequest.OrderLineItems = GroupDuplicateItems(transferOrderCreateRequest.OrderLineItems);
            var itemIds = transferOrderCreateRequest.OrderLineItems.Select(x => x.ItemId);
            var items = await _itemsService.GetItemsByIds(itemIds);

            foreach (var orderLineItem in transferOrderCreateRequest.OrderLineItems)
            {
                var item = items.FirstOrDefault(x => x.Id == orderLineItem.ItemId);

                if (item == null) 
                {
                    throw new ValidationException($"Invalid site id ({orderLineItem.ItemId}).");
                }

                orderLineItem.NetSuiteItemId = item.NetSuiteItemId;
            }

            var netSuiteTransferOrder = _mapper.Map<NetSuiteTransferOrder>(transferOrderCreateRequest);

            var sourceSite = await _sitesService.GetSiteById(transferOrderCreateRequest.SourceLocationId);

            if (sourceSite == null) 
            {
                throw new ValidationException($"Invalid site Source Site Id ({transferOrderCreateRequest.SourceLocationId}).");
            }

            netSuiteTransferOrder.NetSuiteSourceLocationId = sourceSite.NetSuiteLocationId.Value;

            var destinationSite = await _sitesService.GetSiteById(transferOrderCreateRequest.DestinationLocationId);

            if (destinationSite == null)
            {
                throw new ValidationException($"Invalid site Destination Site Id ({transferOrderCreateRequest.DestinationLocationId}).");
            }

            netSuiteTransferOrder.NetSuiteDestinationLocationId = destinationSite.NetSuiteLocationId.Value;

            var netSuiteTransferOrderResponse = await _netSuiteService.CreateTransferOrder(netSuiteTransferOrder);

            var transferOder = _mapper.Map<TransferOrder>(netSuiteTransferOrderResponse);
            transferOder.SourceLocation = sourceSite;
            transferOder.DestinationLocation = destinationSite;
            MapTransferOrderItems(transferOder, items);

            return transferOder;
        }

        public async Task<TransferOrder> FulfillReceiveTransferOrder(int netSuiteTransferOrderId, TOrderFulfillReceiveRequest fulfillReceiveRequest)
        {
            if (netSuiteTransferOrderId != fulfillReceiveRequest.NetSuiteTransferOrderId)
            {
                throw new ValidationException($"NetSuite Transfer Order Id ({fulfillReceiveRequest.NetSuiteTransferOrderId}) is not valid.");
            }

            fulfillReceiveRequest.OrderLineItems = GroupDuplicateItems(fulfillReceiveRequest.OrderLineItems);
            var itemIds = fulfillReceiveRequest.OrderLineItems.Select(x => x.ItemId);
            var fulfilledReceiveditems = await _itemsService.GetItemsByIds(itemIds);

            foreach (var orderLineItem in fulfillReceiveRequest.OrderLineItems)
            {
                var item = fulfilledReceiveditems.FirstOrDefault(x => x.Id == orderLineItem.ItemId);

                if (item == null)
                {
                    throw new ValidationException($"Invalid site id ({orderLineItem.ItemId}).");
                }

                orderLineItem.NetSuiteItemId = item.NetSuiteItemId;
            }

            var netSuiteRequest = _mapper.Map<NetSuiteTOFulfillReceiveRequest>(fulfillReceiveRequest);
            var transferOrderResponse = await _netSuiteService.FulfillReceiveTransferOrder(netSuiteRequest);

            return _mapper.Map<TransferOrder>(transferOrderResponse);
        }

        private async Task<IEnumerable<TransferOrder>> MapTransferOrdersToHms(IEnumerable<NetSuiteTransferOrder> netsuiteTransferOrders)
        {
            var netsuiteLocationIds = netsuiteTransferOrders.Select(x => x.NetSuiteSourceLocationId).ToList();
            netsuiteLocationIds.AddRange(netsuiteTransferOrders.Select(x => x.NetSuiteDestinationLocationId));
            var sites = await _sitesService.GetSitesByNetsuiteLocationIds(netsuiteLocationIds);

            var netSuiteItemIds = netsuiteTransferOrders.SelectMany(x => x.OrderLineItems.Select(x => x.NetSuiteItemId));
            var items = await _itemsService.GetItemsByNetSuiteItemIds(netSuiteItemIds);

            var results = new List<TransferOrder>();
            foreach (var netsuiteTransferOrder in netsuiteTransferOrders)
            {
                var transferOrder = _mapper.Map<TransferOrder>(netsuiteTransferOrder);
                transferOrder.SourceLocation = sites.FirstOrDefault(x => x.NetSuiteLocationId == netsuiteTransferOrder.NetSuiteSourceLocationId);
                transferOrder.DestinationLocation = sites.FirstOrDefault(x => x.NetSuiteLocationId == netsuiteTransferOrder.NetSuiteDestinationLocationId);

                MapTransferOrderItems(transferOrder, items);

                results.Add(transferOrder);
            }

            return results;
        }

        private void MapTransferOrderItems(TransferOrder transferOrder, IEnumerable<Item> items)
        {
            if (transferOrder.OrderLineItems != null && transferOrder.OrderLineItems.Any()
                && items != null && items.Any()
            ) 
            {
                foreach (var lineItem in transferOrder.OrderLineItems)
                {
                    var item = items.FirstOrDefault(x => x.NetSuiteItemId == lineItem.NetSuiteItemId);
                    if (item != null) 
                    {
                        lineItem.ItemId = item.Id;
                    }
                }
            }
        }

        private List<InventoryLineItem> GroupDuplicateItems(IEnumerable<InventoryLineItem> orderLineItems)
        {
            return orderLineItems
                    .GroupBy(x => x.ItemId)
                    .Select(x => new InventoryLineItem
                    {
                        ItemId = x.First().ItemId,
                        IsSerial = x.First().IsSerial,
                        NetSuiteItemId = x.First().NetSuiteItemId,
                        Quantity = x.Sum(x => x.Quantity),
                        QuantityReceived = x.Sum(x => x.QuantityReceived),
                        QuantityShipped = x.Sum(x => x.QuantityShipped),
                        Inventory = GetGroupedInventory(x)
                    }).ToList();
        }

        private List<InventoryRequest> GetGroupedInventory(IGrouping<int, InventoryLineItem> groupedItems) 
        {
            if(!groupedItems.Any(x => x.Inventory != null))
            {
                return null;
            }

            List<InventoryRequest> result = new List<InventoryRequest>();

            foreach (var groupedItem in groupedItems)
            {
                if (groupedItem.Inventory != null)
                {
                    result.AddRange(groupedItem.Inventory);
                }
            }

            return result;
        }
    }
}
