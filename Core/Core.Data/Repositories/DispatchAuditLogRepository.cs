using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories
{
    public class DispatchAuditLogRepository : RepositoryBase<DispatchAuditLog>, IDispatchAuditLogRepository
    {
        public DispatchAuditLogRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public async override Task<IEnumerable<DispatchAuditLog>> GetAllAsync()
        {
            var entities = _dbContext.Set<DispatchAuditLog>()
               .Include(d => d.User)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }
    }
}
