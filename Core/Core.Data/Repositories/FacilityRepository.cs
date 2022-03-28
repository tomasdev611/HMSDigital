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
    public class FacilityRepository : RepositoryBase<Facilities>, IFacilityRepository
    {
        public FacilityRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<Facilities>> GetManyAsync(Expression<Func<Facilities, bool>> where)
        {
            var entities = _dbContext.Set<Facilities>()
               .Include(f => f.Address)
               .Include(f => f.HospiceLocation)
               .Include(f => f.FacilityPhoneNumber)
                .ThenInclude(fa => fa.PhoneNumber)
               .Where(where)
               .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<Facilities> GetByIdAsync(int facilityId)
        {
            var entities = _dbContext.Set<Facilities>()
               .Include(f => f.Address)
               .Include(f => f.HospiceLocation)
               .Include(f => f.Site)
               .Include(f => f.FacilityPhoneNumber)
                    .ThenInclude(fa => fa.PhoneNumber)
               .Where(f => f.Id == facilityId)
               .AsQueryable();
            var facility = (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
            if(facility != null)
            {
                facility.FacilityPhoneNumber = facility.FacilityPhoneNumber.OrderBy(p => p.PhoneNumberId).ToList();
            }
            return facility;
        }
    }
}
