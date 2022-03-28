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
    public class ItemImageFilesRepository : RepositoryBase<ItemImageFiles>, IItemImageFilesRepository
    {
        public ItemImageFilesRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<ItemImageFiles>> GetManyAsync(Expression<Func<ItemImageFiles, bool>> where)
        {
            var entities = _dbContext.Set<ItemImageFiles>()
               .Include(i => i.FileMetadata)
               .Where(where)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<ItemImageFiles> GetByIdAsync(int itemImageFileId)
        {
            var entities = _dbContext.Set<ItemImageFiles>()
               .Include(i => i.FileMetadata)
               .Where(f => f.Id == itemImageFileId)
               .AsQueryable();
            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }

    }
}
