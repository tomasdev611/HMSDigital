using HMSDigital.Common.BusinessLayer.Constants;
using HMSDigital.Common.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Common.API
{
    public static class AuditExtesions
    {
        public static void ConfigureAuditLogs(this IServiceCollection services)
        {
            var svcProvider = services.BuildServiceProvider();
            
            Audit.Core.Configuration
               .AddOnCreatedAction(scope =>
               {
                   var httpContext = svcProvider.GetService<IHttpContextAccessor>().HttpContext;
                   if (httpContext != null)
                   {
                       var userContext = httpContext.User;
                       if (userContext != null)
                       {
                           var userClaim = userContext.FindFirst(Claims.USER_ID_CLAIM);
                           if (userClaim == null)
                           {
                               userClaim = userContext.FindFirst(Claims.AUDIT_USER_ID_CLAIM);
                           }
                           if (userClaim != null)
                           {
                               scope.Event.CustomFields.Add(Constants.AUDIT_USER_ID, userClaim.Value);
                           }
                           scope.Event.CustomFields.Add(Constants.AUDIT_CLIENT_IP_ADDRESS, httpContext.Connection.RemoteIpAddress?.ToString());
                       }
                   }
               });
        }
    }
} 