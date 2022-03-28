using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class UserSites
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SiteId { get; set; }

        public virtual Sites Site { get; set; }
        public virtual UserInformations User { get; set; }
    }
}
