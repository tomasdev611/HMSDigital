using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblInventoryTransfersIntra6102105
    {
        public int InvtransId { get; set; }
        public int? LocationIdFrom { get; set; }
        public int? LocationIdTo { get; set; }
        public int? VehicleId { get; set; }
        public int? InvStatus { get; set; }
        public int? Quantity { get; set; }
        public string AssetTag { get; set; }
        public string SerialNumber { get; set; }
        public string Notes { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? UserId { get; set; }
        public int? Status { get; set; }
        public int? Processed { get; set; }
    }
}
