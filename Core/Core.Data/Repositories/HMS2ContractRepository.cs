using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Sieve.Services;

namespace HMSDigital.Core.Data.Repositories
{
    public class HMS2ContractRepository : RepositoryBase<Hms2Contracts>, IHMS2ContractRepository
    {
        public HMS2ContractRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

    }
}
