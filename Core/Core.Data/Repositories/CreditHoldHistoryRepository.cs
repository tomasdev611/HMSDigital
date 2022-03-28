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
    public class CreditHoldHistoryRepository : RepositoryBase<CreditHoldHistory>, ICreditHoldHistoryRepository
    {
        public CreditHoldHistoryRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public async override Task<IEnumerable<CreditHoldHistory>> GetManyAsync(Expression<Func<CreditHoldHistory, bool>> where)
        {
            var entities = _dbContext.Set<CreditHoldHistory>()
                            .Include(h => h.CreditHoldByUser)
                            .Where(where).AsQueryable();

            return await GetPaginatedSortedListAsync(entities);
        }
    }
}
