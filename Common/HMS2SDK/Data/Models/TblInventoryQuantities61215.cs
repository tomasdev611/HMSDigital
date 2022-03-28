using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblInventoryQuantities61215
    {
        public int QuantityId { get; set; }
        public int? InventoryId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? LastUpdate { get; set; }
        public int? InvStatus { get; set; }
        public int? Quantity { get; set; }
        public int? LocationInvId { get; set; }
        public int? InvtransId { get; set; }
        public int? PoitemId { get; set; }
        public int? InvaddId { get; set; }
        public int? LinkId { get; set; }
        public int? Delivery { get; set; }
        public int? Pickup { get; set; }
        public int? InvAdjustment { get; set; }
        public string AdjustmentNotes { get; set; }
        public DateTime? PiDate { get; set; }
        public string AssetTag { get; set; }
        public string AssetTagPickup { get; set; }
        public int? Imported { get; set; }
        public int? InvwoId { get; set; }
        public int? PiBalanceClear { get; set; }
        public int? PhysicalinvId { get; set; }
    }
}
