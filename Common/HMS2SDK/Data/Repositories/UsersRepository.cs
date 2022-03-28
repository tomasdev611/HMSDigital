using HMS2SDK.Data.Models;
using HMS2SDK.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMS2SDK.Data.Repositories
{
    public class UsersRepository : RepositoryBase<TblUsers>, IUsersRepository
    {
        public UsersRepository(HMSContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {
            
        }

    }
}
