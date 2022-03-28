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
    public class FacilityPatientRepository : RepositoryBase<FacilityPatient>, IFacilityPatientRepository
    {
        public FacilityPatientRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

        public override async Task<IEnumerable<FacilityPatient>> GetManyAsync(Expression<Func<FacilityPatient, bool>> where)
        {
            var entities = _dbContext.Set<FacilityPatient>()
                                .Include(fp => fp.Facility)
                                    .ThenInclude(f => f.Address)
                                .Include(fp => fp.Facility)
                                    .ThenInclude(f => f.FacilityPhoneNumber)
                                        .ThenInclude(fp => fp.PhoneNumber)
                                .Where(where);

            return await GetPaginatedSortedListAsync(entities);
        }
    }
}
