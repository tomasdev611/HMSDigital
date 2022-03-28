using HMSDigital.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IOrderLineItemsService
    {
        Task<IEnumerable<OrderLineItem>> GetAllOrderLineItems();

        Task<OrderLineItem> GetOrderLineItemById(int orderLineItemId);
    }
}
