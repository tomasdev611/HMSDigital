using HMSOnlineSDK.Data.Models;
using HMSOnlineSDK.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HMSOnlineSDK.Data.Repositories
{
    public class UserInformationRepository : RepositoryBase<UserInformations>, IUserInformationRepository
    {
        public UserInformationRepository(HMSOnlineContext dbContext) : base(dbContext)
        {
        }

        public async override Task<IEnumerable<UserInformations>> GetManyAsync(Expression<Func<UserInformations, bool>> where)
        {
            var entities = _dbContext.Set<UserInformations>()
                            .Include(u => u.UserSites)
                                .ThenInclude(us => us.Site)
                            .Where(where).AsQueryable();

            return await GetPaginatedSortedListAsync(entities);
        }

    }
}
