using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Sieve.Services;

namespace HMSDigital.Core.Data.Repositories
{
    public class CsvMappingRepository : RepositoryBase<CsvMappings>, ICsvMappingRepository
    {
        public CsvMappingRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

    }
}
