using System;
using System.Collections.Generic;

namespace HMSOnlineSDK.Data.Models
{
    public partial class Items
    {
        public int Id { get; set; }
        public int? ProductVariantId { get; set; }
        public int? SiteId { get; set; }
        public string Status { get; set; }
        public string AssetTag { get; set; }
        public string SerialNumber { get; set; }
        public string LotNumber { get; set; }
        public DateTime? AcquiredDate { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public bool? IsClean { get; set; }
        public DateTime? CleanedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? InTransit { get; set; }
        public int? DestinationSiteId { get; set; }

        public virtual Sites DestinationSite { get; set; }
        public virtual ProductVariants ProductVariant { get; set; }
        public virtual Sites Site { get; set; }
    }
}
