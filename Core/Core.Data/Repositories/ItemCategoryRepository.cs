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
    public class ItemCategoryRepository : RepositoryBase<ItemCategories>, IItemCategoryRepository
    {
        public ItemCategoryRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }
        public override async Task<IEnumerable<ItemCategories>> GetAllAsync()
        {
            var entities = _dbContext.Set<ItemCategories>()
               .Include(i => i.ItemSubCategories)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<IEnumerable<ItemCategories>> GetManyAsync(Expression<Func<ItemCategories, bool>> where)
        {
            var entities = _dbContext.Set<ItemCategories>()
               .Include(i => i.ItemSubCategories)
               .Where(where)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<ItemCategories> GetByIdAsync(int categoryId)
        {
            var entities = _dbContext.Set<ItemCategories>()
               .Include(i => i.ItemSubCategories)
               .Where(i => i.Id == categoryId)
               .AsQueryable();
            var inventory = (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();

            return inventory;
        }
    }
}
