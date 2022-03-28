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
    public class PatientInventoryRepository : RepositoryBase<PatientInventory>, IPatientInventoryRepository
    {
        public PatientInventoryRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<PatientInventory>> GetManyAsync(Expression<Func<PatientInventory, bool>> where)
        {
            var entities = _dbContext.Set<PatientInventory>()
               .Include(i=> i.Status)
               .Include(i => i.Item)
               .Include(i => i.Inventory)
               .Include(i => i.OrderHeader)
               .Include(i => i.OrderLineItem)
               .Where(where)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<PatientInventory> GetByIdAsync(int patientInventoryId)
        {
            var entities = _dbContext.Set<PatientInventory>()
               .Include(i => i.Status)
               .Include(i => i.Item)
               .Include(i => i.Inventory)
               .Include(i => i.OrderHeader)
               .Include(i => i.OrderLineItem)
               .Where(f => f.Id == patientInventoryId)
               .AsQueryable();
            var inventory = (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();

            return inventory;
        }
    }
}
