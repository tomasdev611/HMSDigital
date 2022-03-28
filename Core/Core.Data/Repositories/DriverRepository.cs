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
    public class DriverRepository : RepositoryBase<Drivers>, IDriverRepository
    {
        public DriverRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<Drivers>> GetAllAsync()
        {
            var entities = _dbContext.Set<Drivers>()
               .Include(d => d.User)
               .Include(d => d.CurrentSite)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<IEnumerable<Drivers>> GetManyAsync(Expression<Func<Drivers, bool>> where)
        {
            var entities = _dbContext.Set<Drivers>()
               .Include(d => d.User)
               .Include(d => d.CurrentVehicle)
               .Where(where)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<Drivers> GetByIdAsync(int driverId)
        {
            var entities = _dbContext.Set<Drivers>()
               .Include(d => d.User)
               .Include(d => d.CurrentSite)
               .Include(d => d.CurrentVehicle)
               .Where(d => d.Id == driverId)
               .AsQueryable();
            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }

        public override async Task<Drivers> GetAsync(Expression<Func<Drivers, bool>> where)
        {
            var entities = _dbContext.Set<Drivers>()
               .Include(d => d.User)
               .Include(d => d.CurrentVehicle)
               .Include(d => d.CurrentSite)
                    .ThenInclude(s => s.Address)
               .Where(where)
               .AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }
    }
}
