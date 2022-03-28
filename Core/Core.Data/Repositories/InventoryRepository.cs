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
    public class InventoryRepository : RepositoryBase<Inventory>, IInventoryRepository
    {
        public InventoryRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<Inventory>> GetManyAsync(Expression<Func<Inventory, bool>> where)
        {
            var entities = _dbContext.Set<Inventory>()
               .Include(i=> i.Status)
               .Include(i => i.Item)
                    .ThenInclude(t => t.ItemCategoryMapping)
                        .ThenInclude(cm => cm.ItemCategory)
                .Include(i => i.Item)
                    .ThenInclude(t => t.ItemSubCategoryMapping)
                        .ThenInclude(cm => cm.ItemSubCategory)
               .Where(where)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<Inventory> GetByIdAsync(int inventoryId)
        {
            var entities = _dbContext.Set<Inventory>()
               .Include(i => i.Status)
               .Include(i => i.Item)
               .Where(f => f.Id == inventoryId)
               .AsQueryable();
            var inventory = (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();

            return inventory;
        }
    }
}
