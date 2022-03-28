using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using HMSDigital.Core.ViewModels.NetSuite;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface IOrderHeadersService
    {
        Task<PaginatedList<OrderHeader>> GetAllOrderHeaders(SieveModel sieveModel, bool includeFulfillmentDetails, int? locationId = null);

        Task<OrderHeader> GetOrderHeaderById(int orderHeaderId, bool includeFulfillmentDetails);

        Task<OrderHeader> UpsertOrderFromWebPortal(OrderHeaderRequest orderHeaderRequest);

        Task<PaginatedList<OrderFulfillmentLineItem>> GetOrderFulfillments(int orderHeaderId, SieveModel sieveModel);

        Task<bool> AssignSiteToOrder(Data.Models.OrderHeaders orderModel);

        Task<OrderHeader> UpsertOrderNotes(int orderHeaderId, IEnumerable<UpdateOrderNotesRequest> orderNotes);
    }
}
