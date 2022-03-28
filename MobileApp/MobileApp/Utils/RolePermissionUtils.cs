using System;
using System.Linq;
using System.Threading.Tasks;
using MobileApp.Service;

namespace MobileApp.Utils
{
    public class RolePermissionUtils
    {
        public async static Task<bool> CheckPermissionExists()
        {
            try
            {
                var permissions = await CacheManager.GetPermissionList();
                return permissions.Any();
            }
            catch
            {
                return false;
            }
        }

        public async static Task<bool> CheckPermission(string permissionName, string access)
        {
            try
            {
                var permissions = await CacheManager.GetPermissionList();
                return permissions.Contains($"{permissionName}:{access}");
            }
            catch
            {
                return false;
            }
        }
    }
}
