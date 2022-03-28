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
    public class UserSitesRepository : RepositoryBase<UserSites>, IUserSitesRepository
    {
        public UserSitesRepository(HMSOnlineContext dbContext) : base(dbContext)
        {
        }


    }
}
