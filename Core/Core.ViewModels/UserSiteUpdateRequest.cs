using System;
using System.Collections.Generic;
using System.Text;

namespace HMSDigital.Core.ViewModels
{
    public class UserSiteUpdateRequest
    {
        public int DefaultSiteId { get; set; }

        public IEnumerable<int> SiteIds { get; set; }
    }
}
