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
    public class HospiceRepository : RepositoryBase<Hospices>, IHospiceRepository
    {
        public HospiceRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public async override Task<IEnumerable<Hospices>> GetManyAsync(Expression<Func<Hospices, bool>> where)
        {
            var entities = _dbContext.Set<Hospices>()
                            .Include(h => h.HospiceLocations)
                            .Include(h => h.Address)
                            .Include(h => h.PhoneNumber)
                            .Where(where).AsQueryable();

            return await GetPaginatedSortedListAsync(entities);
        }

        public async override Task<Hospices> GetAsync(Expression<Func<Hospices, bool>> where)
        {
            var entities = _dbContext.Set<Hospices>()
                            .Include(h => h.HospiceLocations)
                            .Include(h => h.Address)
                            .Include(h => h.PhoneNumber)
                            .Where(where).AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }

        public async override Task<Hospices> GetByIdAsync(int hospiceId)
        {
            var entities = _dbContext.Set<Hospices>()
                            .Include(h => h.HospiceLocations)
                            .Include(h => h.Address)
                            .Include(h => h.PhoneNumber)
                            .Include(h => h.CreditHoldByUser)
                            .Include(h => h.Hms2HmsDigitalHospiceMappings)
                            .Where(h => h.Id == hospiceId).AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }

    }
}
