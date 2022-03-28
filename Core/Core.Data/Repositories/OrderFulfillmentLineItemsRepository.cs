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
    public class OrderFulfillmentLineItemsRepository : RepositoryBase<OrderFulfillmentLineItems>, IOrderFulfillmentLineItemsRepository
    {
        public OrderFulfillmentLineItemsRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<OrderFulfillmentLineItems>> GetManyAsync(Expression<Func<OrderFulfillmentLineItems, bool>> where)
        {
            var entities = _dbContext.Set<OrderFulfillmentLineItems>()
                                .Include(o => o.FulfilledByDriver)
                                    .ThenInclude(d => d.User)
                                .Include(o => o.FulfilledByVehicle)
                                .Include(o => o.OrderLineItem)
                                    .ThenInclude(ol => ol.Item)
                                .Where(where);

            return await GetPaginatedSortedListAsync(entities);
        }
    }
}
