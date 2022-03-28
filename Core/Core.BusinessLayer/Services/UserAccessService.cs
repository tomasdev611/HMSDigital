using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Core.Data.Enums;
using HMSDigital.Core.BusinessLayer.Services.Interfaces;
using HMSDigital.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HMSDigital.Core.BusinessLayer.Services
{
    public class UserAccessService : IUserAccessService
    {
        private readonly IUserService _userService;

        private readonly HttpContext _httpContext;

        private readonly ILogger<UserAccessService> _logger;

        public UserAccessService(IUserService userService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UserAccessService> logger)
        {
            _userService = userService;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
        }

        public async Task<bool> CanAccessUser(int userId)
        {
            try
            {
                var loggedInUserRoles = await GetLoggedInUserRoles();

                var loggedInUserRolesForSites = loggedInUserRoles.Where(ur => ur.ResourceType == ResourceTypes.Site.ToString());

                if (!loggedInUserRolesForSites.Any())
                {
                    return false;
                }
                var loggedInUserRolesForAllSites = loggedInUserRolesForSites.Where(ur => ur.ResourceId == "*");

                //if logged in user is super admin for all sites
                if (loggedInUserRolesForAllSites.Any(ur => ur.RoleLevel == 1))
                {
                    return true;
                }

                var userSites = await _userService.GetUserSites(userId);
                var userSiteIds = userSites.Select(s => s.Id.ToString()).ToList();

                var commonSiteLoggedInUserRoles = loggedInUserRolesForSites.Where(ur => userSiteIds.Contains(ur.ResourceId));

                // if logged in user don't have any access to user sites
                if (!loggedInUserRolesForAllSites.Any() && !commonSiteLoggedInUserRoles.Any())
                {
                    return false;
                }

                var userSiteRoles = await GetTopUserSiteRoles(userId);

                if (userSiteRoles.Count() == 0)
                {
                    return true;
                }

                var commonUserSiteRoles = userSiteRoles.Where(ur => userSiteIds.Contains(ur.ResourceId));

                //if user do not have any role in sites assigned to it
                if (!commonUserSiteRoles.Any())
                {
                    if (!loggedInUserRolesForAllSites.Any())
                    {
                        return true;
                    }

                    var userTopSiteRoleLevel = userSiteRoles.Select(ur => ur.RoleLevel).Min();
                    //if logged in user have any higher role than top user role but no sites assigned with access to all sites
                    return loggedInUserRolesForAllSites.Any(ur => ur.RoleLevel < userTopSiteRoleLevel);
                }

                var userTopCommonSiteRoleLevel = commonUserSiteRoles.Select(ur => ur.RoleLevel).Min();

                //if logged in user have any higher role than top user role with access to all sites
                if (loggedInUserRolesForAllSites.Any(ur => ur.RoleLevel < userTopCommonSiteRoleLevel))
                {
                    return true;
                }

                //if logged in user have any common site with user and have role higher than user role for that site.
                if (commonSiteLoggedInUserRoles.Any(loggedInUR => commonUserSiteRoles.Any(ur => loggedInUR.ResourceId == ur.ResourceId && loggedInUR.RoleLevel < ur.RoleLevel)))
                {
                    return true;
                }

                return false;
            }
            catch (ValidationException ex)
            {
                _logger.LogDebug($"Error Occurred while Validating User Access:{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Validating User Access:{ex.Message}");
                throw;
            }
        }

        public async Task<bool> CanAccessHospice(int hospiceId)
        {
            return await CanAccessResource(ResourceTypes.Hospice, hospiceId);
        }

        public async Task<bool> CanAccessHospiceLocation(int hospiceLocationId)
        {
            return await CanAccessResource(ResourceTypes.HospiceLocation, hospiceLocationId);
        }

        public async Task<bool> CanAccessSite(int siteId)
        {
            return await CanAccessResource(ResourceTypes.Site, siteId);
        }

        private async Task<bool> CanAccessResource(ResourceTypes resourceType,int resourceId)
        {
            try
            {
                var loggedInUserRoles = await GetLoggedInUserRoles();

                var loggedInUserRolesForResource = loggedInUserRoles.Where(ur => ur.ResourceType == resourceType.ToString());

                var canAccessAllResources = loggedInUserRolesForResource.Any(ur => ur.ResourceId == "*");
                if (canAccessAllResources)
                {
                    return true;
                }
                var canAccessResource = loggedInUserRolesForResource.Any(ur => ur.ResourceId == resourceId.ToString());
                if (canAccessResource)
                {
                    return true;
                }

                return false;
            }
            catch (ValidationException ex)
            {
                _logger.LogDebug($"Error Occurred while Validating User Access:{ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error Occurred while Validating User Access:{ex.Message}");
                throw;
            }
        }

        private async Task<IEnumerable<UserRole>> GetTopUserSiteRoles(int userId)
        {
            var userRoles = await _userService.GetUserRoles(userId);
            var userSiteRoles = userRoles.Where(ur => ur.ResourceType == ResourceTypes.Site.ToString());
            var topUserSiteRoles = new List<UserRole>();

            // filter out user roles with top role level for each site
            foreach (var userSiteRole in userSiteRoles)
            {
                if (!topUserSiteRoles.Any(ur => ur.ResourceId == userSiteRole.ResourceId))
                {
                    topUserSiteRoles.Add(userSiteRole);
                }
                else
                {
                    var existingUserRoleForSite = topUserSiteRoles.FirstOrDefault(ur => ur.ResourceId == userSiteRole.ResourceId);
                    if (existingUserRoleForSite.RoleLevel > userSiteRole.RoleLevel)
                    {
                        topUserSiteRoles.Remove(existingUserRoleForSite);
                        topUserSiteRoles.Add(userSiteRole);
                    }
                }
            }
            return topUserSiteRoles;
        }

        private async Task<IEnumerable<UserRole>> GetLoggedInUserRoles()
        {
            var userIdClaim = _httpContext.User.FindFirst(Claims.USER_ID_CLAIM);
            if (userIdClaim == null)
            {
                return null;
            }
            return await _userService.GetUserRoles(Convert.ToInt32(userIdClaim.Value));
        }

        public bool ValidateLoggedInUser(int userId)
        {
            var userIdClaim = _httpContext.User.FindFirst(Claims.USER_ID_CLAIM);
            if(userIdClaim == null)
            {
                return false;
            }
            return Convert.ToInt32(userIdClaim.Value) == userId;
        }
    }
}
