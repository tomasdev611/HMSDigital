using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblInventoryTransfersItems
    {
        public int InvtransitemId { get; set; }
        public int? InvtransId { get; set; }
        public string InvCode { get; set; }
        public int? Quantity { get; set; }
        public string AssetTag { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? UserId { get; set; }
        public int? TransferConfirmed { get; set; }
        public int? QuantityReceived { get; set; }
    }
}
