using HMS2SDK.Data.Models;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMS2SDK.BusinessLayer.Interfaces
{
    public interface IDriversService
    {
        Task<IEnumerable<TblDrivers>> GetAllDrivers(SieveModel SieveModel);
        Task<IEnumerable<DriverResponse>> SearchDrivers(DriverRequest driversRequest);
    }
}
