using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class UserInformations
    {
        public UserInformations()
        {
            UserSites = new HashSet<UserSites>();
        }

        public int Id { get; set; }
        public string UserUuid { get; set; }
        public string Username { get; set; }
        public int? DriverId { get; set; }
        public int? DefaultSiteId { get; set; }

        public virtual Sites DefaultSite { get; set; }
        public virtual ICollection<UserSites> UserSites { get; set; }
    }
}
