using Hms2BillingSDK.Models;
using Hms2BillingSDK.Repositories.Interfaces;
using Sieve.Services;

namespace Hms2BillingSDK.Repositories
{
    public class Hms2BillingContractsRepository : RepositoryBase<TblContracts>, IHms2BillingContractsRepository
    {
        public Hms2BillingContractsRepository(Hms2BillingContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }
    }
}
