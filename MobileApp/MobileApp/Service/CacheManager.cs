using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MobileApp.Assets.Constants;
using MobileApp.Models;

namespace MobileApp.Service
{
    public static class CacheManager
    {
        private static IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions() { });

        private static UserService _userService = new UserService();

        private static SiteService _siteService = new SiteService();

        private static DriverService _driverService = new DriverService();

        private static ItemsService _itemsService = new ItemsService();

        private static PermissionService _permissionService = new PermissionService();

        private const double DefaultExpirationTimeInMinutes = 60;

        public static async Task<Driver> GetDriverDetailsFromCache()
        {
            try
            {
                return await _memoryCache.GetOrCreateAsync(AppConstants.DRIVER_DETAILS, async cachedData =>
                {
                    cachedData.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(DefaultExpirationTimeInMinutes);
                    cachedData.Value = await _driverService.GetDriverUserDetailsAsync();
                    return cachedData.Value;
                }) as Driver;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<SiteMember> GetSiteMemberDetailsFromCache()
        {
            try
            {
                return await _memoryCache.GetOrCreateAsync(AppConstants.SITE_DETAILS, async cachedData =>
                {
                    cachedData.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(DefaultExpirationTimeInMinutes);
                    cachedData.Value = await _siteService.GetSiteMemberDetailsAsync();
                    return cachedData.Value;
                }) as SiteMember;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<Site> GetSiteDetailByIdFromCache(int siteId)
        {
            try
            {
                return await _memoryCache.GetOrCreateAsync(KeyConstants.SITE_DETAILS_BY_ID + siteId, async cachedData =>
                  {
                      cachedData.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(DefaultExpirationTimeInMinutes);
                      cachedData.Value = await _siteService.GetSiteDetailAsync(siteId);
                      return cachedData.Value;
                  }) as Site;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static async Task<IEnumerable<Item>> GetItemsList()
        {
            try
            {
                return await _memoryCache.GetOrCreateAsync(AppConstants.ITEMS_LIST, async cachedData =>
                {
                    cachedData.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(DefaultExpirationTimeInMinutes);
                    cachedData.Value = await _itemsService.GetItemsList();
                    return cachedData.Value;
                }) as IEnumerable<Item>;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<IEnumerable<string>> GetPermissionList()
        {
            try
            {
                return await _memoryCache.GetOrCreateAsync(AppConstants.ROLE_PERMISSION, async cachedData =>
                {
                    cachedData.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(DefaultExpirationTimeInMinutes);
                    cachedData.Value = await _permissionService.GetUserPermissions();
                    return cachedData.Value;
                }) as IEnumerable<string>;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<User> GetUserDetails()
        {
            try
            {
                return await _memoryCache.GetOrCreateAsync(AppConstants.USER_DETAILS, async cachedData =>
                {
                    cachedData.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(DefaultExpirationTimeInMinutes);
                    cachedData.Value = await _userService.GetUserDetailsAsync();
                    return cachedData.Value;
                }) as User;
            }
            catch
            {
                throw;
            }
        }

        public static void ClearCache()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions() { });
        }
    }
}

