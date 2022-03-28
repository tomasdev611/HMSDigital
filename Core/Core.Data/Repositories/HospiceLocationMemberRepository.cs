using HMSDigital.Core.Data.Models;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Sieve.Services;

namespace HMSDigital.Core.Data.Repositories
{
    public class HospiceLocationMemberRepository : RepositoryBase<HospiceLocationMembers>, IHospiceLocationMemberRepository
    {
        public HospiceLocationMemberRepository(HMSDigitalAuditContext dbContext, ISieveProcessor sieveProcessor) : base(dbContext, sieveProcessor)
        {

        }
    }
}
