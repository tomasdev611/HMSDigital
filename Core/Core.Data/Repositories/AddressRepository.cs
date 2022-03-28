using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Sieve.Services;

namespace HMSDigital.Core.Data.Repositories
{
    public class AddressRepository : RepositoryBase<Addresses>, IAddressRepository
    {
        public AddressRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }
    }
}
