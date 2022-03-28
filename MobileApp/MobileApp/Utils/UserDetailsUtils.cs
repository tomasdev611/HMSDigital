using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileApp.Assets.Constants;
using MobileApp.Models;
using MobileApp.Service;

namespace MobileApp.Utils
{
    public class UserDetailsUtils
    {
        public static async Task<int?> GetUsersCurrentSiteId()
        {
            var userRoles = await GetLoggedInUserRoles();

            if (userRoles.Contains(AppConstants.DRIVER))
            {
                try
                {
                    var details = await CacheManager.GetDriverDetailsFromCache();
                    return details.CurrentSiteId;
                }
                catch
                {
                    return null;
                }
            }
            if (userRoles.Contains(AppConstants.SITE_ADMIN))
            {
                try
                {
                    var details = await CacheManager.GetSiteMemberDetailsFromCache();
                    return details.SiteId;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public static async Task<int?> GetUsersCurrentVehicleId()
        {
            var userRoles = await GetLoggedInUserRoles();

            if (userRoles.Contains(AppConstants.DRIVER))
            {
                try
                {
                    var details = await CacheManager.GetDriverDetailsFromCache();
                    return details.CurrentVehicleId;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public static async Task<Vehicle> GetCurrentVehicleDetails()
        {
            try
            {
                var userRoles = await GetLoggedInUserRoles();

                if (userRoles.Contains(AppConstants.DRIVER))
                {
                    var details = await CacheManager.GetDriverDetailsFromCache();
                    return details.CurrentVehicle;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }


        private static async Task<IEnumerable<string>> GetLoggedInUserRoles()
        {
            try
            {
                var userDetails = await CacheManager.GetUserDetails();
                return userDetails.UserRoles.Select(us => us.RoleName);
            }
            catch
            {
                throw;
            }
        }
    }
}
