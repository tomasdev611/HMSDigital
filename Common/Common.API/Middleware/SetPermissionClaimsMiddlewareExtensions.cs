using Microsoft.AspNetCore.Builder;

namespace HMSDigital.Common.API.Middleware
{

    public static class SetPermissionClaimsMiddlewareExtensions
    {
        public static IApplicationBuilder UseSetPermissionClaims(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SetPermissionClaimsMiddleware>();
        }
    }

}
