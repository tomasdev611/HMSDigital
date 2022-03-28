using System;
using System.Collections.Generic;
using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories
{
    public class OrderLineItemsRepository : RepositoryBase<OrderLineItems>, IOrderLineItemsRepository
    {
        public OrderLineItemsRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<OrderLineItems> GetByIdAsync(int lineItemId)
        {
            var entities = _dbContext.Set<OrderLineItems>()
                                .Include(o => o.Site)
                                .Include(o => o.DeliveryAddress)
                                .Include(o => o.PickupAddress)
                                .Include(l => l.Status)
                                .Include(l => l.DispatchStatus)
                                .Include(l => l.Action)
                                .Where(o => o.Id == lineItemId).AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }

        public override async Task<OrderLineItems> GetAsync(Expression<Func<OrderLineItems, bool>> where)
        {
            var entities = _dbContext.Set<OrderLineItems>()
                .Include(o => o.OrderHeader)
                .Include(o => o.Item)
                .ThenInclude(i => i.Inventory)
                .Where(where)
                .AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }
    }
}
