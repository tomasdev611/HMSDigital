using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Sieve.Services;

namespace HMSDigital.Core.Data.Repositories
{
    public class UserVerifyRepository : RepositoryBase<UserVerify>, IUserVerifyRepository
    {
        public UserVerifyRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }

    }
}
