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
    public class HospiceLocationRepository : RepositoryBase<HospiceLocations>, IHospiceLocationRepository
    {
        public HospiceLocationRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<HospiceLocations>> GetManyAsync(Expression<Func<HospiceLocations, bool>> where)
        {
            var entities = _dbContext.Set<HospiceLocations>()
                           .Include(h => h.Address)
                           .Include(h => h.Site)
                           .Where(where).AsQueryable();

            return await GetPaginatedSortedListAsync(entities);
        }
        public override async Task<HospiceLocations> GetByIdAsync(int locationId)
        {
            var entities = _dbContext.Set<HospiceLocations>()
               .Include(l => l.Site)
               .Include(l => l.Address)
               .Include(l => l.PhoneNumber)
               .Where(f => f.Id == locationId)
               .AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }

        public override async Task<HospiceLocations> GetAsync(Expression<Func<HospiceLocations, bool>> where)
        {
            var entities = _dbContext.Set<HospiceLocations>()
               .Include(l => l.Site)
               .Include(l => l.Address)
               .Include(l => l.PhoneNumber)
               .Where(where)
               .AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }
    }
}
