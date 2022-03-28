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
    public class SiteMemberRepository : RepositoryBase<SiteMembers>, ISiteMemberRepository
    {
        public SiteMemberRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }
        public override async Task<IEnumerable<SiteMembers>> GetManyAsync(Expression<Func<SiteMembers, bool>> where)
        {
            var entities = _dbContext.Set<SiteMembers>()
                .Include(m => m.User)
                .Include(m => m.Site)
                .Where(where)
                .AsQueryable();

            return await GetPaginatedSortedListAsync(entities);
        }

        public override async Task<SiteMembers> GetAsync(Expression<Func<SiteMembers, bool>> where)
        {
            var entities = _dbContext.Set<SiteMembers>()
                .Include(m => m.User)
                .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(m => m.Site)
                    .ThenInclude(s => s.Address)
                .Where(where)
                .AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }
    }
}
