using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class Sites
    {
        public Sites()
        {
            ItemsDestinationSite = new HashSet<Items>();
            ItemsSite = new HashSet<Items>();
            QuantityTrackedItemsCopy1 = new HashSet<QuantityTrackedItemsCopy1>();
            UserInformations = new HashSet<UserInformations>();
            UserSites = new HashSet<UserSites>();
        }

        public int Id { get; set; }
        public int? SiteCode { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Items> ItemsDestinationSite { get; set; }
        public virtual ICollection<Items> ItemsSite { get; set; }
        public virtual ICollection<QuantityTrackedItemsCopy1> QuantityTrackedItemsCopy1 { get; set; }
        public virtual ICollection<UserInformations> UserInformations { get; set; }
        public virtual ICollection<UserSites> UserSites { get; set; }
    }
}
