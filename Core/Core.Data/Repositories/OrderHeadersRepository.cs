using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories
{
    public class OrderHeadersRepository : RepositoryBase<OrderHeaders>, IOrderHeadersRepository
    {
        public OrderHeadersRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<OrderHeaders>> GetManyAsync(Expression<Func<OrderHeaders, bool>> where)
        {
            var entities = _dbContext.Set<OrderHeaders>()
                                .Include(o => o.DeliveryAddress)
                                .Include(o => o.PickupAddress)
                                .Include(o => o.Hospice)
                                    .ThenInclude(h => h.PhoneNumber)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.Status)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.DispatchStatus)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.Action)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.Item)
                                .Include(o => o.OrderType)
                                .Include(o => o.Status)
                                .Include(o => o.DispatchStatus)
                                .Include(o => o.OrderNotes)
                                    .ThenInclude(l => l.CreatedByUser)
                                .Where(where)
                                .OrderByDescending(o => o.OrderDateTime);

            return await GetPaginatedSortedListAsync(entities);
        }

        public async Task<IEnumerable<OrderHeaders>> GetDispatchOrderAsync(Expression<Func<OrderHeaders, bool>> where)
        {
            var entities = _dbContext.Set<OrderHeaders>()
                                .Include(o => o.DeliveryAddress)
                                .Include(o => o.PickupAddress)
                                .Include(o => o.OrderNotes)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.Status)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.DispatchStatus)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.Action)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.Item)
                                .Include(o => o.OrderFulfillmentLineItems)
                                .Where(where);

            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<OrderHeaders> GetByIdAsync(int orderHeaderId)
        {
            var entities = _dbContext.Set<OrderHeaders>()
                                .Include(o => o.DeliveryAddress)
                                .Include(o => o.PickupAddress)
                                .Include(o => o.Hospice)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.Status)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.DispatchStatus)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.Action)
                                .Include(o => o.OrderLineItems)
                                    .ThenInclude(l => l.Item)
                                .Include(o => o.OrderType)
                                .Include(o => o.Status)
                                .Include(o => o.DispatchStatus)
                                .Include(o => o.Site)
                                .Include(o => o.OrderNotes)
                                    .ThenInclude(n => n.CreatedByUser)
                                .Where(o => o.Id == orderHeaderId).AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }
    }
}
