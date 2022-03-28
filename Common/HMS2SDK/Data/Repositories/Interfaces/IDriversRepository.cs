using HMS2SDK.Data.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HMS2SDK.Data.Repositories.Interfaces
{
    public interface IDriversRepository : IRepository<TblDrivers>
    {
        Task<IEnumerable<DriverResponse>> SearchDriver(DriverRequest driverRequest);
    }
}
