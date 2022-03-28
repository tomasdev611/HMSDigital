using HMS2SDK.BusinessLayer.Interfaces;
using HMS2SDK.Data.Models;
using HMS2SDK.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Sieve.Models;

namespace HMS2SDK.BusinessLayer
{
    public class DriversService : IDriversService
    {
        private readonly IDriversRepository _driversRepository;
        private readonly IUsersRepository _userRepository;

        public DriversService(IDriversRepository driversRepository, IUsersRepository userRepository)
        {
            _driversRepository = driversRepository;
            _userRepository = userRepository;
        }

        public Task<IEnumerable<TblDrivers>> GetAllDrivers(SieveModel SieveModel)
        {
            _driversRepository.SieveModel = SieveModel;
            return _driversRepository.GetAllAsync();
        }

        public async Task<IEnumerable<DriverResponse>> SearchDrivers(DriverRequest driverRequest)
        {
            return await _driversRepository.SearchDriver(driverRequest);
        }
    }
}
