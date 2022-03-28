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
    public class UserProfilePictureRepository : RepositoryBase<UserProfilePicture>, IUserProfilePictureRepository
    {
        public UserProfilePictureRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public async override Task<IEnumerable<UserProfilePicture>> GetManyAsync(Expression<Func<UserProfilePicture, bool>> where)
        {
           var entities = _dbContext.Set<UserProfilePicture>()
                .Include(u => u.FileMetadata)
                .Where(where)
                .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }
    }
}
