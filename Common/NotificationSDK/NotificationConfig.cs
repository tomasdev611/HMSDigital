using HMSDigital.Common.SDK.Config;
using System.Collections.Generic;

namespace NotificationSDK
{
    public class NotificationConfig
    {
        public IEnumerable<string> AuditEmail { get; set; }

        public string ApiUrl { get; set; }

        public IdentityClientConfig IdentityClient { get; set; }
    }
}
