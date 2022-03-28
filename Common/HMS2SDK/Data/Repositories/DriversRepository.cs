using HMS2SDK.Data.Models;
using HMS2SDK.Data.Repositories.Interfaces;
using LinqKit;
using Sieve.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS2SDK.Data.Repositories
{
    public class DriversRepository : RepositoryBase<TblDrivers>, IDriversRepository
    {
        public DriversRepository(HMSContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {
            
        }

        public override async Task<IEnumerable<TblDrivers>> GetAllAsync()
        {
            var entities = _dbContext.Set<TblDrivers>()
                .AsQueryable();
            return await GetPaginatedSortedListAsync(entities);
        }

        public async Task<IEnumerable<DriverResponse>> SearchDriver(DriverRequest driverRequest)
        {
            var predicate = PredicateBuilder.New<DriverResponse>(false);
            if(!string.IsNullOrEmpty(driverRequest.FirstName))
            {
                predicate.Or(d => d.FirstName == driverRequest.FirstName);
            }
            if (!string.IsNullOrEmpty(driverRequest.LastName))
            {
                predicate.Or(d => d.LastName == driverRequest.LastName);
            }
            if (driverRequest.Mobile != 0)
            {
                predicate.Or(d => d.Mobile == driverRequest.Mobile.ToString());
            }
            if (!string.IsNullOrEmpty(driverRequest.Email))
            {
                predicate.Or(d => d.Email.Contains(driverRequest.Email));
            }

            var entities = from d in _dbContext.TblDrivers
                           join u in _dbContext.TblUsers on d.Username equals u.Username into us
                           from user in us.DefaultIfEmpty()
                           select new DriverResponse
                           {
                              DriverId = d.DriverId,
                              FirstName = d.Firstname,
                              LastName = d.Lastname,
                              VehicleId = d.VehicleId,
                              Mobile = d.Mobile,
                              Email = user.Email
                           };

            entities = entities.Where(predicate);
            return await GetPaginatedSortedListAsync(entities);
        }
    }
}
