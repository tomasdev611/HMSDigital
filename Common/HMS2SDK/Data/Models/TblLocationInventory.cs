using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblLocationInventory
    {
        public int LocationInvId { get; set; }
        public int? LocationId { get; set; }
        public int? InventoryId { get; set; }
        public int? Quantity { get; set; }
        public string AssetTags { get; set; }
        public DateTime? Timestamp { get; set; }
        public string SerialNumbers { get; set; }
        public int? Processed { get; set; }
        public int? TagsProcessed { get; set; }
        public int? ScanType { get; set; }
    }
}
