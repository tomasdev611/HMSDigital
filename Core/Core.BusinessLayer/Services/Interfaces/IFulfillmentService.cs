using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IFulfillmentService
    {
        Task FulfillOrder(OrderFulfillmentRequest fulfillmentRequest);

        Task UpdateOrderStatus(OrderStatusUpdateRequest statusUpdateRequest);

        Task<bool> ConfirmOrderFulfillment(int orderHeaderId, IEnumerable<Data.Models.OrderFulfillmentLineItems> lineItemFullfilments, bool dispatchOnly = false);
    }
}
