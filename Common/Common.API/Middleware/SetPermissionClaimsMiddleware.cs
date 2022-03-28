using AutoMapper;
using HMSDigital.Common.API.Auth;
using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.ViewModels;
using HMSDigital.Core.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HMSDigital.Common.API.Middleware
{
    public class SetPermissionClaimsMiddleware
    {
        private readonly RequestDelegate _next;

        public SetPermissionClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext,
            IUsersRepository usersRepository,
            IRolesRepository rolesRepository)
        {
            var userContext = httpContext.User;
            var apiIdentity = new ClaimsIdentity();
            var cognitoUserIdClaim = userContext.FindFirst(ClaimTypes.NameIdentifier);
            if (cognitoUserIdClaim != null)
            {
                var cognitoUserId = cognitoUserIdClaim.Value;
                var user = await usersRepository.GetByCognitoUserId(cognitoUserId);
                if (user != null && !(user.IsDisabled ?? false))
                {
                    var roleIds = user.UserRoles.Select(ur => ur.RoleId).Distinct().ToList();
                    var roleModels = await rolesRepository.GetManyAsync(r => roleIds.Contains(r.Id));
                    foreach (var role in roleModels)
                    {
                        foreach (var rolePermission in role.RolePermissions)
                        {
                            var permissionClaim = PermissionNounConstants.PERMISSION_PREFIX + rolePermission.PermissionNoun.Name;
                            apiIdentity.AddClaim(new Claim(permissionClaim, rolePermission.PermissionVerb.Name, ClaimValueTypes.String));
                        }
                    }
                    apiIdentity.AddClaim(new Claim(Claims.USER_ID_CLAIM, user.Id.ToString(), ClaimValueTypes.Integer32));
                }
            }

            if (cognitoUserIdClaim != null)
            {
                userContext.AddIdentity(apiIdentity);
            }
            
            // Call the next delegate/middleware in the pipeline
            await _next(httpContext);
        }
    }
}
