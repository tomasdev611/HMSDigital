using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblDepreciationMaster
    {
        public int DepmasterId { get; set; }
        public int? InventoryId { get; set; }
        public DateTime? AssetEntry { get; set; }
        public int? AssetLife { get; set; }
        public string AssetTag { get; set; }
        public decimal? AssetCost { get; set; }
        public int? InvaddId { get; set; }
        public int? PoId { get; set; }
        public int? LocationInvId { get; set; }
    }
}
