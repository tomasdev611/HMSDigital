using System.Threading.Tasks;
using MobileApp.Models;
using Refit;

namespace MobileApp.Interface
{
    public interface ISiteApi
    {
        [Get("/api/sites")]
        Task<ApiResponse<PaginatedList<Site>>> GetAllSiteAsync();

        [Get("/api/sites/{siteId}")]
        Task<ApiResponse<Site>> GetSiteDetailByIdAsync(int siteId);

        [Get("/api/sites/members/me")]
        Task<ApiResponse<SiteMember>> GetSiteMembersAsync();
    }
}
