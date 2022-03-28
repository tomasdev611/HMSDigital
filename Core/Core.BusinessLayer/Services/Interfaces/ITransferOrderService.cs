using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface ITransferOrderService
    {
        Task<IEnumerable<TransferOrder>> GetPendingTransferOrders(int siteId, bool truckTransferOrders, SieveModel sieveModel);
        Task<TransferOrder> CreateTransferOrder(TransferOrderCreateRequest transferOrderCreateRequest);
        Task<TransferOrder> FulfillReceiveTransferOrder(int netSuiteTransferOrderId, TOrderFulfillReceiveRequest fulfillReceiveRequest);
    }
}
