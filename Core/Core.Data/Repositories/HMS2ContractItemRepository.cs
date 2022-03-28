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
    public class Hms2ContractItemRepository : RepositoryBase<Hms2ContractItems>, IHMS2ContractItemRepository
    {
        public Hms2ContractItemRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<Hms2ContractItems>> GetAllAsync()
        {
            var entities = _dbContext.Set<Hms2ContractItems>()
                 .Include(i => i.Item)
                .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<IEnumerable<Hms2ContractItems>> GetManyAsync(Expression<Func<Hms2ContractItems, bool>> where)
        {
            var entities = _dbContext.Set<Hms2ContractItems>()
                           .Include(c => c.Item)
                                .ThenInclude(i => i.ItemImages)
                           .Include(c => c.Item)
                                .ThenInclude(i => i.EquipmentSettingsConfig)
                                    .ThenInclude(e => e.EquipmentSettingType)
                            .Include(c => c.Item)
                                .ThenInclude(i => i.AddOnGroups)
                                    .ThenInclude(g => g.AddOnGroupProducts)
                                        .ThenInclude(gp => gp.Item)
                                            .ThenInclude(i => i.ItemImages)
                           .Where(where).AsQueryable();

            return await GetPaginatedSortedListAsync(entities);
        }

        public async Task<IEnumerable<Hms2ContractItems>> GetContractItemsAsync(Expression<Func<Hms2ContractItems, bool>> where)
        {
            var entities = _dbContext.Set<Hms2ContractItems>()
                                .Include(c=> c.Contract)
                                .Where(where);

            return await GetPaginatedSortedListAsync(entities);
        }
    }
}
