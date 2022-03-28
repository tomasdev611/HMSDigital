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
    public class DispatchInstructionsRepository : RepositoryBase<DispatchInstructions>, IDispatchInstructionsRepository
    {
        public DispatchInstructionsRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<DispatchInstructions>> GetManyAsync(Expression<Func<DispatchInstructions, bool>> where)
        {
            var entities = _dbContext.Set<DispatchInstructions>()
               .Include(d => d.OrderHeader)
                   .ThenInclude(o => o.OrderLineItems)
                        .ThenInclude(l => l.Item)
               .Include(d => d.OrderHeader)
                    .ThenInclude(o => o.OrderType)
               .Include(d => d.OrderHeader)
                    .ThenInclude(o => o.Status)
                .Include(d => d.OrderHeader)
                    .ThenInclude(o => o.DispatchStatus)
               .Include(d => d.OrderHeader)
                    .ThenInclude(o => o.Hospice)
               .Include(i => i.Vehicle)
                    .ThenInclude(v => v.DriversCurrentVehicle)
                         .ThenInclude(d => d.User)
               .Where(where)
               .OrderBy(i => i.SequenceNumber)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<DispatchInstructions> GetByIdAsync(int dispatchInstructionId)
        {
            var entities = _dbContext.Set<DispatchInstructions>()
               .Include(i => i.Vehicle)
               .Where(f => f.Id == dispatchInstructionId)
               .OrderBy(i => i.SequenceNumber)
               .AsQueryable();
            var inventory = (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();

            return inventory;
        }
    }
}
