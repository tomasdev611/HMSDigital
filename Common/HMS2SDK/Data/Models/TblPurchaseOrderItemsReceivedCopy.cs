using System;

namespace HMS2SDK.Data.Models
{
    public partial class TblPurchaseOrderItemsReceivedCopy
    {
        public int PoitemrecId { get; set; }
        public int? PoId { get; set; }
        public int? PoitemId { get; set; }
        public int? QuantityReceived { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? UserId { get; set; }
    }
}
