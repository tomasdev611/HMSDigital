using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileApp.Interface;
using MobileApp.Models;
using Refit;

namespace MobileApp.Service
{
    public class PermissionService
    {
        private readonly IRoleApi _roleApi;

        private readonly UserService _userService;

        public PermissionService()
        {
            _roleApi = RestService.For<IRoleApi>(HMSHttpClientFactory.GetCoreHttpClient());
            _userService = new UserService();
        }

        public async Task<IEnumerable<string>> GetUserPermissions()
        {
            try
            {
                var userRole = await GetUserRole();
                var userDetials = await CacheManager.GetUserDetails();

                if (userRole == null)
                {
                    return null;
                }

                var permission = userRole.Where(u => userDetials.UserRoles
                                         .Select(i => i.RoleId)
                                         .Contains(u.Id))
                                         .SelectMany(i => i.Permissions);

                return permission;
            }
            catch
            {
                throw;
            }
        }

        private async Task<IEnumerable<Role>> GetUserRole()
        {
            try
            {
                var userRole = await _roleApi.GetRolePermissions();
                if (!userRole.IsSuccessStatusCode)
                {
                    return null;
                }
                return userRole.Content;
            }
            catch
            {
                throw;
            }
        }
    }
}
