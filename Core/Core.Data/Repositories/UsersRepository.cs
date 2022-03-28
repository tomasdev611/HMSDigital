using HMSDigital.Core.Data.Enums;
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
    public class UsersRepository : RepositoryBase<Users>, IUsersRepository
    {
        public UsersRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public virtual async Task<Users> GetByCognitoUserId(string cognitoUserId)
        {
            return (await GetManyAsync(u => u.CognitoUserId == cognitoUserId)).FirstOrDefault();
        }

        public override async Task<Users> GetByIdAsync(int userId)
        {
            var entities = _dbContext.Set<Users>()
                                .Include(u => u.UserRoles)
                                    .ThenInclude(ur => ur.Role)
                                .Include(u => u.SiteMembersUser)
                                    .ThenInclude(us => us.Site)
                                .Where(u => u.Id == userId).AsQueryable();

            return (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
        }

        public override async Task<IEnumerable<Users>> GetManyAsync(Expression<Func<Users, bool>> where)
        {
            var entities = _dbContext.Set<Users>()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Where(where)
                .AsQueryable();

            return await GetPaginatedSortedListAsync(entities);
        }

        public virtual async Task<IEnumerable<string>> GetSiteAccessByUserId(int userId)
        {
            return await GetResourceAccessByUserId(userId, ResourceTypes.Site);
        }

        public virtual async Task<IEnumerable<string>> GetHospiceAccessByUserId(int userId)
        {
            return await GetResourceAccessByUserId(userId, ResourceTypes.Hospice);
        }

        public virtual async Task<IEnumerable<string>> GetHospiceLocationAccessByUserId(int userId)
        {
            return await GetResourceAccessByUserId(userId, ResourceTypes.HospiceLocation);
        }

        private async Task<IEnumerable<string>> GetResourceAccessByUserId(int userId, ResourceTypes resourceType)
        {
            var entities = _dbContext.Set<Users>()
                               .Include(u => u.UserRoles)
                               .Where(u => u.Id == userId).AsQueryable();

            var user = (await GetPaginatedSortedListAsync(entities)).FirstOrDefault();
            if(user == null)
            {
                return new List<string>();
            }
            return user.UserRoles.Where(ur => ur.ResourceType == resourceType.ToString())
                                 .Select(ur => ur.ResourceId);
        }
    }
}
