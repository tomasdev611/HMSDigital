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
    public class HospiceMemberRepository : RepositoryBase<HospiceMember>, IHospiceMemberRepository
    {
        public HospiceMemberRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }
        public override async Task<IEnumerable<HospiceMember>> GetManyAsync(Expression<Func<HospiceMember, bool>> where)
        {
            var entities = _dbContext.Set<HospiceMember>()
                .Include(m => m.User)
                .Include(m => m.HospiceLocationMembers)
                    .ThenInclude(l => l.HospiceLocation)
                .Where(where)
                .AsQueryable();

            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<HospiceMember> GetAsync(Expression<Func<HospiceMember, bool>> where)
        {
            var entities = _dbContext.Set<HospiceMember>()
                .Include(m => m.User)
                    .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(m => m.HospiceLocationMembers)
                    .ThenInclude(lm => lm.HospiceLocation)
                .Where(where)
                .AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }
    }
}
