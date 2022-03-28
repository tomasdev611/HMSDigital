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
    public class ItemRepository : RepositoryBase<Items>, IItemRepository
    {
        public ItemRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<Items>> GetManyAsync(Expression<Func<Items, bool>> where)
        {
            var entities = _dbContext.Set<Items>()
               .Include(i => i.ItemCategoryMapping)
                    .ThenInclude(c => c.ItemCategory)
                .Include(i => i.ItemSubCategoryMapping)
                    .ThenInclude(c => c.ItemSubCategory)
                .Include(i => i.ItemImages)
               .Where(where)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<Items> GetByIdAsync(int itemId)
        {
            var entities = _dbContext.Set<Items>()
               .Include(i => i.ItemCategoryMapping)
                    .ThenInclude(c => c.ItemCategory)
               .Include(i => i.ItemSubCategoryMapping)
                    .ThenInclude(c => c.ItemSubCategory)
                .Include(i => i.EquipmentSettingsConfig)
                    .ThenInclude(e => e.EquipmentSettingType)
                .Include(i => i.AddOnGroups)
                    .ThenInclude(a => a.AddOnGroupProducts)
                        .ThenInclude(ap => ap.Item)
               .Where(f => f.Id == itemId)
               .AsQueryable();
            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }

        public override async Task<Items> GetAsync(Expression<Func<Items, bool>> where)
        {
            var entities = _dbContext.Set<Items>()
               .Include(i => i.ItemCategoryMapping)
                    .ThenInclude(c => c.ItemCategory)
                .Include(i => i.ItemSubCategoryMapping)
                    .ThenInclude(c => c.ItemSubCategory)
                .Include(i => i.ItemImages)
                .Include(i => i.Inventory)
               .Where(where)
               .AsQueryable();
            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }

    }
}
