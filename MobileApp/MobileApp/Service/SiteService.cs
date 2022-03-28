using System.Collections.Generic;
using System.Threading.Tasks;
using MobileApp.Interface;
using MobileApp.Models;
using Refit;

namespace MobileApp.Service
{
    public class SiteService
    {
        private readonly ISiteApi _siteApi;

        public SiteService()
        {
            _siteApi = RestService.For<ISiteApi>(HMSHttpClientFactory.GetCoreHttpClient());
        }

        public async Task<IEnumerable<Site>> GetAllSitesAsync()
        {
            try
            {
                var response = await _siteApi.GetAllSiteAsync();
                if (!response.IsSuccessStatusCode && response.Content == null)
                {
                    return null;
                }
                return response.Content.Records;
            }
            catch
            {
                throw;
            }
        }

        public async Task<SiteMember> GetSiteMemberDetailsAsync()
        {
            try
            {
                var response = await _siteApi.GetSiteMembersAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return response.Content;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Site> GetSiteDetailAsync(int currentSiteId)
        {
            try
            {
                var response = await _siteApi.GetSiteDetailByIdAsync(currentSiteId);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                return response.Content;
            }
            catch
            {
                throw;
            }
        }
    }
}
