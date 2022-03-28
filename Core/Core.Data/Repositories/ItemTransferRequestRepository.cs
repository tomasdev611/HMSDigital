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
    public class ItemTransferRequestRepository : RepositoryBase<ItemTransferRequests>, IItemTransferRequestRepository
    {
        public ItemTransferRequestRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<ItemTransferRequests>> GetManyAsync(Expression<Func<ItemTransferRequests, bool>> where)
        {
            var entities = _dbContext.Set<ItemTransferRequests>()
               .Include(i => i.Status)
               .Include(i => i.Item)
               .Include(i => i.DestinationSiteMember)
                    .ThenInclude(sm => sm.User)
               .Where(where)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<ItemTransferRequests> GetByIdAsync(int transferRequestId)
        {
            var entities = _dbContext.Set<ItemTransferRequests>()
               .Include(i => i.Status)
               .Include(i => i.Item)
               .Include(i=> i.DestinationSiteMember)
                    .ThenInclude(sm=> sm.User)
               .Where(f => f.Id == transferRequestId)
               .AsQueryable();
            var inventory = (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();

            return inventory;
        }
    }
}
