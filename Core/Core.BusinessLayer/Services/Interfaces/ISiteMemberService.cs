using HMSDigital.Common.ViewModels;
using HMSDigital.Core.ViewModels;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services.Interfaces
{
    public interface ISiteMemberService
    {
        Task<PaginatedList<SiteMember>> GetAllSiteMembers(int siteId, SieveModel sieveModel);

        Task<SiteMember> GetMySiteMember();

        Task<SiteMember> GetSiteMemberById(int siteId, int memberId);

        Task<SiteMember> CreateSiteMember(int siteId, SiteMemberRequest siteMemberRequest);

        Task<SiteMember> UpdateSiteMember(int siteId, int memberId, SiteMemberRequest siteMemberUpdateRequest);

        Task<IEnumerable<Site>> GetMySites();
    }
}
