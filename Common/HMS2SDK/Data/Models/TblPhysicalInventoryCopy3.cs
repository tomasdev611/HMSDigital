using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPhysicalInventoryCopy3
    {
        public int PhysicalinvId { get; set; }
        public int? LocationId { get; set; }
        public int? InventoryId { get; set; }
        public DateTime? Submitted { get; set; }
        public int? InvCount { get; set; }
        public string AssetTags { get; set; }
        public string SerialNumbers { get; set; }
        public int? UserId { get; set; }
        public int? Processed { get; set; }
    }
}
