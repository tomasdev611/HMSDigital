using HMSOnlineSDK.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HMSOnlineSDK.BusinessLayer.Interfaces
{
    public interface IUserInformationService
    {
        Task<UserInformations> CreateUser(UserInformations userInformationRequest);

        Task<UserInformations> UpdateSites(string username, int defaultSiteId, IEnumerable<int> siteIds, string userId);

        Task<UserInformations> UpdateDriver(string userName, int driverId);

        Task<(int?, IEnumerable<Sites>)> GetUserSites(string userName);
    }
}
