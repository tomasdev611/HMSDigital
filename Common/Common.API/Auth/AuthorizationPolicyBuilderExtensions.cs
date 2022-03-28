using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HMSDigital.Common.API.Auth
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        private static readonly IEnumerable<string> _scopeClaimTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
              "http://schemas.microsoft.com/identity/claims/scope", "scope" };
        public static AuthorizationPolicyBuilder RequireScope(this AuthorizationPolicyBuilder builder, params string[] scopes)
        {
            return builder.RequireAssertion(context =>
                context.User.HasScope(scopes)
            );
        }

        public static bool HasScope(this ClaimsPrincipal user, params string[] scopes)
        {
            return user.Claims
                       .Where(c => _scopeClaimTypes.Contains(c.Type))
                       .SelectMany(c => c.Value.Split(' '))
                       .Any(s => scopes.Contains(s, StringComparer.OrdinalIgnoreCase));
        }
    }
}
