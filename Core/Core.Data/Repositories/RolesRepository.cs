using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HMSDigital.Core.Data.Repositories
{
    public class RolesRepository : RepositoryBase<Roles>, IRolesRepository
    {
        private static readonly IDictionary<string, IEnumerable<Roles>> AllRolesCache = new Dictionary<string, IEnumerable<Roles>>();

        public RolesRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {
        }

        public async override Task<IEnumerable<Roles>> GetManyAsync(Expression<Func<Roles, bool>> where)
        {
            var cacheRoleKey = "all";
            if (SieveModel != null)
            {
                cacheRoleKey += $":{SieveModel.Filters}:{SieveModel.Sorts}:{SieveModel.Page}:{SieveModel.PageSize}";
            }
            if (AllRolesCache.TryGetValue(cacheRoleKey, out var cachedAllRoles))
            {
                return cachedAllRoles.Where(where.Compile());
            }

            var entities = _dbContext.Set<Roles>()
                            .Include(r => r.RolePermissions)
                                .ThenInclude(rp => rp.PermissionNoun)
                            .Include(r => r.RolePermissions)
                                .ThenInclude(rp => rp.PermissionVerb)
                            .Include(r => r.UserRoles)
                                .ThenInclude(rp => rp.User)
                            .AsQueryable();

            var allRoles = await GetPaginatedSortedListAsync(entities);
            AllRolesCache[cacheRoleKey] = allRoles;

            return allRoles.Where(where.Compile());
        }

        public async override Task<Roles> GetByIdAsync(int id)
        {
            return (await GetManyAsync(r => r.Id == id)).FirstOrDefault();
        }
    }
}
